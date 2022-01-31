<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Clients/Clients.master" AutoEventWireup="true" CodeBehind="1TimeBill.aspx.cs" Inherits="Kuduma.Portal.Module_Clients._1TimeBill1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <style type="text/css">
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

    <script>
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
                select: function (event, ui) { $("#<%=ddlclientid.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlCname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlclientid.ClientID %>").trigger('change');
        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {
            $("#<%=ddlCname.ClientID %>").trigger('change');
        }
    </script>

    <script>
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
    <script>
        $(function () {
            bindautofilldesgs();
        });
        var prmInstance = Sys.WebForms.PageRequestManager.getInstance();
        prmInstance.add_endRequest(function () {
            //you need to re-bind your jquery events here
            bindautofilldesgs();
        });

        <%--  function bindautofilldesgs() {
            $(".ItemIDText").autocomplete({
                source: eval($("#<%=hdItemid.ClientID %>").val()),
                minLength: 2
            });

            $(".ItemIDTextName").autocomplete({
                source: eval($("#<%=hditemname.ClientID %>").val()),
                minLength: 2
            });

            $(".ItemIDText").addClass("form-control");
            $(".ItemIDTextName").addClass("form-control");
        }--%>
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".calculate").keyup(function () {
                Calculate();
            });

            debugger
            function Calculate() {
                var grandTotal = 0;
                $('[id*=GVInvPODetails] tr').each(function () {
                    var row = $(this);
                    var qty = $('[id*=txtQuantity]', row).val();
                    var price = $('[id*=txtbuyingprice]', row).val();
                    var total;
                    var perc;
                    if (qty != '' && !isNaN(qty) && price != '' && !isNaN(price)) {
                        total = parseFloat(qty) * parseFloat(price);
                        grandTotal += total;
                        $("[id*=txttotalbuyingprice]").html(total);
                    } else if (qty == '' || price == '') {
                        total = 0;
                        grandTotal += total;
                        $("[id*=txttotalbuyingprice]").html(total);
                    }
                })
                //SetPercentage(grandTotal);
            }

            $(".calculate").addClass("form-control");
            //function SetPercentage(grandTotal) {
            //    $('[id*=lblTotal]').each(function () {
            //        var total = $(this).html();
            //        var row = $(this).closest('tr');
            //        if (total != '' && total != 0) {
            //            var percent = (parseFloat(total) / grandTotal).toFixed(2);
            //            $('[id*=lblPercentage]', row).html(percent);
            //        }
            //    });
            //}
        });
    </script>


    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Clients.aspx" style="z-index: 9;"><span></span>Clients</a></li>

                    <li class="active"><a href="1TimeBill.aspx" style="z-index: 7;" class="active_bread">One Time Bill</a></li>
                </ul>
            </div>
            <asp:ScriptManager runat="server" ID="Scriptmanager1">
            </asp:ScriptManager>
            <div class="dashboard_full">
                <div style="float: right; font-weight: bold">
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">

                    <div class="sidebox">
                        <div class="boxhead">

                            <h2 style="text-align: center">One Time Bill
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">

                                <div align="right">

                                     <div align="right">
                                            <a href="ClientBilling.aspx" id="onetimebill" runat="server"><span>Client Billing</span></a>

                                        </div>
                                    <table width="30%" cellpadding="5" cellspacing="5" align="right">
                                        <tr style="height: 36px">
                                            <td>
                                                <asp:RadioButton ID="rdbwithGST" runat="server" Text="With GST" GroupName="GST" Checked="true" AutoPostBack="true" OnCheckedChanged="rdbwithGST_CheckedChanged" />
                                                <asp:RadioButton ID="rdbwithoutGST" runat="server" Text="With Out GST" GroupName="GST" AutoPostBack="true" OnCheckedChanged="rdbwithGST_CheckedChanged" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbSGST" runat="server" Text="SGST/CGST" GroupName="CGST" Checked="true" />
                                                <asp:RadioButton ID="rdbIGST" runat="server" Text="IGST" GroupName="CGST" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <table width="100%" cellpadding="5" cellspacing="5" style="margin-left: 10px">
                                    <tr>
                                        <td valign="top">
                                            <table width="100%" cellpadding="5" cellspacing="5">

                                                <tr style="height: 36px">

                                                    <td>
                                                        <asp:Label ID="lblmonth" runat="server" Text="Month "></asp:Label>
                                                    </td>

                                                    <td>
                                                        <asp:TextBox ID="txtmonth" runat="server" Text="" CssClass="form-control" Width="180px" AutoComplete="off" class="sinput"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                            Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown"></cc1:CalendarExtender>
                                                    </td>

                                                </tr>


                                                <tr style="height: 36px">
                                                    <td>
                                                        <asp:Label ID="lblclientid" Text="Client ID" runat="server"></asp:Label>

                                                    </td>
                                                    <td>
                                          <asp:DropDownList ID="ddlclientid" runat="server" CssClass="ddlautocomplete chosen-select" AutoPostBack="True" 
                                              OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" Width="120px">
                                                </asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <tr style="height: 36px">
                                                    <td>Shipping Date <span style="color: red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtfrommonth" runat="server" class="form-control" Width="228px" AutoComplete="Off"> </asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true" TargetControlID="txtfrommonth"
                                                            Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtfrommonth"
                                                            ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>

                                                <tr style="height: 36px">
                                                    <td>Invoice No <span style="color: red">*</span></td>
                                                    <td>
                                                        <asp:TextBox ID="txtinvoiceno" runat="server" Enabled="false" class="form-control" Width="228px"></asp:TextBox></td>
                                                </tr>

                                                <tr style="height: 36px" runat="server">
                                                    <td>GST No.</td>
                                                    <td>
                                                        <asp:TextBox ID="txtgstno" runat="server" class="form-control" Width="228px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr style="height: 36px" runat="server">
                                                    <td>Shipping Company</td>
                                                    <td>
                                                        <asp:TextBox ID="txtshipping" runat="server" class="form-control" Width="228px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>

                                                 <tr style="height: 36px" runat="server">
                                                    <td>Tracking No</td>
                                                    <td>
                                                        <asp:TextBox ID="txttrackingno" runat="server" class="form-control" Width="228px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr style="height: 36px" runat="server">
                                                    <td>Weight</td>
                                                    <td>
                                                        <asp:TextBox ID="txtweight" runat="server" class="form-control" Width="228px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>

                                                 <tr style="height: 36px" runat="server">
                                                    <td>Shipping Address</td>
                                                    <td>
                                                        <asp:TextBox ID="txtshippingaddress" runat="server" TextMode="MultiLine" class="form-control" Width="228px">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>

                                                 <tr style="height: 36px">
                                                    <td>Place Of Supply
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtplaceofsupply" runat="server" class="form-control" Width="228px"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>

                                        <td valign="top">
                                            <table width="100%" cellpadding="5" cellspacing="5">
                                                <tr style="height: 36px">
                                                </tr>

                                                <tr style="height: 36px">
                                                    <td>
                                                        <asp:Label ID="lblname" Text="Client Name" runat="server">
                                                            
                                                        </asp:Label>

                                                    </td>
                                                    <td>
                  <asp:DropDownList ID="ddlCname" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCname_OnSelectedIndexChanged"
                                                    Style="width: 355px">
                                                </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr style="height: 36px">
                                                    <td> Date & Time Of Supply 
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txttoDate" runat="server" class="form-control" Width="228px" AutoComplete="Off"> </asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="true" TargetControlID="txttoDate"
                                                            Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txttoDate"
                                                            ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>

                                                <tr style="height: 36px">
                                                    <td>Invoice Date <span style="color: red">*</span></td>
                                                    <td>
                                                        <asp:TextBox ID="txtinvoicedate" runat="server" class="form-control" Width="228px" AutoComplete="Off"></asp:TextBox></td>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true" TargetControlID="txtinvoicedate"
                                                        Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtinvoicedate"
                                                        ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                </tr>

                                                <tr style="height: 36px">
                                                    <td>Address <span style="color: red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGSTAddress" runat="server" class="form-control" TextMode="MultiLine" Width="228px" AutoComplete="Off"> </asp:TextBox>

                                                    </td>
                                                </tr>

                                                 <tr style="height: 36px">
                                                    <td>Vehicle/Vessel No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtvehicleno" runat="server" class="form-control" Width="228px" AutoComplete="Off"> </asp:TextBox>

                                                    </td>
                                                </tr>

                                                 <tr style="height: 36px">
                                                    <td>No Of Packets
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtnoofpackets" runat="server" class="form-control" Width="228px" AutoComplete="Off"> </asp:TextBox>

                                                    </td>
                                                </tr>

                                                 <tr style="height: 36px">
                                                    <td>Narration
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtnarration" runat="server" class="form-control" Width="228px" AutoComplete="Off"> </asp:TextBox>

                                                    </td>
                                                </tr>

                                                 <tr style="height: 36px">
                                                    <td>State Code
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtstatecode" runat="server" class="form-control" Width="228px" AutoComplete="Off"> </asp:TextBox>

                                                    </td>
                                                </tr>


                                                <tr style="height: 36px">
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblresult" runat="server" Text="" Visible="false" Style="color: Red"></asp:Label>
                                                        <asp:Button ID="btnsave" runat="server" ValidationGroup="a1" OnClick="btnsave_Click" Text="Generate" OnClientClick='return confirm("Are you sure you want to add this Item?");'
                                                            ToolTip="SAVE" class=" btn save" />
                                                        <asp:Button ID="btncancel" runat="server" ValidationGroup="a1" Visible="false" Text="Cancel" ToolTip="CANCEL"
                                                            class=" btn save" OnClientClick='return confirm("Are you sure you want to cancel this entry?");' />


                                                        <asp:Button ID="BTNPDF" runat="server" Visible="false" ValidationGroup="a1" OnClick="btninvoiceDownload_Click" Text="PDF"
                                                            ToolTip="Download" class=" btn save" />

                                                        <asp:Button ID="btnMaterial" runat="server" OnClick="btnMaterial_Click" Text="Invoice"
                                                            ToolTip="Download" class=" btn save" />

                                                          <asp:Button ID="btnDeliveryChallan" runat="server" OnClick="btnDeliveryChallan_Click" Text="Delivery Challan"
                                                            ToolTip="Download" class=" btn save" />
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>

                                </table>


                                <div class="rounded_corners" style="overflow: auto; width: 99%; margin-left: 17px">
                                    <asp:GridView ID="gvClientBilling" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-condensed table-hover"
                                        Width="99%" CellPadding="4" CellSpacing="3"
                                        ForeColor="#333333" GridLines="None">

                                        <Columns>
                                            <%-- 0 --%>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>" Width="30px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 1 --%>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lbldesgn" runat="server" Text="" Width="95%" CssClass="txtautofilldesg"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <%-- 2 --%>
                                            <asp:TemplateField HeaderText="HSN Number" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtHSNNumber" runat="server" Width="95%" Style="text-align: left"
                                                        Text=""> </asp:TextBox>

                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 3 --%>
                                            <asp:TemplateField HeaderText="Qty " HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblnoofemployees" runat="server" Text="0" Width="95%"> </asp:TextBox>

                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 4 --%>
                                            <asp:TemplateField HeaderText="UOM" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblUOM" runat="server" Text="" Width="95%"> </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 5 --%>
                                            <asp:TemplateField HeaderText="Pay Rate" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblpayrate" runat="server" Text="0" Width="95%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBPayRate" runat="server" Enabled="True"
                                                        TargetControlID="lblpayrate" ValidChars="-0123456789."></cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 6 --%>
                                            <asp:TemplateField HeaderText="Duties Type" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddldutytype" runat="server" Width="95%">
                                                        <asp:ListItem Value="0">Qty</asp:ListItem>
                                                        <asp:ListItem Value="1">Fixed</asp:ListItem>                                                        
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                        

                                            <%-- 7 --%>
                                            <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblda" runat="server" Text="0" Width="95%"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBDa" runat="server" Enabled="True"
                                                        TargetControlID="lblda" ValidChars="-0123456789."></cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 8 --%>
                                             <asp:TemplateField HeaderText="Discount" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblDiscount" runat="server" Text="0" Width="95%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBDiscount" runat="server" Enabled="True"
                                                        TargetControlID="lblDiscount" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%--9 --%>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblAmount" runat="server" Text="0" Width="99%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 10 --%>
                                            <asp:TemplateField HeaderText="GST %" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblGSTper" runat="server" Text="0" Width="99%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <%-- 11 --%>
                                            <asp:TemplateField HeaderText="CGST">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblCGSTAmount" runat="server" Text="0" Enabled="false" Width="50px"></asp:TextBox>
                                                    <asp:TextBox ID="lblCGSTPrc" runat="server" Text="0" Enabled="false" Visible="false"></asp:TextBox>

                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 12 --%>
                                            <asp:TemplateField HeaderText="SGST">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblSGSTAmount" runat="server" Text="0" Enabled="false" Width="50px"></asp:TextBox>
                                                    <asp:TextBox ID="lblSGSTPrc" runat="server" Text="0" Enabled="false" Visible="false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 13 --%>
                                            <asp:TemplateField HeaderText="IGST">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblIGSTAmount" runat="server" Text="0" Enabled="false" Width="50px"></asp:TextBox>
                                                    <asp:TextBox ID="lblIGSTPrc" runat="server" Text="0" Enabled="false" Visible="false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 14 --%>
                                            <asp:TemplateField HeaderText="Total Amt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblTotalTaxmount" runat="server" Text="0" Enabled="false" Width="80px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>


                                    </asp:GridView>
                                </div>



                                <div style="margin-left: 1%">
                                    <asp:Button ID="btnaddrow" runat="server" Text="Add Row" OnClick="btnaddrow_Click" />
                                    <asp:Button ID="btnCalculateTotals" runat="server" Text="Calculate Totals"
                                        OnClick="btnCalculateTotals_Click" />
                                </div>

                                <table width="100%" cellpadding="5" cellspacing="5">

                                    <tr>
                                        <td width="80%" style="text-align: right">
                                            <asp:Label ID="lblResorces" Text="Duties Amount : " Visible="false" runat="server"></asp:Label>

                                        </td>
                                        <td width="20%" style="text-align: right">
                                            <asp:TextBox ID="txtResources" Text="" Visible="false" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="80%" style="text-align: right">
                                            <asp:Label ID="lblServiceChargeTitle" Text=" Service Charges : " runat="server"></asp:Label>
                                            <asp:TextBox ID="TxtservicechrgPrc" Text="" runat="server" Width="40px"></asp:TextBox>

                                        </td>
                                        <td width="20%" style="text-align: right">
                                            <asp:TextBox ID="lblServiceCharges" Text="" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="100%" style="text-align: right">
                                            <asp:Label ID="lblTotalbeforeTax" Visible="false" Text="Total Before Tax :" runat="server"></asp:Label>
                                            <asp:TextBox ID="TxtTotalbeforeTax" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>
                                        </td>
                                    </tr>


                                    <%-- region for GST as on 17-6-2017 by swathi--%>

                                    <tr>
                                        <td width="80%" style="text-align: right">
                                            <asp:Label ID="lblCGSTTitle" Visible="false" Text="CGST :" runat="server"></asp:Label>
                                            <asp:TextBox ID="TxtCGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                        </td>
                                        <td width="20%" style="text-align: right">
                                            <asp:TextBox ID="lblCGST" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td width="80%" style="text-align: right">
                                            <asp:Label ID="lblSGSTTitle" Visible="false" Text="SGST :" runat="server"></asp:Label>
                                            <asp:TextBox ID="TxtSGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                        </td>
                                        <td width="20%" style="text-align: right">
                                            <asp:TextBox ID="lblSGST" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td width="80%" style="text-align: right">
                                            <asp:Label ID="lblIGSTTitle" Visible="false" Text="IGST :" runat="server"></asp:Label>
                                            <asp:TextBox ID="TxtIGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                        </td>
                                        <td width="20%" style="text-align: right">
                                            <asp:TextBox ID="lblIGST" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>




                                    <tr>
                                        <td width="80%" style="text-align: right; font-weight: bold">
                                            <asp:Label ID="lblgrandtotalss" Text="Grand Total :" Visible="false" runat="server"></asp:Label>
                                        </td>
                                        <td width="20%" style="text-align: right; font-weight: bold">
                                            <asp:TextBox ID="lblGrandTotal" Text="" runat="server" Visible="false" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <asp:Label ID="lblRemarks" Text="" runat="server" Visible="false"></asp:Label>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </div>

                        </div>

                    </div>
                    <%--   </div>--%>
                </div>
                <div class="clear">
                </div>
                <!-- DASHBOARD CONTENT END -->
            </div>
        </div>
        <!-- CONTENT AREA END -->

    </div>

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

                setProperty();
            });
        };
    </script>
</asp:Content>
