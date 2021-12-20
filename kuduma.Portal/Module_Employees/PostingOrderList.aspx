<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Employees/EmployeeMaster.master" CodeBehind="PostingOrderList.aspx.cs" Inherits="Kuduma.Portal.PostingOrderList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />

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
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder" style="height: auto">
            <h1 class="dashboard_heading">
                Transfers Dashboard</h1>
                 <div align="right"> <b>Import Data: </b> <asp:FileUpload  ID="fileupload1" runat="server" Width="50px"/> 
                 <asp:Button ID="btnImportData" runat="server" ValidationGroup="b"
                     Text="Import"  
                        class=" btn save" onclick="btnImportData_Click"  /></div> 
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="Div1">
                <div class="dashboard_full">
                
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Posting Order
                            </h2>
                            
                            
                        </div>
                        </div>
                        <div class="contentarea">
                            <div class="boxinc">
                                <ul>
                                    <li class="left leftmenu">
                                        <ul>
                                            <li><a href="PostingOrderList.aspx" class="sel" id="PostingOrderLink" runat="server">
                                                Posting Order</a></li>
                                            <li><a href="TemproryEmployeeTransferList.aspx" id="TempTransferLink" runat="server" visible="false">
                                                Temporary Transfer</a></li>
                                            <li><a href="DummyTransfer.aspx" id="DummyTransferLink" runat="server" visible="false">Dummy Transfer</a></li>
                                            <li><a href="RemoveTransfers.aspx" id="transferlink" runat="server" >Remove Transfers</a></li>
                                        </ul>
                                    </li>
                                    <li class="right" style="height: auto">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:ScriptManager runat="server" ID="Scriptmanager1">
                                                    </asp:ScriptManager>
                                                    <!--  Content to be add here> -->
                                                    <div class="dashboard_firsthalf" style="width: 100%">
                                                        <table width="100%" style="font-size: 13px">
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" border="0" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td>
                                                                                Unit ID<span style="color: Red">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlUnit" class="sdrop" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Emp ID<span style="color: Red">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlempid" class="sdrop" runat="server" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlempid_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Order Date<span style="color: Red">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtorderdate" TabIndex="9" runat="server" class="sinput" MaxLength="10"
                                                                                        onkeyup="dtval(this,event)"></asp:TextBox>
                                                                                    <cc1:CalendarExtender ID="CEorderdate" runat="server" Enabled="true" TargetControlID="txtorderdate"
                                                                                        Format="dd/MM/yyyy">
                                                                                    </cc1:CalendarExtender>
                                                                                    <cc1:FilteredTextBoxExtender ID="FTBEorderdate" runat="server" Enabled="True" TargetControlID="txtorderdate"
                                                                                        ValidChars="/0123456789">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Joining Date<span style="color: Red">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtjoindate" TabIndex="9" runat="server" class="sinput" MaxLength="10"
                                                                                        onkeyup="dtval(this,event)"></asp:TextBox>
                                                                                    <cc1:CalendarExtender ID="CEjoindate" runat="server" Enabled="true" TargetControlID="txtjoindate"
                                                                                        Format="dd/MM/yyyy">
                                                                                    </cc1:CalendarExtender>
                                                                                    <cc1:FilteredTextBoxExtender ID="FTBEjoindate" runat="server" Enabled="True" TargetControlID="txtjoindate"
                                                                                        ValidChars="/0123456789">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Remarks
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtremarks" runat="server" class="sinput" TextMode="MultiLine"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    PF
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkpf" runat="server" Checked="true" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    ESI
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkesi" runat="server" Checked="true" />
                                                                                </td>
                                                                            </tr>
                                                                            
                                                                            <tr>
                                                                            <td>&nbsp;</td>
                                                                            <td>&nbsp;</td>
                                                                            </tr>
                                                                            
                                                                    </table>
                                                                </td>
                                                                <td align="right">
                                                                    <table width="100%" border="0" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblClientname" runat="server" Text="Unit Name"> </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlcname" class="sdrop" runat="server" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblempname" runat="server" Text="Emp Name"> </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlempname" runat="server" class="sdrop" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlempname_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Order ID
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtorderid" class="sinput" runat="server" Enabled="False"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Previous Unit ID
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPrevUnitId" class="sinput" runat="server" Enabled="False"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblDesignation" runat="server" Text="Emp Desig"> </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlDesignation" runat="server" class="sdrop">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Relieving Date<span style="color: Red">*</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtrelivingdate" TabIndex="9" runat="server" class="sinput" MaxLength="10"
                                                                                    onkeyup="dtval(this,event)"></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="CErelivingdate" runat="server" Enabled="true" TargetControlID="txtrelivingdate"
                                                                                    Format="dd/MM/yyyy">
                                                                                </cc1:CalendarExtender>
                                                                                <cc1:FilteredTextBoxExtender ID="FTBErelivingdate" runat="server" Enabled="True"
                                                                                    TargetControlID="txtrelivingdate" ValidChars="/0123456789">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                PT
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkpt" runat="server" Checked="true" />
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                         <tr>
                                                                            <td>
                                                                               Transfer Type
                                                                            </td>
                                                                            <td>
                                                                               <asp:DropDownList runat="server" ID="ddlTransferType" class="sdrop">
                                                                        <asp:ListItem Value="1">PostingOrder</asp:ListItem>
                                                                        <asp:ListItem Value="0" Selected="True">Temporary Transfer</asp:ListItem>
                                                                        <asp:ListItem Value="-1">Dumy Transfer</asp:ListItem>
                                                                        </asp:DropDownList>

                                                                            </td>
                                                                        </tr>
                                                                        
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Button ID="btntransfer" runat="server" Visible="true" Text="Transfer" class="btn save"
                                                                                    OnClick="btntransfer_Click" OnClientClick='return confirm("Are you sure you want to give posting order to this employee?");' />
                                                                                <asp:Label ID="MessageLabel" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </li>
                                </ul>
                                <table width="100%" border="0">
                                    <tr>
                                        <td width="98%" class="FormSectionHead">
                                            List of Employees working at selected Client
                                        </td>
                                    </tr>
                                    <tr style="width: 100%">
                                        <td>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvemppostingorder" runat="server" AutoGenerateColumns="False" Style="text-align: center;
                                                    font-size: 13px" Width="100%" Height="80%" CellPadding="4" CellSpacing="3" ForeColor="#333333"
                                                    GridLines="None">
                                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                                    <Columns>
                                                    
                                                     <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                                    
                                                    
                                                        <asp:TemplateField HeaderText="Emp Id" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblempid" runat="server" Text='<%#Bind("empid")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Emp Name" ItemStyle-Width="30%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblempname" runat="server" Text='<%#Bind("Name")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Designation" ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblempdesgn" runat="server" Text='<%#Bind("Desgn")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PF">
                                                            <ItemTemplate>
                                                                <%# (Boolean.Parse(Eval("pf").ToString())) ? "Yes" : "No" %></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ESI">
                                                            <ItemTemplate>
                                                                <%# (Boolean.Parse(Eval("ESI").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PT">
                                                            <ItemTemplate>
                                                                <%# (Boolean.Parse(Eval("PT").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                                               
                                                        </asp:TemplateField>
                                                        
                                                         <asp:TemplateField HeaderText="Working due to">
                                                            <ItemTemplate>
                                                      <asp:Label ID="lbltransfertype" runat="server"  Text='<%#Bind("transfettype")%>'></asp:Label>
                                                      </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                    <br />
                </div>
                <!-- DASHBOARD CONTENT END -->
            </div>
        </div>
        <!-- CONTENT AREA END -->
      </asp:content>