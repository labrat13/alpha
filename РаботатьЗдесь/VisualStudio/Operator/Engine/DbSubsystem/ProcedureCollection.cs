using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.DbSubsystem
{
    /// <summary>
    /// NR - Коллекция - список Процедур
    /// </summary>
    internal class ProcedureCollection
    {
        /**
     * Список объектов процедур
     */
        private LinkedList<Procedure> m_proclist;

        /**
         * Default constructor
         */
        public ProcedureCollection()
        {
            this.m_proclist = new LinkedList<Procedure>();
        }

        /**
         * NT-Получить список объектов процедур
         * 
         * @return Список объектов процедур
         */
        public LinkedList<Procedure> get_Procedures()
        {
            return this.m_proclist;
        }

        /**
         * NT-Очистить коллекцию
         */
        public void Clear()
        {
            this.m_proclist.clear();
        }

        /**
         * NT-Get count of collection items.
         * 
         * @return Returns count of collection items.
         */
        public int getCount()
        {
            return this.m_proclist.size();
        }

        // TODO: Процедуры более не должны создаваться из кода.
        // TODO: Проверить, что Процедуры в коллекции сортируются по Весу после
        // заполнения коллекции!

        // /// <summary>
        // /// NR-Заполнить эту коллекцию процедурами вручную, описав их в коде
        // /// </summary>
        // internal void FillHardcodedProcedures()
        // {
        // //Если нужно добавлять процедуры вручную, рекомендуется добавлять
        // процедуры прямо в БД
        // //DONE: Заполнить эту коллекцию процедурами вручную, описав их в коде
        // //TODO: Описать правила заполнения полей объекта, привести ссылки на
        // форматы, документацию.
        //
        // //Procedure p;
        //
        // //Это шаблон, образец, не трогать его!
        //
        // //p = new Procedure();
        // ////заполнить вручную поля
        // //p.Title = "";//Название процедуры, в работе механизма не используется
        // //p.Regex = "";//Регулярное выражение, простое или сложное.
        // //p.ExecutionPath = "";//Путь к функции в сборке или к приложению, может
        // содержать аргументы.
        // //p.Descr = "";//Текст описания сущности, не обязателен, в работе
        // механизма не используется.
        // //p.Ves = 0.00;//Вес процедуры в пределах от 0 (раньше всех в очереди) до
        // 1 (позже всех в очереди)
        // ////добавление в коллекцию
        // //this.m_proclist.Add(p);
        //
        // ///////////////////////////////////////////////////////////////////////////////
        //
        // this.m_proclist.Sort(Procedure.SortByVes);
        //
        // return;
        // }

        /**
         * Fill collection from database and sort by Ves field
         * 
         * @param list
         *            List of Procedure from database
         */
        public void Fill(LinkedList<Procedure> list)
        {
            this.m_proclist.addAll(list);
            // сортировать процедуры по весу обязательно, иначе команды будут
            // исполняться не по их весу.
            SortByVes(this.m_proclist);

            return;
        }

        /**
         * NT- Сортировать процедуры по возрастанию веса
         * 
         * @param list
         *            List of procedures for sorting
         */
        public static void SortByVes(LinkedList<Procedure> list)
        {
            if (list.size() > 1)
            {
                Collections.sort(list, new Comparator<Procedure>()
            {

                @Override
                public int compare(Procedure u1, Procedure u2)
                {
                    return u1.get_Ves().compareTo(u2.get_Ves());
                }
            });
        }

        return;
    }

    /**
     * NT-Выбрать из коллекции Процедуры по названию, без учета регистра символов
     * 
     * @param title
     *            Название Процедуры
     * @return Возвращает список Процедур с указанным названием
     */
    public LinkedList<Procedure> getByTitle(String title)
    {
        LinkedList<Procedure> result = new LinkedList<Procedure>();
        for (Procedure p : this.m_proclist)
        {
            if (title.equalsIgnoreCase(p.get_Title()))
                result.add(p);
        }

        return result;
    }

    /**
     * NT - Получить из коллекции Процедуру по ее Пути, без учета регистра символов.
     * 
     * @param procedurePath
     *            Путь Процедуры.
     * @return Функция возвращает объект Процедуры, соответствующий указанному пути. Функция возвращает null, если Процедура не найдена.
     */
    public Procedure getByPath(String procedurePath)
    {
        for (Procedure p : this.m_proclist)
        {
            if (procedurePath.equalsIgnoreCase(p.get_Path()))
                return p;
        }

        return null;
    }

    /**
     * NT-Получить множество уникальных названий неймспейсов элементов коллекции.
     * 
     * @return Функция возвращает множество уникальных названий неймспейсов элементов коллекции.
     */
    public HashSet<String> getNamespaces()
    {
        HashSet<String> set = new HashSet<String>();
        // add existing item namespaces
        for (Procedure p : this.m_proclist)
            set.add(Utility.GetStringTextNull(p.get_Namespace()));

        return set;
    }

}
}
