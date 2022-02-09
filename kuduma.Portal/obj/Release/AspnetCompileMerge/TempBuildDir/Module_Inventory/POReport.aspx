<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="POReport.aspx.cs" Inherits="Kuduma.Portal.POReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
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



        .Grid th, .Grid td {
            border: 1px solid #66CCFF;
        }
    </style>

    <script type="text/javascript">

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
                select: function (event, ui) { $("#ddlPONo").attr("data-clientId", ui.item.value); OnAutoCompleteDDLPONochange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLPONochange(event, ui) {
            $('#ddlPONo').trigger('change');

        }



        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }
        function checkAll(objRef) {

            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        inputList[i].checked = false;
                    }
                }
            }
        }

    </script>

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="ViewItems.aspx" style="z-index: 9;"><span></span>Inventory</a></li>
                    <li class="active"><a href="POReport.aspx" style="z-index: 7;" class="active_bread">PO Details Report </a></li>
                </ul>
            </div>
            <asp:ScriptManager runat="server" ID="Scriptmanager1">
            </asp:ScriptManager>

            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">

                <div class="sidebox">
                    <div class="boxhead">

                        <h2 style="text-align: center">PO Details Report 
                        </h2>
                    </div>
                    <div class="contentarea" id="Div1">
                        <div class="boxinc">



                            <table cellpadding="5" cellspacing="5" width="60%" style="margin: 10px">
                                <tr>
                                    <td>PO No</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPONo" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px">
                                            <asp:ListItem Value="0">-Select-</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                    <td>
                                        <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                    </td>
                                    <td>
                                        <div align="right">
                                            <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="false">Export to Excel</asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>

                            </table>


                            <br />
                            <br />

                            <%--  <asp:HiddenField ID="hidGridView" runat="server" />--%>
                            <asp:GridView ID="GVPODetails" runat="server" AutoGenerateColumns="False"
                                EmptyDataText="No Records Found" Width="966px" CssClass="table table-striped table-bordered table-condensed table-hover"
                                CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GVPODetails_RowDataBound" ShowFooter="true">


                                <Columns>


                                    <%-- 0--%>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <%-- 1--%>
                                    <%--    <asp:TemplateField HeaderText="Client ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <%-- 2

                                                <asp:TemplateField HeaderText="Client Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("clientname") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>


                                    <%-- 3--%>
                                    <asp:TemplateField HeaderText="Item Id" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemid" runat="server" Text='<%#Bind("ItemId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- 4--%>
                                    <asp:TemplateField HeaderText="Item Name" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemname" runat="server" Text='<%#Bind("ItemName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblGrandTotal" Text="Grand Total" Style="font-weight: bold; text-align: center"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%-- 5--%>
                                    <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%#Bind("Qty") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalQty" Style="font-weight: bold; text-align: center"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%-- 5--%>
                                    <asp:TemplateField HeaderText="Buying Price" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblbuyingpriceunit" runat="server" Text='<%#Bind("BuyingPrice") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalBuyingPriceunit" Style="font-weight: bold; text-align: center"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltotalprice" runat="server" Text='<%#Bind("Total") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalBuyingPrice" Style="font-weight: bold; text-align: center"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%-- 6--%>
                                    <asp:TemplateField HeaderText="Vat 5%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblvat5" runat="server" Text='<%#Bind("VAT5Per") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalvat5" Style="font-weight: bold; text-align: center"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%-- 7--%>
                                    <asp:TemplateField HeaderText="Vat 14.5%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblvat14" runat="server" Text='<%#Bind("VAT14Per") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalvat14" Style="font-weight: bold; text-align: center"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <%-- 8--%>
                                    <asp:TemplateField HeaderText="Grand Total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltotal" runat="server" Text='<%#Bind("grandtotal") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotaamount" Style="font-weight: bold; text-align: center"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Balance" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBalance" runat="server" Text='<%#Bind("Balance") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                </Columns>


                            </asp:GridView>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="clear">
    </div>


    <!-- CONTENT AREA END -->

</asp:Content>



