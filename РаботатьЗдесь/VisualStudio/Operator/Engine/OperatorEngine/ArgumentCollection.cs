using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NT-Список аргументов коллекции
    /// </summary>
    public class ArgumentCollection
    {
        /// <summary>
        /// Список аргументов коллекции
        /// </summary>
        protected List<FuncArgument> m_args;

        /// <summary>
        /// NT- Конструктор
        /// </summary>
        public ArgumentCollection()
        {
            this.m_args = new List<FuncArgument>();
        }

        /// <summary>
        /// NT-Список аргументов коллекции
        /// </summary>
        public List<FuncArgument> Arguments
        {
            get { return m_args; }
            set { m_args = value; }
        }

        /// <summary>
        /// NT-Добавить аргумент в коллекцию
        /// </summary>
        /// <param name="f">Объект аргумента</param>
        public void Add(FuncArgument f)
        {
            m_args.Add(f);
        }

        /// <summary>
        /// NT-Получить первый найденный объект аргумента по его названию, null если не найдено. 
        /// Сравнение без учета регистра.
        /// </summary>
        /// <param name="argname">Название аргумента</param>
        /// <returns>Функция возвращает объект аргумента.</returns>
        public FuncArgument GetByName(String argname)
        {
            foreach (FuncArgument f in m_args)
            {
                if (String.Equals(argname, f.ArgumentName, StringComparison.OrdinalIgnoreCase))
                    return f;

            }
            return null;
        }

        /// <summary>
        /// NT-Получить объект аргумента по его индексу в списке.
        /// </summary>
        /// <param name="i">Индекс элемента в списке.</param>
        /// <returns>Функция возвращает объект аргумента, находящийся по указанному индексу в списке.</returns>
        public FuncArgument getByIndex(int i)
        {
            return this.m_args[i];
        }

    }
}
