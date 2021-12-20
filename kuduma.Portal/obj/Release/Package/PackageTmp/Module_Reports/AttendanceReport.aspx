<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="AttendanceReport.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.AttendanceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
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
            width: 160px;
        }

        .auto-style1 {
            width: 70px;
        }

        .auto-style2 {
            text-align: right;
        }

        .auto-style3 {
            width: 78px;
        }

        .auto-style4 {
            width: 96px;
        }

        .auto-style5 {
            width: 68px;
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
                select: function (event, ui) { $("#<%=ddlclientid.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlcname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlclientid.ClientID %>").trigger('change');
        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {
            $("#<%=ddlcname.ClientID %>").trigger('change');
        }
    </script>
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

    </script>

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <%-- <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="AttendanceReport.aspx" style="z-index: 7;" class="active_bread">Attendance Report</a></li>
                </ul>
            </div>
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Attendance Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <div align="left">
                                        <table>
                                            <tr>
                                                <td>Mode : 
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbclient" runat="server" GroupName="Data" Checked="true" Text="ClientWise" AutoPostBack="true" OnCheckedChanged="rbclient_CheckedChanged" />
                                                    &nbsp
                                                    <asp:RadioButton ID="rbexcel" runat="server" GroupName="Data" Text="ExcelWise" AutoPostBack="true" OnCheckedChanged="rbclient_CheckedChanged" />
                                                </td>

                                            </tr>
                                        </table>
                                    </div>
                                    <div class="dashboard_firsthalf" style="width: 100%">
                                        <div align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btndownloadpdf_Click">Export to PDF</asp:LinkButton>
                                                        &nbsp; &nbsp;
                                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="dashboard_firsthalf" style="width: 100%">
                                            <div align="left">
                                                <table width="100%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td class="auto-style1">
                                                            <asp:Label ID="lblmonth" runat="server" Text="Month :"></asp:Label></td>

                                                        <td class="auto-style3">
                                                            <asp:TextBox ID="txtmonth" runat="server" AutoComplete="off" cssclass="form-control" AutoPostBack="true" OnTextChanged="txtmonth_TextChanged"  Width="150px"> </asp:TextBox>
                                                            <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                        <td class="auto-style5">
                                                            <asp:Label ID="lblexcelno" runat="server" Text="ExcelNo :  " Visible="false"></asp:Label></td>
                                                        <td class="auto-style4">
                                                            <asp:DropDownList ID="ddlexcelno" runat="server" Visible="false" cssclass="form-control" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlexcelno_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltime" runat="server" Text="Time :  " Visible="false"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txttime" runat="server" Visible="false" cssclass="form-control" Width="150px" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <table width="100%" cellpadding="5" cellspacing="5">
                                                <tr>

                                                    <td>
                                                        <asp:Label ID="lblclinetid" runat="server" Text="Clientid" ></asp:Label>
                                                    </td>

                                                    <td>

                                                        <asp:DropDownList ID="ddlclientid" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True" 
                                                            OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" Width="80px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblcname" runat="server" Text="Name" ></asp:Label></td>

                                                    <td>
                                                        <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" TabIndex="1" class="ddlautocomplete chosen-select" 
                                                            OnSelectedIndexChanged="ddlclientname_SelectedIndexChanged" Style="width: 170px">
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td>
                                                        <asp:CheckBox ID="chksummary" runat="server" Text="Summary" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="rounded_corners">
                                            <div style="overflow: scroll; width: 100%">
                                                <asp:GridView ID="GVAttendance" runat="server" AutoGenerateColumns="True" Width="970px" CssClass="table table-striped table-bordered table-condensed table-hover"
                                                    CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="none" Style="margin-left: -2px" OnRowDataBound="GVAttendance_RowDataBound">
                                                    <Columns>
                                                    </Columns>
                                                </asp:GridView>

                                                <asp:Label ID="LblResult" runat="server" Text="" Style="color: red"></asp:Label>
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

                <!-- CONTENT AREA END -->
            </div>
</asp:Content>
