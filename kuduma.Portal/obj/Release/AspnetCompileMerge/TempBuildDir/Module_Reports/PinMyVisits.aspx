<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" 
    CodeBehind="PinMyVisits.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.PinMyVisits" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">


    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>




    <script type="text/javascript">

        function ShowPopup() {
            $(function () {
                $("#dialog").dialog({
                    title: "Zoomed Image",
                    width: 350,
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');

                        }
                    },
                    modal: true
                });
            });
        };

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

        function GetEmpid() {

            $("#<%=txtEmpIDName.ClientID %>").autocomplete({
                source: function (request, response) {
                    var Url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = Url.substring(0, Url.lastIndexOf('/')) + "/Autocompletion.asmx/GetFormEmpIDNames";
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
                minLength: 4

            });
        }



        $(function () {

            GetEmpid();

            $("[id*=GVpinmyvisit]").find("[id*=btnview]").click(function () {


                //Reference the GridView Row.
                var row = $(this).closest("tr");

                document.getElementById("<%=hfPitstopAttachmentId.ClientID %>").value = row.find("td").eq(6).find(":text").val();
                document.getElementById("<%=btnGetImage.ClientID %>").click();


                return false;
            });


        });




    </script>


    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading"></h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Pin My Visit&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </h2>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px; height: auto">
                            <!--  Content to be add here> -->

                            <table width="75%" style="margin: 0px auto">
                                <tr>

                                    <td>Emp ID/Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmpIDName" runat="server" CssClass="form-control" Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMonth" runat="server" Text="Month"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMonth" runat="server" class="sinput" autocomplete="off"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="true" TargetControlID="txtMonth"
                                            Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                            TargetControlID="txtMonth" ValidChars="/0123456789-">
                                        </cc1:FilteredTextBoxExtender>

                                    </td>

                                    <td>
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class=" btn save" OnClick="btnSubmit_Click" ToolTip="Submit" />
                                    </td>

                                </tr>
                            </table>

                            <div style="width: 100%; margin-top: 30px">


                              

                                <asp:GridView ID="GVpinmyvisit" runat="server" CssClass="table table-striped table-bordered table-condensed table-hover" AutoGenerateColumns="false" OnRowDataBound="gvdata_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Emp ID" />
                                        <asp:BoundField DataField="EmpFName" HeaderText="Name" />
                                        <asp:BoundField DataField="UpdatedOn" HeaderText="Created On" />
                                        <asp:BoundField DataField="EmpRemarks" HeaderText="Remarks" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnview" runat="server" Text="View" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  >
                                            <ItemTemplate>
                                                <asp:TextBox ID="lblpitstopattid" runat="server" Text='<%#Bind("PitstopAttachmentId")%>' style="display:none"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>


                            </div>

                            <asp:HiddenField ID="hfPitstopAttachmentId" runat="server" />
                            <asp:Button ID="btnGetImage" runat="server" OnClick="btnGetImage_Click" Style="display: none" />

                            <div id="dialog" style="display: none">

                                <asp:Image ID="imgphoto" runat="server" Width="320" Height="300" />
                            </div>

                        </div>
                    </div>
                    <!-- DASHBOARD CONTENT END -->
                </div>
            </div>
            <!-- CONTENT AREA END -->

        </div>
    </div>




</asp:Content>
