<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Employees/EmployeeMaster.master" CodeBehind="ClientAttendancePage.aspx.cs" Inherits="Kuduma.Portal.ClientAttendancePage" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <link rel="shortcut icon" href="../assets/Mushroom.ico" />
    <link rel="stylesheet" href="../css/global.css" />
    <link href="../css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <script src="../script/jquery.stickytableheaders.js" type="text/javascript"></script>

    <script type="text/javascript">
        //<![CDATA[
        window.onbeforeunload = function () {
            return '';
        };
        //]]>
        (function ($) {
            $.widget("custom.combobox", {
                _create: function () {
                    this.wrapper = $("<span>")
          .addClass("custom-combobox")
          .insertAfter(this.element);

                    this.element.hide();
                    this._createAutocomplete();
                    this._createShowAllButton();
                    this.input.attr("placeholder", this.element.attr('data-placeholder'));
                },

                _createAutocomplete: function () {
                    var selected = this.element.children(":selected"),
          value = selected.val() ? selected.text() : "";

                    this.input = $("<input>")
          .appendTo(this.wrapper)
          .val(value)
          .attr("title", "")
          .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
          .autocomplete({
              delay: 0,
              minLength: 0,
              source: $.proxy(this, "_source")
          })
          .tooltip({
              tooltipClass: "ui-state-highlight"
          });

                    this._on(this.input, {
                        autocompleteselect: function (event, ui) {
                            ui.item.option.selected = true;
                            this._trigger("select", event, {
                                item: ui.item.option
                            });
                        },

                        autocompletechange: "_removeIfInvalid"
                    });
                },

                _createShowAllButton: function () {
                    var input = this.input,
          wasOpen = false;

                    $("<a>")
          .attr("tabIndex", -1)
          .attr("title", "Show All")
          .tooltip()
          .appendTo(this.wrapper)
          .button({
              icons: {
                  primary: "ui-icon-triangle-1-s"
              },
              text: false
          })
          .removeClass("ui-corner-all")
          .addClass("custom-combobox-toggle ui-corner-right btnhgtwt")
          .mousedown(function () {
              wasOpen = input.autocomplete("widget").is(":visible");
          })
          .click(function () {
              input.focus();

              // Close if already visible
              if (wasOpen) {
                  return;
              }

              // Pass empty string as value to search for, displaying all results
              input.autocomplete("search", "");
          });
                },

                _source: function (request, response) {
                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                    response(this.element.children("option").map(function () {
                        var text = $(this).text();
                        if (this.value && (!request.term || matcher.test(text)))
                            return {
                                label: text,
                                value: text,
                                option: this
                            };
                    }));
                },

                _removeIfInvalid: function (event, ui) {

                    // Selected an item, nothing to do
                    if (ui.item) {
                        return;
                    }

                    // Search for a match (case-insensitive)
                    var value = this.input.val(),
          valueLowerCase = value.toLowerCase(),
          valid = false;
                    this.element.children("option").each(function () {
                        if ($(this).text().toLowerCase() === valueLowerCase) {
                            this.selected = valid = true;
                            return false;
                        }
                    });

                    // Found a match, nothing to do
                    if (valid) {
                        return;
                    }

                    // Remove invalid value
                    this.input
          .val("")
          .attr("title", value + " didn't match any item")
          .tooltip("open");
                    this.element.val("");
                    this._delay(function () {
                        this.input.tooltip("close").attr("title", "");
                    }, 2500);
                    this.input.autocomplete("instance").term = "";
                },

                _destroy: function () {
                    this.wrapper.remove();
                    this.element.show();
                }
            });
        })(jQuery);

        // forceNumeric() plug-in implementation
        jQuery.fn.forceNumeric = function () {

            return this.each(function () {
                $(this).keydown(function (e) {
                    var key = e.which || e.keyCode;

                    if (!e.shiftKey && !e.altKey && !e.ctrlKey &&
                        // numbers   
                         key >= 48 && key <= 57 ||
                        // Numeric keypad
                         key >= 96 && key <= 105 ||
                        // comma, period and minus, . on keypad
                        key == 190 || key == 188 || key == 109 || key == 110 ||
                        // Backspace and Tab and Enter
                        key == 8 || key == 9 || key == 13 ||
                        // Home and End
                        key == 35 || key == 36 ||
                        // left and right arrows
                        key == 37 || key == 39 ||
                        // Del and Ins
                        key == 46 || key == 45)
                        return true;

                    return false;
                });
                $(this).keydown(function (e) {
                    CalculateTotals();
                    var linetotal = 0;
                    $(this).parent().parent().find(".num-txt").each(function () {
                        linetotal += parseInt($(this).val());
                    });
                    $(this).parent().parent().find(".txt-linetotal").text(linetotal);
                });
            });
        }


        function chkchange() {
            if (document.getElementById("<%=chkold.ClientID%>").checked) {

                document.getElementById("<%=txtmonth.ClientID %>").style.visibility = "visible";
                document.getElementById("<%=ddlMonth.ClientID %>").style.visibility = "hidden";
                $("<%=txtmonth.ClientID %>").val("");


            } else {
                document.getElementById("<%=txtmonth.ClientID %>").style.visibility = "hidden";
                document.getElementById("<%=ddlMonth.ClientID %>").style.visibility = "visible";
                $("<%=txtmonth.ClientID %>").val("");

            }
        }





        function reorder() {
            var i = 1;
            $('#tblattendancegrid > tbody tr').each(function () {
                $(this).find("td:first").text(i);
                i++;
            });
        }




        $(document).ready(function () {
            $("table").stickyTableHeaders({ scrollableArea: window, "fixedOffset": 0 });
            $('.destroy').on('click', function (e) {
                $("#tblattendancegrid").stickyTableHeaders('destroy');
            });
            $('.apply').on('click', function (e) {
                $('#tblattendancegrid').stickyTableHeaders({ scrollableArea: $(".scrollable-area")[2], "fixedOffset": 2 });
            });
        });

        $(document).ready(function () {
            $(".num-txt").forceNumeric();
            $(".txt-calender").datepicker({ defaultDate: new Date(), dateFormat: 'dd/mm/yy' });
            var tdate = new Date();
            $(".txt-calender").val(getFormattedDate(tdate));
            GetClientsValues();
            $(".ddlautocomplete").combobox({
                select: function (event, ui) { $("#<%=divClient.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLchange(event, ui); },
                minLength: 4
            });


            $("#<%=txtoldEmpId.ClientID %>").autocomplete({

                source: function (request, response) {

                    var month = 0;
                    var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                    if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                        month = $("#<%=txtmonth.ClientID %>").val();
                    }
                    else {
                        month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                    }
                    var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetEmployessData";
                    $.ajax({
                        type: "POST",
                        url: ajaxUrl,
                        //data: '{ "strid": "' + request.term + '"}',
                        data: "{strid:'" + request.term + "',month:'" + month + "',Chk:'" + Chk + "'}",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (json) {
                            console.log(json);
                            if (json != "") {

                                response($.map(json, function (item) {
                                    var obj = { value: item.EmpName + "|<>|" + item.empstatus + "|<>|" + item.EmpId, label: item.OLDEMPID };
                                    return obj;
                                }));
                            }
                        },
                        error: function (json) { InvalidEmpData(); }
                    });
                },
                minLength: 3,
                select: function (event, ui) {

                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-id");
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-name");
                   <%-- $("#<%=trAddData.ClientID %>").removeAttr("data-emp-desg");--%>
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-empstatus");
                    $("#<%=trAddData.ClientID %>").removeAttr("data-oldemp-id");

                    var vals = ui.item.value.split('|<>|');
                    $("#<%=txtEmpName.ClientID %>").val(vals[0]);
                   <%-- $("#<%=ddlEmpDesg.ClientID %>").val(vals[0]);--%>
                    $("#<%=lblempstatus.ClientID %>").val(vals[1]);
                    $("#<%=txtEmpId.ClientID %>").val(vals[2]);

                    $("#<%=trAddData.ClientID %>").attr("data-emp-id", vals[2]);
                    $("#<%=trAddData.ClientID %>").attr("data-emp-name", vals[0]);
                   <%-- $("#<%=trAddData.ClientID %>").attr("data-emp-desg", vals[0]);--%>
                    $("#<%=trAddData.ClientID %>").attr("data-emp-status", vals[1]);
                    $("#<%=trAddData.ClientID %>").attr("data-oldemp-id", ui.item.label);

                    GetAttendanceDuplicates();
                    this.value = ui.item.label
                    return false;
                }
            });


            $("#<%=txtEmpId.ClientID %>").autocomplete({

                source: function (request, response) {

                    var month = 0;
                    var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                    if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                        month = $("#<%=txtmonth.ClientID %>").val();
                    }
                    else {
                        month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                    }
                    var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetSofwareEmployessData";
                    $.ajax({
                        type: "POST",
                        url: ajaxUrl,
                        //data: '{ "strid": "' + request.term + '"}',
                        data: "{strid:'" + request.term + "',month:'" + month + "',Chk:'" + Chk + "'}",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (json) {
                            console.log(json);
                            if (json != "") {

                                response($.map(json, function (item) {
                                    var obj = { value: item.EmpName + "|<>|" + item.empstatus + "|<>|" + item.OLDEMPID, label: item.EmpId };
                                    return obj;
                                }));
                            }
                        },
                        error: function (json) { InvalidEmpData(); }
                    });
                },
                minLength: 3,
                select: function (event, ui) {

                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-id");
                      $("#<%=trAddData.ClientID %>").removeAttr("data-emp-name");
                   <%-- $("#<%=trAddData.ClientID %>").removeAttr("data-emp-desg");--%>
                      $("#<%=trAddData.ClientID %>").removeAttr("data-emp-empstatus");
                      $("#<%=trAddData.ClientID %>").removeAttr("data-oldemp-id");

                      var vals = ui.item.value.split('|<>|');
                      $("#<%=txtEmpName.ClientID %>").val(vals[0]);
                   <%-- $("#<%=ddlEmpDesg.ClientID %>").val(vals[0]);--%>
                      $("#<%=lblempstatus.ClientID %>").val(vals[1]);
                      $("#<%=txtoldEmpId.ClientID %>").val(vals[2]);

                      $("#<%=trAddData.ClientID %>").attr("data-oldemp-id", vals[2]);
                      $("#<%=trAddData.ClientID %>").attr("data-emp-name", vals[0]);
                   <%-- $("#<%=trAddData.ClientID %>").attr("data-emp-desg", vals[0]);--%>
                      $("#<%=trAddData.ClientID %>").attr("data-emp-status", vals[1]);
                      $("#<%=trAddData.ClientID %>").attr("data-emp-id", ui.item.label);


                      GetAttendanceDuplicates();
                      this.value = ui.item.label
                      return false;
                  }
            });






            $("#<%=txtEmpName.ClientID %>").autocomplete({

                source: function (request, response) {

                    var month = 0;
                    var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                    if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                        month = $("#<%=txtmonth.ClientID %>").val();
                    }
                    else {
                        month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                    }
                    var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetEmployessData";
                    $.ajax({
                        type: "POST",
                        url: ajaxUrl,
                        //data: '{ "strid": "' + request.term + '"}',
                        data: "{strid:'" + request.term + "',month:'" + month + "',Chk:'" + Chk + "'}",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (json) {
                            console.log(json);
                            if (json != "") {

                                response($.map(json, function (item) {
                                    var obj = { value: item.OLDEMPID + "|<>|" + item.empstatus + "|<>|" + item.EmpId, label: item.EmpName };
                                    return obj;
                                }));
                            }
                        },
                        error: function (json) { InvalidEmpData(); }
                    });
                },
                minLength: 3,
                select: function (event, ui) {

                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-id");
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-name");
                   <%-- $("#<%=trAddData.ClientID %>").removeAttr("data-emp-desg");--%>
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-empstatus");
                    $("#<%=trAddData.ClientID %>").removeAttr("data-oldemp-id");

                    var vals = ui.item.value.split('|<>|');
                    $("#<%=txtoldEmpId.ClientID %>").val(vals[0]);
                   <%-- $("#<%=ddlEmpDesg.ClientID %>").val(vals[0]);--%>
                    $("#<%=lblempstatus.ClientID %>").val(vals[1]);
                    $("#<%=txtEmpId.ClientID %>").val(vals[2]);

                    $("#<%=trAddData.ClientID %>").attr("data-emp-id", vals[2]);
                    $("#<%=trAddData.ClientID %>").attr("data-emp-name", ui.item.label);
                   <%-- $("#<%=trAddData.ClientID %>").attr("data-emp-desg", vals[0]);--%>
                    $("#<%=trAddData.ClientID %>").attr("data-emp-status", vals[1]);
                    $("#<%=trAddData.ClientID %>").attr("data-oldemp-id", vals[0]);

                    GetAttendanceDuplicates();
                    this.value = ui.item.label
                    return false;
                }
            });





        });

        function InvalidEmpData() {
            $("#<%=txtEmpName.ClientID %>").val("");
            $("#<%=txtEmpId.ClientID %>").val("");
            $("#<%=txtoldEmpId.ClientID %>").val("");
            $("#<%=trAddData.ClientID %>").attr("data-emp-id", "");
            $("#<%=trAddData.ClientID %>").attr("data-emp-name", "");
            $("#<%=trAddData.ClientID %>").attr("data-emp-desg", "");
            alert("invalid!!");
        }

        function GetClientsValues() {
            var json = JSON.parse($("#<%=hdClientData.ClientID %>").val());
           $("#<%=divClient.ClientID %>").attr("data", JSON.stringify(json));
           var data = json;
           BindClientIdDDL(data);
           BindClientNameDDL(data);
       }

       function BindClientIdDDL(data) {
           $("#<%=ddlClientID.ClientID %>").html("");
            $("#<%=ddlClientID.ClientID %>").append("<option value='-1'></option>");

            var databs = [];
            $.each(data, function (index, element) {
                databs.push(element.ClientId);
            });
            databs.sort();
            $.each(databs, function (index, element) {
                $("#<%=ddlClientID.ClientID %>").append("<option value=" + element + ">" + element + "</option>");
            });
            }

            function BindClientNameDDL(data) {
                $("#<%=ddlClientName.ClientID %>").html("");
                $("#<%=ddlClientName.ClientID %>").append("<option value='-1'></option>");
                $.each(data, function (index, element) {
                    $("#<%=ddlClientName.ClientID %>").append("<option value=" + element.ClientId + ">" + element.ClientName + "</option>");
                });
                }

                function SetAutoCompleteValue(ddlid, defValue) {

                    if (ddlid == "id") {
                        $("#<%=ddlClientName.ClientID %>").combobox("destroy");
                       $("#<%=ddlClientName.ClientID %>").val(defValue);
                       $("#<%=ddlClientName.ClientID %>").combobox({
                           select: function (event, ui) { OnAutoCompleteDDLchange(event, ui); }
                       });
                   }
                   else {
                       $("#<%=ddlClientID.ClientID %>").combobox("destroy");
                       $("#<%=ddlClientID.ClientID %>").val(defValue);
                       $("#<%=ddlClientID.ClientID %>").combobox({
                           select: function (event, ui) { OnAutoCompleteDDLchange(event, ui); }
                       });
                   }
               }

               function Reset() {
                   var dropDown = document.getElementById("ddlMonth");
                   dropDown.selectedIndex = 0;
               }

               function OnAutoCompleteDDLchange(event, ui) {

                   var targetddlid = "";
                   console.log(event.target.id);
                   if (event.target.id === "ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_ddlClientID") { targetddlid = "id"; }
                   else if (event.target.id === "ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_ddlClientName") targetddlid = "name";
                   SetAutoCompleteValue(targetddlid, ui.item.value);
                   $("#<%=ddlMonth.ClientID %>").val(0);
            $("#tblattendancegrid>tbody").html("");  //tblattendancegrid
            $("#tblSummary >tbody").html("");
            if (targetddlid == '#<%=ddlClientName.ClientID %>' || targetddlid == '#<%=ddlClientID.ClientID %>') { ChangeClientValues(ui.item.value); }
        }

        function ChangeClientValues(cid) {
            var datastr = $("#divClient").attr("data");
            var data = eval(datastr);
            $.each(data, function (indx, ele) {
                if (ele.ClientId == cid) {
                    $("#txtphonenumbers").val(ele.PhoneNumber);
                    $("#txtocp").val(ele.ContactPerson);
                    $("#<%=ddlMonth.ClientID %>").val(0);
                }
            });
            $("#tblattendancegrid >tbody").html("");
            $(".num-txt").forceNumeric();
            CalculateTotals();
        }

        function ChangeDesgnValues(cid, month) {
            debugger
            $("#<%=ddlEmpDesg.ClientID %>").empty();
                    var o = new Option("Select", "0");
                    $(o).html("Select");
                    $("#<%=ddlEmpDesg.ClientID %>").append(o);
                 $.ajax
                       ({
                           type: "POST",
                           url: "/FameService.asmx/BindDesgnTempAttendance",
                           data: "{clientId:'" + cid + "',month:'" + month + "'}",
                           contentType: "application/json;charset=utf-8",
                           dataType: "json",
                           success: function (r) {
                               $.each(r, function () {
                                   debugger
                                   $("#<%=ddlEmpDesg.ClientID %>").append($('<option>').text(this.Design).attr('value', this.DesignId));

                               });
                           },

                           error: function (err) {

                           }
                       });


                   }

        
        function AddNewEmp(ele) {

            debugger
                       var empid = $("#<%=txtEmpId.ClientID %>").val();
                       var oldempid = $("#<%=txtoldEmpId.ClientID %>").val();
                       var empname = $("#<%=txtEmpName.ClientID %>").val();
                       var empdesgid = $("#<%=ddlEmpDesg.ClientID %>").val();
                       var empdesgname = $('#<%= ddlEmpDesg.ClientID %> option:selected').text();
                       var empttype = $('#<%= ddlTransfertype.ClientID %> option:selected').val();
                       var jdate = $("#txtJoingingDate").val();
                       var rdate = $("#txtRelievingDate").val();
                       var esi = $("#chkESI").is(":checked");
                       var pt = $("#chkPT").is(":checked");
                       var pf = $("#chkPF").is(":checked");
                       var nod = $("#txt-add-nod").val();
                       var ot = $("#txt-add-ot").val();
                       var wo = $("#txt-add-wo").val();
                       var nhs = $("#txt-add-nhs").val();
                       var npots = $("#txt-add-npots").val();
                       var othrs = $("#txt-add-othrs").val();
                       var canadv = $("#txt-add-canadv").val();
                       var pen = $("#txt-add-pen").val();
                       var unided = $("#txt-add-unided").val();
                       var atmded = $("#txt-add-atmded").val();
                       var inctvs = $("#txt-add-inctvs").val();
                       var Arrears = $("#txt-add-arrears").val();

                       var empstatus = $("#<%=lblempstatus.ClientID %>").val();
                       var stoppayment = $("#chkstoppayment").is(":checked");
                       var updated = false;


                       //var ajaxUrl = window.location.href.substring(0, window.location.href.lastIndexOf('/')) + "/FameService.asmx/CheckSite";
                       //var clientId = $("#divClient").attr("data-clientId");
                       //var trmonth = $("#ddlMonth option:selected").index();
                       //var dataparam = JSON.stringify({ empId: empid, empclientid: clientId, month: trmonth, Desgn: empdesgid })

                       //$.ajax({
                       //    type: "POST",
                       //    url: ajaxUrl,
                       //    data: dataparam,
                       //    async: false,
                       //    contentType: "application/json; charset=utf-8",
                       //    dataType: "json",
                       //    success: function (json) {
                       //        if (json != "") {
                       //            if (json.msg == "success") {
                       //                $("#txtEmpId").val("");
                       //                $("#txtEmpName").val("");
                       //                $("#ddlEmpDesg").val("0");
                       //                alert(json.Obj);
                       //            }
                       //            else if (json.msg == "fail") {

                                       if ($('#tblattendancegrid > tbody > tr').length > 0) {
                                           $('#tblattendancegrid > tbody > tr').each(function (i, row) {
                                               var trempid = $(row).attr("data-emp-id");
                                               var trempdesg = $(row).attr("data-emp-desg");
                                               if (empid == trempid && empdesgid == trempdesg) {
                                                   $(row).find(".txt-nod").val(nod);
                                                   $(row).find(".txt-ot").val(ot);
                                                   $(row).find(".txt-wo").val(wo);
                                                   $(row).find(".txt-nhs").val(nhs);
                                                   $(row).find(".txt-nposts").val(npots);
                                                   $(row).find(".txt-othrs").val(othrs);
                                                   $(row).find(".txt-candav").val(canadv);
                                                   $(row).find(".txt-pen").val(pen);
                                                   $(row).find(".txt-unided").val(unided);
                                                   $(row).find(".txt-atmded").val(atmded);
                                                   $(row).find(".txt-inctvs").val(inctvs);
                                                   $(row).find(".txt-Arrears").val(Arrears);
                                                   $(row).find("#chkstoppayment").val(stoppayment);
                                                   alert("Employee attendance updated.");
                                                   updated = true;
                                               }
                                           });
                                       }

                                       if (!updated) {
                                           var nr = "<tr class='tr-emp-att new-row' data-emp-id='##EMPID##' data-emp-desg='##EMPDESG##' data-emp-status='##EMPSTATUS##' data-emp-ttype='##EMPTTYPE##' data-emp-jdate='##EMPJDATE##' data-emp-rdate='##EMPRDATE##' data-emp-pf='##EMPPF##' data-emp-pt='##EMPPT##' data-emp-esi='##EMPESI##' >" +
                                                            "<td></td><td><input type='checkbox' id='chkstoppayment' value='##STOPPAYMENT##' /><td>##EMPID##<br/>##OLDEMPLID##</td><td>##EMPNAME##</td><td>##EMPDESGNAME##</td>" +
                                                            "<td><input type='text' class='form-control num-txt txt-nod line-cal' value='##NOD##'></td>" +
                                                           " <td><input type='text' class='form-control num-txt txt-ot line-cal' value='##OT##'></td>" +
                                                            "<td><input type='text' class='form-control num-txt txt-wo line-cal' value='##WO##'></td>" +
                                                           " <td><input type='text' class='form-control num-txt txt-nhs line-cal' value='##NHS##'></td>" +
                                                           " <td><input type='text' class='form-control num-txt txt-nposts line-cal' value='##NPOSTS##'></td>" +
                                                           " <td><input type='text' class='form-control num-txt txt-othrs line-cal' value='##OTHRS##'></td>" +
                                                           " <td><input type='text' class='form-control num-txt txt-candav' value='##CANADV##'></td>" +
                                                           " <td><input type='text' class='form-control num-txt txt-pen' value='##PEN##'></td>" +
                                                            " <td><input type='text' class='form-control num-txt txt-unided' value='##UNIDED##'></td>" +
                                                             " <td><input type='text' class='form-control num-txt txt-atmded' value='##ATMDED##'></td>" +
                                                           " <td><input type='text' class='form-control num-txt txt-inctvs' value='##INCTVS##'></td>  " +
                                                           " <td><input type='text' class='form-control num-txt txt-Arrears' value='##ARREARS##'></td>  " +
                                                           " <td><label class='txt-linetotal'/> " +
                                                           " <td><button type='button' class='btn btn-danger' onclick='DeleteRow(this); return false;'><i class='glyphicon glyphicon-trash'></i></button></td>" +
                                                          " </tr>";
                                           if (empstatus != "false") {
                                               if (empid != "" && empdesgid != "0") {
                                                   var newrow = nr.replace("##EMPID##", empid).replace("##EMPID##", empid)
                                                              .replace('##OLDEMPLID##', oldempid)
                                                             .replace('##EMPNAME##', empname)
                                                             .replace('##EMPDESG##', empdesgid)
                                                             .replace('##EMPDESGNAME##', empdesgname)
                                                             .replace('##EMPJDATE##', jdate)
                                                             .replace('##EMPRDATE##', rdate)
                                                             .replace('##EMPPF##', pf)
                                                             .replace('##EMPPT##', pt)
                                                             .replace('##EMPESI##', esi)
                                                             .replace('##EMPTTYPE##', empttype)
                                                             .replace('##NOD##', nod)
                                                             .replace('##OT##', ot)
                                                             .replace('##WO##', wo)
                                                             .replace('##NHS##', nhs)
                                                             .replace('##NPOSTS##', npots)
                                                       .replace('##OTHRS##', othrs)
                                                             .replace('##CANADV##', canadv)
                                                             .replace('##PEN##', pen)
                                                        .replace('##UNIDED##', unided)
                                                        .replace('##ATMDED##', atmded)
                                                             .replace('##INCTVS##', inctvs)
                                                       .replace('##ARREARS##', Arrears)
                                                       .replace('##STOPPAYMENT##', stoppayment);

                                                   $("#tblattendancegrid >tbody").append(newrow);
                                                   alert("Employee added.");

                                                   reorder();
                                               }
                                               else {
                                                   alert("Select Employee and Designation");
                                               }
                                           }
                                           else {
                                               alert("Employee Cannot be posted to the unit as the selected employee is inactive");

                                           }
                                       }
                                       $(".num-txt").forceNumeric();
                                       CalculateTotals();
                                       ClearEmpAddValues();
                                   }
                              

                   function ClearEmpAddValues() {
                       $("#<%=txtEmpId.ClientID %>").val("");
                       $("#<%=txtEmpName.ClientID %>").val("");
                       $("#<%=txtoldEmpId.ClientID %>").val("");
                <%--$("#<%=ddlEmpDesg.ClientID %>").val(0);--%>
                       $("#<%=ddlTransfertype.ClientID %>").val(0);
                       // var tdate = new Date();
                       //$(".txt-calender").val(getFormattedDate(tdate));
                       $("#chkESI")[0].checked = true;
                       $("#chkPT")[0].checked = true;
                       $("#chkPF")[0].checked = true;
                       $("#txt-add-nod").val("0");
                       $("#txt-add-ot").val("0");
                       $("#txt-add-wo").val("0");
                       $("#txt-add-nhs").val("0");
                       $("#txt-add-npots").val("0");
                       $("#txt-add-othrs").val("0");
                       $("#txt-add-canadv").val("0");
                       $("#txt-add-pen").val("0");
                       $("#txt-add-unided").val("0");
                       $("#txt-add-atmded").val("0");
                       $("#txt-add-inctvs").val("0");
                       $("#txt-add-arrears").val("0");
                       $("#<%=lblempstatus.ClientID %>").val("");
                 <%-- $("#<%=txtEmpId.ClientID %>").focus();--%>
                       $("#<%=txtoldEmpId.ClientID %>").focus();
                       $("#divDuplicates").hide();

                   }

                   function DeleteRow(ele) {
                       if (confirm("Are you sure you want to remove the employee from this unit?")) {
                           if ($(ele).parent().parent().hasClass("new-row")) {
                               $(ele).parent().parent().remove();
                               alert("Employee deleted for current month.");
                           }
                           else {
                               var trclientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
                                 // var trmonth = $("#ddlMonth option:selected").index();
                                 var trmonth = 0;
                                 var trChk = $("#<%=chkold.ClientID %>").is(":checked");
                          if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                                     trmonth = $("#<%=txtmonth.ClientID %>").val();
                         }
                         else {
                             trmonth = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                         }
                         var trempid = $(ele).parent().parent().attr("data-emp-id");
                         var trempdesg = $(ele).parent().parent().attr("data-emp-desg");
                         var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                         var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/DeleteAttendance";
                         if (trclientId != undefined && trclientId != "0" && trclientId != "" && trmonth != undefined && trmonth != "0") {
                             var dataparam = JSON.stringify({ empId: trempid, empDesgId: trempdesg, clientId: trclientId, month: trmonth });
                             $.ajax({
                                 type: "POST",
                                 url: ajaxUrl,
                                 data: dataparam,
                                 async: false,
                                 contentType: "application/json; charset=utf-8",
                                 dataType: "json",
                                 success: function (json) {
                                     if (json != "") {
                                         $(ele).parent().parent().remove();
                                         alert("Employee deleted for current month.");
                                     }
                                 },
                                 error: function (json) { alert(json); }
                             });
                         } else {
                             alert('select ClientID');
                         }
                     }
                     CalculateTotals();
                 }
                 reorder();
             }

             function Empddlchange(ele) {
                 var id = $(ele).attr("id");
                 if (id == "ddlEmpId") {
                     var val = $("#ddlEmpId option:selected").val();
                     var txt = $("#ddlEmpId option:selected").text();
                     $("#ddlEmpName").val(txt);
                     $("#<%=ddlEmpDesg.ClientID %>").val(val);
                  }
                  if (id == "ddlEmpName") {
                      $("#ddlEmpId option").removeAttr("selected");
                      var val = $("#ddlEmpName option:selected").val();
                      var empdes = $("#ddlEmpId option:contains(" + val + ")").val();
                      var empid = $("#ddlEmpId option:contains(" + val + ")").text();
                      $("#ddlEmpId option").each(function () {
                          if ($(this).text() == empid) {
                              $(this).attr('selected', 'selected');
                          }
                      });
                      $("#<%=ddlEmpDesg.ClientID %>").val(empdes);
                 }
             }


             function DownloadPDF() {
                 debugger
                 // openModal();
                 var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
                 var month = 0;
                 var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                  if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                     month = $("#<%=txtmonth.ClientID %>").val();
                    }
                    else {
                        month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                    }
                    var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/DownloadPDF";
                    if (clientId != undefined && clientId != "0" && clientId != "" && month != undefined && month != "0") {
                        $.ajax({
                            type: "POST",
                            url: ajaxUrl,
                            data: "{clientId:'" + clientId + "',month:'" + month + "',Chk:'" + Chk + "'}",
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (json) {

                            },
                            error: function (json) { alert('fail'); }
                        });
                    } else {
                        alert("Select ClientId and month");
                    }
                }

                function GetAttendance() {
                    GetEmpAttendanceData();

                    var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
                 var month = 0;
                 var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                     month = $("#<%=txtmonth.ClientID %>").val();
                 }
                 else {
                     month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                 }

                 ChangeDesgnValues(clientId, month);
             }

             function GetEmpAttendanceData() {
                 // openModal();
                 var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
            var month = 0;
            var Chk = $("#<%=chkold.ClientID %>").is(":checked");
            if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                month = $("#<%=txtmonth.ClientID %>").val();
                 }
                 else {
                     month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                 }
                 var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                 var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetAttendanceGrid";
                 if (clientId != undefined && clientId != "0" && clientId != "" && month != undefined && month != "0") {
                     $.ajax({
                         type: "POST",
                         url: ajaxUrl,
                         data: "{clientId:'" + clientId + "',month:'" + month + "',Chk:'" + Chk + "'}",
                         async: false,
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         success: function (json) {
                             if (json != "") {
                                 if (json.msg == "success") {
                                     AddrowstoTbl(json.Obj);
                                 }
                                 else if (json.msg == "fail") {
                                     $("#tblattendancegrid >tbody").html("");
                                 }
                                 else if (json.msg == "nodata") {
                                     $("#tblattendancegrid >tbody").html("");
                                 }
                             }
                         },
                         error: function (json) { alert('fail'); }
                     });
                 } else {
                     alert("Select ClientId and month");
                 }
            //closeModal();
                 GetEmpAttendanceDataSummarry();

                 reorder();
             }

             function AddrowstoTbl(data) {
                 debugger;
                 data = eval(data);
                 $("#tblattendancegrid >tbody").html("");
                 $.each(data, function (i, item) {
                     var nr = "<tr class='tr-emp-att' data-emp-id='##EMPID##' data-emp-desg='##EMPDESG##' data-emp-ttype='##EMPTTYPE##' data-emp-jdate='##EMPJDATE##' data-emp-rdate='##EMPRDATE##' data-emp-pf='##EMPPF##' data-emp-pt='##EMPPT##' data-emp-esi='##EMPESI##' >" +
                                      " <td></td> <td><input type='checkbox' id='chkstoppayment'  /></td><td>##EMPID##<br/>##OLDEMPLID##</td><td>##EMPNAME##</td><td>##EMPDESGNAME##</td>" +
                                      "<td><input type='text' class='form-control num-txt txt-nod line-cal' value='##NOD##'></td>" +
                                     " <td><input type='text' class='form-control num-txt txt-ot line-cal' value='##OT##'></td>" +
                                      "<td><input type='text' class='form-control num-txt txt-wo line-cal' value='##WO##'></td>" +
                                     " <td><input type='text' class='form-control num-txt txt-nhs line-cal' value='##NHS##'></td>" +
                                     " <td><input type='text' class='form-control num-txt txt-nposts line-cal' value='##NPOSTS##'></td>" +
                                      " <td><input type='text' class='form-control num-txt txt-othrs line-cal' value='##OTHRS##'></td>" +
                                     " <td><input type='text' class='form-control num-txt txt-candav' value='##CANADV##'></td>" +
                                     " <td><input type='text' class='form-control num-txt txt-pen' value='##PEN##'></td>" +
                                       " <td><input type='text' class='form-control num-txt txt-unided' value='##UNIDED##'></td>" +
                                       " <td><input type='text' class='form-control num-txt txt-atmded' value='##ATMDED##'></td>" +
                                     " <td><input type='text' class='form-control num-txt txt-inctvs' value='##INCTVS##'></td>  " +
                                     " <td><input type='text' class='form-control num-txt txt-Arrears' value='##ARREARS##'></td>  " +
                                     " <td><label class='txt-linetotal'/> " +
                                     " <td><button type='button' class='btn btn-danger' onclick='DeleteRow(this); return false;'><i class='glyphicon glyphicon-trash'></i></button></td>" +
                                    " </tr>";

                     var newrow = nr.replace("##EMPID##", item.EmpId).replace("##EMPID##", item.EmpId)
                               .replace('##OLDEMPLID##', item.OLDEMPID)
                               .replace('##EMPNAME##', item.EmpName)
                               .replace('##EMPDESG##', item.DesgId)
                               .replace('##EMPDESGNAME##', item.DesgName)
                               .replace('##NOD##', item.NoOfDuties)
                               .replace('##OT##', item.OT)
                               .replace('##WO##', item.WO)
                               .replace('##NHS##', item.NHS)
                               .replace('##NPOSTS##', item.NPosts)
                         .replace('##OTHRS##', item.OTHRS)
                               .replace('##CANADV##', item.CanteenAdv)
                               .replace('##PEN##', item.Penalty)
                           .replace('##UNIDED##', item.UNIDED)
                           .replace('##ATMDED##', item.ATMDED)
                               .replace('##INCTVS##', item.Incentivs)
                               .replace('##ARREARS##', item.Arrears);
                     $("#tblattendancegrid >tbody").append(newrow);
                     if (item.stoppayment == false) {
                         $("#tblattendancegrid > tbody >tr[data-emp-id=" + item.EmpId + "]").find("input[type=checkbox]").prop("checked", false);
                     }
                     else { $("#tblattendancegrid > tbody >tr[data-emp-id=" + item.EmpId + "]").find("input[type=checkbox]").prop("checked", true); }
                 });

                 $(".num-txt").forceNumeric();
                 CalculateTotals();
             }

             function GetEmpAttendanceDataSummarry() {
                 // openModal();
                 var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
            var month = 0;
            var Chk = $("#<%=chkold.ClientID %>").is(":checked");
            if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                month = $("#<%=txtmonth.ClientID %>").val();
                 }
                 else {
                     month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                 }
                 var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                 var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetAttendanceSummary";
                 if (clientId != undefined && clientId != "0" && clientId != "" && month != undefined && month != "0") {
                     $.ajax({
                         type: "POST",
                         url: ajaxUrl,
                         data: "{clientId:'" + clientId + "',month:'" + month + "',Chk:'" + Chk + "'}",
                         async: false,
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         success: function (json, b, c) {
                             if (json != "") {
                                 debugger;
                                 //var res = JSON.parse(json.d);
                                 if (json.msg == "success") {
                                     AddSummaryTbl(json.Obj);
                                     $("#divSummary").show();
                                 }
                                 else if (json.msg == "nodata") {
                                     $("#tblSummary >tbody").html("");
                                     $("#divSummary").hide();
                                 }
                             }
                         },
                         error: function (json) { alert('fail'); }
                     });
                 }
            // closeModal();
             }

             function AddSummaryTbl(data) {
                 data = eval(data);
                 $("#tblSummary >tbody").html("");
                 $.each(data, function (i, item) {
                     var strr = "<tr class='tr-emp-summary'>" +
                        " <td><label class='lbl-tdesg lbl-thin'>##Designation##</label></td>" +
                         "<td><label class='lbl-tnod lbl-thin lbl-tots'>##TNOD##</label></td>" +
                         "<td><label class='lbl-tot lbl-thin lbl-tots'>##TOT##</label></td>" +
                         "<td><label class='lbl-two lbl-thin lbl-tots'>##TWO##</label></td>" +
                         "<td><label class='lbl-tnhs lbl-thin lbl-tots'>##TNHS##</label></td>" +
                         "<td><label class='lbl-tnpots lbl-thin lbl-tots'>##TNPOTS##</label></td>" +
                          "<td><label class='lbl-tothrs lbl-thin lbl-tothrs'>##TOTHRS##</label></td>" +
                         "<td><label class='lbl-Totals'></label></td>" +
                         "<td><label class='lbl-tcadv lbl-thin'>##TCADV##</label></td>" +
                         "<td><label class='lbl-tpen lbl-thin'>##TPEN##</label></td>" +
                           "<td><label class='lbl-tunided lbl-thin'>##TUNIDED##</label></td>" +
                             "<td><label class='lbl-tatmded lbl-thin'>##TATMDED##</label></td>" +
                         "<td><label class='lbl-tinctvs lbl-thin'>##TINTVS##</label></td>" +
                         "<td><label class='lbl-tarrears lbl-thin'>##TARREARS##</label></td></tr>";
                     var newrow = strr.replace("##Designation##", item.DesgName)
                                 .replace('##TNOD##', item.NODTotal)
                                 .replace('##TOT##', item.OTTotal)
                                 .replace('##TWO##', item.WOTotal)
                                 .replace('##TNHS##', item.NHSTotal)
                                 .replace('##TNPOTS##', item.NpotsTotal)
                         .replace('##TOTHRS##', item.OTHRSTotal)
                                 .replace('##TCADV##', item.PenTotal)
                                 .replace('##TPEN##', item.InctvsTotal)
                          .replace('##TUNIDED##', item.UNIDEDTotal)
                          .replace('##TATMDED##', item.ATMDEDTotal)
                                 .replace('##TINTVS##', item.CanAdvTotal)
                                 .replace('##TARREARS##', item.ArrearsTotal);
                     $("#tblSummary >tbody").append(newrow);
                 });
                 CalculateSummaryTotals();
             }

             function CalculateTotals() {
                 var nodtotal = 0;
                 $('.txt-nod').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         nodtotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblNOD").text(nodtotal);

                 var ottotal = 0;
                 $('.txt-ot').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         ottotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblOT").text(ottotal);

                 var wototal = 0;
                 $('.txt-wo').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         wototal += parseFloat($(this).val());
                     }
                 });
                 $("#lblWO").text(wototal);

                 var nhstotal = 0;
                 $('.txt-nhs').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         nhstotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblNHS").text(nhstotal);

                 var npoststotal = 0;
                 $('.txt-nposts').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         npoststotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblNpots").text(npoststotal);

                 var othrstotal = 0;
                 $('.txt-othrs').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         othrstotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblOTHRS").text(othrstotal);

                 var candavtotal = 0;
                 $('.txt-candav').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         candavtotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblCanAdv").text(candavtotal);

                 var pentotal = 0;
                 $('.txt-pen').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         pentotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblPen").text(pentotal);


                 var unidedtotal = 0;
                 $('.txt-unided').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         unidedtotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblUNIDED").text(unidedtotal);


                 var atmdedtotal = 0;
                 $('.txt-atmded').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         atmdedtotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblATMDED").text(atmdedtotal);



                 var inctvstotal = 0;
                 $('.txt-candav').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         inctvstotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblInctvs").text(inctvstotal);


                 var Arrearstotal = 0;
                 $('.txt-Arrears').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         Arrearstotal += parseFloat($(this).val());
                     }
                 });
                 $("#lblArrears").text(Arrearstotal);


                 $(".tr-emp-att").each(function () {
                     var linetotal = 0;
                     $(this).find(".line-cal").each(function () {
                         if ($(this).val() != "" && $(this).val() != undefined) {
                             linetotal += parseFloat($(this).val());
                         }
                     });
                     $(this).find(".txt-linetotal").text(linetotal);
                 });


                 var ttls = 0;
                 $('.txt-linetotal').each(function () {
                     if ($(this).val() != "" && $(this).val() != undefined) {
                         ttls += parseFloat($(this).text());
                     }
                 });
                 $("#lblTTotals").text(ttls);

             }

             function CalculateLineTotals() {

             }

             function CalculateSummaryTotals() {
                 var nodtotal = 0;
                 $('.lbl-tnod').each(function () {
                     nodtotal += parseFloat($(this).text());
                 });
                 $("#lblTNOD").text(nodtotal);

                 var ottotal = 0;
                 $('.lbl-tot').each(function () {
                     ottotal += parseFloat($(this).text());
                 });
                 $("#lblTOT").text(ottotal);

                 var wototal = 0;
                 $('.lbl-two').each(function () {
                     wototal += parseFloat($(this).text());
                 });
                 $("#lblTWO").text(wototal);

                 var nhstotal = 0;
                 $('.lbl-tnhs').each(function () {
                     nhstotal += parseFloat($(this).text());
                 });
                 $("#lblTNHS").text(nhstotal);

                 var npoststotal = 0;
                 $('.lbl-tnpots').each(function () {
                     npoststotal += parseFloat($(this).text());
                 });
                 $("#lblTNPOTS").text(npoststotal);

                 var othrstotal = 0;
                 $('.lbl-tothrs').each(function () {
                     othrstotal += parseFloat($(this).text());
                 });
                 $("#lblTOTHRS").text(othrstotal);


                 var candavtotal = 0;
                 $('.lbl-tcadv').each(function () {
                     candavtotal += parseFloat($(this).text());
                 });
                 $("#lblTCADV").text(candavtotal);

                 var pentotal = 0;
                 $('.lbl-tpen').each(function () {
                     pentotal += parseFloat($(this).text());
                 });
                 $("#lblTPEN").text(pentotal);

                 var unidedtotal = 0;
                 $('.lbl-tunided').each(function () {
                     unidedtotal += parseFloat($(this).text());
                 });
                 $("#lblTUNIDED").text(unidedtotal);

                 var atmdedtotal = 0;
                 $('.lbl-tatmded').each(function () {
                     atmdedtotal += parseFloat($(this).text());
                 });
                 $("#lblTATMDED").text(atmdedtotal);


                 var inctvstotal = 0;
                 $('.lbl-tinctvs').each(function () {
                     inctvstotal += parseFloat($(this).text());
                 });
                 $("#lblTInctvs").text(inctvstotal);


                 var Arrearstotal = 0;
                 $('.lbl-tarrears').each(function () {
                     Arrearstotal += parseFloat($(this).text());
                 });
                 $("#lblTARREARS").text(Arrearstotal);




                 $(".tr-emp-summary").each(function () {
                     var linetotal = 0;
                     $(this).find(".lbl-tots").each(function () {
                         linetotal += parseFloat($(this).text());
                     });
                     $(this).find(".lbl-Totals").text(linetotal);
                 });
                 var ttls = 0;
                 $('.lbl-Totals').each(function () {
                     ttls += parseFloat($(this).text());
                 });
                 $("#lblTotals").text(ttls);
             }



             function SaveAttendance() {
                 debugger;
                 var datalst = [];
                 // openModal();
                 var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
            var month = "0";
            var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                   if (Chk == true) {
                       var date = $("#<%=txtmonth.ClientID %>").datepicker('getDate');
                     var year = date.getFullYear().toString();
                     var monthv = date.getMonth();
                     if (monthv == 11) {
                         monthv = 12;
                     }
                     else {
                         monthv = date.getMonth() + 1;
                     }
                     month = monthv + year.substr(2, 2);
                 }
                 else {
                     month = $("#<%=ddlMonth.ClientID %>").find(":selected").index().toString();
                 }
                 var ottype = parseInt($("#ddlOTtype").val());
                 if ($('#tblattendancegrid > tbody > tr').length != undefined && $('#tblattendancegrid > tbody > tr').length > 0) {
                     $('#tblattendancegrid > tbody > tr').each(function (i, row) {
                         var isnewrow = $(row).hasClass("new-row");
                         var EmpAttendance = {
                             ClientId: clientId,
                             MonthIndex: month,
                             Chkbox: Chk,
                             NewAdd: isnewrow,
                             EmpId: $(row).attr("data-emp-id"),
                             EmpDesg: $(row).attr("data-emp-desg"),
                             JoiningDate: (isnewrow) ? $(row).attr("data-emp-jdate") : "",
                             RelievingDate: (isnewrow) ? $(row).attr("data-emp-rdate") : "",
                             PF: (isnewrow) ? $(row).attr("data-emp-pf") : false,
                             PT: (isnewrow) ? $(row).attr("data-emp-pt") : false,
                             ESI: (isnewrow) ? $(row).attr("data-emp-esi") : false,
                             TransferType: (isnewrow) ? $(row).attr("data-emp-ttype") : 1,
                             NOD: parseFloat($(row).find(".txt-nod").val()),
                             OT: parseFloat($(row).find(".txt-ot").val()),
                             WO: parseFloat($(row).find(".txt-wo").val()),
                             NHS: parseFloat($(row).find(".txt-nhs").val()),
                             Nposts: parseFloat($(row).find(".txt-nposts").val()),
                             OTHRS: parseFloat($(row).find(".txt-othrs").val()),
                             CanAdv: parseFloat($(row).find(".txt-candav").val()),
                             Penality: parseFloat($(row).find(".txt-pen").val()),
                             UNIDED: parseFloat($(row).find(".txt-unided").val()),
                             ATMDED: parseFloat($(row).find(".txt-atmded").val()),
                             Incentives: parseFloat($(row).find(".txt-inctvs").val()),
                             Arrears: parseFloat($(row).find(".txt-Arrears").val()),
                             stoppayment: $(row).find("#chkstoppayment").is(":checked"),
                             OTtype: ottype
                         };
                         datalst.push(EmpAttendance);
                     });


                     var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                     var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/SaveAttendance";

                     if (clientId != undefined && clientId != "0" && clientId != "" && month != undefined && month != "0") {
                         if (datalst.length > 200) {
                             var lstdata = [];
                             var startindx = 0; var looplength = 200; var nxtlooplength = 200;
                             do {
                                 if (startindx > 0 && looplength < datalst.length) {
                                     nxtlooplength = datalst.length - looplength;
                                     looplength += nxtlooplength
                                 }
                                 lstdata = datalst.slice(startindx, looplength);
                                 var dataparam = JSON.stringify({ lst: lstdata });
                                 $.ajax({
                                     type: "POST",
                                     url: ajaxUrl,
                                     data: dataparam,
                                     async: false,
                                     contentType: "application/json; charset=utf-8",
                                     dataType: "json",
                                     success: function (json) {
                                         if (json != "") {
                                             if (json.msg == "success") {
                                                 console.log("startindx:" + startindx + " looplenth:" + looplength);
                                             }
                                             else {
                                                 console.log("startindx:" + startindx + " looplenth:" + looplength);
                                                 console.log(json.Obj);
                                             }
                                         }
                                     },
                                     error: function (json) { alert('fail'); }
                                 });
                                 startindx += looplength;

                             } while (startindx < datalst.length);

                             alert("Employees Attendance Saved.");
                             GetEmpAttendanceData();
                         }
                         else if (datalst.length > 0) {
                             var dataparam = JSON.stringify({ lst: datalst });
                             $.ajax({
                                 type: "POST",
                                 url: ajaxUrl,
                                 data: dataparam,
                                 async: false,
                                 contentType: "application/json; charset=utf-8",
                                 dataType: "json",
                                 success: function (json) {
                                     if (json != "") {
                                         if (json.msg == "success") {
                                             alert("Employees Attendance Saved.");
                                             GetEmpAttendanceData();
                                         }
                                         else {
                                             alert(json.Obj);
                                         }
                                     }
                                 },
                                 error: function (json) { alert('fail'); }
                             });
                         }
                     } else {
                         alert("Select ClientId and month.");
                     }
                 }
                 else {
                     alert("Enter Employee to Save Attendance.");
                 }
            //closeModal();

                 reorder();
             }


             function SaveTempAttendance() {
                 debugger;
                 var datalst = [];
                 //openModal();
                 var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
            var month = "0";
            var Chk = $("#<%=chkold.ClientID %>").is(":checked");
            if (Chk == true) {
                var date = $("#<%=txtmonth.ClientID %>").datepicker('getDate');
                     var year = date.getFullYear().toString();
                     var monthv = date.getMonth();
                     if (monthv == 11) {
                         monthv = 12;
                     }
                     else {
                         monthv = date.getMonth() + 1;
                     }
                     month = monthv + year.substr(2, 2);
                 }
                 else {
                     month = $("#<%=ddlMonth.ClientID %>").find(":selected").index().toString();
                 }
                 var ottype = parseInt($("#ddlOTtype").val());
                 var empid = $("#<%=txtEmpId.ClientID %>").val();
            var oldempid = $("#<%=txtoldEmpId.ClientID %>").val();
            var empname = $("#<%=txtEmpName.ClientID %>").val();
            var empdesgid = $("#<%=ddlEmpDesg.ClientID %>").val();
            var empdesgname = $('#<%= ddlEmpDesg.ClientID %> option:selected').text();
            var empttype = $('#<%= ddlTransfertype.ClientID %> option:selected').val();
            var jdate = $("#txtJoingingDate").val();
            var rdate = $("#txtRelievingDate").val();
            var esi = $("#chkESI").is(":checked");
            var pt = $("#chkPT").is(":checked");
            var pf = $("#chkPF").is(":checked");
            var nod = $("#txt-add-nod").val();
            var ot = $("#txt-add-ot").val();
            var wo = $("#txt-add-wo").val();
            var nhs = $("#txt-add-nhs").val();
            var npots = $("#txt-add-npots").val();
            var OTHRS = $("#txt-add-othrs").val();
            var canadv = $("#txt-add-canadv").val();
            var pen = $("#txt-add-pen").val();
            var UNIDED = $("#txt-add-unided").val();
            var ATMDED = $("#txt-add-atmded").val();
            var inctvs = $("#txt-add-inctvs").val();
            var Arrears = $("#txt-add-arrears").val();

            //var isnewrow = $(row).hasClass("new-row");
            var TempEmpAttendance = {
                ClientId: clientId,
                MonthIndex: month,
                EmpId: empid,
                EmpDesg: empdesgid,
                JoiningDate: jdate,
                RelievingDate: rdate,
                PF: pf,
                PT: pt,
                ESI: esi,
                TransferType: empttype,
                NOD: nod,
                OT: ot,
                WO: wo,
                NHS: nhs,
                Nposts: npots,
                OTHRS: OTHRS,
                CanAdv: canadv,
                Penality: pen,
                UNIDED: UNIDED,
                ATMDED: ATMDED,
                Incentives: inctvs,
                Arrears: Arrears,
                OTtype: ottype
            };
            datalst.push(TempEmpAttendance);

            debugger;
            //var ajaxUrl = window.location.href.substring(0, window.location.href.lastIndexOf('/')) + "/FameService.asmx/SaveAttendance";

            var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
            var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/SaveAttendance";

            if (clientId != undefined && clientId != "0" && clientId != "" && month != undefined && month != "0") {
                if (datalst.length > 200) {
                    var lstdata = [];
                    var startindx = 0; var looplength = 500; var nxtlooplength = 500;
                    do {
                        if (startindx > 0 && looplength < datalst.length) {
                            nxtlooplength = datalst.length - looplength;
                            looplength += nxtlooplength
                        }
                        lstdata = datalst.slice(startindx, looplength);
                        var dataparam = JSON.stringify({ lst: lstdata });
                        $.ajax({
                            type: "POST",
                            url: ajaxUrl,
                            data: dataparam,
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (json) {
                                if (json != "") {
                                    if (json.msg == "success") {
                                        console.log("startindx:" + startindx + " looplenth:" + looplength);
                                    }
                                    else {
                                        console.log("startindx:" + startindx + " looplenth:" + looplength);
                                        console.log(json.Obj);
                                    }
                                }
                            },
                            error: function (json) { alert('fail'); }
                        });
                        startindx += looplength;

                    } while (startindx < datalst.length);

                    alert("Employees Attendance Saved.");
                    GetEmpAttendanceData();
                }
                else if (datalst.length > 0) {
                    var dataparam = JSON.stringify({ lst: datalst });
                    $.ajax({
                        type: "POST",
                        url: ajaxUrl,
                        data: dataparam,
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (json) {
                            if (json != "") {
                                if (json.msg == "success") {
                                    alert("Employees Attendance Saved.");
                                    GetEmpAttendanceData();
                                }
                                else {
                                    alert(json.Obj);
                                }
                            }
                        },
                        error: function (json) { alert('fail'); }
                    });
                }
            } else {
                alert("Select ClientId and month.");
            }
            ClearEmpAddValues();
            //closeModal();
        }

        function openModal() {
            document.getElementById('modal').style.display = 'block';
            document.getElementById('fade').style.display = 'block';
        }

        function closeModal() {
            document.getElementById('modal').style.display = 'none';
            document.getElementById('fade').style.display = 'none';
        }

        function getFormattedDate(date) {
            var year = date.getFullYear();
            var month = (1 + date.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;
            var day = date.getDate().toString();
            day = day.length > 1 ? day : '0' + day;
            return day + '/' + month + '/' + year;
        }

        function AddDuplicatesTbl(data) {
            data = eval(data);
            $("#tblDuplicates >tbody").html("");
            $.each(data, function (i, item) {
                var strr = "<tr class='tr-emp-Duplicates'>" +
                     " <td><label class='lbl-tclientid lbl-thin'>##ClientID##</label></td>" +
                      " <td><label class='lbl-tOldEmpid lbl-thin'>##OldEmpid##</label></td>" +
                      " <td><label class='lbl-tEmpName lbl-thin'>##EmpName##</label></td>" +
                   " <td><label class='lbl-tdesg lbl-thin'>##Designation##</label></td>" +
                    "<td><label class='lbl-tnod lbl-thin lbl-tots'>##TNOD##</label></td>" +
                    "<td><label class='lbl-tot lbl-thin lbl-tots'>##TOT##</label></td>" +
                    "<td><label class='lbl-two lbl-thin lbl-tots'>##TWO##</label></td>" +
                    "<td><label class='lbl-tnhs lbl-thin lbl-tots'>##TNHS##</label></td>" +
                    "<td><label class='lbl-tnpots lbl-thin lbl-tots'>##TNPOTS##</label></td></tr>";
                var newrow = strr.replace("##ClientID##", item.ClientID)
                             .replace("##OldEmpid##", item.OldEmpid)
                             .replace("##EmpName##", item.EmpFName)
                            .replace("##Designation##", item.DesgName)
                            .replace('##TNOD##', item.NODTotal)
                            .replace('##TOT##', item.OTTotal)
                            .replace('##TWO##', item.WOTotal)
                            .replace('##TNHS##', item.NHSTotal)
                            .replace('##TNPOTS##', item.NpotsTotal)
                $("#tblDuplicates >tbody").append(newrow);
            });

        }

        function GetAttendanceDuplicates() {
            // openModal();
            debugger;
            var empid = $("#<%=txtEmpId.ClientID %>").val();
            var oldempid = $("#<%=txtoldEmpId.ClientID %>").val();

            var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
            var month = 0;
            var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                 if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                month = $("#<%=txtmonth.ClientID %>").val();
            }
            else {
                month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
            }
            var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
            var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetAttendanceDuplicates";

            if (oldempid != undefined && oldempid != "0" && oldempid != "" && month != undefined && month != "0") {
                $.ajax({
                    type: "POST",
                    url: ajaxUrl,
                    data: "{empid:'" + oldempid + "',month:'" + month + "',Chk:'" + Chk + "'}",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (json, b, c) {
                        if (json != "") {
                            //var res = JSON.parse(json.d);
                            if (json.msg == "success") {
                                AddDuplicatesTbl(json.Obj);
                                debugger;
                                $("#divDuplicates").show();
                            }
                            else if (json.msg == "nodata") {
                                $("#tblDuplicates >tbody").html("");
                                $("#divDuplicates").hide();
                            }
                        }
                    },
                    error: function (json) { alert('fail'); }
                });
            }
            // closeModal();
        }

    </script>

    <style type="text/css">
        .lbl-thin {
            font-weight: 100 !important;
        }

        /* Automatic Serial Number Row */
        .css-serial {
            counter-reset: serial-number; /* Set the serial number counter to 0 */
        }

            .css-serial td:first-child:before {
                counter-increment: serial-number; /* Increment the serial number counter */
                content: counter(serial-number); /* Display the counter */
            }

        #fade {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 2000px;
            background-color: #ababab;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .70;
            filter: alpha(opacity=80);
        }

        #modal {
            display: none;
            position: absolute;
            top: 45%;
            left: 45%;
            width: 100px;
            height: 100px;
            padding: 30px 15px 0px;
            border: 3px solid #ababab;
            box-shadow: 1px 1px 10px #ababab;
            border-radius: 20px;
            background-color: white;
            z-index: 1002;
            text-align: center;
            overflow: auto;
        }

        #results {
            font-size: 1.25em;
            color: red;
        }

        .ui-autocomplete {
            max-height: 200px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden;
        }
        /* IE 6 doesn't support max-height
   * we use height instead, but this forces the menu to always be this tall
   */ * html .ui-autocomplete {
            height: 200px;
        }

        .custom-combobox {
            position: relative;
            display: inline-block;
            width: 84%;
        }

        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
        }

        .custom-combobox-input {
            margin: 0;
            padding: 5px 10px;
            width: 100%;
        }

        .btnhgtwt {
            top: 0px;
            height: 31px;
        }

        .num-txt {
            padding: 0 5px;
            width: 40px;
        }
    </style>
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">Clients Dashboard</h1>
            <div class="row">
                <div class="row">
                    <div id="divClient" runat="server">
                        <asp:HiddenField ID="hdClientData" runat="server" />
                        <asp:HiddenField ID="HiddenField1" runat="server" />

                        <table class="table">
                            <tr>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Client Id</label>
                                </td>
                                <td>
                                    <%--<asp:DropDownList ID="ddlLabourSubCategory"  runat="server"></asp:DropDownList>--%>

                                    <select id="ddlClientID" data-placeholder="select" class="ddlautocomplete chosen-select" runat="server" name="clientid"
                                        style="width: 350px;">
                                    </select>
                                </td>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Client Name</label>
                                </td>
                                <td>
                                    <select id="ddlClientName" data-placeholder="select" class="ddlautocomplete chosen-select" runat="server"
                                        style="width: 350px;">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Phone N0(s)
                                    </label>
                                </td>
                                <td>
                                    <input id="txtphonenumbers" type="text" class="form-control" id="dd" style="width: 350px;">
                                </td>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Our Contact Person</label>
                                </td>
                                <td>
                                    <input id="txtocp" type="text" class="form-control" id="dd" style="width: 350px;">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Month</label>
                                </td>
                                <td>
                                    <select id="ddlMonth" runat="server" class="form-control" onchange="GetAttendance();"
                                        style="width: 350px;">
                                    </select>
                                    <input id="txtmonth" type="text" class="form-control txt-calender" runat="server" style="width: 350px; position: relative; bottom: 25px; visibility: hidden"
                                        onchange="GetEmpAttendanceData();" />
                                </td>
                                <td>
                                    <label for="exampleInputEmail1">
                                        OT in terms of</label>
                                </td>
                                <td>
                                    <select id="ddlOTtype" class="form-control" style="width: 350px;">
                                        <option value="0">Days</option>
                                        <option value="1">Hours</option>
                                    </select>
                                </td>
                            </tr>

                            <tr>

                                <td colspan="2">

                                    <label for="exampleInputEmail1">
                                        Old</label>
                                    <input type="checkbox" id="chkold" title="Old" runat="server" value="false" style="padding-bottom: 5px" onchange="chkchange();" />

                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divSummary" class="row" style="display: none;">
                    <table id="tblSummary" class="table table-striped table-bordered table-condensed table-hover">
                        <thead>
                            <tr class="warning">
                                <th>Designation
                                </th>
                                <th>Number of Duties
                                </th>
                                <th>Ot's
                                </th>
                                <th>WO's
                                </th>
                                <th>NHS's
                                </th>
                                <th>PL Days <%--npots changes to PL days--%>
                                </th>
                                <th>OT Hrs
                                </th>
                                <th>Totals
                                </th>
                                <th>Canteen Advance
                                </th>
                                <th>Advance
                                </th>
                                <th>Uniform Ded
                                </th>
                                <th>Other Ded <%--ATM Ded change to Others Ded--%>
                                </th>
                                <th>Reimbursement <%--Incen  change to Reimbursement--%>
                                </th>
                                <th>Arrears
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                        <tfoot class="active" runat="server" id="IDsummarytotal">
                            <tr>
                                <td>
                                    <label id="lblHead">
                                        Total :</label>
                                </td>
                                <td>
                                    <label id="lblTNOD">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTOT">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTWO">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTNHS">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTNPOTS">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTOTHRS">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTotals">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTCADV">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTPEN">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTUNIDED">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTATMDED">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTInctvs">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTARREARS">
                                    </label>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>


                <div id="divDuplicates" class="row" style="display: none;">
                    <table id="tblDuplicates" class="table table-striped table-bordered table-condensed table-hover">
                        <thead>
                            <tr class="warning">
                                <th>Client ID
                                </th>
                                <th>Old ID
                                </th>
                                <th>Emp Name
                                </th>
                                <th>Designation
                                </th>
                                <th>Number of Duties
                                </th>
                                <th>Ot's
                                </th>
                                <th>WO's
                                </th>
                                <th>NHS's
                                </th>
                                <th>PL Days <%--npots changes to PL days--%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>



                    </table>
                </div>

             


                <div id="divAttendanceGrid" class="row" runat="server">
                    <div>
                        <table id="tblattendancegrid" class="table table-striped table-bordered table-condensed table-hover" style="margin-left: -140px">
                            <thead>
                                <tr id="trAddData" data-emp-id='' data-emp-desg='' data-emp-name="" class="active" runat="server">

                                    <td>
                                        <input id="txtoldEmpId" class="form-control" runat="server" onchange="GetAttendanceDuplicates();" placeholder="Old Id" style="width: 95px;" />

                                    </td>
                                    <td colspan="2">
                                        <input id="txtEmpId" class="form-control" placeholder="Employee Id" runat="server" style="width: 100px;" />
                                        <label id="lblempstatus" visible="false" runat="server" />

                                    </td>
                                    <td>
                                        <input id="txtEmpName" class="form-control" placeholder="Employee Name" runat="server" style="width: 170px;" />
                                    </td>
                                    <td>
                                        <select id="ddlEmpDesg" runat="server" class="form-control emp-ddl" style="width: 150px;">
                                        </select>
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-nod" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-ot" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-wo" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-nhs" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-npots" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-othrs" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-canadv" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-pen" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-unided" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-atmded" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-inctvs" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-arrears" value="0" />
                                    </td>
                                    <td rowspan="2"></td>
                                    <td rowspan="2">
                                        <button class="btn btn-primary" onclick="AddNewEmp(this);return false;" style="height: 60px;">
                                            <i class="glyphicon glyphicon-plus"></i>
                                        </button>
                                    </td>
                                </tr>

                                <tr class="active">
                                    <td colspan="3">
                                        <input type="text" class="form-control txt-calender" id="txtJoingingDate" placeholder="JoiningDate" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control txt-calender" id="txtRelievingDate" placeholder="RevlievingDate" />
                                    </td>
                                    <td>
                                        <select id="ddlTransfertype" class="form-control" style="width: 150px;" runat="server">
                                            <option value="1">PostingOrder</option>
                                            <option value="0" selected="selected">Temporary Transfer</option>
                                            <option value="-1">Dumy Transfer</option>
                                        </select>
                                    </td>
                                    <td>
                                        <input type="checkbox" id="chkESI" checked="checked" />
                                        &nbsp; ESI
                                    </td>
                                    <td>
                                        <input type="checkbox" id="chkPF" checked="checked" />
                                        &nbsp; PF
                                    </td>
                                    <td>
                                        <input type="checkbox" id="chkPT" checked="checked" />
                                        &nbsp; PT
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>


                                </tr>

                                <tr class="warning">
                                    <th>S.No</th>
                                    <th></th>
                                    <th>Emp Id
                                    </th>
                                    <th>Emp Name
                                    </th>
                                    <th>Emp Designation
                                    </th>
                                    <th>NOD
                                    </th>
                                    <th>OT's
                                    </th>
                                    <th>WO's
                                    </th>
                                    <th>NHS
                                    </th>
                                    <th>PL Days
                                    </th>
                                    <th>OT Hrs
                                    </th>
                                    <th>C.Adv
                                    </th>
                                    <th>Advance
                                    </th>
                                    <th>Uni.Ded
                                    </th>
                                    <th>Other Ded <%--ATM Ded change to Others Ded--%>
                                    </th>
                                    <th>Reimb <%--Incen  change to Reimbursement--%>
                                    </th>
                                    <th>Arrears
                                    </th>

                                    <th>Totals
                                    </th>
                                    <th></th>

                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot class="active">
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>


                                    <th>
                                        <label>
                                            Totals :</label>
                                    </th>
                                    <th>
                                        <label id="lblNOD">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblOT">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblWO">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblNHS">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblNpots">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblOTHRS">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblCanAdv">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblPen">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblUNIDED">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblATMDED">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblInctvs">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblArrears">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblTTotals">
                                        </label>
                                    </th>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                    <div class="row">
                        <div class="col-md-10">
                        </div>
                        <div class="col-md-1">
                            <button type='button' id="btnSave" class="btn btn-success" onclick="SaveAttendance();return false;">
                                Save</button>
                        </div>
                        <div class="col-md-1">
                            <button type='button' id="btnCancel" class="btn btn-default" onclick="Cancel();return false;">
                                Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
