<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="DocumentUpload.aspx.cs" Inherits="Kuduma.Portal.Module_Reports.DocumentUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link rel="Stylesheet" href="Styles/bootstrap.min.css" style="" />
    <link rel="Stylesheet" href="Styles/bootstrap.css" />

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/themes/start/jquery-ui.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#dialog").dialog({
                autoOpen: false,
                modal: true,
                height: 600,
                width: 600,
                title: "Zoomed Image"
            });
            $("[id*=GridView1] img").click(function () {
                $('#dialog').html('');
                $('#dialog').append($(this).clone());
                $('#dialog').dialog('open');
            });
        });
    </script>

    <div class="container">
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="Upload" CssClass="btn-primary" />
        <hr />
        <asp:GridView ID="GridView1" runat="server"
            AutoGenerateColumns="false" CssClass="table" OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="File Name" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"
                            CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="DATA"  HeaderText="File Name" />

                <asp:TemplateField HeaderText="Image">
                    <ItemTemplate>
                        <asp:Image ID="Image1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>


</asp:Content>
