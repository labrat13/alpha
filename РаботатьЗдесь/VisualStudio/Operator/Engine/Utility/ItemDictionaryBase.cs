using System;
using System.Collections.Generic;
using Engine.OperatorEngine;

namespace Engine.Utility
{
    /// <summary>
    /// NT-Общий класс словаря списков объектов Item
    /// </summary>
    public class ItemDictionaryBase
    {
        // TODO: попробовать позже реализовать этот класс как шаблон<T>


        /// <summary>
        /// Dictionary of lists
        /// </summary>
        protected Dictionary<String, List<Item>> m_items;

        /// <summary>
        ///  Collection has been modified
        /// </summary>
        protected bool m_Modified;

        #region *** Constructors ***

        /// <summary>
        /// NT-Constructor
        /// </summary>
        public ItemDictionaryBase()
        {
            this.m_items = new Dictionary<String, List<Item>>();
            this.m_Modified = false;
        }
        #endregion
        #region *** Properties ***
        /// <summary>
        /// Collection is modified
        /// </summary>
        public bool isModified
        {
            get { return this.m_Modified; }
            set { this.m_Modified = value; }
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

        /// <summary>
        /// NT- Clear collection
        /// </summary>
        public void Clear()
        {
            // перечислить списки и очистить каждый из них
            foreach (List<Item> it in this.m_items.Values)
                it.Clear();
            // очистить словарь
            this.m_items.Clear();
            // set modified flag
            this.m_Modified = true;
            // TODO: следует ли тут вызвать сборку мусора?
            return;
        }



        /// <summary>
        /// NT-Check key is present
        /// </summary>
        /// <param name="key">key string value</param>
        /// <returns>Returns true if key present in collection, false otherwise.</returns>
        public bool hasKey(String key)
        {
            return this.m_items.ContainsKey(key);
        }


        /// <summary>
        /// NT-Get array of used keys.
        /// </summary>
        /// <param name="sorted">Optionally, sort keys.</param>
        /// <returns>Function returns array of used keynam</returns>
        public String[] getKeys(bool sorted)
        {
            List<String> result = new List<string>(this.m_items.Count);
            result.AddRange(this.m_items.Keys);
            // sort if needed
            if (sorted == true)
                result.Sort();
            String[] ar = result.ToArray();

            return ar;
        }

        /// <summary>
        /// NT-Get count of used keys.
        /// </summary>
        /// <returns>Function returns count of used keys.</returns>
        public int getKeyCount()
        {
            return this.m_items.Count;
        }

        /// <summary>
        /// NT-Get all items from collection as list.
        /// </summary>
        /// <returns>Function returns all items from collection as list</returns>
        public List<Item> getAllItems()
        {
            List<Item> result = new List<Item>();
            // мержим все значения-списки в выходной список.
            foreach (List<Item> li in this.m_items.Values)
                if ((li != null) && (li.Count > 0))
                    result.AddRange(li);
            // возвращаем выходной список
            return result;
        }

        /// <summary>
        /// NT-Получить множество уникальных названий неймспейсов элементов коллекции.
        /// </summary>
        /// <remarks>
        /// Названия неймспейсов учитывают регистр символов: NameSpace != naMespace.
        /// </remarks>
        /// <returns>Функция возвращает множество уникальных названий неймспейсов элементов коллекции.</returns>
        public HashSet<String> getNamespaces()
        {
            HashSet<String> set = new HashSet<String>();
            // add existing item namespaces
            foreach (List<Item> li in this.m_items.Values)
                if ((li != null) && (li.Count > 0))
                    foreach (Item p in li)
                        set.Add(StringUtility.GetStringTextNull(p.Namespace));

            return set;
        }

        /// <summary>
        /// NT-Get item array by title
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <param name="sorted">Optionally, sort keys.</param>
        /// <returns>Returns Item list, or returns null if key not exists in collection.</returns>
        public List<Item> getItems(String key, bool sorted)
        {
            if (!this.m_items.ContainsKey(key))
                return null;
            //else
            List<Item> result = this.m_items[key];
            if (result == null)
                return null;
            //TODO: а по какому параметру сортировать? Пока по Title, но это не правильно?
            if (sorted == true)
                result.Sort(Item.SortByTitle);

            return result;
        }

        /// <summary>
        /// NT-Get only first item by key
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <returns>Returns Item object, or returns null if key not exists in collection.</returns>
        public Item getFirstItem(String key)
        {
            if (!this.m_items.ContainsKey(key))
                return null;
            //else
            List<Item> result = this.m_items[key];

            if (result == null)
                return null;
            if (result.Count < 1)
                return null;
            else return result[0];
        }

        /// <summary>
        /// NT-Add new item to collection.
        /// </summary>
        /// <param name="key">Item field value as key</param>
        /// <param name="item">Item object.</param>
        public void addItem(String key, Item item)
        {
            // get copy of title string
            String tl = String.Copy(key);
            // get list by key

            List<Item> lsi = null;
            // if key not exists, create it and add to dictionary
            if (!this.m_items.ContainsKey(key))
            {
                lsi = new List<Item>();
                this.m_items.Add(tl, lsi);
            }
            else
                lsi = this.m_items[key];
            // add item to list
            lsi.Add(item);

            // set modified flag
            this.m_Modified = true;

            return;
        }



        /// <summary>
        /// NT-Remove list of items from collection
        /// </summary>
        /// <param name="key">Dictionary key.</param>
        public void removeItems(String key)
        {
            // remove list of items by title, if key exists
            bool modif = this.m_items.Remove(key);

            // set modified flag if collection has been modified
            if (modif == true)
                this.m_Modified = true;

            return;
        }

        /// <summary>
        /// NT - remove specified item object
        /// </summary>
        /// <param name="key">Dictionary key.</param>
        /// <param name="item">Объект, уже находящийся в этой коллекции.</param>
        /// <returns>Функция возвращает true, если объект был удален; функция возвращает false, если объект не был найден.</returns>
        /// <exception cref="Exception">Если ключ отсутствует в словаре коллекции.</exception>
        public bool removeItem(String key, Item item)
        {
            if (!this.m_items.ContainsKey(key))
                throw new Exception(String.Format("Ключ \"{0}\" отсутствует в словаре", key));
            //else
            List<Item> list = this.m_items[key];

            // remove item from list
            bool result = list.Remove(item);
            // set modified flag if collection has been modified
            if (result == true)
                this.m_Modified = true;

            return result;
        }
        #endregion
    }
}
