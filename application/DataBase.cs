using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Windows.Forms;

namespace DbGui
{
    public class DataBase
    {
        public NpgsqlConnection conn;

        public DataBase(string server,string dbase,string uname,string passw)
        {
            string launcher = "";
            launcher += "Server=" + server + ";";
            launcher += "User Id=" + uname + ";";
            launcher += "Password=" + passw + ";";
            launcher += "Database=" + dbase + ";";
            conn = new NpgsqlConnection(launcher);
            try
            {
                conn.Open();
            }
            catch (NpgsqlException e)
            {
                MessageBox.Show("Błąd: " + e.BaseMessage);
            }
        }
        public bool isOpen()
        {
            return (conn.State == System.Data.ConnectionState.Open);
        }
        public void close()
        {
            conn.Close();
        }
        ~DataBase()
        {
            conn.Close();
        }
        public NpgsqlDataReader executeQuery(ExecutableQuery query)
        {
            NpgsqlCommand command = new NpgsqlCommand(query.getQuery(), conn);
            NpgsqlDataReader dr = null;
            try
            {
                dr = command.ExecuteReader();
            }
            catch (NpgsqlException e)
            {
                //MessageBox.Show("Błąd: " + e.BaseMessage + "\n\n" + query.getQuery(), "ErrorMessage");
                MessageBox.Show("Niepoprawne dane.");
            }
            return dr;
        }
        public bool executeInsert(ExecutableQuery query)
        {
            NpgsqlCommand command = new NpgsqlCommand(query.getQuery(), conn);
            int status = 0;
            bool result = false;
            try
            {
                status = command.ExecuteNonQuery();
            }
            catch (NpgsqlException e)
            {
                //MessageBox.Show("Błąd: " + e.BaseMessage + "\n\n" + query.getQuery(), "ErrorMessage");
                MessageBox.Show("Niepoprawna operacja.");
            }
            finally
            {
                result = (status > 0);
            }
            return result;
        }
        public int executeFunction(string query)
        {
            NpgsqlCommand command = new NpgsqlCommand(query, conn);
            int result = 0;
            try
            {
                result = (int)command.ExecuteScalar();
            }
            catch (NpgsqlException e)
            {
                //MessageBox.Show("Błąd: " + e.BaseMessage + "\n\n" + query, "ErrorMessage");
                MessageBox.Show("Niepoprawne dane.");
            }
            return result;
        }
        public void executeProcedure(string query)
        {
            NpgsqlCommand command = new NpgsqlCommand(query, conn);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (NpgsqlException e)
            {
                //MessageBox.Show("Błąd: " + e.BaseMessage + "\n\n" + query, "ErrorMessage");
                MessageBox.Show("Błąd synchronizacji.");
            }
        }

    }
}