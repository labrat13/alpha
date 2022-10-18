using System;
using System.Collections.Generic;
using Engine.OperatorEngine;
using Engine.Utility;

namespace Engine.SettingSubsystem
{
    /// <summary>
    /// NT-Класс коллекции элементов настроек.
    /// </summary>
    internal class SettingItemCollection
    {
        #region *** Constants and Fields ***

        /// <summary>
        ///  Вложенный класс коллекции - а не наследуемый, поскольку надо переопределять возвращаемые типы для гладкого применения класса.
        /// </summary>
        protected ItemDictionaryBase m_items;
        #endregion

        /// <summary>
        /// NT-Initializes a new instance of the <see cref="SettingItemCollection"/> class.
        /// </summary>
        public SettingItemCollection()
        {
            this.m_items = new ItemDictionaryBase();
        }

        #region *** Properties ***

        /// <summary>
        /// Gets or Sets a value indicating whether this instance is modified.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        public bool isModified
        {
            get { return this.m_items.isModified; }
            set { this.m_items.isModified = value; }
        }

        #endregion
        #region *** Service functions ***

        /// <summary>
        /// NR-Return string representation to object.
        /// </summary>
        /// <returns></returns>        
        public override String ToString()
        {
            // TODO Auto-generated method stub
            return base.ToString();
        }
        #endregion
        #region *** Work functions ***

        /// <summary>
        /// NT- Add setting items from source.
        /// </summary>
        /// <param name="items">List of items to add.</param>
        public void addItems(List<SettingItem> items)
        {
            foreach (SettingItem item in items)
            {
                this.addItem(item);
            }

            return;
        }

        /// <summary>
        /// NT- Clear collection
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            this.m_items.Clear();

            return;
        }

        /// <summary>
        /// NT-Check setting is present
        /// </summary>
        /// <param name="title">Setting title as key</param>
        /// <returns>
        ///  Returns true if setting present in collection, false otherwise.
        /// </returns>
        public bool hasTitle(String title)
        {
            return this.m_items.hasKey(title);
        }


        /// <summary>
        /// NT-Get array of used titles.
        /// </summary>
        /// <param name="sorted">Sort titles.</param>
        /// <returns>Function returns array of used keyname strings.</returns>
        public String[] getTitles(bool sorted)
        {
            return this.m_items.getKeys(sorted);
        }

        /// <summary>
        /// NT-Get count of used titles.
        /// </summary>
        /// <returns>Function returns count of used titles.</returns>
        public int getTitleCount()
        {
            return this.m_items.getKeyCount();
        }

        // *************************************************

        /// <summary>
        /// NT-Get all items from collection as list.
        /// </summary>
        /// <returns>Function returns all items from collection as list.</returns>
        public List<SettingItem> getAllItems()
        {
            List<Item> its = this.m_items.getAllItems();

            // скопируем элементы в выходной список, чтобы привести типы к требуемым
            List<SettingItem> result = new List<SettingItem>();
            foreach (Item it in its)
                result.Add((SettingItem)it);

            return result;
        }

        /// <summary>
        /// NT-Get settings item array by title
        /// </summary>
        /// <param name="title">Setting item title as key</param>
        /// <param name="sorted">Sort items by title.</param>
        /// <returns>Returns SettingsItem[] array, or returns null if title not exists in collection.</returns>
        public SettingItem[] getItems(String title, bool sorted)
        {
            //TODO: что тут сортировать по алфавиту, если поиск по ключу-названию выдает только элеменоты с одинаковыми названиями?
            List<Item> its = this.m_items.getItems(title, sorted);
            if (its == null)
                return null;

            // скопируем элементы в выходной массив, чтобы привести типы к требуемым
            int size = its.Count;
            SettingItem[] result = new SettingItem[size];
            for (int i = 0; i < size; i++)
                result[i] = (SettingItem)its[i];

            return result;
        }

        /// <summary>
        /// NT-Get settings item's by title
        /// </summary>
        /// <param name="title">Setting item title as key</param>
        /// <returns>Returns SettingsItem list; returns empty list, if title not exists in collection.</returns>
        public List<SettingItem> getByTitle(String title)
        {
            List<Item> its = this.m_items.getItems(title, false);

            //convert types
            List<SettingItem> result = new List<SettingItem>();
            if (its != null)
            {
                foreach (Item it in its)
                    result.Add((SettingItem)it);
            }
            return result;
        }

        /// <summary>
        /// NT-Get only first setting item by title
        /// </summary>
        /// <param name="title">Setting item title as key</param>
        /// <returns>Returns SettingsItem object, or returns null if title not exists in collection.</returns>
        public SettingItem getFirstItem(String title)
        {
            Item result = this.m_items.getFirstItem(title);

            if (result == null)
                return null;
            else return (SettingItem)result;
        }

        // *****************************************************

        /// <summary>
        /// NT-Добавить элемент, используя поле Title в качестве ключа для словаря.
        /// </summary>
        /// <param name="item">Добавляемый элемент.</param>
        /// <returns></returns>
        public void addItem(SettingItem item)
        {
            this.m_items.addItem(item.Title, item);

            return;
        }

        // **********************************************

        /// <summary>
        /// NT-Remove list of items from collection
        /// </summary>
        /// <param name="title">Setting item title as key</param>
        public void removeItems(String title)
        {
            this.m_items.removeItems(title);

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
            return this.m_items.removeItem(item.Title, item);
        }

        /// <summary>
        /// NT-Получить множество уникальных названий неймспейсов элементов коллекции.
        /// </summary>
        /// <returns>Функция возвращает множество уникальных названий неймспейсов элементов коллекции.</returns>
        public HashSet<String> getNamespaces()
        {
            //HashSet<String> set = new HashSet<String>();
            //// add existing item namespaces
            //foreach (Item p in this.m_items.getAllItems())
            //    set.Add(StringUtility.GetStringTextNull(p.Namespace));

            //return set;

            //replaced by this, for optimization 
            return this.m_items.getNamespaces();
        }
        #endregion
        // *** End of file ***
    }
}
