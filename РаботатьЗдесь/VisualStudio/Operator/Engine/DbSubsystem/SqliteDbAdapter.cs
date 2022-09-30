using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.DbSubsystem
{
    /// <summary>
    /// NR-Общая версия адаптера для БД sqlite3
    /// </summary>
    public class SqliteDbAdapter
    {
        // Fields ===================================

        /**
         * Database file connection string
         */
        protected String m_connectionString;

        /**
         * Database connection object
         */
        protected Connection m_connection;

        /**
         * Command execution default timeout
         */
        protected int m_Timeout;

        // ==========================================

        /**
         * RT-Default constructor
         * 
         * @throws Exception
         *             Error on database access occured.
         */
        public SqliteDbAdapter() throws Exception
        {
            // load class from library
            Class.forName("org.sqlite.JDBC");

        this.m_connection = null;
        this.m_connectionString = "";// string.Empty;
        this.m_Timeout = 60;
        this.ClearCommands();

        return;
    }
    // //TODO: как реализовать деструктор на Java, если он необходим?
    // ~SqliteDbAdapter()
    // {
    // this.Close();
    //
    // return;
    // }

    // Properties =========================

    /**
     * NT-Get command execution timeout in seconds.
     * 
     * @return Returns command execution timeout in seconds.
     */
    public int getTimeout()
    {
        return this.m_Timeout;
    }

    /**
     * NT-Set command execution timeout in seconds.
     * 
     * @param sec
     *            Command execution timeout in seconds.
     */
    public void setTimeout(int sec)
    {
        this.m_Timeout = sec;
    }

    /**
     * NT-Get current connection string value.
     * 
     * @return Returns current connection string value.
     */
    public String getConnectionString()
    {
        return this.m_connectionString;
    }

    /**
     * RT- Check is database connection active
     * 
     * @return Returns True if connection active.
     */
    public boolean isConnectionActive()
    {
        boolean result = false;
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

    // Service functions ================================
    /**
     * RT-Clear all member commands
     * 
     * @throws Exception
     *             Function not implemented in this class.
     */
    protected void ClearCommands() throws Exception
    {
        ; // throw new Exception("Function not implemented in this class.");
    }

    /**
     * RT-Open connection to database.
     * Specified connection string will be stored inside this object for next
     * using.
     * 
     * @param connectionString
     *            Connection string for database
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void Open(String connectionString) throws Exception
    {
        this.m_connectionString = Utility.StringCopy(connectionString);
        this.Open();
    }

    /**
     * NT-Open connection to database, using previous stored connection string.
     * If connection already opened then return.
     * 
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void Open() throws Exception
    {
        // skip if connection already active
        if (this.isConnectionActive())
            return;
        else
        {
            DriverManager.setLoginTimeout(this.m_Timeout);
            this.m_connection = DriverManager.getConnection(this.m_connectionString);
            this.m_connection.setAutoCommit(false);
        }
        //clear command object ref's
        this.ClearCommands();
        
        return;
    }

    /**
     * RT- Close connection and reset all resources to initial state.
     * Connection string not cleared.
     * 
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void Close() throws Exception
    {
        if (this.m_connection == null)
            return;
        if (!this.m_connection.isClosed())
            this.m_connection.close();
        this.m_connection = null;
        //clear command object ref's
        this.ClearCommands();

        return;
    }

    /**
     * RT-Create connection string
     * 
     * @param dbFilePath
     *            Database file pathname string
     * @return Returns connection string
     */
    public static String CreateConnectionString(String dbFilePath)
    {
        // connection strings:
        // jdbc:sqlite::memory: - in-memory database
        // jdbc:sqlite:C:/sqlite/db/chinook.db - windows absolute path
        // jdbc:sqlite:test.db - test.db in current folder
        return "jdbc:sqlite:" + dbFilePath;
    }

    /**
     * NT-Get string representation of object.
     * 
     * @see java.lang.Object#toString()
     */
    @Override
    public String toString()
    {
        boolean active = this.isConnectionActive();
        String cstr = Utility.GetStringTextNull(this.m_connectionString);
        String result = String.format("SqliteDbAdapter; connection=\"%s\"; active=%s", cstr, active);

        return result;
    }

    // Transaction functions =======================================
    /**
     * NR-Transaction begins on first query
     * 
     * @throws Exception
     *             Функция не может быть реализована, но является частью общего
     *             интерфейса БД.
     */
    public void TransactionBegin() throws Exception
    {
        // this.m_transaction = this.m_connection.BeginTransaction();
        // this.ClearCommands();
        throw new Exception("Not implemented function");
}

/**
 * RT- Commit current transaction
 * 
 * @throws Exception
 *             Ошибка при использовании БД.
 */
public void TransactionCommit() throws Exception
{
        this.m_connection.commit();
        this.ClearCommands();
        return;
}

/**
 * NT-Rollback current transaction
 * 
 * @throws Exception
 *             Ошибка при использовании БД.
 */
public void TransactionRollback() throws Exception
{
        this.m_connection.rollback();
        this.ClearCommands();
        return;
}

// General adapter functions ===================

/**
 * RT- Execute INSERT UPDATE DELETE query
 * 
 * @param query
 *            SQL query string
 * @param timeout
 *            Execution timeout in seconds.
 * @return Returns number of changed rows or 0 if no changes.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int ExecuteNonQuery(String query, int timeout) throws SQLException
{
    Statement sqLiteCommand = this.m_connection.createStatement();
    sqLiteCommand.setQueryTimeout(timeout);
        int result = sqLiteCommand.executeUpdate(query);
    sqLiteCommand.close();

        return result;
}

/**
 * NT-Execute SELECT query without arguments.
 * Caller must close Statement after reading result set via accessor
 * ResultSet.getStatement();
 * 
 * @param query
 *            SQL query string
 * @param timeout
 *            Execution timeout in seconds.
 * @return Returns ResultSet for this query.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public ResultSet ExecuteReader(String query, int timeout)
            throws SQLException
{
    Statement sqLiteCommand = this.m_connection.createStatement();
    sqLiteCommand.setQueryTimeout(timeout);
    ResultSet result = sqLiteCommand.executeQuery(query);
        // get Statement to close it later: result.getStatement();
        return result;
}

/**
 * NT-Execute SELECT query without arguments.
 * Returns result in first row first column as int.
 * 
 * @param query
 *            SQL query string
 * @param timeout
 *            Execution timeout in seconds.
 * @return Returns result in first row first column as int. Returns -1 if
 *         errors.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int ExecuteScalar(String query, int timeout) throws SQLException
{
    Statement sqLiteCommand = this.m_connection.createStatement();
    sqLiteCommand.setQueryTimeout(timeout);
    ResultSet rs = sqLiteCommand.executeQuery(query);
        int result = -1;
        // read first row in result
        // if(rs.first() == true) - throws exception with: ResultSet has mode
        // FORWARD_ONLY
        if (rs.next() == true)
        {
        // read first column in result
        result = rs.getInt(1);// first column = 1!
    }
    sqLiteCommand.close();

        return result;
}

/**
 * NT-Получить из ридера строку либо нуль как пустую строку.
 * 
 * @param rdr
 *            Объект ридера.
 * @param index
 *            Индекс столбца для ридера, начинается с 1.
 * @return Функция возвращает строку или пустую строку, если исходное
 *         значение было null.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public static String getDbString(ResultSet rdr, int index)
            throws SQLException
{
    String result = rdr.getString(index);
        if (result == null)
            result = "";

        return result;
}

/**
 * RT-Get last used rowid for table.
 * 
 * @param table
 *            Название таблицы.
 * @param timeout
 *            Таймаут операции, в секундах.
 * @return Returns last used rowid for table.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int getLastRowId(String table, int timeout) throws SQLException
{
    String query = String.format("SELECT \"seq\" FROM \"sqlite_sequence\" WHERE \"name\" = \"%s\";", table);
        return this.ExecuteScalar(query, timeout);
}

/**
 * RT-Удалить строки с указанным значением столбца из таблицы.
 * 
 * @param table
 *            Название таблицы.
 * @param column
 *            Название столбца.
 * @param val
 *            Числовое значение столбца в таблице.
 * @param timeout
 *            Таймаут операции, в секундах.
 * @return Функция возвращает число удаленных строк.
 * 
 *         Эта универсальная функция позволяет удалить строку таблицы по
 *         значению одного из ее столбцов.
 *         Например, по ID записи.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int DeleteRow(String table, String column, int val, int timeout)
            throws SQLException
{
    // SQLiteCommand sqLiteCommand = new
    // SQLiteCommand(String.Format((IFormatProvider)
    // CultureInfo.InvariantCulture, "DELETE FROM \"{0}\" WHERE (\"{1}\" =
    // {2});", (object) table, (object) column, (object) val),
    // this.m_connection, this.m_transaction);
    // ((DbCommand) sqLiteCommand).CommandTimeout = timeout;
    // return ((DbCommand) sqLiteCommand).ExecuteNonQuery();
    String query = String.format("DELETE FROM \"%s\" WHERE (\"%s\" = %d);", table, column, val);
        return this.ExecuteNonQuery(query, timeout);
}

/**
 * RT-Получить максимальное значение поля столбца таблицы.
 * 
 * @param table
 *            Название таблицы.
 * @param column
 *            Название столбца.
 * @param timeout
 *            Таймаут операции в секундах.
 * @return Возвращает максимальное значение ячеек столбца таблицы или -1 при
 *         ошибке.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int getTableMaxInt32(String table, String column, int timeout)
            throws SQLException
{
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

    String query = String.format("SELECT MAX(`%s`) FROM `%s`;", column, table);
        return this.ExecuteScalar(query, timeout);

}

/**
 * RT-Получить минимальное значение поля столбца таблицы.
 * 
 * @param table
 *            Название таблицы.
 * @param column
 *            Название столбца.
 * @param timeout
 *            Таймаут операции в секундах.
 * @return Возвращает минимальное значение ячеек столбца таблицы или -1 при
 *         ошибке.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int getTableMinInt32(String table, String column, int timeout)
            throws SQLException
{
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

    String query = String.format("SELECT MIN(`%s`) FROM `%s`;", column, table);
        return this.ExecuteScalar(query, timeout);
}

/**
 * RT-Получить общее число записей в таблице
 * 
 * @param table
 *            Название таблицы.
 * @param column
 *            Название столбца.
 * @param timeout
 *            Таймаут операции в секундах.
 * @return Возвращает общее число записей в таблице или -1 при ошибке.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int GetRowCount(String table, String column, int timeout)
            throws SQLException
{
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

    String query = String.format("SELECT COUNT(\"%s\") FROM \"%s\";", column, table);
        return this.ExecuteScalar(query, timeout);
}

/**
 * NT-Получить число записей таблицы с указанным значением столбца
 * 
 * @param table
 *            Название таблицы.
 * @param column
 *            Название столбца.
 * @param val
 *            Значение столбца.
 * @param timeout
 *            Таймаут операции в секундах.
 * @return Возвращает число записей таблицы с указанным значением столбца
 *         или -1 при ошибке.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int GetRowCount(String table, String column, int val, int timeout)
            throws SQLException
{
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

    String query = String.format("SELECT COUNT(\"%s\") FROM \"%s\" WHERE (\"%s\" = %d);", column, table, column, val);
        return this.ExecuteScalar(query, timeout);
}

/**
 * RT-Проверить существование в таблице записи с указанным идентификатором.
 * 
 * @param table
 *            Название таблицы.
 * @param column
 *            Название столбца идентификаторов записей.
 * @param id
 *            Значение идентификатора записи.
 * @param timeout
 *            Таймаут операции в секундах.
 * @return Возвращает True при существовании записи с указанным ид, иначе
 *         возвращает False.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public boolean IsRowExists(String table, String column, int id, int timeout)
            throws SQLException
{
        return (this.GetRowCount(table, column, id, timeout) > 0);
}

/**
 * NT-Очистить таблицу БД.
 * 
 * @param table
 *            Название таблицы.
 * @param timeout
 *            Таймаут операции в секундах.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public void TableClear(String table, int timeout) throws SQLException
{
    // SQLiteCommand sqLiteCommand = new
    // SQLiteCommand(String.Format((IFormatProvider)
    // CultureInfo.InvariantCulture, "DELETE FROM {0};", new object[1] {
    // (object) table
    // }), this.m_connection, this.m_transaction);
    // ((DbCommand) sqLiteCommand).CommandTimeout = timeout;
    // ((DbCommand) sqLiteCommand).ExecuteNonQuery();

    String query = String.format("DELETE FROM \"%s\";", table);
        this.ExecuteNonQuery(query, timeout);

        return;
}

/**
 * RT-Удалить таблицу БД
 * 
 * @param table
 *            Название таблицы.
 * @param timeout
 *            Таймаут операции в секундах.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public void TableDrop(String table, int timeout) throws SQLException
{
    String query = String.format("DROP TABLE IF EXISTS \"%s\";", table);
        this.ExecuteNonQuery(query, timeout);

        return;
}

    // Service functions =========================

    }
}
