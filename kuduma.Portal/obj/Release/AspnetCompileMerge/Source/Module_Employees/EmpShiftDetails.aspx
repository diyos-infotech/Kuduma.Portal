<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Employees/EmployeeMaster.master" AutoEventWireup="true" CodeBehind="EmpShiftDetails.aspx.cs" Inherits="Kuduma.Portal.Module_Employees.EmpShiftDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />

    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <script src="script/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="script/jscript.js"> </script>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>


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

    <script type="text/javascript">

        function GetEmpid() {

            $("#<%=txtemplyid.ClientID %>").autocomplete({
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

                    $("#<%=txtemplyid.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                }
            });
        }

        function GetEmpName() {

            $("#<%=txtEmpFName.ClientID %>").autocomplete({
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
                    $("#<%=txtEmpFName.ClientID %>").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                }
            });

        }

        function OnAutoCompletetxtEmpidchange(event, ui) {
            $("#<%=txtemplyid.ClientID %>").trigger('change');

        }
        function OnAutoCompletetxtEmpNamechange(event, ui) {
            $("#<%=txtEmpFName.ClientID %>").trigger('change');

        }

        $(document).ready(function () {

            GetEmpid();
            GetEmpName();
        });



    </script>

    <div id="content-holder">
        <div class="content-holder">
            <div class="col-md-12" style="margin-top: 8px; margin-bottom: 8px">
                <asp:ScriptManager runat="server" ID="Scriptmanager2">
                </asp:ScriptManager>
                <div class="panel panel-inverse">
                    <div class="panel-heading">



                        <table width="100%">
                            <tr>
                                <td>
                                    <h3 class="panel-title">Emp Shift Details</h3>
                                </td>
                                <td align="right"><< <a href="Employees.aspx" style="color: #003366">Back</a>  </td>
                            </tr>
                        </table>


                    </div>

                    <div class="panel-body">

                        <div style="text-align: right">
                            <asp:Label ID="txtmodifyempid" runat="server"></asp:Label>
                        </div>
                        <div id="tabs">
                            <div id="tabs-1">

                                <div class="dashboard_firsthalf">
                                    <table cellpadding="5" cellspacing="5">
                                        <tr style="height: 32px">
                                            <td style="width: 100px">Emp ID<span style="color: Red">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtemplyid" AutoPostBack="true" OnTextChanged="txtemplyid_TextChanged" class="form-control" Width="200px"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr style="height: 32px">
                                            <td>Site Posted to
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlPreferedUnit" TabIndex="34" runat="server"  Width="229"
                                                   class="form-control">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr style="height: 32px">
                                            <td>Shift start time
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShiftstarttime" TabIndex="2" runat="server" Enabled="false" class="form-control" Width="200px"></asp:TextBox>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>

                                        <tr style="height: 32px" runat="server" visible="false">
                                            <td>Woff1
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlWoff1" TabIndex="34" runat="server" class="form-control" Width="200px">
                                                    <asp:ListItem>--select--</asp:ListItem>
                                                    <asp:ListItem>Sunday</asp:ListItem>
                                                    <asp:ListItem>Monday</asp:ListItem>
                                                    <asp:ListItem>Tuesday</asp:ListItem>
                                                    <asp:ListItem>Wednesday</asp:ListItem>
                                                    <asp:ListItem>Thursday</asp:ListItem>
                                                    <asp:ListItem>Friday</asp:ListItem>
                                                    <asp:ListItem>Saturday</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr style="height: 32px">
                                            <td>Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmpFName" TabIndex="2" runat="server" class="form-control" Width="200px" MaxLength="25"></asp:TextBox>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>

                                        <tr style="height: 32px">
                                            <td>Role
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRole" TabIndex="34" class="form-control" runat="server"  Width="229"
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                    </table>


                                </div>

                                <div class="dashboard_secondhalf">
                                    <table cellpadding="5" cellspacing="5">
                                        <tr style="height: 32px">
                                            <td style="width: 150px">Employee Name</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtFname" class="form-control" AutoPostBack="true" OnTextChanged="txtFname_TextChanged" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="height: 32px">
                                            <td>Shift
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlShift" TabIndex="34" runat="server" Width="229" AutoPostBack="true"
                                                    class="form-control" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="height: 32px">
                                            <td>Shift End time</td>
                                            <td>
                                                <asp:TextBox ID="txtShiftEndtime" TabIndex="2" runat="server" Enabled="false" class="form-control" Width="200px"></asp:TextBox>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>

                                        <tr style="height: 32px" runat="server" visible="false">
                                            <td>Woff2
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlWoff2" TabIndex="34" runat="server" class="sdrop" Width="200px">
                                                    <asp:ListItem>--select--</asp:ListItem>
                                                    <asp:ListItem>Sunday</asp:ListItem>
                                                    <asp:ListItem>Monday</asp:ListItem>
                                                    <asp:ListItem>Tuesday</asp:ListItem>
                                                    <asp:ListItem>Wednesday</asp:ListItem>
                                                    <asp:ListItem>Thursday</asp:ListItem>
                                                    <asp:ListItem>Friday</asp:ListItem>
                                                    <asp:ListItem>Saturday</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>

                                        <tr style="height: 32px">
                                            <td>Address 
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAddress" TabIndex="2" TextMode="MultiLine" runat="server" class="form-control" Width="200px"></asp:TextBox>
                                            </td>

                                        </tr>

                                        <tr style="height: 32px">
                                            <td>
                                                <asp:Button ID="BtnSave" runat="server" Text="Save" ValidationGroup="a" OnClick="BtnSave_Click" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblresult" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #CC3300;"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

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

                GetEmpid();
                GetEmpName();

            });
        };
    </script>

</asp:Content>
