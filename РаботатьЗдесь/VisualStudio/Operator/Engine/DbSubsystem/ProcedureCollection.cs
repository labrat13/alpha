using System;
using System.Collections.Generic;
using Engine.OperatorEngine;

namespace Engine.DbSubsystem
{
    /// <summary>
    /// NT - Коллекция - список Процедур
    /// </summary>
    internal class ProcedureCollection
    {

        /// <summary>
        /// Список объектов процедур
        /// </summary>
        private List<Procedure> m_proclist;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProcedureCollection()
        {
            this.m_proclist = new List<Procedure>();
        }

        /// <summary>
        /// RT-Получить список объектов процедур.
        /// </summary>
        public List<Procedure> Procedures
        {
            get { return this.m_proclist; }
        }

        /// <summary>
        /// RT-Очистить коллекцию
        /// </summary>
        public void Clear()
        {
            this.m_proclist.Clear();
        }

        /// <summary>
        /// RT-Get count of collection items.
        /// </summary>
        public int Count
        {
            get { return this.m_proclist.Count; }
        }

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

        /// <summary>
        /// NT - Fill collection from database and sort by Ves field.
        /// </summary>
        /// <param name="list">List of Procedure from database</param>
        public void Fill(List<Procedure> list)
        {
            this.m_proclist.AddRange(list);
            // сортировать процедуры по весу обязательно, иначе команды будут
            // исполняться не по их весу.
            this.m_proclist.Sort(Procedure.SortByVes);

            return;
        }

        /// <summary>
        /// NT-Выбрать из коллекции Процедуры по названию, без учета регистра символов
        /// </summary>
        /// <param name="title">Название Процедуры</param>
        /// <returns>Возвращает список Процедур с указанным названием</returns>
        public List<Procedure> getByTitle(String title)
        {
            List<Procedure> result = new List<Procedure>();
            foreach (Procedure p in this.m_proclist)
            {
                if (title.Equals(p.Title, StringComparison.OrdinalIgnoreCase))
                    result.Add(p);
            }

            return result;
        }

        /// <summary>
        /// NT - Получить из коллекции Процедуру по ее Пути, без учета регистра символов.
        /// </summary>
        /// <param name="procedurePath">Путь Процедуры.</param>
        /// <returns>
        /// Функция возвращает объект Процедуры, соответствующий указанному пути.
        /// Функция возвращает null, если Процедура не найдена.
        /// </returns>
        public Procedure getByPath(String procedurePath)
        {
            foreach (Procedure p in this.m_proclist)
            {
                if (procedurePath.Equals(p.Path, StringComparison.OrdinalIgnoreCase))
                    return p;
            }

            return null;
        }

        /// <summary>
        /// NT-Получить множество уникальных названий неймспейсов элементов коллекции.
        /// </summary>
        /// <returns>Функция возвращает множество уникальных названий неймспейсов элементов коллекции.</returns>
        public HashSet<String> getNamespaces()
        {
            HashSet<String> set = new HashSet<String>();
            // add existing item namespaces
            foreach (Procedure p in this.m_proclist)
                set.Add(Utility.StringUtility.GetStringTextNull(p.Namespace));

            return set;
        }

    }
}
