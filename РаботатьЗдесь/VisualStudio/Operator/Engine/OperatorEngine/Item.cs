using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NT - Базовый класс для Процедур и Мест Оператора.
    /// </summary>
    public class Item
    {

        #region Fields

        /// <summary>
        /// Константа для поля m_storage, обозначает, что данный элемент хранится в Бд Оператор.
        /// </summary>
        /// <remarks>
        /// Все остальные значения этого поля должны соответствовать названиям Библиотек Процедур,
        /// из которых извлечен данный элемент (Место или Процедура).
        /// </remarks>
        public const String StorageKeyForDatabaseItem = "Database";

        // TODO: придумать, как это преобразовать в подобие енума, но оставить String, чтобы не пополнять енум именами новых Библиотек Процедур.
        // TODO: описать эту фичу с источниками итемов в документации, иначе я про нее забуду совсем. Проект становится сложным и запутанным.

        /// <summary>
        /// Константа для поля m_storage, обозначает, что данный элемент хранится в ФайлНастроек Оператор.
        /// </summary>
        /// <remarks>
        /// Все остальные значения этого поля должны соответствовать названиям Библиотек Процедур,
        /// из которых извлечен данный элемент (Место или Процедура).
        /// </remarks>
        public const String StorageKeyForSettingFileItem = "SettingFile";


        /// <summary>
        /// первичный ключ таблицы
        /// </summary>
        protected int m_tableid;

        /// <summary>
        /// Название Сущности
        /// </summary>
        protected string m_title;

        /// <summary>
        /// Описание Сущности
        /// </summary>
        protected string m_descr;

        /// <summary>
        /// Путь к Сущности
        /// </summary>
        protected string m_path;

        //TODO: решено позже добавить в объект Item ссылку на объект Хранилища, унифицированный для БД и БиблиотекаПроцедур,
        // и из него запрашивать возможность удаления и изменения итемов.
        //Это должна быть ссылка на объект класса LibraryManagerBase ? Они должны уже храниться где-то в коллекции сборок Процедур где-то в коллекциях тут.

        /// <summary>
        /// Название Хранилища ( Библиотеки Процедур или БД).  Не сохранять в таблицу БД!
        /// </summary>
        protected String m_storage;

        /// <summary>
        /// Название пространства имен Сущности.
        /// </summary>
        protected String m_namespace;
        /// <summary>
        /// Флаг что Сущность не может быть изменена в Хранилище.
        /// </summary>
        protected bool m_readOnly;

        #endregion

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public Item()
        {
            this.m_descr = "";
            this.m_path = "";
            this.m_tableid = 0;
            this.m_title = "";
            this.m_storage = "";
            this.m_namespace = "";
            this.m_readOnly = false;
            return;
        }



        #region Properties

        /// <summary>
        /// первичный ключ таблицы
        /// </summary>
        public int TableId
        {
            get { return m_tableid; }
            set { m_tableid = value; }
        }

        /// <summary>
        /// Уникальное название сущности, до 255 символов
        /// </summary>
        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        /// <summary>
        /// Описание сущности
        /// </summary>
        public string Description
        {
            get { return m_descr; }
            set { m_descr = value; }
        }
        /// <summary>
        /// Путь к сущности, до 255 символов
        /// </summary>
        public string Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        /// <summary>
        /// Название Хранилища ( Библиотеки Процедур или БД).  Не сохранять в таблицу БД!
        /// </summary>
        public String Storage
        {
            get { return this.m_storage; }
            set { this.m_storage = value; }
        }
        /// <summary>
        /// Название пространства имен Сущности.
        /// </summary>
        public String Namespace
        {
            get { return this.m_namespace; }
            set { this.m_namespace = value; }
        }

        /// <summary>
        /// Флаг что Сущность не может быть изменена в Хранилище.
        /// </summary>
        public bool ReadOnly
        {
            get { return this.m_readOnly; }
            set { this.m_readOnly = value; }
        }
        #endregion

        /// <summary>
        /// NT-
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.getSingleLineProperties();
        }

        #region Эти функции надо перенести в объект менеджера библиотеки Процедур
        /// <summary>
        /// NT-Проверить что элемент должен храниться в указанном Хранилище.
        /// </summary>
        /// <param name="storageTitle">Название Хранилища.</param>
        /// <returns>Функция возвращает True, если элемент хранится в указанном Хранилище, False в противном случае.</returns>
        public bool isItemFromStorage(String storageTitle)
        {
            return StringUtility.StringEqualsOrdinalIgnoreCase(this.m_storage, storageTitle);
        }

        /// <summary>
        /// NT-Проверить что элемент должен храниться в БД.
        /// </summary>
        /// <returns>Функция возвращает True, если элемент хранится в БД, False в противном случае.</returns>
        public bool isItemFromDatabase()// TODO: заменить функцию и все ее использования на isItemFromStorage(String storageTitle)
        {
            return StringUtility.StringEqualsOrdinalIgnoreCase(this.m_storage, Item.StorageKeyForDatabaseItem);
        }

        /// <summary>
        /// NT-Может ли объект быть удален из своего Хранилища.
        /// </summary>
        /// <returns>Функция возвращает True, если элемент может быть удален, False в противном случае.</returns>
        public bool isItemCanRemoved()
        {
            // сейчас только объекты, хранящиеся в БД или Файл настроек, могут быть изменены или удалены.
            return isItemFromStorage(Item.StorageKeyForDatabaseItem) || isItemFromStorage(Item.StorageKeyForSettingFileItem);
            // TODO: а вообще, надо получить объект Хранилища и запросить это значение у него.
            
        }

        /// <summary>
        /// NT-Может ли объект быть изменен в своем Хранилище.
        /// </summary>
        /// <returns>Функция возвращает True, если элемент может быть изменен, False в противном случае.</returns>
        public bool isItemCanChanged()
        {
            // сейчас только объекты, хранящиеся в БД или Файл настроек, могут быть изменены или удалены.
            return isItemFromStorage(Item.StorageKeyForDatabaseItem) || isItemFromStorage(Item.StorageKeyForSettingFileItem);
            // TODO: а вообще, надо получить объект Хранилища и запросить это значение у него.
            //TODO: сейчас есть флаг рид-онли и нужно его использовать вместо этой функции
        }
#endregion

        /// <summary>
        /// NT-Получить одну строку описания свойств итема
        /// </summary>
        /// <returns></returns>
        public virtual string getSingleLineProperties()
        {
            // TODO: формат строки свойств Сущности неудовлетворительный - нужно переделать на понятный.
            //Одна строка, 80 символов макс.
            StringBuilder sb = new StringBuilder();
            sb.Append(this.m_storage);
            sb.Append(':');
            sb.Append(this.m_tableid);
            sb.Append(";");
            sb.Append(this.m_title);
            sb.Append(";");
            sb.Append("[").Append(this.m_namespace).Append("]");
            sb.Append(";");
            sb.Append(this.m_path);
            sb.Append(";");
            sb.Append(this.m_descr);
            if (sb.Length > 80)
                sb.Length = 80;
            return sb.ToString();
        }


        /// <summary>
        /// NT-Получить одну строку описания свойств Элемента: название и описание, длиной менее 80 символов.
        /// </summary>
        /// <returns>Функция возвращает строку вроде: название Элемента (Описание Элемента.)</returns>
        public String GetShortInfo()
        {
            // TODO: формат строки свойств Сущности неудовлетворительный - нужно переделать на понятный.
            // Одна строка, 80 символов макс.
            StringBuilder sb = new StringBuilder();
            sb.Append(StringUtility.GetStringTextNull(this.m_title));
            sb.Append(" ");
            sb.Append('(');
            sb.Append(StringUtility.GetStringTextNull(this.m_descr));
            if (sb.Length > 75)
            {
                sb.Length = 75;
            }
            sb.Append(')');

            return sb.ToString();
        }

        /// <summary>
        /// Сортировать список по Названию 
        /// </summary>
        /// <param name="x">Item object</param>
        /// <param name="y">Item object</param>
        /// <returns></returns>
        public static int SortByTitle(Item x, Item y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;// If x is null and y is null, they're equal. 
                else
                    return -1;// If x is null and y is not null, y is greater. 
            }
            else
            {   // If x is not null...
                if (y == null)
                    return 1;// ...and y is null, x is greater.
                else
                {
                    String titleX = x.m_title;
                    String titleY = y.m_title;

                    return titleX.CompareTo(titleY);
                }
            }
        }

    }
}
