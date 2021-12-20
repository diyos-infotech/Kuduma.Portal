
using System.Data;


namespace Kuduma.Portal.DAL
{
    public class MenuBAL
    {
        MenuDAL Dalobj = new MenuDAL();

        public MenuBAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataSet CheckPrevileges(int obj)
        {
            DataSet ds = new DataSet();
            ds = Dalobj.CheckPrevileges(obj);
            return ds;
        }

        public DataSet GetAllPrevileges(string obj)
        {
            DataSet ds = new DataSet();
            ds = Dalobj.GetAllPrevileges(obj);
            return ds;
        }

        public DataSet GetAllFolderMenus(string obj, int id)
        {
            DataSet ds = new DataSet();
            ds = Dalobj.GetAllFolderMenus(obj, id);
            return ds;
        }

        public int updatecolumnposition(string qry)
        {
            int ds;
            ds = Dalobj.updatecolumnposition(qry);
            return ds;
        }

        public int UpdateMenuPrevilege(string PID, string mid, int bit)
        {
            int ds;
            ds = Dalobj.UpdateMenuPrevilege(PID, mid, bit);
            return ds;
        }

        public int ChecKSession(string PID, string mid, string bit)
        {
            int ds;
            ds = Dalobj.ChecKSession(PID, mid, bit);
            return ds;
        }

        public int AddMenu(string mtext, string parent, string rpage, string user, string path, string desc)
        {
            int ds;
            ds = Dalobj.AddMenu(mtext, rpage, parent, user, path, desc);
            return ds;
        }

        public int AddFolderMenu(string Parentpage, string Fname, int parent, string menu, string user)
        {
            int ds;
            ds = Dalobj.AddFolderMenu(Parentpage, Fname, parent, menu, user);
            return ds;
        }

        public int DeleteFolderMenu(int Fid, string URL)
        {
            int ds;
            ds = Dalobj.DeleteFolderMenu(Fid, URL);
            return ds;
        }

        public int AddAliasField(string tbl, string col, string aname, string user, int bit, int pos, string Oldcol, char tp)
        {
            int ds;
            ds = Dalobj.AddAliasField(tbl, col, aname, user, bit, pos, Oldcol, tp);
            return ds;
        }

        public int UpdateAliasField(string tbl, string col, string aname, string user, int bit, string tp)
        {
            int ds;
            ds = Dalobj.UpdateAliasField(tbl, col, aname, user, bit, tp);
            return ds;
        }
    }
}