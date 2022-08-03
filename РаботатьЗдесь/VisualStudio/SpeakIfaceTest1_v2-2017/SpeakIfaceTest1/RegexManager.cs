using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Operator
{
    /// <summary>
    /// Класс-контейнер для регекс-операций
    /// </summary>
    public class RegexManager
    {



        /// <summary>
        /// NT-Определить тип регекса, содержащегося в переданной строке
        /// </summary>
        /// <param name="pattern">Строка, содержащая регекс</param>
        /// <returns>Возвращает одно из <typeparamref name="RegexType"/> значений</returns>
        public static RegexType determineRegexType(String pattern)
        {
            //проверка наличия регекса в процедуре
            if (String.IsNullOrEmpty(pattern)) return RegexType.Empty;
            //проверка наличия ^ и $
            bool b1 = (pattern[0] == '^');
            bool b2 = (pattern[pattern.Length - 1] == '$');
            if ((b1 & b2) == true) return RegexType.NormalRegex; // это сложный регекс
            else if ((b1 | b2) == false) return RegexType.SimpleString; //это простой регекс
            else return RegexType.Invalid; //это ни то ни другое
        }

        /// <summary>
        /// NT-Конвертировать простой регекс в нормальный регекс - не используется, заменено на 2.
        /// </summary>
        /// <param name="rx"></param>
        /// <returns></returns>
        internal static string ConvertSimpleToRegex(string rx)
        {

            //тут надо распарсить строку запроса, выделив аргументы.
            //затем заменить аргументы на строки, с учетом количества аргументов.

            //Пример простого регекса:
            //Копировать %файл в %папка
            String query = rx.Trim();


            //разделим строку на слова по пробелам
            String[] sar = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //теперь если первый символ %, то все слово заменить на регекс-вставку
            //а потом все эти куски собрать обратно в строку через пробелы
            //получим и строку регекса и строго одиночные пробелы меду словами.
            //и потеряем запятые у аргументов наподобие %арг0, %арг1
            //хотя можно проверять, что последний символ не буква и копировать его в заменяемый, но это потом, если потребуется.
            //а то и вовсе можно замены через регекс делать
            int num = 0;
            for(int i = 0; i < sar.Length; i++)
            {
                String s = sar[i];
                if(s[0] == '%')
                {
                    sar[i] = "(?<arg" + num.ToString() + ">.+)";
                    num++;
                }
            }
            String result = String.Join(" ", sar);
            return result;
        }


        ///// <summary>
        ///// RT-Запустить проверку соответствия запроса и нормального регекса
        ///// </summary>
        ///// <param name="rx">регекс</param>
        ///// <param name="cmdline">запрос, команда пользователя</param>
        ///// <returns></returns>
        //internal static bool IsMatchQuery(string rx, string cmdline)
        //{
        //    //Замечу что здесь запросы выполняются без учета регистра символов, а имена мест сейчас учитывают регистр символов.
        //    return Regex.IsMatch(cmdline, rx, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        //}


#region *** Замена аргументов командной строки приложений ***
        /// <summary>
        /// Словарь аргументов для вставки в командную строку.
        /// </summary>
        private static ArgumentCollection m_AppMatchArguments;
        /// <summary>
        /// Замена аргументов командной строки приложений
        /// </summary>
        /// <param name="cmdline"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        internal static string ConvertApplicationCommandString(String cmdline, ArgumentCollection arguments)
        {
            //распарсить строку вида: my app.exe -t -d%arg1%[56*4765] -c"%arg2"
            //аргумент начинается с % и содержит буквы или цифры, но не знаки или пробелы

            String pattern = "%\\w+";

            //отправить аргументы в статическую переменную
            m_AppMatchArguments = arguments;

            MatchEvaluator mev = new MatchEvaluator(AppMatchEvaluator);
            String result = Regex.Replace(cmdline, pattern, mev);
            return result;
        }

        private static string AppMatchEvaluator(Match match)
        {
            //аргумент приходит из выражения со знаком % первым символом
            //а в словаре имена аргументов без %, 
            //поэтому надо первый символ убрать из имени аргумента из выражения.
            String argName = match.Value.Substring(1);
            //TODO: кавычки в командной строке: Исправление от 10.07.2019 : если значение аргумента содержит пробелы, заключать его в кавычки.
            //Это только для аргументов командной строки
            //а можно было там их сразу в БД в кавычки заключать, а не здесь!
            String argValue = RegexManager.m_AppMatchArguments.GetByName(argName).ArgumentValue;
            if(argValue.Contains(" "))
                argValue = '"' + argValue + '"';
            return argValue;
            //если аргумента нет в словаре, здесь будет выброшено исключение.
            //такое может быть если изначально задан неправильный шаблон команды или строка запуска приложения
        }
#endregion


#region *** Конвертировать простой шаблон в регекс, сохраняя названия аргументов ***
        /// <summary>
        /// NT-Конвертировать простой шаблон в регекс, сохраняя названия аргументов 
        /// </summary>
        /// <param name="rx">Текст шаблона</param>
        /// <returns>Строка выражения регекса</returns>
        internal static string ConvertSimpleToRegex2(string rx)
        {
            //тут надо распарсить строку запроса, выделив аргументы.
            //затем заменить аргументы на строки, с учетом количества аргументов.

            //Пример простого регекса:
            //Копировать %файл в %папка
            String query = rx.Trim();
            String pattern = "%\\w+";

            MatchEvaluator mev = new MatchEvaluator(SimpleMatchEvaluator);
            String result = Regex.Replace(query, pattern, mev);
            return "^" + result + "$";
        }

        private static string SimpleMatchEvaluator(Match match)
        {
            String argName = match.Value;
            //у аргумента должен быть первый символ %, его надо выкинуть из имени группы.
            String res = "(?<" + argName.Substring(1) + ">.+)";
            return res;
        }
#endregion

        /// <summary>
        /// NT-Разделить строку запуска приложения на путь приложения и аргументы.
        /// Поддерживаются только exe и com расширения файлов.
        /// </summary>
        /// <param name="cmdline"></param>
        /// <returns></returns>
        internal static string[] ParseCommandLine(string cmdline)
        {
            //Можно было лучше сделать - последовательно брать куски с пробелами и проверять, существует ли такой путь и файл.
            //Если существует, то значит это и есть приложение.
            //Но пока сделаем так.
            //Если надо запустить файл не exe, то можно его переименовать в exe
            //А вот еще надо cmd файлы запускать тоже.
            
            String[] sar = new String[2];
            String[] patterns = new String[] { ".exe ", ".exe\"", ".com ", ".com\"", ".bat ", ".bat\"", ".cmd ", ".cmd\"" };
            int position;
            foreach(string pat in patterns)
            {
                position = cmdline.IndexOf(pat, StringComparison.OrdinalIgnoreCase);
                if (position >= 0)
                {
                    position += pat.Length;
                    sar[0] = cmdline.Substring(0, position); //app path
                    sar[1] = cmdline.Substring(position);//app arguments
                    return sar;
                }
            }
            //не нашли ничего 
            //может быть, там нет аргументов и нет пробела после расширения?
            if(System.IO.File.Exists(cmdline))
            {
                sar[0] = String.Copy(cmdline);
                sar[1] = "";
                return sar;
            }
            //может быть, это URI?
            sar[0] = String.Copy(cmdline);
            sar[1] = "";
            return sar;
            
            //пока нечем проверить что это - файловый путь или веб-адрес или я хз что,
            //так что просто пробуем выполнить. 
            //throw new Exception(String.Format("Неправильная строка запуска приложения: {0}", cmdline));
        }

        /// <summary>
        /// NT-Проверить что это путь к сборке кода 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static bool IsAssemblyCodePath(String path)
        {
            //проверить что путь это путь к функции сборки
            //имясборки.имякласса.имяфункции()
            //имясборки.имякласса.имяфункции(арг1)
            //имясборки.имякласса.имяфункции(арг1, арг2, арг3 )
            //строка паттерна регекса:  

            String pattern = @"^\w+\.\w+\.\w+\([\w,\s]*\)\w*$";
            return Regex.IsMatch(path, pattern);
        }

        /// <summary>
        /// NT-разделить путь сборки на имена частей и имена аргументов
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static string[] ParseAssemblyCodePath(string path)
        {

            List<string> lis = new List<string>();
            String p = path.Trim();
            String[] sar1 = p.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries);
            String names = sar1[0];
            String args = sar1[1];

            String[] sar2 = names.Split( new char[] { '.'}, StringSplitOptions.RemoveEmptyEntries);
            lis.Add(sar2[0]);//assembly name
            lis.Add(sar2[1]);//class name
            lis.Add(sar2[2]);//func name

            //отсечь все что после закрывающей скобки
            int pos = args.IndexOf(')');
            if (pos < 0) throw new Exception(String.Format("Неправильный путь: {0}", path));
            args = args.Remove(pos);

            //если там еще что-то есть, это должны быть аргументы
            if (args.Length > 0)
            {
                //разделить на аргументы
                String[] sar4 = args.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String s in sar4)
                {
                    lis.Add(s.Trim());
                }
            }
            return lis.ToArray();
        }

        /// <summary>
        /// NT-Извлечь аргументы из текста команды, если она совпала с шаблоном
        /// Возвращает список аргументов или нуль если не было совпадения.
        /// </summary>
        /// <param name="command">Текст команды</param>
        /// <param name="pattern">Шаблон команды</param>
        /// <returns>Возвращается коллекция аргументов</returns>
        internal static ArgumentCollection ExtractArgumentsFromCommand(string command, string pattern)
        {
            ArgumentCollection args = null;
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(command);
            
            if (m.Success == true)
            {
                //создаем коллекцию аргументов как флаг успеха сравнения
                args = new ArgumentCollection();
                for (int i = 0; i < m.Groups.Count; i++)
                {
                    //делаем из совпадений регекса аргументы и вносим в список аргументов 
                    String name = r.GroupNameFromNumber(i);
                    String value = m.Groups[i].Value;
                    args.Add(new FuncArgument(name, "", value, value));
                }
                //удаляем первый элемент, который содержит всю строку команды.
                if (args.Arguments.Count > 0)
                    args.Arguments.RemoveAt(0);
            }
            //возвращаем коллекцию аргументов, если матч успешный и нуль,если матч неуспешный
            return args;
        }
    }
}
