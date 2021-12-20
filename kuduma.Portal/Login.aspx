<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Kuduma.Portal.Login" %>
<html lang="en-us" class="no-js"> 
<meta http-equiv="Content-Type" content="text/html;charset=UTF-8"/>
<head id="Head1" runat="server"/>
    <meta charset="utf-8"/>
     <title>FAME SOFTWARE</title>
<meta name="description" content="" />
<link href="images/interface/iOS_icon.png" rel="apple-touch-icon"/>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
<link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Open+Sans:400,700" />
<link href="login/login-style.css" rel="stylesheet" type="text/css" />
<%--<link rel="stylesheet" href="login/login-style.css" type="text/css">--%>
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

                function alert()
                {
                    alert("Invalid UserName/Password");
                }
                
            </script>


            <asp:Label ID="lblerror" runat="server" Text="" Style="color: Red; margin-left: 210px;">  </asp:Label>
            <asp:Label ID="lblcname" runat="server" Style="display: none"></asp:Label>
        </form>
    </div>

</body>
</html>
