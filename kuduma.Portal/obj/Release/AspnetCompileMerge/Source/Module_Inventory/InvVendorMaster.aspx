<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="InvVendorMaster.aspx.cs" Inherits="Kuduma.Portal.InvVendorMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <script type="text/javascript">
        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }


        function checkAll(objRef) {

            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        inputList[i].checked = false;
                    }
                }
            }
        }

    </script>

        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                       <li class="first"><a href="ViewItems.aspx" style="z-index: 9;"><span></span>Inventory</a></li>
                        <li class="active"><a href="InvVendorMaster.aspx" style="z-index: 7;" class="active_bread">Vendor Details </a></li>
                    </ul>
                </div>

                <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

                <div class="dashboard_full">
                    <div style="float: right; font-weight: bold">
                    </div>
                    <!-- DASHBOARD CONTENT BEGIN -->
                    <div class="contentarea" id="contentarea">

                        <div class="sidebox">
                            <div class="boxhead">

                                <h2 style="text-align: center">Vendor Details  
                                </h2>
                            </div>
                            <div class="contentarea" id="Div1">
                                <div class="boxinc">

                                    <ul>

                                        <li class="right">

                                            <table width="140%" cellpadding="5" cellspacing="5" style="margin-left: 10px">
                                                <tr>
                                                    <td>
                                                        <table width="100%" cellpadding="5" cellspacing="5" style="margin: 10px">
                                                           <%-- <tr style="height: 32px">
                                                                 <td>
                                                                            Branch
                                                                        </td>
                                                                        <td>
                                                       <asp:DropDownList ID="ddlbranch" runat="server" Width="228px" Enabled="false" class="form-control">

                                                                           </asp:DropDownList>
                                                                        </td>
                                                            </tr>--%>
                                                            <tr style="height: 32px">
                                                                <td>Vendor ID
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVendorId" runat="server" class="form-control" Width="190px" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 32px">
                                                                <td>Contact Person 
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtContactPerson" runat="server" class="form-control" Width="190px"></asp:TextBox>

                                                                </td>
                                                            </tr>

                                                            <tr style="height: 32px">
                                                                <td>Address
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAddress" runat="server" class="form-control" Width="190px" TextMode="MultiLine" Height="50px"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 32px">
                                                                <td>Email ID
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtemailid" runat="server" class="form-control" Width="190px" Text=""> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>

                                                    <td valign="top">
                                                        <table width="100%" cellpadding="5" cellspacing="5" style="margin: 10px">
                                                            <tr style="height: 32px">
                                                                <td>Vendor Name
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVendorName" runat="server" class="form-control" Width="190px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 32px">
                                                                <td>Contact Nos 
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtContactNos" runat="server" class="form-control" Width="190px"></asp:TextBox>

                                                                </td>
                                                            </tr>

                                                            <tr style="height: 36px">
                                                                <td>Remarks
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRemarks" runat="server" class="form-control" Width="228px" TextMode="MultiLine" Height="50px"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                              <tr style="height: 32px">
                                                                <td>Vendor GSTNo
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtvendorgst" runat="server" class="form-control" Width="190px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div align="right" style="margin-right: -100px; margin-top: 7px">
                                                <asp:Label ID="lblresult" runat="server" Text="" Visible="false" Style="color: Red"></asp:Label>
                                                <asp:Button ID="Button1" runat="server" ValidationGroup="a1" Text="Save" OnClientClick='return confirm("Are you sure you want to add this Item?");'
                                                    ToolTip="SAVE" class=" btn save" OnClick="BtnSave_Click" />
                                                <asp:Button ID="btncancel" runat="server" ValidationGroup="a1" Text="Cancel" ToolTip="CANCEL"
                                                    class=" btn save" OnClientClick='return confirm("Are you sure you want to cancel this entry?");' />
                                            </div>

                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                <ContentTemplate>

                                                    <div class="rounded_corners" style="margin-left: 0px; width: 953px; margin-right: 0px; margin-top: 15px;overflow:scroll">
                                                        <asp:GridView ID="GVItemList" runat="server" AutoGenerateColumns="False" Width="968px" CssClass="table table-striped table-bordered table-condensed table-hover"
                                                            CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="none" OnRowDataBound="GVItemList_RowDataBound">

                                                             <Columns>

                                                                <asp:TemplateField HeaderStyle-Width="10px">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Item ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblitemid" runat="server" Text='<%#Bind("itemid") %>' Width="70px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Item Name" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="300px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblitemname" runat="server" Text='<%#Bind("itemname") %>' Width="300px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="HSN No" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblHSNNo" runat="server" Text='<%#Bind("HSNNumber") %>' Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Buying Price" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblBuyingPrice" runat="server" Text='<%#Bind("BuyingPrice","{0:0.##}") %>' Width="90px" CssClass="form-control" AutoPostBack="true" OnTextChanged="lblBuyingPrice_OnTextChanged"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="GST Per." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGSTPer" runat="server" Text='<%#Bind("GSTPer","{0:0.##}") %>' Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="GST Amt" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGSTAmt" runat="server" Text='<%#Bind("GSTAmt","{0:0.##}") %>' Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderText="VATCmp1" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVATCmp1" runat="server" Text='<%#Bind("VATCmp1","{0:0.##}") %>' Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="VATCmp2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVATCmp2" runat="server" Text='<%#Bind("VATCmp2","{0:0.##}") %>' Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="VATCmp3" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVATCmp3" runat="server" Text='<%#Bind("VATCmp3","{0:0.##}") %>' Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="VATCmp4" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVATCmp4" runat="server" Text='<%#Bind("VATCmp4","{0:0.##}") %>' Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="VATCmp5" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVATCmp5" runat="server" Text='<%#Bind("VATCmp5","{0:0.##}") %>' Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotal" runat="server" Text='<%#Bind("Total","{0:0.##}") %>' Width="90px"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>

                                                        </asp:GridView>
                                                    </div>


                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </li>
                                    </ul>
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
       
   </asp:Content>
