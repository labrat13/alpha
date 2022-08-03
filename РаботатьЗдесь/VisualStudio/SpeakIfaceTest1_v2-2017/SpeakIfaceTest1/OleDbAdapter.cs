using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

namespace Operator
{
    //некоторые функции тут помечены как virtual, чтобы в производном классе их пометить override.
    //тогда при вызове функций внутри кода функций базового класса вызываются переопределенные функции из производного класса.
    //Поэтому, в частности, OleDbAdapter.Open(connectionString) вызывает CachedDbAdapter.Open(), а не DbAdapter.Open() как это следует из кода здесь.
    //Это такая неочевидная вещь, которую надо учитывать.
    
    public class OleDbAdapter
    {
        private String m_conString;

        private OleDbConnection m_connection;

        private OleDbCommand m_cmdGetAllPlaces;
        private OleDbCommand m_cmdAddPlace;

        private OleDbCommand m_cmdGetAllProcedures;
        private OleDbCommand m_cmdAddProcedure;
        public OleDbAdapter()
        {
            m_connection = null;
            m_cmdGetAllPlaces = null;
        }
        /// <summary>
        /// NT-Открыть новое соединение
        /// </summary>
        /// <param name="connectionString">Строка соединения</param>
        public void Open(String connectionString)
        {
            m_conString = String.Copy(connectionString);
            Open();
        }
        /// <summary>
        /// NT-Открыть ранее закрытое соединение
        /// </summary>
        public virtual void Open()
        {
            m_connection = new OleDbConnection(m_conString);
            m_connection.Open();
        }
        /// <summary>
        /// NT-Закрыть текущее соединение
        /// </summary>
        public virtual void Close()
        {
            if(m_connection != null)
                if (m_connection.State != System.Data.ConnectionState.Closed)
                {
                    m_connection.Close();
                    //clear all commands
                    m_connection = null;
                    m_cmdGetAllPlaces = null;
                    m_cmdAddPlace = null;
                    m_cmdGetAllProcedures = null;
                    m_cmdAddProcedure = null;
                }
        }
        /// <summary>
        /// NT-Создать строку соединения
        /// </summary>
        /// <param name="dbfile"></param>
        /// <returns></returns>
        public static string CreateConnectionString(String dbfile)
        {
            OleDbConnectionStringBuilder b = new OleDbConnectionStringBuilder();
            b.Provider = "Microsoft.Jet.OLEDB.4.0";
            b.DataSource = dbfile;
            return b.ConnectionString;
        }

#region *** Функции Таблицы Мест ***
        /// <summary>
        /// NT-Получить все места из таблицы мест
        /// </summary>
        /// <returns></returns>
        public List<Place> GetAllPlaces()
        {
            OleDbCommand cmd = m_cmdGetAllPlaces; //for copypasting
            if (cmd == null)
            {
                String query = "SELECT * FROM `places`";
                cmd = new OleDbCommand(query, m_connection);

                m_cmdGetAllPlaces = cmd;//for copypasting
            }
            //execute cmd
            List<Place> pl = new List<Place>();
            OleDbDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Place p = new Place();
                    p.TableId = rdr.GetInt32(0);
                    p.Title = rdr.GetString(1);
                    p.PlaceTypeExpression = rdr.GetString(2);
                    p.Path = rdr.GetString(3);
                    p.Description = rdr.GetString(4);
                    p.Synonim = rdr.GetString(5);
                    //TODO: remove this stuff after optimizing
                    p.ParseEntityTypeString();
                    pl.Add(p);
                }
            }
            rdr.Close();
            //return 
            return pl;
        }
        /// <summary>
        /// NT-Добавить новое место
        /// </summary>
        /// <param name="p">Добавляемое место</param>
        public void AddPlace(Place p)
        {
            OleDbCommand cmd = m_cmdAddPlace; //for copypasting
            if (cmd == null)
            {
                String query = "INSERT INTO `places`(`title`, `type`, `path`, `descr`, `syno`) VALUES (?,?,?,?,?);";
                cmd = new OleDbCommand(query, m_connection);
                cmd.Parameters.Add(new OleDbParameter("@a0", OleDbType.VarWChar));
                cmd.Parameters.Add(new OleDbParameter("@a1", OleDbType.VarWChar));
                cmd.Parameters.Add(new OleDbParameter("@a2", OleDbType.VarWChar));
                cmd.Parameters.Add(new OleDbParameter("@a3", OleDbType.VarWChar));
                cmd.Parameters.Add(new OleDbParameter("@a4", OleDbType.VarWChar));

                m_cmdAddPlace = cmd;//for copypasting
            }
            //execute
            cmd.Parameters[0].Value = p.Title;
            cmd.Parameters[1].Value = p.PlaceTypeExpression;
            cmd.Parameters[2].Value = p.Path;
            cmd.Parameters[3].Value = p.Description;
            cmd.Parameters[4].Value = p.Synonim;

            cmd.ExecuteNonQuery();
            return;
        }
#endregion

#region *** Функции Таблицы Процедур ***
        /// <summary>
        /// NT-Получить все процедуры из таблицы процедур
        /// </summary>
        /// <returns></returns>
        public List<Procedure> GetAllProcedures()
        {
            OleDbCommand cmd = m_cmdGetAllProcedures; //for copypasting
            if (cmd == null)
            {
                String query = "SELECT * FROM `routines`";
                cmd = new OleDbCommand(query, m_connection);

                m_cmdGetAllProcedures = cmd;//for copypasting
            }
            //execute cmd
            List<Procedure> pl = new List<Procedure>();
            OleDbDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Procedure p = new Procedure();
                    p.TableId = rdr.GetInt32(0);
                    p.Title = rdr.GetString(1);
                    p.Ves = rdr.GetDouble(2);
                    p.Path = rdr.GetString(3);
                    p.Regex = rdr.GetString(4);
                    p.Description = rdr.GetString(5);
                    pl.Add(p);
                }
            }
            rdr.Close();
            //return 
            return pl;
        }
        /// <summary>
        /// NT-Добавить новую процедуру
        /// </summary>
        /// <param name="p">Добавляемая процедура</param>
        public void AddProcedure(Procedure p)
        {
            OleDbCommand cmd = m_cmdAddProcedure; //for copypasting
            if (cmd == null)
            {
                String query = "INSERT INTO `routines`(`title`, `ves`, `path`, `regex`, `descr`) VALUES (?,?,?,?,?);";
                cmd = new OleDbCommand(query, m_connection);
                cmd.Parameters.Add(new OleDbParameter("@a0", OleDbType.VarWChar));
                cmd.Parameters.Add(new OleDbParameter("@a1", OleDbType.Double));
                cmd.Parameters.Add(new OleDbParameter("@a2", OleDbType.VarWChar));
                cmd.Parameters.Add(new OleDbParameter("@a3", OleDbType.VarWChar));
                cmd.Parameters.Add(new OleDbParameter("@a4", OleDbType.VarWChar));
                m_cmdAddProcedure = cmd;//for copypasting
            }
            //execute
            cmd.Parameters[0].Value = p.Title;
            cmd.Parameters[1].Value = p.Ves;
            cmd.Parameters[2].Value = p.Path;
            cmd.Parameters[3].Value = p.Regex;
            cmd.Parameters[4].Value = p.Description;

            cmd.ExecuteNonQuery();
            return;
        }
#endregion



    }
}
