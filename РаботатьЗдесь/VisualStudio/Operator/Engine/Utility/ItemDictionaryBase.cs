using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.OperatorEngine;

namespace Engine.Utility
{
    /// <summary>
    /// NT-Общий класс словаря списков объектов Item
    /// </summary>
    internal class ItemDictionaryBase
    {
        // TODO: попробовать позже реализовать этот класс как шаблон<T>
        //TODO: переделать класс на List объект везде.

            /// <summary>
            /// Dictionary of lists
            /// </summary>
        protected Dictionary<String, LinkedList<Item>> m_items;

        /**
         * Collection has been modified
         */
        protected bool m_Modified;

        // *** Constructors ***

        /**
         * NT-Constructor
         */
        public ItemDictionaryBase()
        {
            this.m_items = new Dictionary<String, LinkedList<Item>>();
            this.m_Modified = false;
        }

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
            foreach (LinkedList<Item> it in this.m_items.Values)
            {
                it.Clear();
            }
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
            foreach (LinkedList<Item> li in this.m_items.Values)
                if ((li != null) && (li.Count > 0))
                    result.AddRange(li);
            // возвращаем выходной список
            return result;
        }

            /// <summary>
            /// NR-Get item array by title
            /// </summary>
            /// <param name="key">Dictionary key</param>
            /// <param name="sorted">Optionally, sort keys.</param>
            /// <returns>Returns Item list, or returns null if key not exists in collection.</returns>
        public List<Item> getItems(String key, bool sorted)
        {
            LinkedList<Item> result = this.m_items[key];
            if (result == null)
                return null;

            if (sorted == true)
                List.sort(result);

            return result;
        }

        /// <summary>
        /// NT-Get only first item by key
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <returns>Returns Item object, or returns null if key not exists in collection.</returns>
        public Item getFirstItem(String key)
        {
            LinkedList<Item> result = this.m_items[key];

            if (result == null)
                return null;
            if (result.Count < 1)
                return null;
            else return result.First();
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
            LinkedList<Item> lsi = this.m_items[tl];
            // if list == null, create it and add to dictionary
            if (lsi == null)
            {
                lsi = new LinkedList<Item>();
                this.m_items.Add(tl, lsi);
            }
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
            // remove list of items by title
            this.m_items.Remove(key);
            // set modified flag
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
            LinkedList<Item> list = this.m_items[key];
        if (list == null)
            throw new Exception(String.Format("Ключ \"s\" отсутствует в словаре", key));
        // remove item from list
        bool result = list.Remove(item);
        if (result == true)
            // set modified flag
            this.m_Modified = true;

        return result;
    }
    #endregion
}
}
