using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NT - Представляет Место Оператора.
    /// </summary>
    public class Place : Item
    {

        #region *** Fields ***

        /// <summary>
        /// Тип места - как класс сущности - аргумента для семантической проверки.
        /// </summary>
        private string m_placetype;

        /// <summary>
        /// синонимы - для поддержки множественных названий одной сущности.
        /// </summary>
        private String m_synonim;
        /// <summary>
        /// Дерево типов сущностей
        /// </summary>
        private EntityTypesCollection m_entityTypes;
        #endregion

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public Place()
        {
            this.m_entityTypes = new EntityTypesCollection();
        }

        /// <summary>
        /// NT-Конструктор копирования
        /// </summary>
        /// <param name="p">Образец копирования</param>
        public Place(Place p)
        {
            this.m_descr = String.Copy(p.m_descr);
            this.m_tableid = p.m_tableid;
            this.m_path = String.Copy(p.m_path);
            this.m_placetype = String.Copy(p.m_placetype);
            this.m_synonim = String.Copy(p.m_synonim);
            this.m_title = String.Copy(p.m_title);
            this.m_storage = String.Copy(p.m_storage);
            this.m_entityTypes = null;
            this.m_readOnly = p.m_readOnly;
            ParseEntityTypeString();// распарсить дерево классов
            this.m_namespace = String.Copy(p.m_namespace);

            return;
        }

        #region *** Properties ***

        /// <summary>
        /// Тип места - как класс сущности - аргумента для семантической проверки. До 255 символов.
        /// </summary>
        public string PlaceTypeExpression
        {
            get { return m_placetype; }
            set { m_placetype = value; }
        }

        /// <summary>
        /// синонимы - для поддержки множественных названий одной сущности. До 255 символов.
        /// </summary>
        public String Synonim
        {
            get { return m_synonim; }
            set { m_synonim = value; }
        }

        /// <summary>
        /// Дерево типов сущностей
        /// </summary>
        public EntityTypesCollection EntityTypes  // TODO: сейчас нигде не используется...
        {
            get { return m_entityTypes; }
            set { m_entityTypes = value; }
        }

        #endregion
        /// <summary>
        /// NT-Получить одну строку описания свойств Места
        /// </summary>
        /// <returns>Функция возвращает одну строку описания свойств Места</returns>
        public override string ToString()
        {
            return this.getSingleLineProperties();
        }

        /// <summary>
        /// NT-Получить одну строку описания свойств Места
        /// Для вывода списка мест в разных случаях работы программы
        /// </summary>
        /// <returns></returns>
        public override String getSingleLineProperties()
        {
            // TODO: формат строки свойств Места неудовлетворительный - нужно переделать на понятный.
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
            sb.Append(this.m_placetype);
            sb.Append(";");
            sb.Append(this.m_path);
            sb.Append(";");
            sb.Append(this.m_descr);
            if (sb.Length > 80)
                sb.Length = 80;

            return sb.ToString();
        }

        //Закомиментировал, чтоыб использовать базовую версию функции: название и описание элемента.
        //Путь, возможно, не следует выводить в качестве описания места. Надо оценить это.
        //    /**
        //     * NT-Получить одну строку описания свойств Места: название и путь, длиной менее 80 символов.
        //     * 
        //     * @return Функция возвращает строку вроде: название места;(Путь места.)
        //     */
        //    @Override
        //    public String GetShortInfo()
        //    {
        //        // TODO: формат строки свойств Сущности неудовлетворительный - нужно переделать на понятный.
        //        // Одна строка, 80 символов макс.
        //        StringBuilder sb = new StringBuilder();
        //        sb.append(Utility.GetStringTextNull(this.m_title));
        //        sb.append(";");
        //        sb.append('(');
        //        sb.append(Utility.GetStringTextNull(this.m_path));
        //        if (sb.length() > 75)
        //        {
        //            sb.setLength(75);
        //        }
        //        sb.append(')');
        //
        //        return sb.toString();
        //    }


        /// <summary>
        /// NT-распарсить выражение типов места
        /// </summary>
        /// <remarks>
        /// Вызывается вручную, после загрузки места из БД через проперти.
        /// Заполняет коллекцию типов места из внутренней переменной, которая хранит упакованное значение.
        /// </remarks>
        public void ParseEntityTypeString()
        {
            this.m_entityTypes = new EntityTypesCollection();
            m_entityTypes.ParseExpression(this.m_placetype);
            //TODO: Тут лучше убрать эту функцию, а проперти EntityTypes переделать - оно должно кешировать дерево.
            //Дерево должно создаваться при первом обращении и храниться потом в памяти до выгрузки или до смены строки типов.
            //а после изменения строки типов дерево должно перестраиваться заново, там надо сделать вызов функции сборки дерева, и все.
        }

        //TODO: перенести этот код разделения в StringUtility
        /// <summary>
        /// Статический массив символов-разделителей для функции GetSynonims()
        /// </summary>
        private static char[] delimiterForSynonims = { ',', ';' };

        /// <summary>
        /// NT-Получить список уникальных синонимов-названий места
        /// </summary>
        /// <returns></returns>
        public List<string> GetSynonims()
        {
            //Поделим строку по разделителям , или ;
            String[] sar = this.m_synonim.Trim().Split(delimiterForSynonims, StringSplitOptions.RemoveEmptyEntries);
            //теперь отправим в список все кусочки, кроме пустых строк
            List<String> lis = new List<string>();
            foreach (String ss in sar)
            {
                String sss = ss.Trim();
                //добавить синонимы в список, если они не пустые и не повторяются
                if ((sss.Length > 0) && Utility.StringUtility.ListNotContains(lis, sss, StringComparison.OrdinalIgnoreCase))
                    lis.Add(sss);
            }

            return lis;
        }

        /// <summary>
        /// NR-Проверить что строка синонимов имеет правильный формат
        /// </summary>
        /// <param name="syno">строка синонимов</param>
        /// <returns>Возвращает True если строка синонимов имеет правильный формат, False в противном случае.</returns>
        public static bool checkSynonimString(String syno)
        {
            // TODO: а как проверять, что строка синонимов имеет правильный формат?
            // я вот сейчас не представляю себе это. Разве что, что это буквы,
            // пробел, цифры, прочерк, тире и хз что еще,
            // разделители ,;
            // не %?. и другие запрещенные для названий Мест символы...
            // сейчас часто вместо запятой я точку ввожу - клавиатура барахлит. Это
            // нарушает формат. Надо проверять на отсутствие точек.
            return true;
        }

    }
}
