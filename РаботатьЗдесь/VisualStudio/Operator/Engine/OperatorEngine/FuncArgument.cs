using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - Представляет аргумент функции
    /// </summary>
    internal class FuncArgument
    {
        //TODO: port this class from Java
        
        // #region *** Fields ***

        /**
         * Тип места или тип данных аргумента
         */
        private String m_argtype;

        /**
         * Название аргумента
         */
        private String m_argname;

        /**
         * Значение аргумента
         */
        private String m_argvalue;

        // Это решение проблемы автоподстановки Мест.
        // Тут должно храниться значение аргумента из строки запроса до того, как
        // оно было заменено свойствами сопоставленного ему Места.
        // Без этого поля все аргументы, совпадающие с синонимами Мест, заменяются
        // движком на данные соответствующего Места.
        // И в процедуре, хотя и можно опрежелить, что произошла подстановка Места,
        // но нет возможности определить исходный текст запроса.
        // Поэтому добавлено это свойство аргумента.
        /**
         * Значение аргумента из строки запроса
         */
        private String m_argQueryValue;

        /**
         * копия подставленного Места
         */
        private Place m_Place;

        // #endregion
        /**
         * Конструктор
         */
        public FuncArgument()
        {
            m_argvalue = "";// String.Empty;
            m_argtype = "";// String.Empty;
            m_argname = "";// String.Empty;
            m_argQueryValue = "";// String.Empty;
            this.m_Place = null;
        }

        /**
         * Конструктор с параметрами
         * 
         * @param name
         *            Название аргумента
         * @param type
         *            Тип данных аргумента
         * @param value
         *            Значение аргумента
         * @param queryValue
         *            Значение аргумента из строки запроса
         */
        public FuncArgument(String name,
                String type,
                String value,
                String queryValue)
        {
            this.m_argname = name;
            m_argtype = type;
            m_argvalue = value;
            m_argQueryValue = queryValue;
            this.m_Place = null;
        }

        // #region *** Properties ***

        /**
         * Тип места или тип данных аргумента
         * 
         * @return Тип места или тип данных аргумента
         */
        public String get_ArgumentType()
        {
            return this.m_argtype;
        }

        /**
         * Тип места или тип данных аргумента
         * 
         * @param val
         *            Тип места или тип данных аргумента
         */
        public void set_ArgumentType(String val)
        {
            this.m_argtype = val;
        }

        /**
         * Название аргумента
         * 
         * @return Название аргумента
         */
        public String get_ArgumentName()
        {
            return this.m_argname;
        }

        /**
         * Название аргумента
         * 
         * @param val
         *            Название аргумента
         */
        public void set_ArgumentName(String val)
        {
            this.m_argname = val;
        }

        /**
         * Значение аргумента для передачи в процедуру
         * 
         * @return Значение аргумента для передачи в процедуру
         */
        public String get_ArgumentValue()
        {
            return this.m_argvalue;
        }

        /**
         * Значение аргумента для передачи в процедуру
         * 
         * @param val
         *            Значение аргумента для передачи в процедуру
         */
        public void set_ArgumentValue(String val)
        {
            this.m_argvalue = val;
        }

        /**
         * Значение текста аргумента из строки запроса
         * 
         * @return Значение текста аргумента из строки запроса
         */
        public String get_ArgumentQueryValue()
        {
            return this.m_argQueryValue;
        }

        /**
         * Значение текста аргумента из строки запроса
         * 
         * @param val
         *            Значение текста аргумента из строки запроса
         */
        public void set_ArgumentQueryValue(String val)
        {
            this.m_argQueryValue = val;
        }

        /**
         * Получить копию подставленного движком Места
         * 
         * @return Получить копию подставленного движком Места
         */
        public Place get_AssociatedPlace()
        {
            return this.m_Place;
        }

        /**
         * Флаг, что произошла автоподстановка Места
         * 
         * @return Флаг, что произошла автоподстановка Места
         */
        public boolean get_АвтоподстановкаМеста()
        {
            return (this.m_Place != null);
        }

        // #endregion

        /**
         * NT-Подставить найденное Место вместо текста аргумента
         * 
         * @param p
         *            Найденное Место
         * @throws Exception Ошибка при создании объекта Места.
         */
        public void ПодставитьМесто(Place p) throws Exception
        {
        // вписываем тип аргумента из типа Места
        this.m_argtype = Utility.StringCopy(p.get_PlaceTypeExpression());
        // заменяем значение аргумента на значение из Места
        // Это единственное, что действительно нужно делать здесь.
        this.m_argvalue = Utility.StringCopy(p.get_Path());
        // и присоединяем копию Места, если вдруг она пригодится в процедуре.
        this.m_Place = new Place(p);

        return;
    }
}
}
