<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="ModifyItem.aspx.cs" Inherits="Kuduma.Portal.ModifyItem" %>

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
                select: function (event, ui) { $("#<%=ddlItemID.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },

                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlItemID.ClientID %>").trigger('change');
         }

    </script>


    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">


                    <li class="first"><a href="ViewItems.aspx" style="z-index: 9;"><span></span>Inventory</a></li>
                    <li class="active"><a href="ModifyItem.aspx" style="z-index: 7;" class="active_bread">Modify Item</a></li>
                </ul>
            </div>
            <asp:ScriptManager runat="server" ID="Scriptmanager1">
            </asp:ScriptManager>
            <div class="dashboard_full">
                <div style="float: right; font-weight: bold">
                    <%-- Select Import Data: --%>
                    <asp:FileUpload ID="fileupload1" runat="server" Width="50px" Visible="false" />


                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">

                    <div class="sidebox">
                        <div class="boxhead">

                            <h2 style="text-align: center">Modify Item     
                            </h2>
                        </div>
                        <div class="contentarea" id="Div1">
                            <div class="boxinc">

                                <ul>

                                    <li class="right">

                                        <table width="130%" cellpadding="5" cellspacing="5" style="margin-left: 10px">
                                            <tr>
                                                <td>
                                                    <table width="100%" cellpadding="5" cellspacing="5" style="margin: 10px">
                                                        <tr style="height: 36px">
                                                            <td>Item ID
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlItemID" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px" class="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlItemID_OnSelectedIndexChanged">
                                                                    <asp:ListItem Value="0">-Select-</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>Units of Measure<span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlmesure" runat="server" class="form-control" Width="228px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>Brand
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtbrand" runat="server" class="form-control" Width="228px"> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>Buying Price
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtprice" runat="server" class="form-control" Width="228px"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                    TargetControlID="txtprice" ValidChars="0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 32px">
                                                            <td>HSN Number
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtHSNNumber" runat="server" class="form-control" Width="228px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>VAT
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox runat="server" Checked="false" ID="ChkVATCmp1" Text="" Visible="false" />
                                                                &nbsp;&nbsp;
                                                                    <asp:CheckBox runat="server" Checked="false" ID="ChkVATCmp2" Text="" Visible="false" />
                                                                &nbsp;&nbsp;
                                                                    <asp:CheckBox runat="server" Checked="false" ID="ChkVATCmp3" Text="" Visible="false" />
                                                                &nbsp;&nbsp;
                                                                    <asp:CheckBox runat="server" Checked="false" ID="ChkVATCmp4" Text="" Visible="false" />
                                                                &nbsp;&nbsp;
                                                                    <asp:CheckBox runat="server" Checked="false" ID="ChkVATCmp5" Text="" Visible="false" />
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </td>

                                                <td valign="top">
                                                    <table width="100%" cellpadding="5" cellspacing="5" style="margin: 10px">

                                                        <tr style="height: 36px">
                                                            <td>Item Name<span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                <%--<asp:DropDownList ID="ddlitemname" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px"  class="form-control"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlitemname_OnSelectedIndexChanged">
                                                                        <asp:ListItem Value="0">-Select-</asp:ListItem>
                                                                    </asp:DropDownList>--%>

                                                                <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control" Width="228px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>Minimum Quantity
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtmq" runat="server" class="form-control" Width="228px"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                    TargetControlID="txtmq" ValidChars="0123456789">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>Category
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCategory" runat="server" class="form-control" Width="228px">
                                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                    <asp:ListItem Value="General">General</asp:ListItem>
                                                                    <asp:ListItem Value="Uniform">Uniform</asp:ListItem>
                                                                    <asp:ListItem Value="Security">Security</asp:ListItem>
                                                                    <asp:ListItem Value="Manpower">Manpower</asp:ListItem>
                                                                    <asp:ListItem Value="Staff">Staff</asp:ListItem>
                                                                    <asp:ListItem Value="Housekeeping">Housekeeping</asp:ListItem>
                                                                    <asp:ListItem Value="Others">Others</asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>Selling price
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtsellingprice" runat="server" class="form-control" Width="228px"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTBEsellingprice" runat="server" Enabled="True"
                                                                    TargetControlID="txtsellingprice" ValidChars="0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>GST %
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlGSTPer" runat="server" class="form-control" Width="228px">
                                                                    <asp:ListItem>0</asp:ListItem>
                                                                    <asp:ListItem>5</asp:ListItem>
                                                                    <asp:ListItem>12</asp:ListItem>
                                                                    <asp:ListItem>18</asp:ListItem>
                                                                    <asp:ListItem>28</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <div align="right" style="margin-right: 15px">
                                            <asp:Label ID="lblresult" runat="server" Text="" Visible="false" Style="color: Red"></asp:Label>
                                            <asp:Button ID="Button1" runat="server" ValidationGroup="a1" Text="Save" OnClientClick='return confirm("Are you sure you want to Modify this Item?");'
                                                ToolTip="SAVE" class=" btn save" OnClick="BtnSave_Click" />
                                            <asp:Button ID="btncancel" runat="server" ValidationGroup="a1" Text="Cancel" ToolTip="CANCEL"
                                                class=" btn save" OnClientClick='return confirm("Are you sure you want to cancel this entry?");' />
                                        </div>

                                    </li>
                                </ul>


                            </div>
                            <div class="clear">
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
</asp:Content>
