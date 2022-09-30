using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - Представляет Место Оператора.
    /// </summary>
    public class Place : Item
    {
        //TODO: Port this class from Java code

         #region Fields

        /**
         * Тип места - как класс сущности - аргумента для семантической проверки.
         */
        private String m_placetype;

        /**
         * синонимы - для поддержки множественных названий одной сущности.
         */
        private String m_synonim;

        /**
         * Дерево типов сущностей
         */
        private EntityTypesCollection m_entityTypes;
         #endregion

        /**
         * Стандартный конструктор
         */
        public Place()
        {
            this.m_entityTypes = new EntityTypesCollection();
        }

        /**
         * NT-Конструктор копирования
         * 
         * @param p
         *            Образец копирования.
         * @throws Exception
         *             Ошибка при копировании.
         */
        public Place(Place p) throws Exception
        {
        this.m_descr = Utility.StringCopy(p.m_descr);
        this.m_tableid = p.m_tableid;
        this.m_path = Utility.StringCopy(p.m_path);
        this.m_placetype = Utility.StringCopy(p.m_placetype);
        this.m_synonim = Utility.StringCopy(p.m_synonim);
        this.m_title = Utility.StringCopy(p.m_title);
        this.m_storage = Utility.StringCopy(p.m_storage);
        this.m_entityTypes = null;
        ParseEntityTypeString();// распарсить дерево классов
        this.m_namespace = Utility.StringCopy(p.m_namespace);

        return;
    }

     #region *** Properties ***

    /**
     * Тип места - как класс сущности - аргумента для семантической проверки. До
     * 255 символов.
     * 
     * @return Тип места - как класс сущности - аргумента для семантической
     *         проверки. До 255 символов.
     */
    public String get_PlaceTypeExpression()
    {
        return this.m_placetype;
    }

    /**
     * Тип места - как класс сущности - аргумента для семантической проверки. До
     * 255 символов.
     * 
     * @param pt
     *            Тип места - как класс сущности - аргумента для семантической
     *            проверки. До 255 символов.
     */
    public void set_PlaceTypeExpression(String pt)
    {
        this.m_placetype = pt;
    }

    /**
     * синонимы - для поддержки множественных названий одной сущности. До 255
     * символов.
     * 
     * @return синонимы - для поддержки множественных названий одной сущности.
     *         До 255 символов.
     */
    public String get_Synonim()
    {
        return this.m_synonim;
    }

    /**
     * синонимы - для поддержки множественных названий одной сущности. До 255
     * символов.
     * 
     * @param sy
     *            синонимы - для поддержки множественных названий одной
     *            сущности. До 255 символов.
     */
    public void set_Synonim(String sy)
    {
        this.m_synonim = sy;
    }

    /**
     * Дерево типов сущностей
     * 
     * @return Дерево типов сущностей
     */
    public EntityTypesCollection get_EntityTypes()
    {
        return this.m_entityTypes;
    }

    /**
     * Дерево типов сущностей
     * 
     * @param val
     *            Дерево типов сущностей
     */
    public void set_EntityTypes(EntityTypesCollection val)
    {
        this.m_entityTypes = val;
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
     * NT-Получить одну строку описания свойств Места
     * Для вывода списка мест в разных случаях работы программы
     * 
     * @return Функция возвращает одну строку описания свойств Места
     */
    @Override
    public String getSingleLineProperties()
    {
        // TODO: формат строки свойств Места неудовлетворительный - нужно переделать на понятный.
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
        sb.append(this.m_placetype);
        sb.append(";");
        sb.append(this.m_path);
        sb.append(";");
        sb.append(this.m_descr);
        if (sb.length() > 80)
            sb.setLength(80);
        return sb.toString();
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

    /**
     * NT-распарсить выражение типов места.
     * Вызывается вручную, после загрузки места из БД через проперти.
     * Заполняет коллекцию типов места из внутренней переменной, которая хранит
     * упакованное значение.
     * 
     * @throws Exception
     *             Ошибка при разборе выражения.
     */
    public void ParseEntityTypeString() throws Exception
    {
        this.m_entityTypes = new EntityTypesCollection();
    m_entityTypes.ParseExpression(this.m_placetype);
        // TODO: Тут лучше убрать эту функцию, а проперти EntityTypes переделать
        // - оно должно кешировать дерево.
        // Дерево должно создаваться при первом обращении и храниться потом в
        // памяти до выгрузки или до смены строки типов.
        // а после изменения строки типов дерево должно перестраиваться заново,
        // там надо сделать вызов функции сборки дерева, и все.
        return;
    }

    /**
     * NT-Получить список уникальных синонимов-названий места
     * 
     * @return список уникальных синонимов-названий места
     */
    public LinkedList<String> GetSynonims()
    {
        // Поделим строку по разделителям , или ;
        String[] sar = this.m_synonim.trim().split("[,;]");
        // теперь отправим в список все кусочки, кроме пустых строк
        LinkedList<String> lis = new LinkedList<String>();
        for (String ss : sar)
        {
            String sss = ss.trim();
            // добавить синонимы в список, если они не пустые и не повторяются
            if ((sss.length() > 0) && (listNotContains(lis, sss)))
                lis.add(sss);
        }
        return lis;
    }

    /**
     * NT-проверяет что список не содержит такой строки
     * 
     * @param lis Проверяемый список строк.
     * @param sss Искомая строка.
     * @return Функция возвращает true если список содержит искомую строку, false  в противном случае.
     */
    private boolean listNotContains(LinkedList<String> lis, String sss)
    {
        /// Использован именно список, а не словарь, чтобы сравнивать слова без
        /// учета регистра символов.
        /// TODO: вынести эту функцию отсюда в общую библиотеку, так как она
        /// явно общего назначения.
        /// Или как расширение списка: List'string'.Contains(string s,
        /// StringComparison mode)

        for (String s : lis)
            if (sss.equalsIgnoreCase(s))
                return false;
        return true;
    }

    /**
     * NR-Проверить что строка синонимов имеет правильный формат
     * 
     * @param syno
     *            строка синонимов
     * @return Возвращает True если строка синонимов имеет правильный формат,
     *         False в противном случае.
     */
    public static boolean checkSynonimString(String syno)
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
