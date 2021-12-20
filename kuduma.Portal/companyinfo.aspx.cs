using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Kuduma.Portal.DAL;

namespace Kuduma.Portal
{
    public partial class companyinfo : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        string CmpIDPrefix = "";
        string BranchID = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            GetWebConfigdata();

            if (!IsPostBack)
            {
                if (this.Master != null)
                {
                    HtmlAnchor emplink = (HtmlAnchor)this.Master.FindControl("li3").FindControl("CompanyInfoLink");
                    if (emplink != null)
                    {
                        emplink.Attributes["class"] = "current";
                    }
                }
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {

                }
                else
                {
                    Response.Redirect("login.aspx");
                }



                LoadPreviousData();
            }
        }

        protected void btnaddclint_Click(object sender, EventArgs e)
        {
            lblresult.Visible = true;
            try
            {

                if (txtcsname.Text.Trim().Length == 0)
                {

                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Bill Seq Don't Leave Empty');", true);
                    return;

                }

                Modify();
            }
            catch (Exception ex)
            {
                //lblresult.Visible = true;
                // lblresult.Text = ex.Message;
            }
        }

        public void Modify()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            string cname = txtcname.Text;
            string csname = txtcsname.Text;
            string address = txtaddress.Text;
            string pfno = txtpfno.Text;
            string esino = txtesino.Text;
            string billdesc = txtbilldesc.Text;
            string cinfo = txtcinfo.Text;
            string billnotes = txtbnotes.Text;
            string billseq = txtbillsq.Text;
            string labourrule = txtlabour.Text;
            string category = txtCategory.Text;
            string GSTNo = txtGSTNo.Text;
            string HSNNumber = txtHsnNummber.Text;
            string SACCode = txtSacCode.Text;
            string Accountno = txtAccountno.Text;
            string IFSCCOde = txtifsccode.Text;
            //string ChequePREPARE = txtPREPARE.Text;
            string Bank = txtBANK.Text;
            string BranchName = txtbranch.Text;
            //string Addresslineone = txtaddresslineone.Text;
            //string Addresslinetwo = txtaddresslinetwo.Text;
            //string SASTC = txtsastcc.Text;
            string Phoneno = txtPhoneno.Text;
            string Faxno = txtFaxno.Text;
            string Emailid = txtEmail.Text;
            string Website = txtWebsite.Text;
            string notes = txtNotes.Text;
            string CorporateIDNO = txtcorporateIDNo.Text;
            string RegNo = txtregno.Text;
            string CINNo = txtcinno.Text;
            string ESICNoForms = txtESICNoForms.Text;
            string BranchOffice = txtBranchOffice.Text;
            var PTaxNo = txtptaxno.Text;
            var MSMENo = txtMSMEno.Text;
            var RecipientNo= txtRecipientNo.Text;
            //string ISOCertNo = txtISOCertNo.Text;
            //string PsaraAct = txtPsaraAct.Text;
            //string KSSAMemberShipNo = txtKSSAMemberShipNo.Text;

            string SqlQry = "select * from companyinfo where BranchID='" + Session["Branch"].ToString() + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQry).Result;
            cmd.Parameters.Clear();
            if (dt.Rows.Count > 0)
            {
                cmd.CommandText = "modifyaddcompanyinfo";
            }
            else
            {
                cmd.CommandText = "addcompanyinfo";
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@pcompanyname", cname);
            cmd.Parameters.AddWithValue("@pshortname", csname);
            cmd.Parameters.AddWithValue("@paddress", address);
            cmd.Parameters.AddWithValue("@ppfno", pfno);
            cmd.Parameters.AddWithValue("@pesino", esino);
            cmd.Parameters.AddWithValue("@pbilldesc", billdesc);
            cmd.Parameters.AddWithValue("@pcompanyinfo", cinfo);
            cmd.Parameters.AddWithValue("@pbillnotes", billnotes);
            cmd.Parameters.AddWithValue("@pbillseq", billseq);
            cmd.Parameters.AddWithValue("@plabourrule", labourrule);
            //cmd.Parameters.AddWithValue("@plogo", Session["imagebytes"]  );
            //cmd.Parameters.AddWithValue("@ChequePrepare", ChequePREPARE);
            cmd.Parameters.AddWithValue("@Bankname", Bank);
            cmd.Parameters.AddWithValue("@bankaccountno", Accountno);
            //cmd.Parameters.AddWithValue("@Addresslineone", Addresslineone);
            //cmd.Parameters.AddWithValue("@Addresslinetwo",Addresslinetwo);
            cmd.Parameters.AddWithValue("@IfscCode", IFSCCOde);
            cmd.Parameters.AddWithValue("@BranchName", BranchName);
            //cmd.Parameters.AddWithValue("@SASTC", SASTC);
            cmd.Parameters.AddWithValue("@ClientidPrefix", CmpIDPrefix);
            cmd.Parameters.AddWithValue("@Phoneno", Phoneno);
            cmd.Parameters.AddWithValue("@Faxno", Faxno);
            cmd.Parameters.AddWithValue("@Emailid", Emailid);
            cmd.Parameters.AddWithValue("@Website", Website);
            cmd.Parameters.AddWithValue("@Notes", notes);
            cmd.Parameters.AddWithValue("@CorporateIDNo", CorporateIDNO);
            cmd.Parameters.AddWithValue("@RegNo", RegNo);
            cmd.Parameters.AddWithValue("@ESICNoForms", ESICNoForms);
            cmd.Parameters.AddWithValue("@BranchOffice", BranchOffice);
            cmd.Parameters.AddWithValue("@Category", category);
            cmd.Parameters.AddWithValue("@RecipientNo", RecipientNo);
            cmd.Parameters.AddWithValue("@GSTNo", GSTNo);
            cmd.Parameters.AddWithValue("@HSNNumber", HSNNumber);
            cmd.Parameters.AddWithValue("@SACCode", SACCode);
            cmd.Parameters.AddWithValue("@PTaxNo", PTaxNo);
            cmd.Parameters.AddWithValue("@CINNo", CINNo);
            cmd.Parameters.AddWithValue("@MSMENo", MSMENo);
            cmd.Parameters.AddWithValue("@branchid", Session["Branch"].ToString());
            //cmd.Parameters.AddWithValue("@ISOCertfNo", ISOCertNo);
            //cmd.Parameters.AddWithValue("@PSARARegNo", PsaraAct);
            //cmd.Parameters.AddWithValue("@KSSAMembershipNo", KSSAMemberShipNo);

            int status = cmd.ExecuteNonQuery();
            con.Close();

            if (status != 0)
            {
                lblresult.Visible = true;
                lblresult.Text = "Record  added Successfully";
                Disablefields();
            }
            else
            {
                lblresult.Visible = true;
                lblresult.Text = "Record Not Inserted";
            }
        }

        public void Insert()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            string cname = txtcname.Text;
            string csname = txtcsname.Text;
            string address = txtaddress.Text;
            string pfno = txtpfno.Text;
            string esino = txtesino.Text;
            string billdesc = txtbilldesc.Text;
            string cinfo = txtcinfo.Text;
            string Category = txtCategory.Text;
            string billnotes = txtbnotes.Text;
            string billseq = txtbillsq.Text;
            string labourrule = txtlabour.Text;
            string Phoneno = txtPhoneno.Text;
            string Faxno = txtFaxno.Text;
            string Emailid = txtEmail.Text;
            string Website = txtWebsite.Text;
            string notes = txtNotes.Text;
            string CorporateIDNO = txtcorporateIDNo.Text;
            string RegNo = txtregno.Text;
            string CINNo = txtcinno.Text;
            string ESICNoForms = txtESICNoForms.Text;
            string BranchOffice = txtBranchOffice.Text;
            //string ISOCertNo = txtISOCertNo.Text ;
            //string PsaraAct = txtPsaraAct.Text ;
            //string KSSAMemberShipNo = txtKSSAMemberShipNo.Text ;
            string GSTNo = txtGSTNo.Text;
            string HSNNumber = txtHsnNummber.Text;
            string SACCode = txtSacCode.Text;
            var PTaxNo = txtptaxno.Text;
            string Accountno = txtAccountno.Text;
            string IFSCCOde = txtifsccode.Text;
            string Bank = txtBANK.Text;
            string BranchName = txtbranch.Text;
            var MSMENo = txtMSMEno.Text;
            var RecipientNo = txtRecipientNo.Text;
            cmd.CommandText = "addcompanyinfo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@pcompanyname", cname);
            cmd.Parameters.AddWithValue("@pshortname", csname);
            cmd.Parameters.AddWithValue("@paddress", address);
            cmd.Parameters.AddWithValue("@ppfno", pfno);
            cmd.Parameters.AddWithValue("@pesino", esino);
            cmd.Parameters.AddWithValue("@pbilldesc", billdesc);
            cmd.Parameters.AddWithValue("@pcompanyinfo", cinfo);
            cmd.Parameters.AddWithValue("@pbillnotes", billnotes);
            cmd.Parameters.AddWithValue("@pbillseq", billseq);
            cmd.Parameters.AddWithValue("@plabourrule", labourrule);
            // cmd.Parameters.AddWithValue("@plogo", Session["imagebytes"]);
            cmd.Parameters.AddWithValue("@notes", notes);
            cmd.Parameters.AddWithValue("@Phoneno", Phoneno);
            cmd.Parameters.AddWithValue("@Faxno", Faxno);
            cmd.Parameters.AddWithValue("@Emailid", Emailid);
            cmd.Parameters.AddWithValue("@Website", Website);
            cmd.Parameters.AddWithValue("@CorporateIDNo", CorporateIDNO);
            cmd.Parameters.AddWithValue("@RegNo", RegNo);
            cmd.Parameters.AddWithValue("@ESICNoForms", ESICNoForms);
            cmd.Parameters.AddWithValue("@BranchOffice", BranchOffice);
            cmd.Parameters.AddWithValue("@Category", Category);
            cmd.Parameters.AddWithValue("@branchid", Session["Branch"].ToString());
            //cmd.Parameters.AddWithValue("@ISOCertfNo", ISOCertNo);
            //cmd.Parameters.AddWithValue("@PSARARegNo", PsaraAct);
            //cmd.Parameters.AddWithValue("@KSSAMembershipNo", KSSAMemberShipNo);
            cmd.Parameters.AddWithValue("@GSTNo", GSTNo);
            cmd.Parameters.AddWithValue("@HSNNumber", HSNNumber);
            cmd.Parameters.AddWithValue("@SACCode", SACCode);
            cmd.Parameters.AddWithValue("@PTaxNo", PTaxNo);
            cmd.Parameters.AddWithValue("@RecipientNo", RecipientNo);
            cmd.Parameters.AddWithValue("@Bankname", Bank);
            cmd.Parameters.AddWithValue("@bankaccountno", Accountno);
            cmd.Parameters.AddWithValue("@BranchName", BranchName);
            cmd.Parameters.AddWithValue("@IfscCode", IFSCCOde);
            cmd.Parameters.AddWithValue("@CINNo", CINNo);
            cmd.Parameters.AddWithValue("@MSMENo", MSMENo);

            int status = cmd.ExecuteNonQuery();
            con.Close();

            if (status != 0)
            {
                lblresult.Visible = true;
                lblresult.Text = "Record  Added Successfully";
                Disablefields();
            }
            else
            {
                lblresult.Visible = true;
                lblresult.Text = "Record Not Inserted";
            }
        }

        private void clearData()
        {
            txtcname.Text = txtcsname.Text = txtaddress.Text = txtpfno.Text = txtesino.Text = txtbilldesc.Text = txtptaxno.Text = txtbranch.Text = txtBANK.Text = txtAccountno.Text = txtifsccode.Text = string.Empty;
            txtcinfo.Text = txtbnotes.Text = txtCategory.Text = txtbillsq.Text = txtlabour.Text = txtGSTNo.Text = txtHsnNummber.Text = txtSacCode.Text = string.Empty;
            //imglogo.Src = "";
            txtPhoneno.Text = txtFaxno.Text = txtEmail.Text = txtWebsite.Text = txtNotes.Text = txtregno.Text = txtcorporateIDNo.Text = txtcinno.Text = txtMSMEno.Text= txtRecipientNo.Text = string.Empty;
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            clearData();
        }


        protected void GetWebConfigdata()
        {

            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            BranchID = Session["BranchID"].ToString();

        }

        protected void LoadPreviousData()
        {
            string selectquery = "select * from companyinfo where BranchID='" + Session["Branch"].ToString() + "' ";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

            if (dt.Rows.Count > 0)
            {
                txtcname.Text = dt.Rows[0]["companyname"].ToString();
                txtcsname.Text = dt.Rows[0]["shortname"].ToString();
                txtaddress.Text = dt.Rows[0]["address"].ToString();
                txtpfno.Text = dt.Rows[0]["pfno"].ToString();
                txtesino.Text = dt.Rows[0]["esino"].ToString();
                txtbilldesc.Text = dt.Rows[0]["billdesc"].ToString();
                txtcinfo.Text = dt.Rows[0]["companyinfo"].ToString();
                txtbnotes.Text = dt.Rows[0]["billnotes"].ToString();
                txtbillsq.Text = dt.Rows[0]["billseq"].ToString();
                txtlabour.Text = dt.Rows[0]["labourrule"].ToString();
                txtCategory.Text = dt.Rows[0]["Category"].ToString();
                txtGSTNo.Text = dt.Rows[0]["GSTNo"].ToString();
                txtHsnNummber.Text = dt.Rows[0]["HSNNumber"].ToString();
                txtSacCode.Text = dt.Rows[0]["SACCode"].ToString();
                txtptaxno.Text = dt.Rows[0]["PTaxNo"].ToString();
                //txtPREPARE.Text = dt.Rows[0]["ChequePrepare"].ToString();
                txtBANK.Text = dt.Rows[0]["Bankname"].ToString();
                txtAccountno.Text = dt.Rows[0]["bankaccountno"].ToString();
                //txtaddresslineone.Text = dt.Rows[0]["Addresslineone"].ToString();
                //txtaddresslinetwo.Text = dt.Rows[0]["Addresslinetwo"].ToString();
                txtifsccode.Text = dt.Rows[0]["IfscCode"].ToString();
                txtbranch.Text = dt.Rows[0]["BranchName"].ToString();
                txtPhoneno.Text = dt.Rows[0]["Phoneno"].ToString();
                txtFaxno.Text = dt.Rows[0]["Faxno"].ToString();
                txtEmail.Text = dt.Rows[0]["Emailid"].ToString();
                txtWebsite.Text = dt.Rows[0]["Website"].ToString();
                txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                txtcorporateIDNo.Text = dt.Rows[0]["CorporateIDNo"].ToString();
                txtregno.Text = dt.Rows[0]["RegNo"].ToString();
                txtcinno.Text = dt.Rows[0]["CINNo"].ToString();
                txtESICNoForms.Text = dt.Rows[0]["ESICNoForms"].ToString();
                txtBranchOffice.Text = dt.Rows[0]["BranchOffice"].ToString();
                txtMSMEno.Text = dt.Rows[0]["MSMENo"].ToString();
                txtRecipientNo.Text= dt.Rows[0]["RecipientNo"].ToString();
                //txtISOCertNo.Text = dt.Rows[0]["ISOCertfNo"].ToString();
                //txtPsaraAct.Text = dt.Rows[0]["PSARARegNo"].ToString();
                //txtKSSAMemberShipNo.Text = dt.Rows[0]["KSSAMembershipNo"].ToString();


            }

            else
            {

                Enabledfields();
            }
        }

        public void Enabledfields()
        {
            txtcname.Enabled = true;
            txtcsname.Enabled = true;
            txtaddress.Enabled = true;
            txtpfno.Enabled = true;
            txtesino.Enabled = true;
            txtbilldesc.Enabled = true;
            txtcinfo.Enabled = true;
            txtCategory.Enabled = true;
            txtbnotes.Enabled = true;
            txtBANK.Enabled = true;
            txtbillsq.Enabled = true;
            txtlabour.Enabled = true;
            txtPhoneno.Enabled = true;
            txtFaxno.Enabled = true;
            txtEmail.Enabled = true;
            txtWebsite.Enabled = true;
            txtNotes.Enabled = true;
            txtpfno.Enabled = true;
            txtcorporateIDNo.Enabled = true;
            txtregno.Enabled = true;
            txtcinno.Enabled = true;
            txtESICNoForms.Enabled = true;
            txtBranchOffice.Enabled = true;
            txtISOCertNo.Enabled = true;
            txtPsaraAct.Enabled = true;
            txtKSSAMemberShipNo.Enabled = true;
            btnaddclint.Enabled = true;
            btncancel.Enabled = true;
            btnEdit.Enabled = false;
            lblresult.Visible = false;
            txtGSTNo.Enabled = true;
            txtHsnNummber.Enabled = true;
            txtSacCode.Enabled = true;
            txtbranch.Enabled = true;
            txtifsccode.Enabled = true;
            txtAccountno.Enabled = true;
            txtHsnNummber.Enabled = true;
            txtSacCode.Enabled = true;
            txtptaxno.Enabled = true;
            txtMSMEno.Enabled = true;
            txtRecipientNo.Enabled = true;
        }

        public void Disablefields()
        {

            txtESICNoForms.Enabled = false;
            txtBranchOffice.Enabled = false;
            txtISOCertNo.Enabled = false;
            txtPsaraAct.Enabled = false;
            txtKSSAMemberShipNo.Enabled = false;
            txtcname.Enabled = false;
            txtcsname.Enabled = false;
            txtaddress.Enabled = false;
            txtpfno.Enabled = false;
            txtesino.Enabled = false;
            txtbilldesc.Enabled = false;
            txtBANK.Enabled = false;
            txtcinfo.Enabled = false;
            txtCategory.Enabled = false;
            txtbnotes.Enabled = false;
            txtbillsq.Enabled = false;
            txtlabour.Enabled = false;
            txtPhoneno.Enabled = false;
            txtFaxno.Enabled = false;
            txtEmail.Enabled = false;
            txtWebsite.Enabled = false;
            txtNotes.Enabled = false;
            txtpfno.Enabled = false;
            txtcorporateIDNo.Enabled = false;
            txtregno.Enabled = false;
            txtcinno.Enabled = false;
            btnaddclint.Enabled = false;
            btncancel.Enabled = false;
            btnEdit.Enabled = true;
            txtGSTNo.Enabled = false;
            txtbranch.Enabled = false;
            txtifsccode.Enabled = false;
            txtAccountno.Enabled = false;
            txtHsnNummber.Enabled = false;
            txtSacCode.Enabled = false;
            txtptaxno.Enabled = false;
            txtMSMEno.Enabled = false;
            txtRecipientNo.Enabled = false;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string selectquery = "select * from companyinfo  ";

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

            if (dt.Rows.Count > 0)
            {
                Enabledfields();
            }
        }
    }
}