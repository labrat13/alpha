using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - Представляет класс сущности
    /// </summary>
    internal class EntityType
    {
        //TODO: Port this class from Java
        
        // #region *** Fields ***

        /**
         * Коллекция абстрактных надклассов
         */
        private HashMap<String, EntityType> m_AbstractionSuperClasses;

        /**
         * Коллекция агрегатных подклассов
         */
        private HashMap<String, EntityType> m_AggregationSubClasses;

        /**
         * Название класса сущности
         */
        private String m_Title;

        // #endregion
        /**
         * Конструктор
         */
        public EntityType()
        {
            this.m_Title = "";// String.Empty;
            this.set_AbstractionSuperClasses(new HashMap<String, EntityType>());
            this.set_AggregationSubClasses(new HashMap<String, EntityType>());
        }

        /**
         * Конструктор
         * 
         * @param title
         *            Название создаваемого типа
         */
        public EntityType(String title)
        {
            this.m_Title = title;
            this.set_AbstractionSuperClasses(new HashMap<String, EntityType>());
            this.set_AggregationSubClasses(new HashMap<String, EntityType>());
        }

        // #region *** Properties ***

        /**
         * Название класса сущности
         * 
         * @return the title
         */
        public String get_Title()
        {
            return m_Title;
        }

        /**
         * Название класса сущности
         * 
         * @param title
         *            the title to set
         */
        public void set_Title(String title)
        {
            this.m_Title = title;
        }

        /**
         * Коллекция абстрактных надклассов
         * 
         * @return the abstractionSuperClasses
         */
        HashMap<String, EntityType> get_AbstractionSuperClasses()
        {
            return m_AbstractionSuperClasses;
        }

        /**
         * Коллекция абстрактных надклассов
         * 
         * @param abstractionSuperClasses
         *            the abstractionSuperClasses to set
         */
        void set_AbstractionSuperClasses(
                HashMap<String, EntityType> abstractionSuperClasses)
        {
            this.m_AbstractionSuperClasses = abstractionSuperClasses;
        }

        /**
         * Коллекция агрегатных подклассов
         * 
         * @return the aggregationSubClasses
         */
        HashMap<String, EntityType> get_AggregationSubClasses()
        {
            return m_AggregationSubClasses;
        }

        /**
         * Коллекция агрегатных подклассов
         * 
         * @param aggregationSubClasses
         *            the aggregationSubClasses to set
         */
        void set_AggregationSubClasses(
                HashMap<String, EntityType> aggregationSubClasses)
        {
            this.m_AggregationSubClasses = aggregationSubClasses;
        }

        // #endregion

        /**
         * NT- return text representation of object
         * 
         * @return Return text representation of object
         */
        public String ToString()
        {
            return String.format("%s;%s;%s", this.m_Title, this.m_AbstractionSuperClasses.size(), this.m_AggregationSubClasses.size());
        }

        /**
         * NT-Ищет запись типа по его названию. Если тип не упомянут, возвращается
         * null
         * 
         * @param nameOfType
         *            Название типа
         * @return Возвращает первый найденный объект записи типа или null если
         *         объект не найден
         */
        public EntityType ContainsType(String nameOfType)
        {
            // проверяем элементы первого уровня Абстракции
            if (this.m_AbstractionSuperClasses.containsKey(nameOfType))
                return this.m_AbstractionSuperClasses.get(nameOfType);
            else
            {
                // проверяем элементы нижнего уровня
                for (Map.Entry<String, EntityType> kvp : this.m_AbstractionSuperClasses.entrySet())
                {
                    EntityType res = kvp.getValue().ContainsType(nameOfType);
                    if (res != null)
                        return res;
                }
            }
            // ничего не нашли

            // проверяем элементы первого уровня Агрегации
            if (this.m_AggregationSubClasses.containsKey(nameOfType))
                return this.m_AggregationSubClasses.get(nameOfType);
            else
            {
                // проверяем элементы нижнего уровня
                for (Map.Entry<String, EntityType> kvp : this.m_AggregationSubClasses.entrySet())
                {
                    EntityType res = kvp.getValue().ContainsType(nameOfType);
                    if (res != null)
                        return res;
                }
            }
            // ничего не нашли
            return null;
        }

        /**
         * Разбор выражения
         * 
         * @param expression
         *            Выражение для разбора
         * @throws Exception
         *             Неправильное выражение
         */
        public void ParseExpression(String expression) throws Exception
        {
            // варианты выражений:
            // [0] Мои места:: Коллекция музыки<Файл::ФайлМузыки>
            // [1] Файловая система ::Папка < Файловая система::Папка,Файл>
            // [2] ФайлМузыки

            // парсим
            // 1) удаляем пробелы с начала и конца выражения
            String exp = expression.trim();
        // 2) разделяем на элементы по < и >
        // String[] sar = exp.Split(new char[] { '<', '>' },
        // StringSplitOptions.RemoveEmptyEntries);
        String[] sar = Utility.StringSplit(exp, "[<>]", true);
        // 3) обрабатываем разделы
        // [0] суперкласс и класс - Мои места:: Коллекция музыки / Файловая
        // система ::Папка / ФайлМузыки
        // [1] агрегированные подклассы - Файл::ФайлМузыки / Файловая
        // система::Папка,Файл / нет

        // 3.1) обрабатываем название класса и суперкласса. Элемент 0 всегда
        // должен существовать.
        ParseClassTitle(sar[0]);

        // 3.2) парсим агрегатные субклассы, если они есть
        if (sar.length == 1)
            return; // все распарсили, выходим
        // если более 2 элементов массива, это неправильный формат, выбрасываем
        // исключение.
        if (sar.length > 2)
            throw new Exception(String.format("Неправильное выражение: %s", expression));
        if (sar.length == 2)
        {
            String s = sar[1];
        // делим по запятым
        // String[] sar2 = s.Split(new char[] { ',' },
        // StringSplitOptions.RemoveEmptyEntries);
        String[] sar2 = Utility.StringSplit(s, ",", true);
            // теперь получаем 1 и более элементов вида Файл::ФайлМузыки
            // Файловая система::Папка Файл
            if (sar2.length == 0)
                throw new Exception(String.format("Неправильная запись агрегированных субклассов: %s", expression));
            for (String ss : sar2)
            {
                // trim делается в функции
                // создаем новый объект для класса
                EntityType tt = new EntityType();
        // отправляем на парсинг для выделения суперкласса и название класса
        tt.ParseClassTitle(ss);
                // Добавляем в коллекцию агрегатных субклассов.
                // Обратной ссылки не делаем - некуда ее писать.
                this.m_AggregationSubClasses.put(tt.get_Title(), tt);
            }
}

return;
    }

    /**
     * Парсим первую часть выражения класса, содержащую имя суперкласса и имя
     * класса
     * 
     * @param expression
     *            Строка выражения. Например, "Файл::Файл музыки"
     */
    public void ParseClassTitle(String expression)
{
    // варианты выражений:
    // [0] Мои места:: Коллекция музыки
    // [1] Файловая система ::Папка
    // [2] ФайлМузыки
    // TODO: вариант, когда суперклассов более 1, обрабатывается
    // неправильно!
    // например: МоиМеста::Музыка::МузыкальныеАльбомы::Альбом1
    // Но тут весь этот движок дерева классов надо переделывать в отдельный
    // компонент.

    String s = expression.trim();

    // проверяем наличие суперкласса в выражении
    if (s.contains("::"))
    {
        // Входные случаи [1] и [2]
        // Делим на название суперкласса и название текущего класса
        // String[] sar2 = s.Split(new string[] { "::" },
        // StringSplitOptions.RemoveEmptyEntries);
        String[] sar2 = Utility.StringSplit(s, "::", true);

        this.m_Title = sar2[1].trim(); // пишем название текущего класса
                                       // создаем объект для суперкласса и пишем в него имя
        EntityType et = new EntityType(sar2[0].trim());
        // добавляем в коллекцию суперклассов. Обратной ссылки не делаем -
        // некуда ее писать.
        this.get_AbstractionSuperClasses().put(et.get_Title(), et);
    }
    else this.set_Title(s); // входной случай [2], в выражении нет названия
                            // суперкласса
}
    }
}
