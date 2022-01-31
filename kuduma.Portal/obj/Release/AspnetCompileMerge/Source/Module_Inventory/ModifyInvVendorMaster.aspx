<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="ModifyInvVendorMaster.aspx.cs" Inherits="Kuduma.Portal.ModifyInvVendorMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <<link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <script type="text/javascript" src="script/jscript.js">
    </script>
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
                select: function (event, ui) { $("#<%=ddlVendorID.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
            select: function (event, ui) { $("#<%=ddlVendorName.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
            minLength: 4
        });

    }

    $(document).ready(function () {
        setProperty();
    });


    function OnAutoCompleteDDLClientidchange(event, ui) {
        $("#<%=ddlVendorID.ClientID %>").trigger('change');
    }

    function OnAutoCompleteDDLClientnamechange(event, ui) {
        $("#<%=ddlVendorName.ClientID %>").trigger('change');
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
                    <li class="active"><a href="ModifyInvVendorMaster.aspx" style="z-index: 7;" class="active_bread">Modify Vendor Details </a></li>
                </ul>
            </div>

            <div class="dashboard_full">
                <div style="float: right; font-weight: bold">
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">

                    <div class="sidebox">
                        <div class="boxhead">

                            <h2 style="text-align: center">Modify Vendor Details  
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
                                                      <%--  <tr>
                                                            <td>
                                                                Branch
                                                            </td>

                                                             <td>
                                             <asp:DropDownList ID="ddlbranch" runat="server" Width="228px" class="form-control" Enabled="false">
                                                            </asp:DropDownList>
                                                                        </td>

                                                        </tr>--%>
                                                        <tr style="height: 36px">
                                                            <td>Vendor ID
                                                            </td>
                                                            <td>

                                                                <asp:DropDownList ID="ddlVendorID" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px" class="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlVendorID_OnSelectedIndexChanged">
                                                                    <asp:ListItem Value="0">-Select-</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>Contact Person 
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtContactPerson" runat="server" class="form-control" Width="228px"></asp:TextBox>

                                                            </td>
                                                        </tr>

                                                        <tr style="height: 36px">
                                                            <td>Address
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAddress" runat="server" class="form-control" Width="228px" TextMode="MultiLine" Height="50px"> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>Email ID
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtemailid" runat="server" class="form-control" Width="228px" Text=""> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                                <td valign="top">
                                                    <table width="100%" cellpadding="5" cellspacing="5" style="margin: 10px">
                                                        <tr style="height: 36px">
                                                            <td>Vendor Name
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlVendorName" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px" class="form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlVendorName_OnSelectedIndexChanged">
                                                                    <asp:ListItem Value="0">-Select-</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 36px">
                                                            <td>Contact Nos 
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtContactNos" runat="server" class="form-control" Width="228px"></asp:TextBox>

                                                            </td>
                                                        </tr>

                                                        <tr style="height: 36px">
                                                            <td>Remarks
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRemarks" runat="server" class="form-control" Width="228px" TextMode="MultiLine" Height="50px"> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 32px">
                                                            <td>Vendor GSTNo
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtvendorgst" runat="server" class="form-control" Width="190px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <div align="right" style="margin-right: -100px; margin-bottom: 7px">
                                            <asp:Label ID="lblresult" runat="server" Text="" Visible="false" Style="color: Red"></asp:Label>
                                            <asp:Button ID="Button1" runat="server" ValidationGroup="a1" Text="Save" OnClientClick='return confirm("Are you sure you want to add this Item?");'
                                                ToolTip="SAVE" class=" btn save" OnClick="BtnSave_Click" />
                                            <asp:Button ID="btncancel" runat="server" ValidationGroup="a1" Text="Cancel" ToolTip="CANCEL"
                                                class=" btn save" OnClientClick='return confirm("Are you sure you want to cancel this entry?");' />
                                        </div>



                                        <div class="rounded_corners" style="width: 124%; overflow-x: scroll">
                                            <asp:GridView ID="GVModifyItemList" runat="server" AutoGenerateColumns="False" Width="124%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                                CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="none" OnRowDataBound="GVItemList_RowDataBound">

                                                <Columns>

                                                    <asp:TemplateField HeaderStyle-Width="10px">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Item ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblitemid" runat="server" Text='<%#Bind("itemid") %>' Width="70px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Item Name" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblitemname" runat="server" Text='<%#Bind("itemname") %>' Width="300px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="HSN No" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHSNNo" runat="server" Text='<%#Bind("HSNNumber") %>' Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Buying Price" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblBuyingPrice" runat="server" Text='<%#Bind("BuyingPrice","{0:0.##}") %>' Width="90px" CssClass="form-control" AutoPostBack="true" OnTextChanged="lblBuyingPrice_OnTextChanged"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="GST Per." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGSTPer" runat="server" Text='<%#Bind("GSTPer","{0:0.##}") %>' Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GST Amt" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGSTAmt" runat="server" Text='<%#Bind("GSTAmt","{0:0.##}") %>' Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="VATCmp1" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVATCmp1" runat="server" Text='<%#Bind("VATCmp1","{0:0.##}") %>' Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="VATCmp2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVATCmp2" runat="server" Text='<%#Bind("VATCmp2","{0:0.##}") %>' Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="VATCmp3" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVATCmp3" runat="server" Text='<%#Bind("VATCmp3","{0:0.##}") %>' Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="VATCmp4" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVATCmp4" runat="server" Text='<%#Bind("VATCmp4","{0:0.##}") %>' Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="VATCmp5" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVATCmp5" runat="server" Text='<%#Bind("VATCmp5","{0:0.##}") %>' Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal" runat="server" Text='<%#Bind("Total","{0:0.##}") %>' Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblType" runat="server" Text='<%#Bind("type")%>'> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>


                                            </asp:GridView>
                                            <asp:Label ID="lblmsg" runat="server" Text="" Visible="false" Style="color: Red"></asp:Label>

                                        </div>

                                    </li>
                                </ul>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
            <!-- DASHBOARD CONTENT END -->
        </div>
    </div>
    <!-- CONTENT AREA END -->
</asp:Content>
