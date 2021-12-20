<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="InvInflowDetails.aspx.cs" Inherits="Kuduma.Portal.InvInflowDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function () {
            ShowProgress();
        });
    </script>
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

        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: rgba(195,195,195,0.5);
            z-index: 10000;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
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

                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

       

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlPONo.ClientID %>").trigger('change');
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

        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="ViewItems.aspx" style="z-index: 9;"><span></span>Inventory</a></li>
                        <li class="active"><a href="InvInflowDetails.aspx" style="z-index: 7;" class="active_bread">Add Inflow Details</a></li>
                    </ul>
                </div>
                <asp:ScriptManager runat="server" ID="Scriptmanager1">
                </asp:ScriptManager>

                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">

                    <div class="sidebox">
                        <div class="boxhead">

                            <h2 style="text-align: center">Add Inflow Details 
                            </h2>
                        </div>
                        <div class="contentarea" id="Div1">
                            <div class="boxinc">



                                <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellpadding="5" cellspacing="5" width="100%" style="margin: 10px">
                                           <%-- <tr>
                                                <td>
                                                    Branch
                                                </td>
                                                 <td>
                                              <asp:DropDownList ID="ddlbranch" runat="server" Width="228px" class="form-control">
                                                  </asp:DropDownList>
                                                 </td>
                                            </tr>--%>
                                            <tr>
                                                <td>PO No</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPONo" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlPONo_OnSelectedIndexChanged">
                                                       <%-- <asp:ListItem Value="0">-Select-</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>Inflow ID</td>
                                                <td>
                                                    <asp:TextBox ID="txtInflowID" runat="server" class="form-control" Width="190px" ReadOnly="true"></asp:TextBox>

                                                </td>

                                                <td>Manual Inflow ID</td>
                                                <td>
                                                    <asp:TextBox ID="txtManualInfID" runat="server" class="form-control" Width="190px" Enabled="false"></asp:TextBox>

                                                </td>

                                            </tr>
                                            <tr style="height: 36px">

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
                                                <td>
                                                    <asp:Label ID="lblmonth" runat="server" Text="Month" Visible="false"></asp:Label>
                                                </td>

                                                <td>
                                                    <asp:TextBox ID="txtMonth" runat="server" class="form-control" Width="228px" Visible="false" Enabled="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="MMM-yyyy" TargetControlID="txtMonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                    </cc1:CalendarExtender>

                                                </td>
                                            </tr>
                                            
                                        </table>


                                    </ContentTemplate>
                                    <Triggers>

                                        <asp:AsyncPostBackTrigger ControlID="ddlPONo" EventName="SelectedIndexChanged" />


                                    </Triggers>
                                </asp:UpdatePanel>

                                <div align="right" style="margin-right: 15px; margin-top: 10px">
                                    <asp:Label ID="lblresult" runat="server" Style="color: Red"></asp:Label>
                                    <asp:Button ID="btnsave" runat="server" ValidationGroup="a1" Text="Save" OnClientClick='return confirm("Are you sure you want to add this Item?");'
                                        ToolTip="Save" class=" btn save" OnClick="BtnSave_Click" />
                                    <asp:Button ID="btncancel" runat="server" ValidationGroup="a1" Text="Cancel" ToolTip="CANCEL"
                                        class=" btn save" OnClientClick='return confirm("Are you sure you want to cancel this entry?");' />
                                </div>
                                <asp:UpdatePanel ID="uppanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <%-- AutoPostBack="true" OnCheckedChanged="CbChecked_CheckedChanged" OnRowDataBound="gvresources_databound"--%>
                                        <div class="rounded_corners" style="margin-top: 10px;">
                                            <asp:GridView ID="gvresources" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                                ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center; margin: 0px auto" Height="50px">

                                                <Columns>
                                                   <%-- <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>

                                                    <asp:TemplateField HeaderText="Item ID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemID" runat="server" Text='<%#Bind("ItemId")%>'> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("ItemName")%>' Width="180px"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="UOM" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUOM" runat="server" Text='<%#Bind("UnitMeasure")%>'> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtItemRemarks" runat="server" Text='<%#Bind("ItemRemarks") %>' CssClass="form-control" Width="80px" TextMode="MultiLine"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ordered Qty" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrderedQty" runat="server" Text='<%#Bind("qty")%>'> </asp:Label>
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

                                                     <asp:TemplateField HeaderText="Delivered Qty" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDeliverdQty" runat="server" Text="0"  Width="90px" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtDeliverdQty_OnTextChanged"> </asp:TextBox>
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

                                    </ContentTemplate>
                                    <%-- <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>




                            </div>

                            <%--   <div class="loading" align="center">

                                    <img src="assets/loader.gif" alt="" />
                                </div>--%>


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
        <!-- FOOTER END -->

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
