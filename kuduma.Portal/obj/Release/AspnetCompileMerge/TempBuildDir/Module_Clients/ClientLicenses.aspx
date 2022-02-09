<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientLicenses.aspx.cs" MasterPageFile="~/Module_Clients/Clients.master" Inherits="Kuduma.Portal.ClientLicenses" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <link rel="shortcut icon" href="assets/Mushroom.ico" />
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
                  select: function (event, ui) { $("#<%=ddlClientId.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                  select: function (event, ui) { $("#<%=ddlCname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
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
              $("#<%=ddlCname.ClientID %>").trigger('change');
          }
    </script>

    <style type="text/css">
        
    </style>
    <script type="text/javascript" src="script/jscript.js">
    </script>

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">
                Clients Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">      
                    <div class="sidebox">
                  <div class="boxhead">
                    <h2>Licenses Expiring this Month</h2>
                  </div>
                  
      <asp:ScriptManager runat="server" ID="Scriptmanager2">
                    </asp:ScriptManager>
                  <div class="boxbody" style="padding:5px 5px 5px 5px;">
                  <div class="rounded_corners">
                  
                       <asp:GridView ID="dgLicExpire" runat="server" AllowPaging="True" 
                           AutoGenerateColumns="False" 
                           EmptyDataRowStyle-BackColor="BlueViolet" 
                           EmptyDataRowStyle-BorderColor="Aquamarine" EmptyDataRowStyle-Font-Italic="true" 
                           EmptyDataText="No Records Found" GridLines="None" Height="97px" PageSize="5" 
                           style="margin-bottom: 0px" Width="100%" 
                           CellPadding="5" CellSpacing="3">
                           <RowStyle HorizontalAlign="Left" BackColor="#EFF3FB" />
                           <EmptyDataRowStyle BackColor="SkyBlue" BorderColor="Aquamarine" 
                               Font-Italic="True" />
                           <Columns>
                               <asp:TemplateField  
                                   HeaderText="Client ID" >
                                   <ItemTemplate>
                                       <asp:Label ID="lblCust0" runat="server" Text='<%#Bind("UnitId")%>'></asp:Label>
                                   </ItemTemplate>
                                   <HeaderStyle Wrap="False" />
                                   <ItemStyle HorizontalAlign="Center" />
                               </asp:TemplateField>
                               <asp:BoundField DataField="ClientName"  
                                   HeaderText="Client Name" >
                                    <ItemStyle HorizontalAlign="Center" />
                               </asp:BoundField>
                               <asp:BoundField DataField="LicenseStartDate" DataFormatString="{0:d}"
                                   HeaderText="License StartDate" >
                                    <ItemStyle HorizontalAlign="Center" />
                               </asp:BoundField>
                               <asp:BoundField DataField="LicenseEndDate" DataFormatString="{0:d}" 
                                  HeaderText="License EndDate" HtmlEncode="False" 
                                  >
                                   <ItemStyle HorizontalAlign="Center" />
                               </asp:BoundField>
                               <asp:BoundField DataField="LicenseOfficeLoc" 
                                   HeaderText="Location of LicenseOffice" HtmlEncode="False" 
                                   >
                                    <ItemStyle HorizontalAlign="Center" />
                               </asp:BoundField>
                           </Columns>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" HorizontalAlign="Center" 
                                    BorderWidth="1px" CssClass = "GridPager"/>
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                       </asp:GridView>
                       </div>
                  </div>
                </div>
                    <div class="sidebox" style="margin:10px 0px 0px 0px;">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Add Licenses</h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <!--  Content to be add here> -->
                            
                           
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width:100%">
                                <br />
                                <table width="100%"  cellpadding="5" cellspacing="5">
                                <tr>
                                <td valign="top">
                                
                                
                                    <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>
                                                    Client ID<span style=" color:Red">*</span>
                                                </td>
                                                <td>
                                                <%--<asp:DropDownList runat="server" class="sdrop" ID="ddlClientId" AutoPostBack="True" 
                                                        onselectedindexchanged="ddlClientId_SelectedIndexChanged"></asp:DropDownList>--%>
                                                    <asp:DropDownList ID="ddlClientId" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged" Width="120px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    LicenseNo<span style=" color:Red">*</span>
                                                </td>
                                                <td>
                                                <asp:TextBox runat="server" ID="txtLicenseNo"  class="sinput" TabIndex="1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    License Start Date<span style=" color:Red">*</span>
                                                </td>
                                                <td>
                                                <asp:TextBox runat="server" ID="txtLicStart"  class="sinput" TabIndex="3"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true" TargetControlID="txtLicStart"
                                                                    Format="MM/dd/yyyy">
                                                                </cc1:CalendarExtender>
                                                                
                                                                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                                                 runat="server" Enabled="True" TargetControlID="txtLicStart"
                                                                  ValidChars="/0123456789">
                                                                  </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                   </table>
                                   
                                   
                                   </td>
                                <td>
                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                Client Name<span style=" color:Red">*</span>
                                            </td>
                                            <td>
                                                <%--<asp:DropDownList ID="ddlCname" runat="server"  class="sdrop" width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlCname_OnSelectedIndexChanged">  </asp:DropDownList>--%>
                                                  <asp:DropDownList ID="ddlCname" runat="server" AutoPostBack="True" TabIndex="2" class="ddlautocomplete chosen-select"
                                                        OnSelectedIndexChanged="ddlCname_OnSelectedIndexChanged" Style="width: 200px">
                                                    </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Location Of LicenseOffice
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtLicOffLoc"  class="sinput" TabIndex="2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                License End Date<span style=" color:Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtLicEnd"  class="sinput" TabIndex="4"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true" TargetControlID="txtLicEnd"
                                                                    Format="MM/dd/yyyy">
                                                                </cc1:CalendarExtender>
                                                                  <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                                 runat="server" Enabled="True" TargetControlID="txtLicEnd"
                                                                  ValidChars="/0123456789">
                                                                  </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr><td>&nbsp;</td></tr>
                                        <tr><td>&nbsp;</td><td align="right"> <asp:Label ID="lblresult" runat="server" Visible="false" Style="color: Red"></asp:Label>
                                <asp:Button ID="btnaddclint" runat="server" 
                                OnClick="btnaddclint_Click" 
                                OnClientClick='return confirm(" Are you sure you  want to add This record?");'
                                    Text="Save" class="btn save" ValidationGroup="a" />
                                <asp:Button ID="btncancel" runat="server" Text="CANCEL" ToolTip="Cancel Client" 
                                    class=" btn save" 
                                    OnClientClick='return confirm(" Are you sure you want to cancel This  entry?");' 
                                    onclick="btncancel_Click" /></td></tr>
                                    </table>
                                    </td>
                                    </tr>
                                </table>
                                   
                                </div>
                                
                            </div>
                           
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
            <!-- DASHBOARD CONTENT END -->
        </div>
       </asp:content>
