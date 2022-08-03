using System;
using System.Collections.Generic;

namespace Operator
{
    /// <summary>
    /// Список процедур
    /// </summary>
    public class ProcedureCollection
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
        /// Список объектов процедур
        /// </summary>
        public List<Procedure> Procedures
        {
            get { return m_proclist; }
        }



        /// <summary>
        /// NR-Заполнить эту коллекцию процедурами вручную, описав их в коде
        /// </summary>
        internal void FillHardcodedProcedures()
        {
            //Если нужно добавлять процедуры вручную, рекомендуется добавлять процедуры прямо в БД
            //DONE: Заполнить эту коллекцию процедурами вручную, описав их в коде
            //TODO: Описать правила заполнения полей объекта, привести ссылки на форматы, документацию.

            //Procedure p;

            //Это шаблон, образец, не трогать его!

            //p = new Procedure();
            ////заполнить вручную поля
            //p.Title = "";//Название процедуры, в работе механизма не используется
            //p.Regex = "";//Регулярное выражение, простое или сложное.
            //p.ExecutionPath = "";//Путь к функции в сборке или к приложению, может содержать аргументы.
            //p.Descr = "";//Текст описания сущности, не обязателен, в работе механизма не используется.
            //p.Ves = 0.00;//Вес процедуры в пределах от 0 (раньше всех в очереди) до 1 (позже всех в очереди) 
            ////добавление в коллекцию
            //this.m_proclist.Add(p);

            ///////////////////////////////////////////////////////////////////////////////

            this.m_proclist.Sort(Procedure.SortByVes);

            return;
        }

        internal void FillFromDb(List<Procedure> list)
        {
            this.m_proclist.AddRange(list);
            //сортировать процедуры по весу обязательно, иначе команды будут исполняться не по их весу.
            this.m_proclist.Sort(Procedure.SortByVes);

            return;
        }

        /// <summary>
        /// NT-Очистить коллекцию
        /// </summary>
        internal void Clear()
        {
            this.m_proclist.Clear();
        }


        /// <summary>
        /// NT-Выбрать из БД Процедуры по названию, без учета регистра символов
        /// </summary>
        /// <param name="title">Название Процедуры</param>
        /// <returns>Возвращает список Процедур с указанным названием</returns>
        internal List<Procedure> getByTitle(string title)
        {
            List<Procedure> result = new List<Procedure>();
            foreach (Procedure p in this.m_proclist)
            {
                if (String.Equals(p.Title, title, StringComparison.OrdinalIgnoreCase))
                    result.Add(p);
            }

            return result;
        }
    }
}
