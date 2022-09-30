using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - Представляет Процедуру Оператора.
    /// </summary>
    public class Procedure :Item
    {
        //TODO: Port this class from Java code
         #region Fields
        /**
         * порядковый номер проверки в очереди проверок для команды - для поддержки
         * очередность проверки выражений
         */
        private Double m_ves;

        /**
         * регулярное выражение - для проверки соответствия команды и процедуры.
         */
        private String m_regex;

         #endregion
        /**
         * NT-Стандартный конструктор
         */
        public Procedure()
        {
            super();
            this.m_ves = 0.5;
            this.m_regex = "";
        }

        /**
         * NT-Конструктор копирования.
         * @param p Образец копирования.
         */
        public Procedure(Procedure p)
        {
            this.m_descr = Utility.StringCopy(p.m_descr);
            this.m_namespace = Utility.StringCopy(p.m_namespace);
            this.m_path = Utility.StringCopy(p.m_path);
            this.m_regex = Utility.StringCopy(p.m_regex);
            this.m_storage = Utility.StringCopy(p.m_storage);
            this.m_tableid = p.m_tableid;
            this.m_title = Utility.StringCopy(p.m_title);
            this.m_ves = p.m_ves;
        }

         #region *** Properties ***


        /**
         * порядковый номер проверки в очереди проверок для команды - для поддержки
         * очередность проверки выражений
         *
         * @return порядковый номер проверки в очереди проверок для команды - для
         *         поддержки очередность проверки выражений
         */
        public Double get_Ves()
        {
            return this.m_ves;
        }

        /**
         * порядковый номер проверки в очереди проверок для команды - для поддержки
         * очередность проверки выражений
         *
         * @param val
         *            порядковый номер проверки в очереди проверок для команды - для
         *            поддержки очередность проверки выражений
         */
        public void set_Ves(Double val)
        {
            this.m_ves = val;
        }

        /**
         * регулярное выражение - для проверки соответствия команды и процедуры. До
         * 255 символов.
         *
         * @return регулярное выражение - для проверки соответствия команды и
         *         процедуры. До 255 символов.
         */
        public String get_Regex()
        {
            return this.m_regex;
        }

        /**
         * регулярное выражение - для проверки соответствия команды и процедуры. До
         * 255 символов.
         *
         * @param val
         *            регулярное выражение - для проверки соответствия команды и
         *            процедуры. До 255 символов.
         */
        public void set_Regex(String val)
        {
            this.m_regex = val;
        }

         #endregion
        /**
         * NT-Получить строку описания свойств Процедуры для отладчика.
         * 
         * @return Функция возвращает описание свойств Процедуры одной строкой.
         */
        @Override
    public String toString()
        {
            return this.getSingleLineProperties();
        }

        /**
         * NT-Получить одну строку описания свойств Процедуры.
         * Для вывода списка Процедур в разных случаях работы программы.
         * 
         * @return Функция возвращает описание свойств Процедуры одной строкой.
         */
        @Override
    public String getSingleLineProperties()
        {
            // TODO: формат строки свойств Сущности неудовлетворительный - нужно переделать на понятный.
            // Одна строка, 80 символов макс.
            StringBuilder sb = new StringBuilder();
            sb.append(this.m_storage);
            sb.append(':');
            sb.append(this.m_tableid);
            sb.append(";");
            sb.append(this.m_title);
            sb.append(";");
            sb.append("[").append(this.m_namespace).append("]");
            sb.append(";");
            sb.append("ves=");
            sb.append(this.m_ves);
            sb.append(";path=");
            sb.append(this.m_path);
            sb.append(";");
            sb.append(this.m_descr);
            if (sb.length() > 80)
            {
                sb.setLength(80);
            }
            return sb.toString();
        }

        // /**
        // * NT-Получить одну строку описания свойств Процедуры: название и описание, длиной менее 80 символов.
        // *
        // * @return Функция возвращает строку вроде: название процедуры;(Описание процедуры.)
        // */
        // @Override
        // public String GetShortInfo()
        // {
        // // TODO: формат строки свойств Сущности неудовлетворительный - нужно переделать на понятный.
        // // Одна строка, 80 символов макс.
        // StringBuilder sb = new StringBuilder();
        // sb.append(Utility.GetStringTextNull(this.m_title));
        // sb.append(";");
        // sb.append('(');
        // sb.append(Utility.GetStringTextNull(this.m_descr));
        // if (sb.length() > 75)
        // {
        // sb.setLength(75);
        // }
        // sb.append(')');
        //
        // return sb.toString();
        // }

        /**
         * NT-Проверить на допустимость значение Вес Процедуры, введенное
         * пользователем.
         *
         * @param str
         *            Текстовое значение веса
         * @return Возвращает true если значение допустимо в качестве Веса
         *         Процедуры, false в противном случае.
         */
        public static boolean IsValidVesFormat(String str)
        {
            boolean result = false;
            try
            {
                // это должно парситься в Double, быть меньше 1 и больше 0
                double d = Double.parseDouble(str);
                if ((d > 0.0d) && (d < 1.0d))
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
    }
}
