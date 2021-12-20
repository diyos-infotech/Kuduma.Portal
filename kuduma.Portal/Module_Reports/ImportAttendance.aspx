<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ImportAttendance.aspx.cs" Inherits="Kuduma.Portal.ImportAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>

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
                                <asp:LinkButton ID="lnkempnameImportfromexcel" Text="Export Sample Excel(Name)" Visible="false" runat="server"
                                    OnClick="lnkempnameImportfromexcel_Click"></asp:LinkButton>

                                <asp:LinkButton ID="lnkImportfromexcel" Text="Export Sample Excel" runat="server"
                                    OnClick="lnkempnameImportfromexcel_Click"></asp:LinkButton>
                            </h4>
                        </div>
                        <div class="boxhead">
                            <h2 style="text-align: center">Employee Attendance&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </h2>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px; height: auto">
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
                                        <%--<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>--%>
                                        <td class="style1">
                                            <asp:Label ID="lblclientname" runat="server" Text="Client Name" Visible="false"></asp:Label>
                                        </td>

                                        <td>
                                            <asp:DropDownList ID="ddlCName" runat="server" class="ddlautocomplete chosen-select" AutoPostBack="True"
                                                Width="125px" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged" Visible="false">
                                            </asp:DropDownList>

                                        </td>
                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown"></cc1:CalendarExtender>

                                        </td>

                                        <%--<td>&nbsp;</td>--%>

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

                                            <asp:Button ID="btnClearAll" runat="server" Text="Clear All" class=" btn save" Visible="false"
                                                OnClick="btnClearAll_Click" />

                                        </td>
                                        <td>
                                            <asp:Button ID="btnExport" runat="server" Text="Unsaved" class=" btn save"
                                                OnClick="btnExport_Click" Visible="false" />
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="width: 60px">
                                            <%-- Contract Id--%></td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlContractId" Width="125px" Visible="false">
                                            </asp:DropDownList>

                                        </td>
                                        <td>&nbsp;</td>

                                        <td class="style1">
                                            <%-- OT in terms of--%></td>

                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlOTType" Width="125px" Visible="false">
                                                <asp:ListItem>Days</asp:ListItem>
                                                <asp:ListItem>Hours</asp:ListItem>
                                            </asp:DropDownList>

                                        </td>


                                        <td>&nbsp;</td>

                                        <td style="width: 60px">&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>

                                        &nbsp;</td>
                                                
                                    </tr>

                                </table>

                            </div>

                            <br />
                            <div>
                                <asp:GridView ID="gvAttendancestatus" runat="server"
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                                    GridLines="Both" HeaderStyle-CssClass="HeaderStyle" Height="140px"
                                    OnRowDataBound="gvAttendancestatus_RowDataBound" ShowFooter="True"
                                    Style="margin-left: 50px" Width="90%">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsno" runat="server" Text="<%#Container.DataItemIndex+1%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesign" runat="server" Text='<%#Bind("Design") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Duties">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotDuties" runat="server" Text='<%#Bind("Duties")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotDuties" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OTs">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotOts" runat="server" Text='<%#Bind("ot") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotOts" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Duties">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text='<%#Bind("TotalDuties") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>




                                    </Columns>
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                            </div>
                            <br />
                            <asp:Label ID="lblMessage" runat="server" Style="color: Red"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                     
                                      <asp:Button ID="btnExportExcel" runat="server" Text="Export Excel"
                                          OnClick="btnExportExcel_Click" Visible="false" class="btn Save" />
                            <br />



                            <%-- <div style="overflow:scroll">--%>
                            <asp:GridView ID="GridView1" runat="server" Width="100%" Visible="false"
                                AutoGenerateColumns="False" CellPadding="2" CellSpacing="2"
                                ForeColor="#333333" BorderStyle="Solid"
                                BorderColor="Black" BorderWidth="0" GridLines="Both" ShowFooter="true"
                                HeaderStyle-CssClass="HeaderStyle"
                                OnRowDataBound="GridView1_RowDataBound">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpid" runat="server" Text=" <%#Bind('EmpId')%>" Style="text-align: left" Width="50px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblName" Width="200px" Style="text-align: left" Text=" <%#Bind('FullName')%>"></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDesg" Style="text-align: left" Text=" <%#Bind('Designation')%>" Width="200px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            Duties
                                            <br />
                                            OTs
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--************************************************************************************************************************************--%>
                                    <asp:TemplateField HeaderText="1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="txtday1" runat="server" Style="text-align: center" Width="20px" Text=" <%#Bind('day1')%>"></asp:Label>
                                            <br />
                                            <asp:Label ID="txtday1ot" runat="server" Style="text-align: center" Width="20px" Text=" <%#Bind('day1ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday1"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="2" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday2" Style="text-align: center" Width="20px" Text=" <%#Bind('day2')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday2ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day2ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday2"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="3" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday3" Style="text-align: center" Width="20px" Text=" <%#Bind('day3')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday3ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day3ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday3"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="4" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday4" Style="text-align: center" Width="20px" Text=" <%#Bind('day4')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday4ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day4ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday4"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="5" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday5" Style="text-align: center" Width="20px" Text=" <%#Bind('day5')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday5ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day5ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday5"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="6" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday6" Style="text-align: center" Width="20px" Text=" <%#Bind('day6')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday6ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day6ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday6"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="7" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday7" Style="text-align: center" Width="20px" Text=" <%#Bind('day7')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday7ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day7ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday7"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="8" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday8" Style="text-align: center" Width="20px" Text=" <%#Bind('day8')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday8ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day8ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday8"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="9" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday9" Style="text-align: center" Width="20px" Text=" <%#Bind('day9')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday9ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day9ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday9"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="10" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday10" Style="text-align: center" Width="20px" Text=" <%#Bind('day10')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday10ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day10ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday10"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="11" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday11" Style="text-align: center" Width="20px" Text=" <%#Bind('day11')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday11ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day11ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday11"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="12" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday12" Style="text-align: center" Width="20px" Text=" <%#Bind('day12')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday12ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day12ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday12"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="13" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday13" Style="text-align: center" Width="20px" Text=" <%#Bind('day13')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday13ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day13ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday13"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="14" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday14" Style="text-align: center" Width="20px" Text=" <%#Bind('day14')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday14ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day14ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday14"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="15" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday15" Style="text-align: center" Width="20px" Text=" <%#Bind('day15')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday15ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day15ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday15"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="16" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday16" Style="text-align: center" Width="20px" Text=" <%#Bind('day16')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday16ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day16ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday16"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="17" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday17" Style="text-align: center" Width="20px" Text=" <%#Bind('day17')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday17ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day17ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday17"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="18" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday18" Style="text-align: center" Width="20px" Text=" <%#Bind('day18')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday18ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day18ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday18"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="19" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday19" Style="text-align: center" Width="20px" Text=" <%#Bind('day19')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday19ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day19ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday19"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="20" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday20" Style="text-align: center" Width="20px" Text=" <%#Bind('day20')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday20ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day20ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday20"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="21" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday21" Style="text-align: center" Width="20px" Text=" <%#Bind('day21')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday21ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day21ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday21"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="22" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday22" Style="text-align: center" Width="20px" Text=" <%#Bind('day22')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday22ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day22ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday22"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="23" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday23" Style="text-align: center" Width="20px" Text=" <%#Bind('day23')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday23ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day23ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday23"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="24" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday24" Style="text-align: center" Width="20px" Text=" <%#Bind('day24')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday24ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day24ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday24"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="25" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday25" Style="text-align: center" Width="20px" Text=" <%#Bind('day25')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday25ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day25ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday25"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="26" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday26" Style="text-align: center" Width="20px" Text=" <%#Bind('day26')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday26ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day26ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday26"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="27" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday27" Style="text-align: center" Width="20px" Text=" <%#Bind('day27')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday27ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day27ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday27"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="28" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday28" Style="text-align: center" Width="20px" Text=" <%#Bind('day28')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday28ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day28ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday28"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="29" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday29" Style="text-align: center" Width="20px" Text=" <%#Bind('day29')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday29ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day29ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday29"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="30" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday30" Style="text-align: center" Width="20px" Text=" <%#Bind('day30')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday30ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day30ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday30"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="31" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtday31" Style="text-align: center" Width="20px" Text=" <%#Bind('day31')%>"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="txtday31ot" Style="text-align: center" Width="20px" Text=" <%#Bind('day31ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday31"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%--************************************************************************************************************************************--%>


                                    <asp:TemplateField HeaderText="Duties" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtDuties" Style="text-align: center" Width="5px" Text=" <%#Bind('noofduties')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalDuties" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField HeaderText="WOs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtWos" style="text-align:center" Width="5px" Text=" <%#Bind('wo')%>"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                            <asp:Label ID="lblTotalWOs" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="NHS" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtNHS" style="text-align:center" Width="5px" Text=" <%#Bind('Wo')%>"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                            <asp:Label ID="lblTotalNHS" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="OTs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="txtOTs" Style="text-align: center" Width="5px" Text=" <%#Bind('ot')%>"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalOTs" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField HeaderText="OTs1" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtOTs1" style="text-align:center" Width="5px" Text=" <%#Bind('ots1')%>"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                            <asp:Label ID="lblTotalOTs1" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Total" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalDts" runat="server" Text=" <%#Bind('Total')%>"></asp:Label>
                                            <%--  <br />
                                      <asp:Label ID="lblTotalOts" runat="server"  Text=" <%#Bind('ot')%>"></asp:Label>--%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField HeaderText="Canteen Adv." FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtCanAdv" style="text-align:center" Width="5px" Text=" <%#Bind('CanteenAdv')%>"></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                            <asp:Label ID="lblTotalCanteenAdv" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Penalty" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtPenalty" style="text-align:center" Width="5px" Text=" <%#Bind('Penalty')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            <asp:Label ID="lblTotalPenalty" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                       
                                       
                                        <asp:TemplateField HeaderText="Incentive" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtIncentivs" style="text-align:center" Width="5px" Text=" <%#Bind('Incentivs')%>"></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                            <asp:Label ID="lblTotalIncentives" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                    
                                    
                                     <asp:TemplateField HeaderText="NA" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtNa" style="text-align:center" Width="5px" Text=" <%#Bind('na')%>"></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                            <asp:Label ID="lblTotalNa" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="AB" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtAb" style="text-align:center" Width="5px" Text=" <%#Bind('ab')%>"></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                            <asp:Label ID="lblTotalAb" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>--%>
                                </Columns>

                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <%-- </div>--%>


                            <div>
                                <asp:GridView ID="GridView2" runat="server" Height="140px" Width="90%" Style="margin-left: 50px"
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="1px" GridLines="None" HeaderStyle-CssClass="HeaderStyle" Visible="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText=" ClientId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientId" runat="server" Text=" <%#Bind('ClientId')%>" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText=" Emp Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpid1" runat="server" Text=" <%#Bind('EmpId')%>" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText=" Emp Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpid1" runat="server" Text=" <%#Bind('EMpName')%>" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDesg1" Text=" <%#Bind('DesignID')%>" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation ID">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDesg1" Text=" <%#Bind('Design')%>" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Duties">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDuties1" Text=" <%#Bind('NoOfDuties')%>" Width="90px"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OTs">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblOts1" Text=" <%#Bind('ot')%>" Width="90px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Created_On">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblCreatedOn" Text=" <%#Bind('CreatedOn')%>" Width="90px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ExcelNo">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblExcelNo" Text=" <%#Bind('ExcelNo')%>" Width="90px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblRemarks" Text=" <%#Bind('Remark')%>" Width="90px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px"></ItemStyle>
                                        </asp:TemplateField>


                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div>

                                <asp:GridView ID="SampleGrid" runat="server" Width="100%"
                                    AutoGenerateColumns="False" CellPadding="2" CellSpacing="2"
                                    ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="0" GridLines="None" Visible="false"
                                    HeaderStyle-CssClass="HeaderStyle">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsClientid" Width="200px" Text=" "></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsEmpid" runat="server" Text=" " Style="text-align: center" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsDesg" Text=" " Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                Duties
                                           <%-- <br />
                                                OTs--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--************************************************************************************************************************************--%>

                                        <%--Duties --%>
                                        <asp:TemplateField HeaderText="1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday1" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday1ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday2" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday2ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday3" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday3ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday4" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday4ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday5" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday5ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday6" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday6ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday7" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday7ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="8" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday8" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday8ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="9" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday9" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday9ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="10" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday10" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday10ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="11" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday11" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday11ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="12" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday12" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday12ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="13" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday13" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday13ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday14" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday14ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="15" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday15" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday15ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="16" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday16" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday16ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="17" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday17" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday17ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="18" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday18" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday18ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="19" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday19" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday19ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="20" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday20" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday20ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="21" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday21" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday21ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="22" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday22" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday22ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="23" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday23" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday23ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="24" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday24" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday24ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="25" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday25" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday25ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="26" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday26" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday26ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="27" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday27" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday27ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="28" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday28" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%--<br />
                                      <asp:Label  ID="txtsday28ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="29" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday29" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday29ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="30" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday30" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday30ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="31" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txtsday31" runat="server" Style="text-align: center" Width="20px" Text="0"></asp:Label>
                                                <%-- <br />
                                      <asp:Label  ID="txtsday31ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OTs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsOTs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="PL Days" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtpldays" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Canteen Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsCanAdv" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsPenalty" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Incentives" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsIncentivs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OT Hrs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsOTHrs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Uniform Ded" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtuniformded" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Other Ded" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtatmded" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Arrears" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtArrears" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Attendance Bonus" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtAttendanceBonus" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Stop Payment" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtStopPayment" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>

                                <asp:GridView ID="grvSample2" runat="server" Width="100%"
                                    AutoGenerateColumns="False" CellPadding="2" CellSpacing="2"
                                    ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="0" GridLines="None" Visible="false"
                                    HeaderStyle-CssClass="HeaderStyle">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsClientid" Width="200px" Text=" "></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsEmpid" runat="server" Text=" " Style="text-align: center" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsDesg" Text=" " Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Duties" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsDuties" Style="text-align: center" Width="5px" Text=" <%#Bind('day1')%>"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OTs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsOTs" Style="text-align: center" Width="5px" Text=" <%#Bind('day2')%>"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="WOs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsWos" Style="text-align: center" Width="5px" Text=" <%#Bind('day3')%>"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="NHs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsNhs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="PL Days" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtspldays" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Canteen Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsCanAdv" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsPenalty" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Incentives" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsIncentivs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OT Hrs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsOTHrs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Uniform Ded" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtuniformded" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Other Ded" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtatmded" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Arrears" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtArrears" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Attendance Bonus" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtAttendanceBonus" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Stop Payment" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtStopPayment" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>

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
                                <%-- <asp:GridView ID="GridView3" runat="server" Width="100%"
                                    AutoGenerateColumns="False" CellPadding="2" CellSpacing="2"
                                    ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="0" GridLines="None" Visible="false"
                                    HeaderStyle-CssClass="HeaderStyle">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsClientid" Width="200px" Text=" "></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsEmpid" runat="server" Text=" " Style="text-align: center" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Emp Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsempname" Text=" " Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsDesg" Text=" " Width="200px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Duties" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsDuties" Style="text-align: center" Width="5px" Text=" <%#Bind('day1')%>"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OTs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsOTs" Style="text-align: center" Width="5px" Text=" <%#Bind('day2')%>"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="WOs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsWos" Style="text-align: center" Width="5px" Text=" <%#Bind('day3')%>"></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="NHs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsNhs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="PL Days" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtpldays" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Canteen Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsCanAdv" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsPenalty" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>



                                        <asp:TemplateField HeaderText="Incentives" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsIncentivs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OT Hrs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsOTHrs" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Uniform Ded" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtuniformded" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Other Ded" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtatmded" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Arrears" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtArrears" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Attendance Bonus" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtAttendanceBonus" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Stop Payment" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtStopPayment" Style="text-align: center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>--%>

                                <div class="rounded_corners" style="overflow: scroll">
                                    <asp:GridView ID="GVEmployeeList" runat="server" Width="80%" Style="margin: 0px auto" Visible="false"
                                        AutoGenerateColumns="False" CellPadding="5" CellSpacing="5" ForeColor="#333333" GridLines="None" CssClass="table table-striped table-bordered table-condensed table-hover">

                                        <Columns>

                                            <asp:TemplateField HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClientid" runat="server" Text='<%#Bind("clientid")%>' Style="text-align: center" Width="100px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Middle" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpid" runat="server" Text='<%#Bind("empid")%>' Style="text-align: center" Width="120px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Employee Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblEmployeeName" Text='<%#Bind("fullname")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDesg" Text='<%#Bind("Design")%>' Width="200px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Duties">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDuties" runat="server" Text='<%#Bind("NoOfDuties")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OTs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblots" runat="server" Text='<%#Bind("ot")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NHs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnhs" runat="server" Text='<%#Bind("nhs")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Wos">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWos" runat="server" Text='<%#Bind("Wo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="OT Hrs">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblothrs" runat="server" Text='<%#Bind("OTHours")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PL Days">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpldays" runat="server" Text='<%#Bind("pldays")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Uniform Ded">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluniformded" runat="server" Text='<%#Bind("UniformDed")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Other Ded">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblatmded" runat="server" Text='<%#Bind("OtherDed")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Canteen Advance">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCanteenAdv" runat="server" Text='<%#Bind("CanteenAdv")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Penalty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPenalty" runat="server" Text='<%#Bind("Penalty")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Incentives">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIncentives" runat="server" Text='<%#Bind("Incentives")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Arrears">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblArrears" runat="server" Text='<%#Bind("Arrears")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Attendance Bonus">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttBonus" runat="server" Text='<%#Bind("AttBonus")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>


                                    </asp:GridView>
                                </div>

                                <table width="100%">
                                    <tr>
                                        <td width="25%"></td>
                                        <td width="25%">
                                            <asp:Label ID="lblTotalDuties" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td width="25%">
                                            <asp:Label ID="lblTotalOts" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td width="25%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lbltotaldesignationlist" runat="server" Text=""> </asp:Label>
                                        </td>

                                    </tr>
                                </table>

                                <br />
                                <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"></asp:Label>



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

