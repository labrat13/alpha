using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text;

namespace Operator
{
    /// <summary>
    /// Класс Процедуры Оператора
    /// </summary>
    public class Procedure: Item
    {
        #region Fields

        /// <summary>
        /// порядковый номер проверки в очереди проверок для команды - для поддержки очередность проверки выражений
        /// </summary>
        private Double m_ves;

        /// <summary>
        /// регулярное выражение - для проверки соответствия команды и процедуры.
        /// </summary>
        private String m_regex;
        
        #endregion

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public Procedure()
        {
            
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
        /// NT-
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.getSingleLineProperties();
        }

        ///// <summary>
        ///// NT-Должна вернуть труе если запрос подходит под регекс
        ///// </summary>
        ///// <param name="cmdline">Текст запроса</param>
        ///// <returns></returns>
        //internal bool IsMatchProcedure(string cmdline)
        //{
        //    String rx = null;
        //    //получить тип регекса
        //    RegexType rt = RegexManager.determineRegexType(this.Regex);
        //    //конвертировать регекс в пригодный для исполнения
        //    if (rt == RegexType.NormalRegex)
        //    {
        //        rx = String.Copy(this.Regex);
        //    }
        //    else if (rt == RegexType.SimpleString)
        //    {
        //        rx = RegexManager.ConvertSimpleToRegex2(this.Regex);
        //    }
        //    else throw new Exception(String.Format("Invalid regex string: {0} in {1}", this.Regex, this.Title));
        //    //выполнить регекс и вернуть результат проверки
        //    bool res = RegexManager.IsMatchQuery(rx, cmdline);
        //    return res;
        //}

        ///// <summary>
        ///// Должна вернуть облом при неподходящих параметрах, успех при исполнении, выход если требуется завершение работы приложения или компьютера
        ///// </summary>
        ///// <param name="cmdline"></param>
        ///// <returns></returns>
        //internal ProcedureResult Execute(string cmdline)
        //{
        //    //надо определить, путь исполнения это путь к процедуре или к приложению.
        //    //если к приложению, его надо запустить и все, вернуть стандартное значение для продолжения работы.
        //    //если к процедуре, надо приготовить аргументы, найти сборку, вызвать функцию, передать ей аргументы и вернуть результат.
        //}

        #region *** Assemblies loading *** - TODO: перенести функции в подходящий класс
        /// <summary>
        /// NT-Get assembly file path for assembly loading
        /// </summary>
        /// <param name="assemblyName">assembly name without extension</param>
        /// <returns>full assmbly file path</returns>
        public static string getAssemblyFilePath(string assemblyName)
        {
            String asmPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            asmPath = System.IO.Path.ChangeExtension(System.IO.Path.Combine(asmPath, assemblyName), ".dll");
            return asmPath;
        }

        /// <summary>
        /// NT-Load assembly and get MethodInfo of method implementation function
        /// </summary>
        /// <returns>MethodInfo object represent method code</returns>
        public static MethodInfo getMethodInfo(string[] names)
        {
            //get assembly pathname
            String asmPath = getAssemblyFilePath(names[0]);
            //load assembly
            Assembly aa = Assembly.LoadFile(asmPath);
            Type tt = aa.GetType(String.Format("{0}.{1}", names[0], names[1]));
            if (tt == null) throw new Exception(String.Format("Класс {0} не найден в сборке {1}", names[1], names[0])); //
            MethodInfo m = tt.GetMethod(names[2]);
            if (m == null) throw new Exception(String.Format("Процедура {0} не найдена в классе {1} сборки {2}", names[2], names[1], names[0])); //
            return m;
        }

        /// <summary>
        /// Get state of method implementation function
        /// </summary>
        /// <returns>One of implementation state values</returns>
        public static ImplementationState getStateOfImplement(MethodInfo mi)
        {
            ImplementationState ist = ImplementationState.NotRealized; //= метод не пригоден для исполнения
            try
            {
                Object[] oo = mi.GetCustomAttributes(typeof(ProcedureAttribute), false);
                if (oo.Length > 0) ist = ((ProcedureAttribute)oo[0]).ElementValue;
            }
            catch (Exception)
            {
                //любое исключение показывает что метод не пригоден для исполнения
            }
            return ist;
        }

        /// <summary>
        /// NT-Запустить процедуру
        /// </summary>
        /// <param name="command">Текст команды пользователя</param>
        /// <param name="names">Путь к процедуре</param>
        /// <param name="args">Готовый для применения список аргументов</param>
        /// <returns></returns>
        public ProcedureResult invokeProcedure(String command, string[] names, Engine engine, ArgumentCollection args)
        {
            //получить сборку и метод в ней
            MethodInfo mi = getMethodInfo(names);
            //проверить готовность кода процедуры
            if (getStateOfImplement(mi) == ImplementationState.NotRealized)
            {
                throw new Exception(String.Format("Процедура {0}.{1}.{2} не готова для исполнения.", names[0], names[1], names[2]));
            }
            //загрузить в нее аргументы
            //make arguments array
            List<Object> li = new List<object>();
            li.Add(engine);//Engine object
            li.Add(command);//user command text
            li.Add(args);//Argument collection
            //запустить метод
            Object resval = mi.Invoke(null, li.ToArray());
            //вернуть результат
            return (ProcedureResult)resval;
        }


        #endregion

        /// <summary>
        /// Предикат сортировки списка процедур
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
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


        /// <summary>
        /// NT-Получить одну строку описания свойств Процедуры
        /// Для вывода списка Процедур в разных случаях работы программы
        /// </summary>
        /// <returns></returns>
        public override string getSingleLineProperties()
        {
            //Одна строка, 80 символов макс.
            StringBuilder sb = new StringBuilder();
            sb.Append(this.m_id.ToString());
            sb.Append(";");
            sb.Append(this.m_title);
            sb.Append(";ves=");
            sb.Append(this.m_ves.ToString());
            sb.Append(";path=");
            sb.Append(this.m_path);
            sb.Append(";");
            sb.Append(this.m_descr);
            if (sb.Length > 80)
                sb.Length = 80;
            return sb.ToString();
        }


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


    }//end class
}
