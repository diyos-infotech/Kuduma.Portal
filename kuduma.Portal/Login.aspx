<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Kuduma.Portal.Login" %>

<html lang="en-us" class="no-js">
<meta http-equiv="Content-Type" content="text/html;charset=UTF-8" />
<head id="Head1" runat="server" />
<meta charset="utf-8" />
<title>FAME SOFTWARE</title>
<meta name="description" content="" />
<link href="images/interface/iOS_icon.png" rel="apple-touch-icon" />
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
<link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Open+Sans:400,700" />
<link href="login/login-style.css" rel="stylesheet" type="text/css" />

<!-- Bootstrap -->
<script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
<script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
<link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
    media="screen" />
<!-- Bootstrap -->
<%--<link rel="stylesheet" href="login/login-style.css" type="text/css">--%>

<style>
    .buttons {
        border-radius: 4px;
        background-color: #f4511e;
        border: none;
        color: #FFFFFF;
        text-align: center;
        font-size: 28px;
        padding: 20px;
        width: 200px;
        transition: all 0.5s;
        cursor: pointer;
        margin: 5px;
    }

        .buttons span {
            cursor: pointer;
            display: inline-block;
            position: relative;
            transition: 0.5s;
        }

            .buttons span:after {
                content: '\00bb';
                position: absolute;
                opacity: 0;
                top: 0;
                right: -20px;
                transition: 0.5s;
            }

        .buttons:hover {
            background-color: black;
            color: black;
        }

            .buttons:hover span {
                padding-right: 25px;
            }

                .buttons:hover span:after {
                    opacity: 1;
                    right: 0;
                }
</style>

</head>
<body>

    <div id="container">
        <form id="Login1" runat="server">
            <div class="login">LOGIN</div>
            <div class="username-text">
                <asp:Label runat="server" ID="lblUserName" Text="User Name"></asp:Label>
            </div>
            <div class="password-text">
                <asp:Label runat="server" ID="lblPassword" Text="Password"></asp:Label>
            </div>
            <div class="username-field">
                <asp:TextBox runat="server" ID="txtUserName" autocomplete="off"></asp:TextBox><br />
                <br />
                <asp:RequiredFieldValidator runat="server" ID="RFVUserName" Visible="true"
                    ControlToValidate="txtUserName" ErrorMessage="UserName Can't be Empty" Display="Dynamic"
                    SetFocusOnError="True"></asp:RequiredFieldValidator>
            </div>
            <div class="password-field">
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password"></asp:TextBox><br />
                <br />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" Visible="true"
                    ControlToValidate="txtPassword" ErrorMessage="Password Can't be Empty" Display="Dynamic"
                    SetFocusOnError="True"></asp:RequiredFieldValidator>
            </div>

            <div style="float: right">
                <asp:Button runat="server" ID="btn_Submit" class="button save" Text="Go"
                    OnClick="btn_Submit_Click" Style="cursor: pointer" />
            </div>
            <div>
                <asp:Button ID="ButtonYes" Text="" OnClick="ButtonYes_Click" runat="server" />
                <asp:Button ID="ButtonNo" Text="" OnClick="ButtonNo_Click" runat="server" />
            </div>

            <%-- jus a min --%>

            <script type="text/javascript">
                function show() {
                    if (confirm("Do You Want To Logout all other session And Continue..?")) {
                        $("#<%=ButtonYes.ClientID %>").click();
                    }
                    else {

                        $("#<%=ButtonNo.ClientID %>").click();
                    }
                }

                function alert() {
                    alert("Invalid UserName/Password");
                }

            </script>


            <asp:Label ID="lblerror" runat="server" Text="" Style="color: Red; margin-left: 210px;">  </asp:Label>
            <asp:Label ID="lblcname" runat="server" Style="display: none"></asp:Label>


            <!-- Modal Popup -->
            <div id="MyPopup" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content" style="margin-top: 180px; width: 450px; font-size: 16px; margin-left: 80px;">
                        <div class="modal-header" style="color: black; font-size: 16px">
                            <button type="button" class="close" data-dismiss="modal">
                                &times;</button>
                            <h4 class="modal-title"></h4>
                        </div>
                        <div class="modal-body" style="color: black">
                        </div>
                        <div class="modal-footer" style="height: 60px">
                            <asp:Button ID="btnOk" Text="Ok" runat="server" Class="buttons" Style="height: 35px; width: 70px; padding: 0px; font-size: 14px; float: right; margin-top: -10px" />
                            <button type="button" class="btn btn-" data-dismiss="modal" style="visibility: hidden">
                                Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal Popup -->

            <asp:HiddenField ID="hfv" runat="server" />

            <script type="text/javascript">
                function ShowPopup(title, body, BtnText, Width) {
                    debugger;

                    $("#MyPopup .modal-title").html(title);
                    $("#MyPopup .modal-body").html(body);
                    $("#MyPopup").modal("show");
                    $("#btnOk").val(BtnText);
                    $("#btnOk").width(Width);
                }

                $('#btnOk').click(function () {

                    if ($("#<%=hfv.ClientID %>").val() == 'INACTIVE') {
                        window.location.href = 'Login.aspx';
                        return false;
                    }
                    else {
                        window.location.href = 'Home.aspx';
                        return false;
                    }
                });
            </script>
        </form>
    </div>

</body>
</html>


