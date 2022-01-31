<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.Master" CodeBehind="AccessPrevilegers.aspx.cs" Inherits="Kuduma.Portal.Module_Setting.AccessPrevilegers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content
    ID="RightOne"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=imgOrdersShow]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
            $("[id*=ImgsubLink]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
        });

        //        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //prm.add_initializeRequest(initializeRequest);

        //function initializeRequest(sender, args) {
        //            //document.getElementById('ploperation').style.display = 'block';
        //}

    </script>

    <style type="text/css">
        .Grid td {
            background-color: #A1DCF2;
            color: black;
            font-size: 10pt;
            line-height: 200%;
            vertical-align: text-top;
        }

        .Grid th {
            background-color: #3AC0F2;
            color: White;
            font-size: 10pt;
            line-height: 200%;
        }

        .ChildGrid {
        }

            .ChildGrid td {
                background-color: #eee !important;
                color: black;
                font-size: 10pt;
                line-height: 200%;
                min-width: 25px;
            }

            .ChildGrid th {
                background-color: #6C6C6C !important;
                color: White;
                font-size: 10pt;
                line-height: 200%;
            }

        .Nested_ChildGrid td {
            background-color: #fff !important;
            color: black;
            font-size: 10pt;
            line-height: 200%;
        }

        .Nested_ChildGrid th {
            background-color: #2B579A !important;
            color: White;
            font-size: 10pt;
            line-height: 200%;
        }

        .img {
            width: 20px;
            height: 20px;
        }

        .chk {
            width: 20px;
            height: 20px;
        }
    </style>

    <div class="contentarea" id="contentarea">
        <div class="dashboard_center">
            <div class="sidebox">
                <div class="boxhead">
                    <h2 style="text-align: center">
                        <asp:Label ID="heading1" runat="server" Text=""></asp:Label>
                    </h2>
                    <asp:HiddenField ID="HfPid" runat="server" />
                </div>
                <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                    <div class="boxin">
                        <div class="rounded_corners">
                            <asp:ScriptManager ID="ScriptManager2" runat="server">
                            </asp:ScriptManager>
                            <asp:UpdatePanel ID="up1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvParentMenu" runat="server" AutoGenerateColumns="false" CssClass="Grid"
                                        DataKeyNames="MENU_ID">
                                        <Columns>
                                            <asp:TemplateField ControlStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAll" CssClass="chk" AutoPostBack="true" EnableViewState="true" OnCheckedChanged="chkAll_CheckedChanged"
                                                        runat="server" Checked='<%#bool.Parse(Eval("Access").ToString())%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgOrdersShow" runat="server" CssClass="img" OnClick="imgChildMenu_Click"
                                                        ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                                    <asp:Panel ID="pnlOrders" runat="server" Visible="false" Style="position: relative">
                                                        <asp:GridView ID="gvChild" runat="server" AutoGenerateColumns="false"
                                                            CssClass="ChildGrid"
                                                            DataKeyNames="MENU_ID">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox CssClass="chk" ID="chkCheck" OnCheckedChanged="chkCheck_CheckedChanged" runat="server" AutoPostBack="true" Checked='<%#bool.Parse(Eval("Access").ToString())%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgsubLink" runat="server" CssClass="img" OnClick="ImgsubLink_Click"
                                                                            ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                                                        <asp:Panel ID="pnlsublink" runat="server" Visible="false" Style="position: relative">
                                                                            <asp:GridView ID="gvSubChild" runat="server" AutoGenerateColumns="false"
                                                                                CssClass="ChildGrid"
                                                                                DataKeyNames="MENU_ID">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox CssClass="chk" ID="chkSub" runat="server" AutoPostBack="true" Checked='<%#bool.Parse(Eval("Access").ToString())%>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField ItemStyle-Width="170px" DataField="MENU_ID" HeaderText="Menu Id" />
                                                                                    <asp:BoundField ItemStyle-Width="170px" DataField="REDIRECT_PAGE" HeaderText="Redirect Page" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>
                                                                <asp:BoundField ItemStyle-Width="200px" DataField="MENU_ID" HeaderText="Menu Id" />
                                                                <asp:BoundField ItemStyle-Width="200px" DataField="REDIRECT_PAGE" HeaderText="Redirect Page" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField ItemStyle-Width="250px" DataField="MENU_ID" HeaderText="Menu Id" />
                                            <asp:BoundField ItemStyle-Width="250px" DataField="REDIRECT_PAGE" HeaderText="Redirect Page" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                            <asp:Button runat="server" ID="Update" Text="Update" OnClick="Update_Click" />
                            <asp:Button ID="btnBack" runat="server" Text="Back" class="btn save" Width="120px"
                                OnClick="btnBack_Click" />
                        </div>
                    </div>
                </div>

            </div>

        </div>

    </div>

</asp:Content>
