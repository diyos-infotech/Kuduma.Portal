<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="CreateLoginPocketFame.aspx.cs" Inherits="Kuduma.Portal.CreateLoginPocketFame" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
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

        $(document).ready(function () {
            $("#<%=txtConfirmPassword.ClientID %>").keyup(validate);
        });


        function validate() {
            var password1 = $("#<%=txtPassword.ClientID %>").val();
            var password2 = $("#<%=txtConfirmPassword.ClientID %>").val();



            if (password1 == password2) {
                $("#<%=lblerror.ClientID %>").text("");
            }
            else {
                $("#<%=lblerror.ClientID %>").text("invalid");
            }

        }

        function Check(evt) {
            if (evt.keyCode == 32) {
                alert("Space not allowed");
                return false;
            }
            return true;
        }


        function onChangeTest(evt) {
            debugger;
            if (evt.value == "") {
                alert("Error: Username cannot be blank!");
                document.getElementById("<%=txtPassword.ClientID %>").value = "";
                form.username.focus();
                return false;
            }

            if (evt.value != "") {
                if (evt.value.length < 8) {
                    alert("Error: Password must contain at least eight characters!");
                    document.getElementById("<%=txtPassword.ClientID %>").value = "";
                    evt.focus();
                    return false;
                }

                re = /[0-9]/;
                if (!re.test(evt.value)) {
                    alert("Error: password must contain at least one number (0-9)!");
                    document.getElementById("<%=txtPassword.ClientID %>").value = "";
                    evt.focus();
                    return false;
                }
                re = /^[a-zA-Z]/;
                if (!re.test(evt.value)) {
                    alert("Error: password must contain at least one or more letter characters (A-Za-z)!");
                    document.getElementById("<%=txtPassword.ClientID %>").value = "";
                    evt.focus();
                    return false;
                }
            } else {
                alert("Error: Please check that you've entered and confirmed your password!");
                form.pwd1.focus();
                return false;
            }

            alert("You entered a valid password: " + evt.value);
            return true;
        }

    </script>

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">

        <div id="breadcrumb">
            <ul class="crumbs">
                <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Create Pocket FaMe login</a></li>
            </ul>
        </div>
        <!-- DASHBOARD CONTENT BEGIN -->
        <div class="contentarea" id="contentarea">
            <div class="dashboard_center">
                <div class="sidebox">
                    <div class="boxhead">
                        <h2 style="text-align: center">Create Pocket FaMe login
                        </h2>
                    </div>
                    <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                        <div class="boxin">
                            <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                            </asp:ScriptManager>


                            <asp:HiddenField ID="hdempid" runat="server" />

                            <table style="width: 100%">

                                <tr style="height: 40px">

                                    <td>Emp ID :<span style="color: Red">*</span>
                                    </td>

                                    <td>Emp Name :<span style="color: Red">*</span>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtEmpid" runat="server" Width="50%" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtemplyid_TextChanged"></asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtName" runat="server" Width="50%" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFname_TextChanged"></asp:TextBox>
                                    </td>
                                </tr>








                                <tr style="height: 40px">
                                    <td>Role<span style="color: Red">*</span>
                                    </td>
                                    <td>Shift<span style="color: Red">*</span>
                                    </td>
                                </tr>
                                <tr style="height: 40px">
                                    <td>
                                        <asp:DropDownList ID="ddlrole" runat="server" CssClass="form-control" Width="55%"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlshift" runat="server" CssClass="form-control" Width="55%" AutoPostBack="true" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                </tr>


                                <tr style="height: 40px">
                                    <td>Shift Start Time :<span style="color: Red">*</span>
                                    </td>

                                    <td>Shift End Time :<span style="color: Red">*</span>
                                    </td>

                                </tr>
                                <tr style="height: 40px">
                                    <td>
                                        <asp:TextBox ID="txtShiftstarttime" TabIndex="2" runat="server" Enabled="false" class="form-control" Width="50%"></asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtShiftEndtime" TabIndex="2" runat="server" Enabled="false" class="form-control" Width="50%"></asp:TextBox>
                                    </td>
                                </tr>


                                <tr style="height: 40px" runat="server">


                                    <td>Site Posted To :<span style="color: Red">*</span>
                                    </td>

                                    <td>User Name :<span style="color: Red">*</span>
                                    </td>

                                </tr>
                                <tr style="height: 40px" runat="server">
                                    <td>
                                        <asp:DropDownList ID="ddlsiteposted" runat="server" CssClass="form-control" Width="55%" AutoPostBack="true"></asp:DropDownList>
                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtusrname" runat="server" Text="" onkeydown="return Check(event)" onpaste="return false;" CssClass="form-control" Width="50%"></asp:TextBox>
                                    </td>

                                </tr>


                                <tr style="height: 40px">
                                    <td>Password:<span style="color: Red">*</span></td>

                                    <td>Confirm Password :<span style="color: Red">*</span>
                                    </td>

                                </tr>
                                <tr style="height: 40px">
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" onchange="onChangeTest(this)" onkeydown="return Check(event)" onpaste="return false;" CssClass="form-control" Width="50%"></asp:TextBox>

                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtConfirmPassword" TextMode="Password" runat="server" onpaste="return false;" CssClass="form-control" Width="50%"></asp:TextBox>
                                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="BtnSave_Click" />
                                    </td>
                                </tr>

                            </table>



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
    <!-- DASHBOARD CONTENT END -->
</asp:Content>
