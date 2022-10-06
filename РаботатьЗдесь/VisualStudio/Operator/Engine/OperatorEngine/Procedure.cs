using System;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NT - Класс Процедуры Оператора
    /// </summary>
    public class Procedure :Item
    {

        #region *** Fields ***

        /// <summary>
        /// порядковый номер проверки в очереди проверок для команды - для поддержки очередность проверки выражений
        /// </summary>
        private Double m_ves;

        /// <summary>
        /// регулярное выражение - для проверки соответствия команды и процедуры.
        /// </summary>
        private String m_regex;

        #endregion
        /**
         * NT-Стандартный конструктор
         */
        public Procedure() : base()
        {
            this.m_ves = 0.5;
            this.m_regex = String.Empty;
        }

        
        /// <summary>
        /// NT-Конструктор копирования.
        /// </summary>
        /// <param name="p">Образец копирования</param>
            public Procedure(Procedure p)
        {
            this.m_descr = String.Copy(p.m_descr);
            this.m_namespace = String.Copy(p.m_namespace);
            this.m_path = String.Copy(p.m_path);
            this.m_regex = String.Copy(p.m_regex);
            this.m_storage = String.Copy(p.m_storage);
            this.m_tableid = p.m_tableid;
            this.m_title = String.Copy(p.m_title);
            this.m_ves = p.m_ves;
            this.m_readOnly = p.m_readOnly;

            return;
        }

        #region *** Properties ***

        /// <summary>
        /// порядковый номер проверки в очереди проверок для команды - для поддержки очередность проверки выражений
        /// </summary>
        public Double Ves
        {
            get { return m_ves; }
            set { m_ves = value; }
        }
        /// <summary>
        /// регулярное выражение - для проверки соответствия команды и процедуры. До 255 символов.
        /// </summary>
        public String Regex
        {
            get { return m_regex; }
            set { m_regex = value; }
        }

        #endregion

        /// <summary>
        /// NT-Получить строку описания свойств Процедуры для отладчика.
        /// </summary>
        /// <returns>Функция возвращает описание свойств Процедуры одной строкой.</returns>
        public override string ToString()
        {
            return this.getSingleLineProperties();
        }

        /// <summary>
        /// NT-Получить одну строку описания свойств Процедуры. Для вывода списка Процедур в разных случаях работы программы.
        /// </summary>
        /// <returns>Функция возвращает описание свойств Процедуры одной строкой.</returns>
    public override String getSingleLineProperties()
        {
            // TODO: формат строки свойств Сущности неудовлетворительный - нужно переделать на понятный.
            // Одна строка, 80 символов макс.
            StringBuilder sb = new StringBuilder();
            sb.Append(this.m_storage);
            sb.Append(':');
            sb.Append(this.m_tableid);
            sb.Append(";");
            sb.Append(this.m_title);
            sb.Append(";");
            sb.Append("[").Append(this.m_namespace).Append("]");
            sb.Append(";");
            sb.Append("ves=");
            sb.Append(this.m_ves);
            sb.Append(";path=");
            sb.Append(this.m_path);
            sb.Append(";");
            sb.Append(this.m_descr);
            if (sb.Length > 80)
            {
                sb.Length = 80;
            }
            return sb.ToString();
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

        /// <summary>
        /// NT-Проверить на допустимость значение Вес Процедуры, введенное пользователем.
        /// </summary>
        /// <param name="str">Текстовое значение веса</param>
        /// <param name="cultureInfo">Информация о языке</param>
        /// <returns>Возвращает true если значение допустимо в качестве Веса Процедуры, false в противном случае.</returns>
        public static bool IsValidVesFormat(string str, System.Globalization.CultureInfo cultureInfo)
        {
            bool result = false;
            try
            {
                //это должно парситься в Double, быть меньше 1 и больше 0
                double d = Double.Parse(str, cultureInfo);
                if ((d > 0.0d) && (d < 1.0d))
                    result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// NT - Предикат сортировки списка процедур
        /// </summary>
        /// <param name="p1">Procedure object</param>
        /// <param name="p2">Procedure object</param>
        /// <returns></returns>
        public static int SortByVes(Procedure p1, Procedure p2)
        {
            //if p1 > p2 return 1
            if (p1 == null)
            {
                if (p2 == null) return 0;
                else return -1;
            }
            else
            {
                if (p2 == null) return 1;
                else
                {
                    if (p1.m_ves > p2.m_ves)
                        return 1;
                    else if (p1.m_ves < p2.m_ves) return -1;
                    else return 0;
                }
            }
        }
    }
}
