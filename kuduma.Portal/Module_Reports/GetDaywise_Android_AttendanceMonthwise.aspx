<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="GetDaywise_Android_AttendanceMonthwise.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.GetDaywise_Android_AttendanceMonthwise" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">


    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <script src="../js/colResizable-1.6.js"></script>
    <script src="../js/colResizable-1.6.min.js"></script>



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
                select: function (event, ui) {
                    $("#<%=ddlClientID.ClientID %>").attr("data-clientId", ui.item.value);
                    OnAutoCompleteDDLClientidchange(event, ui);
                },
                select: function (event, ui) { $("#<%=ddlCName.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                //select: function (event, ui) { $("#ddlFOID").attr("data-clientId", ui.item.value); OnAutoCompleteDDLFoidchange(event, ui); },

                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();


            $("#<%=ddlClientID.ClientID %>").combobox().parent().find("input.ui-autocomplete-input").css('width', '90px')
            $("#<%=ddlCName.ClientID %>").combobox().parent().find("input.ui-autocomplete-input").css('width', '140px');

            //$('.ui-autocomplete-input').css('width', '300px')
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlClientID.ClientID %>").trigger('change');

        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {

            $("#<%=ddlCName.ClientID %>").trigger('change');
               }
               

               $(function () {
                   $('#<%=GvDayWiseAttendance.ClientID %>').colResizable({
                liveDrag: true,
                resizeMode: 'overflow',
                postbackSafe: true,
                gripInnerHtml: "<div class='grip'></div>",
                draggingClass: "dragging"
            });
        });

               function onCalendarHidden() {
                   var cal = $find("calendar1");

                   if (cal._monthsBody) {
                       for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                           var row = cal._monthsBody.rows[i];
                           for (var j = 0; j < row.cells.length; j++) {
                               Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                           }
                       }
                   }
               }

               function onCalendarShown() {

                   var cal = $find("calendar1");

                   cal._switchMode("months", true);

                   if (cal._monthsBody) {
                       for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                           var row = cal._monthsBody.rows[i];
                           for (var j = 0; j < row.cells.length; j++) {
                               Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
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
                           //cal._switchMonth(target.date);
                           cal._blur.post(true);
                           cal.raiseDateSelectionChanged();
                           break;
                   }
               }


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


                                    <table width="100%" cellpadding="5" cellspacing="5">

                                        <tr >
                                            <td >
                                                <asp:Label runat="server" ID="Label2" Text="Branch : "></asp:Label><span style="color: Red">*</span>
                                            </td>
                                            <td >
                                                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Width="100px" class="form-control">
                                                </asp:DropDownList>
                                            </td>

                                        
                                            <td >
                                                <asp:Label runat="server" ID="lblclientid" Text="Client ID"></asp:Label>
                                            </td>
                                            <td >
                                                <asp:DropDownList ID="ddlClientID" runat="server" CssClass="ddlautocomplete chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged"
                                                    >
                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <asp:Label runat="server" ID="lblclientname" Text="Name" style="margin-left:10px"></asp:Label>
                                            </td>
                                            <td >
                                                <asp:DropDownList ID="ddlCName" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                           <td >Month
                                            </td>
                                            <td >

                                                <asp:TextBox ID="txtmonth" runat="server" Text="" CssClass="form-control" Width="100px" AutoComplete="off" class="sinput"></asp:TextBox>
                                                <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                    Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown"></cc1:CalendarExtender>
                                            </td>

                                            <td>
                                                <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                            </td>

                                        </tr>

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
