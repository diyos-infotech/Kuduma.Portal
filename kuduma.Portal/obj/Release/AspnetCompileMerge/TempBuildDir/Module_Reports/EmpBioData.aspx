<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="EmpBioData.aspx.cs" Inherits="Kuduma.Portal.EmpBioData" %>
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

        function bindautofilldesgs() {
            $(".txtautofillempid").autocomplete({
                source: eval($("#hdempid").val()),
                minLength: 4
            });
        }

    </script>
        <script type="text/javascript">
         
            function GetEmpid() {

                $("#<%=txtEmpid.ClientID %>").autocomplete({
                source: function (request, response) {
                    var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpIDs";
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
        overflow:auto;
             
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
            <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">EMPLOYEE FORMS
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div style="margin-left: 20px">
                                        <asp:HiddenField ID="hdempid" runat="server" />
                                        <div>

                                            <table style="width:90%">
                                                <tr style="height:40px">
                                                    <td>Type
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEmpIDoptions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpIDoptions_SelectedIndexChanged" class="form-control">
                                                            <asp:ListItem>Emp Id</asp:ListItem>
                                                            <asp:ListItem>From To</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                     <td>
                                                        <asp:Label runat="server" ID="Label1"  Text="PDF Options" Style="padding-left: 50px;"></asp:Label>

                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddloption" runat="server" class="form-control">
                                                            <asp:ListItem>All</asp:ListItem>
                                                            <asp:ListItem>Enrolment Letter</asp:ListItem>
                                                            <asp:ListItem>ESI Declaration</asp:ListItem>
                                                            <asp:ListItem>PF Declaration</asp:ListItem>
                                                            <asp:ListItem>Appointment Letter</asp:ListItem>
                                                            <asp:ListItem>Movement Order</asp:ListItem>
                                                            <asp:ListItem>ID Card</asp:ListItem>
                                                            <asp:ListItem>Police Verification</asp:ListItem>
                                                            <asp:ListItem>PF Form 11</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                     <td>
                                                          <div style="float:right">
                                                            <asp:Button ID="btndownload" runat="server" Text="Download"
                                                                class="btn save" OnClick="btndownload_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                           
                                                <tr style="height:40px">

                                                    <td>
                                                        <asp:Label runat="server" ID="lblfrmempid" Visible="false"  Text="From Emp ID"></asp:Label>
                                                        <asp:Label runat="server" ID="lblempid" Width="70px" Text="Emp ID"></asp:Label>

                                                    </td>
                                                    <td>
                                                         <asp:TextBox ID="txtEmpid" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtEmpid_TextChanged"></asp:TextBox>
                                                        
                                                        <asp:TextBox ID="txtfromempid" runat="server" Visible="false" CssClass="form-control"></asp:TextBox>
                                                                                                          </td>

                                                    <td>
                                                        <asp:Label runat="server" ID="lbltoempid" Visible="false"  Text="To Emp ID" Style="padding-left: 50px;"></asp:Label>
                                                        <asp:Label runat="server" ID="lblempname" Width="80px" Text="Name" Style="padding-left: 50px;"></asp:Label>

                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txttoempid" runat="server" Visible="false" CssClass="form-control" ></asp:TextBox>
                                                        

                                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>
                                                       
                                                    </td>
                                                   <td></td>
                                                </tr>

                                            </table>

                                            <table>
                                               
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnEnrolment" runat="server" Text="Bio Data"
                                                            Style="margin-left: -90px" class="btn save" OnClick="btnEnrolmentForm_Click" Visible="false" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btndeclaration" runat="server" Text="Declaration"
                                                            Style="margin-left: -10px" class="btn save" OnClick="btnESIDeclaration_Click" Visible="false" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnESIForm" runat="server" Text="ESI Form"
                                                            Style="margin-left: -5px" class="btn save" OnClick="btnESIForm_Click" Visible="false" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnPFForm" runat="server" Text="PF Form"
                                                            Style="margin-left: 5px" class="btn save" OnClick="btnPFForm_Click" Visible="false" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnApplForm" runat="server" Text="Appointment Form"
                                                            Style="margin-left: 5px" class="btn save" OnClick="btnApplForm_Click" Visible="false" />

                                                        <asp:Button ID="btnPFForm11" runat="server" Text="PF Form11"
                                                            Style="margin-left: 5px" class="btn save" OnClick="btnPFForm11_Click" Visible="false" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnICICIForm" runat="server" Text="ICICI Form"
                                                            Style="margin-left: 5px" class="btn save" OnClick="btnICICIForm_Click" Visible="false" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnAppointmentForm" runat="server" Text="Appointment Form"
                                                            Style="margin-left: 5px" class="btn save" OnClick="btnAppointmentForm_Click" Visible="false" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnPoliceVfctn" runat="server" Text="Police Verification Form"
                                                            Style="margin-left: 5px" class="btn save" OnClick="btnPoliceVfctn_Click" Visible="false" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnMovementOrder" runat="server" Text="Movement Order"
                                                            Style="margin-left: 5px" class="btn save" OnClick="btnMovementOrder_Click" Visible="false" />
                                                    </td>
                                                </tr>

                                            </table>


                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
            <!-- DASHBOARD CONTENT END -->
            </asp:content>