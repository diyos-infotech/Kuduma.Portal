<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Kuduma.Portal.Home" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>SELECT BRANCH</title>
<link href="images/interface/iOS_icon.png" rel="apple-touch-icon">
<!-- Styles -->
<link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Open+Sans:400,700">
    <link href="login/login-style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="container">
 <form id="Login1" runat="server">
    <div class="login">LOGIN</div><br /><br /><br /><br /><br /><br /><br />
    <div class="branchname-text"><asp:Label runat="server" ID="lblBranchName" Text="Branch Name"></asp:Label></div>
    
    <div>
     
      <asp:DropDownList ID="ddlbranchnames" runat="server" Width="145px" style="margin-top:40px;"  class="dropdown-field">
                                            </asp:DropDownList>
     
    </div>
    
   
    <div style="float:right;margin-top:70px;margin-right:15px">
     <asp:Button runat="server" ID="btn" class="button save" Text="Move" 
                                            style="cursor:pointer" 
            onclick="btn_Click"/>
     <asp:Button runat="server" ID="Button1" class="button save" Text="temp" 
                                            Visible="False" />
     </div>                                       
    <asp:Label ID ="lblerror" runat="server" Text="" style="color:Red;margin-left:210px;" >  </asp:Label>
  </form>
</div>
</body>
</html>
