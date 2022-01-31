<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Clients/Clients.master" CodeBehind="ReceiveReports.aspx.cs" Inherits="Kuduma.Portal.ReceiveReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">

   
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

        .bordercss td {
            border: 1px solid #A1DCF2;
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
                select: function (event, ui) { $("#<%=ddlclient.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlcname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlclient.ClientID %>").trigger('change');
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
                        <li class="active"><a href="ReceiveReports.aspx" style="z-index: 8;" class="active_bread">Receipt Reports</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Receipts
                                                

                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div class="dashboard_firsthalf" style="width: 100%">
                                        <table width="100%" cellpadding="5" cellspacing="5">


                                            <tr>
                                                <td>Client Id :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlclient" runat="server" AutoPostBack="true" class="ddlautocomplete chosen-select"
                                                        OnSelectedIndexChanged="ddlclient_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>Client Name :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="true" class="ddlautocomplete chosen-select"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>




                                                <td><%--Reciept Mode :--%>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlreceiptmode" Visible="false" runat="server" class="sdrop">
                                                        <asp:ListItem>Month</asp:ListItem>
                                                        <asp:ListItem>Daily</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>


                                                <td>
                                                    <asp:Label ID="lblfrom" runat="server" Text="From"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server"
                                                        Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtMonth">
                                                    </cc1:CalendarExtender>
                                                </td>

                                                <td>
                                                    <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtto" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtto_CalendarExtender" runat="server"
                                                        Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtto">
                                                    </cc1:CalendarExtender>
                                                </td>



                                                <td>
                                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" class="btn save" OnClick="btnSubmit_Click"
                                                        Width="65px" />
                                                </td>
                                                <td colspan="6">

                                                    <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False" OnClientClick="AssignExportHTML()" Text="Export to Excel"></asp:LinkButton>

                                                </td>

                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>

                                                <td>
                                                    <div align="right">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>


                                    </div>
                                    <asp:HiddenField ID="hidGridView" runat="server" />
                                    <div id="forExport" style="overflow-x: scroll; width: 960px">
                                        <%--Here is Begining of Newly added GridView --%>
                                        <asp:Label ID="lbllabel" runat="server" Text="Total Bills For The Selected Client"></asp:Label>

                                        <asp:GridView ID="gvnew" runat="server" AutoGenerateColumns="False" Width="100%"
                                            CssClass="table table-striped table-bordered table-condensed table-hover" CellPadding="4" GridLines="Vertical"
                                            OnRowDataBound="gvnew_RowDataBound" ShowFooter="true">
                                            <Columns>

                                                <%--  0--%>

                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                       <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- 1--%>

                                                <asp:TemplateField HeaderText="Receipt No.">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRecieptNo" Text='<%# Bind("ReceiptNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  2--%>
                                                <asp:BoundField DataField="month" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                <%--  3--%>
                                                <asp:TemplateField HeaderText="Invoice No">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInvoice" Text='<%# Bind("BillNo") %>' Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  4--%>
                                                <asp:TemplateField HeaderText="Month">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblInvoiceMonth" Text='<%# Bind("InvoiceMonth") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  5--%>

                                                <asp:TemplateField HeaderText="Client ID">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblClientid" Text='<%# Bind("Clientid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  6--%>
                                                <asp:TemplateField HeaderText="Client Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblclientName" Text='<%# Bind("clientName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  7--%>
                                                <asp:TemplateField HeaderText="Net Invoice Amt">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblNetInvoice" Text='<%# Bind("NetInvoiceAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalNetInvoice" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 8--%>
                                                <asp:TemplateField HeaderText="Service Tax">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblsertax" Text='<%#Bind("ServiceTax")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalsertax" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--  9--%>

                                                <asp:TemplateField HeaderText="SB Cess">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCess" runat="server" Text='<%#Bind("SBCessAmt")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalSBCess" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>

                                                </asp:TemplateField>
                                                <%--  10--%>
                                                <asp:TemplateField HeaderText="KK Cess">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblkkCess" runat="server" Text='<%#Bind("KKCessAmt")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalkkCess" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>

                                                </asp:TemplateField>

                                                <%--  11--%>
                                                <asp:TemplateField HeaderText="CGST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCGST" runat="server" Text='<%#Bind("CGSTAmt")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalCGST" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>

                                                </asp:TemplateField>

                                                <%--  12--%>
                                                <asp:TemplateField HeaderText="SGST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSGST" runat="server" Text='<%#Bind("SGSTAmt")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalSGST" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>

                                                </asp:TemplateField>

                                                <%--  13--%>
                                                <asp:TemplateField HeaderText="IGST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIGST" runat="server" Text='<%#Bind("IGSTAmt")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalIGST" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>

                                                </asp:TemplateField>

                                                <%-- <asp:TemplateField HeaderText="Cess">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCess" runat="server" Text='<%#Bind("CESS")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SHE Cess">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblShecess" Text='<%#Bind("SHECess")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <%--14--%>
                                                <asp:TemplateField HeaderText="Grand Total">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblGrand" Text='<%#Bind("GrandTotal")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalGrandTotal" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 15--%>
                                                <asp:TemplateField HeaderText="TDS Amt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTDSAmt" runat="server" Text='<%#Bind("TDSAmt")%>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalTDSAmt" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--16--%>
                                                <asp:TemplateField HeaderText="Payment Received">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpay" runat="server" Text='<%#Bind("PaymentReceived")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalPaymentReceived" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--17--%>

                                                <asp:TemplateField HeaderText="Cheque No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblChkno" runat="server" Text='<%#Bind("ChequeNum")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--18--%>
                                                <asp:TemplateField HeaderText="Disallowance" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDisallowance" runat="server" Text='<%#Bind("Disallowance")%>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisallowance" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--19--%>
                                                <asp:TemplateField HeaderText="Reason">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDisallowanceReason" runat="server" Text='<%#Bind("DisallowanceReason")%>' />
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                                <%--20--%>
                                                <asp:TemplateField HeaderText="Bank Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcreditedtobank" runat="server" Text='<%#Bind("Bankname")%>' />
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>

                                        </asp:GridView>
                                        <asp:Label ID="LblReslt" runat="server" Text="" Style="color: red"></asp:Label>
                                        <asp:Label ID="LblReslt1" runat="server" Text="" Style="color: red"></asp:Label>
                                        <%-- <asp:Label ID="lbltotalfrombill" runat="server"  Text="Total:" style=" margin-left:350px"></asp:Label>--%>
                                    </div>
                                    <%-- Here is Ending of Newly added Gridview --%>
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
    </asp:content>