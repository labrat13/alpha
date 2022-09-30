using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.SettingSubsystem
{
    /// <summary>
    /// NR - Представляет базовый класс файла настроек приложения.
    /// </summary>
    public class ApplicationSettingsBase : Engine.OperatorEngine.EngineSubsystem
    {
        // *** Constants and Fields ***

        // Я тут долго и безуспешно пробовал варианты, в итоге, решил задать имя и расширение файла в коде и не менять его, независимо от реального формата файла
        // настроек.
        /**
         * Application settings file name
         */
        public final static String AppSettingsFileName = "settings.txt";

        /**
         * Line separator
         */
        protected final static String lineSeparator = System.lineSeparator();

        // А тут эту коллекцию наружу выдавать не будем - все ее функции повторим тут в классе, чтобы иметь весь интерфейс в этом классе, а не разбивать по
        // вложенным объектам.
        /**
         * Collection for application settings
         */
        protected SettingItemCollection m_Items;

        /**
 * Settings file pathname
 */
        protected String m_filepath;


        /// <summary>
        /// NR - Конструктор
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        public ApplicationSettingsBase(OperatorEngine.Engine engine) : base(engine)
        {
            //TODO: Add code here
        }


        #region  *** Override this from EngineSubsystem parent class ***
        /// <summary>
        /// NR - Initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onOpen()
        {
            throw new Exception("Function must be overridden");//TODO: Add code here
        }

        /// <summary>
        /// NR - De-initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onClose()
        {
            throw new Exception("Function must be overridden");//TODO: Add code here
        }
        #endregion

        // *** Constructors ***

        /**
         * Paramether constructor.
         * 
         * @param engine
         *            Engine object backreference.
         */
        public ApplicationSettingsBase(Engine engine)
        {
            this.m_Engine = engine;
            this.m_Items = new SettingItemCollection();
            this.m_filepath = null;

            return;
        }

        // *** Properties ***

        // *** Service functions ***

        /**
         * NT-Get string representation of object for debug
         */
        @Override
    public String toString()
        {
            return String.format("%i ключей из файла %s", this.m_Items.getTitleCount(), OperatorEngine.Utility.GetStringTextNull(this.m_filepath));
        }

        // *** Work functions ***

        /**
         * NT-Check setting is present
         * 
         * @param title
         *            Setting title as key
         * @return Returns true if setting present in collection, false otherwise.
         */
        public boolean hasSetting(String title)
        {
            return this.m_Items.hasTitle(title);
        }

        /**
         * NT-Get array of used titles.
         * 
         * @param sorted
         *            Sort keys.
         * @return Function returns array of used keyname strings.
         */
        public String[] getKeyArray(boolean sorted)
        {
            return this.m_Items.getTitles(sorted);
        }

        /**
         * NR-Reset settings to default values
         * 
         * @throws Exception
         *             Error on reset settings collection
         */
        public void Reset() throws Exception
        {
        throw new Exception("This function must be overridden in child classes");
    }

    /**
     * NT-Load or reload settings from file
     * 
     * @throws Exception
     *             Error's on read settings file.
     */
    public void Load() throws Exception
    {
        // open current file and read all items to dictionary
        this.Load(this.m_filepath);
    }

    /**
     * NR-Load or reload settings from file
     * 
     * @param filepath
     *            Settings file path
     * @throws Exception
     *             Error's on read settings file.
     */
    public void Load(String filepath) throws Exception
    {
        throw new Exception("This function must be overridden in child classes");
}

/**
 * NT- Write settings to file
 * 
 * @throws Exception
 *             Error's on write settings file.
 */
public void Store() throws Exception
{
        // open current file, write all items from dictionary to file and close
        // file.
        this.Store(this.m_filepath);

        return;
}

/**
 * NT- Write settings to file - if modified only
 * 
 * @throws Exception
 *             Error's on write settings file.
 */
public void StoreIfModified() throws Exception
{
        if (this.m_Items.isModified() == true)
            this.Store(this.m_filepath);

        return;
}

/**
 * NR- Write settings to file
 * 
 * @param filepath
 *            Settings file path
 * @throws Exception
 *             Error's on write settings file.
 */
public void Store(String filepath) throws Exception
{
        throw new Exception("This function must be overridden in child classes");
    }

    // *** Collection functions ***

    /**
     * Collection has been modified
     * 
     * @return the modified
     */
    public boolean isModified()
{
    return this.m_Items.isModified();
}

/**
 * Collection has been modified
 * 
 * @param modified
 *            the modified to set
 */
public void setModified(boolean modified)
{
    this.m_Items.setModified(modified);
}

/**
 * NT-Get settings item array by title
 * 
 * @param title
 *            Setting item title as key
 * @param sorted
 *            Sort items by title.
 * @return Returns SettingsItem[] array, or returns null if title not exists in
 *         collection.
 */
public SettingItem[] getItems(String title, boolean sorted)
{
    return this.m_Items.getItems(title, sorted);
}

/**
 * NT-Добавить элемент, используя поле Title в качестве ключа для словаря.
 * 
 * @param item
 *            Добавляемый элемент.
 */
public void addItem(SettingItem item)
{
    // set item storage title
    item.set_Storage(Item.StorageKeyForSettingFileItem);
    // add
    this.m_Items.addItem(item);

    return;
}

/**
 * NT-Add new settings item in collection.
 * 
 * @param group
 *            Setting item namespace as group.
 * @param title
 *            Setting item title as key.
 * @param value
 *            Setting item value as String.
 * @param descr
 *            Setting item description as multiline String.
 */
public void addItem(String group, String title, String value, String descr)
{
    SettingItem item = new SettingItem(group, title, value, descr);
    // set item storage title
    item.set_Storage(Item.StorageKeyForSettingFileItem);
    // add
    this.m_Items.addItem(item);

    return;
}

/**
 * NT-Add new settings item in collection.
 * 
 * @param group
 *            Setting item namespace as group.
 * @param title
 *            Setting item title as key.
 * @param value
 *            Setting item value as Integer.
 * @param descr
 *            Setting item description as multiline String.
 */
public void addItem(String group, String title, Integer value, String descr)
{
    String val = value.toString();
    this.addItem(group, title, val, descr);

    return;
}

/**
 * NT-Add new settings item in collection.
 * 
 * @param group
 *            Setting item namespace as group.
 * @param title
 *            Setting item title as key.
 * @param value
 *            Setting item value as Boolean.
 * @param descr
 *            Setting item description as multiline String.
 */
public void addItem(String group, String title, Boolean value, String descr)
{
    String val = value.toString();
    this.addItem(group, title, val, descr);

    return;
}

/**
 * NT-Remove from collection all items under specified title
 * 
 * @param title
 *            Setting item title as key
 */
public void removeItems(String title)
{
    this.m_Items.removeItems(title);

    return;
}

/**
 * NT - remove specified item object
 * 
 * @param item
 *            Объект, уже находящийся в этой коллекции.
 * @return Функция возвращает true, если объект был удален; функция возвращает false, если объект не был найден.
 * @throws Exception
 *             Если ключ отсутствует в словаре коллекции.
 */
public boolean removeItem(SettingItem item) throws Exception
{
        return this.m_Items.removeItem(item);
}

    // *** End of file ***


    }
}
