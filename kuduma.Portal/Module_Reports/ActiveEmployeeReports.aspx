<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="ActiveEmployeeReports.aspx.cs" Inherits="Kuduma.Portal.ActiveEmployeeReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

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
            width: 350px;
            margin-left: 10px;
        }



        .Grid th, .Grid td {
            border: 1px solid #66CCFF;
        }
    </style>

    <script type="text/javascript">
        function OnFocus(txt, text) {
            if (txt.value == text) {
                txt.value = "";
            }
        }


        function OnBlur(txt, text) {
            if (txt.value == "") {
                txt.value = text;
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
                select: function (event, ui) { $("#ddlclientid").attr("data-clientId", ui.item.value); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });


    </script>
        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                        <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                        <li class="active"><a href="ActiveEmployeeReports.aspx" style="z-index: 7;" class="active_bread">List of Employees</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">LIST OF EMPLOYEES
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <%--<div align="right">
                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" >Export to Excel</asp:LinkButton>
                            </div>--%>

                                    <div class="dashboard_firsthalf" style="width: 630px;">
                                        <br />
                                        <table width="140%" border="0">
                                            <tr>
                                                <td width="80px">Search Mode :
                                                </td>
                                                <td width="190px">

                                                    <div style="margin-bottom: 3px;">
                                                        <cc1:ComboBox ID="ddlActiveEmp" Height="23px" Width="157px" runat="server" AutoPostBack="True"
                                                            BorderStyle="Solid" BorderColor="#F0F0F0" OnSelectedIndexChanged="ddlActiveEmp_SelectedIndexChanged"
                                                            AutoCompleteMode="SuggestAppend" RenderMode="Block" DropDownStyle="DropDownList">
                                                            <asp:ListItem Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="All"></asp:ListItem>
                                                            <asp:ListItem Text="Active"></asp:ListItem>
                                                            <asp:ListItem Text="InActive"></asp:ListItem>
                                                            <asp:ListItem Text="EmpId"></asp:ListItem>
                                                            <asp:ListItem Text="EmpName"></asp:ListItem>
                                                            <asp:ListItem Text="Designation"></asp:ListItem>
                                                            <asp:ListItem Text="JoiningDate"></asp:ListItem>
                                                            <asp:ListItem Text="LeavingDate"></asp:ListItem>
                                                            <asp:ListItem Text="NonPfDeduct"></asp:ListItem>
                                                            <asp:ListItem Text="NonESIdeduct"></asp:ListItem>
                                                            <asp:ListItem Text="noPFNumber"></asp:ListItem>
                                                            <asp:ListItem Text="noESINumber"></asp:ListItem>
                                                            <asp:ListItem Text="noBankA/CNumber"></asp:ListItem>
                                                            <asp:ListItem Text="PFNumber"></asp:ListItem>
                                                            <asp:ListItem Text="ESINumber"></asp:ListItem>
                                                            <asp:ListItem Text="BankA/CNumber"></asp:ListItem>
                                                            <asp:ListItem Text="SitePostedWise"></asp:ListItem>
                                                            <asp:ListItem Text="PVC Verified"></asp:ListItem>
                                                            <asp:ListItem Text="PVC Not Verified"></asp:ListItem>
                                                            <asp:ListItem Text="BGV Verified"></asp:ListItem>
                                                            <asp:ListItem Text="BGV Not Verified"></asp:ListItem>
                                                        </cc1:ComboBox>
                                                    </div>

                                                </td>
                                                <td>
                                                    <asp:Panel ID="panelempid" runat="server" Visible="false">
                                                        <%-- <asp:TextBox ID="TextEmpid" runat="server" Text="Enter EmpId..." 
                          onfocus="OnFocus(this,'Enter EmpId...')" onblur="OnBlur(this,'Enter EmpId...')" ></asp:TextBox>--%>
                                                        <asp:TextBox ID="TextEmpid" runat="server" Text="Enter EmpId..."></asp:TextBox>
                                                        <cc1:TextBoxWatermarkExtender ID="Tbwmeempid" runat="server" TargetControlID="TextEmpid"
                                                            WatermarkText="Enter EmpId...">
                                                        </cc1:TextBoxWatermarkExtender>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panelemp" runat="server" Visible="false">
                                                        <%--   <asp:TextBox ID="TxtEmpname" runat="server" Text="Enter EmpName..."
                           onfocus="OnFocus(this,'Enter EmpName...')" onblur="OnBlur(this,'Enter EmpName...')" ></asp:TextBox>--%>
                                                        <asp:TextBox ID="TxtEmpname" runat="server" Text="Enter EmpName..."></asp:TextBox>
                                                        <cc1:TextBoxWatermarkExtender ID="Tbwmeempname" runat="server" TargetControlID="TxtEmpname"
                                                            WatermarkText="Enter EmpName...">
                                                        </cc1:TextBoxWatermarkExtender>
                                                    </asp:Panel>
                                                    <asp:Panel ID="paneldesignation" runat="server" Visible="false">
                                                        <asp:DropDownList ID="ddldesgn" runat="server" Width="125px">
                                                        </asp:DropDownList>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panelJdate" runat="server" Visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="TxtJdateFrom" runat="server" placeholder="Enter From Date..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CEJoinDate" runat="server" Enabled="true" TargetControlID="TxtJdateFrom"
                                                                        Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="TxtJdateFrom"
                                                                        ValidChars="/0123456789">
                                                                    </cc1:FilteredTextBoxExtender>

                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtJdateTo" runat="server" placeholder="Enter To Date..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true" TargetControlID="TxtJdateTo"
                                                                        Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="TxtJdateTo"
                                                                        ValidChars="/0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panelLdate" runat="server" Visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="TxtLdateFrom" runat="server" Text="Enter From Date..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="txtLFrom_CalendarExtender" runat="server" Enabled="true"
                                                                        TargetControlID="TxtLdateFrom" Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <cc1:TextBoxWatermarkExtender ID="TbwmeLdateFrom" runat="server" TargetControlID="TxtLdateFrom"
                                                                        WatermarkText="From Date...">
                                                                    </cc1:TextBoxWatermarkExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                        TargetControlID="TxtLdateFrom" ValidChars="/0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtLdateTo" runat="server" Text="Enter To Date..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="txtLto_CalendarExtender" runat="server" Enabled="true"
                                                                        TargetControlID="TxtLdateTo" Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                        TargetControlID="TxtLdateTo" ValidChars="/0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <cc1:TextBoxWatermarkExtender ID="TbwmeLdateTo" runat="server" TargetControlID="TxtLdateTo"
                                                                        WatermarkText="To Date...">
                                                                    </cc1:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panelNonAtten" runat="server" Visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="TxtAdatefrom" runat="server" Text="Enter Month..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="txtA_CalendarExtender" runat="server" Enabled="true" TargetControlID="TxtAdatefrom"
                                                                        Format="MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                        TargetControlID="TxtAdatefrom" ValidChars="/0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                        </table>
                                                    </asp:Panel>


                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblclientid" runat="server" Text="Client ID" Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlclientid" runat="server" CssClass="ddlautocomplete chosen-select" Width="400px" Visible="false">
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                    </table>


                                                </td>
                                                <td>
                                                    <asp:Button ID="Submit" Text="Search" class="btn save" Visible="false" runat="server" OnClick="Esearch_Click" />
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lbtn_Export" runat="server"  OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div>
                                        <div class="rounded_corners" style="display:none">
                                            <asp:GridView ID="gvlistofemp" runat="server" AutoGenerateColumns="False" Width="100%"
                                                ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center" OnRowDataBound="gvlistofemp_RowDataBound">
                                                <RowStyle BackColor="#EFF3FB" Height="30" />
                                                <Columns>

                                                    <%--  0--%>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  1--%>
                                                    <asp:TemplateField HeaderText="ID" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempid" Text='<%# Bind("Empid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <%--  1--%>
                                                    <asp:TemplateField HeaderText="Old EmpID" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbloldempid" Text='<%# Bind("oldEmpid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  2--%>
                                                    <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempName" Text='<%# Bind("EmpFName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  3--%>
                                                    <asp:TemplateField HeaderText="Desgn" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempDesgn" Text='<%# Bind("EmpDesgn") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  4--%>
                                                    <asp:TemplateField HeaderText="Gender">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempGen" Text='<%# Bind("EmpSex") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  5--%>
                                                    <asp:TemplateField HeaderText="Marital Status">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempMs" Text='<%# Bind("EmpMaritalStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  6--%>
                                                    <asp:TemplateField HeaderText="Phone">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempPhone" Text='<%# Bind("EmpPhone") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  7--%>
                                                    <asp:TemplateField HeaderText="FatherName">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempFname" Text='<%# Bind("EmpFatherName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  8--%>
                                                    <asp:BoundField DataField="EmpDtofJoining" HeaderText="Date of Joining" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <%--  9--%>
                                                    <asp:BoundField DataField="EmpDtofBirth" HeaderText="Date of Birth" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <%--  10--%>
                                                    <asp:TemplateField HeaderText="Bank Ac.No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempBacc" Text='<%# Bind("EmpBankAcNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="EmpIFSCcode" HeaderText="IFSC Code" />
                                                    <%--  11--%>
                                                    <asp:TemplateField HeaderText="Bank Name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblBankname" Text='<%# Bind("Bankname") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  12--%>
                                                    <asp:TemplateField HeaderText="PF No.">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempPfno" Text='<%#Bind("empepfno") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  13--%>
                                                    <asp:TemplateField HeaderText="ESI No.">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempEsino" Text='<%#Bind("Empesino") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  14--%>
                                                    <asp:TemplateField HeaderText="PF Deduct">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempPdedu"
                                                                Text='<%# Bind("EmpPFDeduct") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  15--%>
                                                    <asp:TemplateField HeaderText="PT Deduct">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblEmpstatus"
                                                                Text='<%# Bind("EmpPTDeduct") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  16--%>
                                                    <asp:TemplateField HeaderText="ESI Deduct">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempEdedu"
                                                                Text='<%# Bind("EmpESIDeduct") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  17--%>
                                                    <asp:TemplateField HeaderText="Permanent Address" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempPadd" Text='<%# Bind("EmpPermanentAddress") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  18--%>
                                                    <asp:TemplateField HeaderText="Present Address" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempPres" Text='<%# Bind("EmppresentAddress") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  19--%>
                                                    <asp:TemplateField HeaderText="UAN Number">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblEmpstatus" Text='<%# Bind("EmpUANNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  20--%>
                                                    <asp:TemplateField HeaderText="Aadhar Number">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblEmpstatus" Text='<%# Bind("AadharCardNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  21--%>
                                                    <asp:BoundField DataField="PanCardNo" HeaderText="PAN No" />
                                                    <%--  22--%>
                                                    <asp:TemplateField HeaderText="Unit ID">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblUnitIDName" Text='<%# Bind("UnitIDName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <%--<asp:TemplateField HeaderText="PVC Number">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblpvc" Text='<%# Bind("PVCNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <asp:TemplateField HeaderText="BGV Number">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblpvc" Text='<%# Bind("BGVNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <%--  23--%>
                                                    <asp:TemplateField HeaderText="Empstatus">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblEmpstatus" Text='<%# Bind("Empstatus") %>'></asp:Label>
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

                                         <div class="rounded_corners" style="overflow: auto">
                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" Width="100%"
                                                ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center" OnRowDataBound="GridView1_RowDataBound">
                                                  <RowStyle BackColor="#EFF3FB" Height="30" />
                                                  <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                                </asp:GridView>
                                             </div>
                                        <asp:Label ID="LblResult" runat="server" Text="" Style="color: red"></asp:Label>
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
           
    <script type="text/javascript">
        Sys.Browser.WebKit = {};
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function () {

                setProperty();
            });
        };
    </script>
</asp:content>