using System;
using System.Data.Common;
using System.Globalization;

namespace Engine.DbSubsystem
{
    /// <summary>
    /// NR-Общая версия адаптера для БД sqlite3
    /// </summary>
    public class SqliteDbAdapter
    {
        #region *** Fields ***

        /// <summary>
        /// Database file connection string
        /// </summary>
        protected String m_connectionString;

        /// <summary>
        /// Database connection object
        /// </summary>
        protected SQLiteConnection m_connection;

        /// <summary>
        /// Database transaction object
        /// </summary>
        protected SQLiteTransaction m_transaction;

        /// <summary>
        /// Command execution default timeout
        /// </summary>
        protected int m_Timeout;

        #endregion


        /// <summary>
        /// NT-Default constructor
        /// </summary>
        /// <exception cref="Exception">Error on database access occured.</exception>
        public SqliteDbAdapter()
        {

            this.m_connection = null;
            this.m_transaction = null;
            this.m_connectionString = string.Empty;
            this.m_Timeout = 60;
            this.ClearCommands();

            return;
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~SqliteDbAdapter()
        {
            this.Close();

            return;
        }

        #region Properties
        /// <summary>
        /// Default connection timeout, in seconds
        /// </summary>
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

        /// <summary>
        /// NT-Current connection string value.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.m_connectionString;
            }
            //set
            //{
            //    this.m_connectionString = value;
            //    this.m_connection = new SQLiteConnection(this.m_connectionString);
            //}
        }

        /// <summary>
        /// NT-Is database connection active
        /// </summary>
        public bool isConnectionActive
        {
            get
            {
                //return this.m_connection != null && ((DbConnection)this.m_connection).State == ConnectionState.Open;
                bool result = false;
                try
                {
                    if (this.m_connection != null)
                        result = this.m_connection.isValid(m_Timeout);
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }

        /// <summary>
        /// NT - Is database transaction active
        /// </summary>
        public bool isTransactionActive
        {
            get
            {
                return this.m_transaction != null;
            }
        }
        #endregion



        #region *** Service functions ***

        /// <summary>
        /// NT-Clear all member commands
        /// </summary>
        protected virtual void ClearCommands()
        {
            //необходимо переопределить эту функцию в производном классе и добавить в нее обнуление объектов команд.
            //В этом классе таких объектов команд нет, поэтому тут она пустая.
            throw new Exception("Function not implemented in this class.");
        }

        /// <summary>
        /// NT-Open connection to database.
        /// </summary>
        /// <remarks>
        /// Specified connection string will be stored inside this object for next using.
        /// </remarks>
        /// <param name="connectionString">Connection string for database</param>
        public void Open(String connectionString)
        {
            this.m_connectionString = string.Copy(connectionString);
            this.Open();

            return;
        }

        /// <summary>
        /// NT-Open connection to database, using previous stored connection string.
        /// </summary>
        /// <remarks>
        /// If connection already opened then return.
        /// </remarks>
        public virtual void Open()
        {
            // Пропустить, если менеджер уже открыт и соединение активно
            if (this.isConnectionActive)
                return;

            this.m_connection = new SQLiteConnection(this.m_connectionString);
            ((DbConnection)this.m_connection).Open();

            //обнулить объекты команд, если это не первое открытие менеджера
            this.ClearCommands();

            return;
        }


        /// <summary>
        /// NT- Close connection and reset all resources to initial state.
        /// </summary>
        /// <remarks>
        /// Connection string not cleared.
        /// </remarks>
        public virtual void Close()
        {
            if (this.m_connection == null)
                return;
            if (((DbConnection)this.m_connection).State != ConnectionState.Closed)
                ((DbConnection)this.m_connection).Close();
            this.m_connection = (SQLiteConnection)null;
            //обнулить объекты команд перед закрытием менеджера
            this.ClearCommands();

            return;
        }

        /// <summary>
        /// NT-Create connection string
        /// </summary>
        /// <param name="dbFile">Database file pathname string</param>
        /// <param name="readOnly">ReadOnly flag</param>
        /// <returns>Returns connection string</returns>
        public static string CreateConnectionString(string dbFile, bool readOnly)
        {
            SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder();
            connectionStringBuilder.DataSource = dbFile;
            connectionStringBuilder.ReadOnly = readOnly;
            connectionStringBuilder.FailIfMissing = true;
            return connectionStringBuilder.ConnectionString;
        }



        /// <summary>
        /// NT-Get string representation of object.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            bool active = this.isConnectionActive;
            String cstr = Utility.StringUtility.GetStringTextNull(this.m_connectionString);
            String result = String.Format("SqliteDbAdapter; connection=\"{0}\"; active={1}", cstr, active);

            return result;
        }

        #endregion
        #region *** Transaction functions ***

        /// <summary>
        /// NT-Transaction begins on first query.
        /// </summary>        
        public void TransactionBegin()
        {
            this.m_transaction = this.m_connection.BeginTransaction();
            this.ClearCommands();
        }

        /// <summary>
        /// NT-Commit current transaction. Connection must be closed after commit.
        /// </summary>
        public void TransactionCommit()
        {
            ((DbTransaction)this.m_transaction).Commit();
            this.ClearCommands();
            this.m_transaction = (SQLiteTransaction)null;
        }

        /// <summary>
        /// NT-Rollback current transaction. Connection must be closed after rollback.
        /// </summary>
        public void TransactionRollback()
        {
            ((DbTransaction)this.m_transaction).Rollback();
            this.ClearCommands();
            this.m_transaction = (SQLiteTransaction)null;
        }

        #endregion

        #region *** General adapter functions *** 
        /// <summary>
        /// NT-Create new database file
        /// </summary>
        /// <param name="filename">File pathname for new database file.</param>
        public static void DatabaseCreate(string filename)
        {
            SQLiteConnection.CreateFile(filename);
        }

        /// <summary>
        /// NT- Execute INSERT UPDATE DELETE query
        /// </summary>
        /// <param name="query">SQL query string</param>
        /// <param name="timeout">Execution timeout in seconds.</param>
        /// <returns>Returns number of changed rows or 0 if no changes.</returns>
        public int ExecuteNonQuery(string query, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(query, this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            return ((DbCommand)sqLiteCommand).ExecuteNonQuery();
        }

        /// <summary>
        /// NT-Execute SELECT query without arguments.
        /// Caller must close Reader after reading result set.
        /// </summary>
        /// <param name="query">SQL query string</param>
        /// <param name="timeout">Execution timeout in seconds.</param>
        /// <returns>Returns ResultSet for this query.</returns>
        public SQLiteDataReader ExecuteReader(string query, int timeout)
        {
            SQLiteCommand sqLiteCommand = new SQLiteCommand(query, this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            return sqLiteCommand.ExecuteReader();
        }

        /// <summary>
        /// NT-Execute SELECT query without arguments.
        /// Returns result in first row first column as int.
        /// </summary>
        /// <param name="query">SQL query string</param>
        /// <param name="timeout">Execution timeout in seconds.</param>
        /// <returns>Returns result in first row first column as int. Returns -1 if errors.</returns>
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
        /// <param name="index">Индекс столбца для ридера</param>
        /// <returns>Функция возвращает строку или пустую строку, если исходное значение было null.</returns>
        public static string getDbString(SQLiteDataReader rdr, int index)
        {
            if (((DbDataReader)rdr).IsDBNull(index))
                return string.Empty;
            return ((DbDataReader)rdr).GetString(index).Trim();
        }


        /// <summary>
        /// NT-Get last used rowid for table.
        /// </summary>
        /// <param name="table">Название таблицы.</param>
        /// <param name="timeout">Таймаут операции, в секундах.</param>
        /// <returns>Returns last used rowid for table.</returns>        
        public int getLastRowId(String table, int timeout)
        {
            String query = String.Format("SELECT \"seq\" FROM \"sqlite_sequence\" WHERE \"name\" = \"{0}\";", table);
            return this.ExecuteScalar(query, timeout);
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
            String query = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "DELETE FROM \"{0}\" WHERE (\"{1}\" = {2});", (object)table, (object)column, (object)val);
            SQLiteCommand sqLiteCommand = new SQLiteCommand(query, this.m_connection, this.m_transaction);
            ((DbCommand)sqLiteCommand).CommandTimeout = timeout;
            return ((DbCommand)sqLiteCommand).ExecuteNonQuery();
        }

        /// <summary>
        /// NT-Получить максимальное значение поля столбца таблицы
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns>Возвращает максимальное значение ячеек столбца таблицы или -1.</returns>
        public int getTableMaxInt32(string table, string column, int timeout)
        {
            //TODO: remove this comments after function testing
            // SQLiteCommand sqLiteCommand = new
            // SQLiteCommand(String.Format((IFormatProvider)
            // CultureInfo.InvariantCulture, "SELECT MAX(\"{0}\") FROM \"{1}\";",
            // new object[2] {
            // (object) column,
            // (object) table
            // }), this.m_connection, this.m_transaction);
            // ((DbCommand) sqLiteCommand).CommandTimeout = timeout;
            // int num = -1;
            // SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
            // if (((DbDataReader) sqLiteDataReader).HasRows)
            // {
            // ((DbDataReader) sqLiteDataReader).Read();
            // num = ((DbDataReader) sqLiteDataReader).GetInt32(0);
            // }
            // ((DbDataReader) sqLiteDataReader).Close();
            // return num;

            String query = String.Format("SELECT MAX(\"{0}\") FROM \"{1}\";", column, table);
            return this.ExecuteScalar(query, timeout);
        }

        /// <summary>
        /// NT-Получить минимальное значение поля столбца таблицы
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns>Возвращает минимальное значение ячеек столбца таблицы или -1.</returns>
        public int getTableMinInt32(string table, string column, int timeout)
        {
            //TODO: remove this comments after function testing
            // SQLiteCommand sqLiteCommand = new
            // SQLiteCommand(String.Format((IFormatProvider)
            // CultureInfo.InvariantCulture, "SELECT MIN(\"{0}\") FROM \"{1}\";",
            // new object[2] {
            // (object) column,
            // (object) table
            // }), this.m_connection, this.m_transaction);
            // ((DbCommand) sqLiteCommand).CommandTimeout = timeout;
            // int num = -1;
            // SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
            // if (((DbDataReader) sqLiteDataReader).HasRows)
            // {
            // ((DbDataReader) sqLiteDataReader).Read();
            // num = ((DbDataReader) sqLiteDataReader).GetInt32(0);
            // }
            // ((DbDataReader) sqLiteDataReader).Close();
            // return num;

            String query = String.Format("SELECT MIN(\"{0}\") FROM \"{1}\";", column, table);
            return this.ExecuteScalar(query, timeout);
        }

        /// <summary>
        /// NT-Получить общее число записей в таблице
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns>Функция возвращает число записей или -1.</returns>
        public int GetRowCount(string table, string column, int timeout)
        {
            //TODO: remove this comments after function testing
            // SQLiteCommand sqLiteCommand = new
            // SQLiteCommand(String.Format((IFormatProvider)
            // CultureInfo.InvariantCulture, "SELECT COUNT(\"{0}\") FROM \"{1}\";",
            // new object[2] {
            // (object) column,
            // (object) table
            // }), this.m_connection, this.m_transaction);
            // ((DbCommand) sqLiteCommand).CommandTimeout = timeout;
            // int num = -1;
            // SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
            // if (((DbDataReader) sqLiteDataReader).HasRows)
            // {
            // ((DbDataReader) sqLiteDataReader).Read();
            // num = ((DbDataReader) sqLiteDataReader).GetInt32(0);
            // }
            // ((DbDataReader) sqLiteDataReader).Close();
            // return num;

            String query = String.Format("SELECT COUNT(\"{0}\") FROM \"{1}\";", column, table);
            return this.ExecuteScalar(query, timeout);
        }

        /// <summary>
        /// NT-Получить число записей таблицы с указанным значением столбца
        /// </summary>
        /// <param name="table">Название таблицы</param>
        /// <param name="column">Название столбца</param>
        /// <param name="val">Значение столбца</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns>Функция возвращает число записей или -1.</returns>
        public int GetRowCount(string table, string column, int val, int timeout)
        {
            //TODO: remove this comments after function testing
            // SQLiteCommand sqLiteCommand = new
            // SQLiteCommand(String.Format((IFormatProvider)
            // CultureInfo.InvariantCulture, "SELECT COUNT(\"{0}\") FROM \"{1}\"
            // WHERE (\"{0}\" = {2});", (object) column, (object) table, (object)
            // val), this.m_connection, this.m_transaction);
            // ((DbCommand) sqLiteCommand).CommandTimeout = timeout;
            // int num = -1;
            // SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader();
            // if (((DbDataReader) sqLiteDataReader).HasRows)
            // {
            // ((DbDataReader) sqLiteDataReader).Read();
            // num = ((DbDataReader) sqLiteDataReader).GetInt32(0);
            // }
            // ((DbDataReader) sqLiteDataReader).Close();
            // return num;

            String query = String.Format("SELECT COUNT(\"{0}\") FROM \"{1}\" WHERE (\"{2}\" = {3});", column, table, column, val);
            return this.ExecuteScalar(query, timeout);
        }

        /// <summary>
        /// NT-Проверить существование в таблице записи с указанным идентификатором.
        /// </summary>
        /// <param name="tablename">Название таблицы</param>
        /// <param name="column">Название столбца идентификаторов записей таблицы</param>
        /// <param name="idValue">Идентификатор записи таблицы</param>
        /// <param name="timeout">Таймаут операции в секундах</param>
        /// <returns>
        /// Возвращает True при существовании записи с указанным ид, иначе возвращает False.
        /// </returns>
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
            //TODO: remove this comments after function testing
            // SQLiteCommand sqLiteCommand = new
            // SQLiteCommand(String.Format((IFormatProvider)
            // CultureInfo.InvariantCulture, "DELETE FROM {0};", new object[1] {
            // (object) table
            // }), this.m_connection, this.m_transaction);
            // ((DbCommand) sqLiteCommand).CommandTimeout = timeout;
            // ((DbCommand) sqLiteCommand).ExecuteNonQuery();

            String query = String.Format("DELETE FROM \"{0}\";", table);
            this.ExecuteNonQuery(query, timeout);

            return;
        }


        /// <summary>
        /// RT-Удалить таблицу БД
        /// </summary>
        /// <param name="table">Название таблицы.</param>
        /// <param name="timeout">Таймаут операции в секундах.</param>
        public void TableDrop(String table, int timeout)
        {
            String query = String.Format("DROP TABLE IF EXISTS \"{0}\";", table);
            this.ExecuteNonQuery(query, timeout);

            return;
        }
        #endregion


    }
}
