<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewManualBill.aspx.cs" MasterPageFile="~/Module_Clients/Clients.master" Inherits="Kuduma.Portal.NewManualBill" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
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
        function bindautofilldesgs() { 
         $(".txtautofilldesg").autocomplete({
                source:eval($("#hdDesignations").val()),
                minLength: 4
          });
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
        .modalBackground
        {
            background-color: Gray;
            z-index: 10000;
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


    </script>
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">
                Manual Billing</h1>
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Manual Billing</h2>
                        </div>
                        <div style="text-align: center">
                            <asp:Label ID="lblResult" runat="server" Text="" Style="color: Red"></asp:Label>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="Scriptmanager1">
                                </asp:ScriptManager>
                                <table width="95%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            Client ID<span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlclientid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged"
                                                class="sdrop" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Client Name<span style="color: Red">*</span>
                                        </td>
                                        <td colspan="5">
                                            <asp:DropDownList ID="ddlCname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCname_OnSelectedIndexChanged"
                                                class="sdrop" Width="355px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Month<span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged"
                                                class="sdrop" Width="150px">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtmonth" runat="server" AutoPostBack="true" Width="126px" Height="19px" class="sdrop"
                                                            OnTextChanged="txtmonthOnTextChanged" Visible="false"></asp:TextBox>
                                                   &nbsp;&nbsp;&nbsp;  <asp:CheckBox ID="Chk_Month" runat="server" 
                                                            Text="Old" />
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true" 
                                                            Format="dd/MM/yyyy" TargetControlID="txtmonth">
                                                        </cc1:CalendarExtender>
                                             <cc1:ModalPopupExtender ID="modelLogindetails" runat="server" TargetControlID="Chk_Month" PopupControlID="pnllogin"
                                                BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>
                                        </td>
                                        <td>
                                            Year&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtyear" runat="server" Text="2015" Enabled="False" class="sinput"
                                                Width="50px"></asp:TextBox>
                                        </td>
                                        <td>
                                            From
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtfromdate" runat="server" Enabled="true" class="sinput" Width="80px"
                                                onkeyup="dtval(this,event)"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtfromdate_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtfromdate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            To
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttodate" runat="server" Enabled="true" class="sinput" Width="80px"
                                                onkeyup="dtval(this,event)"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txttodate_Calender" runat="server" Enabled="true" TargetControlID="txttodate"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Manual Billing Bill No's
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMBBillnos" runat="server" OnSelectedIndexChanged="ddlMBBillnos_OnSelectedIndexChanged"
                                                AutoPostBack="true" Width="150px" CssClass="sdrop">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbilldate" runat="server" Text="Bill Date :" Style="font-weight: bold;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbilldate" runat="server" Text="" class="sinput" Width="100px"
                                                onkeyup="dtval(this,event)"> </asp:TextBox>
                                            <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtbilldate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBEstartdate" runat="server" Enabled="True" TargetControlID="txtbilldate"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td colspan="5">
                                            <asp:Label ID="lblbillnolatesttest" runat="server" Style="font-weight: bold;" Text="BillNo :"> </asp:Label>
                                            <asp:Label ID="lblbillnolatest" runat="server" Style="font-weight: bold;" Text=""> </asp:Label>
                                            <asp:CheckBox ID="ChkWithDates" runat="server" Text=" With Dates" Checked="true" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Bill Type :
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbcreatebill" runat="server" Text="Create" GroupName="MB" Checked="true" />
                                            <asp:RadioButton ID="rdbmodifybill" runat="server" Text="Modify" GroupName="MB" />
                                        </td>
                                    </tr>
                                     <tr>
                                            <td>
                                            <h3 style="border:none;background:none;">Invoice Description</h3>
                                               
                                            </td>
                                            <td>
                                                  <asp:TextBox ID="txtdescription" runat="server" MaxLength="200" TabIndex="35"  Width="170px" Height="110px"
                                                    Text="We are presenting our bill for the Security Services provided at your establishment. Kindly release the payment at the earliest."
                                                    Style="font-variant: normal; padding: 10px" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                            </td>
                                        </tr>

                                </table>
                              
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
                                <br />
                                <asp:UpdatePanel ID="updatepanel" runat="server">
                                    <ContentTemplate>
                                <div class="rounded_corners" style="overflow: auto; width: 99%; padding: 0;">
                                    <asp:HiddenField ID="hdDesignations" runat="server" />
                                    <asp:GridView ID="gvClientBilling" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-BackColor="BlueViolet"
                                        EmptyDataRowStyle-BorderColor="Aquamarine" Width="100%" CellPadding="5" CellSpacing="5"
                                        ForeColor="#333333" GridLines="None"  >
                                        <RowStyle BackColor="#EFF3FB" Height="30" />
                                        <Columns>
                                           <%-- <asp:TemplateField>
                                            <ItemTemplate>
                                             <asp:LinkButton runat="server" ID="Delete" Text="Delete" CommandArgument='<%# Bind("Designation") %>' CommandName="Delete"  OnClientClick="return confirm('Are you sure you want to Delete this Record?');"
                                                 class="linkbuttons"></asp:LinkButton>
                                            </ItemTemplate>

                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Sl.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvSlno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtgvdesgn" runat="server" CssClass="txtautofilldesg" Style="text-align: center"
                                                        Text='<%#Bind("Designation")%>'> </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtnoofemployees" runat="server" Style="text-align: center; width: 98%"
                                                        Text='<%#Bind("NoofEmps")%>'> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        TargetControlID="txtnoofemployees" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             
                                            <asp:TemplateField HeaderText="No.of Duties">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNoOfDuties" runat="server" Style="text-align: center; width: 98%"
                                                        Text='<%#Bind("DutyHours")%>'> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                        TargetControlID="txtNoOfDuties" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPayRate" runat="server" Style="text-align: center; width: 98%"
                                                        Text='<%#Eval("payrate", "{0:0.##}")%>'> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                        TargetControlID="txtPayRate" ValidChars="-0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duties Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddldutytype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldutytype_SelectedIndexChanged" >
                                                        <asp:ListItem Value="0">P.M</asp:ListItem>
                                                        <asp:ListItem Value="1">P.D</asp:ListItem>
                                                        <asp:ListItem Value="2">P.Hr</asp:ListItem>
                                                        <asp:ListItem Value="3">P.Sft</asp:ListItem>
                                                        <asp:ListItem Value="4">Fixed</asp:ListItem>
                                                         <asp:ListItem Value="5">Heading</asp:ListItem>
                                                         <asp:ListItem Value="6">P.M(8Hrs)</asp:ListItem>
                                                         

                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NOD">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlnod" runat="server" AppendDataBoundItems="True" >
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
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtda" runat="server" Style="text-align: center; width: 98%" Text='<%#Eval("BasicDa", "{0:0.##}")%>'> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                        TargetControlID="txtda" ValidChars="-0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdNOD" runat="server" Value='<%#Bind("NoOfDays")%>' />
                                                    <asp:TextBox ID="txtAmount" runat="server" Style="text-align: center; width: 98%"
                                                        Text='<%#Eval("Totalamount", "{0:0.##}")%>'> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                        TargetControlID="txtAmount" ValidChars="-0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
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
                                        </ContentTemplate>
                                </asp:UpdatePanel>
                                <div>
                                    <asp:Button ID="btnAddNewRow" runat="server" Text="Add Row" Visible="false" OnClick="btnAddNewRow_Click" />
                                    <asp:Button ID="btnCalculateTotals" runat="server" Text="Calculate Totals" Visible="false"
                                        OnClick="btnCalculateTotals_Click" />
                                    
                                </div>
                                <div class="rounded_corners" style="overflow: auto; width: 99%;">
                                    <table width="100%" cellpadding="5" cellspacing="5" style="">

                                       
                                            
                                        <tr>
                                            <td align="right" valign="top">
                                                <table width="60%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td style="position:relative;left:-350px">
                                                            Remarks :
                                                        </td>
                                                        <td >
                                                            <asp:TextBox ID="txtremarks" runat="server"  MaxLength="200" TabIndex="35"  Width="170px" Height="50px" TextMode="MultiLine" style="position:relative;right:590px; top: -2px;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="80%" style="text-align: right">
                                                            Total :
                                                        </td>
                                                        <td width="20%" style="text-align: right">
                                                           <%-- <asp:Label ID="lblTotalResources" Text="" runat="server"></asp:Label>--%>
                                                            <asp:TextBox ID="lblTotalResources" runat="server" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="80%" style="text-align: right">
                                                            <asp:Label ID="Labelservicechrg" Text="Service charges  @" runat="server"></asp:Label>
                                                            <asp:TextBox ID="Txtservicechrg" runat="server" Width="50"></asp:TextBox>
                                                        </td>
                                                        <td width="20%" style="text-align: right">
                                                          <%--  <asp:Label ID="lblServiceCharges" runat="server"></asp:Label>--%>
                                                            <asp:TextBox ID="lblServiceCharges" runat="server" ></asp:TextBox>

                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="80%" style="text-align: right">
                                                            <asp:Label ID="Labelsubtotal" Text="Sub Total :" runat="server"></asp:Label>
                                                        </td>
                                                        <td width="20%" style="text-align: right">
                                                          <%--  <asp:Label ID="lblSubTotal" runat="server"></asp:Label>--%>
                                                             <asp:TextBox ID="lblSubTotal" runat="server" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="80%" style="text-align: right">
                                                            <asp:Label ID="lblServiceTaxTitle" Visible="false" Text="Service Tax :" runat="server"></asp:Label>
                                                        </td>
                                                        <td width="20%" style="text-align: right">
                                                           <%-- <asp:Label ID="lblServiceTax" Text="" Visible="false" runat="server"></asp:Label>--%>
                                                           <asp:TextBox ID="lblServiceTax" runat="server" Visible="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSBCESSTitle" Visible="false" Text="SB CESS :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                       <%-- <asp:Label ID="lblSBCESS" Text="" Visible="false" runat="server"></asp:Label>--%>
                                                         <asp:TextBox ID="lblSBCESS" runat="server" Visible="false"></asp:TextBox>
                                                    </td>
                                                    </tr>

                                                    <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblKKCESSTitle" Visible="false" Text="KK CESS :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <%--<asp:Label ID="lblKKCESS" Text="" Visible="false" runat="server"></asp:Label>--%>
                                                        <asp:TextBox ID="lblKKCESS" runat="server" Visible="false"></asp:TextBox>
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="80%" style="text-align: right">
                                                            <asp:Label ID="lblCESSTitle" Visible="false" Text="CESS :" runat="server"></asp:Label>
                                                        </td>
                                                        <td width="20%" style="text-align: right">
                                                           <%-- <asp:Label ID="lblCESS" Text="" Visible="false" runat="server"></asp:Label>--%>
                                                             <asp:TextBox ID="lblCESS" runat="server" Visible="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="80%" style="text-align: right">
                                                            <asp:Label ID="lblSheCESSTitle" Visible="false" Text="S&H Ed. CESS :" runat="server"></asp:Label>
                                                        </td>
                                                        <td width="20%" style="text-align: right">
                                                            <%--<asp:Label ID="lblSheCESS" Visible="false" Text="" runat="server"></asp:Label>--%>
                                                            <asp:TextBox ID="lblSheCESS" runat="server" Visible="false" Text=""></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="80%" style="text-align: right; font-weight: bold">
                                                            Grand Total :
                                                        </td>
                                                        <td width="20%" style="text-align: right; font-weight: bold">
                                                           <%-- <asp:Label ID="lblGrandTotal" Text="" runat="server"></asp:Label>--%>
                                                            <asp:TextBox ID="lblGrandTotal" runat="server" Text=""></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Button ID="btngenratepayment" runat="server" class="btn save" Text="Genrate Bill"
                                                                OnClick="btngenratepayment_Click" OnClientClick='return confirm(" Are you sure you  want to  generate bill ?");' />
                                                            <asp:Button ID="btninvoice" runat="server" Text="Tax Invoice" class="btn save" OnClick="btninvoiceNew_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbltotalamount" runat="server"> </asp:Label>
                                                            <asp:Label ID="lblamtinwords" Text="" runat="server" Visible="false"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
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
                <!-- DASHBOARD CONTENT END -->
            </div>
        </div>
         
        <!-- CONTENT AREA END -->
       </asp:content>