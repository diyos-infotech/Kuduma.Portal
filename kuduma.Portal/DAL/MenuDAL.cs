using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Kuduma.Portal.DAL
{
    public class MenuDAL
    {
        string str = ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString;

        public MenuDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int AddMenu(string MenuTxt, string Rpage, string parent, string user, string path, string desc)
        {
            SqlParameter[] p = new SqlParameter[6];
            p[0] = new SqlParameter("@MenuTxt", MenuTxt);
            p[1] = new SqlParameter("@Rpage", Rpage);
            p[2] = new SqlParameter("@PID", parent);
            p[3] = new SqlParameter("@user", user);
            p[4] = new SqlParameter("@path", path);
            p[5] = new SqlParameter("@mdesc", desc);
            int res = DatabaseMethods.ExecuteNonquery(DatabaseMethods.conection, CommandType.StoredProcedure, "[AddMenus]", p);
            return res;
        }

        public int ChecKSession(string Uid, string Sid, string Chk)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("CheckUserSession", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UID", Uid);
                    cmd.Parameters.AddWithValue("@SID", Sid);
                    cmd.Parameters.AddWithValue("@chk", Chk);
                    cmd.Parameters.Add("@ret", SqlDbType.Int);
                    cmd.Parameters["@ret"].Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    i = Convert.ToInt32(cmd.Parameters["@ret"].Value);
                }
            }
            return i;
        }

        public DataSet CheckPrevileges(int PID)
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(str);
            string query = "SELECT     MENU_PREVILIGE.previligeid, MENU_PREVILIGE.Menu_ID," +
                           "MENU_PREVILIGE.Access, MENU.REDIRECT_PAGE " +
                           "FROM         MENU_PREVILIGE INNER JOIN " +
                           "MENU ON MENU_PREVILIGE.Menu_ID = MENU.MENU_ID where Access = 0 and previligeid = " + PID;
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            adpt.Fill(ds);
            return ds;
        }

        public DataSet GetAllPrevileges(string PID)
        {

            DataSet ds = new DataSet();
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@PreviligerName", PID);
            ds = DatabaseMethods.ExecuteDataset(DatabaseMethods.conection, CommandType.StoredProcedure, "[GetAllPreviligers]", p);
            return ds;
        }

        public DataSet GetAllFolderMenus(string page, int pid)
        {

            DataSet ds = new DataSet();
            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("@page", page);
            p[1] = new SqlParameter("@previlegId", pid);
            ds = DatabaseMethods.ExecuteDataset(DatabaseMethods.conection, CommandType.StoredProcedure, "[GetFolderNdMenus]", p);
            return ds;
        }

        public int UpdateMenuPrevilege(string PID, string mid, int bit)
        {
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@PREVILEGE", PID);
            p[1] = new SqlParameter("@ACCESS", bit);
            p[2] = new SqlParameter("@MENU", mid);
            int res = DatabaseMethods.ExecuteNonquery(DatabaseMethods.conection, CommandType.StoredProcedure, "[updatePreviligers]", p);
            return res;
        }

        public int AddFolderMenu(string Parentpage, string Fname, int parent, string menu, string user)
        {
            SqlParameter[] p = new SqlParameter[5];
            p[0] = new SqlParameter("@ParentPage", Parentpage);
            p[1] = new SqlParameter("@FolderName", Fname);
            p[2] = new SqlParameter("@Parent", parent);
            p[3] = new SqlParameter("@Menu", menu);
            p[4] = new SqlParameter("@User", user);
            int res = DatabaseMethods.ExecuteNonquery(DatabaseMethods.conection, CommandType.StoredProcedure, "[AddFolderMenus]", p);
            return res;
        }

        public int updatecolumnposition(string qry)
        {
            SqlConnection con = new SqlConnection(str);
            SqlCommand cmd1 = new SqlCommand(qry, con);
            con.Open();
            int i = cmd1.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int DeleteFolderMenu(int Fid, string URL)
        {
            SqlParameter[] p = new SqlParameter[2];
            p[0] = new SqlParameter("@FID", Fid);
            p[1] = new SqlParameter("@URL", URL);

            int res = DatabaseMethods.ExecuteNonquery(DatabaseMethods.conection, CommandType.StoredProcedure, "[DeleteolderMenus]", p);
            return res;
        }

        public int AddAliasField(string tbl, string col, string aname, string user, int bit, int pos, string oldcol, char tp)
        {
            SqlParameter[] p = new SqlParameter[8];
            p[0] = new SqlParameter("@Table", tbl);
            p[1] = new SqlParameter("@Col", col);
            p[2] = new SqlParameter("@Alias", aname);
            p[3] = new SqlParameter("@user", user);
            p[4] = new SqlParameter("@Isvis", bit);
            p[5] = new SqlParameter("@ColPos", pos);
            p[6] = new SqlParameter("@OldCo", oldcol);
            p[7] = new SqlParameter("@Tp", tp);
            int res = DatabaseMethods.ExecuteNonquery(DatabaseMethods.conection, CommandType.StoredProcedure, "[AddAliasTableField]", p);
            return res;
        }

        public int UpdateAliasField(string tbl, string col, string aname, string user, int bit, string tp)
        {
            SqlParameter[] p = new SqlParameter[6];
            p[0] = new SqlParameter("@Table", tbl);
            p[1] = new SqlParameter("@Col", col);
            p[2] = new SqlParameter("@Alias", aname);
            p[3] = new SqlParameter("@user", user);
            p[4] = new SqlParameter("@Isvis", bit);
            p[5] = new SqlParameter("@TP", tp);
            int res = DatabaseMethods.ExecuteNonquery(DatabaseMethods.conection, CommandType.StoredProcedure, "[UpdateAliasTableField]", p);
            return res;
        }
    }
}