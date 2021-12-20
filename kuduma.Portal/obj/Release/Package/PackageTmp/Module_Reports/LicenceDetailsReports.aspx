<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="LicenceDetailsReports.aspx.cs" Inherits="Kuduma.Portal.LicenceDetailsReports" %>
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

    <style type="text/css">
        .style2
        {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
           <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="LicenceDetailsReports.aspx" style="z-index: 7;" class="active_bread">Licence Details
                        </a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Licenses
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                    <asp:ScriptManager runat="server" ID="ScriptEmployReports"></asp:ScriptManager>
                    
                        <div class="dashboard_firsthalf" style="width: 100%">
                          
                                <table width="90%">
                              <tr>
                                  <td style="width:10%">
                                           Client ID :</td>
                                    <td> 
                                        <%--<asp:DropDownList runat="server" class="sdrop" ID="ddlClientId"  AutoPostBack="true"
                                              onselectedindexchanged="ddlClientId_SelectedIndexChanged"></asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddlClientId" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged" Width="120px">
                                                    </asp:DropDownList>
                                    </td>
                                   <td >  Client Name :</td>
                           <td> <%-- <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="true"  class="sdrop" OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                             </asp:DropDownList>--%>

                               <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" TabIndex="2" class="ddlautocomplete chosen-select"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" Style="width: 200px">
                                                    </asp:DropDownList>
                           </td>
                             <td><asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btn_SubmitClick" />
                       </td>
                       </tr>


                                    <tr style="height:10px"></tr>
                                            </table>
                            </div>
                           
                               <div class="rounded_corners">
                        
                       <%--<asp:GridView ID="dgLicExpire" runat="server" AllowPaging="True" 
                           AutoGenerateColumns="False" HeaderStyle-BackColor="SkyBlue"
                           EmptyDataRowStyle-BackColor="BlueViolet" 
                           EmptyDataRowStyle-BorderColor="Aquamarine" EmptyDataRowStyle-Font-Italic="true" 
                           EmptyDataText="No Records Found" GridLines="None" PageSize="5" 
                           style="margin-bottom: 0px" Width="100%" 
                           ForeColor="#333333" CellPadding="5" CellSpacing="0">
                           <RowStyle HorizontalAlign="Left" BackColor="#EFF3FB" Height="30" />
                           <EmptyDataRowStyle BackColor="SkyBlue" BorderColor="Aquamarine" 
                               Font-Italic="True" />--%>
                                    <asp:GridView ID="dgLicExpire" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    CellPadding="4" ForeColor="#333333">
                           <Columns>
                               <asp:TemplateField  
                                   HeaderText="Client ID" >
                                   <ItemTemplate>
                                       <asp:Label ID="lblCust0" runat="server" Text='<%#Bind("UnitId")%>'></asp:Label>
                                   </ItemTemplate>
                                   <HeaderStyle Wrap="False" />
                               </asp:TemplateField>
                               <asp:BoundField DataField="ClientName"  
                                   HeaderText="Client Name" >
                               </asp:BoundField>
                               <asp:BoundField DataField="LicenseStartDate" DataFormatString="{0:d}"
                                   HeaderText="License StartDate" >
                               </asp:BoundField>
                               <asp:BoundField DataField="LicenseEndDate" DataFormatString="{0:d}" 
                                  HeaderText="License EndDate" HtmlEncode="False" 
                                  >
                               </asp:BoundField>
                               <asp:BoundField DataField="LicenseOfficeLoc" 
                                   HeaderText="Location of License Office" HtmlEncode="False" 
                                   >
                               </asp:BoundField>
                           </Columns>
                           <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                           <PagerStyle HorizontalAlign="Center" />
                           <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                           <HeaderStyle HorizontalAlign="Left"  Height="30" />
                           <EditRowStyle CssClass="row" BackColor="#2461BF" />
                           <AlternatingRowStyle CssClass="altrow" BackColor="White" />
                       </asp:GridView>
                  </div>
                                <asp:Label ID="LblResult" runat="server" Text="" style=" color:red"></asp:Label>
                                
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
