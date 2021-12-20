<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="clientattendance.aspx.cs" MasterPageFile="~/Module_Clients/Clients.master" Inherits="Kuduma.Portal.clientattendance" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.js" type="text/javascript"></script>
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
    <style type="text/css">
        #social div
        {
            display: block;
        }
        .HeaderStyle
        {
            text-align: Left;
        }
        .style3
        {
            height: 24px;
        }
        
       
        
    </style>
    <style type="text/css">
         .modalBackground
            {
            background-color: Gray;
            z-index: 10000;
            }
        </style>

    <script type="text/javascript">

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
    </script>

    <style type="text/css">
        .slidingDiv
        {
            background-color: #99CCFF;
            padding: 10px;
            margin-top: 10px;
            border-bottom: 5px solid #3399FF;
        }
        .show_hide
        {
            display: none;
        }
    </style>    

    <script type="text/javascript">

        $(document).ready(function() {


            $(".slidingDiv").hide();
            $(".show_hide").show();

            $('.show_hide').click(function() {
                $(".slidingDiv").slideToggle();
            })
            if (isPostBack) { $(".slidingDiv").show(); }

        });

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
                            <h2 style="text-align: center">
                                CLIENT ATTENDANCE</h2>
                        </div>
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px; min-height: 200px; height: auto">
                            <!--  Content to be add here> -->
                            <div class="boxin">
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="95%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                <table width="100%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td width="110px">
                                                            Client ID<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                           <%-- <asp:DropDownList ID="ddlClientID" runat="server" class="sdrop" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>
                                             <asp:DropDownList ID="ddlClientID" runat="server" class="ddlautocomplete chosen-select" TabIndex="1" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Phone N0(s)
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtphonenumbers" runat="server" class="sinput" Enabled="False" AutoCompleteType="Disabled"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Month
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlMonth" class="sdrop" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="Txt_Month" runat="server" AutoPostBack="true" class="sinput"
                                                                OnTextChanged="Txt_Month_OnTextChanged" Text="" Visible="false"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtMonth_CalendarExtender" runat="server" 
                                                                Enabled="true" Format="dd/MM/yyyy" TargetControlID="Txt_Month">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="txtMonth_FilteredTextBoxExtender" 
                                                                runat="server" Enabled="True" TargetControlID="Txt_Month" 
                                                                ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:CheckBox ID="Chk_Month" runat="server" Text="Old" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table style="height: 100%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>
                                                            Client Name
                                                        </td>
                                                        <td>
                                                           <%-- <asp:DropDownList ID="ddlCName" runat="server" AutoPostBack="True" class="sdrop"
                                                                Width="305px" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>

                                                            <asp:DropDownList ID="ddlCName" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged"
                                                            Style="width: 355px">
                                                        </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Our Contact Person
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtocp" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            OT in terms of
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlOTType" class="sdrop">
                                                                <asp:ListItem>Days</asp:ListItem>
                                                                <asp:ListItem>Hours</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;Attendance Mode :
                                    <asp:RadioButton ID="radioall" runat="server" Text="All" AutoPostBack="true" GroupName="a"  Checked="true"
                                        OnCheckedChanged="radioindividual_CheckedChanged" />

                                    <asp:RadioButton ID="radioindividual" runat="server" Text="Individual" Visible="false" AutoPostBack="true"
                                        GroupName="a" OnCheckedChanged="radioindividual_CheckedChanged" />
                                    <%--  <asp:RadioButton ID="radioall" runat="server" Text="All" AutoPostBack="true" 
                                       GroupName="a" oncheckedchanged="radioindividual_CheckedChanged"/>
                              --%>
                                    
                                    <asp:RadioButton ID="radiospecialdays" runat="server" Text="Special Days" Visible="false"
                                        OnCheckedChanged="radioindividual_CheckedChanged" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                          <asp:LinkButton ID="lnkClear" runat="server" Text="Clear" Visible="false"
                                     
                                        OnClientClick='return confirm("Are you sure you want to Clear The Attendance . ?");' 
                                        onclick="lnkClear_Click" >
                                       </asp:LinkButton>
                                </div>
                                <br />
                                <div class="dashboard_firsthalf" style="width: 97%; margin-left: 15px;visibility:hidden"  >
                                    <a href="#" class="show_hide">
                                        <h2>
                                            </h2>
                                    </a>
                                    <div class="slidingDiv">
                                        <table width="100%" style="font-size: 13px">
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td>
                                                                Emp ID<span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlempid" class="sdrop" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlempid_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                            <tr>
                                                                <td>
                                                                    Order Date<span style="color: Red">*</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtorderdate" TabIndex="9" runat="server" class="sinput" MaxLength="10"
                                                                        onkeyup="dtval(this,event)"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CEorderdate" runat="server" Enabled="true" TargetControlID="txtorderdate"
                                                                        Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FTBEorderdate" runat="server" Enabled="True" TargetControlID="txtorderdate"
                                                                        ValidChars="/0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Joining Date<span style="color: Red">*</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtjoindate" TabIndex="9" runat="server" class="sinput" MaxLength="10"
                                                                        onkeyup="dtval(this,event)"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CEjoindate" runat="server" Enabled="true" TargetControlID="txtjoindate"
                                                                        Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FTBEjoindate" runat="server" Enabled="True" TargetControlID="txtjoindate"
                                                                        ValidChars="/0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Remarks
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtremarks" runat="server" class="sinput" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    PF
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkpf" runat="server" Checked="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    ESI
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkesi" runat="server" Checked="true" />
                                                                </td>
                                                            </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table width="100%" border="0" cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td width="90px">
                                                                <asp:Label ID="lblempname" runat="server" Text="Emp Name"> </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlempname" runat="server" class="sdrop" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlempname_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Order ID
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtorderid" class="sinput" runat="server" Enabled="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Previous Unit ID
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPrevUnitId" class="sinput" runat="server" Enabled="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDesignation" runat="server" Text="Emp Desig"> </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDesignation" runat="server" class="sdrop">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Relieving Date<span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtrelivingdate" TabIndex="9" runat="server" class="sinput" MaxLength="10"
                                                                    onkeyup="dtval(this,event)"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CErelivingdate" runat="server" Enabled="true" TargetControlID="txtrelivingdate"
                                                                    Format="dd/MM/yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="FTBErelivingdate" runat="server" Enabled="True"
                                                                    TargetControlID="txtrelivingdate" ValidChars="/0123456789">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                PT
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkpt" runat="server" Checked="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Transfer Type
                                                            </td>
                                                            <td class="style8">
                                                                <asp:DropDownList ID="ddlTransfertype" runat="server" class="sdrop">
                                                                     <asp:ListItem Value="1">PostingOrder</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">Temporary Transfer</asp:ListItem>
                                                                        <asp:ListItem Value="-1">Dumy Transfer</asp:ListItem>
                                                                </asp:DropDownList>
                                                                &nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="Button1" runat="server" Visible="true" Text="Transfer" class="btn save"
                                                                    OnClick="btntransfer_Click" OnClientClick='return confirm("Are you sure you want to give posting order to this employee?");' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <br />
 <cc1:ModalPopupExtender ID="modelLogindetails" runat="server" TargetControlID="Chk_Month" PopupControlID="pnllogin"
                            BackgroundCssClass="modalBackground">
                            </cc1:ModalPopupExtender>
                           
                             <asp:Panel ID="pnllogin" runat="server" Height="100px" Width="300px"  style="display:none;position:absolute; background-color:Silver;">
                             <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                             <ContentTemplate>
                            <table>
                            <tr>
                            <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            </tr>
                            <tr>
                            <td style="font:bold;font-size:medium">&nbsp;&nbsp;&nbsp;
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
                            <table style="background-position:center;">
                             <tr>
                             <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" class="btn Save"/>
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" class="btn Save" />
                            </td>
                            </tr>
                            </table>
                            </asp:Panel>
                                <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server"  UpdateMode="Always">
                                   <ContentTemplate>--%>
                                <div class="rounded_corners">
                                    <asp:GridView ID="GridView1" runat="server" Height="75px" Width="100%" Style="margin-left: 0px"
                                        AutoGenerateColumns="False" CellPadding="5" CellSpacing="3" ForeColor="#333333"
                                        BorderStyle="Solid" BorderColor="Black" BorderWidth="0" GridLines="None" HeaderStyle-CssClass="HeaderStyle"
                                        AllowPaging="True" 
                                        PageSize="400" ShowFooter="true" onrowdatabound="GridView1_RowDataBound">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpid" runat="server" Text=" <%#Bind('EmpId')%>" Style="text-align: center"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblName" Text=" <%#Bind('Empname')%>"    Width="180px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDesg" Text=" <%#Bind('DesignID')%>" Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="lbldesignname" Text=" <%#Bind('design')%>" Width="150px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duties" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDuties" Text=" <%#Bind('NoOfDuties')%>" Style="text-align: center" Width="40px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        TargetControlID="txtDuties" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OTs" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtOTs" Text=" <%#Bind('Ot')%>" Style="text-align: center" Width="40px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                        TargetControlID="txtOTs" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WOs" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtwos" Text=" <%#Bind('WO')%>" Style="text-align: center" Width="40px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEwos" runat="server" Enabled="True" TargetControlID="txtwos"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NHs" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtnhs" Text=" <%#Bind('NHS')%>" Style="text-align: center" Width="40px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEnhs" runat="server" Enabled="True" TargetControlID="txtnhs"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NPOTs" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtnpots"  Text=" <%#Bind('Npots')%>"  Style="text-align: center" Width="40px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEnpots" runat="server" Enabled="True" TargetControlID="txtnpots"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                 
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Canteen Adv." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtCanAdv" Text=" <%#Bind('CanteenAdv')%>" Style="text-align: center" Width="40px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                        TargetControlID="txtCanAdv" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                 
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penalty" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtPenalty" Text=" <%#Bind('Penalty')%>" Style="text-align: center" Width="40px" ></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                        TargetControlID="txtPenalty" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                 
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Incentive" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="70px" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtIncentivs" Text=" <%#Bind('Incentivs')%>"  Style="text-align: center" Width="70px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBE4" runat="server" Enabled="True" TargetControlID="txtIncentivs"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>
                                <%--    </ContentTemplate>
            <Triggers >
             <asp:AsyncPostBackTrigger ControlID="radioindividual" EventName="CheckedChanged"  />
            </Triggers>
            
        </asp:UpdatePanel>--%>
                                <%--
                <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Always">
                                   <ContentTemplate>--%>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvfromcontracts" runat="server" Height="75px" Width="100%" Style="margin-left: 0px"
                                        AutoGenerateColumns="False" CellPadding="5" CellSpacing="3" ForeColor="#333333"
                                        BorderStyle="Solid" BorderColor="Black" BorderWidth="0px" GridLines="None" HeaderStyle-CssClass="HeaderStyle"
                                        HeaderStyle-Height="10px" RowStyle-Height="8px" ShowFooter="true" 
                                        onrowdatabound="gvfromcontracts_RowDataBound">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left"  HeaderStyle-Width="50%">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbldesginid" Text="<%#Bind('Designid') %>" Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="lblDesignation" Text="<%#Bind('Design') %>" ></asp:Label>
                                                     <asp:Label runat="server" ID="lblType" Text="<%#Bind('type') %>" Visible="false"></asp:Label>
                                                
                                                </ItemTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="TxtQuantity" Text="<%#Bind('Quantity') %>" Style="text-align: center" Width="90%"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                <asp:Label ID="lblTotalQty" runat="server" ></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duties" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDuties" Text="<%#Bind('duties') %>" Style="text-align: center" Width="90%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                        TargetControlID="txtDuties" FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                    
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                <asp:Label ID="lblTotalDuties" runat="server" ></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duties(In Hours)" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDutiesInHours" Text="" AutoPostBack="true" Width="90%"
                                                     OnTextChanged="txtDutiesInHours_textChanged"
                                                        Style="text-align: center"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                        TargetControlID="txtDutiesInHours" FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                            
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                <asp:Label ID="lblTotalDutiesinhrs" runat="server"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OTs" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtOTs" Text="<%#Bind('OT') %>" Style="text-align: center" Width="90%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                        TargetControlID="txtOTs" FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                  
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                <asp:Label ID="lblTotalOts" runat="server" Text="" ></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>
                                <%--     
                                     </ContentTemplate>
            <Triggers  >
             <asp:AsyncPostBackTrigger ControlID="radioall" EventName="CheckedChanged"   />
            </Triggers>
            
        </asp:UpdatePanel>--%>
                                <%-- start Gridview for special days  --%>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvspecialdays" runat="server" Height="75px" Width="100%" Style="margin-left: 0px"
                                        AutoGenerateColumns="False" CellPadding="5" CellSpacing="3" ForeColor="#333333"
                                        BorderStyle="Solid" BorderColor="Black" BorderWidth="0px" GridLines="None" HeaderStyle-CssClass="HeaderStyle">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText=" Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmpid" runat="server" Text=" <%#Bind('EmpId')%>"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle />
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" ItemStyle-Width="180px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblName"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDesg" Text=" <%#Bind('Designation')%>"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle />
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duties" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDuties"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        TargetControlID="txtDuties" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OTs" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtOTs"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                        TargetControlID="txtOTs" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Canteen Adv." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtCanAdv"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                        TargetControlID="txtCanAdv" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Penalty" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtPenalty"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                        TargetControlID="txtPenalty" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle></ItemStyle>
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField>
                                       <ItemTemplate>  
                                       <asp:LinkButton ID="linkedit" runat="server" Text="Update" CommandName="update" > </asp:LinkButton>
                                       </ItemTemplate>
                                       <ItemStyle Width="80px"></ItemStyle>
                                       </asp:TemplateField>--%>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>
                                <%-- End Gridview for special days  --%>
                                <table width="100%">
                                    <tr>
                                        <td width="25%">
                                        </td>
                                        <td width="25%">
                                            <asp:Label ID="lblTotalDuties" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td width="25%">
                                            <asp:Label ID="lblTotalOts" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td width="25%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="25%">
                                        </td>
                                        <td width="25%">
                                        </td>
                                        <td width="25%">
                                        </td>
                                        <td colspan="4">
                                            <asp:Label ID="lbltotaldesignationlist" runat="server" Text="">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btn_Save_AttenDance" runat="server" Text="Save" class=" btn save"
                                                OnClick="btn_Save_AttenDanceClick" OnClientClick='return confirm(" Are you sure  you want to add this record ?");' />
                                            <asp:Button ID="Btn_Cancel_AttenDance" runat="server" class=" btn save" Text="Cancel"
                                                OnClientClick='return confirm(" Are you sure you  want to cancel this entry ?");'
                                                OnClick="Btn_Cancel_AttenDance_Click" />
                                            <br />
                                            <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"></asp:Label>
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
        </asp:content>
