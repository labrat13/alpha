using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Operator
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }



        #region *** Operator version ***
        /// <summary>
        /// NT-Получить строку версии сборки Оператора
        /// </summary>
        /// <remarks>Возвращает версию сборки, в которой находится данная функция.</remarks>
        /// <returns></returns>
        internal static string getOperatorVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// NT-Получить версию сборки Оператора
        /// </summary>
        /// <returns></returns>
        internal static Version getOperatorVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }
        #endregion
    }
}
