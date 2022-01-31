<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="ModifyInflowDetails.aspx.cs" Inherits="Kuduma.Portal.ModifyInflowDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder1" runat="Server">
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
              
                select: function (event, ui) { $("#<%=ddlPONo.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlInflowID.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },

                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });


        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlPONo.ClientID %>").trigger('change');
        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {
            $("#<%=ddlInflowID.ClientID %>").trigger('change');
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
                        <li class="active"><a href="ModifyInflowDetails.aspx" style="z-index: 7;" class="active_bread">Modify Inflow Details</a></li>
                    </ul>
                </div>
                <asp:ScriptManager runat="server" ID="Scriptmanager1">
                </asp:ScriptManager>

                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">

                    <div class="sidebox">
                        <div class="boxhead">

                            <h2 style="text-align: center">Modify Inflow Details
                            </h2>
                        </div>
                        <div class="contentarea" id="Div1">
                            <div class="boxinc">



                                <%--  <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>


                                <table cellpadding="5" cellspacing="5" width="100%" style="margin: 10px">

                                   <%--  <tr style="height:37px">
                                         <td>
                                              Branch
                                              </td>
                                           <td>
                                             <asp:DropDownList ID="ddlbranch" runat="server" Width="228px" class="form-control" Enabled="false">
                                                                           </asp:DropDownList>
                                                                        </td>
                                                                    </tr>--%>
                                    <tr style="height:37px">
                                        <td>PO No</td>
                                        <td>
                                            <asp:DropDownList ID="ddlPONo" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlPONo_OnSelectedIndexChanged">
                                                <asp:ListItem Value="0">-Select-</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>Inflow ID</td>
                                        <td>
                                            <asp:DropDownList ID="ddlInflowID" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlInflowID_OnSelectedIndexChanged">
                                                <%--<asp:ListItem Value="0">-Select-</asp:ListItem>--%>
                                            </asp:DropDownList>


                                        </td>

                                        <td>Manual Inflow ID</td>
                                        <td>
                                            <asp:TextBox ID="txtManualInfID" runat="server" class="form-control" Width="190px" Enabled="false"></asp:TextBox>

                                        </td>


                                    </tr>
                                    <tr>
                                        <td>Inflow Date</td>
                                        <td>
                                            <asp:TextBox ID="txtDate" runat="server" class="form-control" Width="228px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CE_To_Date" runat="server" Enabled="True" TargetControlID="txtDate"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBE_To_Date" runat="server" Enabled="True" TargetControlID="txtDate"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>

                                    </tr>

                                </table>


                                <div align="right" style="margin-right: 15px; margin-top: 10px">
                                    <asp:Label ID="lblresult" runat="server" Text="" Visible="false" Style="color: Red"></asp:Label>
                                    <asp:Button ID="btnsave" runat="server" ValidationGroup="a1" Text="Save" OnClientClick='return confirm("Are you sure you want to add this Item?");'
                                        ToolTip="Save" class=" btn save" OnClick="BtnSave_Click" />
                                    <asp:Button ID="btncancel" runat="server" ValidationGroup="a1" Text="Cancel" ToolTip="CANCEL"
                                        class=" btn save" OnClientClick='return confirm("Are you sure you want to cancel this entry?");' />
                                </div>
                                <%-- AutoPostBack="true" OnCheckedChanged="CbChecked_CheckedChanged" OnRowDataBound="gvresources_databound"--%>
                                <div class="rounded_corners" style="margin-top: 10px;">
                                    <asp:GridView ID="gvresources" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                        ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center; margin: 0px auto" Height="50px">

                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item ID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemID" runat="server" Text='<%#Bind("ItemId")%>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltype" runat="server" Text='<%#Bind("type")%>' Width="120px" Visible="false"> </asp:Label>

                                                    <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("ItemName")%>' Width="120px"> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UOM" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUOM" runat="server" Text='<%#Bind("UnitMeasure")%>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ordered Qty" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderedQty" runat="server" Text='<%#Bind("OrderedQty")%>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <asp:TemplateField HeaderText="Already Delivered Qty" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAlreadyDeliveredQty" runat="server" Text="0"  Width="150px"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Balance" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBalance" runat="server" Text="0"  Width="90px"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <asp:TemplateField HeaderText="Deliverd Qty" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDeliverdQty" runat="server" Text='<%#Bind("DeliveredQty")%>'  Width="90px" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtDeliverdQty_OnTextChanged"> </asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Buying Price" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBuyingPrice" runat="server" Text='<%#Bind("BuyingPrice")%>'  Width="90px" CssClass="form-control"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                        </Columns>

                                    </asp:GridView>
                                </div>



                                <%-- </ContentTemplate>
                                    <Triggers>

                                        <asp:AsyncPostBackTrigger ControlID="ddlPONo" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlInflowID" EventName="SelectedIndexChanged" />

                                        <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />

                                    </Triggers>
                                </asp:UpdatePanel>

                                --%>
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
</asp:content>
