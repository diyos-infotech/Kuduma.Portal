<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="GetDaywise_Android_Attendance_ICS.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.GetDaywise_Android_Attendance_ICS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">


    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <script src="../js/colResizable-1.6.js"></script>
    <script src="../js/colResizable-1.6.min.js"></script>



    <style type="text/css">
        td {
            max-width: 180px;
            white-space: nowrap;
            text-overflow: ellipsis;
            overflow: hidden;
        }

        #social div {
            display: block;
        }

        .HeaderStyle {
            text-align: Left;
        }

        .style3 {
            height: 24px;
        }

        .modalBackground {
            background-color: Gray;
            z-index: 10000;
        }

        .slidingDiv {
            background-color: #99CCFF;
            padding: 10px;
            margin-top: 10px;
            border-bottom: 5px solid #3399FF;
        }

        .show_hide {
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
        .auto-style1 {
            width: 77px;
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
                select: function (event, ui) { $("#<%=ddlClientID.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlCName.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                //select: function (event, ui) { $("#ddlFOID").attr("data-clientId", ui.item.value); OnAutoCompleteDDLFoidchange(event, ui); },

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
               //function OnAutoCompleteDDLFoidchange(event, ui) {

               //    $('#ddlFOID').trigger('change');
               //}

               $(function () {
                   $('#<%=GvDayWiseAttendance.ClientID %>').colResizable({
                liveDrag: true,
                resizeMode: 'overflow',
                postbackSafe: true,
                gripInnerHtml: "<div class='grip'></div>",
                draggingClass: "dragging"
            });
        });

    </script>

    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Android Attendance </a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Android Attendance 
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">

                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>



                                <div class="dashboard_firsthalf" style="width: 100%">

                                    <div style="margin-right: 10px; float: right">
                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False" OnClientClick="AssignExportHTML()">Export to Excel</asp:LinkButton>
                                    </div>

                                     <table cellspacing="5" cellpadding="5">
                                        <tr style="height: 32px" cellpadding="5">
                                            <td class="auto-style1">
                                                <asp:Label runat="server" ID="Label2" Text="Branch : "></asp:Label><span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Width="125px" class="form-control">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>

                                    <table width="100%" cellpadding="5" cellspacing="5">

                                        <tr style="height: 32px">
                                            <td>Option
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlOption" Width="125px" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddltypes_SelectedIndexChanged">
                                                    <asp:ListItem>Client Wise</asp:ListItem>
                                                    <asp:ListItem>FO Wise</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                            <td>Type
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddltypes" Width="125px" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="ddltypes_SelectedIndexChanged">
                                                    <asp:ListItem>Day Wise</asp:ListItem>
                                                    <asp:ListItem>Month Wise</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Month
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmonth" runat="server" Text="" class="form-control" Width="120px"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>

                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                            </td>

                                        </tr>


                                        <tr style="height: 32px">



                                            <td>
                                                <asp:Label runat="server" ID="lblclientid" Text="Client ID"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlClientID" runat="server" CssClass="ddlautocomplete chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged"
                                                    Width="120px">
                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <asp:Label runat="server" ID="lblclientname" Text="Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCName" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>
                                    </table>
                                    <table width="50%" cellpadding="5" style="margin-top: -30px" cellspacing="5">
                                        <tr>
                                            <td>
                                                <asp:Label Visible="false" runat="server" ID="lblFOId" Text="FO ID"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFOID" runat="server" Visible="false" CssClass="ddlautocomplete chosen-select"
                                                    Width="120px">
                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <asp:Label runat="server" ID="Label1" Visible="false" Text="Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList1" runat="server" Visible="false" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>


                                    </table>

                                    <table width="50%" cellpadding="5" style="margin-top: 40px" cellspacing="5">
                                    </table>
                                </div>

                                <div class="rounded_corners" style="overflow-x: scroll; width: 97%; margin-left: 17px; margin-bottom: 30px">
                                    <asp:GridView ID="GvDayWiseAttendance" runat="server" AutoGenerateColumns="True"
                                        Width="100%" CellPadding="4" CellSpacing="3" CssClass="table table-striped table-bordered table-condensed table-hover">
                                        <Columns>
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


