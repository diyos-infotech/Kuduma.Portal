<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Employees/EmployeeMaster.master" CodeBehind="EmployeePayments.aspx.cs" Inherits="Kuduma.Portal.EmployeePayments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
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
            padding: 5px 2px;
            /*width:130px;*/
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
                select: function (event, ui) { $("#<%=ddlClients.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlcname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlClients.ClientID %>").trigger('change');
        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {
            $("#<%=ddlcname.ClientID %>").trigger('change');
        }
    </script>


    <script type="text/javascript">
        function AssignExportHTML() {

            document.getElementById("ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_hidGridView").value =
                htmlEscape(ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_forExport.innerHTML);
        }
        function htmlEscape(str) {
            return String(str)
                .replace(/&/g, '&amp;')
                .replace(/"/g, '&quot;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');
        }

    </script>

    <script type="text/javascript">

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

    </script>

    <style type="text/css">
        .style1 {
            width: 135px;
        }

        .modalBackground {
            background-color: Gray;
            z-index: 10000;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">Payments Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Employee Payments
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkalllist" runat="server" AutoPostBack="true" GroupName="Checked" Text="All Clients" OnCheckedChanged="chkalllist_CheckedChanged" />
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkexcludelist" runat="server" Text="Exclude Generated Clients" GroupName="Checked" AutoPostBack="true" OnCheckedChanged="chkexcludelist_CheckedChanged"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>Month :
                                        </td>
                                        <td width="17%">
                                            <asp:DropDownList ID="ddlmonth" Width="100px" runat="server" class="sdrop" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="Txt_Month" Width="100px" runat="server" AutoPostBack="true" class="sinput"
                                                Text="" Visible="false" OnTextChanged="Txt_Month_TextChanged"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd/MM/yyyy" TargetControlID="Txt_Month"></cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="Txt_Month_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" TargetControlID="Txt_Month"
                                                ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                            &nbsp;&nbsp;
                                            <asp:CheckBox ID="Chk_Month" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Month_CheckedChanged"
                                                Text="Old" />


                                            <%--  OnTextChanged="Txt_Month_OnTextChanged"--%>
                                            <cc1:ModalPopupExtender ID="modelLogindetails" runat="server" TargetControlID="btnSubmit" PopupControlID="pnllogin"
                                                BackgroundCssClass="modalBackground">
                                            </cc1:ModalPopupExtender>

                                        </td>

                                        <td style="padding-left: -40px">Client ID : 
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="ddlClients" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlClients_SelectedIndexChanged" Width="50px">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="padding-left: 20px">Client Name :
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" TabIndex="2" class="ddlautocomplete chosen-select"
                                                OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" Style="width: 130px">
                                            </asp:DropDownList>
                                        </td>

                                        <td style="padding-left: 20px">
                                            <asp:Button ID="btnpayment" runat="server" Text="Generate Payment " class=" btn save"
                                                OnClick="btnpayment_Click" Width="120px" OnClientClick='return confirm("Are you sure you want  to genrate  payment?");' />
                                            <asp:LinkButton ID="linkrefresh" runat="server" Text="Refresh" Visible="false" OnClick="linkrefresh_Click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div class="dashboard_full">
                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                <asp:Button ID="Button3" runat="server" Text="Wage Slips New" class="btn save"
                                                    OnClick="btnEmpWageSlip_Click" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkPerOne" runat="server" Text="Wage Slip Two" />

                                                <asp:DropDownList ID="ddlfontSize" runat="server">
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>9</asp:ListItem>
                                                    <asp:ListItem>8</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ChkWithoutClient" runat="server" Text="Without Client" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <%-- Attendance :--%>
                                            </td>
                                            <td style="visibility: hidden">
                                                <asp:DropDownList ID="ddlnoofattendance" class="sdrop" Width="75px" runat="server">
                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                    <asp:ListItem>10-Above</asp:ListItem>
                                                    <asp:ListItem>0-10</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <%-- Order By :--%>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="radioempid" runat="server" Checked="true" GroupName="Orderby" Visible="false"
                                                    Text="Empid" />
                                                <asp:RadioButton ID="radiobankno" runat="server" GroupName="Orderby" Visible="false" Text="Bank A/C No" />
                                            </td>
                                            <td>Payment Options :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlpaymenttype" runat="server" Width="125px" class="sdrop">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>Only Duties</asp:ListItem>
                                                    <asp:ListItem>Only OTs</asp:ListItem>
                                                </asp:DropDownList>

                                            </td>

                                            <td>
                                                <asp:Button ID="btndownloadpdffile" runat="server" Text="Download" class="btn save"
                                                    OnClick="btndownloadpdffile_Click" />
                                            </td>


                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnwithoutots" runat="server" Text="Wage Slips WithoutOT'S" Visible="false" class="btn save"
                                                    OnClick="btnwithoutots_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="Button2" runat="server" Text="Wage Slip (Dts)" class="btn save"
                                                    OnClick="btnEmpOnlyDtsWageSlip_Click" />
                                            </td>
                                            <td>
                                                <asp:CheckBox runat="server" Visible="false" ID="chkbonus" Text="Bonus" />
                                            </td>
                                            <td colspan="9"></td>
                                            <td>

                                                <asp:Button ID="btnFreeze" runat="server" Text="Freeze" class="btn save" Visible="false"
                                                    OnClick="btnFreeze_Click" />

                                            </td>

                                            <td>

                                                <asp:Button ID="btnUnFreeze" runat="server" Text="UnFreeze" class="btn save" Visible="false"
                                                    OnClick="btnUnFreeze_Click" OnClientClick='return confirm(" Are you sure you want to unfreeze the Paysheet ?");' />

                                               
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:ScriptManager runat="server" ID="Scriptmanager2">
                                </asp:ScriptManager>
                                &nbsp;
                                  <%--  <cc1:ModalPopupExtender ID="ModalFreezeDetails" runat="server" TargetControlID="btnadminUnFreeze" PopupControlID="pnlFreeze"
                                        BackgroundCssClass="PnlBackground">
                                    </cc1:ModalPopupExtender>--%>

                                <asp:Panel ID="pnlFreeze" runat="server" Height="100px" Width="300px" DefaultButton="btnFreezeSubmit" Style="display: none; position: absolute; background-color: white; box-shadow: rgba(0,0,0,0.4)">
                                    <asp:UpdatePanel ID="UpFreeze" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font: bold; font-size: medium">&nbsp;&nbsp;&nbsp;
                            Enter Password:
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="TxtFreeze" runat="server" TextMode="Password"></asp:TextBox>
                                                    </td>
                                                </tr>

                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />
                                    <table style="background-position: center;">
                                        <tr>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnFreezeSubmit" runat="server" Text="Submit" OnClick="btnFreezeSubmit_Click" class="btn Save" />
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnFreezeClose" runat="server" Text="Close" OnClick="btnFreezeClose_Click" class="btn Save" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                &nbsp;

                                    <asp:Panel ID="pnllogin" runat="server" Height="100px" Width="300px" Style="display: none; position: absolute; background-color: Silver;">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font: bold; font-size: medium">&nbsp;&nbsp;&nbsp;
                            Enter Password:
                                                        </td>
                                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <br />
                                        <table style="background-position: center;">
                                            <tr>
                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" class="btn Save" />
                                                </td>
                                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" class="btn Save" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                <br />
                                <div class="rounded_corners" style="overflow: auto; width: 99%">


                                    <asp:Panel ID="PnlNonGeneratedPaysheet" runat="server"
                                        Visible="false">
                                        <div style="border: 1px solid #A1DCF2; margin-left: 13px; width: 98%; text-align: center; width: 94%; padding: 15px">
                                            <asp:Label ID="lblPaysheetGeneratedTime" runat="server" Text="Label"></asp:Label><br />
                                            <asp:GridView ID="GvBillVsPaysheet" runat="server" AutoGenerateColumns="False" GridLines="None" CellPadding="10" Style="margin: 0px auto; margin-top: 10px;" Visible="false">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblType" runat="server" Text='<%#Bind("Type") %>' Style="padding-left: 7px;"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:BoundField HeaderText="Billing Duties" DataField="BillingDuties" NullDisplayText="0" />

                                                    <asp:TemplateField HeaderText="Paysheet Duties" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="130px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPaysheetDuties" runat="server" Text='<%#Bind("PaysheetDuties") %>' Style="padding-left: 7px;"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Difference" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDifference" runat="server" Text='<%#Bind("Difference") %>' Style="padding-left: 7px;"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                            <asp:Label ID="lblText" runat="server" Text="" Visible="false"></asp:Label><br />
                                            <asp:Label ID="lblReason" runat="server" Text="" Visible="false"></asp:Label><br />
                                            <asp:GridView ID="GvNonGeneratedEmp" runat="server" AutoGenerateColumns="False" GridLines="None" CellPadding="10" Style="margin: 0px auto; margin-top: 10px;" Visible="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSlno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesignation" runat="server" Text='<%#Bind("Designation") %>' Style="padding-left: 7px;"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                            <br />
                                            <asp:Label ID="lblEmplist" runat="server" Text="" Visible="false"></asp:Label><br />
                                            <asp:GridView ID="GvEmpList" runat="server" AutoGenerateColumns="False" GridLines="None" CellPadding="10" Style="margin: 0px auto; margin-top: 10px;" Visible="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSlno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Emp ID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpid" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Emp Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmpName" runat="server" Text='<%#Bind("empname") %>' Style="padding-left: 7px;"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesignation" runat="server" Text='<%#Bind("Designation") %>' Style="padding-left: 7px;"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>

                                    <asp:HiddenField ID="hidGridView" runat="server" />
                                    <asp:GridView ID="gvattendancezero" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-BackColor="BlueViolet"
                                        EmptyDataRowStyle-BorderColor="Aquamarine" EmptyDataText="No Records Found" Width="100%"
                                        CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvattendancezero_PageIndexChanging"
                                        ShowFooter="true">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <EmptyDataRowStyle BackColor="LightSkyBlue" BorderColor="Aquamarine" Font-Italic="false"
                                            Font-Bold="true" />
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
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" Checked="true" onclick="checkAll(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkindividual" runat="server" Checked="true" onclick="Check_Click(this)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 2--%>
                                            <asp:TemplateField HeaderText="Client ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                                <HeaderStyle Width="15px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 3--%>
                                            <asp:TemplateField HeaderText="Client Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px">
                                                <HeaderStyle Width="15px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("clientname") %>'> </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 4--%>
                                            <asp:TemplateField HeaderText="Old ID">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblAreaName" Text='<%# Bind("OldEmpid") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalAreaName"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 5--%>
                                            <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblempid" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 6--%>
                                            <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblempname" runat="server" Text='<%#Bind("EmpMname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>


                                            <%-- 7--%>
                                            <asp:TemplateField HeaderText="Bank A/C No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbankno" runat="server" Text='<%# Eval("EmpBankAcNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 8--%>
                                            <asp:TemplateField HeaderText="Desgn" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesgn" runat="server" Text='<%#Bind("Desgn") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 9--%>
                                            <asp:TemplateField HeaderText="Month-Year" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmonth" runat="server" Text='<%#Bind("Monthname") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 10--%>
                                            <asp:TemplateField HeaderText="Duties" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldutyhrs" runat="server" Text='<%#Bind("NoOfDuties") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDuties"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 11--%>
                                            <asp:TemplateField HeaderText="GW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOts" runat="server" Text='<%#Bind("OTs") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOTs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 12--%>
                                            <asp:TemplateField HeaderText="OTHrs" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNPCl25Per" runat="server" Text='<%#Bind("OTHrs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNPCl25Per"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 13--%>
                                            <asp:TemplateField HeaderText="WO" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwos" runat="server" Text='<%#Bind("WO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalwos"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 14--%>
                                            <asp:TemplateField HeaderText="Nhs" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNhs" runat="server" Text='<%#Bind("NHS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNhs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 15--%>
                                            <asp:TemplateField HeaderText="PL Days" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNpots" runat="server" Text='<%#Bind("PLdays") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNpots"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 16--%>
                                            <asp:TemplateField HeaderText="Sal Rate" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltempgross" runat="server" Text='<%#Bind("TempGross") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotaltempgross"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 17--%>
                                            <asp:TemplateField HeaderText="Basic" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%-- <asp:Label ID="lblbasic" runat="server" Text='<%#Bind("basic") %>'>--%>
                                                    <asp:Label ID="lblbasic" runat="server" Text='<%#Eval("basic", "{0:0}") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalBasic"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 18--%>
                                            <asp:TemplateField HeaderText="DA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblda" runat="server" Text='<%#Eval("da","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 19--%>
                                            <asp:TemplateField HeaderText="HRA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblhra" runat="server" Text='<%#Bind("hra","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalHRA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 20--%>
                                            <asp:TemplateField HeaderText="CCA" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcca" runat="server" Text='<%#Bind("CCa","{0:0}") %>'>  
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCCA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 21--%>
                                            <asp:TemplateField HeaderText="Conv" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConveyance" runat="server" Text='<%#Bind("conveyance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalConveyance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 22--%>
                                            <asp:TemplateField HeaderText="W.A." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwashallowance" runat="server" Text='<%#Bind("WashAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalWA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 23--%>
                                            <asp:TemplateField HeaderText="O.A." ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtherallowance" runat="server" Text='<%#Bind("OtherAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOA"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 24--%>
                                            <asp:TemplateField HeaderText="Special AllW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSpecialAllowance" runat="server" Text='<%#Bind("SpecialAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalSpecialAllowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 25--%>
                                            <asp:TemplateField HeaderText="Uniform AllW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUniformAllw" runat="server" Text='<%#Bind("UniformAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalUniformAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 26--%>
                                            <asp:TemplateField HeaderText="Arrears" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMobileAllowance" runat="server" Text='<%#Bind("Arrears","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalMobileAllowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 27--%>
                                            <asp:TemplateField HeaderText="Medical Re-imbursement" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblmedicalallowance" runat="server" Text='<%#Bind("medicalallowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalmedicalallowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 28--%>
                                            <asp:TemplateField HeaderText="Food Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFoodAllowance" runat="server" Text='<%#Bind("FoodAllowance","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalFoodAllowance"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 29--%>
                                            <asp:TemplateField HeaderText="Performance Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNightShiftAllw" runat="server" Text='<%#Bind("PerformanceAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNightShiftAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 30--%>
                                            <asp:TemplateField HeaderText="Travel Allw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTravelAllw" runat="server" Text='<%#Bind("TravelAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTravelAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 31--%>
                                            <asp:TemplateField HeaderText="Adv L.W" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLeaveEncashAmt" runat="server" Text='<%#Bind("LeaveEncashAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalLeaveEncashAmt"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 32--%>
                                            <asp:TemplateField HeaderText="Adv Gratuity" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGratuity" runat="server" Text='<%#Bind("Gratuity","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalGratuity"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 33--%>
                                            <asp:TemplateField HeaderText="Adv Bonus" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBonus" runat="server" Text='<%#Bind("Bonus","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalBonus"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 34--%>
                                            <asp:TemplateField HeaderText="Nfhs" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNfhs" runat="server" Text='<%#Bind("Nfhs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNfhs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 35--%>
                                            <asp:TemplateField HeaderText="RC" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrc" runat="server" Text='<%#Bind("rc","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalrc"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 36--%>
                                            <asp:TemplateField HeaderText="CS" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcs" runat="server" Text='<%#Bind("cs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalcs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 37--%>
                                            <asp:TemplateField HeaderText="Reimbursement" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIncentivs" runat="server" Text='<%#Bind("Incentivs","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalIncentivs"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 38--%>
                                            <asp:TemplateField HeaderText="WO Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWoAmt" runat="server" Text='<%#Bind("WOAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalWOAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 39--%>
                                            <asp:TemplateField HeaderText="NHs Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNhsAmt" runat="server" Text='<%#Bind("Nhsamt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNhsAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 40--%>
                                            <asp:TemplateField HeaderText="NPOTs Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNpotsAmt" runat="server" Text='<%#Bind("Npotsamt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNpotsAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 41--%>
                                            <asp:TemplateField HeaderText="Att Bonus" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttBonus" runat="server" Text='<%#Bind("AttBonus","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalAttBonus"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 42--%>
                                            <asp:TemplateField HeaderText="Service Weightage" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransport6Per" runat="server" Text='<%#Bind("ServiceWeightage","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTransport6Per"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 43--%>
                                            <asp:TemplateField HeaderText="Addl Amount" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddlAmount" runat="server" Text='<%#Bind("AddlAmount","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lbltTotalAddlAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 44--%>
                                            <asp:TemplateField HeaderText="ADDL4HR" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblADDL4HR" runat="server" Text='<%#Bind("ADDL4HR","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalADDL4HR"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 45--%>
                                            <asp:TemplateField HeaderText="QTRALLOW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQTRALLOW" runat="server" Text='<%#Bind("QTRALLOW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalQTRALLOW"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 46--%>
                                            <asp:TemplateField HeaderText="RELALLOW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRELALLOW" runat="server" Text='<%#Bind("RELALLOW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalRELALLOW"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 47--%>
                                            <asp:TemplateField HeaderText="SITEALLOW" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSITEALLOW" runat="server" Text='<%#Bind("SITEALLOW","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalSITEALLOW"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 48--%>
                                            <asp:TemplateField HeaderText="GunAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGunAllw" runat="server" Text='<%#Bind("GunAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalGunAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 49--%>
                                            <asp:TemplateField HeaderText="FireAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFireAllw" runat="server" Text='<%#Bind("FireAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalFireAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 50--%>
                                            <asp:TemplateField HeaderText="TelephoneAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTelephoneAllw" runat="server" Text='<%#Bind("TelephoneAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTelephoneAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 51--%>
                                            <asp:TemplateField HeaderText="Reimbursement" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReimbursement" runat="server" Text='<%#Bind("Reimbursement","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalReimbursement"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 52--%>
                                            <asp:TemplateField HeaderText="HardshipAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHardshipAllw" runat="server" Text='<%#Bind("HardshipAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalHardshipAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 53--%>
                                            <asp:TemplateField HeaderText="PaidHolidayAllw" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidHolidayAllw" runat="server" Text='<%#Bind("PaidHolidayAllw","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPaidHolidayAllw"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 54--%>
                                            <asp:TemplateField HeaderText="Gross" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGross" runat="server" Text='<%#Bind("Gross","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalGross"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 55--%>
                                            <asp:TemplateField HeaderText="GWR" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOTAmt" runat="server" Text='<%#Bind("OTAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalOTAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 56--%>

                                            <asp:TemplateField HeaderText="OTHrs Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransport" runat="server" Text='<%#Bind("OTHrsAmt","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTransport"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 57--%>
                                            <asp:TemplateField HeaderText="PF" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPF" runat="server" Text='<%#Bind("PF","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPF"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 58--%>
                                            <asp:TemplateField HeaderText="ESI" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblESI" runat="server" Text='<%#Bind("ESI","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalESI"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 59--%>
                                            <asp:TemplateField HeaderText="ProfTax" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProfTax" runat="server" Text='<%#Bind("ProfTax","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalProfTax"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 60--%>
                                            <asp:TemplateField HeaderText="Sal.Adv" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblsaladv" runat="server" Text='<%#Bind("SalAdvDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalsaladv"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 61--%>
                                            <asp:TemplateField HeaderText="MIS Ded" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbladvded" runat="server" Text='<%#Bind("ADVDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotaladvded"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 62--%>
                                            <asp:TemplateField HeaderText="WC Ded" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwcded" runat="server" Text='<%#Bind("WCDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalwcded"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 63--%>
                                            <asp:TemplateField HeaderText="U.D." ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluniform" runat="server" Text='<%#Bind("UniformDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalUniformDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 64--%>
                                            <asp:TemplateField HeaderText="Others" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtherDed" runat="server" Text='<%#Bind("OtherDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalOtherDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 65--%>
                                            <asp:TemplateField HeaderText="Total Loan ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltotalloanded" runat="server" Text='<%#Bind("LoanDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotaltotalloanded"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 66--%>
                                            <asp:TemplateField HeaderText="C.A" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcantadv" runat="server" Text='<%#Bind("CanteenAdv","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalcantadv"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 67--%>
                                            <asp:TemplateField HeaderText="Sec Dep" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSecDepDed" runat="server" Text='<%#Bind("SecurityDepDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalSecDepDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 68--%>
                                            <asp:TemplateField HeaderText="Gen Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGeneralDed" runat="server" Text='<%#Bind("GeneralDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalGeneralDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 69--%>
                                            <asp:TemplateField HeaderText="LWF" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblowf" runat="server" Text='<%#Bind("OWF","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalowf"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 70--%>
                                            <asp:TemplateField HeaderText="Advance" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPenalty" runat="server" Text='<%#Bind("Penalty","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalPenalty"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 71--%>
                                            <asp:TemplateField HeaderText="ATM Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRentDed" runat="server" Text='<%#Bind("ATMDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalRentDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 72--%>
                                            <asp:TemplateField HeaderText="Medical Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMedicalDed" runat="server" Text='<%#Bind("MedicalDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalMedicalDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 73--%>
                                            <asp:TemplateField HeaderText="MLWF Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMLWFDed" runat="server" Text='<%#Bind("MLWFDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalMLWFDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 74--%>
                                            <asp:TemplateField HeaderText="Food Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFoodDed" runat="server" Text='<%#Bind("FoodDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalFoodDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 75--%>
                                            <asp:TemplateField HeaderText="IDCard Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblElectricityDed" runat="server" Text='<%#Bind("IDCardDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalElectricityDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 76--%>
                                            <asp:TemplateField HeaderText="Rent Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransportDed" runat="server" Text='<%#Bind("RentDed1","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalTransportDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 77--%>
                                            <asp:TemplateField HeaderText="Other Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDccDed" runat="server" Text='<%#Bind("Finesded1","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalDccDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 78--%>
                                            <asp:TemplateField HeaderText="PVC Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLeaveDed" runat="server" Text='<%#Bind("PVCDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalLeaveDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 79--%>
                                            <asp:TemplateField HeaderText="License Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLicenseDed" runat="server" Text='<%#Bind("LicenseDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalLicenseDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 80--%>
                                            <asp:TemplateField HeaderText="Admin Chrg" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdv4Ded" runat="server" Text='<%#Bind("Adv4Ded","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalAdv4Ded"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 81--%>
                                            <asp:TemplateField HeaderText="TDS Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNightRoundDed" runat="server" Text='<%#Bind("TDSDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalNightRoundDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 82--%>
                                            <asp:TemplateField HeaderText="ManpowerMob Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblManpowerMobDed" runat="server" Text='<%#Bind("ManpowerMobDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalManpowerMobDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 83--%>
                                            <asp:TemplateField HeaderText="Mobileusage Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMobileusageDed" runat="server" Text='<%#Bind("MobileusageDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalMobileusageDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 84--%>
                                            <asp:TemplateField HeaderText="MediClaim Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMediClaimDed" runat="server" Text='<%#Bind("MediClaimDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalMediClaimDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <%-- 85--%>
                                            <asp:TemplateField HeaderText="Crisis Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrisisDed" runat="server" Text='<%#Bind("CrisisDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalCrisisDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 86--%>
                                            <asp:TemplateField HeaderText="Telephone Bill Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTelephoneBillDed" runat="server" Text='<%#Bind("TelephoneBillDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalTelephoneBillDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>


                                            <%-- 87--%>
                                            <asp:TemplateField HeaderText="Registration Fee" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationFee" runat="server" Text='<%#Bind("RegistrationFee","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalRegistrationFee"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>



                                            <%-- 88--%>
                                            <asp:TemplateField HeaderText="Total Ded" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeductions" runat="server" Text='<%#Bind("TotalDeductions","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDeductions"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%--89--%>
                                            <asp:TemplateField HeaderText="Net Amt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblnetamount" runat="server" Text='<%#Bind("ActualAmount","{0:0}") %>'> </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalNetAmount"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                    <br />

                                    <asp:GridView ID="GVNewpaysheet" runat="server" AutoGenerateColumns="true" OnRowDataBound="GVNewpaysheet_RowDataBound" GridLines="None" CellPadding="10" Style="margin: 0px auto; margin-top: 10px;" Visible="false">
                                        <Columns>
                                        </Columns>

                                    </asp:GridView>
                                </div>
                                <div style="margin-left: 550px; margin-top: 150px; display: none">
                                    <asp:Label ID="lblpayment" runat="server" Text="Total Amount For This Month" Style="color: Red"></asp:Label>
                                    &nbsp; &nbsp; &nbsp;
                                    <asp:Label ID="lblamount" runat="server" Text=""></asp:Label>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <br />
                                    <asp:Label ID="lbltotaldesignationlist" runat="server"></asp:Label>
                                </div>
                                <!-- DASHBOARD CONTENT END -->
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <!-- DASHBOARD CONTENT END -->
        </div>
</asp:Content>
