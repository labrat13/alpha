using System;
using System.Collections.Generic;
using System.Text;

namespace Operator
{
    /// <summary>
    /// Абстрактный класс для Процедур и Мест Оператора
    /// </summary>
    public class Item
    {

        #region Fields

        /// <summary>
        /// первичный ключ таблицы
        /// </summary>
        protected int m_id;

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

        #endregion

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public Item()
        {

        }




        #region Properties

        /// <summary>
        /// первичный ключ таблицы
        /// </summary>
        public int TableId
        {
            get { return m_id; }
            set { m_id = value; }
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

        #endregion

        /// <summary>
        /// NT-
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.getSingleLineProperties();
        }

        /// <summary>
        /// NT-Получить одну строку описания свойств итема
        /// </summary>
        /// <returns></returns>
        public virtual string getSingleLineProperties()
        {
            //Одна строка, 80 символов макс.
            StringBuilder sb = new StringBuilder();
            sb.Append(this.m_id.ToString());
            sb.Append(";");
            sb.Append(this.m_title);
            sb.Append(";");
            sb.Append(this.m_path);
            sb.Append(";");
            sb.Append(this.m_descr);
            if (sb.Length > 80)
                sb.Length = 80;
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
