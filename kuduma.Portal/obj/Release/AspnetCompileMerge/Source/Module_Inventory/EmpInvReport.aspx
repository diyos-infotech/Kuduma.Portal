<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="EmpInvReport.aspx.cs" Inherits="Kuduma.Portal.EmpInvReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- CONTENT AREA BEGIN -->

    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="ViewItems.aspx" style="z-index: 9;"><span></span>Inventory</a></li>
                    <li class="active"><a href="POReport.aspx" style="z-index: 7;" class="active_bread">Emp Inv Details Report </a></li>
                </ul>
            </div>
            <asp:ScriptManager runat="server" ID="Scriptmanager1">
            </asp:ScriptManager>

            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">

                <div class="sidebox">
                    <div class="boxhead">

                        <h2 style="text-align: center">Emp Inv Details Report 
                        </h2>
                    </div>
                    <div class="contentarea" id="Div1">
                        <div class="boxinc">



                            <table cellpadding="5" cellspacing="5" width="80%" style="margin: 10px">

                                <tr style="height: 36px">
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch"></asp:Label><span style="color: Red">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" class="form-control" Width="228px">
                                        </asp:DropDownList>

                                    </td>
                                </tr>

                                <tr>
                                    <td>From Date<span style="color: Red">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_From_Date" Width="180px" runat="server"  class="sinput"
                                            Text=""></asp:TextBox>
                                        <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server"
                                            Enabled="true" Format="dd/MM/yyyy" TargetControlID="Txt_From_Date">
                                        </cc1:CalendarExtender>
                                        <cc1:FilteredTextBoxExtender ID="Txt_Month_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                            ValidChars="/0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>

                                    <td>To Date<span style="color: Red">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_ToDate" Width="180px" runat="server" class="sinput"
                                            Text=""></asp:TextBox>

                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server"
                                            Enabled="true" Format="dd/MM/yyyy" TargetControlID="Txt_ToDate">
                                        </cc1:CalendarExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                            runat="server" Enabled="True" TargetControlID="Txt_ToDate"
                                            ValidChars="/0123456789">
                                        </cc1:FilteredTextBoxExtender>

                                    </td>

                                    <td>
                                        <asp:Button runat="server" ID="btn_Submit" Text="Submit" OnClick="Btn_Submit_OnClick"
                                            class="btn save" /></td>

                                    <td>
                                        <asp:LinkButton ID="Lnkbtnexcel" runat="server" OnClick="Lnkbtnexcel_Click">Export to Excel</asp:LinkButton></td>

                                </tr>

                            </table>


                            <br />
                            <br />

                            <div style="overflow-x: scroll">
                                <asp:GridView ID="GVListOfItems" runat="server" AutoGenerateColumns="True" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                    CellPadding="4" ForeColor="#333333">
                                    <Columns>
                                    </Columns>

                                </asp:GridView>
                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="clear">
    </div>
    <!-- CONTENT AREA END -->
</asp:Content>
