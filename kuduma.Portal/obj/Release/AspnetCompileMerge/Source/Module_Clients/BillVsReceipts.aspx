<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Clients/Clients.master" CodeBehind="BillVsReceipts.aspx.cs" Inherits="Kuduma.Portal.BillVsReceipts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">


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

            document.getElementById('hidGridView').value = htmlEscape(forExport.innerHTML);
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
    </script>

        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="Receipts.aspx" style="z-index: 9;"><span></span>Receipts</a></li>
                        <li class="active"><a href="BillVsReceipts.aspx" style="z-index: 8;" class="active_bread">Bills Vs Receipts</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Bills Vs Receipts
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div style="width: 100%">
                                        <div align="right">
                                            <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False" OnClientClick="AssignExportHTML()">Export to Excel</asp:LinkButton>
                                        </div>

                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>Client ID
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="ddlClientId" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged">
                                                    </asp:DropDownList>

                                                </td>
                                                <td>Client Name
                                                </td>
                                                <td>
                                                    <%--<asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="true" class="form-controldrop" Width="300px"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                    </asp:DropDownList>--%>

                                                    <asp:DropDownList ID="ddlcname" runat="server" class="ddlautocomplete chosen-select" AutoPostBack="True" TabIndex="2"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>GSTIN/UIN</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOurGSTIN" runat="server" class="form-controldrop">
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>Bill Type
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlbilltype" runat="server" class="form-controldrop">
                                                        <asp:ListItem>All</asp:ListItem>
                                                        <asp:ListItem>Software</asp:ListItem>
                                                        <asp:ListItem>Manual</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>Period  
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPeriod" runat="server"  OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem>From-To</asp:ListItem>
                                                        <asp:ListItem>Month</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfromdate" runat="server" Text="From Date :"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_From_Date" runat="server" class="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CE_From_Date" runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                                        Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBE_From_Date" runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>

                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltodate" runat="server" Text="To Date :"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_To_Date" runat="server" class="form-control"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="MMM-yyyy" TargetControlID="txtEndDate" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                    </cc1:CalendarExtender>

                                                </td>
                                                <td></td>
                                                <td>
                                                    <asp:Button runat="server" ID="Btn_Search_Invoice_Btn_Dates" Text="Submit" class="btn save" Style="margin-left: 80px"
                                                        OnClick="Btn_Search_Invoice_Btn_Dates_Click" />
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                    <asp:HiddenField ID="hidGridView" runat="server" />
                                    <div id="forExport" style="overflow: scroll; width: 960px;">
                                        <asp:GridView ID="GVInvoiceBills" runat="server" AutoGenerateColumns="False"
                                            CellPadding="4" ForeColor="#333333" CssClass="table table-striped table-bordered table-condensed table-hover"
                                            OnRowDataBound="GVInvoiceBills_RowDataBound" ShowFooter="true" Style="overflow: scroll; width: 950px">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblsno" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Our GSTIN">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblOURGSTIN" Text='<%# Bind("GSTNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Client Id">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblClientID" Text='<%# Bind("unitid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client Name" ItemStyle-Width="125px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblClientName" Text='<%# Bind("clientname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Client GSTIN" ItemStyle-Width="125px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblClientGSTIN" Text='<%# Bind("GSTIN") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Bill Date" ItemStyle-Width="125px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblbilldt" Text='<%# Bind("BillDt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="125px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInvoiceNo" Text='<%# Bind("Billno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Period From-To" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblfromtext" runat="server" Text="From :"></asp:Label>
                                                        <asp:Label ID="lblfrom" runat="server" Text='<%#Eval("FromDt", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                        <asp:Label ID="lbltotext" runat="server" Text="To:"></asp:Label>
                                                        <asp:Label ID="lblto" runat="server" Text='<%#Eval("ToDt", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="60px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Total" DataField="total" DataFormatString="{0:0.00}" />
                                                <asp:BoundField HeaderText="Others" DataField="Others" DataFormatString="{0:0.00}" />
                                                <asp:BoundField HeaderText="Service Charges" DataField="ServiceChrg" DataFormatString="{0:0.00}" />

                                                <asp:TemplateField HeaderText="Service Tax" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceTax" runat="server" Text='<%#Bind("ServiceTax","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalServiceTax"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="SB Cess" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSBCess" runat="server" Text='<%#Bind("SBCessAmt","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalSBCessAmt"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="KK Cess" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKKCess" runat="server" Text='<%#Bind("KKCessAmt","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalKKCessAmt"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="CGST" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCGST" runat="server" Text='<%#Bind("CGSTAmt","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalCGSTAmt"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="SGST" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSGST" runat="server" Text='<%#Bind("SGSTAmt","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalSGSTAmt"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>



                                                <asp:TemplateField HeaderText="IGST" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIGST" runat="server" Text='<%#Bind("IGSTAmt","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalIGSTAmt"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Grand Total" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGrandTotal" runat="server" Text='<%#Bind("GrandTotal","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalGrandTotal"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="TDS" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTDSAmt" runat="server" Text='<%#Bind("TDSAmt","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalTDSAmt"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Net Receivable Amt" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNetAmt" runat="server" Text='<%#Bind("NetAmt","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalNetAmt"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Recieved Amt" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRecievedAmt" runat="server" Text='<%#Bind("Receivedamt","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalReceivedamt"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Disallowance" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDisallowance" runat="server" Text='<%#Bind("Disallowance","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalDisallowance"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Outstanding Amt" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOutstandingAmt" runat="server" Text='<%#Bind("OutstandingAmt","{0:0.00}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotalOutstandingAmt"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>




                                                <asp:BoundField HeaderText="Cheque No" DataField="DDorCheckno" />
                                                <asp:TemplateField HeaderText="Cheque Date" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCheque" runat="server" Text='<%#Eval("DDorCheckDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="60px"></ItemStyle>
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="Bank Name" DataField="Bankname" />






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
            
            <!-- CONTENT AREA END -->
        </div>
  </asp:Content>