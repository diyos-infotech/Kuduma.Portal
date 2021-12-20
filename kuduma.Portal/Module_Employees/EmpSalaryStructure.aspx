<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Employees/EmployeeMaster.master" CodeBehind="EmpSalaryStructure.aspx.cs" Inherits="Kuduma.Portal.EmpSalaryStructure" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <style type="text/css">
        .style1 {
            width: 135px;
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
        }
    </style>

      <script type="text/javascript">

          function GetEmpid() {

              $("#<%=txtEmpid.ClientID %>").autocomplete({
                  source: function (request, response) {
                      var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                      var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetEmpIDandoldids";
                      $.ajax({
                          url: ajaxUrl,
                          method: 'post',
                          contentType: 'application/json;charset=utf-8',

                          data: JSON.stringify({
                              term: request.term,
                          }),
                          datatype: 'json',
                          success: function (data) {
                              response(data.d);
                          },
                          error: function (err) {
                              alert(err);
                          }
                      });
                  },
                  minLength: 4,
                  select: function (event, ui) {

                      $("#<%=txtEmpid.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                  }
              });
              }

              function GetEmpName() {

                  $("#<%=txtName.ClientID %>").autocomplete({
                    source: function (request, response) {
                        var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                        var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpNames";
                        $.ajax({

                            url: ajaxUrl,
                            method: 'post',
                            contentType: 'application/json;charset=utf-8',
                            data: JSON.stringify({
                                term: request.term
                            }),
                            datatype: 'json',
                            success: function (data) {
                                response(data.d);
                            },
                            error: function (err) {
                                alert(err);
                            }
                        });
                    },
                    minLength: 4,
                    select: function (event, ui) {
                        $("#<%=txtName.ClientID %>").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                  }
                });

              }

              function OnAutoCompletetxtEmpidchange(event, ui) {
                  $("#<%=txtEmpid.ClientID %>").trigger('change');

            }
            function OnAutoCompletetxtEmpNamechange(event, ui) {
                $("#<%=txtName.ClientID %>").trigger('change');

          }

          $(document).ready(function () {

              GetEmpid();
              GetEmpName();
          });



    </script>

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
            $(".txtautofillempid").autocomplete({
                source: eval($("#hdempid").val()),
                minLength: 4
            });
        }

    </script>

    <style type="text/css">
        .style1 {
            width: 135px;
        }

        .completionList {
            background: white;
            border: 1px solid #DDD;
            border-radius: 3px;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            min-width: 165px;
            height: 120px;
            overflow: auto;
        }

        .listItem {
            display: block;
            padding: 5px 5px;
            border-bottom: 1px solid #DDD;
        }

        .itemHighlighted {
            color: black;
            background-color: rgba(0, 0, 0, 0.1);
            text-decoration: none;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            border-bottom: 1px solid #DDD;
            display: block;
            padding: 5px 5px;
        }
    </style>
        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div align="center">
                        <asp:Label ID="lblMsg" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #CC3300;"></asp:Label>
                    </div>
                    <div align="center">
                        <asp:Label ID="lblSuc" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #000;"></asp:Label>
                    </div>
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <%--  <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                        <%-- <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>--%>
                        <%-- <li class="active"><a href="EmpSalaryStructure.aspx" style="z-index: 7;" class="active_bread">Emp Salary Structure </a></li>--%>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Emp Salary Structure
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div style="margin-left: 20px">
                                        <asp:HiddenField ID="hdempid" runat="server" />
                                        <div>
                                         

                                                    <div class="dashboard_firsthalf" style="padding: 10px">
                                                        <table>
                                                            <tr>
                                                                <td style="height: 20px">Status
                                                    </td>
                                                    <td style="height: 20px; padding-left: 8px">
                                                        <asp:DropDownList ID="ddlempsalstatus" runat="server"  CssClass="sdrop">
                                                         <asp:ListItem Value="1">Active</asp:ListItem>
                                                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                                                            </asp:DropDownList>
                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px;width:114px">
                                                                    <asp:Label runat="server" ID="lblempid" Text="Emp ID"></asp:Label></td>                                                         
                                                                 <td style="height: 20px; padding-left: 8px">
                                                                   <asp:TextBox ID="txtEmpid" runat="server"  CssClass="form-control" AutoPostBack="true" OnTextChanged="txtEmpid_TextChanged" Width="190px"></asp:TextBox>                                                 
                                                            </td>

                                                            </tr>
                                                              <tr>
                                                    <td style="height: 20px">ID
                                                    </td>
                                                    <td style="height: 20px; padding-left: 8px">
                                                        <asp:DropDownList ID="ddlID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlID_SelectedIndexChanged" CssClass="sdrop">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="Btn_Renewal" runat="server" OnClick="Btn_Renewal_Click" Text="Renewal" />

                                                    </td>
                                                </tr>
                                                            <tr>
                                                                <td style="height: 20px">From Date<span style="color: Red">*</span>
                                                    </td>
                                                    <td style="height: 20px; padding-left: 8px">
                                                        <asp:TextBox ID="txtStartingDate" TabIndex="3" runat="server" class="sinput"
                                                            MaxLength="10" onkeyup="dtval(this,event)"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEStartingDate" runat="server" Enabled="true" TargetControlID="txtStartingDate"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEStartingDate" runat="server" Enabled="True"
                                                            TargetControlID="txtStartingDate" ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                            </tr>

                                                        </table>
                                                    </div>

                                                      <div class="dashboard_secondhalf" style="padding: 10px">
                                                          <table>
                                                              <tr>
                                                                  <td style="width:114px"">
                                                                <asp:Label runat="server" style="visibility:hidden" ID="Label1" Text="Name" Width="50px"></asp:Label></td>

                                                            <td style="padding-left:35px">

                                                                <asp:TextBox ID="TextBox1" runat="server" style="visibility:hidden"  TabIndex="2" class="form-control" Width="190px" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>

                                                            </td>
                                                              </tr>
                                                        <tr>
                                                            <td style="width:114px"">
                                                                <asp:Label runat="server" ID="lblempname" Text="Name" Width="50px"></asp:Label></td>

                                                            <td style="padding-left:35px">

                                                                <asp:TextBox ID="txtName" runat="server"  TabIndex="2" class="form-control" Width="190px" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                              <tr>
                                                <td><%--Id--%>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtid" runat="server" ReadOnly="true" TabIndex="9" class="sinput" Style="margin-left:40px" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                                              <tr>
                                                                  <td></td>
                                                                   <td>
                                                    <asp:TextBox ID="txtdesignation" runat="server"  TabIndex="9" class="sinput" Style="margin-left:40px" Visible="false"></asp:TextBox>
                                                </td>
                                                              </tr>
                                                              
                                                              <tr>
                                                                  <td></td><td></td>
                                                              </tr>
                                                              
                                                              <tr>
                                                                  <td></td><td></td>
                                                              </tr>
                                                               <tr>
                                                                  <td></td><td></td>
                                                              </tr>
                                                               <tr>
                                                                  <td></td><td></td>
                                                              </tr>
                                                               <tr>
                                                                  <td></td><td></td>
                                                              </tr>
                                                            <tr>
                                                                  <td></td><td></td>
                                                              </tr>
                                                              <tr>
                                                                  <td></td><td></td>
                                                              </tr>
                                                              <tr>
                                                                  <td></td><td></td>
                                                              </tr>
                                                              <tr>
                                                    

                                                    <td>To Date<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEndingDate" TabIndex="4" runat="server" class="sinput" MaxLength="10"
                                                            onkeyup="dtval(this,event)" Style="margin-left:40px" ></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEEndingDate" runat="server" Enabled="true" TargetControlID="txtEndingDate"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEEndingDate" runat="server" Enabled="True" TargetControlID="txtEndingDate"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>

                                                   
                                                </tr>
                                                    </table>
                                                          </div>
                                             
                                            
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td style="height: 20px">No.Of Days
                                                        </td>
                                                        <td style="height: 20px; padding-left: 8px">
                                                            <asp:DropDownList ID="ddlNoOfDaysWages" runat="server" TabIndex="27" CssClass="sdrop">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>G-S</asp:ListItem>
                                                                <asp:ListItem>G-4</asp:ListItem>
                                                                <asp:ListItem>P.Hr</asp:ListItem>
                                                                <asp:ListItem>P.Day</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>30.45</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 20px">Basic
                                                        </td>
                                                        <td style="height: 20px; padding-left: 8px">
                                                            <asp:TextBox ID="TxtBasic" runat="server" TabIndex="28" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FTBBasic" runat="server" Enabled="True"
                                                                TargetControlID="TxtBasic" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">HRA</td>
                                                        <td style="padding-left: 8px; padding-left: 8px">
                                                            <asp:TextBox ID="txthra" runat="server" TabIndex="30" class="sinput"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FTBHRA" runat="server" Enabled="True" TargetControlID="txthra"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">CCA</td>
                                                        <td style="padding-left: 8px; padding-left: 8px">
                                                            <asp:TextBox ID="txtcca" runat="server" TabIndex="32" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FTBCCA" runat="server" Enabled="True" TargetControlID="txtcca"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Gratuity</td>
                                                        <td style="padding-left: 8px; padding-left: 8px">
                                                            <asp:TextBox ID="txtgratuty" runat="server" TabIndex="34" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="F5" runat="server" Enabled="True" TargetControlID="txtgratuty"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">WA</td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtwa" runat="server" TabIndex="36" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="F7" runat="server" Enabled="True" TargetControlID="txtwa"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                      <tr>
                                                        <td style="height: 20px">RC
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="Txtrc" runat="server" TabIndex="40" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Filterrc" runat="server" Enabled="True" TargetControlID="Txtrc"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">NFHs</td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtNfhs1" TabIndex="38" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Fnhs" runat="server" Enabled="True" TargetControlID="txtNfhs1"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                  
                                                    <tr>
                                                        <td style="height: 20px">OT Rate
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="TxtOTRate" runat="server" TabIndex="42" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Filterotrate" runat="server" Enabled="True" TargetControlID="TxtOTRate"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                      <tr>
                                                        <td style="height: 20px">NHS Rate
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="TxtNHSRate" runat="server" TabIndex="42" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilterNHSrate" runat="server" Enabled="True" TargetControlID="TxtNHSRate"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                  
                                                     <tr  runat="server">
                                                <td style="height: 20px">
                                                   Service Weightage
                                                </td>
                                                <td style="padding-left:8px">
                                                    <asp:TextBox ID="txtServiceWeightage" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True" TargetControlID="txtServiceWeightage"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                             <tr runat="server" visible="false">
                                                <td style="height: 20px">
                                                    Insurance
                                                </td>
                                                <td style="padding-left:8px">
                                                    <asp:TextBox ID="txtInsurance" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True" TargetControlID="txtInsurance"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                                     <tr runat="server" visible="false">
                                                        <td style="height: 20px">Insurance Type
                                                        </td>
                                                        <td style="padding-left:8px">
                                                            <asp:DropDownList ID="ddlInsuranceType" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Fixed</asp:ListItem>
                                                                <asp:ListItem>Monthly</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                               <tr runat="server" visible="false" >
                                                <td style="height: 20px">
                                                    Uniform Charges
                                                </td>
                                                <td style="padding-left:8px">
                                                    <asp:TextBox ID="txtunicharges" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txtunicharges"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                                     <tr runat="server" visible="false">
                                                        <td style="height: 20px">Uniform Charges Type
                                                        </td>
                                                        <td style="padding-left:8px">
                                                            <asp:DropDownList ID="ddlUniformChargesType" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Fixed</asp:ListItem>
                                                                <asp:ListItem>Monthly</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" visible="false">
                                                <td style="height: 20px">
                                                    WMC
                                                </td>
                                                <td style="padding-left:8px">
                                                    <asp:TextBox ID="txtwmc" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" TargetControlID="txtwmc"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                                       
                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px"> Leave With Wages</td>
                                                        <td style="padding-left:8px">
                                                             <asp:TextBox ID="txtBillLeaveWages" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" TargetControlID="txtBillLeaveWages"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" visible="false">
                                                        <td style="height: 20px">Bonus (Bill)</td>
                                                        <td style="padding-left:8px">
                                                             <asp:TextBox ID="txtBillBonus" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" TargetControlID="txtBillBonus"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" visible="false">
                                                <td style="height: 20px">
                                                    OutStation Charges
                                                </td>
                                                <td style="padding-left:8px">
                                                    <asp:TextBox ID="txtoutstationCharges" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" TargetControlID="txtoutstationCharges"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                                     <tr runat="server" visible="false">
                                                        <td style="height: 20px">Outstation Chrg Caln
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:DropDownList ID="ddlOutstationChrgCaln" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Only Billing</asp:ListItem>
                                                                <asp:ListItem>Billing&Paysheet</asp:ListItem>

                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" visible="false">
                                                        <td style="height: 20px">OutStationCharges Type
                                                        </td>
                                                        <td style="padding-left: 7px; height: 20px">
                                                            <asp:DropDownList ID="ddlOutStationChargesType" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Fixed</asp:ListItem>
                                                                <asp:ListItem>Monthly</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                      <tr runat="server">
                                                        <td style="height: 20px">Travelling Allowance
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtTravellingAllowance" runat="server" TabIndex="42" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertr17" runat="server" Enabled="True" TargetControlID="txtTravellingAllowance"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>


                                                    <tr runat="server" >
                                                        <td style="height: 20px">Mobile Allowance
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtMobileAllowance" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FTBMobAllw" runat="server" Enabled="True" TargetControlID="txtMobileAllowance"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                     <tr runat="server" >
                                                        <td style="height: 20px">ADDL 4HR
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtADDL4HR" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" TargetControlID="txtADDL4HR"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" >
                                                        <td style="height: 20px">Qtr Allowance
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtQTRALLOW" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True" TargetControlID="txtQTRALLOW"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" >
                                                        <td style="height: 20px">REL Allowance
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtRELALLOW" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True" TargetControlID="txtRELALLOW"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" >
                                                        <td style="height: 20px">OT ESIC WAGES
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtOTESICWAGES" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" Enabled="True" TargetControlID="txtOTESICWAGES"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                     <tr>
                                                        <td style="height: 20px">OT ESIC Days
                                                        </td>
                                                        <td style="padding-left: 8px; height: 20px">
                                                            <asp:DropDownList ID="ddlotesidays" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>G-S</asp:ListItem>
                                                                <asp:ListItem>G-4</asp:ListItem>
                                                                <asp:ListItem>P.Hr</asp:ListItem>
                                                                <asp:ListItem>P.Day</asp:ListItem>
                                                                <asp:ListItem>P.M/P.D</asp:ListItem>
                                                                <asp:ListItem>P.M/8</asp:ListItem>
                                                                <asp:ListItem>G-S/8 Hrs</asp:ListItem>
                                                                <asp:ListItem>26 Days/8 Hrs</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" >
                                                        <td style="height: 20px">Site Allowance
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtSITEALLOW" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" Enabled="True" TargetControlID="txtSITEALLOW"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" >
                                                        <td style="height: 20px">GUN Allowance
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtGunAllw" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" Enabled="True" TargetControlID="txtGunAllw"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 20px">GUN Allowance Type
                                                        </td>
                                                        <td style="padding-left: 8px; height: 20px">
                                                            <asp:DropDownList ID="ddlGunAllwType" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem Value="0">Monthly</asp:ListItem>
                                                                <asp:ListItem  Value="1">Monthly(spl)</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" >
                                                        <td style="height: 20px">Fire Allowance
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtFireAllw" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True" TargetControlID="txtFireAllw"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>


                                                     
                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">Shift1 Rate
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtshift1Rate" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Filteretxtshift1Rate7" runat="server" Enabled="True" TargetControlID="txtshift1Rate"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">Shift2 Rate
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtshift2Rate" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Filtertxtshift2Rater17" runat="server" Enabled="True" TargetControlID="txtshift2Rate"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">Night Shift Rate
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtnightshiftRate" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTtxtnightshiftRate" runat="server" Enabled="True" TargetControlID="txtnightshiftRate"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">Late Login Rate
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtlateloginRate" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextxtlateloginRater18" runat="server" Enabled="True" TargetControlID="txtlateloginRate"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">APA
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtExtraOne" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTtxtExtraOne19" runat="server" Enabled="True" TargetControlID="txtExtraOne"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                             <tr runat="server" visible="false">
                                                <td style="height: 20px">
                                                   Extra1
                                                </td>
                                                <td style="padding-left:8px">
                                                    <asp:TextBox ID="txtExta1" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoghjxExtender19" runat="server" Enabled="True" TargetControlID="txtExta1"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                              <tr runat="server" visible="false">
                                                <td style="height: 20px">
                                                   Extra3
                                                </td>
                                                <td style="padding-left:8px">
                                                    <asp:TextBox ID="txtExra3" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" Enabled="True" TargetControlID="txtExra3"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">Edu. Allowance
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtEducationAllowance" runat="server" TabIndex="46" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True" TargetControlID="txtEducationAllowance"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 20px">Shift
                                                        </td>
                                                        <td style="height: 20px; padding-left: 8px">
                                                            <asp:DropDownList ID="ddlShift" runat="server" TabIndex="47" CssClass="sdrop">
                                                                <asp:ListItem>8</asp:ListItem>
                                                                <asp:ListItem>10</asp:ListItem>
                                                                <asp:ListItem>12</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                   

                                                    <tr>
                                                        <td style="height: 20px">
                                                            <asp:CheckBox ID="ChkSum" runat="server" AutoPostBack="true" OnCheckedChanged="ChkSum_CheckedChanged"
                                                 Text="Sum" />
                                                        </td>
                                                        <td style="padding-left: 8px">
                                                            <asp:TextBox ID="txtsum" runat="server" TabIndex="42" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" TargetControlID="txtsum"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                    <td style="height: 20px">&nbsp;
                                                    </td>
                                                    <td style="height: 20px">&nbsp;
                                                    </td>
                                                </tr>

                                                    <tr>
                                                        <td style="height: 20px">DA
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtda" runat="server" TabIndex="29" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                                                TargetControlID="txtda" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Conveyance</td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtConveyance" runat="server" TabIndex="31" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="F2" runat="server" Enabled="True" TargetControlID="txtConveyance"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">LA</td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtleaveamount" runat="server" TabIndex="33" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="F4" runat="server" Enabled="True" TargetControlID="txtleaveamount"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Bonus</td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtbonus" runat="server" TabIndex="35" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True" TargetControlID="txtbonus"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 20px">NBonus
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:DropDownList ID="ddlnbonus" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Monthly</asp:ListItem>
                                                                <asp:ListItem>Quarterly</asp:ListItem>
                                                                <asp:ListItem>Half yearly</asp:ListItem>
                                                                <asp:ListItem>Yearly</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 20px">OA</td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtoa" runat="server" TabIndex="37" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="F8" runat="server" Enabled="True" TargetControlID="txtoa"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server">
                                                        <td style="height: 20px">Spl. Allowance
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtSplAllw" runat="server" TabIndex="39" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True" TargetControlID="txtoa"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">CS
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="TxtCs" runat="server" TabIndex="41" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Filtercs" runat="server" Enabled="True" TargetControlID="TxtCs"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Nots
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:DropDownList ID="ddlNoOfOtsPaysheet" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>G-S</asp:ListItem>
                                                                <asp:ListItem>G-4</asp:ListItem>
                                                                <asp:ListItem>P.Hr</asp:ListItem>
                                                                <asp:ListItem>P.Day</asp:ListItem>
                                                                <asp:ListItem>P.M/P.D</asp:ListItem>
                                                                <asp:ListItem>P.M/8</asp:ListItem>
                                                                <asp:ListItem>G-S/8 Hrs</asp:ListItem>
                                                                <asp:ListItem>26 Days/8 Hrs</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 20px">NNhs
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:DropDownList ID="ddlNoOfNhsPaysheet" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>G-S</asp:ListItem>
                                                                <asp:ListItem>G-4</asp:ListItem>
                                                                <asp:ListItem>P.Hr</asp:ListItem>
                                                                <asp:ListItem>P.Day</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                  
                                                      <tr >
                                                <td style="height: 20px">
                                                    Rank Allowance
                                                </td>
                                                <td style="padding-left:22px;height: 20px">
                                                    <asp:TextBox ID="txtRankAllowance" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextnBoxExtender19" runat="server" Enabled="True" TargetControlID="txtRankAllowance"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false">
                                                <td style="height: 20px">
                                                    Mobile Recharge
                                                </td>
                                                <td style="padding-left:22px;height: 20px">
                                                    <asp:TextBox ID="txtMobileRecharge" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxjnExtender21" runat="server" Enabled="True" TargetControlID="txtMobileRecharge"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                                       <tr  runat="server" visible="false">
                                                        <td style="height: 20px">Mobile Recharge Type
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:DropDownList ID="ddlMobileRechargeType" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Fixed</asp:ListItem>
                                                                <asp:ListItem>Monthly</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>


                                                     <tr runat="server" visible="false">
                                                        <td style="height: 20px">Mobile Chrg Caln
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:DropDownList ID="ddlMobileChrgCaln" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Only Billing</asp:ListItem>
                                                                <asp:ListItem>Billing&Paysheet</asp:ListItem>

                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                      <tr runat="server" visible="false">
                                                        <td style="height: 20px">NUniCharges
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:DropDownList ID="ddlNunichrges" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Only Billing</asp:ListItem>
                                                                <asp:ListItem>Billing&Paysheet</asp:ListItem>

                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" visible="false">
                                                <td style="height: 20px">
                                                    Medical Policy
                                                </td>
                                                <td style="padding-left:22px;height: 20px">
                                                    <asp:TextBox ID="txtmedicalpolicy" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtmedicalpolicy"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                                      <tr runat="server" visible="false">
                                                        <td style="height: 20px"> Leave Wages Type (Bill)</td>
                                                        <td style="padding-left:22px;height: 20px">
                                                              <asp:DropDownList ID="ddlBillLeaveWagesType" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Fixed</asp:ListItem>
                                                                <asp:ListItem>Monthly</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" visible="false">
                                                        <td style="height: 20px"> Bonus Type (Bill)</td>
                                                        <td style="padding-left:22px;height: 20px">
                                                              <asp:DropDownList ID="ddlBillBonusType" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Fixed</asp:ListItem>
                                                                <asp:ListItem>Monthly</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                        <tr runat="server" visible="false">
                                                <td style="height: 20px">
                                                    LWF
                                                </td>
                                                <td style="padding-left:22px;height: 20px">
                                                    <asp:TextBox ID="txtlwf" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtlwf"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                                      <tr runat="server">
                                                        <td style="height: 20px">Performance Allowance
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtPerformanceAllowance" runat="server" TabIndex="41" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtepernder17" runat="server" Enabled="True" TargetControlID="txtPerformanceAllowance"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server">
                                                        <td style="height: 20px">Medical Allowance
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtMedicalAllw" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilterMedAll" runat="server" Enabled="True" TargetControlID="txtMedicalAllw"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" visible="false">
                                                        <td style="height: 20px">BGV Amount
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtbgv" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftbbgv" runat="server" Enabled="True" TargetControlID="txtbgv"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                     <tr runat="server" visible="false">
                                                        <td style="height: 20px">PVC Amount
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtpvc" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftbpvc" runat="server" Enabled="True" TargetControlID="txtpvc"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server">
                                                        <td style="height: 20px">Telephone Allw
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtTelephoneAllw" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True" TargetControlID="txtTelephoneAllw"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server">
                                                        <td style="height: 20px">Food Allw
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtFoodAllw" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" Enabled="True" TargetControlID="txtFoodAllw"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server">
                                                        <td style="height: 20px">Re-imbursement
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtReimbursement" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True" TargetControlID="txtReimbursement"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server">
                                                        <td style="height: 20px">Hardship Allw
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtHardshipAllw" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" Enabled="True" TargetControlID="txtHardshipAllw"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server">
                                                        <td style="height: 20px">Paid Holiday Allw
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtPaidHolidayAllw" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" Enabled="True" TargetControlID="txtPaidHolidayAllw"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server">
                                                        <td style="height: 20px">Service Charge
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtServiceCharge" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" Enabled="True" TargetControlID="txtServiceCharge"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">NShift1
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">

                                                            <asp:DropDownList ID="ddl1shiftdays" CssClass="sdrop" runat="server">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>G-S</asp:ListItem>
                                                                <asp:ListItem>G-4</asp:ListItem>
                                                                <asp:ListItem>22</asp:ListItem>
                                                                <asp:ListItem>23</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>

                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">NShift2
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">

                                                            <asp:DropDownList ID="ddl2shiftdays" CssClass="sdrop" runat="server">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>G-S</asp:ListItem>
                                                                <asp:ListItem>G-4</asp:ListItem>
                                                                <asp:ListItem>22</asp:ListItem>
                                                                <asp:ListItem>23</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>

                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">NNightshift
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">

                                                            <asp:DropDownList ID="ddlNightshiftdays" CssClass="sdrop" runat="server">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>G-S</asp:ListItem>
                                                                <asp:ListItem>G-4</asp:ListItem>
                                                                <asp:ListItem>22</asp:ListItem>
                                                                <asp:ListItem>23</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>

                                                    <tr runat="server" visible="false">
                                                        <td style="height: 20px">NLateLogin
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:DropDownList ID="ddlLateLogindays" TabIndex="43" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>G-S</asp:ListItem>
                                                                <asp:ListItem>G-4</asp:ListItem>
                                                                <asp:ListItem>P.Hr</asp:ListItem>
                                                                <asp:ListItem>P.Day</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                       <tr runat="server" visible="false">
                                                    <td> Nots (Spl)</td>
                                                    <td style="padding-left: 22px; height: 20px">
                                                        <asp:DropDownList ID="ddlnotsspl" runat="server" CssClass="sdrop">
                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                            <asp:ListItem Value="1">PM/PD(12 Hrs)</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>

                                            <tr runat="server" visible="false">
                                                <td style="height: 20px">
                                                    Extra2
                                                </td>
                                                <td style="padding-left:22px;height: 20px">
                                                    <asp:TextBox ID="txtExtra2" runat="server"  class="sinput"> </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Filtere4dTextBoxExtender22" runat="server" Enabled="True" TargetControlID="txtExtra2"
                                                            ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                                     <tr runat="server" >
                                                        <td>PF No of Days
                                                        </td>
                                                        <td style="padding-left: 22px">
                                                            <asp:DropDownList ID="ddlPFNoOfDaysForWages" runat="server" CssClass="sdrop">
                                                                <asp:ListItem>Gen</asp:ListItem>
                                                                <asp:ListItem>G-S</asp:ListItem>
                                                                <asp:ListItem>G-4</asp:ListItem>
                                                                <asp:ListItem>P.Hr</asp:ListItem>
                                                                <asp:ListItem>P.Day</asp:ListItem>

                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" >
                                                        <td style="height: 20px">PF PayRate</td>
                                                        <td style="padding-left: 22px">
                                                            <asp:TextBox ID="TxtPFPayRate" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Filterpfpayrate" runat="server" Enabled="True" TargetControlID="TxtPFPayRate"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr runat="server" >
                                                        <td style="height: 20px">ESI PayRate</td>
                                                        <td style="padding-left: 22px">
                                                            <asp:TextBox ID="TxtESIPayRate" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True" TargetControlID="TxtESIPayRate"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                         <td></td>
                                                     <td>
                                                        <div style="float: right">
                                                            <asp:Button ID="btnSubmit" runat="server" Text="Save"
                                                                class="btn save" OnClick="btnSubmit_Click" />
                                                        </div>
                                                    </td>
                                                    </tr>

                                                    <tr style="visibility: hidden">
                                                        <td style="height: 20px">Extra Two
                                                        </td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="txtExtraTwo" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextxtExtraTwor19" runat="server" Enabled="True" TargetControlID="txtExtraTwo"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr style="visibility: hidden">
                                                        <td style="height: 20px">&nbsp;</td>
                                                        <td style="height: 20px">&nbsp;</td>
                                                        <td style="height: 20px">&nbsp;</td>

                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td style="height: 20px">PF Voluntary</td>
                                                        <td style="padding-left: 22px; height: 20px">
                                                            <asp:TextBox ID="TxtPFVoluntary" runat="server" class="sinput"> </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilterpfVoluntary" runat="server" Enabled="True" TargetControlID="TxtPFVoluntary"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td style="height: 30px">&nbsp;</td>
                                                        <td style="height: 30px">&nbsp;</td>


                                                    </tr>

                                                    <tr style="visibility: hidden">
                                                        <td style="height: 20px">&nbsp;</td>
                                                        <td style="height: 20px">&nbsp;</td>


                                                    </tr>
                                                    
                                                </table>
                                            </div>


                                        </div>
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