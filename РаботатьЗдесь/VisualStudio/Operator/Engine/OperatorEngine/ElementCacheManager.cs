using System;
using System.Collections.Generic;
using Engine.DbSubsystem;
using Engine.ProcedureSubsystem;
using Engine.SettingSubsystem;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NT - Менеджер кэша Процедур и Мест, загружает их из БД и Библиотек Процедур.
    /// </summary>
    /// <remarks>
    /// Замена CachedDbAdapter по причине введения новой архитектуры Сборок Процедур.
    /// Теперь он должен собирать и хранить Места и Процедуры и принимать вызовы операций с ним,
    ///  перенаправляя их менеджерам соответствующих подсистем и библиотек.
    /// Для всех операций с Процедурами и Местами из кода Процедур использовать этот класс, а не Бд итп.
    /// </remarks>
    internal class ElementCacheManager : EngineSubsystem
    {
        //- Неймспейсы лучше всего хранить в объектах HashSet или SortedSet - решить.

        // Этот класс должен быть реализован так, чтобы он открывал БД только на
        // время чтения или записи, а не держал ее постоянно открытой.
        // Так меньше возможность повредить БД при глюках OS.

        #region *** Constants and Fields ***

        /// <summary>
        /// Db adapter backreference
        /// </summary>
        protected OperatorDbAdapter m_db;

        /// <summary>
        /// PEM backreference.
        /// </summary>
        protected ProcedureExecutionManager m_PEM;

        /// <summary>
        /// Список процедур, все процедуры держим здесь в памяти. Они примерно будут занимать до 1 мб на 1000 процедур.
        /// </summary>
        private ProcedureCollection m_procedures;

        /// <summary>
        /// Список мест, все места держим здесь в памяти. Они будут мало памяти занимать, наверно..
        /// </summary>
        private PlacesCollection m_places;

        /// <summary>
        /// Коллекция настроек, все настройки держим в памяти.
        /// </summary>
        private SettingItemCollection m_settings;

        #endregion

        #region *** Constructors ***

        /// <summary>
        /// NT- Constructor
        /// </summary>
        /// <param name="engine">Engine backreference.</param>
        /// <param name="db">Db adapter backreference.</param>
        /// <param name="pem">PEM backreference.</param>
        public ElementCacheManager(Engine engine,
                OperatorDbAdapter db,
                ProcedureExecutionManager pem) : base(engine)
        {
            //this.m_Engine = engine; done in base class
            this.m_db = db;
            this.m_PEM = pem;
            // create collection objects
            this.m_places = new PlacesCollection();
            this.m_procedures = new ProcedureCollection();
            this.m_settings = new SettingItemCollection();

            return;
        }

        #endregion

        #region ***  Properties ***

        /// <summary>
        /// NT-Список процедур, все процедуры держим здесь в памяти.
        /// </summary>
        public ProcedureCollection Procedures
        {
            get { return this.m_procedures; }
        }

        /// <summary>
        /// NT-Список мест, все места держим здесь в памяти
        /// </summary>
        public PlacesCollection Places
        {
            get { return this.m_places; }
        }

        /// <summary>
        /// NT-Получить коллекцию настроек.
        /// </summary>
        public SettingItemCollection Settings
        {
            get { return this.m_settings; }
        }

        #endregion

        /// <summary>
        /// NT-Get string representation of object.
        /// </summary>
        public override String ToString()
        {
            int procCount = 0;
            if (this.m_procedures != null)
                procCount = this.m_procedures.Count;

            int placeCount = 0;
            if (this.m_places != null)
                placeCount = this.m_places.Count;

            int settingCount = 0;
            if (this.m_settings != null)
                settingCount = this.m_settings.getTitleCount();

            String result = String.Format("ElementCacheManager; procedures={0}; places={1}; setting keys={2}", procCount, placeCount, settingCount);

            return result;
        }

        #region  *** Override this in child classes ***

        /// <summary>
        /// NT-Initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onOpen()
        {
            //Заполнить кеши элементов и подготовить к работе.
            this.ReloadProceduresPlacesSettings();
        }

        /// <summary>
        /// NT-De-initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onClose()
        {
            //Завершить работу и сбросить кеши элементов
            this.m_places.Clear();
            this.m_procedures.Clear();
            this.m_settings.Clear();

            return;
        }

        #endregion

        #region *** Procedure function ***

        /// <summary>
        /// NT-Добавить Процедуру в БД и обновить кеш процедур.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="p">Procedure for adding to database</param>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void AddProcedure(Procedure p)
        {
            // Тут вообще-то не должно такого быть, так как все Процедуры добавляются только в БД Оператора.
            if (!p.isItemFromDatabase())
                throw new Exception(String.Format("Error: cannot add Procedure \"{0}\" to read-only Procedure library", p.Title));
            // else
            try
            {
                // sure database is opened
                this.m_db.Open();
                // add procedure
                this.m_db.AddProcedure(p);
                // reload cache
                this.reloadProcedures();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with adding Procedure \"{0}\" : {1}", p.Title, e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        /// <summary>
        /// NT-Добавить несколько Процедур в БД и обновить кеш процедур.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="procedures"> Список заполненных Процедур</param>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void AddProcedure(List<Procedure> procedures)
        {
            // TODO: тут надо весь список Процедур добавить в БД атомарно: либо весь, либо ничего.
            // значит, это надо делать в пределах одной транзакции.

            // если список пустой, сразу выйти
            if (procedures.Count <= 0)
                return;
            // 1. Проверить, что все объекты списка предназначены для записи в бд, иначе выбросить исключение.
            foreach (Procedure p in procedures)
                if (p.isItemFromDatabase() == false)
                    throw new Exception(String.Format("Error: cannot add Procedure \"{0}\" to read-only Procedure library", p.Title));
            // 2. Добавить объект в БД
            Procedure p_ref = procedures[0];
            try
            {
                // sure database is opened
                this.m_db.Open();
                // add procedures
                foreach (Procedure p in procedures)
                {
                    this.m_db.AddProcedure(p);
                    p_ref = p; // for exception string formatting
                }
                // reload cache
                this.reloadProcedures();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.

                // commit transaction
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with adding Procedure \"{0}\" : {1}", p_ref.Title, e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }
            return;
        }

        /// <summary>
        /// NT-Remove Procedure from database и обновить кеш процедур.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="p">Procedure for remove</param>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void RemoveProcedure(Procedure p)
        {
            if (!p.isItemFromDatabase())
                throw new Exception(String.Format("Error: cannot delete Procedure \"{0}\" from read-only Procedure library", p.Title));
            // else
            try
            {
                // sure database is opened
                this.m_db.Open();
                // add procedure
                this.m_db.RemoveProcedure(p.TableId);
                // reload cache
                this.reloadProcedures();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with deleting Procedure \"{0}\" : {1}", p.Title, e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        /// <summary>
        /// NT-Update Procedure in database и обновить кеш процедур.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="p">Procedure for update</param>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void UpdateProcedure(Procedure p)
        {
            if (!p.isItemFromDatabase())
                throw new Exception(String.Format("Error: cannot update Procedure \"{0}\" from read-only Procedure library", p.Title));
            // else
            try
            {
                // sure database is opened
                this.m_db.Open();
                // add procedure
                this.m_db.UpdateProcedure(p);
                // reload cache
                this.reloadProcedures();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with update Procedure \"{0}\" : {1}", p.Title, e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        /// <summary>
        /// NT-Удалить все Процедуры из БД Оператора и обновить кеш процедур.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void RemoveAllProceduresFromDatabase()
        {
            try
            {
                // sure database is opened
                this.m_db.Open();
                // remove procedures
                this.m_db.RemoveAllProcedures();
                // reload cache
                this.reloadProcedures();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(e.ToString());
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        #endregion

        #region *** Place function ***

        /// <summary>
        /// NT-Добавить новое место и обновить кеш мест.
        /// </summary>
        /// <remarks>
        ///  БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="p">Добавляемое место</param>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void AddPlace(Place p)
        {
            // Тут вообще-то не должно такого быть, так как все Места добавляются только в БД Оператора.
            if (!p.isItemFromDatabase())
                throw new Exception(String.Format("Error: cannot add Place \"{0}\" to read-only Procedure library", p.Title));
            // else
            try
            {
                // sure database is opened
                this.m_db.Open();
                // add place
                this.m_db.AddPlace(p);
                // reload cache
                this.reloadPlaces();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with adding Place \"{0}\" : {1}", p.Title, e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        /// <summary>
        /// NT-Добавить несколько Мест в БД и обновить кеш мест.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="places">Список заполненных Мест</param>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void AddPlace(List<Place> places)
        {
            // TODO: тут надо весь список Мест добавить в БД атомарно: либо весь, либо ничего.
            // значит, это надо делать в пределах одной транзакции.

            // если список пустой, сразу выйти
            if (places.Count <= 0)
                return;
            // 1. Проверить, что все объекты списка предназначены для записи в бд, иначе выбросить исключение.
            foreach (Place p in places)
                if (p.isItemFromDatabase() == false)
                    throw new Exception(String.Format("Error: cannot add Place \"{0}\" to read-only Procedure library", p.Title));
            // 2. Добавить объект в БД
            Place p_ref = places[0];
            try
            {
                // sure database is opened
                this.m_db.Open();
                // add places
                foreach (Place p in places)
                {
                    this.m_db.AddPlace(p);
                    p_ref = p; // for exception string formatting
                }
                // reload cache
                this.reloadPlaces();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.

                // commit transaction
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with adding Place \"{0}\" : {1}", p_ref.Title, e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }
            return;
        }

        /// <summary>
        /// NT-Remove Place from database и обновить кеш мест.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="p">Place for remove.</param>
        /// <exception cref="Exception">Ошибка при исполнении.</exception>
        public void RemovePlace(Place p)
        {
            if (!p.isItemFromDatabase())
                throw new Exception(String.Format("Error: cannot delete Place \"{0}\" from read-only Procedure library", p.Title));
            // else
            try
            {
                // sure database is opened
                this.m_db.Open();
                // remove place
                this.m_db.RemovePlace(p.TableId);
                // reload cache
                this.reloadPlaces();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with deleting Place \"{0}\" : {1}", p.Title, e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        /// <summary>
        /// NT-Update Place in database и обновить кеш мест.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="p">Place for update.</param>
        /// <exception cref="Exception">Ошибка при исполнении</exception>
        public void UpdatePlace(Place p)
        {
            if (!p.isItemFromDatabase())
                throw new Exception(String.Format("Error: cannot update Place \"{0}\" from read-only Procedure library", p.Title));
            // else
            try
            {
                // sure database is opened
                this.m_db.Open();
                // update place
                this.m_db.UpdatePlace(p);
                // reload cache
                this.reloadPlaces();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with update Place \"{0}\" : {1}", p.Title, e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        /// <summary>
        /// NT-Удалить все Места из БД Оператора и обновить кеш процедур.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void RemoveAllPlacesFromDatabase()
        {
            try
            {
                // sure database is opened
                this.m_db.Open();
                // remove places
                this.m_db.RemoveAllPlaces();
                // reload cache
                this.reloadPlaces();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(e.ToString());
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        #endregion

        #region *** Setting function ***

        /// <summary>
        /// NT-Добавить новую настройку и обновить кеш настроек.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="p">Добавляемая настройка</param>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void AddSetting(SettingItem p)
        {

            try
            {
                // sure database is opened
                this.m_db.Open();
                // add place
                this.m_db.AddSetting(p);
                // reload cache
                this.reloadSettings();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with adding Setting \"{0}\" : {1}", p.ToString(), e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        /// <summary>
        /// NT-Добавить несколько Настроек в БД и обновить кеш настроек.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="settings">Список заполненных Настроек</param>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void AddSetting(List<SettingItem> settings)
        {
            // TODO: тут надо весь список Настроек добавить в БД атомарно: либо весь, либо ничего.
            // значит, это надо делать в пределах одной транзакции.

            // если список пустой, сразу выйти
            if (settings.Count <= 0)
                return;
            // 1. Проверить, что все объекты списка предназначены для записи в бд, иначе выбросить исключение.
            foreach (SettingItem p in settings)
                if (p.isItemFromDatabase() == false)
                    throw new Exception(String.Format("Error: cannot add Setting \"{0}\" to Database.", p.Title));
            // 2. Добавить объект в БД
            SettingItem p_ref = settings[0];
            try
            {
                // sure database is opened
                this.m_db.Open();
                // add places
                foreach (SettingItem p in settings)
                {
                    this.m_db.AddSetting(p);
                    p_ref = p; // for exception string formatting
                }
                // reload cache
                this.reloadSettings();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.

                // commit transaction
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with adding Setting \"{0}\" : {1}", p_ref.ToString(), e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }
            return;
        }

        /// <summary>
        /// NT-Удалить Настройку из БД и обновить кеш Настроек.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="p">Удаляемая настройка.</param>
        /// <exception cref="Exception">Ошибка при исполнении.</exception>
        public void RemoveSetting(SettingItem p)
        {
            if (!p.isItemFromDatabase())
                throw new Exception(String.Format("Error: cannot delete Setting \"{0}\" - is not from Database", p.ToString()));
            // else
            try
            {
                // sure database is opened
                this.m_db.Open();
                // remove place
                this.m_db.RemoveSetting(p.TableId);
                // reload cache
                this.reloadSettings();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with deleting Setting \"{0}\" : {1}", p.ToString(), e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        /// <summary>
        /// NT-Изменить Настройку в БД и обновить кеш Настроек.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <param name="p">Изменяемая Настройка.</param>
        /// <exception cref="Exception">Ошибка при исполнении.</exception>
        public void UpdateSetting(SettingItem p)
        {
            if (!p.isItemFromDatabase())
                throw new Exception(String.Format("Error: cannot update Setting \"{0}\" with invalid ID to Database.", p.ToString()));
            // else
            try
            {
                // sure database is opened
                this.m_db.Open();
                // update place
                this.m_db.UpdateSetting(p);
                // reload cache
                this.reloadSettings();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(String.Format("Error with update Setting \"{0}\" : {1}", p.ToString(), e.ToString()));
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        /// <summary>
        /// NT-Удалить все Места из БД Оператора и обновить кеш процедур.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
        /// </remarks>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        public void RemoveAllSettingsFromDatabase()
        {
            try
            {
                // sure database is opened
                this.m_db.Open();
                // remove settings
                this.m_db.RemoveAllSettings();
                // reload cache
                this.reloadSettings();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
                // commit changes
                this.m_db.TransactionCommit();
            }
            catch (Exception e)
            {
                // cancel adding and rethrow exception
                this.m_db.TransactionRollback();
                throw new Exception(e.ToString());
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        #endregion

        #region *** Reloading functions ***

        /// <summary>
        /// NT-Перезагрузить кеш-коллекции мест данными из источника, чтобы они соответствовали содержимому источника, если он был изменен.
        /// </summary>
        /// <remarks>
        /// БД должна быть уже открыта и не будет закрыта в коде функции.
        /// Таблица не будет изменена, коммит транзакции не нужен.
        /// </remarks>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        protected void reloadPlaces()
        {
            // тут заполнить коллекцию мест данными мест.
            this.m_places.Clear();
            // 1. добавить из БД
            List<Place> llp = this.m_db.GetAllPlaces();
            this.m_places.Fill(llp);
            // 2. Добавить из PEM (из Библиотек Процедур)
            List<Place> llp2 = this.m_PEM.GetAllPlaces();
            this.m_places.Fill(llp2);
            // 3. постобработка Мест?
            // - TODO: проверить что постобработка объектов мест выполнена.
            // 4. Сортировка коллекции Мест по названию - невозможна, там словарь,
            // можно сортировать по названию только при получении общего списка Мест.

            // clean up
            llp.Clear();
            llp = null;
            llp2.Clear();
            llp2 = null;

            return;
        }

        /// <summary>
        /// NT-Перезагрузить кеш-коллекции процедур данными из источника. Чтобы они соответствовали содержимому источника, если он был изменен.
        /// </summary>
        /// <remarks>
        /// БД должна быть уже открыта и не будет закрыта в коде функции.
        /// Таблица не будет изменена, коммит транзакции не нужен.
        /// </remarks>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        protected void reloadProcedures()
        {
            // тут заполнить коллекцию процедур данными процедур.
            // И не забыть их отсортировать по весу.
            this.m_procedures.Clear();
            // 1. добавить из БД
            List<Procedure> llp = this.m_db.GetAllProcedures();
            this.m_procedures.Fill(llp);
            // 2. Добавить из PEM (из Библиотек Процедур)
            List<Procedure> llp2 = this.m_PEM.GetAllProcedures();
            this.m_procedures.Fill(llp2);
            // 3. постобработка Процедур?
            // - TODO: проверить что постобработка объектов выполнена.
            // 4. Сортировка коллекции Процедур по весу - already done in Fill() function.

            // clean up
            llp.Clear();
            llp = null;
            llp2.Clear();
            llp2 = null;

            return;
        }

        /// <summary>
        /// NT-Перезагрузить кеш-коллекции настроек данными из источника. Чтобы они соответствовали содержимому источника, если он был изменен.
        /// </summary>
        /// <remarks>
        ///  БД должна быть уже открыта и не будет закрыта в коде функции.
        /// Таблица не будет изменена, коммит транзакции не нужен.
        /// </remarks>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        protected void reloadSettings()
        {
            // тут заполнить коллекцию настроек данными настроек
            this.m_settings.Clear();
            // 1. добавить из БД
            List<SettingItem> llp = this.m_db.GetAllSettings();
            this.m_settings.addItems(llp);
            // 2. Добавить из PEM (из Библиотек Процедур)
            List<SettingItem> llp2 = this.m_PEM.GetAllSettings();
            this.m_settings.addItems(llp2);
            // 3. постобработка Настроек?
            // - TODO: проверить что постобработка объектов выполнена.

            // clean up
            llp.Clear();
            llp2.Clear();
            llp = null;
            llp2 = null;

            return;
        }

        /// <summary>
        /// NT- Перезагрузить кеш-коллекции Процедур и Мест из БД и Библиотек Процедур.
        /// </summary>
        /// <remarks>
        /// БД открывается, если еще не открыта, затем закрывается.
        /// </remarks>
        /// <exception cref="Exception">Ошибка при использовании БД.</exception>
        protected void ReloadProceduresPlacesSettings()
        {
            try
            {
                // sure database is opened
                this.m_db.Open();
                // reload items
                // reload cache
                this.reloadPlaces();
                this.reloadProcedures();
                this.reloadSettings();
                // TODO: если тут возникнет исключение, то в кеше будут неправильные данные - в нем же нельзя откатить транзакцию.
                // А хорошо бы иметь возможность полностью откатить изменения и в кеше тоже.
            }
            finally
            {
                // close db connection
                this.m_db.Close();
            }

            return;
        }

        #endregion

        #region *** Получить Процедуры и Места как списки ***

        /// <summary>
        /// NT-Получить список всех Процедур для перечисления.
        /// </summary>
        /// <returns>Функция возвращает список всех Процедур для перечисления.</returns>
        public List<Procedure> getProceduresAsList()
        {
            return this.m_procedures.Items;
        }

        /// <summary>
        /// NT-Получить список всех Мест для перечисления.
        /// </summary>
        /// <returns>Функция возвращает список всех Мест для перечисления.</returns>
        public List<Place> getPlacesAsList()
        {
            return this.m_places.getPlacesAsList();
        }

        /// <summary>
        /// NT-Получить список всех Настроек для перечисления.
        /// </summary>
        /// <returns>Функция возвращает список всех Настроек для перечисления.</returns>
        public List<SettingItem> getSettingsAsList()
        {
            return this.m_settings.getAllItems();
        }

        #endregion

        /// <summary>
        /// NT-Получить первое же значение настройки.
        /// </summary>
        /// <param name="title">Название настройки.</param>
        /// <returns>Функция возвращает значение настройки либо null, если настройка не найдена.</returns>
        public String getSettingFirstValue(String title)
        {
            //TODO: Почему эту функцию нельзя перенести в МенеджерНастроек или КоллекциюНастроек?
            String result = null;
            SettingItem si = this.m_settings.getFirstItem(title);
            if (si != null)
                result = si.Path.Trim();

            return result;
        }

        #region *** Namespace functions ***

        //TODO: функции неймспейсов не следует ли перенести в собственный класс?      

        /// <summary>
        /// NT- Получить облако тегов-неймспейсов для элементов этого менеджера.
        /// </summary>
        /// <param name="forProcedures">Извлекать теги коллекции Процедур.</param>
        /// <param name="forPlaces">Извлекать теги коллекции Мест.</param>
        /// <param name="forSettings">Извлекать теги коллекции Настроек.</param>
        /// <returns>Функция возвращает строку - перечисление названий неймспейсов элементов этого менеджера.</returns>
        public String getNamespacesChainString(
        bool forProcedures,
        bool forPlaces,
        bool forSettings)
        {
            // Получить сортированный массив уникальных названий неймспейсов
            List<String> nss = this.getNamespaces(forProcedures, forPlaces, forSettings, true);
            // собрать их в строку с разделителем - пробелом или табом. 
            String result = String.Join(", ", nss.ToArray());

            return result.Trim();
        }

        /// <summary>
        /// NT-Получить список названий неймспейсов для элементов этого менеджера.
        /// </summary>
        /// <param name="forProcedures">Извлекать теги коллекции Процедур.</param>
        /// <param name="forPlaces">Извлекать теги коллекции Мест.</param>
        /// <param name="forSettings">Извлекать теги коллекции Настроек.</param>
        /// <param name="sorted">Сортировать по алфавиту.</param>
        /// <returns>Функция возвращает список названий неймспейсов элементов этого менеджера.</returns>
        public List<String> getNamespaces(bool forProcedures, bool forPlaces, bool forSettings, bool sorted)
        {
            //Это набор уникальных значений, добавление не вызывает исключения.
            HashSet<String> set = new HashSet<String>();
            HashSet<String> nss = null;
            //add default namespace constant
            set.Add(NamespaceConstants.NsDefault);
            //add Procedure namespaces
            if (forProcedures == true)
            {
                nss = this.m_procedures.getNamespaces();
                foreach (String s in nss)
                    set.Add(s);
            }
            //add Place namespaces
            if (forPlaces == true)
            {
                nss = this.m_places.getNamespaces();
                foreach (String s in nss)
                    set.Add(s);
            }
            //add Setting namespaces
            if (forSettings == true)
            {
                nss = this.m_settings.getNamespaces();
                foreach (String s in nss)
                    set.Add(s);
            }
            //form output array
            //String[] result = set.ToArray<String>();
            List<string> result = new List<string>(set);
            if (sorted)
                result.Sort();//Array.Sort(result);

            return result;
        }

        #endregion

    }
}
