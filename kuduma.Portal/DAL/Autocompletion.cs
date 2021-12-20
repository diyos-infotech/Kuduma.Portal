using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Globalization;

/// <summary>
/// Summary description for Autocompletion
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Autocompletion : System.Web.Services.WebService
{

    public Autocompletion()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]
    public List<string> getempid(string term)
    {
        var BranchID = string.Empty;
        BranchID = HttpContext.Current.Session["BranchID"].ToString();
        List<string> listempid = new List<string>();
        string cs = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;
        using (SqlConnection cn = new SqlConnection(cs))
        {

            SqlCommand cmd = new SqlCommand("select empid from empdetails where empid like '%" + term + "%'  and Branch in (" + BranchID + ")  ", cn);
            cmd.CommandType = CommandType.Text;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listempid.Add(dr["empid"].ToString());

            }
            return listempid;
        }

    }


    [WebMethod(EnableSession = true)]
    public List<string> GetEmpIDs(string term, string IssueType)
    {
        var EmpIDPrefix = string.Empty;
        EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();

        var BranchID = string.Empty;
        BranchID = HttpContext.Current.Session["BranchID"].ToString();

        List<string> EmpIds = new List<string>();
        string cs = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;
        SqlCommand cmd;
        using (SqlConnection cn = new SqlConnection(cs))
        {
            if (IssueType == "I" || IssueType == "D")
            {
                cmd = new SqlCommand("Select empid from empdetails where EmpId like '%" + EmpIDPrefix + "%'  and Branch in (" + BranchID + ")  and empid like '%" + term + "%' ", cn);
            }
            else
            {
                cmd = new SqlCommand("Select empid from empdetails where EmpId like '%" + EmpIDPrefix + "%'  and Branch in (" + BranchID + ")  and empid like '%" + term + "%'  and employeetype='S'  ", cn);
            }

            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@term", term);
            cmd.Parameters.AddWithValue("@EmpIDPrefix", EmpIDPrefix);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                EmpIds.Add(dr["empid"].ToString());
            }

            return EmpIds;
        }


    }

    [WebMethod(EnableSession = true)]
    public List<string> GetEmpIDandoldids(string term)
    {
        var EmpIDPrefix = string.Empty;
        EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();

        var BranchID = string.Empty;
        BranchID = HttpContext.Current.Session["BranchID"].ToString();

        List<string> EmpIds = new List<string>();
        string cs = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;
        SqlCommand cmd;
        using (SqlConnection cn = new SqlConnection(cs))
        {

            cmd = new SqlCommand("Select empid from empdetails where EmpId like '" + EmpIDPrefix + "%' and Branch in (" + BranchID + ") and (empid like '%" + term + "%') and empid not like 'nya%'   ", cn);


            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@term", term);
            cmd.Parameters.AddWithValue("@EmpIDPrefix", EmpIDPrefix);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                EmpIds.Add(dr["empid"].ToString());
            }

            return EmpIds;
        }


    }


    [WebMethod(EnableSession = true)]
    public List<string> GetFormEmpIDs(string term)
    {
        var EmpIDPrefix = string.Empty;
        EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();

        var BranchID = string.Empty;
        BranchID = HttpContext.Current.Session["BranchID"].ToString();

        List<string> EmpIds = new List<string>();
        string cs = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;
        SqlCommand cmd;
        using (SqlConnection cn = new SqlConnection(cs))
        {

            cmd = new SqlCommand("Select empid   from empdetails where EmpId like '" + EmpIDPrefix + "%'  and Branch in (" + BranchID + ")  and (empid like '%" + term + "%') and empid not like 'nya%'   ", cn);


            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@term", term);
            cmd.Parameters.AddWithValue("@EmpIDPrefix", EmpIDPrefix);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                EmpIds.Add(dr["empid"].ToString());
            }

            return EmpIds;
        }


    }


    [WebMethod(EnableSession = true)]
    public List<string> GetFormEmpNames(string term)
    {
        var EmpIDPrefix = string.Empty;
        EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();

        var BranchID = string.Empty;
        BranchID = HttpContext.Current.Session["BranchID"].ToString();

        List<string> EmpNames = new List<string>();
        string cs = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;

        using (SqlConnection cn = new SqlConnection(cs))
        {

            SqlCommand cmd = new SqlCommand("Select (empfname+' '+empmname+' '+emplname) as Name from empdetails where EmpId like '" + EmpIDPrefix + "%'  and Branch in (" + BranchID + ")  and empid not like 'nya%' and (empfname+' '+empmname+' '+emplname) like '%" + term + "%' ", cn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@term", term);
            cmd.Parameters.AddWithValue("@EmpIDPrefix", EmpIDPrefix);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                EmpNames.Add(dr["Name"].ToString());
            }

            return EmpNames;
        }


    }


    [WebMethod(EnableSession = true)]
    public List<string> GetFormEmpIDNames(string term)
    {
        var EmpIDPrefix = string.Empty;
        EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();

        var BranchID = string.Empty;
        BranchID = HttpContext.Current.Session["BranchID"].ToString();

        List<string> EmpNames = new List<string>();
        string cs = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;

        using (SqlConnection cn = new SqlConnection(cs))
        {

            SqlCommand cmd = new SqlCommand("Select (empid+' - '+empfname+' '+empmname+' '+emplname) as Name from empdetails where EmpId like '" + EmpIDPrefix + "%'  and Branch in (" + BranchID + ")  and empid not like 'nya%' and (empid like '%" + term + "%' or  (empfname+' '+empmname+' '+emplname) like '%" + term + "%') ", cn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@term", term);
            cmd.Parameters.AddWithValue("@EmpIDPrefix", EmpIDPrefix);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                EmpNames.Add(dr["Name"].ToString());
            }

            return EmpNames;
        }


    }


    [WebMethod(EnableSession = true)]
    public List<string> GetEmpNames(string term, string IssueType)
    {
        var EmpIDPrefix = string.Empty;
        EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();

        var BranchID = string.Empty;
        BranchID = HttpContext.Current.Session["BranchID"].ToString();

        List<string> EmpNames = new List<string>();
        string cs = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;
        SqlCommand cmd;
        using (SqlConnection cn = new SqlConnection(cs))
        {
            if (IssueType == "I" || IssueType == "D")
            {

                cmd = new SqlCommand("Select (empfname+' '+empmname+' '+emplname) as Name from empdetails where EmpId like '%" + EmpIDPrefix + "%' and Branch in (" + BranchID + ")   and  (empfname+' '+empmname+' '+emplname) like '%" + term + "%' ", cn);
            }
            else
            {
                cmd = new SqlCommand("Select (empfname+' '+empmname+' '+emplname) as Name from empdetails where EmpId like '%" + EmpIDPrefix + "%' and Branch in (" + BranchID + ")   and employeetype='S' and  (empfname+' '+empmname+' '+emplname) like '%" + term + "%' ", cn);

            }

            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@term", term);
            cmd.Parameters.AddWithValue("@EmpIDPrefix", EmpIDPrefix);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                EmpNames.Add(dr["Name"].ToString());
            }

            return EmpNames;
        }


    }

    [WebMethod(EnableSession = true)]
    public List<string> GetFormBillNos(string term)
    {
        var EmpIDPrefix = string.Empty;
        EmpIDPrefix = HttpContext.Current.Session["EmpIDPrefix"].ToString();

        var BranchID = string.Empty;
        BranchID = HttpContext.Current.Session["BranchID"].ToString();

        List<string> BillNos = new List<string>();
        string cs = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;
        SqlCommand cmd;
        using (SqlConnection cn = new SqlConnection(cs))
        {

            cmd = new SqlCommand("select BillNo from UnitBill where BillNo like '%" + term + "%' union select billno from MUnitBill where BillNo like '%" + term + "%'", cn);


            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@term", term);
            // cmd.Parameters.AddWithValue("@EmpIDPrefix", EmpIDPrefix);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                BillNos.Add(dr["BillNo"].ToString());
            }

            return BillNos;
        }


    }

}

