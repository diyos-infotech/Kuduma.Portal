<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Clients/Clients.master" CodeBehind="AddReceipts.aspx.cs" Inherits="Kuduma.Portal.AddReceipts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
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
    <script type="text/javascript" src="script/jscript.js">
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
                    <li class="active"><a href="AddReceipts.aspx" style="z-index: 8;" class="active_bread">Add Receipts</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                        <div class="boxhead">
                          
                            <h2 style="text-align: center">
                                Add Receipts
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
                            
                            <div class="dashboard_firsthalf" style="width:100%">
                             <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td valign="top">
                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>
                                                    Client ID<span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlClientID" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Receipt No
                                                </td>
                                                 <td>
                                                    <asp:TextBox ID="txtRecieptNo" runat="server" class="form-control" Enabled="False" TabIndex="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                               <td>
                                                    Amount :<span style="color: Red">*</span>

                                                </td>
                                              <td>
                                                    <asp:TextBox ID="txtAmount" runat="server" class="form-control" TabIndex="5"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                        TargetControlID="txtAmount" FilterMode="ValidChars" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td>
                                                    DD (Or) Cheque No :
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtddorcheckno" runat="server" class="form-control" TabIndex="7"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Bank Name :
                                                </td>
                                                 <td>
                                                    <asp:TextBox ID="txtbankname" runat="server" class="form-control" ></asp:TextBox>
                                                    <asp:DropDownList ID="Ddl_Bank" runat="server"  class="form-controldrop"   TabIndex="9" Visible="false"></asp:DropDownList>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                              <td>
                                                    Extra Amount :
                                                </td>
                                                 <td>
                                                    <asp:TextBox ID="txtextraAmount" runat="server" class="form-control" TabIndex="11" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                 <td colspan="2">
                                                    <asp:Label ID="lblresult" runat="server" Visible="false" Style="color: Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        </td>
                                        
                                       <td valign="top">
                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>
                                                    Client Name :
                                                </td>
                                               <td>
                                                    <asp:DropDownList ID="ddlCName" runat="server" AutoPostBack="True" TabIndex="2" class="ddlautocomplete chosen-select"
                                                        OnSelectedIndexChanged="ddlCName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Received Date:<span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDate" runat="server" class="form-control" TabIndex="4"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtdate_CalendarExtender" runat="server" Enabled="true"
                                                        TargetControlID="txtDate" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        TargetControlID="txtDate" ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Received Mode<span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlReciveMode" class="form-controldrop" TabIndex="6">
                                                        <asp:ListItem>--Select--</asp:ListItem>
                                                        <asp:ListItem>Cash</asp:ListItem>
                                                        <asp:ListItem>DD</asp:ListItem>
                                                        <asp:ListItem>Cheque</asp:ListItem>
                                                        <asp:ListItem>NEFT/RTGS</asp:ListItem>
                                                        <%--<asp:ListItem>Discount</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                               <td>
                                                    DD (or) Cheque Date:
                                                </td>
                                               <td>
                                                    <asp:TextBox ID="txtddorcheckdate" runat="server" class="form-control" TabIndex="8"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true" TargetControlID="txtddorcheckdate"
                                                        Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                        TargetControlID="txtddorcheckdate" ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr style="visibility:hidden">
                                                <td>
                                                    DD (or) Cheque Status:<span style="color: Red">*</span>
                                                </td>
                                                 <td>
                                                    <asp:DropDownList ID="ddlDDOrCheckstatus" runat="server" class="form-controldrop" TabIndex="10">
                                                        <asp:ListItem>--Select--</asp:ListItem>
                                                        <asp:ListItem>Deposited</asp:ListItem>
                                                        <asp:ListItem>Cleared</asp:ListItem>
                                                        <asp:ListItem>Bounsed</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;&nbsp
                                                </td>
                                            </tr>
                                        </table>
                                        </td>
                                        </tr>
                                        </table>
                             </div>

                                <div style="float:right;margin-right:20px">
                                    <asp:Button ID="btnaddclint" runat="server" Text="Save" class="btn save" ValidationGroup="a"
                                    OnClick="btnaddReciept_Click" OnClientClick='return confirm(" Are you sure you  want to add this record ?");' />
                                <asp:Button ID="btncancel" runat="server" Text="Cancel" ToolTip="Cancel Client" OnClientClick='return confirm(" Are you sure you  want to cancel this entry ?");'
                                    class="btn save" />
                                </div>
                          
                            <div class="rounded_corners" style="overflow:auto">
                                <asp:GridView ID="gvreciepts" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-BackColor="BlueViolet"
                                    EmptyDataRowStyle-BorderColor="Aquamarine" EmptyDataRowStyle-Font-Italic="true" ShowFooter="true"
                                    EmptyDataText="No Records Found" Width="100%" HeaderStyle-HorizontalAlign="Center"
                                    RowStyle-HorizontalAlign="Center" CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="gvreciepts_RowDataBound" OnRowCommand="gvreciepts_RowCommand">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <EmptyDataRowStyle BackColor="BlueViolet" BorderColor="Aquamarine" Font-Italic="True">
                                    </EmptyDataRowStyle>
                                    <Columns>
                                      <%-- 0--%>

                                     

                                        <asp:TemplateField Visible="false">
                                             <ItemTemplate>
                                                <asp:Label ID="lblmonthn" runat="server" Text='<%#Bind("month") %>' style="font-weight:bold"></asp:Label>

                                                 </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- 1--%>
                                          <asp:TemplateField>
                                             <ItemTemplate>
                                                        <asp:ImageButton ID="lbtn_Add" ImageUrl="~/css/assets/plus.png" runat="server" CommandName="Add" ToolTip="Edit" />
                                                 </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- 2--%>
                                        <asp:TemplateField HeaderText="Details" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text="Client" style="font-weight:bold"></asp:Label>
                                                <asp:Label ID="Label4" runat="server" Text=" : " style="font-weight:bold"></asp:Label>
                                                <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("unitid") %>'></asp:Label><br />
                                                <asp:Label ID="Label1" runat="server" Text="Bill No" style="font-weight:bold"></asp:Label>
                                                <asp:Label ID="Label5" runat="server" Text=" : " style="font-weight:bold"></asp:Label>
                                                <asp:Label ID="lblbillno" runat="server" Text='<%#Bind("Billno") %>'></asp:Label><br />
                                                <asp:Label ID="Label3" runat="server" Text="Month" style="font-weight:bold"></asp:Label>
                                                <asp:Label ID="Label6" runat="server" Text=" : " style="font-weight:bold"></asp:Label>
                                                <asp:Label ID="lblmonth" runat="server" Text=""></asp:Label>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      <%-- 3--%>
                                        <asp:TemplateField HeaderText="Total Bill Amt" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrandtotal" runat="server" Text='<%#Bind("Total") %>'></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalgrandtotal"></asp:Label>
                                                </FooterTemplate>
                                        </asp:TemplateField>
                                        <%-- 4--%>
                                        <asp:TemplateField HeaderText="TDS Amt">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txttdsamt" runat="server"   Text='<%#Bind("tdsAmt","{0:0}") %>' AutoPostBack="true" OnTextChanged="txtrecievedamt_OnTextChanged"
                                                    Width="50px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- 5--%>
                                        <asp:TemplateField HeaderText="Net Payble Amt" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblnetpaybleamt" runat="server" Text='<%#Bind("netpayable","{0:0}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- 6--%>
                                        <asp:TemplateField HeaderText="Pending Amt" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpendingamt" runat="server" Text='<%#Bind("netpayable","{0:0}") %>'></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalpendingamt"></asp:Label>
                                                </FooterTemplate>
                                        </asp:TemplateField>
                                        <%-- 7--%>
                                        <asp:TemplateField HeaderText="Received Amt" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtrecievedamt" runat="server" Text="" AutoPostBack="true" OnTextChanged="txtrecievedamt_OnTextChanged"
                                                    Width="50px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- 8--%>
                                        <asp:TemplateField HeaderText="Disallowance" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDisallowance" runat="server" Text="" AutoPostBack="true" OnTextChanged="txtrecievedamt_OnTextChanged"
                                                    Width="50px"></asp:TextBox><br />
                                                 <asp:TextBox ID="txtDisallowanceReason" runat="server" Text="" placeholder="Reason" TextMode="MultiLine"
                                                    Width="160px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Due Amt" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldueamt" runat="server" Text="" Width="70px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle HorizontalAlign="Center" BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30">
                                    </HeaderStyle>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                  </div>
                          
                           <div style="margin-right: 10px;float: right;">
                                <asp:Button ID="btnreceipt" runat="server" Text="Receipt" class="btn save" Visible="false"
                                    OnClick="btnreceipt_Click" />
                                
                            </div>
                            
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
 </asp:Content>
      
