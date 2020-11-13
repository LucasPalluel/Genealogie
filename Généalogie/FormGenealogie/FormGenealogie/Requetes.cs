using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FormGenealogie
{
    unsafe public class Requetes
    {




        private static string connstr =

          "Provider = sqloledb; " +

          "Data Source = STR-SIOCR; " +

          "Initial Catalog = Genealogie_Palluel;" +          

          "User Id =sio; " +

          "Password=slam;";

       

        public OleDbConnection oconn = new OleDbConnection(connstr);



        public bool OuvreLaBase()
        {
            bool ok = true;
            try
            {
                oconn.Open();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
                ok = false;
            }
            return ok;
        }

        public void FermeLaBase()
        {
            try
            {
                oconn.Close();
            }
            catch (Exception exc)
            {
                //Program.AfficheMessage("Requetes", "FermeLaBase", "Erreur d'ouverture de la base", exc.Message.ToString());
            }
        }

        public OleDbDataReader Extraction(string strSql, string ordre)
        {
            OleDbCommand oCmd = new OleDbCommand(strSql, oconn);
            oCmd.CommandText = strSql;
            oCmd.CommandType = CommandType.Text;
            OleDbDataReader oDr = null;

            try
            {
                oDr = oCmd.ExecuteReader();
            }
            catch (Exception exc)
            {
                //Program.AfficheMessage("Requetes", "Extraction", "", exc.Message.ToString());
            }
            return oDr;
        }

        public int ExtractionSimple(string strSql, string ordre, string type)
        {
            int n = -1;
            OleDbCommand oCmd = new OleDbCommand(strSql, oconn);
            oCmd.CommandText = strSql;
            oCmd.CommandType = CommandType.Text;
            try
            {
                n = Convert.ToInt32(oCmd.ExecuteScalar());
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }
            return n;
        }

        public int Insertion(string strSql, string ordre = "Aucun", string type = "Aucun")
        {
            int n = -1;
            ExecuteRequete(strSql);
            return n;
        }

        public int Modification(string strSql)
        {
            int n = -1;
            ExecuteRequete(strSql);
            return n;
        }

        public int Suppression(string strSql)
        {
            int n = -1;
            ExecuteRequete(strSql);
            return n;
        }

        public int ExecuteRequete(string strSql)
        {
            int n = -1;
            OleDbCommand oCmd = new OleDbCommand(strSql, oconn);
            oCmd.CommandText = strSql;
            oCmd.CommandType = CommandType.Text;

            try
            {
                n = oCmd.ExecuteNonQuery();
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }
            return n;
        }

        public OleDbDataReader RequeteStockee(string strSql)
        {
            OleDbCommand oCmd = new OleDbCommand(strSql, oconn);
            oCmd.CommandText = strSql;
            oCmd.CommandType = CommandType.StoredProcedure;
            OleDbDataReader oDr = null;

            try
            {
                oDr = oCmd.ExecuteReader();
            }
            catch (Exception exc)
            {
                //Program.AfficheMessage("Requetes", "RequeteStockee", "", exc.Message.ToString());
            }
            return oDr;
        }
    }
}