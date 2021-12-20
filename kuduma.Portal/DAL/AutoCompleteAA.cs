using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using KLTS.Data;
using System.Data;

/// <summary>
/// Summary description for AutoCompleteAA
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ToolboxItem(false)]
[System.Web.Script.Services.ScriptService]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class AutoCompleteAA : System.Web.Services.WebService {

     
    public AutoCompleteAA () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetClientids(string prefixText, int count)
    {
        string SPName = "GetClientIdsBasedontext";
        Hashtable HTSpParameters = new Hashtable();
        HTSpParameters.Add("@Clientid", prefixText);
        DataTable DtCIds = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTSpParameters);

        string[] prefixTextArray = null;
        if (DtCIds.Rows.Count > 0)
        {
            // Get the Products From Data Source. Change this method to use Database
            List<string> ListClientids = new List<string>();
            for (int i = 0; i < DtCIds.Rows.Count; i++)
            {
                if (i < count)
                {
                    ListClientids.Add(DtCIds.Rows[i]["Clientid"].ToString());
                }
                else
                    break;
            }
            //Convert to Array as We need to return Array
            prefixTextArray = ListClientids.ToArray();//ToArray<string>();
        }
        //Return Selected Products
        return prefixTextArray;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetClientNames(string prefixText, int count)
    {

        string SPName = "GetClientNamesBasedontext";
        Hashtable HTSpParameters = new Hashtable();
        HTSpParameters.Add("@ClientName", prefixText);
        DataTable DtCNames = SqlHelper.Instance.ExecuteStoredProcedureWithParams(SPName, HTSpParameters);


        string[] prefixTextArray = null;
        if (DtCNames.Rows.Count > 0)
        {
            // Get the Products From Data Source. Change this method to use Database
            List<string> ListClientNames = new List<string>();
            for (int i = 0; i < DtCNames.Rows.Count; i++)
            {
                if (i < count)
                {
                    ListClientNames.Add(DtCNames.Rows[i]["Clientname"].ToString());
                }
                else
                    break;
            }
            //Convert to Array as We need to return Array
            prefixTextArray = ListClientNames.ToArray();//ToArray<string>();
        }
        //Return Selected Products
        return prefixTextArray;
    }


    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetStateNames(string prefixText, int count)
    {

        string sqlqry = "Select State from States where State like '%'+ '" + prefixText + "' +'%'  ";
        DataTable DtStateNames = SqlHelper.Instance.GetTableByQuery(sqlqry);


        string[] prefixTextArray = null;
        if (DtStateNames.Rows.Count > 0)
        {
            // Get the Products From Data Source. Change this method to use Database
            List<string> ListStateNames = new List<string>();
            for (int i = 0; i < DtStateNames.Rows.Count; i++)
            {
                if (i < count)
                {
                    ListStateNames.Add(DtStateNames.Rows[i]["State"].ToString());
                }
                else
                    break;
            }
            //Convert to Array as We need to return Array
            prefixTextArray = ListStateNames.ToArray();//ToArray<string>();
        }
        //Return Selected Products
        return prefixTextArray;
    }

    //States List
    [WebMethod]
    public string[] GetCompletionCityList(string prefixText, int count)
    {
        string strQry = "Select inststate as state from InstituteDetails where inststate like '" + prefixText + "%'  and isactive=1" +
                         "union   select instcity as state from InstituteDetails Where instcity like '" + prefixText + "%' and isactive=1";
        DataTable dt = SqlHelper.Instance.GetTableByQuery(strQry);
        string[] prefixTextArray = null;
        if (dt.Rows.Count > 0)
        {
            // Get the Products From Data Source. Change this method to use Database
            List<string> productList = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < count)
                {
                    productList.Add(dt.Rows[i]["state"].ToString());
                }
                else
                    break;
            }

            //Convert to Array as We need to return Array
            prefixTextArray = productList.ToArray();//ToArray<string>();
        }
        //Return Selected Products
        return prefixTextArray;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetEmpID(string prefixText, int count,string BranchID)
    {

        string sqlqry = "Select empid from empdetails where empid like '%'+ '" + prefixText + "' +'%'  ";
        DataTable dt = SqlHelper.Instance.GetTableByQuery(sqlqry);


        string[] prefixTextArray = null;
        if (dt.Rows.Count > 0)
        {
            // Get the Products From Data Source. Change this method to use Database
            List<string> EmpID = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < count)
                {
                    EmpID.Add(dt.Rows[i]["empid"].ToString());
                }
                else
                    break;
            }
            //Convert to Array as We need to return Array
            prefixTextArray = EmpID.ToArray();//ToArray<string>();
        }
        //Return Selected Products
        return prefixTextArray;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetEmpName(string prefixText, int count, string BranchID)
    {

        string sqlqry = "Select (EmpFname+' '+EmpMName+' '+EmpLName) as EmpName  from empdetails where (EmpFname+' '+EmpMName+' '+EmpLName) like '%'+ '" + prefixText + "' +'%' ";
        DataTable dt = SqlHelper.Instance.GetTableByQuery(sqlqry);


        string[] prefixTextArray = null;
        if (dt.Rows.Count > 0)
        {
            // Get the Products From Data Source. Change this method to use Database
            List<string> EmpName = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < count)
                {
                    EmpName.Add(dt.Rows[i]["EmpName"].ToString());
                }
                else
                    break;
            }
            //Convert to Array as We need to return Array
            prefixTextArray = EmpName.ToArray();//ToArray<string>();
        }
        //Return Selected Products
        return prefixTextArray;
    }
  

    //Locations List
    [WebMethod]
    public string[] GetCompletionLocationList(string prefixText, int count)
    {
        string strQry = "Select distinct InstLocation as location from InstituteDetails where InstLocation like'" + prefixText + "%'  and isactive=1";
        DataTable dt = SqlHelper.Instance.GetTableByQuery(strQry);
        string[] prefixTextArray = null;
        if (dt.Rows.Count > 0)
        {
            // Get the Products From Data Source. Change this method to use Database
            List<string> productList = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < count)
                {
                    productList.Add(dt.Rows[i]["location"].ToString());
                }
                else
                    break;
            }

            //Convert to Array as We need to return Array
            prefixTextArray = productList.ToArray();
        }
        //Return Selected Products
        return prefixTextArray;
    }
    [WebMethod]
    public string[] GetCompletionCourses(string prefixText, int count)
    {
        string strQry = "Select Modulename as Course from Coursemodules where Modulename like '" + prefixText + "%'" +
                       " union  Select Coursename as course from courses Where coursename like '" + prefixText + "%'";
        DataTable dt = SqlHelper.Instance.GetTableByQuery(strQry);
        string[] prefixTextArray = null;
        if (dt.Rows.Count > 0)
        {
            // Get the Products From Data Source. Change this method to use Database
            List<string> productList = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < count)
                {
                    productList.Add(dt.Rows[i]["Course"].ToString());
                }
                else
                    break;
            }
            //Convert to Array as We need to return Array
            prefixTextArray = productList.ToArray();//ToArray<string>();
        }
        //Return Selected Products
        return prefixTextArray;
    }

    [WebMethod]
    public string[] GetIntitutelist(string prefixText, int count)
    {
        string strQry = " Select instname as Course from InstituteDetails Where instname like '" + prefixText + "%'  and isactive=1";
        DataTable dt = SqlHelper.Instance.GetTableByQuery(strQry);
        string[] prefixTextArray = null;
        if (dt.Rows.Count > 0)
        {
            // Get the Products From Data Source. Change this method to use Database
            List<string> productList = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < count)
                {
                    productList.Add(dt.Rows[i]["Course"].ToString());
                }
                else
                    break;
            }
            //Convert to Array as We need to return Array
            prefixTextArray = productList.ToArray();//ToArray<string>();
        }
        //Return Selected Products
        return prefixTextArray;
    }

    [WebMethod]
    public Item[] GetCloudItems()
    {
        Item [] items = null;
        string strQry = "Select TOP 2 Modulename,count from Coursemodules ORDER BY count desc";
        DataTable dt = SqlHelper.Instance.GetTableByQuery(strQry);
        if (dt.Rows.Count > 0)
        {
            items = new Item[dt.Rows.Count];
            string name;
            string cCount;
            int count = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                name = null;
                cCount = null;
                count = 0;
                name = dt.Rows[i]["Modulename"].ToString();
                cCount = dt.Rows[i]["count"].ToString();
                if(cCount.Length > 0)
                    count = Convert.ToInt32(cCount);
                Item item = new Item(name, count);
                items[i] = item;
            }
            return items;
        }
        else
            return items;
        //return new Item[]{
        //    new Item(".NET ",130,"www.microsoft.com") ,
        //    new Item("Accessibility ",126,"http://en.wikipedia.org/wiki/Accessibility") ,
        //    new Item("Ajax ",218,"http://en.wikipedia.org/wiki/Ajax") ,
        //    new Item("Articles ",193,"http://en.wikipedia.org/wiki/Articles") ,
        //    new Item("Book Reviews ",13,"http://en.wikipedia.org/wiki/Book_reviews") ,
        //    new Item("Books ",81,"http://en.wikipedia.org/wiki/Books") ,
        //    new Item("Browsers ",30,"http://en.wikipedia.org/wiki/Browsers") ,
        //    new Item("Builds ",31,"http://en.wikipedia.org/wiki/Builds") ,
        //    new Item("Email ",2,"http://en.wikipedia.org/wiki/Email"),
        //    new Item("Examples ",88,"http://en.wikipedia.org/wiki/Examples") ,
        //    new Item("Firefox ",18) ,
        //    new Item("Flash ",20) ,
        //    new Item("Framework ",2) ,
        //    new Item("Fun ",11) 			
        //    };
    }

}

public class Item
{
    public Item(string name, int weight)
    {
        this._name = name;
        this._weight = weight;
    }

    public Item(string name, int weight, string url)
    {
        this._name = name;
        this._weight = weight;
        this._url = url;
    }

    private string _name;

    public string Name
    {
        get { return _name; }
    }

    private int _weight;

    public int Weight
    {
        get { return _weight; }
    }

    private string _url;

    public string Url
    {
        get { return _url; }
    }
}