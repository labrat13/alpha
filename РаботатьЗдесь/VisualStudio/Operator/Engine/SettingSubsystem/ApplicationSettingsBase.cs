using System;
using Engine.OperatorEngine;
using Engine.Utility;

namespace Engine.SettingSubsystem
{
    /// <summary>
    /// NT - Представляет базовый класс файла настроек приложения.
    /// Функции должны быть реализованы в производном классе.
    /// </summary>
    public class ApplicationSettingsBase : Engine.OperatorEngine.EngineSubsystem
    {
        #region *** Constants and Fields ***

        // Я тут долго и безуспешно пробовал варианты, в итоге, решил задать имя и расширение файла в коде и не менять его, независимо от реального формата файла
        // настроек.

        //TODO: check entire class tree for architecture errors!

        /// <summary>
        /// Application settings file name
        /// </summary>
        public const String AppSettingsFileName = "settings.txt";

        /// <summary>
        /// The line separator
        /// </summary>
        protected static readonly String lineSeparator = Utility.SystemInfoManager.GetLineSeparator();

        // А тут эту коллекцию наружу выдавать не будем - все ее функции повторим тут в классе, чтобы иметь весь интерфейс в этом классе, а не разбивать по
        // вложенным объектам.

        /// <summary>
        /// Collection for application settings
        /// </summary>
        protected SettingItemCollection m_Items;

        /// <summary>
        /// Settings file pathname
        /// </summary>
        protected String m_filepath;

        #endregion
        /// <summary>
        /// NR - Конструктор
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        public ApplicationSettingsBase(OperatorEngine.Engine engine) : base(engine)
        {
            this.m_Items = new SettingItemCollection();
            this.m_filepath = String.Empty;

            return;
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


        #region *** Properties ***
        #endregion
        // *** Service functions ***

        /// <summary>
        /// NT-Get string representation of object for debug
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return String.Format("{0} ключей из файла {1}", this.m_Items.getTitleCount(), StringUtility.GetStringTextNull(this.m_filepath));
        }

        #region *** Work functions ***

        /// <summary>
        /// NT-Check setting is present
        /// </summary>
        /// <param name="title">Setting title as key.</param>
        /// <returns>
        ///   Returns true if setting present in collection, false otherwise.
        /// </returns>
        public bool hasSetting(String title)
        {
            return this.m_Items.hasTitle(title);
        }

        /// <summary>
        /// NT-Get array of used titles.
        /// </summary>
        /// <param name="sorted">Sort keys.</param>
        /// <returns>Function returns array of used keyname strings.</returns>
        public String[] getKeyArray(bool sorted)
        {
            return this.m_Items.getTitles(sorted);
        }


        /// <summary>
        /// NR-Reset settings to default values
        /// </summary>
        /// <exception cref="Exception">This function must be overridden in child classes</exception>
        public virtual void Reset()
        {
            throw new Exception("This function must be overridden in child classes");
        }


        /// <summary>
        /// NT-Load or reload settings from file
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void Load()
        {
            // open current file and read all items to dictionary
            //должно быть реализовано в производном классе
            this.Load(this.m_filepath);
        }


        /// <summary>
        /// NR-Load or reload settings from file
        /// </summary>
        /// <param name="filepath">Settings file path</param>
        /// <exception cref="Exception">This function must be overridden in child classes</exception>
        public virtual void Load(String filepath)
        {
            throw new Exception("This function must be overridden in child classes");
        }

        /// <summary>
        /// NT- Write settings to file
        /// </summary>
        public void Store()
        {
            // open current file, write all items from dictionary to file and close file.
            //должно быть реализовано в производном классе
            this.Store(this.m_filepath);

            return;
        }


        /// <summary>
        /// NT- Write settings to file - if modified only
        /// </summary>
        public void StoreIfModified()
        {
            if (this.m_Items.isModified == true)
                this.Store(this.m_filepath);

            return;
        }


        /// <summary>
        /// NR- Write settings to file
        /// </summary>
        /// <param name="filepath">Settings file path</param>
        /// <exception cref="Exception">This function must be overridden in child classes</exception>
        public virtual void Store(String filepath)
        {
            throw new Exception("This function must be overridden in child classes");
        }
        #endregion

        #region *** Collection functions ***

        /// <summary>
        /// Collection has been modified
        /// </summary>
        /// <returns></returns>
        public bool isModified
        {
            get { return this.m_Items.isModified; }
            set { this.m_Items.isModified = value; }
        }


        /// <summary>
        /// NT-Get settings item array by title
        /// </summary>
        /// <param name="title">Setting item title as key</param>
        /// <param name="sorted">Sort items by title.</param>
        /// <returns>Returns SettingsItem[] array, or returns null if title not exists in collection.</returns>
        public SettingItem[] getItems(String title, bool sorted)
        {
            //TODO: зачем сортировать по названию тут - они же все одинаковые?
            return this.m_Items.getItems(title, sorted);
        }

        /// <summary>
        /// NT-Добавить элемент, используя поле Title в качестве ключа для словаря.
        /// </summary>
        /// <param name="item">Добавляемый элемент.</param>
        public void addItem(SettingItem item)
        {
            // set item storage title
            item.Storage = Item.StorageKeyForSettingFileItem;
            // add
            this.m_Items.addItem(item);

            return;
        }


        /// <summary>
        /// NT-Add new settings item in collection.
        /// </summary>
        /// <param name="group">Setting item namespace as group.</param>
        /// <param name="title">Setting item title as key.</param>
        /// <param name="value">Setting item value as String.</param>
        /// <param name="descr">Setting item description as multiline String.</param>
        public void addItem(String group, String title, String value, String descr)
        {
            SettingItem item = new SettingItem(group, title, value, descr);
            // set item storage title
            item.Storage = Item.StorageKeyForSettingFileItem;
            // add
            this.m_Items.addItem(item);

            return;
        }


        /// <summary>
        /// NT-Add new settings item in collection.
        /// </summary>
        /// <param name="group">Setting item namespace as group.</param>
        /// <param name="title">Setting item title as key.</param>
        /// <param name="value">Setting item value as Int32.</param>
        /// <param name="descr">Setting item description as multiline String.</param>
        /// <returns></returns>
        public void addItem(String group, String title, Int32 value, String descr)
        {
            String val = value.ToString();
            this.addItem(group, title, val, descr);

            return;
        }


        /// <summary>
        ///  NT-Add new settings item in collection.
        /// </summary>
        /// <param name="group">Setting item namespace as group.</param>
        /// <param name="title">Setting item title as key.</param>
        /// <param name="value">Setting item value as Boolean.</param>
        /// <param name="descr">Setting item description as multiline String.</param>
        /// <returns></returns>
        public void addItem(String group, String title, Boolean value, String descr)
        {
            String val = value.ToString();
            this.addItem(group, title, val, descr);

            return;
        }


        /// <summary>
        /// NT-Remove from collection all items under specified title
        /// </summary>
        /// <param name="title">Setting item title as key</param>
        /// <returns></returns>
        public void removeItems(String title)
        {
            this.m_Items.removeItems(title);

            return;
        }


        /// <summary>
        /// NT - remove specified item object
        /// </summary>
        /// <param name="item">Объект, уже находящийся в этой коллекции.</param>
        /// <returns>Функция возвращает true, если объект был удален; функция возвращает false, если объект не был найден.</returns>
        /// <exception cref="Exception">Если ключ отсутствует в словаре коллекции.</exception>
        public bool removeItem(SettingItem item)
        {
            return this.m_Items.removeItem(item);
        }
        #endregion
        // *** End of file ***


    }
}
