<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ArrearWages.aspx.cs" Inherits="Kuduma.Portal.ArrearWages" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
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

        .GridviewScrollHeader TD {
            padding: 5px;
            font-weight: bold;
            white-space: normal;
            border-right: 1px solid #AAAAAA;
            border-bottom: 1px solid #AAAAAA;
            background-color: #EFEFEF;
            text-align: left;
            vertical-align: bottom;
            height: 70px;
        }

        .GridviewScrollItem TD {
            border-right: 1px solid #AAAAAA;
            border-bottom: 1px solid #AAAAAA;
            background-color: #FFFFFF;
            white-space: normal;
        }

        .GridviewScrollPager {
            border-top: 1px solid #AAAAAA;
            background-color: #FFFFFF;
        }



            .GridviewScrollPager TD {
                font-size: 14px;
            }

            .GridviewScrollPager A {
                color: #666666;
            }

            .GridviewScrollPager SPAN {
                font-size: 16px;
                font-weight: bold;
            }
    </style>


    <script type="text/javascript">
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
                        // row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            //row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            //row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">

        debugger
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
                select: function (event, ui) { $("#<%=ddlClientId.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                  select: function (event, ui) { $("#<%=ddlcname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                  minLength: 4
              });
          }

          $(document).ready(function () {
              setProperty();
          });

          function OnAutoCompleteDDLClientidchange(event, ui) {
              $("#<%=ddlClientId.ClientID %>").trigger('change');
          }

          function OnAutoCompleteDDLClientnamechange(event, ui) {
              $("#<%=ddlcname.ClientID %>").trigger('change');
          }
    </script>
        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                       <%-- <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                        <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                        <li class="active"><a href="ArrearWages.aspx" style="z-index: 7;" class="active_bread">Arrear Wages</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Arrear Wages
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div class="dashboard_firsthalf" style="width: 100%">
                                        <table width="100%">
                                            <tr style="width: 30%">
                                                <td>Client ID</td>
                                                <td>
                                                    <%--<asp:DropDownList runat="server" class="sdrop" ID="ddlClientId" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged">
                                                    </asp:DropDownList>--%>
                                                    <asp:DropDownList ID="ddlClientId" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged" Width="120px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="padding-left:50px">Client Name</td>
                                                <td>
                                                    <%--<asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="true" class="sdrop"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                    </asp:DropDownList>--%>
                                                     <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" TabIndex="2" class="ddlautocomplete chosen-select"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" Style="width: 200px">
                                                    </asp:DropDownList>
                                                </td>
                                                </tr>
                                            <tr>
                                                <td>Month</td>
                                                <td>
                                                    <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput" AutoComplete="off" OnTextChanged="txtmonth_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td style="padding-left:55px">
                                                    <asp:Label ID="labelDays" runat="server" Text=" No of Days"></asp:Label>

                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNoofdays" runat="server" class="sinput" Width="50px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBNoofDays" runat="server" Enabled="True" TargetControlID="txtNoofdays"
                                                        ValidChars="0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                                </td>
                                            </tr>
                                            <tr style="width: 100%">
                                                <td colspan="6">
                                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <br />
                                    <br />

                                    <div class="rounded_corners" style="overflow: auto; margin-top: 20px; width: 99%">
                                        <asp:Panel ID="PnlNonGeneratedPaysheet" runat="server"
                                            Visible="false">
                                            <div style="border: 1px solid #A1DCF2; margin-left: 13px; width: 98%; text-align: center; width: 94%; padding: 15px">
                                                <asp:Label ID="lblPaysheetGeneratedTime" runat="server" Text="Label"></asp:Label>
                                            </div>
                                        </asp:Panel>

                                    </div>

                                    <br />
                                    <br />

                                    <div style="float: right; margin-bottom: 10px">
                                        <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" Visible="false" Style="margin-top: 10px" />
                                        <asp:Button ID="btnGeneratePayment" runat="server" Text="Generate Payment" OnClick="btnGeneratePayment_Click" Visible="false" OnClientClick='return confirm("Are you sure you want to generate payment?");' Style="margin-top: 10px" />
                                    </div>
                                    <%-- style="overflow-x:scroll"--%>
                                    <div class="rounded_corners">
                                        <div style="width: auto">
                                            <asp:GridView ID="GVListEmployeess" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                                Height="50px" CellPadding="5" CellSpacing="2" ForeColor="#333333" GridLines="None">

                                                <Columns>

                                                    <%--  0--%>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--  1--%>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkindividual" runat="server"  />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <%--  2--%>
                                                    <asp:TemplateField HeaderText="Emp ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-Height="50px">
                                                        <HeaderStyle />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpid" runat="server" Text='<%#Bind("empid") %>' Width="80px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <%--  3--%>
                                                    <asp:TemplateField HeaderText="Emp Name" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpname" runat="server" Text='<%#Bind("Empname") %>' Width="120px"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <%--  4--%>
                                                    <asp:TemplateField HeaderText="Desgn" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesgn" runat="server" Text='<%#Bind("Design") %>' Width="120px"> </asp:Label>
                                                            <asp:Label ID="lbldesignid" runat="server" Style="text-align: left" Text='<%#Bind("designations") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--  5--%>
                                                    <asp:TemplateField HeaderText="No.of Days" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblnodays" runat="server" Text='<%#Bind("NoOfDuties") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalNoDays"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  6--%>
                                                    <asp:TemplateField HeaderText="OTs" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblots" runat="server" Text='<%#Bind("ots") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalOTs"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  7--%>
                                                    <asp:TemplateField HeaderText="WOs" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWOs" runat="server" Text='<%#Bind("WO") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalWOs"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  8--%>
                                                    <asp:TemplateField HeaderText="NHS" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblnhs" runat="server" Text='<%#Bind("NHS") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalNHs"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  9--%>
                                                    <asp:TemplateField HeaderText="Spl Duties" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsplduties" runat="server" Text='<%#Bind("npots") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalsplduties"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  10--%>
                                                    <asp:TemplateField HeaderText="Total days" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltldays" runat="server" Text='<%#Bind("totaldays") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotaltotaldays"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>


                                                    <%--  11--%>
                                                    <asp:TemplateField HeaderText="Salary Rate" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsrate" runat="server" Text='<%#Bind("TempGross") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalSrate"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  12--%>
                                                    <asp:TemplateField HeaderText="Basic" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblbasic" runat="server" Text='<%#Bind("basic") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotabasic"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  13--%>
                                                    <asp:TemplateField HeaderText="New Basic">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtnewbasics" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("BasicSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                    TargetControlID="txtnewbasics" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalnewbasic"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  14--%>
                                                    <asp:TemplateField HeaderText="Arrear Basic">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearBasic" runat="server" Style="text-align: center;" Width="70px" Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearBasic" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalArrearBasic"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  15--%>
                                                    <asp:TemplateField HeaderText="DA" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDAs" runat="server" Text='<%#Bind("da") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalDA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>


                                                    <%--  16--%>
                                                    <asp:TemplateField HeaderText="New DA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtnewdas" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("dass")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                    TargetControlID="txtnewdas" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalnewda"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  17--%>
                                                    <asp:TemplateField HeaderText="Arrear DA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArreardas" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender71" runat="server" Enabled="True"
                                                                    TargetControlID="txtArreardas" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearda"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  18--%>
                                                    <asp:TemplateField HeaderText="HRA" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblhra" runat="server" Text='<%#Bind("hra") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalhra"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  19--%>
                                                    <asp:TemplateField HeaderText="New HRA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewHRA" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("hrass")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender711" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewHRA" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewHRA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  20--%>
                                                    <asp:TemplateField HeaderText="Arrear HRA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearHRA" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender721" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearHRA" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearHRA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  21--%>
                                                    <asp:TemplateField HeaderText="CCA" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcca" runat="server" Text='<%#Bind("cca") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalcca"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  22--%>
                                                    <asp:TemplateField HeaderText="New CCA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtnewCCA" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("ccass")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender70" runat="server" Enabled="True"
                                                                    TargetControlID="txtnewCCA" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalnewCCA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  23--%>
                                                    <asp:TemplateField HeaderText="Arrear CCA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearCCA" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender704" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearCCA" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearCCA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  24--%>
                                                    <asp:TemplateField HeaderText="Conv" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblConv" runat="server" Text='<%#Bind("Conveyance") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalConv"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  25--%>
                                                    <asp:TemplateField HeaderText="New Conv">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewConv" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("ConveyanceSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewConv" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewConv" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewConv"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>


                                                    <%--  26--%>
                                                    <asp:TemplateField HeaderText="Arrear Conv">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearConv" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderConv" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearConv" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearConv"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  27--%>
                                                    <asp:TemplateField HeaderText="WA" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWA" runat="server" Text='<%#Bind("WashAllowance") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalWA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  28--%>
                                                    <asp:TemplateField HeaderText="New WA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewWA" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("WASS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewCWA" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewWA" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewWA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>


                                                    <%--  29--%>
                                                    <asp:TemplateField HeaderText="Arrear WA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearWA" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderWA" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearWA" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearWA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  30--%>
                                                    <asp:TemplateField HeaderText="OA" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOA" runat="server" Text='<%#Bind("OtherAllowance") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalOA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  31--%>
                                                    <asp:TemplateField HeaderText="New OA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewOA" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("OASS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewCOA" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewOA" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewOA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  32--%>
                                                    <asp:TemplateField HeaderText="Arrear OA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearOA" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderOA" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearOA" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearOA"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  33--%>

                                                    <asp:TemplateField HeaderText="LA" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeaveEncashAmt" runat="server" Text='<%#Bind("LeaveEncashAmt") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalLeaveEncashAmt"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  34--%>

                                                    <asp:TemplateField HeaderText="New LA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewLeaveEncashAmt" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("leaveamountSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewCLeaveEncashAmt" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewLeaveEncashAmt" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewLeaveEncashAmt"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  35--%>

                                                    <asp:TemplateField HeaderText="Arrear LA">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearLeaveEncashAmt" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderLeaveEncashAmt" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearLeaveEncashAmt" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearLeaveEncashAmt"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  36--%>


                                                    <asp:TemplateField HeaderText="Gratuity" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGratuity" runat="server" Text='<%#Bind("Gratuity") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalGratuity"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  37--%>

                                                    <asp:TemplateField HeaderText="New Gratuity">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewGratuity" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("GratuitySS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewCGratuity" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewGratuity" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewGratuity"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  38--%>

                                                    <asp:TemplateField HeaderText="Arrear Gratuity">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearGratuity" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderGratuity" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearGratuity" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearGratuity"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  39--%>

                                                    <asp:TemplateField HeaderText="NFHs" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNfhs" runat="server" Text='<%#Bind("Nfhs") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalNfhs"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  40--%>

                                                    <asp:TemplateField HeaderText="New NFHs">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewNfhs" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("NfhsSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewCNfhs" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewNfhs" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewNfhs"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  41--%>

                                                    <asp:TemplateField HeaderText="Arrear NFHs">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearNfhs" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNfhs" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearNfhs" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearNfhs"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  42--%>

                                                    <asp:TemplateField HeaderText="Bonus" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBonus" runat="server" Text='<%#Bind("bonus") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalBonus"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  43--%>

                                                    <asp:TemplateField HeaderText="New Bonus">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewBonus" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("bonusSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewBonus" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewBonus" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewBonus"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  44--%>

                                                    <asp:TemplateField HeaderText="Arrear Bonus">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearBonus" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendeBonus" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearBonus" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearBonus"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%-- 45--%>


                                                    <asp:TemplateField HeaderText="Att Bonus" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAttBonus" runat="server" Text='<%#Bind("attbonus") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalAttBonus"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  46--%>

                                                    <asp:TemplateField HeaderText="New Att Bonus">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewAttBonus" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("AttbonusSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewAttBonus" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewAttBonus" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewAttBonus"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  47--%>

                                                    <asp:TemplateField HeaderText="Arrear Att Bonus">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearattBonus" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendeattBonus" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearattBonus" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearattBonus"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  48--%>

                                                    <asp:TemplateField HeaderText="Spl Allw" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSplAllw" runat="server" Text='<%#Bind("SplAllowance") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalSplAllw"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  49--%>

                                                    <asp:TemplateField HeaderText="New Spl Allw">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewSplAllw" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("splSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewSplAllw" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewSplAllw" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewSplAllw"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  50--%>

                                                    <asp:TemplateField HeaderText="Arrear Spl Allw">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearSplAllw" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderSplALl" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearSplAllw" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearSplAllw"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  51--%>

                                                    <asp:TemplateField HeaderText="Prof Allw" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProfAllw" runat="server" Text='<%#Bind("Profallowance") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalProfAllw"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  52--%>

                                                    <asp:TemplateField HeaderText="New Prof Allw">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewProfAllw" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("ProfAllowanceSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewProfAllw" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewProfAllw" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewProfAllw"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  53--%>
                                                    <asp:TemplateField HeaderText="Arrear Prof Allw">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearProfAllw" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderProfAllw" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearProfAllw" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearProfAllw"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  54--%>

                                                    <asp:TemplateField HeaderText="Uniform Allw" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUniformAllw" runat="server" Text='<%#Bind("UniformAllw") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalUniformAllw"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  55--%>

                                                    <asp:TemplateField HeaderText="New Uniform Allw">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewUniformAllw" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("UniformAllowanceSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewUniformAllw" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewUniformAllw" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewUniformAllw"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  56--%>

                                                    <asp:TemplateField HeaderText="Arrear Uniform Allw">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearUniformAllw" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderUniformALlw" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearUniformAllw" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearUniformAllw"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>


                                                   

                                                    <%--  57--%>

                                                    <asp:TemplateField HeaderText="RC" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRC" runat="server" Text='<%#Bind("RC") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalRC"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  58--%>

                                                    <asp:TemplateField HeaderText="New RC">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewRC" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("RCSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewRC" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewRC" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewRC"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  59--%>

                                                    <asp:TemplateField HeaderText="Arrear RC">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearRC" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderRC" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearRC" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearRC"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  60--%>

                                                    <asp:TemplateField HeaderText="CS" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCS" runat="server" Text='<%#Bind("CS") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalCS"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  61--%>

                                                    <asp:TemplateField HeaderText="New CS">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewCS" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("CSSS")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewCS" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewCS" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewCS"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  62--%>

                                                    <asp:TemplateField HeaderText="Arrear CS">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearCS" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderCS" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearCS" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalArrearcs"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    

                                                    <%--  63--%>
                                                    <asp:TemplateField HeaderText="Arrear Gross">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearGross" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("Gross")%>' Enabled="false"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender81" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearGross" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalArrearGross"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  64--%>
                                                    <asp:TemplateField HeaderText="OT Amt" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOTAmt" runat="server" Text='<%#Bind("OTAmt") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalOTamt"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  65--%>

                                                    <asp:TemplateField HeaderText="New OT Amt">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewOTAmt" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("OTAmtss")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewOTAmt" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewOTAmt" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewOTAmt"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>



                                                    <%--  66--%>

                                                    <asp:TemplateField HeaderText="Arrear OT Amt" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                                        <HeaderStyle Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblArrearOTAmt" runat="server" Text="0" Width="120px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalArrearOTAmt"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>


                                                    <%--  67--%>

                                                    <asp:TemplateField HeaderText="NHs Amt" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNHsAmt" runat="server" Text='<%#Bind("NHsAmt") %>' Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalNHsamt"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  68--%>

                                                    <asp:TemplateField HeaderText="New NHs Amt">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtNewNHsAmt" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("NHsAmtss")%>'> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNewNHsAmt" runat="server" Enabled="True"
                                                                    TargetControlID="txtNewNHsAmt" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" Style="text-align: right" ID="lblTotalNewNHsAmt"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>


                                                    <%--  69--%>

                                                    <asp:TemplateField HeaderText="Arrear NHs Amt" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                                        <HeaderStyle Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblArrearNHsAmt" runat="server" Text="0" Width="120px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalArrearNHsAmt"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  70--%>

                                                    <asp:TemplateField HeaderText="Arrear PF">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearpf" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("pf")%>' Enabled="false"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender81pf" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearpf" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalArrearpf"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  71--%>
                                                    <asp:TemplateField HeaderText="Arrear ESI">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearesi" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("esi")%>' Enabled="false"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender91" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearesi" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalArrearesi"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>

                                                    <%--  72--%>

                                                    <asp:TemplateField HeaderText="Arrear PT">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearPT" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0" Enabled="false"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPT" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearPT" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalArrearPT"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  73--%>
                                                    <asp:TemplateField HeaderText="Arrear Total Ded">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtArrearTotalDed" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text="0" Enabled="false"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9e" runat="server" Enabled="True"
                                                                    TargetControlID="txtArrearTotalDed" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalDeductions"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--  74--%>

                                                    <asp:TemplateField HeaderText="Arrear Net" ItemStyle-HorizontalAlign="Center">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblArrearNet" runat="server" Text="0" Enabled="false" Width="70px"> </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label runat="server" ID="lblTotalArrearNet"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="PF Deduct" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPFDeduct" runat="server" Text='<%#Bind("EmpPFDeduct") %>' Enabled="false"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="ESI Deduct" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblESIDeduct" runat="server" Text='<%#Bind("EmpESIDeduct") %>' Enabled="false"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Gross" Visible="false">
                                                        <ItemTemplate>
                                                            <div style="text-align: center;">
                                                                <asp:TextBox ID="txtPaysheetGross" runat="server" Style="text-align: center;" Width="70px"
                                                                    Text='<%#Bind("Gross")%>' Enabled="false"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FTBPaySheetGross" runat="server" Enabled="True"
                                                                    TargetControlID="txtPaysheetGross" ValidChars="-0123456789.">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </div>
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
                </div>
            </div>
            <!-- DASHBOARD CONTENT END -->

             <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
            <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    
    <script src="script/gridviewScroll.min.js" type="text/javascript"></script>
            <style>
            .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
        }
                </style>

    <script type="text/javascript">
          $(document).ready(function () {
              gridviewScroll();
          });

          function gridviewScroll() {
              $('#<%=GVListEmployeess.ClientID%>').gridviewScroll({
                width: 945,
                height: 500,
                freezesize: 5
            });
        }
    </script>
   </asp:content>

