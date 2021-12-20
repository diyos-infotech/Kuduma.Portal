<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MainMaster.master" CodeBehind="Reminders.aspx.cs" Inherits="Kuduma.Portal.Reminders" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/reminders.css" rel="stylesheet" type="text/css" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js" type="text/javascript"></script>
    <script src="css/js/jquery.bpopup-x.x.x.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        ; (function($) {


            $(function() {


                $('#my-button').bind('click', function(e) {
                    e.preventDefault();
                    $('#element_to_pop_up').bPopup();
                });


            });

            $(function() {


                $('#my-button1').bind('click', function(f) {
                    f.preventDefault();
                    $('#element_to_pop_up1').bPopup();
                });


            });

            $(function() {


                $('#my-button2').bind('click', function(f) {
                    f.preventDefault();
                    $('#element_to_pop_up2').bPopup();
                });


            });

            $(function() {


                $('#my-button3').bind('click', function(f) {
                    f.preventDefault();
                    $('#element_to_pop_up3').bPopup();
                });


            });

            $(function() {


                $('#my-button4').bind('click', function(f) {
                    f.preventDefault();
                    $('#element_to_pop_up4').bPopup();
                });


            });

            $(function() {


                $('#my-button5').bind('click', function(f) {
                    f.preventDefault();
                    $('#element_to_pop_up5').bPopup();
                });


            });

            $(function() {


                $('#my-button6').bind('click', function(f) {
                    f.preventDefault();
                    $('#element_to_pop_up6').bPopup();
                });


            });

            $(function() {


                $('#my-button7').bind('click', function(f) {
                    f.preventDefault();
                    $('#element_to_pop_up7').bPopup();
                });


            });

        })(jQuery);
    </script>

    <script src="css/js/jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="css/js/jquery.easing.min.js"></script>

    <script type="text/javascript" src="css/js/jquery.easy-ticker.js"></script>

    <script type="text/javascript">



        $(document).ready(function() {

            var dd = $('.vticker1').easyTicker({
                direction: 'up',
                easing: 'swing',
                speed: 'slow',
                interval: 2500,
                height: 'auto',
                visible: 3,
                mousePause: 1,
                controls: {
                    up: '.up',
                    down: '.down',
                    toggle: '.toggle',
                    stopText: 'Stop !!!'
                }
            }).data('easyTicker');
        });
    </script>


    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder" style="background: #ffffff;">
        <div class="content-holder">
            <h1>
                Reminders</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="row">
                    <!-- Birthdays Section -->
                    <article class="col-sm-4">
        <div class="data-block turquoise">
          <header>
            <h2>Birthdays</h2>
           
          </header>
          <section>
           <asp:Label ID="lblheadingone" runat="server"
            Text="DIYOS Technologies wishes " style="font-weight:bold;line-height:2;height:200px"></asp:Label>
            <br />
      
		
		<asp:ListView ID="GVBirthday" runat="server"  >
   <LayoutTemplate  >
   
   <div class="vticker1">
	<ul>
	<asp:PlaceHolder runat="server" ID="itemPlaceholder"  />
	</ul>
	</div>
   </LayoutTemplate>
   <ItemTemplate >
     <li>
      <div style="float:left;padding-right:8px">
      <img src="css/assets/birthday.png" width="20" height="20" />
      </div>
			<div>
            <%#(Eval("Name").ToString())%>
            </div>
           
            </li> 
            
   </ItemTemplate> 
   </asp:ListView> 
   
   
    </section>
    
    <div id="viewl"><a href="#" id="my-button">View More</a></div> 
   </div> 
   </article>
                    <div id="element_to_pop_up">
                        <asp:ListView ID="GVBirthday_Viewmore" runat="server">
                            <LayoutTemplate>
                                <div class="viewmore">
                                    <h2>
                                        Birthdays <a href="#" class="bClose" style="float: right">
                                            <img src="css/assets/close-button.png" width="30" height="30"></a></h2>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <div style="float: left; padding-right: 8px">
                                        <img src="css/assets/birthday.png" width="20" height="20" />
                                    </div>
                                    <div>
                                        <%#(Eval("Name").ToString())%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <!-- Birthdays Section End -->
                    <!-- Contract Renewals -->
                    <article class="col-sm-4">
        <div class="data-block turquoise">
          <header>
            <h2>Contract Renewals</h2>
          
          </header>
          <section>
         
          	
		<asp:ListView ID="GVContract" runat="server">
   <LayoutTemplate>
   <div class="vticker1">
	<ul>
	<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</ul>
	</div>
   </LayoutTemplate>
   <ItemTemplate>
     <li>
     <div style="float:left;padding-right:8px;height: 50px;"><img src="css/assets/contract.png" width="20" height="20" /></div>
			<div>
			<%#(Eval("ElapsedTime").ToString())%>
            </div>
        </li>
        
   </ItemTemplate>
</asp:ListView>

		<br />
         
          
           </section>
           <div id="viewl"><a href="#" id="my-button1">View More</a></div>
        </div>
      </article>
                    <div id="element_to_pop_up1">
                        <asp:ListView ID="GVContract_Viewmore" runat="server">
                            <LayoutTemplate>
                                <div class="viewmore">
                                    <h2>
                                        Contract Renewals <a href="#" class="bClose" style="float: right">
                                            <img src="css/assets/close-button.png" width="30" height="30"></a></h2>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <div style="float: left; padding-right: 8px">
                                        <img src="css/assets/contract.png" width="20" height="20" />
                                    </div>
                                    <div>
                                        <%#(Eval("ElapsedTime").ToString())%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <!-- Contract Renewals End-->
                    <!-- License Renewals Renewals -->
                    <article class="col-sm-4">
        <div class="data-block red">
          <header>
            <h2><span class="elusive icon-fire"></span> License Renewals</h2>
          
          </header>
          <section>
            
            
            
            <asp:ListView ID="ListView1" runat="server">
   <LayoutTemplate>
   <div class="vticker">
	<ul>
	<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</ul>
	</div>
   </LayoutTemplate>
   <ItemTemplate>
     <li>
     <div style="float:left;padding-right:8px"><img src="css/assets/renewal.png" width="20" height="20" /></div>
			<div>
			<%#(Eval("").ToString())%>
            </div>
        </li>
        
   </ItemTemplate>
</asp:ListView>


          </section>
          <div id="viewl"><a href="#" id="A1">View More</a></div>
        </div>
      </article>
                    <div id="Div1">
                        <asp:ListView ID="ListView3" runat="server">
                            <LayoutTemplate>
                                <div class="viewmore">
                                    <h2>
                                        License Renewals</h2>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <div style="float: left; padding-right: 8px">
                                        <img src="css/assets/renewal.png" width="20" height="20" />
                                    </div>
                                    <div>
                                        <%#(Eval("ElapsedTime").ToString())%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div class="row">
                    <!-- License Renewals Section End -->
                    <!-- Latest Bills Generated Section -->
                    <article class="col-sm-4">
        <div class="data-block turquoise">
          <header>
            <h2>Latest Bills Generated</h2>
           
          </header>
          <section>
          
          
           <asp:ListView ID="gvLatestbills" runat="server">
   <LayoutTemplate>
   <div class="vticker">
	<ul style="overflow:hidden;height:170px">
	<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</ul>
	</div>
   </LayoutTemplate>
   <ItemTemplate>
     <li>
     <div style="float:left;padding-right:8px;height:70px">
     <img src="css/assets/billing.png" width="20" height="20" /></div>
			<div>
			<%#(Eval("BillStaus").ToString())%>
            </div>
        </li>
        
   </ItemTemplate>
</asp:ListView>

          </section>
          <div id="viewl"><a href="#" id="my-button2">View More</a></div>
        </div>
      </article>
                    <div id="element_to_pop_up2">
                        <asp:ListView ID="gvLatestbills_Viewmore" runat="server">
                            <LayoutTemplate>
                                <div class="viewmore">
                                    <h2>
                                        Latest Bills Generated <a href="#" class="bClose" style="float: right">
                                            <img src="css/assets/close-button.png" width="30" height="30"></a></h2>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <div style="float: left; padding-right: 8px">
                                        <img src="css/assets/billing.png" width="20" height="20" />
                                    </div>
                                    <div>
                                        <%#(Eval("BillStaus").ToString())%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <!-- Latest Bills Generated Section End -->
                    <!-- Latest Paysheets Generated Section -->
                    <article class="col-sm-4">
        <div class="data-block turquoise">
          <header>
            <h2>Latest Paysheets Generated</h2>
          
          </header>
          <section>
          
           <asp:ListView ID="gvlatesPaysheet" runat="server">
   <LayoutTemplate>
   <div class="vticker">
	<ul style="overflow:hidden;height:170px">
	<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</ul>
	</div>
   </LayoutTemplate>
   <ItemTemplate>
     <li>
     <div style="float:left;padding-right:8px;height:70px"><img src="css/assets/paysheet.png" width="20" height="20" /></div>
			<div>
			<%#(Eval("Paysheetstatus").ToString())%>
            </div>
        </li>
        
   </ItemTemplate>
   
</asp:ListView><br />
 
          </section>
         <div id="viewl"><a href="#" id="my-button3">View More</a></div>
        </div>
      </article>
                    <div id="element_to_pop_up3">
                        <asp:ListView ID="gvlatesPaysheet_Viewmore" runat="server">
                            <LayoutTemplate>
                                <div class="viewmore">
                                    <h2>
                                        Latest Paysheets Generated <a href="#" class="bClose" style="float: right">
                                            <img src="css/assets/close-button.png" width="30" height="30"></a></h2>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <div style="float: left; padding-right: 8px">
                                        <img src="css/assets/paysheet.png" width="20" height="20" />
                                    </div>
                                    <div>
                                        <%#(Eval("Paysheetstatus").ToString())%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <!-- Latest Paysheets Generated Section End-->
                    <!-- Latest Receipts Section -->
                    <article class="col-sm-4">
        <div class="data-block red">
          <header>
            <h2><span class="elusive icon-fire"></span> Latest Receipts</h2>
          
          </header>
          <section>
          
            <asp:ListView ID="gvReciepts" runat="server">
   <LayoutTemplate>
   <div class="vticker">
	<ul>
	<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</ul>
	</div>
   </LayoutTemplate>
   <ItemTemplate>
     <li>
     <div style="float:left;padding-right:8px;height:40px"><img src="css/assets/receipt.gif" width="20" height="20" /></div>
			<div>
			<%#(Eval("Reciepts").ToString())%>
            </div>
        </li>
        
   </ItemTemplate>
</asp:ListView>

          </section>
         <div id="viewl"><a href="#" id="my-button4">View More</a></div>
        </div>
      </article>
                    <div id="element_to_pop_up4">
                        <asp:ListView ID="gvReciepts_Viewmore" runat="server">
                            <LayoutTemplate>
                                <div class="viewmore">
                                    <h2>
                                        Latest Paysheets Generated <a href="#" class="bClose" style="float: right">
                                            <img src="css/assets/close-button.png" width="30" height="30"></a></h2>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <div style="float: left; padding-right: 8px">
                                        <img src="css/assets/receipt.gif" width="20" height="20" />
                                    </div>
                                    <div>
                                        <%#(Eval("Reciepts").ToString())%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <!-- Latest Receipts Section End-->
                </div>
                <div class="row">
                    <!-- Billing Section -->
                    <article class="col-sm-4">
        <div class="data-block turquoise">
          <header>
            <h2> Billing</h2>
           
          </header>
          <section>
            <asp:Label ID="Label9" runat="server"
            Text="" style="font-weight:bold;line-height:2;height:200px"></asp:Label>
          
          
          
          
           <asp:ListView ID="gvBills" runat="server">
   <LayoutTemplate>
   <div class="vticker">
	<ul>
	<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</ul>
	</div>
   </LayoutTemplate>
   <ItemTemplate>
    <li>
     <div style="float:left;padding-right:8px;height:40px"><img src="css/assets/billing.png" width="20" height="20" /></div>
			<div>
			<%#(Eval("BillingDeatils2").ToString())%>
            </div>
        </li>
        
   </ItemTemplate>
</asp:ListView>

          </section>
         <div id="viewl"><a href="#" id="my-button5">View More</a></div>
        </div>
      </article>
                    <div id="element_to_pop_up5">
                        <asp:ListView ID="gvBills_Viewmore" runat="server">
                            <LayoutTemplate>
                                <div class="viewmore">
                                    <h2>
                                        Billing <a href="#" class="bClose" style="float: right">
                                            <img src="css/assets/close-button.png" width="30" height="30"></a></h2>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <div style="float: left; padding-right: 8px">
                                        <img src="css/assets/billing.png" width="20" height="20" />
                                    </div>
                                    <div>
                                        <%#(Eval("BillingDeatils2").ToString())%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <!-- Billing Section End-->
                    <!-- Paysheets Section -->
                    <article class="col-sm-4">
        <div class="data-block turquoise">
          <header>
            <h2>Paysheets</h2>
          
          </header>
          <section>
           
           <asp:ListView ID="gvPaysheets" runat="server">
   <LayoutTemplate>
    <div class="vticker">
	<ul>
	<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</ul>
	</div>
   </LayoutTemplate>
   <ItemTemplate>
    <li>
     <div style="float:left;padding-right:8px;height:40px"><img src="css/assets/paysheet.png" width="20" height="20" /></div>
			<div>
			<%#(Eval("EmppaysheetAmount").ToString())%>
            </div>
        </li>
        
   </ItemTemplate>
</asp:ListView>


 
          </section>
         <div id="viewl"><a href="#" id="my-button6">View More</a></div>
        </div>
      </article>
                    <div id="element_to_pop_up6">
                        <asp:ListView ID="gvPaysheets_Viewmore" runat="server">
                            <LayoutTemplate>
                                <div class="viewmore">
                                    <h2>
                                        Paysheets <a href="#" class="bClose" style="float: right">
                                            <img src="css/assets/close-button.png" width="30" height="30"></a></h2>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <div style="float: left; padding-right: 8px">
                                        <img src="css/assets/paysheet.png" width="20" height="20" />
                                    </div>
                                    <div>
                                        <%#(Eval("EmppaysheetAmount").ToString())%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <!-- Paysheets Section End -->
                    <!-- Receipts Section -->
                    <article class="col-sm-4">
        <div class="data-block red">
          <header>
            <h2><span class="elusive icon-fire"></span> Receipts</h2>
          
          </header>
          <section>
            <asp:ListView ID="ListView2" runat="server">
   <LayoutTemplate>
    <div class="vticker">
	<ul>
	<asp:PlaceHolder runat="server" ID="itemPlaceholder" />
	</ul>
	</div>
   </LayoutTemplate>
   <ItemTemplate>
    <li>
     <div style="float:left;padding-right:8px;height:40px"><img src="css/assets/receipt.gif" width="20" height="20" /></div>
			<div>
			<%#(Eval("").ToString())%>
            </div>
        </li>
        
   </ItemTemplate>
</asp:ListView>


 
          </section>
         <div id="viewl"><a href="#" id="my-button7">View More</a></div>
        </div>
      </article>
                    <div id="element_to_pop_up7">
                        <asp:ListView ID="ListView4" runat="server">
                            <LayoutTemplate>
                                <div class="viewmore">
                                    <h2>
                                        Paysheets <a href="#" class="bClose" style="float: right">
                                            <img src="css/assets/close-button.png" width="30" height="30"></a></h2>
                                    <ul>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                    </ul>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <div style="float: left; padding-right: 8px">
                                        <img src="css/assets/receipt.gif" width="20" height="20" />
                                    </div>
                                    <div>
                                        <%#(Eval("EmppaysheetAmount").ToString())%>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <!-- Receipts Section End-->
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
        
        <!-- CONTENT AREA END -->
    </div>
    </asp:Content>