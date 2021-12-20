<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Clients/Clients.master" CodeBehind="ReceiptDetails.aspx.cs" Inherits="Kuduma.Portal.ReceiptDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/jscript.js">
    </script>
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
                select: function (event, ui) { $("#<%=ddlClientID.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlCName.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlClientID.ClientID %>").trigger('change');
        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {
            $("#<%=ddlCName.ClientID %>").trigger('change');
        }
    </script>

        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Receipts.aspx" style="z-index: 9;"><span></span>Receipts</a></li>
                    <li class="active"><a href="ReceiptDetails.aspx" style="z-index: 8;" class="active_bread">Receipt Details</a></li>
                </ul>
            </div>

                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_full">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Receipt Details
                                </h2>
                            </div>
                            <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                            </asp:ScriptManager>

                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <!--  Content to beDelete Employee
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px; min-height:300px; height:auto">
                            <!--  Content to be add here> -->
                                <div class="boxin">
                                    <div class="content">
                                        <div class="dashboard_firsthalf" style="width: 100%">
                                            
                                            <table width="100%" cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>Client ID<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlClientID" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged" Width="100px">
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td>Client Name :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCName" runat="server" AutoPostBack="True" TabIndex="2" class="ddlautocomplete chosen-select"
                                                            OnSelectedIndexChanged="ddlCName_SelectedIndexChanged" Width="300px">
                                                        </asp:DropDownList>
                                                    </td>


                                                    <td>Receipt No
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlReceiptNos" runat="server" class="form-controldrop" TabIndex="3" Width="100px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" class=" btn save" OnClick="btnSearch_Click" ToolTip="Search" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="rounded_corners" style="overflow: auto; margin-top: 50px">
                                        <asp:GridView ID="gvreciepts" runat="server" CellPadding="2" ForeColor="Black" Style="margin: 0px auto"
                                            AutoGenerateColumns="False" Width="90%" BackColor="#f9f9f9" BorderColor="LightGray" PageSize="15" OnPageIndexChanging="gvreciepts_PageIndexChanging" OnRowDeleting="gvreciepts_RowDeleting"
                                            BorderWidth="1px" AllowPaging="True">
                                            <RowStyle Height="30px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>



                                                <asp:TemplateField HeaderText="Client Id" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientid" runat="server" Text='<%#Bind("clientid")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="40px"></ItemStyle>
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="Client Name" ItemStyle-Width="300px" DataField="clientname">
                                                    <ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Receipt No" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReceiptNo" runat="server" Text='<%#Bind("RecieptNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50px"></ItemStyle>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Bill No(s)" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBillNo" runat="server" Text='<%#Bind("billno")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px"></ItemStyle>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Actions">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/css/assets/download.png" runat="server" OnClick="lbtn_download_Click" ToolTip="Download" />
                                                        <asp:ImageButton ID="lbtn_Edit" ImageUrl="~/css/assets/edit.png" runat="server" OnClick="lbtn_Edit_Click" ToolTip="Edit" />
                                                        <asp:ImageButton ID="linkdelete" CommandName="Delete" ImageUrl="~/css/assets/delete.png" runat="server"
                                                            ToolTip="Delete" OnClientClick='return confirm("Do you want to delete this record ?");' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="60px"></ItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="Tan" />
                                            <PagerStyle BackColor="LightBlue" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                            <HeaderStyle BackColor="LightBlue" Font-Bold="True" Height="30px" />
                                            <AlternatingRowStyle BackColor="White" Height="30px" />
                                        </asp:GridView>
                                    </div>



                                </div>
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <!-- DASHBOARD CONTENT END -->
                </div>
            </div>
            <!-- CONTENT AREA END -->
   </div>
    </asp:content>
