<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="companyinfo.aspx.cs" Inherits="Kuduma.Portal.companyinfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tdsize
        {
            height: 15px;
        }
        .style8
        {
            width: 335px;
            height: 29px;
        }
    </style>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">
                CompanyInfo Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                ADD COMPANY INFORMATION
                            </h2>
                        </div>
                         <asp:ScriptManager runat="server" ID="Scriptmanager1">
                                </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin" style="min-height: 560px;" >
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td valign="top">
                                                <table width="100%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>
                                                            Company Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcname" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                           
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Company Short Name<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcsname" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="style8">
                                                        <td>
                                                            Address
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtaddress" runat="server" TextMode="MultiLine" class="sinput" Height="100px" Enabled="False"></asp:TextBox>
                                                            &nbsp;
                                                           
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                            Phone No</td>
                                                        <td>
                                                           <asp:TextBox ID="txtPhoneno" runat="server" class="sinput"  Enabled="False"></asp:TextBox>
                                                         <%--  <cc1:FilteredTextBoxExtender ID="FilterExtenderPhone" runat="server" FilterMode="ValidChars" FilterType="Numbers" 
                                                           ValidChars="0123456789" TargetControlID="txtPhoneno"></cc1:FilteredTextBoxExtender>--%>
                                                           
                                                           </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Fax No</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFaxno" runat="server" class="sinput" MaxLength="11" Enabled="False"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilterExtenderFaxno" runat="server" FilterMode="ValidChars" FilterType="Custom" 
                                                            ValidChars=".0123456789" TargetControlID="txtFaxno"></cc1:FilteredTextBoxExtender>
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Email</td>
                                                        <td>
                                                           <asp:TextBox ID="txtEmail" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                          
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Website</td>
                                                        <td>
                                                           <asp:TextBox ID="txtWebsite" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                          
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Service Tax No
                                                            <%--Bill notes replace with service tax no --%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbnotes" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            PAN No
                                                            <%--Labour rule  replace with PAN no --%>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtlabour" runat="server" MaxLength="80" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            PF No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtpfno" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            ESI No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtesino" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            P Tax No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtptaxno" runat="server" MaxLength="200" class="sinput" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                           Corporate Identity No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcorporateIDNo" runat="server"  class="sinput"
                                                                Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Reg.No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtregno" runat="server"  class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                      <tr>
                                                        <td>
                                                            CINNo
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcinno" runat="server"  class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            MSME No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMSMEno" runat="server"  class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Recipient No </td>
                                                         <td>
                                                            <asp:TextBox ID="txtRecipientNo" runat="server"  class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    
                                                   
                                                </table>
                                            </td>
                                            <td align="right">
                                                <table width="100%" cellpadding="5" cellspacing="5" >
                                                    <tr>
                                                        <td>
                                                            Billsq<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbillsq" runat="server" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                    <tr>
                                                        <td>
                                                            Notes
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" class="sinput" Height="100px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>
                                                            Bill Description
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbilldesc" runat="server" TextMode="MultiLine" class="sinput"
                                                                Height="35px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Company Info
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcinfo" runat="server" TextMode="MultiLine" class="sinput" Height="35px" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Category
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCategory" runat="server"  class="sinput"  Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                     <tr >
                                                        <td>
                                                            ESIC No for Forms
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtESICNoForms" runat="server"  class="sinput"  Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr >
                                                        <td>
                                                            Branch Office
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBranchOffice" runat="server"  class="sinput"  Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>GST No</td>
                                                        <td>
                                                            <asp:TextBox ID="txtGSTNo" runat="server"  class="sinput"  Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                     <tr >
                                                        <td>
                                                          HSN NUMBER
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtHsnNummber" runat="server"  class="sinput"  Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr> <tr >
                                                        <td>
                                                          SAC CODE
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSacCode" runat="server"  class="sinput"  Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                      <tr>
                                                        <td>
                                                            ACCOUNT NO
                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAccountno" runat="server" MaxLength="50" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                      <tr>
                                                        <td>
                                                            Bank Name
                                                            
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBANK" runat="server" MaxLength="50" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                 
                                                    <tr>
                                                        <td>
                                                            IFSC CODE
                                                           
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtifsccode" runat="server" MaxLength="100" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                 
                                                    <tr>
                                                        <td>
                                                            Branch
                                                        </td>
                                                        <td>
                                                           <asp:TextBox ID="txtbranch" runat="server" MaxLength="100" class="sinput" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                     <tr style="visibility:hidden">
                                                        <td>
                                                           ISO CERFT NO
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtISOCertNo" runat="server"  class="sinput"  Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                     <tr style="visibility:hidden">
                                                        <td>
                                                          PSARA ACT REG NO
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPsaraAct" runat="server"  class="sinput"  Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility:hidden">
                                                        <td>
                                                          KSSA MEMBERSHIP NO
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtKSSAMemberShipNo" runat="server"  class="sinput"  Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                  <%--  <tr>
                                                        <td>
                                                            PREPARE
                                                             
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPREPARE" runat="server" MaxLength="300" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                    
                                                  
                                                    <tr>
                                                        <%--<td>
                                                            Company Logo
                                                        </td>--%>
                                                        <td>
                                                            <%--<img id="imglogo" runat="server" height="100" width="100" alt="Ther IS No Image" />
                                                            <div style="margin-top: 10px">
                                                                <asp:Button ID="btnphoto" runat="server" Text="Select Photo" class="btn save" OnClick="btnphoto_Click"
                                                                    OnClientClick="beforeadd()" style="width:100px" />
                                                                <asp:FileUpload ID="fcpicture" runat="server" Visible="true" OnDataBinding="btnphoto_Click" />--%>
                                                                <asp:Label ID="lblresult" runat="server" Style="color: Red" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style=" float: right;">
                                     <asp:Button ID="btnEdit" runat="server" Text="EDIT" ToolTip="Add Client" class=" btn save"
                                        ValidationGroup="a1" OnClick="btnEdit_Click"   />
                                    <asp:Button ID="btnaddclint" runat="server" Text="SAVE" ToolTip="Add Client" class=" btn save" Enabled="false"
                                        ValidationGroup="a1" OnClick="btnaddclint_Click" OnClientClick='return confirm(" Are you sure you  want to add this record ?");' />
                                    <asp:Button ID="btncancel" runat="server" Text="CANCEL" ToolTip="Cancel Client" OnClientClick='return confirm(" Are you sure  you  want to cancel  this record?");'
                                        class=" btn save" OnClick="btncancel_Click" Enabled="false" />
                                </div>
                                 
                                                                </div>
                        </div>
                    
                </div>
                <!-- DASHBOARD CONTENT END -->
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    </div>
    <!-- CONTENT AREA END -->

</asp:Content>