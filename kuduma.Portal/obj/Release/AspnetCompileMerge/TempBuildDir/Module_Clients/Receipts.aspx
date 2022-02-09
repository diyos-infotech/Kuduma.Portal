<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Clients/Clients.master" CodeBehind="Receipts.aspx.cs" Inherits="Kuduma.Portal.Receipts" %>
<asp:content id="RightOne" contentplaceholderid="ContentPlaceHolder3" runat="Server">

<!-- CONTENT AREA BEGIN -->
<div id="content-holder">
    <div class="content-holder">
        <!-- DASHBOARD CONTENT BEGIN -->
        <div class="contentarea" id="contentarea">
            <ul class="shortcuts-r" style="margin-left: 13px">

                <li><a href="AddReceipts.aspx"><span class="shortcuts-icon iconsi-event"></span>
                    <span class="shortcuts-label">Add Receipt Details</span> </a></li>

                <li><a href="ReceiptDetails.aspx"><span class="shortcuts-icon iconsi-event"></span>
                    <span class="shortcuts-label">Receipt Details</span> </a></li>

            </ul>
            <ul class="shortcuts-re" style="margin-left: 13px">

                <li><a href="ReceiveReports.aspx"><span class="shortcuts-icon iconsi-event"></span>
                    <span class="shortcuts-label">Receipts Report</span> </a></li>

                <li><a href="BillVsReceipts.aspx"><span class="shortcuts-icon iconsi-event"></span>
                    <span class="shortcuts-label">Bills Vs Receipts</span> </a></li>
            </ul>

            <div class="clear">
            </div>
        </div>
    </div>
    <!-- DASHBOARD CONTENT END -->
    <!-- CONTENT AREA END -->
</div>
</asp:content>