<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="BranchResourceDetails.aspx.cs" Inherits="Kuduma.Portal.Module_Inventory.BranchResourceDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/chosen.css" rel="stylesheet" />
    <link href="../css/Calendar.css" rel="stylesheet" type="text/css" />
    <link href="../css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <script src="../script/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../script/jscript.js"> </script>
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

        .chosen{
            width:250px;
        }

        .custom-combobox-input {
            margin: 0;
            padding: 5px 10px;
        }
    </style>

     <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="ViewItems.aspx" style="z-index: 8;"><span></span>Inventory</a></li>
                        <%-- <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>--%>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Branch Resource Details</a></li>
                    </ul>
                </div>
                <asp:ScriptManager runat="server" ID="Scriptmanager1">
                </asp:ScriptManager>

                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">

                    <div class="sidebox">
                        <div class="boxhead">

                            <h2 style="text-align: center">Branch Resource Issue
                            </h2>
                        </div>
                        <div class="contentarea" id="Div1">
                            <div class="boxinc">



                                <asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                     

                                        <table cellpadding="5" cellspacing="5" width="100%" style="margin: 10px; margin-left: 20px">
                                           
                                            <tr style="height: 32px">
                                                <td>Select Branch<span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBranch" runat="server" Width="228px" CssClass="form-control" style="margin-left:-28px" ></asp:DropDownList>
                                                </td>
                                                <td>
                                                    ID
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTransactionID" runat="server" CssClass="form-control" Width="70px" Enabled="false"> </asp:TextBox>
                                                </td>
                                                <td>LR Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtLRNumber" runat="server" CssClass="form-control" Width="228px" style="margin-left:-28px"> </asp:TextBox>
                                                </td>
                                            </tr>
                                           

                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div align="right" style="margin-right: 30px; margin-top: 10px">
                                    <asp:Label ID="lblresult" runat="server" Text="" Visible="true" Style="color: Red"></asp:Label>
                                    <asp:Button ID="btnSave" runat="server" ValidationGroup="a" Text="SAVE" ToolTip="SAVE"
                                        TabIndex="5" class="btn save" OnClientClick='return confirm("Are you sure you want to generate a new loan?");'
                                        OnClick="btnSave_Click" />
                                    <asp:Button ID="btncancel" runat="server" ValidationGroup="b" TabIndex="6" Text="CANCEL"
                                        ToolTip="CANCEL" class=" btn save" OnClientClick='return confirm("Are you sure you want  to cancel this entry?");' />
                                </div>
                             
                             
                                      
                              <%--  <table width="97%" style="margin-top:-30px">
                                    <tr>
                                        
                                            <td>
                                                    <div align="right">
                                                    <asp:Button ID="btnPDF" runat="server"  TabIndex="6" Text="PDF" 
                                                        ToolTip="PDF" class=" btn save"  Style="margin-left: 20px" />
                                                        </div>
                                                </td>
                                       
                                    </tr>
                                </table>--%>
                                   <asp:UpdatePanel ID="UpGv" runat="server" UpdateMode="Conditional" >
                                    <ContentTemplate>
                                        <div class="rounded_corners" style="margin-top: 10px;">
                                           
                                              <asp:GridView ID="GVUniformGrid" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                                ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center; margin: 0px auto" Height="50px" HeaderStyle-HorizontalAlign="Center" 
                                                  >

                                                <Columns>
                                                     <%-- 0 --%>

                                                  

                                                     <%-- 1 --%>
                                                    <asp:TemplateField HeaderText="Resource ID" HeaderStyle-Width="100px"  HeaderStyle-BackColor="#fcf8e3">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblresourceid" runat="server" Text='<%#Bind("ResourceID")%>' Width="100px"></asp:Label>
                                                            <%--  --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                     <%-- 2 --%>
                                                    <asp:TemplateField HeaderText="Resource Name"  ItemStyle-HorizontalAlign="Left" HeaderStyle-BackColor="#fcf8e3" >
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblresourcename" runat="server" Text='<%#Bind("ItemName")%>' Width="400px"></asp:Label>
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>

                                                    
                                                   

                                                     <%-- 4 --%>
                                                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width="70px" HeaderStyle-BackColor="#fcf8e3">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQty" runat="server" Width="70px" Text='<%#Bind("Qty")%>' Enabled="true" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtQty_TextChanged1"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FTBEQty" runat="server" TargetControlID="txtQty"
                                                                ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <%----%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                     <%--  5--%>
                                                    <asp:TemplateField HeaderText="Price" HeaderStyle-Width="90px" HeaderStyle-BackColor="#fcf8e3">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtresourceprice" runat="server" Width="90px" Enabled="false" Text='<%#Bind("Price") %>' CssClass="form-control"></asp:TextBox>
                                                            <%----%>
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>


                                                      <%-- 3 --%>

                                                     <asp:TemplateField HeaderText="Total Amount"  ItemStyle-HorizontalAlign="Left"  HeaderStyle-BackColor="#fcf8e3" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblTotalAmount" runat="server" Text='<%#Bind("TotalAmt")%>' Width="90px" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                            <%--  --%>
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Category" HeaderStyle-Width="90px" Visible="false" HeaderStyle-BackColor="#fcf8e3">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCategory" runat="server" Width="70px" Enabled="false" Text='<%#Bind("category") %>' CssClass="form-control"></asp:Label>
                                                            <%----%>
                                                        </ItemTemplate>
                                                        
                                                    </asp:TemplateField>


                                                </Columns>
                                            </asp:GridView>





                                            <asp:Label ID="lblTotalamt" runat="server" Text=""></asp:Label>

                                        </div>

                                     </ContentTemplate>
                                    <%-- <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>




                            </div>

                            <%--   <div class="loading" align="center">

                                    <img src="assets/loader.gif" alt="" />
                                </div>--%>


                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <%--   </div>--%>
                </div>
                <div class="clear">
                </div>
                <!-- DASHBOARD CONTENT END -->
            </div>

            <!-- CONTENT AREA END -->
        </div>
</asp:Content>
