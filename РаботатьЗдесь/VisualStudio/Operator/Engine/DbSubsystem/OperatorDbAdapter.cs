using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.OperatorEngine;

namespace Engine.DbSubsystem
{
    /// <summary>
    /// NR - Адаптер БД Оператор, для БД sqlite3
    /// </summary>
    public class OperatorDbAdapter :  SqliteDbAdapter
    {
        //DONE: OperatorEngine.EngineSubsystem решено не имплементить здесь, но и не внедрять в SqliteDbAdapter.
        //А вовсе без него тут обойтись как исключение из правила, насколько возможно.

        #region *** Constants and Fields ***

     /// <summary>
     /// Application database file name
     /// </summary>
        public const String AppDbFileName = "db.sqlite";

        /// <summary>
        /// Places table title
        /// </summary>
        public const String TablePlaces = "places";

        /// <summary>
        /// Routines table title
        /// </summary>
        public const String TableProcs = "routines";

        /// <summary>
        /// Settings table title
        /// </summary>
        public const String TableSetting = "setting";

        /// <summary>
        /// Значение неправильного TableID итема, если итем не из таблиц БД.
        /// </summary>
        public const int Invalid_TableID = -1;

        /// <summary>
        /// Backreference to Engine object - for logging. Can be null where call this.CreateNewDatabase() !
        /// </summary>
        protected Engine.OperatorEngine.Engine m_Engine;

        // TODO: add new command here! Add init to constructor, Add code for new command to ClearCommand()!

        /// <summary>
        /// SQL Command for AddPlace function
        /// </summary>
        protected SQLiteCommand m_cmdAddPlace;

        /// <summary>
        /// SQL Command for UpdatePlace function
        /// </summary>
        protected SQLiteCommand m_cmdUpdatePlace;

        /// <summary>
        ///  SQL Command for AddProcedure function
        /// </summary>
        protected SQLiteCommand m_cmdAddProcedure;
 
        /// <summary>
        /// SQL Command for UpdateProcedure function
        /// </summary>
        protected SQLiteCommand m_cmdUpdateProcedure;

        /// <summary>
        /// SQL Command for AddSetting function
        /// </summary>
        protected SQLiteCommand m_cmdAddSetting;

        /// <summary>
        ///  SQL Command for UpdateSetting function
        /// </summary>
        protected SQLiteCommand m_cmdUpdateSetting;
#endregion

         
         /// <summary>
         /// NT-Default constructor
         /// </summary>
         /// <param name="engine">Engine backreference for log writing.</param>
         /// <exception cref="Exception">Error in database.</exception>
        public OperatorDbAdapter(Engine.OperatorEngine.Engine engine):base()
        {
            
        this.m_Engine = engine;// for logging
        // reset command object
        // TODO: Add code for new command here!
        this.m_cmdAddPlace = null;
        this.m_cmdUpdatePlace = null;
        this.m_cmdAddProcedure = null;
        this.m_cmdUpdateProcedure = null;
        this.m_cmdAddSetting = null;
        this.m_cmdUpdateSetting = null;

        return;
    }

/// <summary>
/// NT-Get string representation of object.
/// </summary>
/// <returns></returns>
    public override String ToString()
    {
        bool active = this.isConnectionActive;
        String cstr = Utility.StringUtility.GetStringTextNull(this.m_connectionString);
        String result = String.Format("OperatorDbAdapter; connection=\"{0}\"; active={1}", cstr, active);

        return result;
    }

    /**
     * NT-Clear all member commands
     * 
     * @throws SQLException
     *             Error on database access occured.
     */
    protected override void ClearCommands()
    {
        // TODO: add code for new command here!
        //Эта функция вызывается 4 раза при каждом добавлении или изменении любого Элемента в таблицах.
        //закрыть и обнулить каждую команду адаптера
        if(this.m_cmdAddPlace != null)
        {
            this.m_cmdAddPlace.Close();
            this.m_cmdAddPlace = null;
        }
        //CloseAndClearCmd(this.m_cmdUpdatePlace);
        if(this.m_cmdUpdatePlace != null)
        {
            this.m_cmdUpdatePlace.Close();
            this.m_cmdUpdatePlace = null;
        }
        //CloseAndClearCmd(this.m_cmdAddProcedure);
        if(this.m_cmdAddProcedure != null)
        {
            this.m_cmdAddProcedure.Close();
            this.m_cmdAddProcedure = null;
        }
        //CloseAndClearCmd(this.m_cmdUpdateProcedure);
        if(this.m_cmdUpdateProcedure != null)
        {
            this.m_cmdUpdateProcedure.Close();
            this.m_cmdUpdateProcedure = null;
        }
        //CloseAndClearCmd(this.m_cmdAddSetting);
        if(this.m_cmdAddSetting != null)
        {
            this.m_cmdAddSetting.Close();
            this.m_cmdAddSetting = null;
        }
        //CloseAndClearCmd(this.m_cmdUpdateSetting);
        if(this.m_cmdUpdateSetting != null)
        {
            this.m_cmdUpdateSetting.Close();
            this.m_cmdUpdateSetting = null;
        }
        
        return;
    }

    /// <summary>
    /// NT-Create new application database and fill with tables and initial data
    /// </summary>
    /// <param name="engine">Engine object reference for logging</param>
    /// <param name="dbFile">Database file path</param>    
    public static void CreateNewDatabase(Engine.OperatorEngine.Engine engine, String dbFile)
    {
        // try
        // {
        // Open
        String connectionString = OperatorDbAdapter.CreateConnectionString(dbFile, false);
        OperatorDbAdapter db = new OperatorDbAdapter(engine);
        db.Open(connectionString);
        // Write
        db.CreateDatabaseTables();
        // Close database
        db.Close();
        // }
        // catch(Exception ex)
        // {
        // //если произошла ошибка, перезапустить исключение с понятным
        // описанием причины.
        // String msg = "";
        // throw new Exception(msg);
        // }

        return;
    }


/// <summary>
/// NT-Create database tables
/// </summary>
/// <returns>Returns True if success, False otherwise.</returns>
public bool CreateDatabaseTables()
{

    bool result = true;
        try
        {
        // create Places table
        TableDrop(OperatorDbAdapter.TablePlaces, 60);
        String query = String.Format("CREATE TABLE \"{0}\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT, \"ns\" TEXT NOT NULL DEFAULT '', \"title\" TEXT NOT NULL DEFAULT '', \"type\" TEXT NOT NULL DEFAULT '', \"path\" TEXT NOT NULL DEFAULT '', \"descr\" TEXT NOT NULL DEFAULT '', \"syno\" TEXT NOT NULL DEFAULT '');", OperatorDbAdapter.TablePlaces);
        this.ExecuteNonQuery(query, 60);
        // create Procedures table
        TableDrop(OperatorDbAdapter.TableProcs, 60);
        query = String.Format("CREATE TABLE \"{0}\"(\"id\" INTEGER PRIMARY KEY AUTOINCREMENT, \"ns\" TEXT NOT NULL DEFAULT '', \"title\" TEXT NOT NULL DEFAULT '', \"ves\" Real, \"path\" TEXT NOT NULL DEFAULT '', \"regex\" TEXT NOT NULL DEFAULT '', \"descr\" TEXT NOT NULL DEFAULT '');", OperatorDbAdapter.TableProcs);
        this.ExecuteNonQuery(query, 60);
        // create Setting table
        TableDrop(OperatorDbAdapter.TableSetting, 60);
        query = String.Format("CREATE TABLE \"{0}\" ( \"id\" INTEGER PRIMARY KEY AUTOINCREMENT, \"ns\" TEXT NOT NULL DEFAULT '', \"title\" TEXT NOT NULL DEFAULT '', \"descr\" TEXT NOT NULL DEFAULT '', \"val\" TEXT NOT NULL DEFAULT '' )", OperatorDbAdapter.TableSetting);
        this.ExecuteNonQuery(query, 60);
        // create index
        query = String.Format("CREATE INDEX \"{0}_ix_title\" ON \"{1}\" (\"title\" ASC);", OperatorDbAdapter.TableSetting, OperatorDbAdapter.TableSetting);
        this.ExecuteNonQuery(query, 60);
        this.m_connection.commit();
    }
        catch (Exception ex)
        {
        result = false;
        this.TransactionRollback();
    }

        return result;
}

#region *** Places table function ***


/// <summary>
/// NT-Получить все записи таблицы Places
/// </summary>
/// <returns>Функция возвращает список записей таблицы Places</returns>
public List<Place> GetAllPlaces()
    {
        List<Place> list = new List<Place>();

String query = String.Format("SELECT * FROM \"{0}\";", OperatorDbAdapter.TablePlaces);
SQLiteDataReader reader = this.ExecuteReader(query, this.m_Timeout);

while (reader.next())
{
    Place place = new Place();
    place.TableId = reader.getInt(1);
    place.Namespace = reader.getString(2);
    place.Title = reader.getString(3);
    place.PlaceTypeExpression = reader.getString(4);
    place.Path = reader.getString(5);
    place.Description = reader.getString(6);
    place.Synonim = reader.getString(7);
    place.ParseEntityTypeString();// TODO: перенести этот вызов на более
                                  // поздний этап и обложить катчем на
                                  // всякий случай.
                                  // set storage title as database
    place.Storage = Item.StorageKeyForDatabaseItem;
    // add to result list
    list.Add(place);
}

// close command and result set objects
reader.Close();

return list;
    }

    /**
     * NT-Добавить Место в таблицу Мест.
     * 
     * @param p
     *            Добавляемое Место.
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void AddPlace(Place p) 
{
    PreparedStatement ps = this.m_cmdAddPlace;

        // create if not exists
        if (ps == null)
        {
        String query = String.format("INSERT INTO \"%s\"(\"ns\", \"title\", \"type\", \"path\", \"descr\", \"syno\") VALUES (?,?,?,?,?,?);", OperatorDbAdapter.TablePlaces);
        ps = this.m_connection.prepareStatement(query);
        // set timeout here
        ps.setQueryTimeout(this.m_Timeout);
        // write back
        this.m_cmdAddPlace = ps;
    }

    // set parameters
    ps.setString(1, p.get_Namespace());
    ps.setString(2, p.get_Title());
    ps.setString(3, p.get_PlaceTypeExpression());
    ps.setString(4, p.get_Path());
    ps.setString(5, p.get_Description());
    ps.setString(6, p.get_Synonim());

    ps.executeUpdate();
        // Do not close command here - for next reusing

        return;
}


/// <summary>
/// NT-Удалить Место по ИД.
/// </summary>
/// <param name="placeId">ИД Места.</param>
/// <returns>Функция возвращает число измененных строк таблицы.</returns>
public int RemovePlace(int placeId)
{
        return this.DeleteRow(OperatorDbAdapter.TablePlaces, "id", placeId, this.m_Timeout);
}

/**
 * NT-Update Place
 * 
 * @param p
 *            Place object
 * @return Функция возвращает число измененных строк таблицы.
 * @throws Exception
 *             Ошибка при использовании БД.
 */
public int UpdatePlace(Place p)
{
    PreparedStatement ps = this.m_cmdUpdatePlace;

        // create if not exists
        if (ps == null)
        {
        String query = String.format("UPDATE \"%s\" SET \"ns\" = ?, \"title\" = ?, \"type\" = ?, \"path\" = ?, \"descr\" = ?, \"syno\" = ? WHERE(\"id\" = ?);", OperatorDbAdapter.TablePlaces);
        ps = this.m_connection.prepareStatement(query);
        // set timeout here
        ps.setQueryTimeout(this.m_Timeout);
        // write back
        this.m_cmdUpdatePlace = ps;
    }

    // set parameters
    ps.setString(1, p.get_Namespace());
    ps.setString(2, p.get_Title());
    ps.setString(3, p.get_PlaceTypeExpression());
    ps.setString(4, p.get_Path());
    ps.setString(5, p.get_Description());
    ps.setString(6, p.get_Synonim());
    ps.setInt(7, p.get_TableId());

        int result = ps.executeUpdate();
        // Do not close command here - for next reusing

        return result;
}


/// <summary>
/// NT-Remove all Places
/// </summary>
public void RemoveAllPlaces()
{
        this.TableClear(TablePlaces, m_Timeout);
}

#endregion

#region *** Procedures table function ***

/**
 * NT-Получить все записи таблицы Процедур
 * 
 * @return Функция возвращает список записей из таблицы Процедур.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public LinkedList<Procedure> GetAllProcedures() throws SQLException
{
    LinkedList<Procedure> list = new LinkedList<Procedure>();

String query = String.format("SELECT * FROM \"%s\";", OperatorDbAdapter.TableProcs);
ResultSet reader = this.ExecuteReader(query, this.m_Timeout);

while (reader.next())
{
    Procedure proc = new Procedure();
    proc.set_TableId(reader.getInt(1));
    proc.set_Namespace(reader.getString(2));
    proc.set_Title(reader.getString(3));
    proc.set_Ves(reader.getDouble(4));
    proc.set_Path(reader.getString(5));
    proc.set_Regex(reader.getString(6));
    proc.set_Description(reader.getString(7));
    // set storage title as database
    proc.set_Storage(Item.StorageKeyForDatabaseItem);
    // add to result list
    list.add(proc);
}
// close command and result set objects
reader.getStatement().close();

return list;
    }

    /**
     * NT-Добавить Процедуру.
     * 
     * @param p
     *            Добавляемая Процедура.
     * @throws SQLException
     *             Ошибка при использовании БД.
     */
    public void AddProcedure(Procedure p) throws SQLException
{

    PreparedStatement ps = this.m_cmdAddProcedure;

        if (ps == null)
        {
        String query = String.format("INSERT INTO \"%s\"(\"ns\", \"title\", \"ves\", \"path\", \"regex\", \"descr\") VALUES (?,?,?,?,?,?);", OperatorDbAdapter.TableProcs);
        ps = this.m_connection.prepareStatement(query);
        // set timeout here
        ps.setQueryTimeout(this.m_Timeout);
        // write back
        this.m_cmdAddProcedure = ps;
    }

    // set parameters
    ps.setString(1, p.get_Namespace());
    ps.setString(2, p.get_Title());
    ps.setDouble(3, p.get_Ves());
    ps.setString(4, p.get_Path());
    ps.setString(5, p.get_Regex());
    ps.setString(6, p.get_Description());

    ps.executeUpdate();
        // Do not close command here - for next reusing

        return;
}


 * NT-Удалить Процедуру
 * 
 * @param id
 *            ИД Процедуры
 * @return Функция возвращает число измененных строк таблицы.
 * @throws SQLException
 *             Ошибка при использовании БД.
public int RemoveProcedure(int id) 
{
        return this.DeleteRow(OperatorDbAdapter.TableProcs, "id", id, this.m_Timeout);
}

/**
 * NT-Update Procedure
 * 
 * @param p
 *            Procedure object
 * @return Функция возвращает число измененных строк таблицы.
 * @throws Exception
 *             Ошибка при использовании БД.
 */
public int UpdateProcedure(Procedure p) throws Exception
{
    PreparedStatement ps = this.m_cmdUpdateProcedure;

        if (ps == null)
        {
        String query = String.format("UPDATE \"%s\" SET \"ns\" = ?, \"title\" = ?, \"ves\" = ?, \"path\" = ?, \"regex\" = ?, \"descr\" = ? WHERE (\"id\" = ?);", OperatorDbAdapter.TableProcs);
        ps = this.m_connection.prepareStatement(query);
        // set timeout here
        ps.setQueryTimeout(this.m_Timeout);
        // write back
        this.m_cmdUpdateProcedure = ps;
    }

    // set parameters
    ps.setString(1, p.get_Namespace());
    ps.setString(2, p.get_Title());
    ps.setDouble(3, p.get_Ves());
    ps.setString(4, p.get_Path());
    ps.setString(5, p.get_Regex());
    ps.setString(6, p.get_Description());
    ps.setInt(7, p.get_TableId());

        int result = ps.executeUpdate();
        // Do not close command here - for next reusing

        return result;
}


 * NT-Remove all Procedures
 * @throws Exception Error in process
public void RemoveAllProcedures() 
{
        this.TableClear(TableProcs, m_Timeout);
}

#endregion

#region *** Setting table function ***

/**
 * NT- Получить все записи таблицы настроек Оператора
 * 
 * @return Функция возвращает все записи из ТаблицыНастроекОператора.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public LinkedList<SettingItem> GetAllSettings() throws SQLException
{
    // SELECT * FROM `setting` WHERE (`id` = 1);

    LinkedList<SettingItem> list = new LinkedList<SettingItem>();

String query = String.format("SELECT * FROM \"%s\";", OperatorDbAdapter.TableSetting);
ResultSet reader = this.ExecuteReader(query, this.m_Timeout);

while (reader.next())
{
    SettingItem si = new SettingItem();
    si.set_TableId(reader.getInt(1));
    si.set_Namespace(reader.getString(2));
    si.set_Title(reader.getString(3));
    si.set_Description(reader.getString(4));
    si.set_Path(reader.getString(5));// set value as Item.Path
                                     // set storage field as db
    si.set_Storage(Item.StorageKeyForDatabaseItem);
    // add to result list
    list.add(si);
}

// close command and result set objects
reader.getStatement().close();

return list;

    }

    /**
     * NT-Добавить Настройку.
     * 
     * @param item
     *            Добавляемая Настройка.
     * @throws SQLException
     *             Ошибка при использовании БД.
     */
    public void AddSetting(SettingItem item) throws SQLException
{

    PreparedStatement ps = this.m_cmdAddSetting;

        if (ps == null)
        {
        String query = String.format("INSERT INTO \"%s\"(\"ns\", \"title\", \"descr\", \"val\") VALUES (?,?,?,?);", OperatorDbAdapter.TableSetting);
        ps = this.m_connection.prepareStatement(query);
        // set timeout here
        ps.setQueryTimeout(this.m_Timeout);
        // write back
        this.m_cmdAddSetting = ps;
    }

    // set parameters
    ps.setString(1, item.get_Namespace());
    ps.setString(2, item.get_Title());
    ps.setString(3, item.get_Description());
    ps.setString(4, item.get_Path());// get value as Item.Path

    ps.executeUpdate();
        // Do not close command here - for next reusing

        return;
}

/**
 * NT-Удалить Настройку.
 * 
 * @param id
 *            ИД Настройки.
 * @return Функция возвращает число измененных строк таблицы.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int RemoveSetting(int id) throws SQLException
{
        // DELETE FROM `setting` WHERE (`id` = 1);
        return this.DeleteRow(OperatorDbAdapter.TableSetting, "id", id, this.m_Timeout);
}

/**
 * NT- Изменить Настройку (title, descr, value)
 * 
 * @param item
 *            Изменяемая Настройка
 * @return Функция возвращает число измененных строк таблицы.
 * @throws SQLException
 *             Ошибка при использовании БД.
 */
public int UpdateSetting(SettingItem item) throws SQLException
{
    PreparedStatement ps = this.m_cmdUpdateSetting;

        if (ps == null)
        {
        String query = String.format("UPDATE \"%s\" SET \"ns\" = ?, \"title\" = ?, \"descr\" = ?, \"val\" = ? WHERE (\"id\" = ?);", OperatorDbAdapter.TableSetting);
        ps = this.m_connection.prepareStatement(query);
        // set timeout here
        ps.setQueryTimeout(this.m_Timeout);
        // write back
        this.m_cmdUpdateSetting = ps;
    }

    // set parameters
    ps.setString(1, item.get_Namespace());
    ps.setString(2, item.get_Title());
    ps.setString(3, item.get_Description());
    ps.setString(4, item.get_Path());// get value as Item.Path
    ps.setInt(5, item.get_TableId());

        int result = ps.executeUpdate();
        // Do not close command here - for next reusing

        return result;
}


 * NT-Remove all Settings
 * @throws Exception Error in process

public void RemoveAllSettings()
{
        this.TableClear(TableSetting, m_Timeout);
}
    #endregion

    }
}
