<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientBilling.aspx.cs" MasterPageFile="~/Module_Clients/Clients.master" Inherits="Kuduma.Portal.ClientBilling" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <script type="text/javascript" src="script/jscript.js">
    </script>

    <script type="text/javascript">

        function pageLoad(sender, args) {
            if (!args.get_isPartialLoad()) {
                //  add our handler to the document's
                //  keydown event
                $addHandler(document, "keydown", onKeyDown);
            }
        }

        function dtval(d, e) {
            var pK = e ? e.which : window.event.keyCode;
            if (pK == 8) { d.value = substr(0, d.value.length - 1); return; }
            var dt = d.value;
            var da = dt.split('/');
            for (var a = 0; a < da.length; a++) { if (da[a] != +da[a]) da[a] = da[a].substr(0, da[a].length - 1); }
            if (da[0] > 31) { da[1] = da[0].substr(da[0].length - 1, 1); da[0] = '0' + da[0].substr(0, da[0].length - 1); }
            if (da[1] > 12) { da[2] = da[1].substr(da[1].length - 1, 1); da[1] = '0' + da[1].substr(0, da[1].length - 1); }
            if (da[2] > 9999) da[1] = da[2].substr(0, da[2].length - 1);
            dt = da.join('/');
            if (dt.length == 2 || dt.length == 5) dt += '/';
            d.value = dt;
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
                select: function (event, ui) { $("#<%=ddlclientid.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlCname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
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
            $("#<%=ddlCname.ClientID %>").trigger('change');
        }


        $(function () {
            bindautofilldesgs();
        });
        var prmInstance = Sys.WebForms.PageRequestManager.getInstance();
        prmInstance.add_endRequest(function () {
            //you need to re-bind your jquery events here
            bindautofilldesgs();
        });

        function bindautofilldesgs() {
            $(".txtautofilldesg").autocomplete({
                source: eval($("#hdDesignations").val()),
                minLength: 4
            });
        }

    </script>

    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #social div {
            display: block;
        }

        .HeaderStyle {
            text-align: Left;
        }


        .modalBackground {
            background-color: Gray;
            z-index: 10000;
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
            width:150px;
        }

        .PnlBackground {
            background-color: rgba(128, 128, 128,0.5);
            z-index: 10000;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">Clients Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">CLIENT BILLING</h2>
                        </div>

                        <div style="text-align: center">
                            <asp:Label ID="lblResult" runat="server" Text="" Style="color: Red"></asp:Label>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <!--  Content to be add here> -->
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="Scriptmanager1">
                                </asp:ScriptManager>


                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <table cellpadding="5" cellspacing="5" width="100%">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkalllist" runat="server" AutoPostBack="true" GroupName="Checked" Text="All Clients" OnCheckedChanged="chkalllist_CheckedChanged" />
                                                <asp:CheckBox ID="chklistformanual" runat="server" Text="All Clients" Visible="false" AutoPostBack="true" OnCheckedChanged="chklistformanual_CheckedChanged" />

                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkexcludelist" runat="server" Text="Exclude Generated Clients" GroupName="Checked" AutoPostBack="true" OnCheckedChanged="chkexcludelist_CheckedChanged"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Month<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged"
                                                    class="sdrop" Width="120px">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtmonth" runat="server" AutoPostBack="true" Width="120px"
                                                    OnTextChanged="txtmonthOnTextChanged" Visible="false"></asp:TextBox>
                                                &nbsp;&nbsp;
                                                            <asp:CheckBox ID="Chk_Month" runat="server" OnCheckedChanged="Chk_Month_CheckedChanged" AutoPostBack="true"
                                                                Text="Old" />
                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true"
                                                    Format="dd/MM/yyyy" TargetControlID="txtmonth"></cc1:CalendarExtender>
                                            </td>
                                            <td style="padding-left: 20px">Type</td>
                                            <td>
                                                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlType_OnSelectedIndexChanged"
                                                    class="sdrop" Width="120px">
                                                    <asp:ListItem>Normal</asp:ListItem>
                                                    <asp:ListItem>Manual</asp:ListItem>
                                                    <asp:ListItem>Arrears</asp:ListItem>
                                                    <asp:ListItem>Material</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr>

                                            <td>Client ID<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlclientid" runat="server" CssClass="ddlautocomplete chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged"
                                                    Width="120px">
                                                </asp:DropDownList>
                                            </td>

                                            <td style="padding-left: 20px">Client Name<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCname" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCname_OnSelectedIndexChanged"
                                                    Style="width: 355px">
                                                </asp:DropDownList>
                                            </td>

                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btngenratepayment"
                                                runat="server" class="btn save" Text="Generate Bill" OnClick="Btn_Genrate_Invoice_Click" Style="margin-left: 20px"
                                                OnClientClick='return confirm(" Are you sure you  want to  generate bill ?");' />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnFreeze" Visible="false"
                                                    runat="server" class="btn save" Text="Freeze" OnClick="btnFreeze_Click"
                                                    OnClientClick='return confirm(" Are you sure you want to freeze the bill ?");' />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnUnFreeze" Visible="false"
                                                    runat="server" class="btn save" Text="UnFreeze" OnClick="btnUnFreeze_Click"
                                                    OnClientClick='return confirm(" Are you sure you want to unfreeze the bill ?");' />

                                            
                                            </td>

                                            <td>
                                                <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" OnClick="btnSendMail_Click" Visible="false"
                                                    OnClientClick='return confirm("Are you sure you want to send mail ?");' Style="margin-left: 46px" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Year
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtyear" runat="server" Text="2013" Enabled="False" class="sinput"
                                                    Width="50px"></asp:TextBox>
                                            </td>

                                            <td colspan="3"></td>

                                        </tr>

                                        <tr>

                                            <td>
                                                <asp:Label ID="lblbilltype" runat="server" Text="Bill Type :" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbcreatebill" runat="server" Text="Create" GroupName="MB" Checked="true" Visible="false" />
                                                <asp:RadioButton ID="rdbmodifybill" runat="server" Text="Modify" GroupName="MB" Visible="false" />
                                            </td>

                                            <td style="padding-left: 20px">
                                                <asp:Label ID="lblManualBillNo" runat="server" Text="Manual Billing Bill No's" Visible="false"></asp:Label>

                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList ID="ddlMBBillnos" runat="server" OnSelectedIndexChanged="ddlMBBillnos_OnSelectedIndexChanged"
                                                    AutoPostBack="true" Width="150px" CssClass="sdrop" Visible="false">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>

                                        <tr>

                                            <td>From
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtfromdate" runat="server" Enabled="true" class="sinput" Width="80px"
                                                    onkeyup="dtval(this,event)"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtfromdate_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtfromdate" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                            </td>
                                            <td style="padding-left: 20px">
                                                <asp:Label ID="Label2" runat="server" Text="To "></asp:Label></td>

                                            <td style="padding-left: 58px">
                                                <asp:TextBox ID="txttodate" runat="server" Enabled="true" class="sinput" Width="80px" Style="margin-left: -60px"
                                                    onkeyup="dtval(this,event)"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txttodate_Calender" runat="server" Enabled="true" TargetControlID="txttodate"
                                                    Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lblattndance" runat="server" Text="Go to Attendance" Visible="false"></asp:LinkButton>
                                            </td>
                                        </tr>



                                        <tr>
                                            <td>
                                                <asp:Label ID="lblbillnolatesttest" runat="server" Style="font-weight: bold;" Text="BillNo :"> </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblbillnolatest" runat="server" Style="font-weight: bold;" Text=""> </asp:Label>
                                            </td>
                                            <td style="padding-left: 20px">
                                                <asp:Label ID="lblbilldate" runat="server" Text="Bill Date :" Style="font-weight: bold;"></asp:Label>
                                            </td>


                                            <td>
                                                <asp:TextBox ID="txtbilldate" runat="server" Text="" class="sinput" Width="80px" onkeyup="dtval(this,event)"> </asp:TextBox>
                                                <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                    TargetControlID="txtbilldate" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBEstartdate" runat="server" Enabled="True" TargetControlID="txtbilldate"
                                                    ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>

                                        <tr runat="server">
                                            <td>
                                              <asp:Label ID="lblnatureOfSupply" runat="server" Text="Nature Of Supply" Style="font-weight: bold;"></asp:Label>
                                            </td>
                                            <td>
                                                 <asp:TextBox ID="txtnatureofsupply" runat="server" Text="" class="sinput" Width="80px"> </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPlaceOfSupply" runat="server" Text="Place Of Supply" Style="font-weight: bold;"></asp:Label>
                                            </td>
                                             <td>
                                                 <asp:TextBox ID="txtPlaceOfSupply" runat="server" Text="" class="sinput" Width="80px"> </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblJPCode" runat="server" Text="JP Code" Style="font-weight: bold;"></asp:Label>
                                            </td>
                                            <td>
                                                 <asp:TextBox ID="txtJPCode" runat="server" Text="" class="sinput" Width="80px"> </asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Due Date :" Visible="false" Style="font-weight: bold;"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtduedate" runat="server" Text="" Visible="false" class="sinput" Width="80px" onkeyup="dtval(this,event)"> </asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true"
                                                    TargetControlID="txtduedate" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBduedate" runat="server" Enabled="True" TargetControlID="txtduedate"
                                                    ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                            </td>

                                        </tr>

                                    </table>

                                </table>
                                <table width="70%" cellpadding="5" cellspacing="5">
                                </table>

                                <asp:Panel ID="PnlMaterialImport" runat="server" GroupingText="&nbsp;Import&nbsp;" Style="font-weight: bold" Visible="false">
                                    <table width="90%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>Import :
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkExportexcel" Text="Export Sample Excel" runat="server" OnClick="lnkExportexcel_Click"></asp:LinkButton>
                                            </td>
                                            <td style="width: 50px"></td>
                                            <td>Select File :
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="fileupload1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnImport" runat="server" Text="Import Data" class=" btn save" OnClick="lnkImportexcel_Click" />
                                            </td>

                                        </tr>
                                    </table>
                                </asp:Panel>

                                <table width="50%" cellpadding="5" cellspacing="5" style="margin-left: 17px; visibility: hidden">
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="linkmanualbilling" runat="server" Text="Manual Bills" PostBackUrl="~/Manual Billing.aspx"></asp:LinkButton>
                                        </td>

                                        <td>
                                            <asp:LinkButton ID="LINKNEWMANUALBILLING" runat="server" Text="New Manual Bill Model" PostBackUrl="~/newmanualbill.aspx"></asp:LinkButton>
                                        </td>

                                    </tr>
                                </table>

                                &nbsp;
                                <asp:LinkButton ID="linkdelete" runat="server" Text="Delete Bills" Visible="false"></asp:LinkButton>
                                <div style="margin-left: 30px">
                                    <cc1:ModalPopupExtender ID="mpebilldelete" runat="server" TargetControlID="linkdelete"
                                        PopupControlID="pnlbilldeletedetails" CancelControlID="btncancel">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="pnlbilldeletedetails" runat="server" Width="400px" Style="background-color: Silver"
                                        Visible="false">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Enter Bill No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbillno" runat="server" Width="240px" AutoPostBack="true" OnTextChanged="txtbillno_OnTextChanged"> 
                                   
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <br />

                                                    <tr>
                                                        <td>Client Id
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtclientid" runat="server" Width="240px"> 
                                   
                                                            </asp:TextBox>
                                                        </td>
                                                        <tr>
                                                            <td>Client Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtclientname" runat="server" Width="240px"> 
                                   
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btndelelte" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <table style="margin-left: 150px">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btndelelte" runat="server" Text="Delete" CssClass="btn save" OnClientClick='return confirm(" Are you sure you  want to  delete bill ?");'
                                                        OnClick="btndelelte_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btncancel" runat="server" Text="Cancel/Close" CssClass="btn save"
                                                        Width="95px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>

                                &nbsp;
                                 <%--   <cc1:ModalPopupExtender ID="ModalFreezeDetails" runat="server" TargetControlID="btnadminUnFreeze" PopupControlID="pnlFreeze"
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

                                    <cc1:ModalPopupExtender ID="modelLogindetails" runat="server" TargetControlID="btnSubmit" PopupControlID="pnllogin"
                                        BackgroundCssClass="modalBackground">
                                    </cc1:ModalPopupExtender>

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

                                <table style="width: 5%; float: right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btncleardata" runat="server" Text="Clear" OnClick="btncleardata_Click" />
                                        </td>
                                    </tr>
                                </table>


                                <asp:HiddenField ID="hdDesignations" runat="server" ViewStateMode="Enabled" />


                                <div class="rounded_corners" style="overflow: auto; width: 99%; margin-left: 17px">
                                    <asp:GridView ID="gvClientBilling" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-BackColor="BlueViolet"
                                        EmptyDataRowStyle-BorderColor="Aquamarine" Width="99%" CellPadding="4" CellSpacing="3"
                                        ForeColor="#333333" GridLines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />


                                        <Columns>
                                            <%-- 0 --%>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>" Width="30px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 1  --%>
                                            <asp:TemplateField HeaderText="Location/Store Code" HeaderStyle-Width="40px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtempname" runat="server" placeholder="Location" Width="95%" Style="text-align: left"
                                                        Text='<%# Bind("Location") %>'> </asp:TextBox>

                                                    <asp:TextBox ID="txtstorecode" runat="server" placeholder="Store Code" Width="95%" Style="text-align: left"
                                                        Text='<%# Bind("storecode") %>'> </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 2 --%>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("Designid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lbltype" runat="server" Text='<%# Bind("type") %>' Visible="false"></asp:Label>
                                                    <asp:TextBox ID="lbldesgn" runat="server" Text='<%# Bind("Designation") %>' Width="95%" Enabled="false" CssClass="txtautofilldesg" AutoPostBack="True" OnTextChanged="lbldesgn_TextChanged"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender runat="server" ID="Ftbdesignid" TargetControlID="lbldesgn"
                                                        FilterMode="InvalidChars" InvalidChars="'"></cc1:FilteredTextBoxExtender>
                                                    <asp:Label ID="lblnoofdays" runat="server" Text='<%# Bind("Noofdays") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <%-- 3 --%>
                                            <asp:TemplateField HeaderText="HSN Number" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtHSNNumber" runat="server" Width="95%" Style="text-align: left"
                                                        Text='<%# Bind("HSNNumber") %>'> </asp:TextBox>

                                                    <asp:TextBox ID="txtUOM" runat="server" Visible="false" Text='<%# Bind("UOM") %>' Style="width: 50px"></asp:TextBox>

                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 4 --%>
                                            <asp:TemplateField HeaderText="No. of Emps " HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblnoofemployees" runat="server" Text='<%#Bind("NoofEmps")%>' Enabled="false" Width="95%"> </asp:TextBox>
                                                    <asp:Label ID="lblextra" runat="server" Text='<%# Bind("Extra") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 5 --%>
                                            <asp:TemplateField HeaderText="No.of Dts/Hrs" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblNoOfDuties" runat="server" Text='<%#Bind("DutyHours")%>' Enabled="false" Width="95%"> </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 6 --%>
                                            <asp:TemplateField HeaderText="Pay Rate" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblpayrate" runat="server" Text='<%#Eval("payrate", "{0:0.##}")%>' Enabled="false" Width="95%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBPayRate" runat="server" Enabled="True"
                                                        TargetControlID="lblpayrate" ValidChars="-0123456789."></cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <%-- 7 --%>
                                            <asp:TemplateField HeaderText="New Pay Rate" Visible="false" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNewPayRate" runat="server" Style="text-align: center"
                                                        Text='<%#Eval("newpayrate", "{0:0.##}")%>' Width="95%"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBNewPayRate" runat="server" Enabled="True"
                                                        TargetControlID="txtNewPayRate" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 8 --%>
                                            <asp:TemplateField HeaderText="Duties Type" Visible="false" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddldutytype" runat="server" Width="95%">
                                                        <asp:ListItem Value="0">P.M</asp:ListItem>
                                                        <asp:ListItem Value="1">P.D</asp:ListItem>
                                                        <asp:ListItem Value="2">P.Hr</asp:ListItem>
                                                        <asp:ListItem Value="3">P.Sft</asp:ListItem>
                                                        <asp:ListItem Value="4">Fixed</asp:ListItem>
                                                        <asp:ListItem Value="5">Heading</asp:ListItem>
                                                        <asp:ListItem Value="6">P.M(8Hrs)</asp:ListItem>
                                                        <asp:ListItem Value="7">Qty</asp:ListItem>

                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 9 --%>
                                            <asp:TemplateField HeaderText="NOD" HeaderStyle-Width="50px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlnod" runat="server" AppendDataBoundItems="True" Width="95%">
                                                        <asp:ListItem Value="22" Selected="True">22</asp:ListItem>
                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                        <asp:ListItem Value="24">24</asp:ListItem>
                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                        <asp:ListItem Value="26">26</asp:ListItem>
                                                        <asp:ListItem Value="27">27</asp:ListItem>
                                                        <asp:ListItem Value="28">28</asp:ListItem>
                                                        <asp:ListItem Value="29">29</asp:ListItem>
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                        <asp:ListItem Value="31">31</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 10 --%>
                                            <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblda" runat="server" Text='<%#Eval("BasicDa", "{0:0.##}")%>' Enabled="false" Width="95%"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBDa" runat="server" Enabled="True"
                                                        TargetControlID="lblda" ValidChars="-0123456789."></cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 11 --%>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblAmount" runat="server" Text='<%#Eval("BasicDa", "{0:0.##}")%>' Enabled="false" Width="99%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 12 --%>
                                            <asp:TemplateField HeaderText="GST %" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblGSTper" runat="server" Text='<%#Eval("GSTper", "{0:0.##}")%>' Width="99%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <%-- 13 --%>
                                            <asp:TemplateField HeaderText="CGST">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblCGSTAmount" runat="server" Text='<%#Eval("CGSTAmt", "{0:0.##}")%>' Enabled="false" Width="50px"></asp:TextBox>
                                                    <asp:TextBox ID="lblCGSTPrc" runat="server" Text='<%#Eval("CGSTPrc", "{0:0.##}")%>' Enabled="false" Visible="false"></asp:TextBox>

                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 14 --%>
                                            <asp:TemplateField HeaderText="SGST">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblSGSTAmount" runat="server" Text='<%#Eval("SGSTAmt", "{0:0.##}")%>' Enabled="false" Width="50px"></asp:TextBox>
                                                    <asp:TextBox ID="lblSGSTPrc" runat="server" Text='<%#Eval("SGSTPrc", "{0:0.##}")%>' Enabled="false" Visible="false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 15 --%>
                                            <asp:TemplateField HeaderText="IGST">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblIGSTAmount" runat="server" Text='<%#Eval("IGSTAmt", "{0:0.##}")%>' Enabled="false" Width="50px"></asp:TextBox>
                                                    <asp:TextBox ID="lblIGSTPrc" runat="server" Text='<%#Eval("IGSTPrc", "{0:0.##}")%>' Enabled="false" Visible="false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 16 --%>
                                            <asp:TemplateField HeaderText="Total Amt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblTotalTaxmount" runat="server" Text='<%#Eval("TotalTaxAmount", "{0:0.##}")%>' Enabled="false" Width="80px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 17 --%>
                                            <asp:TemplateField HeaderText="OT Amount" HeaderStyle-Width="70px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtAmount" runat="server" Text='<%#Eval("OTAmount", "{0:0.##}")%>' Width="70px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="10px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkExtra" runat="server" Width="10px" Enabled="false" Style="text-align: center"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>

                                        <EmptyDataRowStyle BackColor="BlueViolet" BorderColor="Aquamarine"></EmptyDataRowStyle>

                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>
                                <div>
                                    <asp:Button ID="btnAddNewRow" runat="server" Text="Add Row" Visible="false" OnClick="btnAddNewRow_Click" />
                                    <asp:Button ID="btnCalculateTotals" runat="server" Text="Calculate Totals" Visible="false"
                                        OnClick="btnCalculateTotals_Click" />

                                </div>

                                <table width="100%" cellpadding="5" cellspacing="5" style="margin-left: 17px">
                                    <tr>
                                        <td valign="top" width="37%">
                                            <asp:CheckBox ID="checkExtraData" Visible="false" Text="&nbsp;&nbsp;Extra Data for Billing" runat="server"
                                                Checked="false" AutoPostBack="True" OnCheckedChanged="checkExtraData_CheckedChanged" />
                                            <asp:Panel ID="panelRemarks" runat="server" Visible="false">
                                                <table width="100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td>GST
                                                        </td>
                                                        <td>Service Charge
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtmachinarycost" runat="server" Text="Machinery Cost :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMachinery" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesMachinary" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtMachinery" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesMachinary" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtMaterialcost" runat="server" Text="Material Cost :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMaterial" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesMaterial" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtMaterial" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesMaterial" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtMaintanancecost" runat="server" Text="Maintenance Work :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtElectical" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesElectrical" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesElectrical" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtElectical" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtextraonetitle" runat="server" Text="Extra Amount one :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtextraonevalue" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesExtraone" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesExtraone" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                            TargetControlID="txtextraonevalue" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtextratwotitle" runat="server" Height="19px" Text="Extra Amount Two :"
                                                                class="sinput" Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtextratwovalue" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesExtratwo" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesExtratwo" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                            TargetControlID="txtextratwovalue" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtdiscount" runat="server" Text="Discounts :" class="sinput" Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDiscounts" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtDiscounts" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTDiscountone" runat="server" Checked="false" Text=" Before Service Tax" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtdiscounttwotitle" runat="server" Text="Discount Two:" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtdiscounttwovalue" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                TargetControlID="txtdiscounttwovalue" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTDiscounttwo" runat="server" Checked="false" Text=" Before Service Tax" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td>Remarks :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRemarks" runat="server" Text="" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td align="right" valign="top">
                                            <table width="70%" cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblRelChrTitle" Visible="false" Text=" 1/6 Reliever Charges : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblRelChrgAmt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblBPFTitle" Visible="false" Text=" PF " runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtPfPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblBPFAmt" Text="" Visible="false" runat="server"></asp:Label>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblBESiTitle" Visible="false" Text="ESI  " runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtEsiPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblBESiAmt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lbltotal" Visible="false" Text="Total:" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblTotalResources" Text="" runat="server" Visible="false" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>



                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblServiceChargeTitle" Visible="false" Text=" Service Charges : " runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtservicechrgPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblServiceCharges" Text="" Visible="false" Enabled="false" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblStaxamtonServicechargetitle" Visible="false" Text=" Service Tax on Service Charges : " runat="server"></asp:Label></td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblStaxamtonServicecharge" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSChargeamtonMachinarytitle" Visible="false" Text=" Service Charge on Machinary Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSChargeamtonMachinary" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaintenancetitle" Visible="false" Text=" Service Charge on Maintenance Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaintenance" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaterialtitle" Visible="false" Text=" Service Charge on Material Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaterial" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtraonetitle" Visible="false" Text=" Service Charge on Extra amount one : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtraone" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtratwotitle" Visible="false" Text=" Service Charge on Extra amount two : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtratwo" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMachineryTitlewithst" Visible="false" Text=" Machinery Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMachinerywithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMaterialTitlewithst" Visible="false" Text=" Material Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMaterialwithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblElectricalTitlewithst" Visible="false" Text=" Maintenance Work : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblElectricalwithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextraonetitlewithst" Visible="false" Text="Extra Amount One : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextraonewithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextratwotitlewithst" Visible="false" Text="Extra Amount Two : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextratwowithst" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscountTitlewithst" Visible="false" Text="Discount : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscountwithst" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwotitlewithst" Visible="false" Text="Discount Two: " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwowithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <asp:Label ID="lblTotalbeforeTax" Visible="false" Text="Total Before Tax :" runat="server"></asp:Label>
                                                    <asp:TextBox ID="TxtTotalbeforeTax" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblServiceTaxTitle" Visible="false" Text="Service Tax :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtServiceTaxPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblServiceTax" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSBCESSTitle" Visible="false" Text="SB CESS :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtSBCESSPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblSBCESS" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblKKCESSTitle" Visible="false" Text="KK CESS :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtKKCESSPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblKKCESS" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <%-- region for GST as on 17-6-2017 by swathi--%>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCGSTTitle" Visible="false" Text="CGST :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblCGST" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSGSTTitle" Visible="false" Text="SGST :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtSGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblSGST" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblIGSTTitle" Visible="false" Text="IGST :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtIGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblIGST" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCess1Title" Visible="false" Text="Cess1 :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCess1Prc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblCess1" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCess2Title" Visible="false" Text="Cess2 :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCess2Prc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblCess2" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <%-- endregion for GST as on 17-6-2017 by swathi--%>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCESSTitle" Visible="false" Text="CESS :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCESSPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblCESS" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSheCESSTitle" Visible="false" Text="S&H Ed. CESS :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtSheCESSPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblSheCESS" Visible="false" Text="" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblST75Title" Visible="false" Text="Less 75% Service Tax :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblST75" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblST25Title" Visible="false" Text="Service Tax Chargable @3.09%:" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblST25" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMachineryTitle" Visible="false" Text=" Machinery Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMachinery" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMaterialTitle" Visible="false" Text=" Material Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMaterial" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblElectricalTitle" Visible="false" Text=" Maintenance Work : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblElectrical" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextraoneamttitle" Visible="false" Text="Extra Amount One : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextraamt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextratwoamttitle" Visible="false" Text="Extra Amount Two : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextratwoamt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscountTitle" Visible="false" Text="Discount : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscount" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwoTitle" Visible="false" Text="Discount Two: " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwo" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right; font-weight: bold">
                                                        <asp:Label ID="lblgrandtotalss" Text="Grand Total :" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right; font-weight: bold">
                                                        <asp:TextBox ID="lblGrandTotal" Text="" runat="server" Visible="false" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <asp:Label ID="lblRemarks" Text="" runat="server" Visible="false"></asp:Label>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>

                                                <div>
                                                    <asp:Label ID="lbltotalamount" runat="server"> </asp:Label>
                                                </div>
                                                <div style="width: 100%; font-weight: bold">
                                                    <asp:Label ID="lblamtinwords" Text="" runat="server" Visible="false"> </asp:Label>
                                                </div>
                                            </table>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkcopy" runat="server" Text="Copy" Width="50px" />
                                        </td>
                                        <td style="width: 50px">
                                            <asp:DropDownList ID="ddlfont" runat="server">
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>9</asp:ListItem>
                                                <asp:ListItem>8</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkletterhead" runat="server" Text="Letter Head" Width="80px" />
                                        </td>
                                        <td style="text-align: right; font-weight: bold; padding-left: 10px">
                                            <asp:Button ID="Button2" runat="server" Text="Invoice New" class="btn save" OnClick="btninvoiceDownload_Click" /><br />
                                        </td>

                                          <td style="text-align: right; font-weight: bold; padding-left: 10px">
                                            <asp:Button ID="Button1" runat="server" Text="Invoice Pdf (Manual)" Visible="false" class="btn save" OnClick="btnInvoicePdfDownload_Click" /><br />
                                        </td>

                                         <td style="text-align: right; font-weight: bold; padding-left: 10px">
                                            <asp:Button ID="Button3" runat="server" Text="Invoice Pdf (Manual)" class="btn save" OnClick="Button3_Click" /><br />
                                        </td>

                                        <td>
                                            <asp:Button ID="btnlifestyleBill" runat="server" Visible="false" Text="Invoice(Life Style)" OnClick="btnlifestyleBill_Click" />
                                        </td>
                                        <td style="text-align: right; font-weight: bold; padding-left: 10px">
                                            <asp:Button ID="btninvoiceemp" runat="server" Text="Invoice New(Emp)" class="btn save" Visible="false"
                                                OnClick="btninvoiceemp_Click" /><br />
                                        </td>
                                        <td style="text-align: right; font-weight: bold; padding-left: 10px">
                                            <asp:Button ID="Btnannexure" runat="server" Text="ANNEXURE" class="btn save" Visible="false" OnClick="Btnannexure_Click" /><br />
                                        </td>
                                    </tr>


                                    <tr>



                                        <td style="text-align: right; font-weight: bold">
                                            <asp:Button ID="btninvMaterial" runat="server" Text="Material" class="btn save" Visible="false" OnClick="btnMaterialInv_Click" /><br />
                                        </td>


                                    </tr>

                                </table>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
                <!-- DASHBOARD CONTENT END -->
            </div>
        </div>
        <!-- CONTENT AREA END -->
</asp:Content>
