<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="PaySheetWagesReport.aspx.cs" Inherits="Kuduma.Portal.PaySheetWagesReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/Calendar.css" rel="stylesheet" type="text/css" />
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
                    <li class="active"><a href="PaySheetWagesReport.aspx" style="z-index: 7;" class="active_bread">Paysheet Wages
                        Report</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Paysheet Wages Report
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div align="right">
                                    <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                </div>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td style
                                                ="width:7%">
                                                Client ID :
                                            </td>
                                            <td>
                                                <%--<asp:DropDownList runat="server" class="sdrop" ID="ddlClientId" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                                 <asp:DropDownList ID="ddlClientId" runat="server" CssClass="ddlautocomplete chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged"
                                                            Width="120px">
                                                        </asp:DropDownList>
                                            </td>
                                            <td style="padding-left:50px;width:15%">
                                                Client Name :
                                            </td>
                                            <td>
                                                <%--<asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="true" class="sdrop"
                                                    OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                                <asp:DropDownList ID="ddlcname" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged"
                                                            Style="width: 355px">
                                                        </asp:DropDownList>
                                            </td>
                                            <td style="width: 40%;padding-left:50px">
                                                &nbsp;<asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="rounded_corners">
                                    <div style="overflow: scroll; width: auto">
                                        <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                            Height="50px" CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#EFF3FB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client ID">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblclientid" Text="<%# Bind('ClientId') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblclientname" Text="<%# Bind('ClientName') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ContractStartDate" HeaderText="Contract Start Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="ContractEndDate" HeaderText="Contract End Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="Design" HeaderText="Designation" />
                                                <asp:BoundField DataField="NoOfDays" HeaderText="No Of Days" />
                                                <asp:BoundField DataField="Nots" HeaderText="NOTs" />
                                                <asp:BoundField DataField="Basic" HeaderText="Basic" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="DA" HeaderText="DA" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="HRA" HeaderText="HRA" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="Conveyance" HeaderText="Conv." DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="CCA" HeaderText="CCA" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="LeaveAmount" HeaderText="L.A" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="Gratuity" HeaderText="Gratuity" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="Bonus" HeaderText="Bonus" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="WashAllowance" HeaderText="W.A" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="OtherAllowance" HeaderText="O.A" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="NFhs" HeaderText="Nfhs" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="RC" HeaderText="RC" DataFormatString="{0:0}" />
                                                <asp:BoundField DataField="CS" HeaderText="CS" DataFormatString="{0:0}" />
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div>
                                    <table width="100%">
                                        <tr style="width: 100%; font-weight: bold">
                                            <td style="width: 60%">
                                                <asp:Label ID="lbltamttext" runat="server" Visible="false" Text="Total Amount"></asp:Label>
                                            </td>
                                            <td style="width: 40%">
                                                <asp:Label ID="lbltmtemppf" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="lbltemprpf" runat="server" Text="" Style="margin-left: 30%"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
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
