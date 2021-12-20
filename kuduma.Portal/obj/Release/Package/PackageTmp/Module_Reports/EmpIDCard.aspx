<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="EmpIDCard.aspx.cs" Inherits="Kuduma.Portal.EmpIDCard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
     <script src="../script/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../script/jscript.js"></script>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8/jquery.min.js"></script>


    <style type="text/css">
        .style1 {
            width: 135px;
        }

        .drpdwn {
            width: 350px;
            padding: 10px;
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

     <script src="../script/chosen.jquery.js" type="text/javascript"></script>
    <link href="../css/chosen.css" rel="stylesheet" />

   <script type="text/javascript">
       jQuery(document).ready(function mchoose() {
           jQuery(".chosen").data("placeholder", "Select Frameworks...").chosen();
       });
    </script>
        <!-- CONTENT AREA BEGIN -->
        <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <%--<li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                        <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                        <li class="active"><a href="EmpIDCard.aspx" style="z-index: 7;" class="active_bread">EMPLOYEE ID CARD</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox" style="background:none">
                            <div class="boxhead">
                                <h2 style="text-align: center">ID CARD
                                </h2>
                            </div>
                            <div  style="padding: 5px 5px 5px 5px;">
                                <div >
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>

                                     <div align="center"> <asp:Label ID="lblMsg" runat="server" style="border-color: #f0c36d;background-color: #f9edbe;width:auto;font-weight:bold;color:#CC3300;"></asp:Label></div> 
                    <div align="center"> <asp:Label ID="lblSuc" runat="server" style="border-color: #f0c36d;background-color: #f9edbe;width:auto;font-weight:bold;color:#000;"></asp:Label></div> 
              
                                     <table style="margin-top:8px;margin-bottom:8px" width="80%">
                                        
                        <tr>
                            <td style="font-weight: bold;width:120px">
                                Employee ID / Name:
                            </td>
                            <td>
                              
                                <asp:ListBox ID="lstEmpIdName" runat="server"  SelectionMode="Multiple" class="chosen" Width="150px"
                                ToolTip="Enter Searched Employee ID Or Name"></asp:ListBox>
                            </td>
                           
                            <td>
                         <asp:Button ID="BtnIDCard" runat="server" Text="Download ID Card" OnClick="BtnIDCard_Click" />

                            </td>

                            <td>
                         <asp:Button ID="Button1" runat="server" Text="Download ID Card old" OnClick="Button1_Click" Visible="false" />

                            </td>
                        </tr>
                    </table>

                                                           
            <div style="height:auto">
                           
                                    <table cellspacing="5" cellpadding="5" border="0" style="height: 50px" width="100%">
                                        <tr>
                                            <td width="20%">
                                               <%-- Operational Manager<span style=" color:Red">*</span>--%>
                                            </td>
                                            <td width="30%">
                                              <%--  <asp:DropDownList ID="ddloperationalmanager" runat="server" Width="160px" 
                                                    AutoPostBack="True" 
                                                    onselectedindexchanged="ddloperationalmanager_SelectedIndexChanged">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                </asp:DropDownList>--%>
                                            </td>
                                            
                                        </tr>
                                    </table>  
                                    </div>
                             
                               
                               <%-- <div class="dashboard_FirstOfThree">--%>
                                <div class="rounded_corners">
                                    <asp:GridView ID="GvSearchEmp" runat="server" AllowPaging="True" AutoGenerateColumns="False" width="98%"
                                            CellPadding="5" CellSpacing="3" ForeColor="#333333" EmptyDataText="No Records Found" 
                                            GridLines="None" BorderStyle="Outset" PageSize="100" OnPageIndexChanging="GvSearchEmp_PageIndexChanging">
                                            <RowStyle BackColor="#EFF3FB" Height="30px" HorizontalAlign="Left" 
                                                VerticalAlign="Middle" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                                Height="20px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkindividual" runat="server"   onclick = "Check_Click(this)" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                          <%--  <asp:TemplateField HeaderText="Emp ID">
                                                                <ItemTemplate>
                                                                   <%-- <asp:Label ID="lblempid" runat="server" Text='<%#Eval("empid") %>' />
                                                               

                                                                     </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                  <asp:BoundField DataField="empid" HeaderText="ID NO"  />
                                                            <asp:BoundField DataField="FullName" HeaderText="Name" />
                                                            <asp:BoundField DataField="Designation" HeaderText="Designation" />

                                                            <asp:BoundField DataField="EmpDtofJoining" HeaderText="Date Of Joining"  />


                                                        </Columns>
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                        <EditRowStyle BackColor="#2461BF" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
                                                        </div>
                                             </div>                          
                                            </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
        
                    <div class="clear">
                    </div>
               
            <!-- DASHBOARD CONTENT END -->
            </asp:content>
