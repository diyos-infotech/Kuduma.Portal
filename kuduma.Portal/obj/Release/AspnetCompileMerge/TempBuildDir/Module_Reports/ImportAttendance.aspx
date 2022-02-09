<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ImportAttendance.aspx.cs" Inherits="Kuduma.Portal.ImportAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>


    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        #social div {
            display: block;
        }

        .HeaderStyle {
            text-align: Left;
        }

        .style1 {
            width: 106px;
            padding-left: 40px;
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
            width: 130px;
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
                .replace(/'/g, '&#39;')
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
            <h1 class="dashboard_heading"></h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                        <div>
                            <h4 style="text-align: right">


                                <asp:LinkButton ID="lnkImportfromexcel" Text="Export Sample Excel" runat="server"
                                    OnClick="lnkempnameImportfromexcel_Click"></asp:LinkButton>
                            </h4>
                        </div>
                        <div class="boxhead">
                            <h2 style="text-align: center">Employee Attendance</h2>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px; min-height: 600px">
                            <!--  Content to be add here> -->

                            <div>

                                <table style="width: 100%">
                                    <tr>
                                        <td>Option</td>

                                        <td>
                                            <asp:DropDownList runat="server" ID="ddloption" Width="125px" OnSelectedIndexChanged="ddloption_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem>Month Wise</asp:ListItem>
                                                <asp:ListItem>Client Wise</asp:ListItem>
                                            </asp:DropDownList>

                                        </td>
                                        <%--<td></td>--%>
                                        <td class="style1">Type</td>

                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlempidtype" Width="125px">
                                                <asp:ListItem>Software Emp ID</asp:ListItem>
                                                <asp:ListItem>Old EmpID</asp:ListItem>
                                            </asp:DropDownList>

                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:Label ID="lblexcelno" runat="server" Text="Excel No"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="ddlExcelNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExcelNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 60px">
                                            <asp:Label ID="lblclientid" runat="server" Text="Client ID" Visible="false"></asp:Label><span style="color: Red"></span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlClientID" runat="server" Width="120px" TabIndex="2" class="ddlautocomplete chosen-select" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged" Visible="false">
                                            </asp:DropDownList>
                                        </td>
                                        
                                        <td class="style1">
                                            <asp:Label ID="lblclientname" runat="server" Text="Client Name" Visible="false"></asp:Label>
                                        </td>

                                        <td>
                                            <asp:DropDownList ID="ddlCName" runat="server" class="ddlautocomplete chosen-select" AutoPostBack="True"
                                                Width="125px" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged" Visible="false">
                                            </asp:DropDownList>

                                        </td>
                                        <td>
                                        </td>

                                        <td style="width: 60px">

                                            <asp:Button ID="btnClear" runat="server" Text="Clear" class=" btn save" Visible="false"
                                                OnClick="btnClear_Click" />

                                        </td>

                                    </tr>

                                    <tr>

                                        <td style="width: 60px">Month</td>
                                        <td>

                                            <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput" Width="100px" AutoPostBack="True" OnTextChanged="txtmonth_TextChanged"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                            </cc1:CalendarExtender>

                                        </td>

                                     
                                        <td class="style1">Attendance Mode</td>

                                        <td style="padding-top: 10px">
                                            <asp:DropDownList runat="server" ID="ddlAttendanceMode" Width="125px">
                                                <asp:ListItem>Full Attendance</asp:ListItem>
                                                <asp:ListItem>Individual Attendance</asp:ListItem>
                                            </asp:DropDownList>&nbsp;</td>

                                        <td>&nbsp;</td>

                                        <td style="width: 60px">Select File: </td>
                                        <td>
                                            <asp:FileUpload ID="fileupload1" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnImport" runat="server" Text="Import Data" class=" btn save" OnClick="btnImport_Click" />
                                        </td>


                                        <td>

                                          

                                        </td>
                                        <td>
                                           <asp:Button ID="btnExport" runat="server" Text="Unsaved" Style="background-color: #d9534f; border-color: #d43f3a;"
                                                OnClick="btnExport_Click" Visible="false" Width="98px" />
                                        </td>

                                    </tr>

                                </table>
                            </div>

                            <div>

                                <asp:GridView ID="GridView3" runat="server" Width="100%"
                                    AutoGenerateColumns="True" CellPadding="2" CellSpacing="2"
                                    ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="0" GridLines="None" Visible="false"
                                    HeaderStyle-CssClass="HeaderStyle">
                                    <RowStyle BackColor="#EFF3FB" />


                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>

                            </div>

                            <br />
                            <br />

                            <div class="panel panel-default" runat="server" id="pnlAttSummary" visible="false" style="font-size: 14px; font-family: Calibri; width: 900px; border-color: #1dacd6; border-width: 4px; margin: 0px auto; box-shadow: rgba(0, 0, 0, 0.25) 0px 14px 28px, rgba(0, 0, 0, 0.22) 0px 10px 10px;">
                                <div class="panel-heading" style="font-weight: bold; background-color: #1dacd6">Attendance Summary</div>
                                <div class="panel-body" style="overflow-x: scroll">

                                    <asp:GridView ID="gvattsummarydata" runat="server" AutoGenerateColumns="true" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover">
                                        <Columns></Columns>
                                    </asp:GridView>

                                </div>

                            </div>

                            <div class="panel panel-default" runat="server" id="pnlnotinsertdata" visible="false" style="font-size: 14px; position: relative; top: 20px; font-family: Calibri; width: 900px; border-color: #ff4040; border-width: 4px; margin: 0px auto; box-shadow: rgba(0, 0, 0, 0.25) 0px 14px 28px, rgba(0, 0, 0, 0.22) 0px 10px 10px;">
                                <div class="panel-heading" style="font-weight: bold; background-color: #ff4040">Unsaved data</div>
                                <div class="panel-body">

                                    <asp:GridView ID="gvnotinsert" runat="server" AutoGenerateColumns="true" ShowHeader="True" Style="background-color: white" CssClass="table table-striped table-bordered table-condensed table-hover">
                                        <Columns></Columns>
                                    </asp:GridView>

                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- DASHBOARD CONTENT END -->
                </div>
            </div>
            <!-- CONTENT AREA END -->

        </div>
    </div>

</asp:Content>

