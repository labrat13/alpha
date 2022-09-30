using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.OperatorEngine;
using Engine.SettingSubsystem;

namespace Engine.Utility
{
    /// <summary>
    /// NR-Словарь итемов с группировкой по неймспейсу
    /// </summary>
    internal class ItemDictionaryByNamespace
    {

        /// <summary>
        /// NT-Constructor
        /// </summary>
        public ItemDictionaryByNamespace() : base()
        {

        }

        /// <summary>
        /// NT- Add setting items from source.
        /// </summary>
        /// <param name="items">List of items to add.</param>
        public void addItems(LinkedList<Item> items)
        {
            foreach (Item item in items)
            {
                this.addItem(item.Namespace, item);
            }

            return;
        }

        /// <summary>
        /// NT- Add setting items from source.
        /// </summary>
        /// <param name="items">List of items to add.</param>
        public void addSettingItems(LinkedList<SettingItem> items)
        {
            foreach (Item item in items)
            {
                this.addItem(item.Namespace, item);
            }

            return;
        }

        /// <summary>
        /// NT- Add Procedure items from source.
        /// </summary>
        /// <param name="items">List of items to add.</param>
        public void addProcedureItems(LinkedList<Procedure> items)
        {
            foreach (Item item in items)
            {
                this.addItem(item.Namespace, item);
            }

            return;
        }

        /// <summary>
        /// NT- Add Place items from source.
        /// </summary>
        /// <param name="items">List of items to add.</param>
        public void addPlaceItems(LinkedList<Place> items)
        {
            foreach (Item item in items)
            {
                this.addItem(item.Namespace, item);
            }

            return;
        }

        /// <summary>
        /// NT-Добавить элемент, используя поле Namespace в качестве ключа для словаря.
        /// </summary>
        /// <param name="item">Добавляемый элемент.</param>
        public void addItem(Item item)
        {
            this.addItem(item.Namespace, item);

            return;
        }

        /// <summary>
        /// NT - remove specified item object
        /// </summary>
        /// <param name="item">Объект, уже находящийся в этой коллекции.</param>
        /// <returns>Функция возвращает true, если объект был удален; функция возвращает false, если объект не был найден.</returns>
        /// <exception cref="Exception">Если ключ отсутствует в словаре коллекции.</exception>
        public bool removeItem(Item item)
        {
            String key = item.Namespace;

            return base.removeItem(key, item);
        }

    }
}
