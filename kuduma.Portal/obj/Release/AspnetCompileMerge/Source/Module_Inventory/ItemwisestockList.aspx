<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="ItemwisestockList.aspx.cs" Inherits="Kuduma.Portal.ItemwisestockList" %>

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
                    <li class="active"><a href="POReport.aspx" style="z-index: 7;" class="active_bread">Emp Inv Details Report </a></li>
                </ul>
            </div>
            <asp:ScriptManager runat="server" ID="Scriptmanager1">
            </asp:ScriptManager>

          

            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">

                <div class="sidebox">
                    <div class="boxhead">

                        <h2 style="text-align: center">Emp Inv Details Report 
                        </h2>
                    </div>
                    <div class="contentarea" id="Div1">
                        <div class="boxinc">



                            <table cellpadding="5" cellspacing="5" width="80%" style="margin: 10px">

                                <tr style="height: 36px">
                                  <%--  <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch"></asp:Label><span style="color: Red">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" class="form-control" Width="228px">
                                        </asp:DropDownList>

                                    </td>--%>

                                    <td>Item ID<span style="color: Red">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlItemID" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px" class="form-control">
                                            <asp:ListItem Value="0">-Select-</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>


                                <tr>
                                    <td>From Date<span style="color: Red">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_From_Date" Width="180px" runat="server"  class="sinput"
                                            Text=""></asp:TextBox>
                                        <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server"
                                            Enabled="true" Format="dd/MM/yyyy" TargetControlID="Txt_From_Date">
                                        </cc1:CalendarExtender>
                                        <cc1:FilteredTextBoxExtender ID="Txt_Month_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                            ValidChars="/0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>

                                    <td>To Date<span style="color: Red">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_ToDate" Width="180px" runat="server" class="sinput"
                                            Text=""></asp:TextBox>

                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server"
                                            Enabled="true" Format="dd/MM/yyyy" TargetControlID="Txt_ToDate">
                                        </cc1:CalendarExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                            runat="server" Enabled="True" TargetControlID="Txt_ToDate"
                                            ValidChars="/0123456789">
                                        </cc1:FilteredTextBoxExtender>

                                    </td>

                                    <td>
                                        <asp:Button runat="server" ID="btn_Submit" Text="Submit" OnClick="Btn_Submit_OnClick"
                                            class="btn save" /></td>

                                    <td>
                                        <asp:LinkButton ID="Lnkbtnexcel" runat="server" OnClick="Lnkbtnexcel_Click">Export to Excel</asp:LinkButton></td>

                                </tr>

                            </table>


                            <br />
                            <br />

                            <div style="overflow-x: scroll">
                                <asp:GridView ID="GVListOfItems" runat="server" AutoGenerateColumns="True" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    CellPadding="4" ForeColor="#333333">
                                    <Columns>
                                    </Columns>

                                </asp:GridView>
                            </div>

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

