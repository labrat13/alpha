using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Engine.OperatorEngine;
using Engine.SettingSubsystem;

namespace Engine.DbSubsystem
{
    /// <summary>
    /// NT - Адаптер БД Оператор, для БД sqlite3
    /// </summary>
    public class OperatorDbAdapter : SqliteDbAdapter
    {
        //DONE: OperatorEngine.EngineSubsystem решено не имплементить здесь, но и не внедрять в SqliteDbAdapter.
        //А вовсе без него тут обойтись как исключение из правила, насколько возможно.

        //DONE: ported from Java to CS

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
        public OperatorDbAdapter(Engine.OperatorEngine.Engine engine) : base()
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


        ///<summary>
        ///NT-Clear all member commands
        ///</summary>
        protected override void ClearCommands()
        {
            // TODO: add code for new command here!
            //Эта функция вызывается 4 раза при каждом добавлении или изменении любого Элемента в таблицах.
            //закрыть и обнулить каждую команду адаптера

            this.m_cmdAddPlace = null;
            this.m_cmdUpdatePlace = null;
            this.m_cmdAddProcedure = null;
            this.m_cmdUpdateProcedure = null;
            this.m_cmdAddSetting = null;
            this.m_cmdUpdateSetting = null;

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
                this.TransactionCommit();
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
            if (reader.HasRows)
                while (reader.Read())
                {
                    Place place = new Place();
                    place.TableId = reader.GetInt32(0);
                    place.Namespace = reader.GetString(1);
                    place.Title = reader.GetString(2);
                    place.PlaceTypeExpression = reader.GetString(3);
                    place.Path = reader.GetString(4);
                    place.Description = reader.GetString(5);
                    place.Synonim = reader.GetString(6);
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


        /// <summary>
        /// NT-Добавить Место в таблицу Мест.
        /// </summary>
        /// <param name="p">Добавляемое Место.</param>    
        public void AddPlace(Place p)
        {
            SQLiteCommand ps = this.m_cmdAddPlace;

            // create if not exists
            if (ps == null)
            {
                String query = String.Format("INSERT INTO \"{0}\"(\"ns\", \"title\", \"type\", \"path\", \"descr\", \"syno\") VALUES (?,?,?,?,?,?);", OperatorDbAdapter.TablePlaces);
                ps = new SQLiteCommand(query, this.m_connection, this.m_transaction);
                // set timeout here
                ps.CommandTimeout = this.m_Timeout;
                //create parameters
                ps.Parameters.Add("a0", DbType.String);
                ps.Parameters.Add("a1", DbType.String);
                ps.Parameters.Add("a2", DbType.String);
                ps.Parameters.Add("a3", DbType.String);
                ps.Parameters.Add("a4", DbType.String);
                ps.Parameters.Add("a5", DbType.String);
                // write back
                this.m_cmdAddPlace = ps;
            }

            // set parameters
            ps.Parameters[0].Value = p.Namespace;
            ps.Parameters[1].Value = p.Title;
            ps.Parameters[2].Value = p.PlaceTypeExpression;
            ps.Parameters[3].Value = p.Path;
            ps.Parameters[4].Value = p.Description;
            ps.Parameters[5].Value = p.Synonim;
            //execute command
            ps.ExecuteNonQuery();

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

        /// <summary>
        /// NT-Update Place
        /// </summary>
        /// <param name="p">Place object</param>
        /// <returns>Функция возвращает число измененных строк таблицы.</returns>
        public int UpdatePlace(Place p)
        {
            SQLiteCommand ps = this.m_cmdUpdatePlace;

            // create if not exists
            if (ps == null)
            {
                String query = String.Format("UPDATE \"{0}\" SET \"ns\" = ?, \"title\" = ?, \"type\" = ?, \"path\" = ?, \"descr\" = ?, \"syno\" = ? WHERE(\"id\" = ?);", OperatorDbAdapter.TablePlaces);
                ps = new SQLiteCommand(query, this.m_connection, this.m_transaction);
                // set timeout here
                ps.CommandTimeout = this.m_Timeout;
                //create parameters
                ps.Parameters.Add("a0", DbType.String);
                ps.Parameters.Add("a1", DbType.String);
                ps.Parameters.Add("a2", DbType.String);
                ps.Parameters.Add("a3", DbType.String);
                ps.Parameters.Add("a4", DbType.String);
                ps.Parameters.Add("a5", DbType.String);
                ps.Parameters.Add("a6", DbType.Int32);
                // write back
                this.m_cmdUpdatePlace = ps;
            }
            // set parameters
            ps.Parameters[0].Value = p.Namespace;
            ps.Parameters[1].Value = p.Title;
            ps.Parameters[2].Value = p.PlaceTypeExpression;
            ps.Parameters[3].Value = p.Path;
            ps.Parameters[4].Value = p.Description;
            ps.Parameters[5].Value = p.Synonim;
            ps.Parameters[6].Value = p.TableId;
            //execute command
            int result = ps.ExecuteNonQuery();

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

        /// <summary>
        /// NT-Получить все записи таблицы Процедур
        /// </summary>
        /// <returns>Функция возвращает список записей из таблицы Процедур.</returns>
        public List<Procedure> GetAllProcedures()
        {
            List<Procedure> list = new List<Procedure>();

            String query = String.Format("SELECT * FROM \"{0}\";", OperatorDbAdapter.TableProcs);
            SQLiteDataReader reader = this.ExecuteReader(query, this.m_Timeout);

            if (reader.HasRows)
                while (reader.Read())
                {
                    Procedure proc = new Procedure();
                    proc.TableId = reader.GetInt32(0);
                    proc.Namespace = reader.GetString(1);
                    proc.Title = reader.GetString(2);
                    proc.Ves = reader.GetDouble(3);
                    proc.Path = reader.GetString(4);
                    proc.Regex = reader.GetString(5);
                    proc.Description = reader.GetString(6);
                    // set storage title as database
                    proc.Storage = Item.StorageKeyForDatabaseItem;
                    // add to result list
                    list.Add(proc);
                }
            // close command and result set objects
            reader.Close();

            return list;
        }


        /// <summary>
        /// NT-Добавить Процедуру.
        /// </summary>
        /// <param name="p">Добавляемая Процедура.</param>        
        public void AddProcedure(Procedure p)
        {
            SQLiteCommand ps = this.m_cmdAddProcedure;

            // create if not exists
            if (ps == null)
            {
                String query = String.Format("INSERT INTO \"{0}\"(\"ns\", \"title\", \"ves\", \"path\", \"regex\", \"descr\") VALUES (?,?,?,?,?,?);", OperatorDbAdapter.TableProcs);
                ps = new SQLiteCommand(query, this.m_connection, this.m_transaction);
                // set timeout here
                ps.CommandTimeout = this.m_Timeout;
                //create parameters
                ps.Parameters.Add("a0", DbType.String);
                ps.Parameters.Add("a1", DbType.String);
                ps.Parameters.Add("a2", DbType.Double);
                ps.Parameters.Add("a3", DbType.String);
                ps.Parameters.Add("a4", DbType.String);
                ps.Parameters.Add("a5", DbType.String);
                // write back
                this.m_cmdAddProcedure = ps;
            }

            // set parameters
            ps.Parameters[0].Value = p.Namespace;
            ps.Parameters[1].Value = p.Title;
            ps.Parameters[2].Value = p.Ves;
            ps.Parameters[3].Value = p.Path;
            ps.Parameters[4].Value = p.Regex;
            ps.Parameters[5].Value = p.Description;
            //execute command
            ps.ExecuteNonQuery();

            return;
        }

        /// <summary>
        /// NT-Удалить Процедуру
        /// </summary>
        /// <param name="id">ИД Процедуры</param>
        /// <returns>Функция возвращает число измененных строк таблицы.</returns>
        public int RemoveProcedure(int id)
        {
            return this.DeleteRow(OperatorDbAdapter.TableProcs, "id", id, this.m_Timeout);
        }


        /// <summary>
        /// NT-Update Procedure
        /// </summary>
        /// <param name="p">Procedure object</param>
        /// <returns>Функция возвращает число измененных строк таблицы.</returns>
        public int UpdateProcedure(Procedure p)
        {
            SQLiteCommand ps = this.m_cmdUpdateProcedure;

            // create if not exists
            if (ps == null)
            {
                String query = String.Format("UPDATE \"{0}\" SET \"ns\" = ?, \"title\" = ?, \"ves\" = ?, \"path\" = ?, \"regex\" = ?, \"descr\" = ? WHERE (\"id\" = ?);", OperatorDbAdapter.TableProcs);
                ps = new SQLiteCommand(query, this.m_connection, this.m_transaction);
                // set timeout here
                ps.CommandTimeout = this.m_Timeout;
                //create parameters
                ps.Parameters.Add("a0", DbType.String);
                ps.Parameters.Add("a1", DbType.String);
                ps.Parameters.Add("a2", DbType.Double);
                ps.Parameters.Add("a3", DbType.String);
                ps.Parameters.Add("a4", DbType.String);
                ps.Parameters.Add("a5", DbType.String);
                ps.Parameters.Add("a6", DbType.Int32);
                // write back
                this.m_cmdUpdateProcedure = ps;
            }
            // set parameters
            ps.Parameters[0].Value = p.Namespace;
            ps.Parameters[1].Value = p.Title;
            ps.Parameters[2].Value = p.Ves;
            ps.Parameters[3].Value = p.Path;
            ps.Parameters[4].Value = p.Regex;
            ps.Parameters[5].Value = p.Description;
            ps.Parameters[6].Value = p.TableId;
            //execute command
            int result = ps.ExecuteNonQuery();

            return result;
        }

        /// <summary>
        /// NT-Remove all Procedures
        /// </summary>
        public void RemoveAllProcedures()
        {
            this.TableClear(TableProcs, m_Timeout);
        }

        #endregion

        #region *** Setting table function ***

        /// <summary>
        /// NT- Получить все записи таблицы настроек Оператора
        /// </summary>
        /// <returns>Функция возвращает все записи из ТаблицыНастроекОператора.</returns>
        public List<SettingItem> GetAllSettings()
        {

            List<SettingItem> list = new List<SettingItem>();

            String query = String.Format("SELECT * FROM \"{0}\";", OperatorDbAdapter.TableSetting);
            SQLiteDataReader reader = this.ExecuteReader(query, this.m_Timeout);
            if (reader.HasRows)
                while (reader.Read())
                {
                    SettingItem si = new SettingItem();
                    si.TableId = reader.GetInt32(0);
                    si.Namespace = reader.GetString(1);
                    si.Title = reader.GetString(2);
                    si.Description = reader.GetString(3);
                    si.Path = reader.GetString(4);// set value as Item.Path
                                                  // set storage field as db
                    si.Storage = Item.StorageKeyForDatabaseItem;
                    // add to result list
                    list.Add(si);
                }

            // close command and result set objects
            reader.Close();

            return list;
        }



        /// <summary>
        /// NT-Добавить Настройку.
        /// </summary>
        /// <param name="p">Добавляемая Настройка.</param>
        public void AddSetting(SettingItem p)
        {

            SQLiteCommand ps = this.m_cmdAddSetting;

            // create if not exists
            if (ps == null)
            {
                String query = String.Format("INSERT INTO \"{0}\"(\"ns\", \"title\", \"descr\", \"val\") VALUES (?,?,?,?);", OperatorDbAdapter.TableSetting);
                ps = new SQLiteCommand(query, this.m_connection, this.m_transaction);
                // set timeout here
                ps.CommandTimeout = this.m_Timeout;
                //create parameters
                ps.Parameters.Add("a0", DbType.String);
                ps.Parameters.Add("a1", DbType.String);
                ps.Parameters.Add("a2", DbType.String);
                ps.Parameters.Add("a3", DbType.String);
                // write back
                this.m_cmdAddPlace = ps;
            }
            // set parameters
            ps.Parameters[0].Value = p.Namespace;
            ps.Parameters[1].Value = p.Title;
            ps.Parameters[2].Value = p.Description;
            ps.Parameters[3].Value = p.Path;
            //execute command
            ps.ExecuteNonQuery();

            return;
        }

        /// <summary>
        /// NT-Удалить Настройку.
        /// </summary>
        /// <param name="id">ИД Настройки.</param>
        /// <returns>Функция возвращает число измененных строк таблицы.</returns>
        public int RemoveSetting(int id)
        {
            // DELETE FROM `setting` WHERE (`id` = 1);
            return this.DeleteRow(OperatorDbAdapter.TableSetting, "id", id, this.m_Timeout);
        }

        /// <summary>
        /// NT- Изменить Настройку (title, descr, value)
        /// </summary>
        /// <param name="p">Изменяемая Настройка</param>
        /// <returns>Функция возвращает число измененных строк таблицы.</returns>
        public int UpdateSetting(SettingItem p)
        {
            SQLiteCommand ps = this.m_cmdUpdateSetting;

            // create if not exists
            if (ps == null)
            {
                String query = String.Format("UPDATE \"{0}\" SET \"ns\" = ?, \"title\" = ?, \"descr\" = ?, \"val\" = ? WHERE (\"id\" = ?);", OperatorDbAdapter.TableSetting);
                ps = new SQLiteCommand(query, this.m_connection, this.m_transaction);
                // set timeout here
                ps.CommandTimeout = this.m_Timeout;
                //create parameters
                ps.Parameters.Add("a0", DbType.String);
                ps.Parameters.Add("a1", DbType.String);
                ps.Parameters.Add("a2", DbType.String);
                ps.Parameters.Add("a3", DbType.String);
                ps.Parameters.Add("a4", DbType.Int32);
                // write back
                this.m_cmdUpdateSetting = ps;
            }
            // set parameters
            ps.Parameters[0].Value = p.Namespace;
            ps.Parameters[1].Value = p.Title;
            ps.Parameters[2].Value = p.Description;
            ps.Parameters[3].Value = p.Path;// get value as Item.Path
            ps.Parameters[4].Value = p.TableId;
            //execute command
            int result = ps.ExecuteNonQuery();

            return result;
        }

        /// <summary>
        /// NT-Remove all Settings
        /// </summary>
        public void RemoveAllSettings()
        {
            this.TableClear(TableSetting, m_Timeout);
        }
        #endregion

    }
}
