using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;

namespace Operator
{
    public class SqliteDbAdapter
    {
        #region Fields
        public const string DatabaseFileExtension = ".sqlite";
        internal const string TablePlaces = "places";
        internal const string TableProcs = "routines";
        protected string m_connectionString;
        protected SQLiteConnection m_connection;
        protected SQLiteTransaction m_transaction;
        protected int m_Timeout;
        //TODO: add new command here! Add code for new command to ClearCommand()!
        protected SQLiteCommand m_cmdAddPlace;
        protected SQLiteCommand m_cmdAddProcedure;
        #endregion

        public SqliteDbAdapter()
        {
            this.m_connection = (SQLiteConnection)null;
            this.m_transaction = (SQLiteTransaction)null;
            this.m_connectionString = string.Empty;
            this.m_Timeout = 60;
            this.ClearCommands();

            return;
        }

        ~SqliteDbAdapter()
        {
            this.Close();

            return;
        }

        #region Properties
        public int Timeout
        {
            get
            {
                return this.m_Timeout;
            }
            set
            {
                this.m_Timeout = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.m_connectionString;
            }
            set
            {
                this.m_connectionString = value;
                this.m_connection = new SQLiteConnection(this.m_connectionString);
            }
        }

        public bool isConnectionActive
        {
            get
            {
                return this.m_connection != null && ((DbConnection)this.m_connection).State == ConnectionState.Open;
            }
        }

        public bool isTransactionActive
        {
            get
            {
                return this.m_transaction != null;
            }
        }



        #endregion



        #region Service functions
        protected void ClearCommands()
        {
            //TODO: add code for new command here!
            this.m_cmdAddPlace = (SQLiteCommand)null;
            this.m_cmdAddProcedure = (SQLiteCommand)null;
        }

        public void Open(string connectionString)
        {
            this.m_connectionString = string.Copy(connectionString);
            this.Open();
        }

        public virtual void Open()
        {
            this.m_connection = new SQLiteConnection(this.m_connectionString);
            ((DbConnection)this.m_connection).Open();
        }

        public virtual void Close()
        {
            if (this.m_connection == null)
                return;
            if (((DbConnection)this.m_connection).State != ConnectionState.Closed)
                ((DbConnection)this.m_connection).Close();
            this.m_connection = (SQLiteConnection)null;
            this.ClearCommands();
        }

        public static string CreateConnectionString(string dbFile, bool readOnly)
        {
            SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder();
            connectionStringBuilder.DataSource = dbFile;
            connectionStringBuilder.ReadOnly = readOnly;
            connectionStringBuilder.FailIfMissing = true;
            return connectionStringBuilder.ConnectionString;
        }
        #endregion

        #region Transaction functions
        public void TransactionBegin()
        {
            this.m_transaction = this.m_connection.BeginTransaction();
            this.ClearCommands();
        }

        public void TransactionCommit()
        {
            ((DbTransaction)this.m_transaction).Commit();
            this.ClearCommands();
            this.m_transaction = (SQLiteTransaction)null;
        }

        public void TransactionRollback()
        {
            ((DbTransaction)this.m_transaction).Rollback();
            this.ClearCommands();
            this.m_transaction = (SQLiteTransaction)null;
        }
        #endregion


        #region General adapter functions
        public static void DatabaseCreate(string filename)
        {
            SQLiteConnection.CreateFile(filename);
        }

        public int ExecuteNonQuery(string query, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(query, this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            return ((DbCommand)sqLiteCommand).ExecuteNonQuery();
        }

        public SQLiteDataReader ExecuteReader(string query, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(query, this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            return sqLiteCommand.ExecuteReader();
        }

        public int ExecuteScalar(string query, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(query, this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            int num = -1;
            SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
            if (((DbDataReader)sqLiteDataReader).HasRows)
            {
                ((DbDataReader)sqLiteDataReader).Read();
                num = ((DbDataReader)sqLiteDataReader).GetInt32(0);
            }
            ((DbDataReader)sqLiteDataReader).Close();
            return num;
        }
        /// <summary>
        /// NT-Получить из ридера строку либо нуль как пустую строку.
        /// </summary>
        /// <param name="rdr">Объект ридера.</param>
        /// <param name="p">Индекс столбца для ридера</param>
        /// <returns></returns>
        public static string getDbString(SQLiteDataReader rdr, int p)
        {
            if (((DbDataReader)rdr).IsDBNull(p))
                return string.Empty;
            return ((DbDataReader)rdr).GetString(p).Trim();
        }


        /// <summary>
        /// NT-Удалить строки с указанным значением столбца из таблицы
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="val">Числовое значение столбца в таблице</param>
        /// <param name="timeout">Таймаут операции, в секундах</param>
        /// <returns>Функция возвращает число удаленных строк.</returns>
        /// <remarks>
        /// Эта универсальная функция позволяет удалить строку таблицы по значению одного из ее столбцов.
        /// Например, по ID записи.
        /// </remarks>
        public int DeleteRow(string table, string column, int val, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "DELETE FROM \"{0}\" WHERE (\"{1}\" = {2});", (object)table, (object)column, (object)val), this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            return ((DbCommand)sqLiteCommand).ExecuteNonQuery();
        }
        /// <summary>
        /// Получить максимальное значение поля столбца таблицы
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns>Возвращает максимальное значение ячеек столбца таблицы или -1.</returns>
        public int getTableMaxInt32(string table, string column, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "SELECT MAX(\"{0}\") FROM \"{1}\";", new object[2]
      {
        (object) column,
        (object) table
      }), this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            int num = -1;
            SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
            if (((DbDataReader)sqLiteDataReader).HasRows)
            {
                ((DbDataReader)sqLiteDataReader).Read();
                num = ((DbDataReader)sqLiteDataReader).GetInt32(0);
            }
            ((DbDataReader)sqLiteDataReader).Close();
            return num;
        }
        /// <summary>
        /// Получить минимальное значение поля столбца таблицы
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns>Возвращает минимальное значение ячеек столбца таблицы или -1.</returns>
        public int getTableMinInt32(string table, string column, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "SELECT MIN(\"{0}\") FROM \"{1}\";", new object[2]
      {
        (object) column,
        (object) table
      }), this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            int num = -1;
            SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
            if (((DbDataReader)sqLiteDataReader).HasRows)
            {
                ((DbDataReader)sqLiteDataReader).Read();
                num = ((DbDataReader)sqLiteDataReader).GetInt32(0);
            }
            ((DbDataReader)sqLiteDataReader).Close();
            return num;
        }
        /// <summary>
        /// Получить общее число записей в таблице
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns></returns>
        public int GetRowCount(string table, string column, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "SELECT COUNT(\"{0}\") FROM \"{1}\";", new object[2]
      {
        (object) column,
        (object) table
      }), this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            int num = -1;
            SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
            if (((DbDataReader)sqLiteDataReader).HasRows)
            {
                ((DbDataReader)sqLiteDataReader).Read();
                num = ((DbDataReader)sqLiteDataReader).GetInt32(0);
            }
            ((DbDataReader)sqLiteDataReader).Close();
            return num;
        }
        /// <summary>
        /// Получить число записей таблицы с указанным значением столбца
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="val"></param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns></returns>
        public int GetRowCount(string table, string column, int val, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "SELECT COUNT(\"{0}\") FROM \"{1}\" WHERE (\"{0}\" = {2});", (object)column, (object)table, (object)val), this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            int num = -1;
            SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
            if (((DbDataReader)sqLiteDataReader).HasRows)
            {
                ((DbDataReader)sqLiteDataReader).Read();
                num = ((DbDataReader)sqLiteDataReader).GetInt32(0);
            }
            ((DbDataReader)sqLiteDataReader).Close();
            return num;
        }
        /// <summary>
        /// Проверить существование в таблице записи с указанным идентификатором.
        /// </summary>
        /// <param name="tablename">Название таблицы</param>
        /// <param name="column">Название столбца идентификаторов записей таблицы</param>
        /// <param name="idValue">Идентификатор записи таблицы</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns></returns>
        public bool IsRowExists(string tablename, string column, int idValue, int timeout)
        {
            return this.GetRowCount(tablename, column, idValue, timeout) > 0;
        }
        /// <summary>
        /// NT-Очистить таблицу БД.
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        public void TableClear(string table, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "DELETE FROM {0};", new object[1]
      {
        (object) table
      }), this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            ((DbCommand)sqLiteCommand).ExecuteNonQuery();
        }
        #endregion

        #region Custom functions
        /// <summary>
        /// Создать таблицы БД
        /// </summary>
        public void CreateDatabaseTables()
        {
            using (SQLiteTransaction sqLiteTransaction = this.m_connection.BeginTransaction())
            {
                using (SQLiteCommand sqLiteCommand = new SQLiteCommand(this.m_connection))
                {
                    ((DbCommand)sqLiteCommand).CommandText = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "DROP TABLE IF EXISTS `{0}`;", new object[1]
          {
            (object) "places"
          });
                    ((DbCommand)sqLiteCommand).ExecuteNonQuery();
                    ((DbCommand)sqLiteCommand).CommandText = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "CREATE TABLE \"{0}\"(\"id\" Integer Primary Key Autoincrement,\"title\" Text,\"type\" Text,\"path\" Text,\"descr\" Text,\"syno\" Text);", new object[1]
          {
            (object) "places"
          });
                    ((DbCommand)sqLiteCommand).ExecuteNonQuery();
                    ((DbCommand)sqLiteCommand).CommandText = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "DROP TABLE IF EXISTS `{0}`;", new object[1]
          {
            (object) "routines"
          });
                    ((DbCommand)sqLiteCommand).ExecuteNonQuery();
                    ((DbCommand)sqLiteCommand).CommandText = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "CREATE TABLE \"{0}\"(\"id\" Integer Primary Key Autoincrement,\"title\" Text,\"ves\" Real,\"path\" Text,\"regex\" Text,\"descr\" Text);", new object[1]
          {
            (object) "routines"
          });
                    ((DbCommand)sqLiteCommand).ExecuteNonQuery();
                }
                ((DbTransaction)sqLiteTransaction).Commit();
            }
        }
        /// <summary>
        /// Получить все записи таблицы Places
        /// </summary>
        /// <returns></returns>
        public List<Place> GetAllPlaces()
        {
            List<Place> list = new List<Place>();
            SQLiteDataReader sqLiteDataReader = this.ExecuteReader(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "SELECT * FROM \"{0}\"", new object[1]
      {
        (object) "places"
      }), this.m_Timeout);
            if (((DbDataReader)sqLiteDataReader).HasRows)
            {
                while (((DbDataReader)sqLiteDataReader).Read())
                {
                    Place place = new Place();
                    place.TableId = ((DbDataReader)sqLiteDataReader).GetInt32(0);
                    place.Title = ((DbDataReader)sqLiteDataReader).GetString(1);
                    place.PlaceTypeExpression = ((DbDataReader)sqLiteDataReader).GetString(2);
                    place.Path = ((DbDataReader)sqLiteDataReader).GetString(3);
                    place.Description = ((DbDataReader)sqLiteDataReader).GetString(4);
                    place.Synonim = ((DbDataReader)sqLiteDataReader).GetString(5);
                    place.ParseEntityTypeString();
                    list.Add(place);
                }
            }
            ((DbDataReader)sqLiteDataReader).Close();
            return list;
        }
        /// <summary>
        /// Добавить Место в таблицу Мест
        /// </summary>
        /// <param name="p"></param>
        public void AddPlace(Place p)
        {
            SQLiteCommand sqLiteCommand = this.m_cmdAddPlace;
            if (sqLiteCommand == null)
            {
                sqLiteCommand = new SQLiteCommand(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "INSERT INTO \"{0}\"(\"title\", \"type\", \"path\", \"descr\", \"syno\") VALUES (?,?,?,?,?);", new object[1]
        {
          (object) "places"
        }), this.m_connection, this.m_transaction);
                ((DbCommand)sqLiteCommand).CommandTimeout = this.m_Timeout;
                sqLiteCommand.Parameters.Add("a0", DbType.String);
                sqLiteCommand.Parameters.Add("a1", DbType.String);
                sqLiteCommand.Parameters.Add("a2", DbType.String);
                sqLiteCommand.Parameters.Add("a3", DbType.String);
                sqLiteCommand.Parameters.Add("a4", DbType.String);
                this.m_cmdAddPlace = sqLiteCommand;
            }
            ((DbParameter)sqLiteCommand.Parameters[0]).Value = (object)p.Title;
            ((DbParameter)sqLiteCommand.Parameters[1]).Value = (object)p.PlaceTypeExpression;
            ((DbParameter)sqLiteCommand.Parameters[2]).Value = (object)p.Path;
            ((DbParameter)sqLiteCommand.Parameters[3]).Value = (object)p.Description;
            ((DbParameter)sqLiteCommand.Parameters[4]).Value = (object)p.Synonim;
            ((DbCommand)sqLiteCommand).ExecuteNonQuery();
        }
        /// <summary>
        /// Удалить Место по ид.
        /// </summary>
        /// <param name="placeId"></param>
        /// <returns></returns>
        public int RemovePlace(int placeId)
        {
            return this.DeleteRow("places", "id", placeId, this.m_Timeout);
        }
        /// <summary>
        /// Получить все записи таблицы Procedures
        /// </summary>
        /// <returns></returns>
        public List<Procedure> GetAllProcedures()
        {
            List<Procedure> list = new List<Procedure>();
            SQLiteDataReader sqLiteDataReader = this.ExecuteReader(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "SELECT * FROM \"{0}\"", new object[1]
      {
        (object) "routines"
      }), this.m_Timeout);
            if (((DbDataReader)sqLiteDataReader).HasRows)
            {
                while (((DbDataReader)sqLiteDataReader).Read())
                {
                    Procedure procedure = new Procedure();
                    procedure.TableId = ((DbDataReader)sqLiteDataReader).GetInt32(0);
                    procedure.Title = ((DbDataReader)sqLiteDataReader).GetString(1);
                    procedure.Ves = ((DbDataReader)sqLiteDataReader).GetDouble(2);
                    procedure.Path = ((DbDataReader)sqLiteDataReader).GetString(3);
                    procedure.Regex = ((DbDataReader)sqLiteDataReader).GetString(4);
                    procedure.Description = ((DbDataReader)sqLiteDataReader).GetString(5);
                    list.Add(procedure);
                }
            }
            ((DbDataReader)sqLiteDataReader).Close();
            return list;
        }
        /// <summary>
        /// Добавить Процедуру
        /// </summary>
        /// <param name="p"></param>
        public void AddProcedure(Procedure p)
        {
            SQLiteCommand sqLiteCommand = this.m_cmdAddProcedure;
            if (sqLiteCommand == null)
            {
                sqLiteCommand = new SQLiteCommand(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "INSERT INTO \"{0}\"(\"title\", \"ves\", \"path\", \"regex\", \"descr\") VALUES (?,?,?,?,?);", new object[1]
        {
          (object) "routines"
        }), this.m_connection, this.m_transaction);
                ((DbCommand)sqLiteCommand).CommandTimeout = this.m_Timeout;
                sqLiteCommand.Parameters.Add("a0", DbType.String);
                sqLiteCommand.Parameters.Add("a1", DbType.Double);
                sqLiteCommand.Parameters.Add("a2", DbType.String);
                sqLiteCommand.Parameters.Add("a3", DbType.String);
                sqLiteCommand.Parameters.Add("a4", DbType.String);
                this.m_cmdAddProcedure = sqLiteCommand;
            }
            ((DbParameter)sqLiteCommand.Parameters[0]).Value = (object)p.Title;
            ((DbParameter)sqLiteCommand.Parameters[1]).Value = (object)p.Ves;
            ((DbParameter)sqLiteCommand.Parameters[2]).Value = (object)p.Path;
            ((DbParameter)sqLiteCommand.Parameters[3]).Value = (object)p.Regex;
            ((DbParameter)sqLiteCommand.Parameters[4]).Value = (object)p.Description;
            ((DbCommand)sqLiteCommand).ExecuteNonQuery();
        }
        /// <summary>
        /// Удалить Процедуру.
        /// </summary>
        /// <param name="placeId"></param>
        /// <returns></returns>
        public int RemoveProcedure(int placeId)
        {
            return this.DeleteRow("routines", "id", placeId, this.m_Timeout);
        }
        #endregion
    }
}
