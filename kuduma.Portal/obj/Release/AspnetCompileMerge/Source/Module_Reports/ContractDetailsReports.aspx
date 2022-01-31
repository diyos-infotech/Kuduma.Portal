<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ContractDetailsReports.aspx.cs" Inherits="Kuduma.Portal.ContractDetailsReports" %>
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
                  select: function (event, ui) { $("#<%=ddlClientID.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                  select: function (event, ui) { $("#<%=ddlcname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
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
              $("#<%=ddlcname.ClientID %>").trigger('change');
          }
    </script>
    <style type="text/css">
        .style1
        {
            width: 135px;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
           <div id="breadcrumb">
                <ul class="crumbs">
                    <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="ContractDetailsReports.aspx" style="z-index: 7;" class="active_bread">Contract Details</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                             Contract Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                            
                    <asp:ScriptManager runat="server" ID="ScriptEmployReports"></asp:ScriptManager>
                    
                       <div class="dashboard_firsthalf" style="width: 100%">
                       
                             <div align="right">
                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" >Export to Excel</asp:LinkButton>
                            </div>
                            
                        
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEmployee" runat="server" Text="Select Client ID "> </asp:Label><span style=" color:Red">*</span></td>
                                         <td>   <%--<asp:DropDownList runat="server" AutoPostBack="true" ID="ddlClientID" 
                                                class="sdrop" onselectedindexchanged="ddlClientID_SelectedIndexChanged"> 
                                                
                                            </asp:DropDownList>--%>

                                               <asp:DropDownList ID="ddlClientID" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged" Width="120px">
                                                    </asp:DropDownList>
                                        </td>
                                        <td style="padding-left:50px">
                                              <asp:Label ID="lblClientname" runat="server" Text="Name" > </asp:Label> </td>
                                        <td>
                                            <%--<asp:DropDownList ID="ddlcname" runat="server" class="sdrop" AutoPostBack="true" OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged"></asp:DropDownList>--%>
                                            <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" TabIndex="2" class="ddlautocomplete chosen-select"
                                                        OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" Style="width: 200px">
                                                    </asp:DropDownList>

                                        </td>
                                        <td style="padding-left:30px">
                                            <asp:Label ID="lblContractID" runat="server" Text="Contract ID"> </asp:Label></td>
                                           <td> <asp:TextBox ID="txtContractID" class="sinput" runat="server" Enabled="false"></asp:TextBox>
                                          
                                           </td>
                                    </tr>   
                                    <tr>
                                       <td>
                                            <asp:Label ID="lblDays" runat="server" Text="Days Per Month " > </asp:Label></td>
                                        <td>    <asp:TextBox ID="txtDays" runat="server" class="sinput" Enabled="false" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblServiceCharge" runat="server" Text="Service Charge" > </asp:Label></td>
                                          <td>  <asp:TextBox ID="txtServiceCharge" class="sinput" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                              
                                               <td>
                                            <asp:Label ID="lblMachinaryCost" runat="server" Text="Machinary Cost"> </asp:Label></td>
                                         <td>   <asp:TextBox ID="txtMachinaryCost" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                        </td>   
                                    </tr>        
                                    <tr>
                                      
                                        <td>
                                            <asp:Label ID="lblMaterialCost" runat="server" Text="Material Cost"> </asp:Label></td>
                                          <td>  <asp:TextBox ID="txtMaterialCost" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                        </td>
                                         <td>
                                            <asp:Label ID="lblOTpercent" runat="server" Text="OT Percent"> </asp:Label></td>
                                           <td> <asp:TextBox ID="txtOTpercent" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td align="left">
                                          <asp:Button runat="server" ID="btnSubmit" Text="Submit" class="btn save" 
                                                onclick="btnSubmit_Click" /><br />
                                                <asp:Label ID="LblResult" runat="server" Visible="false" style=" color:Red"> </asp:Label>
                                           </td>
                                    </tr> 
                                                           
                                </table>
                                
                                
                            </div>                            
                            <div class="rounded_corners" style="overflow:scroll">
                                <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CellSpacing="3" CellPadding="4" ForeColor="#333333" GridLines="None">
                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                    <Columns>
                                    
                                      <asp:TemplateField HeaderText="Clientid">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblClientid" Text="<%# Bind('Clientid') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                      <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblclientname" Text="<%# Bind('clientname') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                       <asp:TemplateField HeaderText="Contract ID">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblclientname" Text="<%# Bind('ContractId') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>      
                                            
                                              <asp:TemplateField HeaderText="No Of Days For Billing">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblNoofDays" 
                                                Text='<%# (Eval("NoofDays")!=DBNull.Value ? ((Convert.ToInt32(Eval("NoofDays"))==0)? "General":Eval("NoofDays")):"NULL")%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                             
                                              <asp:TemplateField HeaderText="No Of Days For Wages">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblNoOfDaysWages"
                                                Text='<%# (Eval("NoOfDaysWages")!=DBNull.Value ? ((Convert.ToInt32(Eval("NoOfDaysWages"))==0)? "General":Eval("NoOfDaysWages")):"NULL")%>'>
                                               </asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpId" Text="<%# Bind('Design') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpFirstName" Text="<%# Bind('Quantity') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Basic">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpMiddleName" Text="<%# Bind('Basic') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DA">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpLastName" Text="<%# Bind('DA') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="HRA">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpMiddleName" Text="<%# Bind('HRA') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CCA">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpLastName" Text="<%# Bind('CCA') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Conveyance">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpMiddleName" Text="<%# Bind('Conveyance') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wash Allowance">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpLastName" Text="<%# Bind('WashAllowance') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Other Allownce">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpMiddleName" Text="<%# Bind('OtherAllowance') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay Rate">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpLastName" Text="<%# Bind('Amount') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Leave Amount">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpLastName" Text="<%# Bind('LeaveAmount') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bonus">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpMiddleName" Text="<%# Bind('Bonus') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gratuity">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpLastName" Text="<%# Bind('Gratuity') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PF %">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpMiddleName" Text="<%# Bind('PF') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ESI %">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpLastName" Text="<%# Bind('ESI') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PF On">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpMiddleName" Text='<%# (Eval("PfFrom")!=DBNull.Value ? ((Convert.ToInt32(Eval("PfFrom"))!=0)? "Basic+DA":"Basic"):"NULL")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ESI On">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpLastName" Text='<%# (Eval("ESIFrom")!=DBNull.Value ? ((Convert.ToInt32(Eval("ESIFrom"))!=0)? "Gross":"Gross-WA"):"NULL")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="OT Percent">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblotpercent" Text='<%# Eval("OTPersent")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Material Cost">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmaterialcost" Text="<%# Bind('MaterialCostPerMonth') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Machinary Cost">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmachinarycost" Text="<%# Bind('MachinaryCostPerMonth') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
       </asp:content>
