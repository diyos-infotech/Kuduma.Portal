<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="EmpWisePaysheetDetails.aspx.cs" Inherits="Kuduma.Portal.EmpWisePaysheetDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
     <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <style type="text/css">
        .style2
        {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
    </style>
    <script type="text/javascript">
         function AssignExportHTML() {

             document.getElementById('hidGridView').value = htmlEscape(forExport.innerHTML);
         }
         function htmlEscape(str) {
             return String(str)
             .replace(/&/g, '&amp;')
             .replace(/"/g, '&quot;')
             .replace(/'/g, '&#39;')
             .replace(/</g, '&lt;')
             .replace(/>/g, '&gt;');
         }
   
    function onCalendarShown() {

             var cal = $find("calendar1");
             //Setting the default mode to month
             cal._switchMode("months", true);

             //Iterate every month Item and attach click event to it
             if (cal._monthsBody) {
                 for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                     var row = cal._monthsBody.rows[i];
                     for (var j = 0; j < row.cells.length; j++) {
                         Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                     }
                 }
             }
         }

         function onCalendarHidden() {
             var cal = $find("calendar1");
             //Iterate every month Item and remove click event from it
             if (cal._monthsBody) {
                 for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                     var row = cal._monthsBody.rows[i];
                     for (var j = 0; j < row.cells.length; j++) {
                         Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                     }
                 }
             }

         }

         function call(eventElement) {
             var target = eventElement.target;
             switch (target.mode) {
                 case "month":
                     var cal = $find("calendar1");
                     cal._visibleDate = target.date;
                     cal.set_selectedDate(target.date);
                     cal._switchMonth(target.date);
                     cal._blur.post(true);
                     cal.raiseDateSelectionChanged();
                     break;
             }
         }
         </script>

    <script type="text/javascript">
         debugger
         function GetEmpid() {

             $("#<%=txtEmpid.ClientID %>").autocomplete({
                    source: function (request, response) {
                        var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                        var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpIDs";
                        $.ajax({
                            url: ajaxUrl,
                            method: 'post',
                            contentType: 'application/json;charset=utf-8',

                            data: JSON.stringify({
                                term: request.term,
                            }),
                            datatype: 'json',
                            success: function (data) {
                                response(data.d);
                            },
                            error: function (err) {
                                alert(err);
                            }
                        });
                    },
                    minLength: 4,
                    select: function (event, ui) {

                        $("#<%=txtEmpid.ClientID %>").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                }
                });
            }

            function GetEmpName() {

                $("#<%=txtName.ClientID %>").autocomplete({
                    source: function (request, response) {
                        var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                        var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpNames";
                        $.ajax({

                            url: ajaxUrl,
                            method: 'post',
                            contentType: 'application/json;charset=utf-8',
                            data: JSON.stringify({
                                term: request.term
                            }),
                            datatype: 'json',
                            success: function (data) {
                                response(data.d);
                            },
                            error: function (err) {
                                alert(err);
                            }
                        });
                    },
                    minLength: 4,
                    select: function (event, ui) {
                        $("#<%=txtName.ClientID %>").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                }
                });

            }

            function OnAutoCompletetxtEmpidchange(event, ui) {
                $("#<%=txtEmpid.ClientID %>").trigger('change');

            }
            function OnAutoCompletetxtEmpNamechange(event, ui) {
                $("#<%=txtName.ClientID %>").trigger('change');

            }

            $(document).ready(function () {

                GetEmpid();
                GetEmpName();
            });



    </script>
    <style type="text/css">
        .style1 {
            width: 135px;
        }

         .completionList {
        

        background: white;
	    border: 1px solid #DDD;
	    border-radius: 3px;
	    box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
	    min-width: 165px;

        height: 120px;
        overflow:auto;
             
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
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
<%--                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>--%>
                    <li class="first"><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="EmpWisePaysheetDetails.aspx" style="z-index: 7;" class="active_bread">EmpWise Paysheet Details</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                             EmpWise Paysheet Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                            
                        <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                        </asp:ScriptManager>
                        
                                            <div align="right">
                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False" OnClientClick="AssignExportHTML()">Export to Excel</asp:LinkButton>
                                               
                                            
                                            </div>
                        <div class="dashboard_firsthalf" style="width: 100%">
                            
                            <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td >
                                                        <asp:Label runat="server" ID="lblempid" Text="Emp ID  :" Width="60px"></asp:Label></td>
                                                    <td>                                                                  
                                                         <asp:TextBox runat="server" ID="txtEmpid" class="form-control" AutoPostBack="true" OnTextChanged="txtEmpid_TextChanged" Width="190px"></asp:TextBox>
                                                    </td>
                                                    <td >&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblempname" Text="Name :" Width="50px" ></asp:Label></td>

                                                    <td>                                   
                                              <asp:TextBox ID="txtName" runat="server"  TabIndex="2" class="form-control" Width="190px" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>
                                                    </td>
                                            <td>
                                                Month :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmonth" runat="server" Text=""  AutoComplete="off" class="sinput" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server"  BehaviorID="calendar1"
                                                            Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden"  OnClientShown="onCalendarShown">
                                                        </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 30%">
                                            <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                                <asp:HiddenField ID="hidGridView" runat="server" />
                            <div id="forExport" class="rounded_corners" style="overflow:scroll">
                                <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                   CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="Both" ShowFooter="true" 
                                    onpageindexchanging="GVListEmployees_PageIndexChanging" 
                                    onrowdatabound="GVListEmployees_RowDataBound">
                                    
                                    <Columns>
                                               
                                                 <%-- 0--%>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                 <%-- 1--%>
                                                <asp:TemplateField HeaderText="Client ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientid" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <%-- 2--%>

                                                <asp:TemplateField HeaderText="Client Name" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180px">
                                                    <HeaderStyle Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientname" runat="server" Text='<%#Bind("clientname") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <%-- 3--%>
                                                <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblempid" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 4--%>
                                                <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblempname" runat="server" Text='<%#Bind("EmpMname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <%-- 5--%>
                                                <asp:TemplateField HeaderText="Desgn" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldesgn" runat="server" Text='<%#Bind("Desgn") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 6--%>
                                                <asp:TemplateField HeaderText="Month-Year" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmonth" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                         <%-- 7--%>
                                                <asp:TemplateField HeaderText="Duties" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldutyhrs" runat="server" Text='<%#Bind("NoOfDuties") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDuties"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 8--%>
                                                <asp:TemplateField HeaderText="OTs" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOts" runat="server" Text='<%#Bind("OTs") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalOTs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 9--%>
                                                <asp:TemplateField HeaderText="WO" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblwos" runat="server" Text='<%#Bind("WO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalwos"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 10--%>
                                                <asp:TemplateField HeaderText="Nhs" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNhs" runat="server" Text='<%#Bind("NHS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNhs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 11--%>
                                                <asp:TemplateField HeaderText="Npots" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNpots" runat="server" Text='<%#Bind("npots") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNpots"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 12--%>
                                                <asp:TemplateField HeaderText="Sal Rate" ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltempgross" runat="server" Text='<%#Bind("TempGross") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotaltempgross"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                               
                                                <%-- 13--%>

                                                <asp:TemplateField HeaderText="Basic" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblbasic" runat="server" Text='<%#Bind("basic") %>'>--%>
                                                        <asp:Label ID="lblbasic" runat="server" Text='<%#Eval("basic", "{0:0}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBasic"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 14--%>
                                                <asp:TemplateField HeaderText="DA" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblda" runat="server" Text='<%#Eval("da","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 15--%>
                                                <asp:TemplateField HeaderText="HRA" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblhra" runat="server" Text='<%#Bind("hra","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalHRA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 16--%>
                                                <asp:TemplateField HeaderText="CCA" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcca" runat="server" Text='<%#Bind("CCa","{0:0}") %>'>  
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalCCA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 17--%>
                                                <asp:TemplateField HeaderText="Conv" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConveyance" runat="server" Text='<%#Bind("conveyance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalConveyance"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 18--%>
                                                <asp:TemplateField HeaderText="W.A." ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblwashallowance" runat="server" Text='<%#Bind("WashAllowance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalWA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 19--%>
                                                <asp:TemplateField HeaderText="O.A." ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOtherallowance" runat="server" Text='<%#Bind("OtherAllowance","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalOA"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 20--%>
                                                <asp:TemplateField HeaderText="L.W" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLeaveEncashAmt" runat="server" Text='<%#Bind("LeaveEncashAmt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalLeaveEncashAmt"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 21--%>
                                                <asp:TemplateField HeaderText="Gratuity" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGratuity" runat="server" Text='<%#Bind("Gratuity","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalGratuity"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 22--%>
                                                <asp:TemplateField HeaderText="Bonus" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBonus" runat="server" Text='<%#Bind("Bonus","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalBonus"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 23--%>
                                                <asp:TemplateField HeaderText="Nfhs" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNfhs" runat="server" Text='<%#Bind("Nfhs","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNfhs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 24--%>
                                                <asp:TemplateField HeaderText="RC" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrc" runat="server" Text='<%#Bind("rc","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalrc"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 25--%>
                                                <asp:TemplateField HeaderText="CS" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcs" runat="server" Text='<%#Bind("cs","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalcs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                 <%-- 26--%>

                                                <asp:TemplateField HeaderText="Incentivs" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIncentivs" runat="server" Text='<%#Bind("Incentivs","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalIncentivs"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                
                                                <%-- 27--%>

                                                <asp:TemplateField HeaderText="WO Amt" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWoAmt" runat="server" Text='<%#Bind("WOAmt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalWOAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 28--%>

                                                <asp:TemplateField HeaderText="NHs Amt" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNhsAmt" runat="server" Text='<%#Bind("Nhsamt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNhsAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%-- 29--%>

                                                <asp:TemplateField HeaderText="NPOTs Amt" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNpotsAmt" runat="server" Text='<%#Bind("Npotsamt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNpotsAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                               


                                                <%-- 30--%>
                                                <asp:TemplateField HeaderText="Gross" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGross" runat="server" Text='<%#Bind("Gross","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalGross"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <%-- 31--%>

                                                <asp:TemplateField HeaderText="OT Amt" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOTAmt" runat="server" Text='<%#Bind("OTAmt","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalOTAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                

                                                
                                                <%-- 32--%>
                                                <asp:TemplateField HeaderText="PF" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPF" runat="server" Text='<%#Bind("PF","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalPF"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 33--%>
                                                <asp:TemplateField HeaderText="ESI" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblESI" runat="server" Text='<%#Bind("ESI","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalESI"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 34--%>
                                                <asp:TemplateField HeaderText="ProfTax" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProfTax" runat="server" Text='<%#Bind("ProfTax","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalProfTax"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 35--%>
                                                <asp:TemplateField HeaderText="Sal.Adv" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsaladv" runat="server" Text='<%#Bind("SalAdvDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalsaladv"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 36--%>
                                            <asp:TemplateField HeaderText="ADV Ded" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbladvded" runat="server" Text='<%#Bind("ADVDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                 <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotaladvded"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                                 <%-- 37--%>
                                            <asp:TemplateField HeaderText="WC Ded" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwcded" runat="server" Text='<%#Bind("WCDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalwcded"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <%-- 38--%>
                                            <asp:TemplateField HeaderText="U.D." ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluniform" runat="server" Text='<%#Bind("UniformDed","{0:0}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: justify">
                                                        <asp:Label runat="server" ID="lblTotalUniformDed"></asp:Label>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>




                                                <%-- 39--%>
                                                <asp:TemplateField HeaderText="Other Ded" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOtherDed" runat="server" Text='<%#Bind("OtherDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalOtherDed"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 40--%>
                                                <asp:TemplateField HeaderText="Total Loan ded" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltotalloanded" runat="server" Text='<%#Bind("LoanDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotaltotalloanded"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 41--%>
                                                <asp:TemplateField HeaderText="C.A" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcantadv" runat="server" Text='<%#Bind("CanteenAdv","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalcantadv"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 42--%>
                                                <asp:TemplateField HeaderText="Sec Dep" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSecDepDed" runat="server" Text='<%#Bind("SecurityDepDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalSecDepDed"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 43--%>
                                                <asp:TemplateField HeaderText="Gen Ded" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGeneralDed" runat="server" Text='<%#Bind("GeneralDed","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalGeneralDed"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>



                                                <%-- 44--%>
                                                <asp:TemplateField HeaderText="OWF" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblowf" runat="server" Text='<%#Bind("OWF","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalowf"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 45--%>
                                                <asp:TemplateField HeaderText="Penalty" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPenalty" runat="server" Text='<%#Bind("Penalty","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div style="text-align: justify">
                                                            <asp:Label runat="server" ID="lblTotalPenalty"></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 46--%>
                                                <asp:TemplateField HeaderText="Total Ded" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeductions" runat="server" Text='<%#Bind("TotalDeductions","{0:0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalDeductions"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <%-- 47--%>
                                                <asp:TemplateField HeaderText="Net Amt" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblnetamount" runat="server" Text='<%#Bind("ActualAmount","{0:0}") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTotalNetAmount"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                 <%-- 48--%>
                                                <asp:TemplateField HeaderText="Bank A/C No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbankno" runat="server" Text='<%# Eval("Empbankacno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <%-- 49--%>
                                                <asp:BoundField DataField="EmpBankCardRef" HeaderText="Reference No." DataFormatString="{0}&nbsp;" />
                                                 <%-- 50--%>
                                                <asp:BoundField DataField="EmpIFSCcode" HeaderText="IFSC Code" DataFormatString="{0}&nbsp;" />
                                                 <%-- 51--%>
                                                <asp:BoundField DataField="Empbankname" HeaderText="Bank Name" DataFormatString="{0}&nbsp;" />
                                            </Columns>
                                    <FooterStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                    <PagerStyle BackColor="white" ForeColor="black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                    <EditRowStyle BackColor="white" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                            
                               
                       </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
       </asp:content>
