using System;
using System.Collections.Generic;
using System.Text;

namespace Operator
{
    
    /// <summary>
    /// Представляет аргумент функции
    /// </summary>
    public class FuncArgument
    {
        
#region *** Fields ***        
        /// <summary>
        /// Тип места или тип данных аргумента
        /// </summary>
        private string m_argtype;

        /// <summary>
        /// Название аргумента
        /// </summary>
        private string m_argname;

        /// <summary>
        /// Значение аргумента
        /// </summary>
        private string m_argvalue;

        /// <summary>
        /// Значение аргумента из строки запроса
        /// </summary>
        private string m_argQueryValue;
        /// <summary>
        /// копия подставленного Места
        /// </summary>
        private Place m_Place;

#endregion

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="name">Название аргумента</param>
        /// <param name="type">Тип данных аргумента</param>
        /// <param name="value">Значение аргумента</param>
        /// <param name="queryValue">Значение аргумента из строки запроса</param>
        public FuncArgument(string name, string type, string value, string queryValue)
        {
            this.m_argname = name;
            m_argtype = type;
            m_argvalue = value;
            m_argQueryValue = queryValue;
            this.m_Place = null;
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        public FuncArgument()
        {
            m_argvalue = String.Empty;
            m_argtype = String.Empty;
            m_argname = String.Empty;
            m_argQueryValue = String.Empty;
            this.m_Place = null;
        }

        #region *** Properties ***
        /// <summary>
        /// Тип места или тип данных аргумента
        /// </summary>
        public string ArgumentType
        {
            get { return m_argtype; }
            set { m_argtype = value; }
        }
        /// <summary>
        /// Название аргумента
        /// </summary>
        public string ArgumentName
        {
            get { return m_argname; }
            set { m_argname = value; }
        }
        /// <summary>
        /// Значение аргумента для передачи в процедуру
        /// </summary>
        public string ArgumentValue
        {
            get { return m_argvalue; }
            set { m_argvalue = value; }
        }
        /// <summary>
        /// Значение текста аргумента из строки запроса
        /// </summary>
        /// <remarks>
        /// Это решение проблемы автоподстановки Мест. 
        /// Тут должно храниться значение аргумента из строки запроса до того, как оно было заменено свойствами сопоставленного ему Места.
        /// Без этого поля все аргументы, совпадающие с синонимами Мест, заменяются движком на данные соответствующего Места. 
        /// И в процедуре, хотя и можно опрежелить, что произошла подстановка Места, но нет возможности определить исходный текст запроса.
        /// Поэтому добавлено это свойство аргумента.
        /// </remarks>
        public string ArgumentQueryValue
        {
            get { return m_argQueryValue; }
            set { m_argQueryValue = value; }
        }

        /// <summary>
        /// Получить копию подставленного движком Места
        /// </summary>
        public Place AssociatedPlace
        {
            get { return m_Place; }
        }

        /// <summary>
        /// Флаг, что произошла автоподстановка Места
        /// </summary>
        public bool АвтоподстановкаМеста
        {
            get { return (this.m_Place != null); }
        }
        #endregion


        /// <summary>
        /// NT-Подставить найденное Место вместо текста аргумента
        /// </summary>
        /// <param name="p"></param>
        internal void ПодставитьМесто(Place p)
        {
            //вписываем тип аргумента из типа Места
            this.m_argtype = String.Copy(p.PlaceTypeExpression);
            //заменяем значение аргумента на значение из Места
            //Это единственное, что действительно нужно делать здесь.
            this.m_argvalue = String.Copy(p.Path);
            //и присоединяем копию Места, если вдруг она пригодится в процедуре.
            this.m_Place = new Place(p);

            return;
        }
    }
}
