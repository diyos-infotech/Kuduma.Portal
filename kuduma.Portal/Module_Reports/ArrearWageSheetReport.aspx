<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ArrearWageSheetReport.aspx.cs" Inherits="Kuduma.Portal.ArrearWageSheetReport" %>

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
         function AssignExportHTML() {

             document.getElementById('hidGridView').value = htmlEscape(forExport.innerHTML);
         }
         function htmlEscape(str) {
             return String(str)
             .replace(/&/g, '&amp;')
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
        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                       <%-- <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                        <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                        <li class="active"><a href="ArrearWageSheetReport.aspx" style="z-index: 7;" class="active_bread">Arrear Wage Sheet</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Arrear Wage Sheet Report
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">

                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>

                                    <div class="dashboard_firsthalf" style="width: 100%">
                                        <div align="right">
                                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" OnClientClick="AssignExportHTML()">Export to Excel</asp:LinkButton>
                                                    </div>
                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td style="width:6%">Client Id
                                                </td>
                                                <td>
                                                    <%--<asp:DropDownList ID="ddlclientid" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" class="sdrop">
                                                    </asp:DropDownList>--%>
                                                    <asp:DropDownList ID="ddlclientid" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" Width="120px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="padding-left:50px">Name
                                                </td>
                                                <td>
                                                   <%-- <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" class="sdrop">
                                                    </asp:DropDownList>--%>
                                                     <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" TabIndex="2" class="ddlautocomplete chosen-select"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" Style="width: 200px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width:6%;padding-left:20px">Month
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmonth" runat="server" Text=""  AutoComplete="off" class="sinput"></asp:TextBox>
                                                     <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 30%">
                                                    <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                      <asp:HiddenField ID="hidGridView" runat="server" />
                                    <div id="forExport" class="rounded_corners" style="overflow: scroll">
                                        <asp:GridView ID="GVListEmployees" runat="server" ShowFooter="true" AutoGenerateColumns="False" Width="100%"
                                             CellPadding="5" ForeColor="#333333"  CssClass="bordercss"
                                             OnRowDataBound="GVListEmployees_RowDataBound">
                                            <RowStyle BackColor="white"  />
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
                                                <asp:TemplateField HeaderText="Client ID" ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                                                    <HeaderStyle Width="70px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <%-- 2--%>

                                                <asp:TemplateField HeaderText="Client Name" ItemStyle-HorizontalAlign="left" ItemStyle-Width="200px">
                                                    <HeaderStyle Width="200px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("clientname") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <%-- 3--%>
                                                <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblempid" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 4--%>
                                                <asp:TemplateField HeaderText="Emp Name" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblempname" runat="server" Text='<%#Bind("EmpMname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <%-- 5--%>
                                                <asp:TemplateField HeaderText="Desgn" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldesgn" runat="server" Text='<%#Bind("Desgn") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 6--%>
                                                <asp:TemplateField HeaderText="Month-Year" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmonth" runat="server" Text='<%#Bind("month") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 7--%>
                                                <asp:TemplateField HeaderText="Duties" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldutyhrs" runat="server" Text='<%#Bind("NoOfDuties") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDuties"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 8--%>
                                                <asp:TemplateField HeaderText="OTs" ItemStyle-HorizontalAlign="Center"  >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOts" runat="server" Text='<%#Bind("OTs") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalOTs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 9--%>
                                                <asp:TemplateField HeaderText="WO" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblwos" runat="server" Text='<%#Bind("WO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalwos"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 10--%>
                                                <asp:TemplateField HeaderText="Nhs" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNhs" runat="server" Text='<%#Bind("NHS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNhs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 11--%>
                                                <asp:TemplateField HeaderText="Npots" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNpots" runat="server" Text='<%#Bind("npots") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNpots"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 12--%>
                                                <asp:TemplateField HeaderText="Sal Rate" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltempgross" runat="server" Text='<%#Bind("TempGross") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotaltempgross"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 13--%>

                                                <asp:TemplateField HeaderText="Basic" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblbasic" runat="server" Text='<%#Bind("basic") %>'>--%>
                                                        <asp:Label ID="lblbasic" runat="server" Text='<%#Eval("basic", "{0:0}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBasic"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 14--%>
                                                <asp:TemplateField HeaderText="DA" ItemStyle-HorizontalAlign="Right"  >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblda" runat="server" Text='<%#Eval("da","{0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 15--%>
                                                <asp:TemplateField HeaderText="HRA" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblhra" runat="server" Text='<%#Bind("hra","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalHRA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 16--%>
                                                <asp:TemplateField HeaderText="CCA" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcca" runat="server" Text='<%#Bind("CCa","{0}") %>'>  
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalCCA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 17--%>
                                                <asp:TemplateField HeaderText="Conv" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConveyance" runat="server" Text='<%#Bind("conveyance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalConveyance"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 18--%>
                                                <asp:TemplateField HeaderText="WA" ItemStyle-HorizontalAlign="Right"  >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblwashallowance" runat="server" Text='<%#Bind("WashAllowance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalWA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 19--%>
                                                <asp:TemplateField HeaderText="OA" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOtherallowance" runat="server" Text='<%#Bind("OtherAllowance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalOA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 20--%>
                                                <asp:TemplateField HeaderText="LA" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLeaveEncashAmt" runat="server" Text='<%#Bind("LeaveEncashAmt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalLeaveEncashAmt"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 21--%>
                                                <asp:TemplateField HeaderText="Gratuity" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGratuity" runat="server" Text='<%#Bind("Gratuity","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalGratuity"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 22--%>
                                                <asp:TemplateField HeaderText="Bonus" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBonus" runat="server" Text='<%#Bind("Bonus","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBonus"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                 <%-- 23--%>
                                                <asp:TemplateField HeaderText="Att Bonus" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAttBonus" runat="server" Text='<%#Bind("AttBonus","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalAttBonus"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 24--%>
                                                <asp:TemplateField HeaderText="NFHs" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNfhs" runat="server" Text='<%#Bind("Nfhs","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNfhs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 25--%>
                                                <asp:TemplateField HeaderText="RC" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrc" runat="server" Text='<%#Bind("rc","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalrc"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 26--%>
                                                <asp:TemplateField HeaderText="CS" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcs" runat="server" Text='<%#Bind("cs","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalcs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                 <%-- 27--%>

                                                <asp:TemplateField HeaderText="Spl Allw" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSplAllw" runat="server" Text='<%#Bind("specialallw","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalSplAllw"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                 <%-- 28--%>

                                                <asp:TemplateField HeaderText="Prof Allw" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProfAllw" runat="server" Text='<%#Bind("Profallowance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalProfAllw"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                 <%-- 29--%>

                                                <asp:TemplateField HeaderText="OT Amt" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOTAmt" runat="server" Text='<%#Bind("OTAmt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalOTAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 30--%>

                                                  <asp:TemplateField HeaderText="NHs Amt" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNHsAmt" runat="server" Text='<%#Bind("NHsamt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNHsAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <%-- 31--%>
                                                <asp:TemplateField HeaderText="Gross" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGross" runat="server" Text='<%#Bind("Gross","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalGross"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                           
                                                
                                                 <%-- 32--%>

                                                <asp:TemplateField HeaderText="Uniform Allw" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUniformAllw" runat="server" Text='<%#Bind("UniformAllw","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalUniformAllw"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                  <%-- 33--%>

                                                <asp:TemplateField HeaderText="Total Earnings" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalEarnings" runat="server" Text='<%#Bind("TotalEarnings","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalTotalEarnings"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                               
                                                <%-- 34--%>
                                                <asp:TemplateField HeaderText="PF" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPF" runat="server" Text='<%#Bind("PF","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalPF"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 35--%>
                                                <asp:TemplateField HeaderText="ESI" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblESI" runat="server" Text='<%#Bind("ESI","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalESI"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                   <%-- 36--%>
                                                <asp:TemplateField HeaderText="PT" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPT" runat="server" Text='<%#Bind("proftax","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalPT"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 37--%>
                                                <asp:TemplateField HeaderText="Total Ded" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeductions" runat="server" Text='<%#Bind("TotalDeductions","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDeductions"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 38--%>
                                                <asp:TemplateField HeaderText="Net Amt" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblnetamount" runat="server" Text='<%#Bind("ActualAmount","{0:0}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNetAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                 <%-- 39--%>
                                                <asp:TemplateField HeaderText="Bank A/C No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbankno" runat="server" Text='<%# Eval("Empbankacno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <%-- 40--%>
                                                <asp:BoundField DataField="EmpBankCardRef" HeaderText="Reference No." DataFormatString="{0}&nbsp;" />
                                                 <%-- 41--%>
                                                <asp:BoundField DataField="EmpIFSCcode" HeaderText="IFSC Code" DataFormatString="{0}&nbsp;" />
                                                 <%-- 42--%>
                                                <asp:BoundField DataField="Empbankname" HeaderText="Bank Name" DataFormatString="{0}&nbsp;" />
                                            </Columns>
                                            <FooterStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                            <PagerStyle BackColor="white" ForeColor="black" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                            <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                            <EditRowStyle BackColor="white" />
                                            <AlternatingRowStyle BackColor="White" />
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
           </asp:content>
