using System;
using System.Collections.Generic;
using System.Text;

namespace Operator
{
    public class ArgumentCollection
    {
        /// <summary>
        /// Список аргументов коллекции
        /// </summary>
        private List<FuncArgument> m_args;


        public ArgumentCollection()
        {
            m_args = new List<FuncArgument>();
        }

        /// <summary>
        /// Список аргументов коллекции
        /// </summary>
        public List<FuncArgument> Arguments
        {
            get { return m_args; }
            set { m_args = value; }
        }
        /// <summary>
        /// Добавить аргумент в коллекцию
        /// </summary>
        /// <param name="f"></param>
        public void Add(FuncArgument f)
        {
            m_args.Add(f);
        }

        /// <summary>
        /// Получить первый найденный объект аргумента по его названию, null если не найдено. 
        /// Сравнение без учета регистра.
        /// </summary>
        /// <param name="argname">Название аргумента</param>
        /// <returns></returns>
        public FuncArgument GetByName(String argname)
        {
            foreach (FuncArgument f in m_args)
            {
                if (String.Equals(argname, f.ArgumentName, StringComparison.OrdinalIgnoreCase))
                    return f;

            }
            return null;
        }


    }
}
