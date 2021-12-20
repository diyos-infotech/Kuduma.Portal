using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.IO;
using System.Data.OleDb;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class AddClient : System.Web.UI.Page
    {

        AppConfiguration Config = new AppConfiguration();
        DataTable dt;

        string CmpIDPrefix = "";
        string BranchID = "";
        string UserName = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
                BranchID = Session["BranchID"].ToString();
                UserName = Session["UserId"].ToString();
                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                    clientid();
                    LoadSegments();
                    LoadDesignations();
                    LoadClients();
                    LoadopmEmpsIDs();
                    LoadDivisions();
                    LoadStatenames();
                    LoadOurGSTNos();
                    LoadShipStatenames();
                    LoadBranches();
                    LoadEsibranches();
                    LoadPFbranches();
                    //fillfieldofficer();
                    LoadUnitIncharges();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void LoadBranches()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtBranches = GlobalData.Instance.LoadLoginBranch(dtBranch);
            if (DtBranches.Rows.Count > 0)
            {
                ddlBranch.DataValueField = "BranchId";
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataSource = DtBranches;
                ddlBranch.DataBind();
            }
            ddlBranch.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void LoadEsibranches()
        {
            DataTable dtEsibranches = GlobalData.Instance.LoadEsibranches();
            if (dtEsibranches.Rows.Count > 0)
            {
                ddlEsibranch.DataValueField = "EsiBranchid";
                ddlEsibranch.DataTextField = "ESIBranchNo";
                ddlEsibranch.DataSource = dtEsibranches;
                ddlEsibranch.DataBind();
            }
            ddlEsibranch.Items.Insert(0, "-Select-");
        }

        protected void LoadPFbranches()
        {
            DataTable dtPFbranches = GlobalData.Instance.LoadPFbranches();
            if (dtPFbranches.Rows.Count > 0)
            {
                ddlPFbranch.DataValueField = "PFBranchid";
                ddlPFbranch.DataTextField = "PFBranchNo";
                ddlPFbranch.DataSource = dtPFbranches;
                ddlPFbranch.DataBind();
            }
            ddlPFbranch.Items.Insert(0, "-Select-");
        }
        protected void LoadShipStatenames()
        {

            DataTable DtStateNames = GlobalData.Instance.LoadStateNames();
            if (DtStateNames.Rows.Count > 0)
            {
                ddlShipToSate.DataValueField = "StateID";
                ddlShipToSate.DataTextField = "State";
                ddlShipToSate.DataSource = DtStateNames;
                ddlShipToSate.DataBind();


                ddlShipToStateCode.DataValueField = "StateID";
                ddlShipToStateCode.DataTextField = "GSTStateCode";
                ddlShipToStateCode.DataSource = DtStateNames;
                ddlShipToStateCode.DataBind();
            }
            ddlShipToSate.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlShipToStateCode.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        private void LoadOurGSTNos()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtGSTNos = GlobalData.Instance.LoadGSTNumbers(dtBranch);
            if (DtGSTNos.Rows.Count > 0)
            {
                ddlOurGSTIN.DataValueField = "Id";
                ddlOurGSTIN.DataTextField = "GSTNo";
                ddlOurGSTIN.DataSource = DtGSTNos;
                ddlOurGSTIN.DataBind();
            }
        }

        protected void LoadDivisions()
        {

            DataTable DtDivision = GlobalData.Instance.LoadDivision();
            if (DtDivision.Rows.Count > 0)
            {
                ddlZones.DataValueField = "DivisionId";
                ddlZones.DataTextField = "DivisionName";
                ddlZones.DataSource = DtDivision;
                ddlZones.DataBind();
                // ddlDivision.SelectedValue = "1";

            }
            ddlZones.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        protected void LoadStatenames()
        {

            DataTable DtStateNames = GlobalData.Instance.LoadStateNames();
            if (DtStateNames.Rows.Count > 0)
            {
                ddlstate.DataValueField = "StateID";
                ddlstate.DataTextField = "State";
                ddlstate.DataSource = DtStateNames;
                ddlstate.DataBind();

                ddlPTState.DataValueField = "StateID";
                ddlPTState.DataTextField = "State";
                ddlPTState.DataSource = DtStateNames;
                ddlPTState.DataBind();


                ddlStateCode.DataValueField = "StateID";
                ddlStateCode.DataTextField = "GSTStateCode";
                ddlStateCode.DataSource = DtStateNames;
                ddlStateCode.DataBind();

                ddlLWFState.DataValueField = "StateID";
                ddlLWFState.DataTextField = "State";
                ddlLWFState.DataSource = DtStateNames;
                ddlLWFState.DataBind();
            }

            ddlstate.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlPTState.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlStateCode.Items.Insert(0, new ListItem("-Select-", "0"));
            ddlLWFState.Items.Insert(0, new ListItem("-Select-", "0"));

        }

        protected void LoadClients()
        {
            DataTable dtBranch = GlobalData.Instance.LoadBranchOnUserID(BranchID);
            DataTable DtCnames = GlobalData.Instance.LoadCNames(CmpIDPrefix, dtBranch);
            if (DtCnames.Rows.Count > 0)
            {
                ddlUnits.DataValueField = "clientid";
                ddlUnits.DataTextField = "clientname";
                ddlUnits.DataSource = DtCnames;
                ddlUnits.DataBind();
            }
            ddlUnits.Items.Insert(0, "-Select-");
        }

        protected void LoadopmEmpsIDs()
        {
            DataTable DtopmEmpsIDs = GlobalData.Instance.LoadOpManagerIds();
            if (DtopmEmpsIDs.Rows.Count > 0)
            {
                ddlEmpId.DataValueField = "EmpId";
                ddlEmpId.DataTextField = "EmpId";
                ddlEmpId.DataSource = DtopmEmpsIDs;
                ddlEmpId.DataBind();
            }
            ddlEmpId.Items.Insert(0, "-Select-");
        }

        protected void LoadDesignations()
        {
            DataTable DtDesignations = GlobalData.Instance.LoadDesigns();
            if (DtDesignations.Rows.Count > 0)
            {
                ddldesgn.DataValueField = "Designid";
                ddldesgn.DataTextField = "Design";
                ddldesgn.DataSource = DtDesignations;
                ddldesgn.DataBind();
            }
            ddldesgn.Items.Insert(0, "-Select-");
        }

        protected void LoadSegments()
        {
            DataTable DtSegments = GlobalData.Instance.LoadSegments();
            if (DtSegments.Rows.Count > 0)
            {
                ddlsegment.DataValueField = "segid";
                ddlsegment.DataTextField = "segname";
                ddlsegment.DataSource = DtSegments;
                ddlsegment.DataBind();
            }
            ddlsegment.Items.Insert(0, "-Select-");
        }

        private void clientid()
        {
            txtCId.Text = GlobalData.Instance.LoadMaxClientid(CmpIDPrefix);
        }

        protected void LoadUnitIncharges()
        {
            string query = "Select ISNULL(EmpId,'') as EmpId from EmpDetails where EmployeeType='S' and Empstatus=1";
            DataTable dt = Config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                ddlUnitIncharge.DataTextField = "EmpId";
                ddlUnitIncharge.DataValueField = "EmpId";
                ddlUnitIncharge.DataSource = dt;
                ddlUnitIncharge.DataBind();
                ddlUnitIncharge.Items.Insert(0, "--select--");
            }
        }

        protected void btnaddclint_Click(object sender, EventArgs e)
        {
            try
            {

                #region  Begin  Check Validations as on  [19-09-2013]

                #region     Begin Check Client Name is  Empty or ?
                if (txtCname.Text.Trim().Length == 0)
                {
                    lblMsg.Text = "Please Enter The Client Name";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Enter The Client Name ');", true);
                    return;
                }
                #endregion  End Check Client Name is  Empty or ?

                #region   Begin Check   Contact Person   Name
                //if (txtcontactperson.Text.Trim().Length == 0)
                //{
                //    lblMsg.Text = "Please fill Contact Person name";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please fill Contact Person name ');", true);
                //    return;
                //}
                #endregion  Begin Check   Contact Person   Name

                #region  Begin Check Designation Selected or ?
                //if (ddldesgn.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select Designation";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Designation ');", true);
                //    return;
                //}
                #endregion End Check Designation Selected or ?

                #region  Begin Check Zone Selected or ?
                //if (ddlZones.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select Zone";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Designation ');", true);
                //    return;
                //}
                #endregion End Check Zone Selected or ?

                #region Begin Check Phone Number Entered or ?
                //if (txtphonenumbers.Text.Trim().Length == 0)
                //{
                //    lblMsg.Text = "Please Enter the Phone No.";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Enter the Phone No.');", true);
                //    return;
                //}
                //if (txtphonenumbers.Text.Trim().Length < 8)
                //{
                //    lblMsg.Text = "Please enter a valid Phone No.";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please enter a valid Phone No.');", true);
                //    return;
                //}

                if (ddlBranch.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select the Branch";
                    return;
                }

                //if (ddlPTState.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select the PT State";
                //    return;
                //}

                //if (ddlLWFState.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select the LWF State";
                //    return;
                //}

                //if (ddlPFbranch.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select the PF Branch";
                //    return;
                //}

                //if (ddlEsibranch.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select the ESI Branch";
                //    return;
                //}

                #endregion  End Check Phone Number Entered or ?

                #region  Begin Check if Sub unit Check then Should be Select MAin unit ID
                if (chkSubUnit.Checked)
                {
                    if (ddlUnits.SelectedIndex == 0)
                    {
                        lblMsg.Text = "Please select Main unit Id";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please select Main unit Id');", true);
                        return;
                    }

                    if (radioyesmu.Checked == true)
                    {
                        lblMsg.Text = "Please Select the Main Unit Mode NO";
                        return;
                    }
                }
                #endregion End Check if Sub unit Check then Should be Select MAin unit ID


                #region  Begin Check Invoice  Selected or ?
                if (radioinvoiceyes.Checked == false && radioinvoiceno.Checked == false)
                {
                    lblMsg.Text = "Please Select the Invoice Mode";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select the Invoice Mode ');", true);
                    return;
                }
                #endregion End Check Invoice Selected or ?

                #region Begin Check Paysheet Selected  or ?
                if (radiopaysheetyes.Checked == false && radiopaysheetno.Checked == false)
                {
                    lblMsg.Text = "Please Select the  Paysheet Mode";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select the  Paysheet Mode');", true);
                    return;
                }

                #endregion  End Check Paysheet Selected  Entered or ?

                #region Begin Check Main Unit  Selected  or ?
                if (radioyesmu.Checked == false && radionomu.Checked == false)
                {
                    lblMsg.Text = "Please Select the  Client Is Main Unit (YES/NO)";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select the  Client Is Main Unit (YES/NO)');", true);
                    return;
                }
                #endregion  End Check  Main Unit Selected  Entered or ?

                #region Begin Check Field Officer
                //if(dllfieldofficer.SelectedIndex==0)
                //{
                //    lblMsg.Text = "Please Select the  Field Officer";
                //    //ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select the  Field Officer');", true);
                //    return;
                //}
                #endregion End Check Field Officer


                #endregion End   Check Validations as on  [19-09-2013]

                #region     Begin Declare Variables as on [19-09-2013]
                #region     Begin Code Client-id to  Contact Person
                var ClientId = string.Empty;
                var ClientName = string.Empty;
                var ClientShortName = string.Empty;
                var ClientSegment = string.Empty;
                var ClientContactPerson = string.Empty;
                #endregion  End Code Client-id to Contact Person

                #region  Begin Code  Person-Designation To PIN-No
                var ClientPersonDesgn = string.Empty;
                var ClientPhonenumbers = string.Empty;
                var ClientFax = string.Empty;
                var ClientEmail = string.Empty;
                var ClientAddrPin = string.Empty;
                var EmailCC = string.Empty;

                #endregion  End Code  Person-Designation To PIN-No

                #region  Begin Code  Line-One To Line-Five
                var ClientAddrHno = string.Empty;
                var ClientAddrStreet = string.Empty;
                var ClientAddrArea = string.Empty;
                var ClientAddrCity = string.Empty;
                var ClientAddrColony = string.Empty;
                var Line7 = "";
                var Line8 = "";

                #endregion  End Code Line-One To Line-Five

                #region Begin Code Line Six To PaySheet
                var ClientAddrState = string.Empty;
                var SubUnitStatus = string.Empty;
                var MainUnitId = "0";
                var MAinunitStatus = 0;
                var Invoice = 0;
                var Paysheet = 0;
                var ClientDesc = string.Empty;
                #endregion End Code Line Six To PaySheet



                var Zone = "0";
                int Category = 0;
                var State = "0";
                var StateCode = "0";
                var GSTIN = "";
                var OurGSTIN = "";

                var ShiptoLine1 = "";
                var ShiptoLine2 = "";
                var ShiptoLine3 = "";
                var ShiptoLine4 = "";
                var ShiptoLine5 = "";
                var ShiptoLine6 = "";
                var ShipToState = "0";
                var ShipToStateCode = "0";
                var ShipToGSTIN = "";
                var Locationid = "0";
                var Fieldofficer = "0";
                var Areamanager = "0";
                var ClientType = 0;


                #region   Begin Extra Varibles for This Event   As on [20-09-2013]
                var IRecordStatus = 0;
                #endregion End Extra Varibles for This Event   As on [20-09-2013]

                #endregion  End Declare Variables as on [19-09-2013]

                #region    Begin Code For Assign Values Into Declared Variables as on [19-09-2013]
                #region    Begin Code Client-id to  Contact Person
                ClientId = txtCId.Text;
                ClientName = txtCname.Text;
                ClientShortName = txtshortname.Text;
                if (ddlsegment.SelectedIndex == 0)
                {
                    ClientSegment = "0";
                }
                else
                {
                    ClientSegment = ddlsegment.SelectedValue;
                }

                ClientContactPerson = txtcontactperson.Text;
                #endregion  End Code Client-id to Contact Person

                #region  Begin Code  Person-Designation To PIN-No
                ClientPersonDesgn = ddldesgn.SelectedValue;
                ClientPhonenumbers = txtphonenumbers.Text;
                ClientFax = txtfaxno.Text;
                ClientEmail = txtemailid.Text;
                ClientAddrPin = txtpin.Text;
                EmailCC = txtEmailCC.Text;

                #endregion  End Code  Person-Designation To PIN-No

                #region  Begin Code  Line-One To Line-Five
                ClientAddrHno = txtchno.Text;
                ClientAddrStreet = txtstreet.Text;
                ClientAddrArea = txtarea.Text;
                ClientAddrCity = txtcity.Text;
                ClientAddrColony = txtcolony.Text;

                #endregion  End Code Line-One To Line-Five

                #region Begin Code Line Six To PaySheet
                ClientAddrState = txtstate.Text;
                if (chkSubUnit.Checked)
                {
                    SubUnitStatus = "1";
                    MainUnitId = ddlUnits.SelectedValue;
                }

                if (radioyesmu.Checked)
                {
                    MAinunitStatus = 1;
                }
                if (radioinvoiceyes.Checked)
                {
                    Invoice = 1;
                }
                if (radiopaysheetyes.Checked)
                {
                    Paysheet = 1;
                }
                ClientDesc = txtdescription.Text;
                #endregion End Code Line Six To PaySheet


                if (ddlZones.SelectedIndex > 0)
                {
                    Zone = ddlZones.SelectedValue;

                }

                //if (ddlCategory.SelectedIndex == 0)
                //{
                //    lblMsg.Text = "Please Select Category";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' Please Select Category');", true);
                //    return;
                //}

                //if (ddlCategory.SelectedIndex > 0)
                //{
                //    Category = ddlCategory.SelectedIndex;

                //}

                if (ddlstate.SelectedIndex > 0)
                {
                    State = ddlstate.SelectedValue;
                }

                if (ddlStateCode.SelectedIndex > 0)
                {
                    StateCode = ddlStateCode.SelectedValue;
                }


                GSTIN = txtGSTUniqueID.Text;
                OurGSTIN = ddlOurGSTIN.SelectedValue;

                string BuyersOrderNo = "";
                BuyersOrderNo = txtBuyerOrderNo.Text;
                ClientType = ddlclienttype.SelectedIndex;



                var PTState = "0";

                if (ddlPTState.SelectedIndex > 0)
                {
                    PTState = ddlPTState.SelectedValue;
                }

                var LWFState = "0";

                if (ddlLWFState.SelectedIndex > 0)
                {
                    LWFState = ddlLWFState.SelectedValue;
                }


                #endregion End Code Line Six To PaySheet



                if (txtShipToLine1.Text.Trim().Length > 0)
                {
                    ShiptoLine1 = txtShipToLine1.Text;
                }


                if (txtShipToLine2.Text.Trim().Length > 0)
                {
                    ShiptoLine2 = txtShipToLine2.Text;
                }

                if (txtShipToLine3.Text.Trim().Length > 0)
                {
                    ShiptoLine3 = txtShipToLine3.Text;
                }


                if (txtShipToLine4.Text.Trim().Length > 0)
                {
                    ShiptoLine4 = txtShipToLine4.Text;
                }

                if (txtShipToLine5.Text.Trim().Length > 0)
                {
                    ShiptoLine5 = txtShipToLine5.Text;
                }

                if (txtShipToLine6.Text.Trim().Length > 0)
                {
                    ShiptoLine6 = txtShipToLine6.Text;
                }

                if (txtShipToGSTIN.Text.Trim().Length > 0)
                {
                    ShipToGSTIN = txtShipToGSTIN.Text;
                }

                if (dllfieldofficer.SelectedIndex == 0)
                {
                    Fieldofficer = "0";
                }
                else
                {
                    Fieldofficer = dllfieldofficer.SelectedValue;
                }


                if (ddllocation.Text.Length > 0)
                {
                    Locationid = ddllocation.Text;
                }


                if (ddlAreamanager.SelectedIndex == 0)
                {
                    Areamanager = ddlAreamanager.SelectedValue;
                }
                //else
                //{
                //    Areamanager = ddlAreamanager.SelectedValue;
                //}


                ShipToState = ddlShipToSate.SelectedValue;
                ShipToStateCode = ddlShipToStateCode.SelectedValue;
                var Branch = "0";
                Branch = ddlBranch.SelectedValue;

                var PFBranch = "0";

                if (ddlPFbranch.SelectedIndex > 0)
                {
                    PFBranch = ddlPFbranch.SelectedValue;
                }


                var ESIBranch = "0";

                if (ddlEsibranch.SelectedIndex > 0)
                {
                    ESIBranch = ddlEsibranch.SelectedValue;
                }

                var UnitIncharge = "0";

                if (ddlUnitIncharge.SelectedIndex > 0)
                {
                    UnitIncharge = ddlUnitIncharge.SelectedValue;
                }

                var BillToPanno = "0";
                BillToPanno = txtBilltoPanno.Text;

                var SHIPToPanno = "0";
                SHIPToPanno = txtshiptopanno.Text;

                var Created_On = DateTime.Now;
                var Created_By = UserName;
                #region    Begin Code For Stored Procedure Parameters as on [20-09-2013]
                Hashtable AddClientDetails = new Hashtable();
                string AddClientDetailsPName = "ADDClientDetails";

                #region     Begin Code Client-id to  Contact Person
                AddClientDetails.Add("@ClientId", ClientId);
                AddClientDetails.Add("@ClientName", ClientName);
                AddClientDetails.Add("@ClientShortName", ClientShortName);
                AddClientDetails.Add("@ClientSegment", ClientSegment);
                AddClientDetails.Add("@ClientContactPerson", ClientContactPerson);
                #endregion  End Code Client-id to Contact Person


                #region  Begin Code  Person-Designation To PIN-No

                AddClientDetails.Add("@ClientPersonDesgn", ClientPersonDesgn);
                AddClientDetails.Add("@ClientPhonenumbers", ClientPhonenumbers);
                AddClientDetails.Add("@ClientFax", ClientFax);
                AddClientDetails.Add("@ClientEmail", ClientEmail);
                AddClientDetails.Add("@ClientAddrPin", ClientAddrPin);
                AddClientDetails.Add("@EmailCC", EmailCC);
                AddClientDetails.Add("@ClientType",ClientType);

                #endregion  End Code  Person-Designation To PIN-No


                #region  Begin Code  Line-One To Line-Five

                AddClientDetails.Add("@ClientAddrHno", ClientAddrHno);
                AddClientDetails.Add("@ClientAddrStreet", ClientAddrStreet);
                AddClientDetails.Add("@ClientAddrArea", ClientAddrArea);
                AddClientDetails.Add("@ClientAddrCity", ClientAddrCity);
                AddClientDetails.Add("@ClientAddrColony", ClientAddrColony);
                AddClientDetails.Add("@Line7", Line7);
                AddClientDetails.Add("@Line8", Line8);

                #endregion  End Code Line-One To Line-Five

                #region Begin Code Line Six To PaySheet

                AddClientDetails.Add("@ClientAddrState", ClientAddrState);
                AddClientDetails.Add("@SubUnitStatus", SubUnitStatus);
                AddClientDetails.Add("@MainUnitId", MainUnitId);
                AddClientDetails.Add("@MAinunitStatus", MAinunitStatus);
                AddClientDetails.Add("@Invoice", Invoice);
                AddClientDetails.Add("@Paysheet", Paysheet);
                AddClientDetails.Add("@ClientDesc", ClientDesc);
                AddClientDetails.Add("@ClientPrefix", CmpIDPrefix);

                #endregion End Code Line Six To PaySheet


                AddClientDetails.Add("@Zone", Zone);
                AddClientDetails.Add("@Category", Category);
                AddClientDetails.Add("@state", State);
                AddClientDetails.Add("@StateCode", StateCode);
                AddClientDetails.Add("@GSTIN", GSTIN);
                AddClientDetails.Add("@OurGSTIN", OurGSTIN);
                AddClientDetails.Add("@ShiptoLine1", ShiptoLine1);
                AddClientDetails.Add("@ShiptoLine2", ShiptoLine2);
                AddClientDetails.Add("@ShiptoLine3", ShiptoLine3);
                AddClientDetails.Add("@ShiptoLine4", ShiptoLine4);
                AddClientDetails.Add("@ShiptoLine5", ShiptoLine5);
                AddClientDetails.Add("@ShiptoLine6", ShiptoLine6);
                AddClientDetails.Add("@ShipToState", ShipToState);
                AddClientDetails.Add("@ShipToStateCode", ShipToStateCode);
                AddClientDetails.Add("@ShipToGSTIN", ShipToGSTIN);
                AddClientDetails.Add("@PTState", PTState);
                AddClientDetails.Add("@LWFState", LWFState);
                AddClientDetails.Add("@BuyersOrderNo", BuyersOrderNo);
                AddClientDetails.Add("@Locationid", Locationid);
                AddClientDetails.Add("@Fieldofficer", Fieldofficer);
                AddClientDetails.Add("@Areamanager", Areamanager);
                AddClientDetails.Add("@Created_By", Created_By);
                AddClientDetails.Add("@Created_On", Created_On);
                AddClientDetails.Add("@BranchID", Branch);
                AddClientDetails.Add("@PFBranch", PFBranch);
                AddClientDetails.Add("@ESIBranch", ESIBranch);
                AddClientDetails.Add("@BillToPanno", BillToPanno);
                AddClientDetails.Add("@SHIPToPanno", SHIPToPanno);
                AddClientDetails.Add("@UnitIncharge", UnitIncharge);

                #endregion End Code For Stored Procedure Parameters as on [20-09-2013]

                #region     Begin Code For Calling Stored Procedure as on [20-09-2013]
                IRecordStatus = Config.ExecuteNonQueryParamsAsync(AddClientDetailsPName, AddClientDetails).Result;
                #endregion   End   Code For Calling Stored Procedure as on [20-09-2013]

                #region     Begin Code For Status/Resulted Message of the Inserted Record as on [20-09-2013]

                if (IRecordStatus > 0)
                {
                    lblMsg.Text = "";
                    lblSuc.Text = "Client Details Added Sucessfully  With  Client Id   :- " + txtCId.Text + " ";

                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ');", true);
                    clientid();
                    ClearClientsFieldsData();

                    return;
                }
                else
                {
                    lblMsg.Text = "Client Details Not  Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Not  Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ');", true);
                    return;
                }
                #endregion  End Code For Status/Resulted Message of the Inserted Record as on [20-09-2013]

            }
            catch (Exception ex)
            {
            }
        }

        private void ClearClientsFieldsData()
        {

            txtCname.Text = txtshortname.Text = txtcontactperson.Text = txtphonenumbers.Text = txtfaxno.Text = ddllocation.Text = txtemailid.Text =
            txtpin.Text = txtchno.Text = txtstreet.Text = txtarea.Text = txtcity.Text = txtcolony.Text =
            txtstate.Text = txtdescription.Text = txtGSTUniqueID.Text = txtEmailCC.Text = txtBilltoPanno.Text = txtshiptopanno.Text = string.Empty;

            ddlsegment.SelectedIndex = ddldesgn.SelectedIndex = ddlPFbranch.SelectedIndex = ddlEsibranch.SelectedIndex = ddlUnits.SelectedIndex = ddlZones.SelectedIndex = ddlstate.SelectedIndex = ddlStateCode.SelectedIndex=ddlclienttype.SelectedIndex = dllfieldofficer.SelectedIndex = ddlAreamanager.SelectedIndex = ddlUnitIncharge.SelectedIndex = 0;
            ddlUnits.Visible = false;

            chkSubUnit.Checked = false;

            radioinvoiceyes.Checked = radioinvoiceno.Checked = radiopaysheetyes.Checked = radiopaysheetno.Checked = radioyesmu.Checked = radionomu.Checked = false;

        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            ClearClientsFieldsData();
        }

        protected void chkSubUnit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSubUnit.Checked)
            {
                ddlUnits.Visible = true;
            }
            else
            {
                ddlUnits.Visible = false;
            }
        }

        #region Begin New code for client imports from excel on 24/02/2014 by venkat

        public string GetExcelSheetNames()
        {
            string ExcelSheetname = "";
            OleDbConnection con = null;
            DataTable dt = null;
            string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
            fileupload1.PostedFile.SaveAs(filename);
            string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
            string conStr = string.Empty;
            if (extn.ToLower() == ".xls")
            {
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (extn.ToLower() == ".xlsx")
            {
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
            }

            con = new OleDbConnection(conStr);
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();
            ////foreach (DataRow row in dt.Rows)
            ////{
            ////    ExcelSheetname = row["TABLE_NAME"].ToString();
            ////}

            return ExcelSheetname;
        }

        protected void btnImportData_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = Path.Combine(Server.MapPath("ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
                fileupload1.PostedFile.SaveAs(filename);
                string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
                string constring = string.Empty;

                if (extn.ToLower() == ".xls")
                {
                    constring = "Provider=Microsoft.Jet.OLEDB.8.0;Data Source='" + filename + "';Extended Properties=\"excel 8.0;HDR=Yes;IMEX\"";
                }
                if (extn.ToLower() == ".xlsx")
                {
                    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filename + "';Extended Properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                }
                string qry = "select [Client Id],[Client Name],[Short Name],[Segment],[Contact Person],[Designation]," +
                        " [Phone],[Fax No],[Email Id],[Pin No],[Line One],[Line Two],[Line Three],[Line Four]," +
                        " [Line Five],[Line Six],[Description],[Sub Unit],[Main Unit],[Pay Sheet],[Invoice] from [" + GetExcelSheetNames() + "]" + "";
                OleDbConnection con = new OleDbConnection(constring);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                OleDbCommand cmd = new OleDbCommand(qry, con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                con.Close();
                con.Dispose();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    var ClientId = string.Empty;
                    var Name = string.Empty;
                    var ShortName = string.Empty;
                    var Segment = string.Empty;
                    var Contactperson = string.Empty;
                    var Designation = string.Empty;
                    var Phoneno = string.Empty;
                    var Emailid = string.Empty;
                    var Faxno = string.Empty;
                    var Pinno = string.Empty;
                    var ClientAddrHno = string.Empty;
                    var ClientAddrStreet = string.Empty;
                    var ClientAddrArea = string.Empty;
                    var ClientAddrCity = string.Empty;
                    var ClientAddrColony = string.Empty;
                    var ClientAddrState = string.Empty;
                    var Description = string.Empty;

                    int Subunit = 0;
                    int Mainunit = 0;
                    int Paysheet = 0;
                    int Invoice = 0;
                    int MAinunitStatus = 0;

                    var IRecordStatus = 0;
                    var URecordStatus = 0;


                    ClientId = dr["Client Id"].ToString();
                    Name = dr["Client Name"].ToString();
                    ShortName = dr["Short Name"].ToString();
                    Segment = dr["Segment"].ToString();
                    Contactperson = dr["Contact Person"].ToString();
                    Designation = dr["Designation"].ToString();
                    Phoneno = dr["Phone"].ToString();
                    Emailid = dr["Email Id"].ToString();
                    Faxno = dr["Fax No"].ToString();
                    Pinno = dr["Pin No"].ToString();
                    ClientAddrHno = dr["Line One"].ToString();
                    ClientAddrStreet = dr["Line Two"].ToString();
                    ClientAddrArea = dr["Line Three"].ToString();
                    ClientAddrCity = dr["Line Four"].ToString();
                    ClientAddrColony = dr["Line Five"].ToString();
                    ClientAddrState = dr["Line Six"].ToString();
                    Description = dr["Description"].ToString();


                    // MAinunitStatus = int.Parse(dr["Main Unit"].ToString().ToString());

                    if (String.IsNullOrEmpty(dr["Sub Unit"].ToString()) == false)
                    {
                        Subunit = int.Parse(dr["Sub Unit"].ToString());
                    }
                    if (String.IsNullOrEmpty(dr["Main Unit"].ToString()) == false)
                    {
                        Mainunit = int.Parse(dr["Main Unit"].ToString());
                    }

                    if (String.IsNullOrEmpty(dr["Pay Sheet"].ToString()) == false)
                    {
                        Paysheet = int.Parse(dr["Pay Sheet"].ToString());
                    }

                    if (String.IsNullOrEmpty(dr["Invoice"].ToString()) == false)
                    {
                        Invoice = int.Parse(dr["Invoice"].ToString());
                    }

                    if (ClientId.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Client Id');", true);
                        return;
                    }
                    if (Name.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Client Name');", true);
                        return;
                    }
                    if (Contactperson.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Contact Person');", true);
                        return;
                    }
                    if (Designation.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Designation');", true);
                        return;
                    }
                    if (Phoneno.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Phone No');", true);
                        return;
                    }


                    string sqlclient = "select * from clients where clientid='" + ClientId + "'";
                    DataTable dtclient = Config.ExecuteReaderWithQueryAsync(sqlclient).Result;
                    if (dtclient.Rows.Count > 0)
                    {
                        Hashtable ModifyClientDetails = new Hashtable();
                        string ModifyClientDetailsPName = "ModifyClientDetails";

                        #region     Begin Code Client-id to  Contact Person
                        ModifyClientDetails.Add("@ClientId", ClientId);
                        ModifyClientDetails.Add("@ClientName", Name);
                        ModifyClientDetails.Add("@ClientShortName", ShortName);
                        ModifyClientDetails.Add("@ClientSegment", Segment);
                        ModifyClientDetails.Add("@ClientContactPerson", Contactperson);
                        #endregion  End Code Client-id to Contact Person


                        #region  Begin Code  Person-Designation To PIN-No

                        ModifyClientDetails.Add("@ClientPersonDesgn", Designation);
                        ModifyClientDetails.Add("@ClientPhonenumbers", Phoneno);
                        ModifyClientDetails.Add("@ClientFax", Faxno);
                        ModifyClientDetails.Add("@ClientEmail", Emailid);
                        ModifyClientDetails.Add("@ClientAddrPin", Pinno);

                        #endregion  End Code  Person-Designation To PIN-No


                        #region  Begin Code  Line-One To Line-Five

                        ModifyClientDetails.Add("@ClientAddrHno", ClientAddrHno);
                        ModifyClientDetails.Add("@ClientAddrStreet", ClientAddrStreet);
                        ModifyClientDetails.Add("@ClientAddrArea", ClientAddrArea);
                        ModifyClientDetails.Add("@ClientAddrCity", ClientAddrCity);
                        ModifyClientDetails.Add("@ClientAddrColony", ClientAddrColony);

                        #endregion  End Code Line-One To Line-Five

                        #region Begin Code Line Six To PaySheet

                        ModifyClientDetails.Add("@ClientAddrState", ClientAddrState);
                        ModifyClientDetails.Add("@SubUnitStatus", Subunit);
                        ModifyClientDetails.Add("@MainUnitId", Mainunit);
                        ModifyClientDetails.Add("@MAinunitStatus", MAinunitStatus);
                        ModifyClientDetails.Add("@Invoice", Invoice);
                        ModifyClientDetails.Add("@Paysheet", Paysheet);
                        ModifyClientDetails.Add("@ClientDesc", Description);
                        ModifyClientDetails.Add("@ClientPrefix", CmpIDPrefix);

                        #endregion End Code Line Six To PaySheet

                        URecordStatus = Config.ExecuteNonQueryParamsAsync(ModifyClientDetailsPName, ModifyClientDetails).Result;

                        if (URecordStatus > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Modified Sucessfully.  With  Client Id   :- " + ClientId + " -: ');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Not  Added Sucessfully  With  Client Id   :- " + ClientId + " -: ');", true);
                            //return;
                        }
                    }
                    else
                    {


                        Hashtable AddClientDetails = new Hashtable();
                        string AddClientDetailsPName = "ADDClientDetails";

                        #region     Begin Code Client-id to  Contact Person
                        AddClientDetails.Add("@ClientId", ClientId);
                        AddClientDetails.Add("@ClientName", Name);
                        AddClientDetails.Add("@ClientShortName", ShortName);
                        AddClientDetails.Add("@ClientSegment", Segment);
                        AddClientDetails.Add("@ClientContactPerson", Contactperson);
                        #endregion  End Code Client-id to Contact Person


                        #region  Begin Code  Person-Designation To PIN-No

                        AddClientDetails.Add("@ClientPersonDesgn", Designation);
                        AddClientDetails.Add("@ClientPhonenumbers", Phoneno);
                        AddClientDetails.Add("@ClientFax", Faxno);
                        AddClientDetails.Add("@ClientEmail", Emailid);
                        AddClientDetails.Add("@ClientAddrPin", Pinno);

                        #endregion  End Code  Person-Designation To PIN-No


                        #region  Begin Code  Line-One To Line-Five

                        AddClientDetails.Add("@ClientAddrHno", ClientAddrHno);
                        AddClientDetails.Add("@ClientAddrStreet", ClientAddrStreet);
                        AddClientDetails.Add("@ClientAddrArea", ClientAddrArea);
                        AddClientDetails.Add("@ClientAddrCity", ClientAddrCity);
                        AddClientDetails.Add("@ClientAddrColony", ClientAddrColony);

                        #endregion  End Code Line-One To Line-Five

                        #region Begin Code Line Six To PaySheet

                        AddClientDetails.Add("@ClientAddrState", ClientAddrState);
                        AddClientDetails.Add("@SubUnitStatus", Subunit);
                        AddClientDetails.Add("@MainUnitId", Mainunit);
                        AddClientDetails.Add("@MAinunitStatus", MAinunitStatus);
                        AddClientDetails.Add("@Invoice", Invoice);
                        AddClientDetails.Add("@Paysheet", Paysheet);
                        AddClientDetails.Add("@ClientDesc", Description);
                        AddClientDetails.Add("@ClientPrefix", CmpIDPrefix);

                        #endregion End Code Line Six To PaySheet

                        IRecordStatus = Config.ExecuteNonQueryParamsAsync(AddClientDetailsPName, AddClientDetails).Result;

                        if (IRecordStatus > 0)
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Show Alert", "alert('Client Details Not  Added Sucessfully  With  Client Id   :- " + txtCId.Text + " -: ');", true);

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please upload valid data');", true);
            }

        }

        #endregion

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select GSTstatecode,stateid from states where stateid='" + ddlstate.SelectedValue + "'";
            DataTable dt = Config.ExecuteReaderWithQueryAsync(qry).Result;
            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["stateid"].ToString() != "0")
                {
                    ddlStateCode.SelectedValue = dt.Rows[0]["stateid"].ToString();
                }
                else
                {
                    ddlStateCode.SelectedIndex = 0;
                }

            }
            else
            {
                ddlStateCode.SelectedIndex = 0;

            }
        }

        protected void ddlShipToSate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select GSTstatecode,stateid from states where stateid='" + ddlShipToSate.SelectedValue + "'";
            DataTable dt = SqlHelper.Instance.GetTableByQuery(qry);
            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["stateid"].ToString() != "0")
                {
                    ddlShipToStateCode.SelectedValue = dt.Rows[0]["stateid"].ToString();
                }
                else
                {
                    ddlShipToStateCode.SelectedIndex = 0;
                }
            }
            else
            {
                ddlShipToStateCode.SelectedIndex = 0;

            }
        }

        protected void ddlBillToSate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select GSTstatecode,stateid from states where stateid='" + ddlShipToSate.SelectedValue + "'";
            DataTable dt = SqlHelper.Instance.GetTableByQuery(qry);
            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["stateid"].ToString() != "0")
                {
                    ddlShipToStateCode.SelectedValue = dt.Rows[0]["stateid"].ToString();
                }
                else
                {
                    ddlShipToStateCode.SelectedIndex = 0;
                }

            }
            else
            {
                ddlShipToStateCode.SelectedIndex = 0;

            }
        }

        protected void fillfieldofficer()
        {
            string query = "select Fieldofficer ,id from AreaWiseManager";
            DataTable dt = Config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                dllfieldofficer.DataTextField = "Fieldofficer";
                dllfieldofficer.DataValueField = "id";
                dllfieldofficer.DataSource = dt;
                dllfieldofficer.DataBind();
                dllfieldofficer.Items.Insert(0, "--select--");
            }
        }

        protected void dllfieldofficer_SelectedIndexChanged(object sender, EventArgs e)
        {


            string Areamanager = "";
            string query = "select l.Locationname,L.locationid,a.areamanager,a.id from Areawisemanager a inner join location l on l.locationid=a.locationid where fieldofficer='" + dllfieldofficer.SelectedItem.ToString() + "'";
            DataTable dt = Config.ExecuteAdaptorAsyncWithQueryParams(query).Result;


            if (dt.Rows.Count > 0)
            {
                ddlAreamanager.DataTextField = "Areamanager";
                ddlAreamanager.DataValueField = "id";
                ddlAreamanager.DataSource = dt;
                ddlAreamanager.DataBind();
            }
        }

        protected void ddlUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkSubUnit.Checked == true)
            {
                string SubQry = "Select * from clients where clientId='" + ddlUnits.SelectedValue + "' and SubUnitStatus=1";
                DataTable dt = Config.ExecuteAdaptorAsyncWithQueryParams(SubQry).Result;
                if (dt.Rows.Count > 0)
                {
                    lblMsg.Text = "Selected Main Unitid incorrect";
                    ddlUnits.SelectedIndex = 0;
                    return;
                }
            }

        }


    }
}