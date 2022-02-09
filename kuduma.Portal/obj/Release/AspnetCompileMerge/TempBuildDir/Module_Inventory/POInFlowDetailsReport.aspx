<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master"  CodeBehind="POInFlowDetailsReport.aspx.cs" Inherits="Kuduma.Portal.POInFlowDetailsReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
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
    </style>
    <script type="text/javascript">
        function AssignExportHTML() {

            document.getElementById('hidGridView').value = htmlEscape(forExport.innerHTML);
        }
        function htmlEscape(str) {
            return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            //.replace(/'/g, '&#39;')
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
         </script>
     <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

     
    
    <style type="text/css">
        #social div
        {
            display: block;
        }
        .HeaderStyle
        {
            text-align: Left;
        }
        .style3
        {
            height: 24px;
        }
        
         .modalBackground
            {
            background-color: Gray;
            z-index: 10000;
            }
        
           .slidingDiv
        {
            background-color: #99CCFF;
            padding: 10px;
            margin-top: 10px;
            border-bottom: 5px solid #3399FF;
        }
        .show_hide
        {
            display: none;
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

        function dtval(d, e) {
            var pK = e ? e.which : window.event.keyCode;
            if (pK == 8) { d.value = substr(0, d.value.length - 1); return; }
            var dt = d.value;
            var da = dt.split('/');
            for (var a = 0; a < da.length; a++) { if (da[a] != +da[a]) da[a] = da[a].substr(0, da[a].length - 1); }
            if (da[0] > 31) { da[1] = da[0].substr(da[0].length - 1, 1); da[0] = '0' + da[0].substr(0, da[0].length - 1); }
            if (da[1] > 12) { da[2] = da[1].substr(da[1].length - 1, 1); da[1] = '0' + da[1].substr(0, da[1].length - 1); }
            if (da[2] > 9999) da[1] = da[2].substr(0, da[2].length - 1);
            dt = da.join('/');
            if (dt.length == 2 || dt.length == 5) dt += '/';
            d.value = dt;
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
                select: function (event, ui) { $("#ddlClientID").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#ddlCName").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $('#ddlClientID').trigger('change');

        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {

            $('#ddlCName').trigger('change');
        }

    </script>

        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="ViewItems.aspx" style="z-index: 8;"><span></span>Reports</a></li>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">PO/Inflow Details Report</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">PO/Inflow Details Report
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                                    <div align="right">
                                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="false">Export to Excel</asp:LinkButton>
                                                    </div>
                            <table width="70%" cellpadding="5" cellspacing="5">
                                  
                                            <tr>
                                            

                                             
                                          <td>From Date
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" runat="server" Text="" class="sinput"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                        TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBFromDate" runat="server" Enabled="True" TargetControlID="txtFromDate"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td>To Date
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToDate" runat="server" Text="" class="sinput"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtTo_CalendarExtender" runat="server" Enabled="true"
                                                        TargetControlID="txtToDate" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBToDate" runat="server" Enabled="True" TargetControlID="txtToDate"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>

                                                <td>
                                                    <div align="right">
                                                    <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                                </div>

                                                </td>
                                                
                                            </tr>
                                        </table>
                                    <div  class="rounded_corners" style="overflow: scroll">
                                        <asp:GridView ID="GVPOInFlowDetails" runat="server" AutoGenerateColumns="False"
                                            EmptyDataText="No Records Found" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                            CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GVPOInFlowDetails_RowDataBound" ShowFooter="true">


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
                                                <asp:TemplateField HeaderText="PO Number" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblponumber" runat="server" Text='<%#Bind("pono") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 2--%>


                                                  <asp:TemplateField HeaderText="PO Date" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpodate" runat="server" Text='<%#Bind("DeliveryDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- 3--%>


                                                  <asp:TemplateField HeaderText="Vendor ID & Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="250px" headerstyle-width="250px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemid" runat="server" Text='<%#Bind("VENDORNAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                      <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>


                                                    <%-- 4--%>
                                                <asp:TemplateField HeaderText="Client ID & Name" ItemStyle-HorizontalAlign="Center" headerstyle-width="250px" ItemStyle-Width="250px" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientID" runat="server" Text='<%#Bind("Clientid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                      <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                    <%-- 5--%>

                                                  <asp:TemplateField HeaderText="Total Amt" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltotalprice" runat="server" Text='<%#Bind("totalbuyingprice") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBuyingPrice" Style="font-weight: bold; text-align: center"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                    <%-- 6--%>
                                               
                                                <asp:TemplateField HeaderText="VAT @5%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblvat5" runat="server" Text='<%#Bind("VAT5Per") %>' Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalvat5" Style="font-weight: bold; text-align: center"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                    <%-- 7--%>

                                                <asp:TemplateField HeaderText="VAT @14.5%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblvat14" runat="server" Text='<%#Bind("VAT14Per") %>' Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalvat14" Style="font-weight: bold; text-align: center"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                    <%-- 8--%>

                                                <asp:TemplateField HeaderText="Grand Total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltotal" runat="server" Text='<%#Bind("total") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotaamount" Style="font-weight: bold; text-align: center"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                    <%-- 9--%>

                                                <asp:TemplateField HeaderText="Inflow ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblinflowid" runat="server" Text='<%#Bind("inflowid") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                    <%-- 10--%>

                                                 <asp:TemplateField HeaderText="Inflow Date" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                <asp:Label ID="lblInflowdate" runat="server" Text='<%#Bind("InflowDate") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                    <%-- 11--%>


                                                  <asp:TemplateField HeaderText="Total Amt" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltotalAmt" runat="server" Text='<%#Bind("TotalAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblInFlowTotalBuyingPrice" Style="font-weight: bold; text-align: center"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                    <%-- 12--%>


                                                 <asp:TemplateField HeaderText="VAT @5%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInflowvat5" runat="server" Text='<%#Bind("Vat5") %>' Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblInflowTotalvat5" Style="font-weight: bold; text-align: center"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                    <%-- 13--%>

                                                <asp:TemplateField HeaderText="VAT @14.5%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInflowvat14" runat="server" Text='<%#Bind("Vat14") %>' Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblInflowTotalvat14" Style="font-weight: bold; text-align: center"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                    <%-- 14--%>

                                                <asp:TemplateField HeaderText="Grand Total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInflowtotal" runat="server" Text='<%#Bind("GrandTotal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblInflowTotalamount" Style="font-weight: bold; text-align: center"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>


                                        </asp:GridView>
                                   </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
            <!-- DASHBOARD CONTENT END -->
           
            <!-- CONTENT AREA END -->
        </div>
    </asp:Content>


