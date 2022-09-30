using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - Менеджер кэша Процедур и Мест, загружает их из БД и Библиотек Процедур.
    /// </summary>
    /// <remarks>
    /// Замена CachedDbAdapter по причине введения новой архитектуры Сборок Процедур.
    /// Теперь он должен собирать и хранить Места и Процедуры и принимать вызовы операций с ним,
    ///  перенаправляя их менеджерам соответствующих подсистем и библиотек.
    /// Для всех операций с Процедурами и Местами из кода Процедур использовать этот класс, а не Бд итп.
    /// </remarks>
    internal class ElementCacheManager
    {
        //TODO: Port this classs from Java

        // Этот класс должен быть реализован так, чтобы он открывал БД только на
        // время чтения или записи, а не держал ее постоянно открытой.
        // Так меньше возможность повредить БД при глюках OS.

        // Constants and Fields ==============================
        /**
         * Engine backreference
         */
        protected Engine m_Engine;

        /**
         * Db adapter backreference.
         */
        protected OperatorDbAdapter m_db;

        /**
         * PEM backreference.
         */
        protected ProcedureExecutionManager m_PEM;

        /**
         * Список процедур, все процедуры держим здесь в памяти. Они примерно будут
         * занимать до 1 мб на 1000 процедур.
         */
        private ProcedureCollection m_procedures;

        /**
         * Список мест, все места держим здесь в памяти. Они будут мало памяти
         * занимать, наверно..
         */
        private PlacesCollection m_places;

        /**
         * Коллекция настроек, все настройки держим в памяти.
         */
        private SettingItemCollection m_settings;

        // Constructors =============================

        /**
         * NT- Constructor
         * 
         * @param engine
         *            Engine backreference.
         * @param db
         *            Db adapter backreference.
         * @param pem
         *            PEM backreference.
         */
        public ElementCacheManager(Engine engine,
                OperatorDbAdapter db,
                ProcedureExecutionManager pem)
        {
            this.m_Engine = engine;
            this.m_db = db;
            this.m_PEM = pem;
            // create collection objects
            this.m_places = new PlacesCollection();
            this.m_procedures = new ProcedureCollection();
            this.m_settings = new SettingItemCollection();

            return;
        }

        /**
         * NT-Список процедур, все процедуры держим здесь в памяти.
         * 
         * @return the procedures collection object.
         */
        public ProcedureCollection get_ProcedureCollection()
        {
            return m_procedures;
        }

        /**
         * NT-Список мест, все места держим здесь в памяти.
         * 
         * @return the places collection object.
         */
        public PlacesCollection get_PlaceCollection()
        {
            return m_places;
        }

        /**
         * NT-Получить коллекцию настроек.
         * 
         * @return Функция возвращает объект коллекции настроек.
         */
        public SettingItemCollection get_SettingCollection()
        {
            return this.m_settings;
        }

        /**
         * NT-Get string representation of object.
         * 
         * @see java.lang.Object#toString()
         */
        @Override
    public String toString()
        {
            int procCount = 0;
            if (this.m_procedures != null)
                procCount = this.m_procedures.getCount();

            int placeCount = 0;
            if (this.m_places != null)
                placeCount = this.m_places.getCount();

            int settingCount = 0;
            if (this.m_settings != null)
                settingCount = this.m_settings.getTitleCount();

            String result = String.format("ElementCacheManager; procedures=%d; places=%d; setting keys=%d", procCount, placeCount, settingCount);

            return result;
        }

        /**
         * NT-Заполнить кеши элементов и подготовить к работе.
         * 
         * @throws Exception
         *             Ошибка.
         */
        public void Open() throws Exception
        {
        this.ReloadProceduresPlacesSettings();

        return;
    }

    /**
     * NT-Завершить работу и сбросить кеши элементов
     * 
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void Close() throws Exception
    {
        this.m_places.Clear();
        this.m_procedures.Clear();
        this.m_settings.Clear();

        return;
    }

    // *** Procedure function ***

    /**
     * NT-Добавить Процедуру в БД и обновить кеш процедур.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @param p
     *            Procedure for adding to database
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void AddProcedure(Procedure p) throws Exception
    {
        // Тут вообще-то не должно такого быть, так как все Процедуры добавляются только в БД Оператора.
        if (!p.isItemFromDatabase())
            throw new Exception(String.format("Error: cannot add Procedure \"%s\" to read-only Procedure library", p.get_Title()));
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
            throw new Exception(String.format("Error with adding Procedure \"%s\" : %s", p.get_Title(), e.toString()));
        }
        finally
{
    // close db connection
    this.m_db.Close();
}

return;
    }

    /**
     * NT-Добавить несколько Процедур в БД и обновить кеш процедур.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @param procedures
     *            Список заполненных Процедур
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void AddProcedure(LinkedList<Procedure> procedures) throws Exception
{
        // TODO: тут надо весь список Процеду добавить в БД атомарно: либо весь, либо ничего.
        // значит, это надо делать в пределах одной транзакции.

        // если список пустой, сразу выйти
        if (procedures.size() <= 0)
            return;
        // 1. Проверить, что все объекты списка предназначены для записи в бд, иначе выбросить исключение.
        for (Procedure p : procedures)
            if (p.isItemFromDatabase() == false)
                throw new Exception(String.format("Error: cannot add Procedure \"%s\" to read-only Procedure library", p.get_Title()));
// 2. Добавить объект в БД
Procedure p_ref = procedures.get(0);
try
{
    // sure database is opened
    this.m_db.Open();
    // add procedures
    for (Procedure p : procedures)
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
    throw new Exception(String.format("Error with adding Procedure \"%s\" : %s", p_ref.get_Title(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}
return;
    }

    /**
     * NT-Remove Procedure from database и обновить кеш процедур.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @param p
     *            Procedure for remove
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void RemoveProcedure(Procedure p) throws Exception
{
        if (!p.isItemFromDatabase())
            throw new Exception(String.format("Error: cannot delete Procedure \"%s\" from read-only Procedure library", p.get_Title()));
// else
try
{
    // sure database is opened
    this.m_db.Open();
    // add procedure
    this.m_db.RemoveProcedure(p.get_TableId());
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
    throw new Exception(String.format("Error with deleting Procedure \"%s\" : %s", p.get_Title(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}

return;
    }

    /**
     * NT-Update Procedure in database и обновить кеш процедур.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @param p
     *            Procedure for update
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void UpdateProcedure(Procedure p) throws Exception
{
        if (!p.isItemFromDatabase())
            throw new Exception(String.format("Error: cannot update Procedure \"%s\" from read-only Procedure library", p.get_Title()));
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
    throw new Exception(String.format("Error with update Procedure \"%s\" : %s", p.get_Title(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}

return;
    }

    /**
     * NT-Удалить все Процедуры из БД Оператора и обновить кеш процедур.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void RemoveAllProceduresFromDatabase() throws Exception
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
        throw new Exception(e.toString());
    }
        finally
        {
        // close db connection
        this.m_db.Close();
    }

        return;
}

// ==== Place function ==============================
/**
 * NT-Добавить новое место и обновить кеш мест.
 * БД открывается, если еще не открыта, затем закрывается.
 * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
 * 
 * @param p
 *            Добавляемое место
 * @throws Exception
 *             Ошибка при использовании БД.
 */
public void AddPlace(Place p) throws Exception
{
        // Тут вообще-то не должно такого быть, так как все Места добавляются только в БД Оператора.
        if (!p.isItemFromDatabase())
            throw new Exception(String.format("Error: cannot add Place \"%s\" to read-only Procedure library", p.get_Title()));
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
    throw new Exception(String.format("Error with adding Place \"%s\" : %s", p.get_Title(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}

return;
    }

    /**
     * NT-Добавить несколько Мест в БД и обновить кеш мест.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @param places
     *            Список заполненных Мест
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void AddPlace(LinkedList<Place> places) throws Exception
{
        // TODO: тут надо весь список Мест добавить в БД атомарно: либо весь, либо ничего.
        // значит, это надо делать в пределах одной транзакции.

        // если список пустой, сразу выйти
        if (places.size() <= 0)
            return;
        // 1. Проверить, что все объекты списка предназначены для записи в бд, иначе выбросить исключение.
        for (Place p : places)
            if (p.isItemFromDatabase() == false)
                throw new Exception(String.format("Error: cannot add Place \"%s\" to read-only Procedure library", p.get_Title()));
// 2. Добавить объект в БД
Place p_ref = places.get(0);
try
{
    // sure database is opened
    this.m_db.Open();
    // add places
    for (Place p : places)
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
    throw new Exception(String.format("Error with adding Place \"%s\" : %s", p_ref.get_Title(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}
return;
    }

    /**
     * NT-Remove Place from database и обновить кеш мест.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @param p
     *            Place for remove.
     * @throws Exception
     *             Ошибка при исполнении.
     */
    public void RemovePlace(Place p) throws Exception
{
        if (!p.isItemFromDatabase())
            throw new Exception(String.format("Error: cannot delete Place \"%s\" from read-only Procedure library", p.get_Title()));
// else
try
{
    // sure database is opened
    this.m_db.Open();
    // remove place
    this.m_db.RemovePlace(p.get_TableId());
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
    throw new Exception(String.format("Error with deleting Place \"%s\" : %s", p.get_Title(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}

return;

    }

    /**
     * NT-Update Place in database и обновить кеш мест.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @param p
     *            Place for update.
     * @throws Exception
     *             Ошибка при исполнении.
     */
    public void UpdatePlace(Place p) throws Exception
{

        if (!p.isItemFromDatabase())
            throw new Exception(String.format("Error: cannot update Place \"%s\" from read-only Procedure library", p.get_Title()));
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
    throw new Exception(String.format("Error with update Place \"%s\" : %s", p.get_Title(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}

return;

    }

    /**
     * NT-Удалить все Места из БД Оператора и обновить кеш процедур.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void RemoveAllPlacesFromDatabase() throws Exception
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
        throw new Exception(e.toString());
    }
        finally
        {
        // close db connection
        this.m_db.Close();
    }

        return;
}

// ==== Setting function ==============================
/**
 * NT-Добавить новую настройку и обновить кеш настроек.
 * БД открывается, если еще не открыта, затем закрывается.
 * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
 * 
 * @param p
 *            Добавляемая настройка
 * @throws Exception
 *             Ошибка при использовании БД.
 */
public void AddSetting(SettingItem p) throws Exception
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
        throw new Exception(String.format("Error with adding Setting \"%s\" : %s", p.toString(), e.toString()));
    }
        finally
        {
        // close db connection
        this.m_db.Close();
    }

        return;
}


/**
 * NT-Добавить несколько Настроек в БД и обновить кеш настроек.
 * БД открывается, если еще не открыта, затем закрывается.
 * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
 * 
 * @param settings
 *            Список заполненных Настроек
 * @throws Exception
 *             Ошибка при использовании БД.
 */
public void AddSetting(LinkedList<SettingItem> settings) throws Exception
{
        // TODO: тут надо весь список Настроек добавить в БД атомарно: либо весь, либо ничего.
        // значит, это надо делать в пределах одной транзакции.

        // если список пустой, сразу выйти
        if (settings.size() <= 0)
            return;
        // 1. Проверить, что все объекты списка предназначены для записи в бд, иначе выбросить исключение.
        for (SettingItem p : settings)
            if (p.isItemFromDatabase() == false)
                throw new Exception(String.format("Error: cannot add Setting \"%s\" to Database.", p.get_Title()));
// 2. Добавить объект в БД
SettingItem p_ref = settings.get(0);
try
{
    // sure database is opened
    this.m_db.Open();
    // add places
    for (SettingItem p : settings)
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
    throw new Exception(String.format("Error with adding Setting \"%s\" : %s", p_ref.toString(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}
return;
    }

    /**
     * NT-Удалить Настройку из БД и обновить кеш Настроек.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @param p
     *            Удаляемая настройка.
     * @throws Exception
     *             Ошибка при исполнении.
     */
    public void RemoveSetting(SettingItem p) throws Exception
{
        if (!p.isItemFromDatabase())
            throw new Exception(String.format("Error: cannot delete Setting \"%s\" - is not from Database", p.toString()));
// else
try
{
    // sure database is opened
    this.m_db.Open();
    // remove place
    this.m_db.RemoveSetting(p.get_TableId());
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
    throw new Exception(String.format("Error with deleting Setting \"%s\" : %s", p.toString(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}

return;

    }

    /**
     * NT-Изменить Настройку в БД и обновить кеш Настроек.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @param p
     *            Изменяемая Настройка.
     * @throws Exception
     *             Ошибка при исполнении.
     */
    public void UpdateSetting(SettingItem p) throws Exception
{
        if (!p.isItemFromDatabase())
            throw new Exception(String.format("Error: cannot update Setting \"%s\" with invalid ID to Database.", p.toString()));
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
    throw new Exception(String.format("Error with update Setting \"%s\" : %s", p.toString(), e.toString()));
}
finally
{
    // close db connection
    this.m_db.Close();
}

return;

    }

    /**
     * NT-Удалить все Места из БД Оператора и обновить кеш процедур.
     * БД открывается, если еще не открыта, затем закрывается.
     * Если выброшено исключение, транзакция откатывается и исключение перевыбрасывается.
     * 
     * @throws Exception
     *             Ошибка при использовании БД.
     */
    public void RemoveAllSettingsFromDatabase() throws Exception
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
        throw new Exception(e.toString());
    }
        finally
        {
        // close db connection
        this.m_db.Close();
    }

        return;
}

// ========= Reloading functions =========================
/**
 * NT-Перезагрузить кеш-коллекции мест данными из источника.
 * Чтобы они соответствовали содержимому источника, если он был изменен.
 * БД должна быть уже открыта и не будет закрыта в коде функции.
 * Таблица не будет изменена, коммит транзакции не нужен.
 * 
 * @throws Exception
 *             Ошибка при использовании БД.
 */
protected void reloadPlaces() throws Exception
{
        // тут заполнить коллекцию мест данными мест.
        this.m_places.Clear();
    // 1. добавить из БД
    LinkedList<Place> llp = this.m_db.GetAllPlaces();
        this.m_places.Fill(llp);
    // 2. Добавить из PEM (из Библиотек Процедур)
    LinkedList<Place> llp2 = this.m_PEM.GetAllPlaces();
        this.m_places.Fill(llp2);
    // 3. постобработка Мест?
    // - TODO: проверить что постобработка объектов мест выполнена.
    // 4. Сортировка коллекции Мест по названию - невозможна, там словарь,
    // можно сортировать по названию только при получении общего списка Мест.

    // clean up
    llp.clear();
    llp = null;
    llp2.clear();
    llp2 = null;

        return;
}

/**
 * NT-Перезагрузить кеш-коллекции процедур данными из источника.
 * Чтобы они соответствовали содержимому источника, если он был изменен.
 * БД должна быть уже открыта и не будет закрыта в коде функции.
 * Таблица не будет изменена, коммит транзакции не нужен.
 * 
 * @throws Exception
 *             Ошибка при использовании БД.
 */
protected void reloadProcedures() throws Exception
{
        // тут заполнить коллекцию процедур данными процедур.
        // И не забыть их отсортировать по весу.
        this.m_procedures.Clear();
    // 1. добавить из БД
    LinkedList<Procedure> llp = this.m_db.GetAllProcedures();
        this.m_procedures.Fill(llp);
    // 2. Добавить из PEM (из Библиотек Процедур)
    LinkedList<Procedure> llp2 = this.m_PEM.GetAllProcedures();
        this.m_procedures.Fill(llp2);
    // 3. постобработка Процедур?
    // - TODO: проверить что постобработка объектов выполнена.
    // 4. Сортировка коллекции Процедур по весу - already done in Fill() function.

    // clean up
    llp.clear();
    llp = null;
    llp2.clear();
    llp2 = null;

        return;
}

/**
 * NT-Перезагрузить кеш-коллекции настроек данными из источника.
 * Чтобы они соответствовали содержимому источника, если он был изменен.
 * БД должна быть уже открыта и не будет закрыта в коде функции.
 * Таблица не будет изменена, коммит транзакции не нужен.
 * 
 * @throws Exception
 *             Ошибка при использовании БД.
 */
protected void reloadSettings() throws Exception
{
        // тут заполнить коллекцию настроек данными настроек
        this.m_settings.Clear();
    // 1. добавить из БД
    LinkedList<SettingItem> llp = this.m_db.GetAllSettings();
        this.m_settings.addItems(llp);
    // 2. Добавить из PEM (из Библиотек Процедур)
    LinkedList<SettingItem> llp2 = this.m_PEM.GetAllSettings();
        this.m_settings.addItems(llp2);
    // 3. постобработка Настроек?
    // - TODO: проверить что постобработка объектов выполнена.

    // clean up
    llp.clear();
    llp2.clear();
    llp = null;
    llp2 = null;

        return;
}

/**
 * NT- Перезагрузить кеш-коллекции Процедур и Мест из БД и Библиотек Процедур.
 * БД открывается, если еще не открыта, затем закрывается.
 * 
 * @throws Exception
 *             Ошибка при использовании БД.
 */
protected void ReloadProceduresPlacesSettings() throws Exception
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

// *** Получить Процедуры и Места как списки ***
/**
 * NT-Получить список всех Процедур для перечисления.
 * 
 * @return Функция возвращает список всех Процедур для перечисления.
 */
public LinkedList<Procedure> getProceduresAsList()
{
    return this.m_procedures.get_Procedures();
}

/**
 * NT-Получить список всех Мест для перечисления.
 * 
 * @return Функция возвращает список всех Мест для перечисления.
 */
public LinkedList<Place> getPlacesAsList()
{
    return this.m_places.getPlacesAsList();
}
/**
 * NT-Получить список всех Настроек для перечисления.
 * 
 * @return Функция возвращает список всех Настроек для перечисления.
 */
public LinkedList<SettingItem> getSettingsAsList()
{
    return this.m_settings.getAllItems();
}

/** NT-Получить первое же значение настройки.
 * @param title Название настройки.
 * @return Функция возвращает значение настройки либо null, если настройка не найдена.
 */
public String getSettingFirstValue(String title)
{
    String result = null;
    SettingItem si = this.m_settings.getFirstItem(title);
    if (si != null)
        result = si.get_Path().trim();

    return result;
}

/**
 * NT- Получить облако тегов-неймспейсов для элементов этого менеджера.
 * 
 * @param forProcedures
 *            Извлекать теги коллекции Процедур.
 * @param forPlaces
 *            Извлекать теги коллекции Мест.
 * @param forSettings
 *            Извлекать теги коллекции Настроек.
 * @return
 *         Функция возвращает строку - перечисление названий неймспейсов элементов этого менеджера.
 */
public String getNamespacesChainString(
        boolean forProcedures,
        boolean forPlaces,
        boolean forSettings)
{
    // Получить сортированный массив уникальных названий неймспейсов
    String[] nss = this.getNamespaces(forProcedures, forPlaces, forSettings, true);
    // собрать их в строку с разделителем - пробелом или табом.
    // StringBuilder sb = new StringBuilder();
    // for(String s : nss)
    // sb.append(s).append(", ");
    // result = sb.toString().trim();

    //заменено на 
    String result = String.join(", ", nss);

    return result.trim();
}

/** NT-Получить массив названий неймспейсов для элементов этого менеджера.
 * @param forProcedures Извлекать теги коллекции Процедур.
 * @param forPlaces Извлекать теги коллекции Мест.
 * @param forSettings Извлекать теги коллекции Настроек.
 * @param sorted Сортировать по алфавиту.
 * @return
 * Функция возвращает массив названий неймспейсов элементов этого менеджера.
 */
public String[] getNamespaces(boolean forProcedures, boolean forPlaces, boolean forSettings, boolean sorted)
{
    HashSet<String> set = new HashSet<String>();
    HashSet<String> nss = null;
    //add default namespace constant
    set.add(NamespaceConstants.NsDefault);
    //add Procedure namespaces
    if (forProcedures == true)
    {
        nss = this.m_procedures.getNamespaces();
        set.addAll(nss);
    }
    //add Place namespaces
    if (forPlaces == true)
    {
        nss = this.m_places.getNamespaces();
        set.addAll(nss);
    }
    //add Setting namespaces
    if (forSettings == true)
    {
        nss = this.m_settings.getNamespaces();
        set.addAll(nss);
    }
    //form output array
    String[] result = set.toArray(new String[set.size()]);
    if (sorted)
        Arrays.sort(result);

    return result;
}




    }
}
