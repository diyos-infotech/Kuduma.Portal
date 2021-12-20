<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpWeeklyAttendance.aspx.cs" MasterPageFile="~/Module_Employees/EmployeeMaster.master" Inherits="Kuduma.Portal.Module_Employees.EmpWeeklyAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Employee Weekly Attendance
                            </h2>
                        </div>

                        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>


                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <table style="margin:0px auto" width="80%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromdate" runat="server" Text="From Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromdate" runat="server" class="sinput" autocomplete="off" MaxLength="10"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Fromdate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtFromdate">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTodate" runat="server" Text="To Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTodate" runat="server" class="sinput" autocomplete="off" MaxLength="10"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Todate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtTodate">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class=" btn save" OnClick="btnSubmit_Click" ToolTip="Submit" />
                                        </td>
                                    </tr>
                                </table>

                                <div>
                                    <asp:GridView ID="GVAttendanceData" runat="server" AutoGenerateColumns="true" CssClass="table table-striped table-bordered table-condensed table-hover">
                                        <Columns>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <!-- DASHBOARD CONTENT END -->
            <%-- </div> </div>--%>
            <!-- CONTENT AREA END -->
            <!-- FOOTER BEGIN -->
        </div>
    </div>
</asp:Content>
