<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ClientForms.aspx.cs" Inherits="Kuduma.Portal.ClientForms" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
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

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                //row.style.backgroundColor = "aqua";
            }
            else {
                //If not checked change back to original color
                if (row.rowIndex % 2 == 0) {
                    //Alternating Row Color
                    //row.style.backgroundColor = "#C2D69B";
                }
                else {
                    //row.style.backgroundColor = "white";
                }
            }

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

    </script>
        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                       <%-- <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                        <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                        <li class="active"><a href="ClientForms.aspx" style="z-index: 7;" class="active_bread">Client Forms</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Client Forms
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">

                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>

                                    <div class="dashboard_firsthalf" style="width: 100%">

                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>Forms
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlForms" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlForms_SelectedIndexChanged" class="sdrop">
                                                        <asp:ListItem>--Select--</asp:ListItem>
                                                        <%-- <asp:ListItem>Form XVII</asp:ListItem>--%>
                                                        <asp:ListItem>Form XIV</asp:ListItem>
                                                        <asp:ListItem>Form XV</asp:ListItem>
                                                        <asp:ListItem>Form XX</asp:ListItem>
                                                        <%--   <asp:ListItem>ESI DECLARATION</asp:ListItem>
                                                        <asp:ListItem>PF DECLARATION</asp:ListItem>--%>
                                                        <asp:ListItem>Form XXI</asp:ListItem>
                                                        <asp:ListItem>Form XXII</asp:ListItem>
                                                        <asp:ListItem>Form XXIII</asp:ListItem>
                                                        <asp:ListItem>Form XXV</asp:ListItem>
                                                        <%--  <asp:ListItem>Form C</asp:ListItem>
                                                        <asp:ListItem>Form</asp:ListItem>--%>
                                                        <asp:ListItem>Form XIII</asp:ListItem>
                                                        <asp:ListItem>Form Q</asp:ListItem>
                                                        <asp:ListItem>Form F</asp:ListItem>
                                                        <asp:ListItem>Form D</asp:ListItem>
                                                        <asp:ListItem>Form T(ATTENDANCE)</asp:ListItem>
                                                        <asp:ListItem>Form T(PAYSHEET)</asp:ListItem>
                                                        <asp:ListItem>Form 22(ATTENDANCE)</asp:ListItem>
                                                        <asp:ListItem>Form 22(RATE OF WAGES/SALARY)</asp:ListItem>
                                                        <asp:ListItem>Form 22(DEDUCTIONS)</asp:ListItem>
                                                        <asp:ListItem>Form XIII(Excel)</asp:ListItem>
                                                        <asp:ListItem>Form XXIV</asp:ListItem>

                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>

                                            <tr>
                                                <td style="width: 120px">

                                                    <asp:Label ID="lblclientid" runat="server" Text="Client Id" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                 <%--  <asp:DropDownList ID="ddlclientid" runat="server" AutoPostBack="True" Visible="false"
                                                        OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" class="sdrop">
                                                    </asp:DropDownList>--%>
                                                     <asp:DropDownList ID="ddlclientid" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" Width="120px" Visible="false">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 70px">
                                                    <asp:Label ID="lblclientname" runat="server" Text=" Client Name" Visible="false" Style="margin-left: -50px"></asp:Label>
                                                </td>
                                                <td>
                                                   <%-- <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" Visible="false" Style="width: 200px; margin-left: -15px"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" class="sdrop">
                                                    </asp:DropDownList>--%>
                                                     <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" TabIndex="2" class="ddlautocomplete chosen-select"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" Style="width: 200px" Visible="false">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblmonth" runat="server" Text="Month" Visible="false"></asp:Label>

                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtmonth" runat="server" Visible="false" Text="" class="sinput"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                    </cc1:CalendarExtender>
                                                </td>

                                                <td></td>
                                                <td>
                                                    <asp:Label ID="lblDOJ" runat="server" Text="  Date Of Joining" Visible="false" Style="margin-left: -124px"></asp:Label>

                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmpDtofJoining" runat="server" Text="" class="sinput" Visible="false" Style="margin-left: -377px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true"
                                                        TargetControlID="txtEmpDtofJoining" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEDOI1" runat="server" Enabled="True" TargetControlID="txtEmpDtofJoining"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfrom" runat="server" Text="From" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfrom" runat="server" CssClass="sinput" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtfrom_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtfrom">
                                                    </cc1:CalendarExtender>
                                                </td>

                                                <td>
                                                    <asp:Label ID="lblto" runat="server" Text="To" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtto" runat="server" CssClass="sinput" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtto_CalendarExtender" runat="server" BehaviorID="calendar2"
                                                        Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtto">
                                                    </cc1:CalendarExtender>
                                                </td>


                                            </tr>

                                        </table>
                                        <div style="float: right">
                                            <asp:Button runat="server" ID="btnSubmit" Text="Submit" class="btn save" OnClick="btnSubmit_Click" />
                                            <asp:Button runat="server" ID="btn_Submit" Text="Download" class="btn save" OnClick="btnsearch_Click" />

                                        </div>

                                        <div class="rounded_corners">
                                            <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="60%"
                                                CellSpacing="3" CellPadding="5" ForeColor="#333333" CssClass="table table-striped table-bordered table-condensed table-hover" Style="margin: 0px auto">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Emp ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblempid" runat="server" Text='<%#Eval("empid") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="Name" HeaderText="Emp Name" />

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
           </asp:content>
