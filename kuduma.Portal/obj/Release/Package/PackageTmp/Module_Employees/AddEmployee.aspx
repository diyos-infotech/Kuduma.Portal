<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Employees/EmployeeMaster.master" CodeBehind="AddEmployee.aspx.cs" Inherits="Kuduma.Portal.AddEmployee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <style type="text/css">
        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $("Btn_Save_Personal_Tab").click(function () {
            ShowProgress();
        });
    </script>
    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <!-- jQuery -->

    <script type="text/javascript" src="date/jquery00.js"></script>

    <!-- required plugins -->

    <script type="text/javascript" src="date/date0000.js"></script>

    <!--[if lt IE 7]><script type="text/javascript" src="scripts/jquery.bgiframe.min.js"></script><![endif]-->
    <!-- jquery.datePicker.js -->

    <script type="text/javascript" src="date/jquery01.js"></script>

    <!-- datePicker required styles -->
    <link rel="stylesheet" type="text/css" media="screen" href="date/datePick.css">
    <!-- page specific scripts -->



    <script type="text/javascript" charset="utf-8">
        $(function () {
            $('.date-pick').datePicker({ startDate: '01/01/1996' });

        });
    </script>

    <script type="text/javascript">



        function checkDate(sender, args) {
            debugger
            var now = new Date();
            if (now.getMonth() == 11) {
                var current = new Date(now.getFullYear() + 1, 0, 1);
            } else {
                var current = new Date(now.getFullYear(), now.getMonth() + 1, 1);

                current.setDate(current.getDate() + 4);
            }

            var nowdate = new Date();
            var monthStartDay = new Date(nowdate.getFullYear(), nowdate.getMonth(), 1);

            //var monthEndDay = new Date(nowdate.getFullYear(), nowdate.getMonth() + 1, 0);
            //monthEndDay.setDate(monthStartDay.getDate() + 4);

            if (sender._selectedDate < monthStartDay) {
                alert("Please Check the Date of Joining!");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
            if (current < sender._selectedDate) {
                alert("Please Check the Date of Joining!");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }
    </script>


    <style type="text/css">
        .pstyle {
            width: 450px;
            margin: 0px auto;
        }
    </style>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            if (!args.get_isPartialLoad()) {
                //  add our handler to the document's
                //  keydown event
                $addHandler(document, "keydown", onKeyDown);
            }
        }
        function onKeyDown(e) {
            if (e && e.keyCode == Sys.UI.Key.esc) {
                // if the key pressed is the escape key, dismiss the dialog
                $find('modelExRejoin').hide();
                $("select#ddloldempdrop")[0].selectedIndex = 0;
            }
        }

        if (typeof (Sys.Browser.WebKit) == "undefined") {
            Sys.Browser.WebKit = {};
        }
        if (navigator.userAgent.indexOf("WebKit/") > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = "WebKit";
        }

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

    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>

    <script type="text/javascript">
        $(function () {

            $("#<%= ChkCriminalOff.ClientID%>").change(function () {
                var status = this.checked;
                if (status)
                    $('#<%= txtCriminalOffCName.ClientID%>').prop("disabled", false),
                        $('#<%= txtCriminalOffcaseNo.ClientID%>').prop("disabled", false),
                    $('#<%= txtCriminalOff.ClientID%>').prop("disabled", false);
                else
                    $('#<%= txtCriminalOffCName.ClientID%>').prop("disabled", true),
                            $('#<%= txtCriminalOffcaseNo.ClientID%>').prop("disabled", true),
                        $('#<%= txtCriminalOff.ClientID%>').prop("disabled", true);
            })

            $("#<%= ChkCriminalProc.ClientID%>").change(function () {
                var status = this.checked;




                if (status)
                    $('#<%= txtCriminalProCName.ClientID%>').prop("disabled", false),
                    $('#<%= txtCriminalProCaseNo.ClientID%>').prop("disabled", false),
                        $('#<%= txtCriminalProOffence.ClientID%>').prop("disabled", false);
                else
                    $('#<%= txtCriminalProCName.ClientID%>').prop("disabled", true),
                $('#<%= txtCriminalProCaseNo.ClientID%>').prop("disabled", true),
                    $('#<%= txtCriminalProOffence.ClientID%>').prop("disabled", true);
            })

            $("#<%= ChkCrimalArrest.ClientID%>").change(function () {
                var status = this.checked;
                if (status)
                    $('#<%= txtCriminalArrestCName.ClientID%>').prop("disabled", false),
                    $('#<%= txtCriminalArrestCaseNo.ClientID%>').prop("disabled", false),
                    $('#<%= txtCriminalArrestOffence.ClientID%>').prop("disabled", false);
                else
                    $('#<%= txtCriminalArrestCName.ClientID%>').prop("disabled", true),
                   $('#<%= txtCriminalArrestCaseNo.ClientID%>').prop("disabled", true),
                   $('#<%= txtCriminalArrestOffence.ClientID%>').prop("disabled", true);
            })

            $("#<%= rdbResigned.ClientID%>").change(function () {
                var status = this.checked;

                if (status)
                    $('#<%= txtDofleaving.ClientID%>').prop("disabled", false);
                else
                    $('#<%= txtDofleaving.ClientID%>').prop("disabled", true);
            })

            $("#<%= rdbactive.ClientID%>").change(function () {
                var status = this.checked;
                if (status)
                    $('#<%= txtDofleaving.ClientID%>').prop("disabled", true);
                else
                    $('#<%= txtDofleaving.ClientID%>').prop("disabled", true);
            })


            $("#<%= rdbVerified.ClientID%>").change(function () {
                var status = this.checked;
                if (status)

                    $('#<%= txtPoliceVerificationNo.ClientID%>').prop("disabled", false);

                else
                    $('#<%= txtPoliceVerificationNo.ClientID%>').prop("disabled", true);

            })

            $("#<%= rdbNotVerified.ClientID%>").change(function () {
                var status = this.checked;
                if (status)
                    $('#<%= txtPoliceVerificationNo.ClientID%>').prop("disabled", true);


                else
                    $('#<%= txtPoliceVerificationNo.ClientID%>').prop("disabled", false);

            })

            $("#<%= rdbbgvverified.ClientID%>").change(function () {
                var status = this.checked;
                if (status)

                    $('#<%= txtbgvno.ClientID%>').prop("disabled", false);

                else
                    $('#<%= txtbgvno.ClientID%>').prop("disabled", true);

            })

            $("#<%= rdbbgvnotverified.ClientID%>").change(function () {
                var status = this.checked;
                if (status)
                    $('#<%= txtbgvno.ClientID%>').prop("disabled", true);


                else
                    $('#<%= txtbgvno.ClientID%>').prop("disabled", false);

            })

        })
    </script>

    <link rel="stylesheet" href="script/jquery-ui.css" />

    <script type="text/javascript" src="script/jquery.min.js"></script>

    <script type="text/javascript" src="script/jquery-ui.js"></script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css"
        rel="stylesheet" type="text/css" />


    <script type="text/javascript">
        var currentTab = 0;
        $(function () {
            $("#tabs").tabs({
                select: function (e, i) {
                    currentTab = i.index;
                }
            });
        });
        $("#btnNext").live("click", function () {
            var tabs = $('#tabs').tabs();
            var c = $('#tabs').tabs("length");
            currentTab = currentTab == (c - 1) ? currentTab : (currentTab + 1);
            tabs.tabs('select', currentTab);
            $("#btnPrevious").show();
            if (currentTab == (c - 1)) {
                $("#btnNext").hide();
            } else {
                $("#btnNext").show();
            }
        });
        $("#btnPrevious").live("click", function () {
            var tabs = $('#tabs').tabs();
            var c = $('#tabs').tabs("length");
            currentTab = currentTab == 0 ? currentTab : (currentTab - 1);
            tabs.tabs('select', currentTab);
            if (currentTab == 0) {
                $("#btnNext").show();
                $("#btnPrevious").hide();
            }
            if (currentTab < (c - 1)) {
                $("#btnNext").show();
            }
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

        .modalBackground {
            background-color: rgba(195,195,195,0.5);
            z-index: 10000;
        }
    </style>

    

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div class="col-md-12" style="margin-top: 8px; margin-bottom: 8px">
                <asp:ScriptManager runat="server" ID="Scriptmanager2">

                </asp:ScriptManager>

    <script src="script/Extension.min.js" type="text/javascript"></script>




                <div align="center">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                        <ContentTemplate>
                            <asp:Label ID="lblMsg" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #CC3300;"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div align="center">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                        <ContentTemplate>
                            <asp:Label ID="lblSuc" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #000;"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>


                <div class="panel panel-inverse" style="height: 1400px";>
                    <div class="panel-heading">



                        <table width="100%">
                            <tr>
                                <td>
                                    <h3 class="panel-title">Add Employee</h3>
                                </td>
                                <td align="right"><< <a href="Employees.aspx" style="color: #003366">Back</a>  </td>
                            </tr>
                        </table>


                    </div>

                    <div id="dialog" style="display: none">

                        <table cellpadding="5" cellspacing="5">
                            <tr>
                                <td>Emp ID
                                </td>
                                <td>

                                    <asp:TextBox ID="txtoldid" runat="server"
                                        class="sinput"></asp:TextBox>

                                    <cc1:AutoCompleteExtender ID="EmpIdtoAutoCompleteExtender" runat="server"
                                        ServiceMethod="GetEmpID"
                                        ServicePath="AutoCompleteAA.asmx"
                                        MinimumPrefixLength="4"
                                        CompletionInterval="100"
                                        EnableCaching="true"
                                        TargetControlID="txtoldid"
                                        FirstRowSelected="false"
                                        CompletionListCssClass="completionList"
                                        CompletionListItemCssClass="listItem"
                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Button ID="BtnOldEmpidDetails" runat="server" Text="Search" />

                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="panel-body">
                         <table width="20%" align="right">
                        <tr>
                            <td>
                                <input type="button" id="btnPrevious" value="Previous" style="display: none" /></td>
                            <td>
                                <input type="button" id="btnNext" value="Next" /></td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="Save"
                                    OnClick="Btn_Save_Personal_Tab_Click" ValidationGroup="a" /></td>
                            <td>
                                <asp:Button ID="Button2" runat="server" Text="Cancel" OnClientClick='return confirm("Are you sure you want to Cancel this entry?");'
                                    OnClick="Btn_Cancel_Personal_Tab_Click" /></td>


                        </tr>
                    </table>
                        <div style="text-align: right">
                            <asp:Label ID="txtmodifyempid" runat="server"></asp:Label>
                        </div>
                        <div id="tabs" style="height: 1220px;margin-top:20px";>
                            <ul>
                                <li><a href="#tabs-1">Personal Information</a></li>
                                <li><a href="#tabs-2">References</a></li>
                                <li><a href="#tabs-3">Bank/PF/ESI</a></li>
                                <%-- <li><a href="#tabs-4">Images</a></li>--%>
                                <li><a href="#tabs-4">Proofs</a></li>
                                <li><a href="#tabs-5">Qualification/Previous Experience</a></li>
                                <%--<li><a href="#tabs-5">Images</a></li>--%>
                                <li><a href="#tabs-6">Police Record</a></li>
                                <%--  <li><a href="#tabs-7">Salary</a></li>--%>
                            </ul>
                            <div id="tabs-1">
                                <asp:UpdatePanel runat="server" ID="uppersonal">
                                    <ContentTemplate>
                                        <div class="dashboard_firsthalf">
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbGeneral" TabIndex="1" runat="server" GroupName="E1" Text=" General Enrollment" AutoPostBack="True" OnCheckedChanged="rdbGeneral_CheckedChanged"  Checked="True" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbStaff" TabIndex="2" runat="server" GroupName="E1" Visible="true" AutoPostBack="True" OnCheckedChanged="rdbGeneral_CheckedChanged"   Text=" Staff" />
                                                        <asp:RadioButton ID="rdbmanual" runat="server" GroupName="E1" Text=" Manual" Style="padding-left: 3px" AutoPostBack="True" OnCheckedChanged="rdbmanual_CheckedChanged" />
                                                        <asp:RadioButton ID="rdbRejoin" runat="server" GroupName="E1" Text=" Rejoin"  Style="padding-left: 3px" Visible="false" />
                                                        <asp:RadioButton ID="RadioButton1" runat="server" GroupName="E1" Text=" RDB1" Style="padding-left: 3px" Visible="false" />

                                                    </td>


                                                    <td>
                                                        <cc1:ModalPopupExtender ID="modelRejoin" runat="server" TargetControlID="rdbRejoin" PopupControlID="pnlRadioButton1"
                                                            BackgroundCssClass="modalBackground" BehaviorID="modelExRejoin">
                                                        </cc1:ModalPopupExtender>

                                                        <asp:Panel ID="pnlRadioButton1" runat="server" DefaultButton="btnSubmit" Height="200px" Width="400px" Style="display: none; position: absolute; background-color: white; border-radius: 10px; box-shadow: 0 0 15px #333333;">
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>

                                                                        <tr style="margin-top: 10px">
                                                                            <td style="font: bold; font-size: medium; padding-left: 12px">&nbsp;&nbsp;&nbsp;
                                                                                Empid
                                                                            </td>
                                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                        <asp:DropDownList ID="ddloldempdrop" runat="server" class="sdrop" Width="100px" Style="margin-left: 10px"></asp:DropDownList>
                                                                            </td>
                                                                        </tr>

                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <br />
                                                            <table style="background-position: center;">
                                                                <tr>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnSubmit" runat="server" Text="Ok" Style="float: right; margin-left: 190px" CssClass="btn Save" OnClick="BtnOldEmpidDetails_Click" />
                                                                    </td>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnClose" runat="server" Text="Close" Style="float: right; margin-left: -13px" class="btn Save" Visible="false" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Emp ID
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmID" TabIndex="1" runat="server" ReadOnly="True"
                                                            class="sinput"></asp:TextBox>

                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>First Name  <span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpFName" TabIndex="2" runat="server" class="sinput" ></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr runat="server" visible="false">
                                                    <td>Last Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmplname" TabIndex="4" runat="server" class="sinput" MaxLength="25"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Gender  <span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbmale" TabIndex="6" runat="server" GroupName="g1" Text="Male" Checked="True" />
                                                        <asp:RadioButton ID="rdbfemale" TabIndex="7" runat="server" GroupName="g1" Text="Female" />
                                                        <asp:RadioButton ID="rdbTransgender" runat="server" GroupName="g1" Text="Transgender" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbactive" TabIndex="12" runat="server" GroupName="g2" Text="Active" Checked="true" />
                                                        &nbsp;
                                                    <asp:RadioButton ID="rdbResigned" TabIndex="13" runat="server" GroupName="g2" Text="Resigned" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Qualification
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" TabIndex="14" ID="txtQualification" MaxLength="15" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Date of Interview
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpDtofInterview" TabIndex="16" runat="server" class="sinput"
                                                            MaxLength="10"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEDtofInterview" runat="server" Enabled="true" TargetControlID="txtEmpDtofInterview"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtEmpDtofInterview"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>Phone No. 
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPhone" TabIndex="18" MaxLength="12" runat="server" class="sinput">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtPhone" FilterMode="ValidChars" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Mother Tongue
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtmtongue" TabIndex="20" runat="server" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Nationality
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtnationality" TabIndex="22" runat="server" class="sinput" MaxLength="50" Text="INDIAN"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Father Name 
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFatherName" runat="server" MaxLength="50" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Father Occupation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtfatheroccupation" runat="server" MaxLength="50" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Spouse Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSpousName" runat="server" MaxLength="50" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                                        <td>
                                                            Old Employee ID
                                                        </td>
                                                        <td>
                                                           
                                                            <asp:TextBox ID="txtoldemployeeid"  runat="server" MaxLength="50" ReadOnly="true" class="sinput"></asp:TextBox>
                                                       
                                                        </td>
                                                    </tr>--%>
                                                <tr>
                                                    <td>Branch <span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBranch" runat="server" TabIndex="30"
                                                            class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Department
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddldepartment" runat="server" TabIndex="32"
                                                            class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Site Posted to <span style="color: red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DdlPreferedUnit" TabIndex="34" runat="server"
                                                            class="sdrop">
                                                        </asp:DropDownList>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>PSARA Emp Code
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtpsaraempcode" runat="server" CssClass="sinput" TabIndex="36"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>ID card issued date
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtIDCardIssuedDt" runat="server" CssClass="sinput" TabIndex="38" OnTextChanged="TxtIDCardIssuedDt_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true" TargetControlID="TxtIDCardIssuedDt"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FtBIDCardIssuedDt" runat="server" Enabled="True" TargetControlID="TxtIDCardIssuedDt"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Client Employee Id
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txoldempid" TabIndex="39" MaxLength="100" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Emergency Contact number
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtemercontactno" TabIndex="39" MaxLength="100" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Reliever
                                                    </td>
                                                    <td>
                                                       <asp:CheckBox ID="chkreliever" runat="server" />
                                                    </td>
                                                </tr>

                                                <tr runat="server" visible="false">
                                                    <td>Registration Fee
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtregistrationfee" TabIndex="39" Text="500" Enabled="false" MaxLength="100" runat="server" class="sinput"></asp:TextBox>
                                                          <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" Enabled="True" TargetControlID="txtregistrationfee"
                                                            ValidChars=".0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>

                                                <tr style="visibility: hidden">
                                                    <td>Community/Classification
                                                    </td>
                                                    <td style="padding-top: 10px">
                                                        <asp:RadioButton ID="rdsc" runat="server" GroupName="m1" Text="SC" />
                                                        <asp:RadioButton ID="rdst" runat="server" GroupName="m1" Text="ST" />
                                                        <asp:RadioButton ID="rdobc" runat="server" GroupName="m1" Text="OBC" />
                                                        <asp:RadioButton ID="rdur" runat="server" GroupName="m1" Text="Others"
                                                            Checked="true" />
                                                    </td>
                                                </tr>

                                                

                                            </table>


                                        </div>

                                        <div class="dashboard_secondhalf">
                                            <table cellpadding="5" cellspacing="5">
                                                <tr style="display: none">
                                                    <td>Old Emp ID
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txoldempid1" TabIndex="2" MaxLength="100" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>Title </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTitle" runat="server" class="sdrop" TabIndex="1" OnSelectedIndexChanged="ddlTitle_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem>--Select--</asp:ListItem>
                                                            <asp:ListItem>Mr</asp:ListItem>
                                                            <asp:ListItem>Miss</asp:ListItem>
                                                            <asp:ListItem>Mrs</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                 <tr runat="server" visible="false">
                                                    <td>Middle Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpmiName" TabIndex="3" MaxLength="40" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Date of Birth <span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpDtofBirth" TabIndex="5" runat="server" class="sinput" MaxLength="10"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEEmpDtofBirth" runat="server" Enabled="true" TargetControlID="txtEmpDtofBirth"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEDOB" runat="server" Enabled="True" TargetControlID="txtEmpDtofBirth"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Marital Status
                                                    </td>
                                                    <td>

                                                        <asp:RadioButton ID="rdbsingle" TabIndex="8" runat="server" GroupName="m1" Text="Single" />
                                                        <asp:RadioButton ID="rdbmarried" TabIndex="9" runat="server" GroupName="m1" Text="Married" Style="margin-left: 17px" Checked="true" />

                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbdivorcee" runat="server" GroupName="m1" Text="Divorcee" TabIndex="10" Style="margin-top: 10px" />
                                                        <asp:RadioButton ID="rdbWidower" runat="server" GroupName="m1" Text="Widower" TabIndex="11" Style="margin-top: 10px" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Designation  <span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" TabIndex="15" class="sdrop" ID="ddlDesignation">
                                                        </asp:DropDownList>


                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Date of Joining  <span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpDtofJoining" TabIndex="17" runat="server"   class="sinput" size="20" 
                                                            MaxLength="10"></asp:TextBox>
 <cc1:CalendarExtender ID="CalendarExtender7" runat="server" Enabled="true" TargetControlID="txtEmpDtofJoining"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" Enabled="True" TargetControlID="txtEmpDtofJoining"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                        
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Date of Leaving
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDofleaving" TabIndex="19" runat="server" class="sinput" MaxLength="10" Enabled="false"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEDofleaving" runat="server" Enabled="true" TargetControlID="txtDofleaving"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                            TargetControlID="txtDofleaving" ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Languages Known
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" TabIndex="21" ID="txtLangKnown" class="sinput" MaxLength="80">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Religion
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtreligion" TabIndex="23" runat="server" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Previous Employer
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPreEmp" TabIndex="29" runat="server" TextMode="MultiLine" Style="height: 50px" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Mother Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMotherName" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Division
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDivision" runat="server" TabIndex="31"
                                                            class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Reporting Manager
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlReportingMgr" runat="server" TabIndex="33"
                                                            class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Net Pay
                                                            
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrossSalary" runat="server" class="sinput" TabIndex="35"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True" TargetControlID="txtGrossSalary"
                                                            ValidChars="0123456789">
                                                        </cc1:FilteredTextBoxExtender>

                                                        <asp:ImageButton ID="btncalculate" runat="server" ImageUrl="~/assets/calculator.png" class="btn save" OnClick="btncalculate_Click" ToolTip="Calculate" Style="margin-left: 2px" />


                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Email
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtemail" runat="server" class="sinput" TabIndex="37"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>ID card valid till
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtIdCardValid" runat="server" CssClass="sinput" TabIndex="39"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true" TargetControlID="TxtIdCardValid"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBIdCardValid" runat="server" Enabled="True" TargetControlID="TxtIdCardValid"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Rejoin Empid
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRejoinEmpid" runat="server" CssClass="sinput" TabIndex="36" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                 <tr>
                                                    <td>Employee Type </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlemptype" runat="server" class="sdrop" TabIndex="1" >
                                                            <asp:ListItem>General</asp:ListItem>
                                                            <asp:ListItem>Staff</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </td>
                                                </tr>

                                            </table>
                                        </div>
                                        <asp:Panel ID="PnlPFDetails" runat="server" GroupingText="<strong>&nbsp;PF Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>PF Deduct 
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" Checked="true" ID="ChkPFDed" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>EPF No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="17" ID="txtEmpPFNumber" class="sinput" MaxLength="15" Style="margin-left: 68px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>PF Enroll Date
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="18" class="sinput" ID="txtPFEnrollDate" size="20" Style="margin-left: 68px"
                                                                MaxLength="10"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CEPFEnrollDate" runat="server" Enabled="true" TargetControlID="txtPFEnrollDate"
                                                                Format="dd/MM/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                TargetControlID="txtPFEnrollDate" ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr style="visibility: hidden">
                                                        <td>PF Nominee
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="19" ID="txtPFNominee" class="sinput" MaxLength="80" Style="margin-left: 68px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td>PT Deduct
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="ChkPTDed" Checked="true" />
                                                        </td>
                                                    </tr>
                                                     <tr runat="server" visible="false">
                                                        <td>PT State
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlPTState" class="sdrop">
                                                            </asp:DropDownList>
                                                        </td>

                                                    </tr>

                                                      <tr runat="server" visible="false">
                                                        <td>LWF State
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlLWFState" class="sdrop">
                                                            </asp:DropDownList>
                                                        </td>

                                                    </tr>

                                                    <tr>
                                                        <td>UAN No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSSNumber" TabIndex="16" MaxLength="12" runat="server" class="sinput"></asp:TextBox>
                                                             <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtSSNumber" ValidChars="0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:TextBox ID="txtprvSSNumber" Visible="false" TabIndex="16" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr style="visibility: hidden">
                                                        <td>PF Nominee Relation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtPFNomineeRel" TabIndex="20" class="sinput" Style="margin-left: 2px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="PnlESIDetails" runat="server" GroupingText="<strong>&nbsp;ESI Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>

                                                        <td>ESI Deduct <%-- <span style="color: Red">*</span>--%> </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="ChkESIDed" Text="" Checked="true" /><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>ESI No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="21" ID="txtESINum" class="sinput" MaxLength="10" Style="margin-left: 63px"></asp:TextBox>
                                                               <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" Enabled="True"
                                                                TargetControlID="txtESINum" ValidChars="0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td>ESI Nominee
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="23" ID="txtESINominee" class="sinput" MaxLength="80" Style="margin-left: 63px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>ESI Disp Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="22" ID="txtESIDiSName" class="sinput" Style="margin-left: 2px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td>ESI Nominee Relation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="24" ID="txtESINomRel" class="sinput" Style="margin-left: 2px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="PnlProofsSubmitted" runat="server" GroupingText="<strong>&nbsp;Proofs Submitted&nbsp;</strong>" Style="margin-top: 10px">
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="ChkAadharCard" runat="server" Text="  Aadhar Card" TabIndex="1" OnCheckedChanged="ChkAadharCard_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                            <span style="color: Red">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtAadharCard" runat="server" MaxLength="12"  class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="2"></asp:TextBox>
                                                         <cc1:FilteredTextBoxExtender ID="FTBtxtAadharCard" runat="server" Enabled="True"
                                                                TargetControlID="txtAadharCard" ValidChars="0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtAadharName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" visible="false">
                                                        <td style="padding-left: 18px">Image
                                                        </td>
                                                        <td>
                                                            <asp:HyperLink ID="AadharImga" runat="server">
                                                                <asp:Image ID="AadharImg" runat="server" Height="85" Width="85" />
                                                            </asp:HyperLink>
                                                        </td>
                                                    </tr>
                                                   <tr runat="server" visible="false">
                                                        <td>Modify Image</td>
                                                        <td>
                                                            <asp:FileUpload ID="FileUploadAadharImage" runat="server" /></td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="ChkPanCard" runat="server" Text=" Pan Card" TabIndex="3" OnCheckedChanged="ChkPanCard_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                        
                                                        </td>


                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPanCard" runat="server" class="sinput" Enabled="false" TabIndex="4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPanCardName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="PnlBankDetails" runat="server" GroupingText="<strong>&nbsp;Bank Details&nbsp;</strong>">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Bank Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlbankname" runat="server" TabIndex="1" class="sdrop" MaxLength="100">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Bank A/C No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankAccNum" TabIndex="2" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td>IFSC Code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtIFSCcode" runat="server" MaxLength="20" TabIndex="4" class="sinput"> </asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>
                                            </div>

                                        </asp:Panel>

                                        <asp:Panel ID="pnlSecondarybank" Visible="false" runat="server" GroupingText="<strong>&nbsp;Secondary Bank Details&nbsp;</strong>">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Bank Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlsecondarybankname" runat="server" TabIndex="1" class="sdrop" MaxLength="100">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>Second Bank A/C No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtsecondBankAccNum" TabIndex="2" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td>IFSC Code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtsecondIFSCcode" runat="server" MaxLength="20" TabIndex="4" class="sinput"> </asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>

                                        </asp:Panel>

                                        <asp:Panel ID="pnlimages" runat="server" GroupingText="<strong>&nbsp;Images&nbsp;</strong>">

                                            <div class="dashboard_firsthalf">
                                                <table cellpadding="5" cellspacing="5" style="margin-top: 10px">
                                                    <tr>
                                                        <td>Employee Photo</td>

                                                        <td>
                                                            <asp:FileUpload ID="FileUploadImage" runat="server"/></td>
                                                        <%--<cc1:AsyncFileUpload OnClientUploadError="uploadError"
                                                OnClientUploadComplete="uploadComplete" runat="server"
                                                ID="FileUploadImage" Width="400px" UploaderStyle="Modern"
                                                CompleteBackColor = "White"
                                                UploadingBackColor="#CCFFFF"  
                                                 />
                                                </td>--%>
                                                    </tr>

                                                </table>
                                            </div>

                                            <div class="dashboard_Secondhalf">
                                                <table cellpadding="5" cellspacing="5" style="margin-top: 10px;">
                                                    <tr>

                                                        <td>Emp Sign</td>
                                                        <td>
                                                            <asp:FileUpload ID="FileUploadSign" runat="server"/></td>
                                                    </tr>

                                                </table>
                                            </div>

                                        </asp:Panel>
                                        <br />
                                        <%-- OnClientClick='return confirm("Are you sure you want to create an employee?");'--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>


                            </div>
                            <div id="tabs-2">
                                <asp:UpdatePanel runat="server" ID="Updatepanel3">
                                    <ContentTemplate>
                                        <asp:Panel ID="PnlEmployeeInfo" runat="server" GroupingText="<strong>&nbsp;Employee Info&nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">

                                                <table cellpadding="5" cellspacing="5">

                                                    <%-- <tr style="visibility:hidden">
                                                            <td>Birth Village
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBirthVillage" runat="server" class="sinput" TabIndex="1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="visibility:hidden">
                                                            <td>Birth State
                                                            </td>
                                                            <td>
                                                                <%--<asp:TextBox ID="txtBirthState" runat="server" class="sinput" TabIndex="3"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlbirthstate" runat="server" class="sdrop" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="ddlbirthstate_SelectedIndexChanged"></asp:DropDownList>

                                                            </td>
                                                        </tr>--%>
                                                    <tr>
                                                        <td>Ref Name &amp; Address1
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtREfAddr1" runat="server" TabIndex="5" class="sinput" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Blood Group
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlBloodGroup" runat="server" TabIndex="7" class="sdrop">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Physical Remarks
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhyRem" runat="server" TabIndex="9" class="sinput" MaxLength="55"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Identification Marks1
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtImark1" runat="server" TabIndex="10" class="sinput" MaxLength="80"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                    <tr>

                                                        <td>Specially Abled</td>
                                                        <td>
                                                            <asp:CheckBox ID="ChkSpeciallyAbled" runat="server" Text=" Specially Abled" TabIndex="11" AutoPostBack="True" OnCheckedChanged="ChkSpeciallyAbled_CheckedChanged" />
                                                        </td>
                                                    </tr>

                                                    <tr style="display: none">
                                                        <td>Family Details
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFamDetails" runat="server" TextMode="MultiLine"
                                                                class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>

                                            </div>



                                            <div class="dashboard_secondhalf" style="padding-top: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <%-- <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>--%>
                                                    <%-- <tr style="visibility:hidden">
                                                            <td>Birth Country
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBirthCountry" runat="server" class="sinput" Style="margin-left: 5px" TabIndex="2" Text="INDIA"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="visibility:hidden">
                                                            <td>Birth District
                                                            </td>
                                                            <td>
                                                                <%--<asp:TextBox ID="txtBirthDistrict" runat="server" class="sinput" Style="margin-left: 5px" TabIndex="4"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlBirthDistrict" runat="server" CssClass="sdrop" Style="margin-left: 5px" TabIndex="4" Enabled="false"></asp:DropDownList>
                                                            </td>
                                                        </tr>--%>

                                                    <tr>
                                                        <td>Ref Name &amp; Address2
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtREfAddr2" runat="server" TabIndex="6" TextMode="MultiLine" class="sinput" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Remarks
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpRemarks" runat="server" TabIndex="8" TextMode="MultiLine"
                                                                class="sinput" MaxLength="50" Height="50px" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Identification Marks2
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtImark2" runat="server" TabIndex="10" class="sinput" MaxLength="80" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>

                                                        <td>Applicant Category</td>
                                                        <td>
                                                            <%--<asp:TextBox ID="TxtAppCategory" runat="server" class="sinput" ></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlAppCategory" runat="server" Style="margin-left: 5px" TabIndex="12" CssClass="sdrop" Enabled="false">
                                                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="LOCOMOTIVE" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="VISUAL" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="HEARING" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="OTHERS" Value="4"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label runat="server" ID="lblrefresult" Style="color: Red"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlphysicalTesting" runat="server" GroupingText="<strong>&nbsp;Physical Standards &nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Height
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtheight" runat="server" TabIndex="13" class="sinput" MaxLength="80" Style="margin-left: 70px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Weight
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtweight" runat="server" TabIndex="15" class="sinput" MaxLength="80" Style="margin-left: 70px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Hair Colour
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txthaircolour" runat="server" class="sinput" MaxLength="80" TabIndex="17" Style="margin-left: 70px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>
                                            </div>

                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">


                                                    <tr>
                                                        <td>Chest UnExpand
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcheunexpan" runat="server" TabIndex="14" class="sinput" MaxLength="50" Style="margin-left: 48px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Chest Expand
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcheexpan" runat="server" TabIndex="16" class="sinput" MaxLength="25" Style="margin-left: 48px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Eye Colour
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEyeColour" runat="server" class="sinput" MaxLength="25" Style="margin-left: 48px" TabIndex="18"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>

                                        </asp:Panel>

                                        <asp:Panel ID="PnlAddressDetails" runat="server" GroupingText="<strong>&nbsp;Address Details&nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td class="style4">
                                                            <strong>Present Address :</strong>
                                                        </td>
                                                        <td>

                                                            <asp:CheckBox ID="chkSame" runat="server" Text=" Copy" AutoPostBack="true" OnCheckedChanged="chkSame_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <%-- <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:TextBox ID="txtPresentAddress" runat="server" TabIndex="19" class="sinput" Height="55px"  TextMode="MultiLine" Style="margin-left: 12px"></asp:TextBox>
                                                    </td>
                                                </tr>--%>

                                                    <tr>
                                                        <td>Land Mark</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprLandmark" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Village/Town</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprvillage" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Post Office</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprPostOffice" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Taluka/Hobli</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprtaluka" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>Police Station</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprPoliceStation" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>State
                                                        </td>
                                                        <td>

                                                            <%--<asp:TextBox ID="txtstate" runat="server" TabIndex="18" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlpreStates" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="21" AutoPostBack="true" OnSelectedIndexChanged="ddlpreStates_SelectedIndexChanged1"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>District
                                                        </td>
                                                        <td>
                                                            <%--<asp:TextBox ID="txtcity" runat="server" TabIndex="15" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlpreCity" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="23" Enabled="false"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Pin code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtprpin" runat="server" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Date Since Residing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtprResidingDate" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="true" TargetControlID="txtprResidingDate"
                                                                Format="dd/MM/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                TargetControlID="txtprResidingDate" ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Period of stay
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtprPeriodofStay" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>

                                                        </td>
                                                    </tr>

                                                    <%--<td>
                                                                <asp:TextBox ID="txtprntaddress" runat="server" TabIndex="4" Width="160px"></asp:TextBox>
                                                            </td>

                                              <tr>
                                                    <td>
                                                        Door No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPrdoor" runat="server" TabIndex="12" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Street
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtstreet" runat="server" TabIndex="13" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Land mark
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtlmark" runat="server" TabIndex="14" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Area
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtarea" runat="server" TabIndex="14" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                
                                               <tr>
                                                    <td>
                                                        District
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtdistrictt" runat="server" TabIndex="16" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>


                                                    <tr>
                                                        <td>Phone(if any)
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtmobile" runat="server" TabIndex="25" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td class="style4">
                                                            <strong>Permanent Address :</strong>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:TextBox ID="txtPermanentAddress" runat="server" TabIndex="20" class="sinput" Height="55px" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>--%>
                                                    <tr>
                                                        <td>Land Mark</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpeLandmark" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Village/Town</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpevillage" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Post Office</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpePostOffice" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Taluka/Hobli</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpeTaluka" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Police Station</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpePoliceStattion" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>State
                                                        </td>
                                                        <td>
                                                            <%--<asp:TextBox ID="txtstate8" runat="server" TabIndex="28" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="DdlStates" runat="server" class="sdrop" TabIndex="22" AutoPostBack="true" OnSelectedIndexChanged="DdlStates_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>District
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlcity" runat="server" class="sdrop" TabIndex="24" Enabled="false"></asp:DropDownList>
                                                            <%-- <asp:TextBox ID="txtcity5" runat="server" TabIndex="25" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Pin code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtpePin" runat="server" TabIndex="27" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Date Since Residing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtResidingDate" runat="server" class="sinput"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="true" TargetControlID="txtResidingDate"
                                                                Format="dd/MM/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                TargetControlID="txtResidingDate" ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Period of stay
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPeriodofStay" runat="server" class="sinput"></asp:TextBox>

                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                    <td>
                                                        Door No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtdoor1" runat="server" TabIndex="21" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Street
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtstreet2" runat="server" TabIndex="22" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Land mark
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtlmark3" runat="server" TabIndex="23" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Area
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtarea4" runat="server" TabIndex="24" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>



                                                    <%--<tr>
                                                    <td>
                                                        Perm. District
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPDist" runat="server" TabIndex="26" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>


                                                    <tr>
                                                        <td>Phone(if any)
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtmobile9" runat="server" TabIndex="26" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>


                                            </div>
                                        </asp:Panel>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="tabs-3">
                                <asp:Panel ID="Panel1" runat="server" GroupingText="<strong>&nbsp;Bank Details&nbsp;</strong>">

                                    <div class="dashboard_firsthalf" style="padding: 10px">
                                        <table cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>Branch Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtbranchname" runat="server" MaxLength="80" TabIndex="3" class="sinput"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Branch Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBranchCode" runat="server" TabIndex="5" class="sinput" MaxLength="50"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        TargetControlID="txtBranchCode" FilterMode="ValidChars" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Bank App No.
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBankAppNum" runat="server" TabIndex="7" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Insurance Nominee
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmpInsNominee" runat="server" TabIndex="9" class="sinput" MaxLength="100"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>Nominee Date of Birth
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNomDoB" runat="server" TabIndex="11" class="sinput"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CENomDoB" runat="server" Enabled="true" TargetControlID="txtNomDoB"
                                                        Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                        TargetControlID="txtNomDoB" ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Insurance Cover
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInsCover" TabIndex="13" runat="server" class="sinput" MaxLength="10"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEInsCover" runat="server" Enabled="True" TargetControlID="txtInsCover"
                                                        FilterMode="ValidChars" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                            <tr style="visibility: hidden">
                                                <td>Aadhaar No
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" TabIndex="15" ID="txtaadhaar" class="sinput"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr style="display: none">
                                                <td>Cmp Short Name
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtCmpShortName" class="sinput" MaxLength="50">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="dashboard_secondhalf" style="padding: 10px">
                                        <table cellpadding="5" cellspacing="5">

                                            <tr>
                                                <td>Bank Code No.
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBankCodenum" TabIndex="6" runat="server" class="sinput"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Region Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRegCode" TabIndex="8" runat="server" class="sinput"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                        TargetControlID="txtRegCode" FilterMode="ValidChars" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Bank Card Reference
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBankCardRef" TabIndex="10" runat="server" class="sinput"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Nominee Relation
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmpNomRel" TabIndex="12" runat="server" class="sinput"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Ins Debt Amount
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInsDeb" TabIndex="14" runat="server" class="sinput"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                        TargetControlID="txtInsDeb" FilterMode="ValidChars" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                        </table>
                                    </div>
                                    <%--  <div style="float: right; margin-top: 300px; margin-left: 250px">
                                                    <asp:Button ID="btn_BankSave" runat="server" Text="Save" class="btn save" OnClick="btn_BankSave_Click"
                                                        OnClientClick='return confirm("Are you sure you want to Add Details?");' />
                                                    <asp:Button ID="btn_BankCancel" runat="server" Text="Cancel" class="btn save" OnClick="btn_BankCancel_Click"
                                                        OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                                    <div style="text-align: center float:right">
                                                        <asp:Label runat="server" ID="lblBankRes" Visible="false" Style="color: Red"></asp:Label>
                                                    </div>
                                                </div>
                                    --%>
                                </asp:Panel>

                                <asp:Panel ID="PnlSalaryDetails" runat="server" GroupingText="<strong>&nbsp;Salary Details&nbsp;</strong>" Style="margin-top: 10px">
                                    <div class="dashboard_firsthalf" style="padding: 10px">
                                        <table>
                                            <tr>
                                                <td style="height: 20px">Additional Amount
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtaddlamt" runat="server" TabIndex="25" class="sinput" MaxLength="50" Style="margin-left: 35px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="dashboard_secondhalf" style="padding: 10px">
                                        <table>
                                            <tr>
                                                <td style="height: 20px">Food Allowance
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtfoodallowance" runat="server" TabIndex="26" class="sinput" MaxLength="50" Style="margin-left: 50px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                </asp:Panel>



                            </div>

                            <div id="tabs-4">
                                <asp:Panel ID="Panel2" runat="server" GroupingText="<strong>&nbsp;Proofs Submitted&nbsp;</strong>" Style="margin-top: 10px">

                                    <div class="dashboard_firsthalf" style="padding: 10px">
                                        <table cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="ChkdrivingLicense" runat="server" Text=" Driving License" TabIndex="5" OnCheckedChanged="ChkdrivingLicense_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtDrivingLicense" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="6"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">KYC Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtDrivingLicenseName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="padding-left: 18px">Expiry date</td>
                                                <td>
                                                    <asp:TextBox ID="txtDrivingLicenseExpiry" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                </td>
                                                <cc1:CalendarExtender ID="CalendarExtender6" runat="server" Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtDrivingLicenseExpiry">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True" TargetControlID="txtDrivingLicenseExpiry" ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </tr>

                                            <tr>

                                                <td>
                                                    <asp:CheckBox ID="ChkVoterID" runat="server" Text=" Voter ID" TabIndex="9" OnCheckedChanged="ChkVoterID_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtVoterID" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="10"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">KYC Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtVoterName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="ChkRationCard" runat="server" Text=" Ration Card" TabIndex="13" OnCheckedChanged="ChkRationCard_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtRationCard" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="14"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">KYC Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtRationCardName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>
                                                    <asp:CheckBox ID="Chkother" runat="server" Text=" if Others, Specify" TabIndex="15" OnCheckedChanged="Chkother_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                </td>

                                            </tr>
                                            <tr>

                                                <td style="padding-left: 18px">Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtOther" runat="server" class="sinput" Enabled="false" TabIndex="16" Style="margin-left: 5px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">KYC Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtOtherName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="dashboard_secondhalf" style="padding: 10px">
                                        <table cellpadding="5" cellspacing="5">


                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkGunLicense" runat="server" Text=" Gun License" TabIndex="7" OnCheckedChanged="chkGunLicense_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtGunLicense" runat="server" class="sinput" Enabled="false" TabIndex="8"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">KYC Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtGunLicensename" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="padding-left: 18px">Expiry date</td>
                                                <td>
                                                    <asp:TextBox ID="txtGunLicenseExpirydate" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                </td>
                                                <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtGunLicenseExpirydate">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True" TargetControlID="txtGunLicenseExpirydate" ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                            </tr>


                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="ChkBankPassbook" runat="server" Text=" Bank PassBook" TabIndex="7" OnCheckedChanged="ChkBankPassbook_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtBankPassbook" runat="server" class="sinput" Enabled="false" TabIndex="8"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">KYC Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtBankPassBookName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="ChkElectricityBill" runat="server" Text=" Electricity Bill" TabIndex="11" OnCheckedChanged="ChkElectricityBill_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtElectricityBill" runat="server" class="sinput" Enabled="false" TabIndex="12"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">KYC Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtElecBillname" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="ChkESICCard" runat="server" Text=" ESIC Card" TabIndex="15" AutoPostBack="true" OnCheckedChanged="ChkESICCard_CheckedChanged" Style="font-weight: bold" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">Number</td>
                                                <td>
                                                    <asp:TextBox ID="txtESICCardNo" runat="server" class="sinput" Enabled="false" TabIndex="16"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 18px">KYC Name</td>
                                                <td>
                                                    <asp:TextBox ID="txtESICName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                                <asp:UpdatePanel runat="server" ID="Upproofs" UpdateMode="Always">
                                    <ContentTemplate>



                                        <asp:Panel ID="PnlExService" runat="server" GroupingText="<strong>&nbsp;Ex-Service&nbsp;</strong>" Style="margin-top: 15px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td>EMP Ex-service
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="ChkExService" Text="" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 20px">Service No.
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtServiceNum" runat="server" TabIndex="17" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Date of Enrollment
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtDOfEnroll" runat="server" TabIndex="19" class="sinput" size="20"
                                                                MaxLength="10"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CEDOfEnroll" runat="server" Enabled="true" TargetControlID="txtDOfEnroll"
                                                                Format="dd/MM/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FTBEDOfEnroll" runat="server" Enabled="True" TargetControlID="txtDOfEnroll"
                                                                ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Crops
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtCrops" runat="server" TabIndex="21" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Medical Category
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtMCategory" runat="server" TabIndex="23" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Conduct
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtConduct" runat="server" TabIndex="25" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Rank
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtRank" runat="server" TabIndex="18" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Date of Discharge
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtDofDischarge" runat="server" TabIndex="20" class="sinput" size="20"
                                                                MaxLength="10"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CEDofDischarge" runat="server" Enabled="true" TargetControlID="txtDofDischarge"
                                                                Format="dd/MM/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FTBEDofDischarge" runat="server" Enabled="True"
                                                                TargetControlID="txtDofDischarge" ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Trade
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtTrade" runat="server" TabIndex="22" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Reason of Discharge
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="TxtROfDischarge" runat="server" TabIndex="24" TextMode="MultiLine" MaxLength="50"
                                                                class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label runat="server" ID="lblExRes" Visible="false" Style="color: Red"></asp:Label>
                                                        </td>
                                                    </tr>

                                                </table>

                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlfamilydetails" runat="server" GroupingText="<strong>&nbsp;Family Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div style="padding: 10px">
                                                <asp:GridView ID="gvFamilyDetails" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderStyle="Solid" CellPadding="5" ForeColor="#333333" Height="180px" PageSize="25" Visible="true"
                                                    ShowHeader="true" Style="margin: 0px auto" Width="100%" CellSpacing="5">
                                                    <HeaderStyle Wrap="True" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="S.No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF3FB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Name" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEmpName" runat="server" Text=""></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Date Of Birth" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRelDtofBirth" runat="server" Width="98%"
                                                                    MaxLength="10" placeholder="DD/MM/YYYY"></asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Age" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderStyle-Font-Size="Small" ItemStyle-Font-Size="Small" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAge" runat="server" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Relationship" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlRelation" runat="server" Width="99%">
                                                                    <asp:ListItem runat="server" Value="--Select--" />
                                                                    <asp:ListItem runat="server" Value="Father" />
                                                                    <asp:ListItem runat="server" Value="Wife" />
                                                                    <asp:ListItem runat="server" Value="Husband" />
                                                                    <asp:ListItem runat="server" Value="Son" />
                                                                    <asp:ListItem runat="server" Value="Daughter" />
                                                                    <asp:ListItem runat="server" Value="Brother" />
                                                                    <asp:ListItem runat="server" Value="Sister" />
                                                                    <asp:ListItem runat="server" Value="Mother" />
                                                                    <asp:ListItem runat="server" Value="Uncle" />
                                                                    <asp:ListItem runat="server" Value="Aunty" />
                                                                    <asp:ListItem runat="server" Value="Other" />
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Occupation" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtReloccupation" runat="server" Text="" Width="98%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="PF Nominee" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkPFNominee" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="ESI Nominee" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkESINominee" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Whether residing with him/her ?" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlresidence" runat="server" Width="80%">
                                                                    <asp:ListItem runat="server" Value="--Select--" />
                                                                    <asp:ListItem runat="server" Value="Yes" />
                                                                    <asp:ListItem runat="server" Value="No" />
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="If 'No' Place of Residence" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtplace" runat="server" Text="" Width="98%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <asp:Button ID="btnFamilyDetailsAdd" runat="server" Text="Add" Style="margin-left: 10px; margin-right: 10px; margin-bottom: 10px;" OnClick="btnFamilyDetailsAdd_Click" />
                                        </asp:Panel>

                                    </ContentTemplate>


                                </asp:UpdatePanel>
                            </div>

                            <div id="tabs-5">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <%--<div class="dashboard_firsthalf">
                                        <table cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td style="height: 20px" class="style4">
                                                    <strong>SSC :</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Name &amp; Address of School/Clg
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtschool" runat="server" TabIndex="1" TextMode="MultiLine" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Board/University
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtbrd" runat="server" TabIndex="2" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Year of Study
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtyear" runat="server" TabIndex="3" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Whether Pass/Failed
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpsfi" runat="server" TabIndex="4" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Percentage of Marks
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpmarks" runat="server" TabIndex="5" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px" class="style4">
                                                    <strong>INTERMEDIATE :</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Name &amp; Address of School/Clg
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimschool" runat="server" TabIndex="6" TextMode="MultiLine" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Board/University
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimbrd" runat="server" TabIndex="7" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Year of Study
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimyear" runat="server" TabIndex="8" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Whether Pass/Failed
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimpsfi" runat="server" TabIndex="9" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Percentage of Marks
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimpmarks" runat="server" TabIndex="10" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>
                                    
                                    <div class="dashboard_secondhalf">
                                        <table cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td style="height: 20px" class="style4">
                                                    <strong>DEGREE :</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Name &amp; Address of School/Clg
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgschool" runat="server" TabIndex="11" TextMode="MultiLine" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Board/University
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgbrd" runat="server" TabIndex="12" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Year of Study
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgyear" runat="server" TabIndex="13" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Whether Pass/Failed
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgpsfi" runat="server" TabIndex="14" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Percentage of Marks
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgpmarks" runat="server" TabIndex="15" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px" class="style4">
                                                    <strong>PG :</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Name &amp; Address of School/Clg
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgschool" runat="server" TabIndex="16" TextMode="MultiLine" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Board/University
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgbrd" runat="server" TabIndex="17" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Year of Study
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgyear" runat="server" TabIndex="18" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Whether Pass/Failed
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgpsfi" runat="server" TabIndex="19" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Percentage of Marks
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgpmarks" runat="server" TabIndex="20" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label runat="server" ID="lblquresult" Visible="false" Style="color: Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>--%>

                                        <asp:Panel ID="pnlEducationDetails" runat="server" GroupingText="<strong>&nbsp;Education Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div style="padding: 10px">
                                                <asp:GridView ID="GvEducationDetails" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderStyle="Solid" CellPadding="5" ForeColor="#333333" Height="180px" PageSize="25" Visible="true"
                                                    ShowHeader="true" Style="margin: 0px auto" Width="100%" CellSpacing="5">
                                                    <HeaderStyle Wrap="True" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="S.No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF3FB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Qualification" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlQualification" runat="server" Width="92%">
                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="ILLITERATE" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="NON-MATRIC" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="MATRIC" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="SENIOR SECONDARY" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="GRADUATE" Value="5"></asp:ListItem>
                                                                    <asp:ListItem Text="POST GRADUATE" Value="6"></asp:ListItem>
                                                                    <asp:ListItem Text="DOCTOR" Value="7"></asp:ListItem>
                                                                    <asp:ListItem Text="TECHNICAL/PROFESSIONAL" Value="8"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Description" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEdLevel" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Name & Address of School/College" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtNameofSchoolColg" runat="server" TextMode="MultiLine" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>



                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Board / University" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtBoard" runat="server" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Year of Study" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtyear" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Pass / Fail" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPassFail" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Percentage of Marks" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPercentage" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>


                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Button ID="btnEduAdd" runat="server" Text="Add" Style="margin-top: 10px" OnClick="btnEduAdd_Click" />

                                            </div>
                                        </asp:Panel>


                                        <asp:Panel ID="pnlPreviousExpereince" runat="server" GroupingText="<strong>&nbsp;Previous Experience&nbsp;</strong>" Style="margin-top: 10px">
                                            <div style="padding: 10px;">
                                                <asp:GridView ID="GvPreviousExperience" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderStyle="Solid" CellPadding="5" ForeColor="#333333" Height="180px" PageSize="25" Visible="true"
                                                    ShowHeader="true" Style="margin: 0px auto;" Width="100%" CellSpacing="5">
                                                    <HeaderStyle Wrap="True" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="S.No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF3FB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Region Code" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtregioncode" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Employer Code" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtempcode" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Extension" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtExtension" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Designation" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPrevDesignation" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>



                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="19%"
                                                            HeaderText="Company Name/Address" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCompAddress" runat="server" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Years of Experience" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtyearofexp" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="PF No." ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPFNo" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="ESI No." ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtESINo" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="15%"
                                                            HeaderText="Date Of Resigned" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDtofResigned" runat="server"
                                                                    MaxLength="10" placeholder="DD/MM/YYYY" Width="95%"></asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Button ID="btnPrevExpAdd" runat="server" Text="Add" Style="margin-top: 10px" OnClick="btnPrevExpAdd_Click" />
                                            </div>
                                        </asp:Panel>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>

                            <div id="tabs-6">

                                <div>
                                    <table cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>Police Verification No</td>
                                            <td>
                                                <asp:RadioButton ID="rdbVerified" runat="server" GroupName="P1" Text=" Verified" />
                                                <asp:RadioButton ID="rdbNotVerified" runat="server" GroupName="P1" Text=" Not Verified" Checked="True" /></td>
                                            <td>
                                                <asp:TextBox ID="txtPoliceVerificationNo" runat="server" CssClass="sinput" Enabled="false"></asp:TextBox></td>
                                            <td>Nearest Police Station</td>
                                            <td>
                                                <asp:TextBox ID="txtPoliceStation" runat="server" CssClass="sinput"></asp:TextBox></td>
                                        </tr>
                                    </table>

                                    <div style="margin-top: 10px">
                                        Criminal Offence 
                                             <asp:CheckBox ID="ChkCriminalOff" runat="server" Text=" (if criminal off is there)" />


                                        <asp:Panel ID="pnlGroupBox" runat="server" GroupingText="<strong>&nbsp;Criminal Offence&nbsp;</strong>" CssClass="pstyle" Enabled="false" Style="padding: 10px">
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>Criminal Off Court Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCriminalOffCName" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>Criminal Off Case No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCriminalOffcaseNo" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>Criminal Offence 
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCriminalOff" runat="server" class="sinput" Enabled="false" Style="margin-left: 15px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <br />
                                        Criminal Proceeding
                                            <asp:CheckBox ID="ChkCriminalProc" runat="server" Text=" (if any criminal proceeding are there,then tick)" />
                                        <asp:Panel ID="PnlCriminalProceeding" runat="server" GroupingText="<strong>&nbsp;Criminal Proceeding&nbsp;</strong>" CssClass="pstyle" Enabled="false" Style="padding: 10px">
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>Criminal Pro Court Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCriminalProCName" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>Criminal Pro Case No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCriminalProCaseNo" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>Criminal Pro Offence 
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCriminalProOffence" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <br />
                                        Criminal Arrest Warrant
                                            <asp:CheckBox ID="ChkCrimalArrest" runat="server" Text=" (if any criminal arrest warrant is issued,then tick)" />
                                        <asp:Panel ID="PnlCriminalArrest" runat="server" GroupingText="<strong>&nbsp;Criminal Arrest Warrant&nbsp;</strong>" CssClass="pstyle" Enabled="false" Style="padding: 10px">
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>Criminal Arrest Court Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCriminalArrestCName" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>Criminal Arrest Case No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCriminalArrestCaseNo" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>Criminal Arrest Offence 
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCriminalArrestOffence" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlbgv" runat="server" GroupingText="<strong>&nbsp;Back Ground Verification &nbsp;</strong>" CssClass="pstyle" Style="padding: 10px">
                                            <table cellpadding="5" cellspacing="5" style="width: 100%">
                                                <tr>
                                                    <td>BGV No</td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbbgvverified" runat="server" GroupName="P1" Text=" Verified" />
                                                        <asp:RadioButton ID="rdbbgvnotverified" runat="server" GroupName="P1" Text=" Not Verified" Checked="True" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtbgvno" runat="server" CssClass="sinput" Enabled="false"></asp:TextBox></td>

                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>

                            <div id="tabs-7" style="visibility: hidden">
                                <asp:Panel ID="PnlPaySheet" runat="server" GroupingText="<strong>&nbsp; Salary Structure &nbsp;</strong>" Style="margin-top: 10px">


                                    <div class="dashboard_firsthalf" style="padding: 10px">
                                        <table cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td style="height: 20px">No.Of Days
                                                </td>
                                                <td style="height: 20px; padding-left: 8px">
                                                    <asp:DropDownList ID="ddlNoOfDaysWages" runat="server" TabIndex="27" CssClass="sdrop">
                                                        <asp:ListItem>Gen</asp:ListItem>
                                                        <asp:ListItem>G-S</asp:ListItem>
                                                        <asp:ListItem>G-4</asp:ListItem>
                                                        <asp:ListItem>P.Hr</asp:ListItem>
                                                        <asp:ListItem>P.Day</asp:ListItem>
                                                        <asp:ListItem>24</asp:ListItem>
                                                        <asp:ListItem>25</asp:ListItem>
                                                        <asp:ListItem>26</asp:ListItem>
                                                        <asp:ListItem>27</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>31</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="height: 20px">Basic
                                                </td>
                                                <td style="height: 20px; padding-left: 8px">
                                                    <asp:TextBox ID="TxtBasic" runat="server" TabIndex="28" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBBasic" runat="server" Enabled="True"
                                                        TargetControlID="TxtBasic" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">HRA</td>
                                                <td style="padding-left: 8px; padding-left: 8px">
                                                    <asp:TextBox ID="txthra" runat="server" TabIndex="30" class="sinput"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBHRA" runat="server" Enabled="True" TargetControlID="txthra"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">CCA</td>
                                                <td style="padding-left: 8px; padding-left: 8px">
                                                    <asp:TextBox ID="txtcca" runat="server" TabIndex="32" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBCCA" runat="server" Enabled="True" TargetControlID="txtcca"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Gratuity</td>
                                                <td style="padding-left: 8px; padding-left: 8px">
                                                    <asp:TextBox ID="txtgratuty" runat="server" TabIndex="34" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F5" runat="server" Enabled="True" TargetControlID="txtgratuty"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">WA</td>
                                                <td style="padding-left: 8px">
                                                    <asp:TextBox ID="txtwa" runat="server" TabIndex="36" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F7" runat="server" Enabled="True" TargetControlID="txtwa"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">NFHs</td>
                                                <td style="padding-left: 8px">
                                                    <asp:TextBox ID="txtNfhs1" TabIndex="38" runat="server" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Fnhs" runat="server" Enabled="True" TargetControlID="txtNfhs1"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">RC
                                                </td>
                                                <td style="padding-left: 8px">
                                                    <asp:TextBox ID="Txtrc" runat="server" TabIndex="40" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Filterrc" runat="server" Enabled="True" TargetControlID="Txtrc"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">OT Rate
                                                </td>
                                                <td style="padding-left: 8px">
                                                    <asp:TextBox ID="TxtOTRate" runat="server" TabIndex="42" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Filterotrate" runat="server" Enabled="True" TargetControlID="TxtOTRate"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="height: 20px">Travelling Allowance
                                                </td>
                                                <td style="padding-left: 8px">
                                                    <asp:TextBox ID="txtTravellingAllowance" runat="server" TabIndex="42" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertr17" runat="server" Enabled="True" TargetControlID="txtTravellingAllowance"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td style="height: 20px">Mobile Allowance
                                                </td>
                                                <td style="padding-left: 8px">
                                                    <asp:TextBox ID="txtMobileAllowance" runat="server" TabIndex="44" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBMobAllw" runat="server" Enabled="True" TargetControlID="txtMobileAllowance"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                            <tr style="visibility: hidden">
                                                <td style="height: 20px">Edu. Allowance
                                                </td>
                                                <td style="padding-left: 8px">
                                                    <asp:TextBox ID="txtEducationAllowance" runat="server" TabIndex="46" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True" TargetControlID="txtEducationAllowance"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                            <tr style="height: 20px; visibility: hidden">
                                                <td>PF No of Days
                                                </td>
                                                <td style="padding-left: 8px">
                                                    <asp:DropDownList ID="ddlPFNoOfDaysForWages" runat="server" CssClass="sdrop">
                                                        <asp:ListItem>Gen</asp:ListItem>
                                                        <asp:ListItem>G-S</asp:ListItem>
                                                        <asp:ListItem>G-4</asp:ListItem>
                                                        <asp:ListItem>P.Hr</asp:ListItem>
                                                        <asp:ListItem>P.Day</asp:ListItem>

                                                        <asp:ListItem>24</asp:ListItem>
                                                        <asp:ListItem>25</asp:ListItem>
                                                        <asp:ListItem>26</asp:ListItem>
                                                        <asp:ListItem>27</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>31</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr style="visibility: hidden">
                                                <td style="height: 20px">PF PayRate</td>
                                                <td style="padding-left: 8px">
                                                    <asp:TextBox ID="TxtPFPayRate" runat="server" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Filterpfpayrate" runat="server" Enabled="True" TargetControlID="TxtPFPayRate"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                            <tr style="visibility: hidden">
                                                <td style="height: 20px">ESI PayRate</td>
                                                <td style="padding-left: 8px">
                                                    <asp:TextBox ID="TxtESIPayRate" runat="server" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True" TargetControlID="TxtESIPayRate"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>


                                        </table>
                                    </div>
                                    <div class="dashboard_secondhalf" style="padding: 10px">
                                        <table cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td style="height: 20px">&nbsp;
                                                </td>
                                                <td style="height: 20px">&nbsp;
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="height: 20px">DA
                                                </td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="txtda" runat="server" TabIndex="29" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                                        TargetControlID="txtda" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Conveyance</td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="txtConveyance" runat="server" TabIndex="31" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F2" runat="server" Enabled="True" TargetControlID="txtConveyance"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">LA</td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="txtleaveamount" runat="server" TabIndex="33" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F4" runat="server" Enabled="True" TargetControlID="txtleaveamount"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Bonus</td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="txtbonus" runat="server" TabIndex="35" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True" TargetControlID="txtbonus"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Bonus Type</td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:DropDownList ID="ddlBonusType" runat="server" CssClass="sdrop">
                                                        <asp:ListItem>Monthly</asp:ListItem>
                                                        <asp:ListItem>Yearly</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">OA</td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="txtoa" runat="server" TabIndex="37" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="F8" runat="server" Enabled="True" TargetControlID="txtoa"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Spl. Allowance
                                                </td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="txtSplAllw" runat="server" TabIndex="39" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True" TargetControlID="txtoa"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">CS
                                                </td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="TxtCs" runat="server" TabIndex="41" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="Filtercs" runat="server" Enabled="True" TargetControlID="TxtCs"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Nots
                                                </td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:DropDownList ID="ddlNoOfOtsPaysheet" TabIndex="43" runat="server" CssClass="sdrop">
                                                        <asp:ListItem>Gen</asp:ListItem>
                                                        <asp:ListItem>G-S</asp:ListItem>
                                                        <asp:ListItem>G-4</asp:ListItem>
                                                        <asp:ListItem>P.M(8Hrs)</asp:ListItem>
                                                        <asp:ListItem>G-S(8Hrs)</asp:ListItem>
                                                        <asp:ListItem>P.Hr</asp:ListItem>
                                                        <asp:ListItem>P.Day</asp:ListItem>
                                                        <asp:ListItem>24</asp:ListItem>
                                                        <asp:ListItem>25</asp:ListItem>
                                                        <asp:ListItem>26</asp:ListItem>
                                                        <asp:ListItem>27</asp:ListItem>
                                                        <asp:ListItem>30</asp:ListItem>
                                                        <asp:ListItem>31</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="height: 20px">Performance Allowance
                                                </td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="txtPerformanceAllowance" runat="server" TabIndex="41" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtepernder17" runat="server" Enabled="True" TargetControlID="txtPerformanceAllowance"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="height: 20px">Medical Allowance
                                                </td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="txtMedicalAllw" runat="server" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilterMedAll" runat="server" Enabled="True" TargetControlID="txtMedicalAllw"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr style="visibility: hidden">
                                                <td style="height: 20px">&nbsp;</td>
                                                <td style="height: 20px">&nbsp;</td>
                                                <td style="height: 20px">&nbsp;</td>

                                            </tr>
                                            <tr style="visibility: hidden">
                                                <td style="height: 20px">PF Voluntary</td>
                                                <td style="padding-left: 22px; height: 20px">
                                                    <asp:TextBox ID="TxtPFVoluntary" runat="server" class="sinput"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilterpfVoluntary" runat="server" Enabled="True" TargetControlID="TxtPFVoluntary"
                                                        ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr style="visibility: hidden">
                                                <td style="height: 30px">&nbsp;</td>
                                                <td style="height: 30px">&nbsp;</td>


                                            </tr>

                                            <tr style="visibility: hidden">
                                                <td style="height: 20px">&nbsp;</td>
                                                <td style="height: 20px">&nbsp;</td>


                                            </tr>

                                        </table>
                                    </div>

                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />



                    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" AutoPostBack="true">
                    </cc1:TabContainer>
                </div>
            </div>
        </div>

        <div class="clear">
        </div>
        <!-- DASHBOARD CONTENT END -->
        <%-- </div> </div>--%>
        <!-- CONTENT AREA END -->
</asp:Content>
