using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NT - Класс для регекс-операций
    /// </summary>
    internal class RegexManager
    {
        //TODO: отладить этот код и убрать все эти остатки из Java-версии.

        // TODO: надо написать тесты по функциям регексов здешних - чтобы все протестировать и уверенно работать дальше.
        //TODO: надо ускорить выполнение работ с регексами, поскольку для поиска процедуры для запроса  все имеющиеся процедуры перебираются каждый раз.
        //Это очень медленно, этот процесс надо ускорить, значит:
        //1. надо кешировать все регексы и все, что можно кешировать.
        //2. надо отбирать процедуры еще до проверки их регексов по совпадению первого слову запроса и первого слова Команды, для этого надо превое слово команды извлекать и отдельно хранить при загрузке команды в память.
        //3. надо отбирать процедуры по наличию их неймспейса в списке текущих неймспейсов в контексте сеанса, но это на будущее.


        #region Статические переменные для регекса Java версии - оставлены пока для справки, до релиза.
        ///**
        // * Символы, которые необходимо экранировать при переработке простого регекса
        // * в нормальный регекс
        // */
        //protected  static String unsafeRegexChars = ".$%[](?+*:^\\|{}";

        // если указан флаг Pattern.UNICODE_CHARACTER_CLASS, то \\w обозначает все
        // Unicode символы. А без флага - только [a-zA-Z_0-9]

        ///**
        // * Pattern string for isAssemblyCodePath() function
        // */
        //protected final static String ACPPatternString = "^\\w+\\.\\w+\\.\\w+\\([\\w,\\s]*\\)\\w*$";

        ///**
        // * Pattern constant regex object for isAssemblyCodePath() function -
        // * optimization
        // */
        //protected static Pattern ACPPattern = Pattern.compile(ACPPatternString);

        ///**
        // * Pattern string for makeNormalRegex() function
        // */
        //protected  static String MNRPatternString = "%\\w+";

        ///**
        // * Pattern constant regex object for makeNormalRegex() function -
        // * optimization
        // */
        //protected static Pattern MNRPattern = Pattern.compile(MNRPatternString, Pattern.UNICODE_CHARACTER_CLASS);

        ///**
        // * Pattern string for getGroupNames() function
        // */
        //protected  static String GGNPatternString = "\\(\\?<([\\w]+)>";

        ///**
        // * Pattern constant regex object for getGroupNames() function - optimization
        // */
        //protected static Pattern GGNPattern = Pattern.compile(GGNPatternString, Pattern.UNICODE_CHARACTER_CLASS);

        ///**
        // * Pattern string for makeNewArgName() function
        // */
        //protected  static String MNANPatternString = "[a-zA-Z][a-zA-Z_0-9]*";

        ///**
        // * Pattern constant regex object for makeNewArgName() function -
        // * optimization
        // */
        //protected static Pattern MNANPattern = Pattern.compile(MNANPatternString);
        #endregion

        /// <summary>
        /// NT-Определить тип регекса, содержащегося в переданной строке
        /// </summary>
        /// <param name="pattern">Строка, содержащая регекс</param>
        /// <returns>Возвращает одно из <typeparamref name="RegexType"/> значений</returns>
        public static EnumRegexType determineRegexType(String pattern)
        {
            //проверка наличия регекса в процедуре
            if (String.IsNullOrEmpty(pattern)) return EnumRegexType.Empty;
            //проверка наличия ^ и $
            bool b1 = (pattern[0] == '^');
            bool b2 = (pattern[pattern.Length - 1] == '$');
            if ((b1 & b2) == true) return EnumRegexType.NormalRegex; // это сложный регекс
            else if ((b1 | b2) == false) return EnumRegexType.SimpleString; //это простой регекс
            else return EnumRegexType.Invalid; //это ни то ни другое
        }



        #region *** Конвертировать простой шаблон в регекс, сохраняя названия аргументов ***

        ///**
        // * NT-Конвертировать простой шаблон в регекс, сохраняя названия аргументов.
        // * 
        // * @param simpleRx
        // *            Строка Простой регекс.
        // * @return Функция возвращает строку Нормальный регекс.
        // */
        //public static String ConvertSimpleToRegex2(String simpleRx)
        //{
        //    // тут надо распарсить строку запроса, выделив аргументы.
        //    // затем заменить аргументы на код групп, с учетом количества
        //    // аргументов.
        //    // И еще - экранировать слешами все служебные символы, которые
        //    // встретятся в регексе
        //    // иначе они испортят регекс и он не будет ничего находить.
        //    // Затем добавить символы ^ и $ для нормального регекса

        //    // Пример простого регекса:
        //    // Копировать %файл в %папка
        //    String query = simpleRx.Trim();

        //    Matcher m = RegexManager.MNRPattern.matcher(query);
        //    StringBuilder sb = new StringBuilder();
        //    // тут надо найти совпадение, получить его границы и текст
        //    // до начала совпадения поместить в выходной буфер после санобработки,
        //    // а текст совпадения заменить на правильный текст группы,
        //    // и тоже поместить в выходной буфер.
        //    // пример: *****1111*****222**
        //    int lastEnd = 0;// end of previous matched string
        //    int argCounter = 0;// counter of arguments
        //    int start, end;
        //    String part, partS, argname, newArgName;
        //    // iterate matches
        //    while (m.find())
        //    {
        //        // find match
        //        argname = m.group();
        //        start = m.start();
        //        end = m.end();// next after end
        //                      // получить часть текста до группы или между группами
        //        part = query.substring(lastEnd, start);
        //        // Экранировать служебные символы регекса до группы или между
        //        // группами.
        //        partS = makeSafeRegexChars(part);
        //        sb.append(partS);
        //        // добавить замену простого аргумента и удалить первый символ "%" из
        //        // имени аргумента
        //        // если имя аргумента содержит символы кроме [a-zA-Z_0-9], то
        //        // заменить все имя на arg_#, где # - порядковый номер аргумента в
        //        // выражении.
        //        sb.append("(?<");
        //        newArgName = makeNewArgName(argname, argCounter);
        //        sb.append(newArgName);
        //        sb.append(">.+)");
        //        // set new lastend
        //        lastEnd = end;
        //        // increment loop counter
        //        argCounter++;
        //    }
        //    // add last part after last match
        //    // if no matches found, then lastEnd = 0, all query be copied to result.
        //    part = query.substring(lastEnd);
        //    partS = makeSafeRegexChars(part);
        //    sb.append(partS);
        //    // добавить символы ^ и $ для нормального регекса
        //    return "^" + sb.toString() + "$";
        //    // TODO: Оптимизация: эти символы впихать в этот билдер сразу в
        //    // правильных местах, чтобы выкинуть операцию +.
        //}

        ///**
        // * NT-Создать правильное название аргумента для нормального регекса
        // * 
        // * @param argname
        // *            Исходное название аргумента из Простого регекса
        // * @param argCounter
        // *            Порядковый номер аргумента
        // * @return Функция возвращает правильное название аргумента для нормального
        // *         регекса
        // */
        //private static String makeNewArgName(String argname, int argCounter)
        //{
        //    // удалить первый символ аргумента простого регекса = %
        //    String newName = argname.Trim().substring(1);
        //    // использовать регекс
        //    Matcher m = RegexManager.MNANPattern.matcher(newName);
        //    // все ли название состоит из допустимых символов?
        //    Boolean valid = m.find();
        //    if (valid)
        //        return newName;
        //    else return "arg_" + Integer.toString(argCounter);

        //}

        ///**
        // * NT-Экранировать символы простого регекса, используемые в нормальном
        // * регексе, чтобы они не нарушали разбор.
        // * 
        // * @param part
        // *            Входной текст
        // * @return Функция возвращает текст с экранированными служебными символами
        // *         регекса.
        // */
        //private static String makeSafeRegexChars(String part)
        //{
        //    // Список символов, которые надо экранировать - перенесен в статические
        //    // члены класса.

        //    StringBuilder sb = new StringBuilder();
        //    for (char ch : part.toCharArray())
        //    {
        //        // если символ из входной строки есть в списке символов, которые
        //        // нужно экранировать, то
        //        if (RegexManager.unsafeRegexChars.indexOf((int)ch) != -1)
        //            sb.append("\\");// добавить
        //                            // экранирующий
        //                            // слеш
        //                            // Добавить сам символ
        //        sb.append(ch);
        //    }

        //    return sb.toString();
        //}

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
        /// NT-Проверить, что это путь к методу в сборке кода, а не что-либо другое. 
        /// </summary>
        /// <param name="path">Assembly code path</param>
        /// <returns> Функция возвращает True, если путь соответствует формату. False в противном случае.</returns>
        internal static bool IsAssemblyCodePath(String path)
        {
            //проверить что путь это путь к функции сборки
            //имясборки.имякласса.имяфункции()
            //имясборки.имякласса.имяфункции(арг1)
            //имясборки.имякласса.имяфункции(арг1, арг2, арг3 )  

            // неправильные случаи:
            // имясборки.имякласса.имяфункции (арг1) - пробел перед скобкой
            // имясборки.имякласса.имяфункции( арг1) - пробел после скобки
            //TODO: кешировать объект регекса в статической переменной, и строку регекса всделать константой.
            String pattern = @"^\w+\.\w+\.\w+\([\w,\s]*\)\w*$";
            // Проверить полное совпадение с паттерном
            return Regex.IsMatch(path, pattern);
        }

        

        /// <summary>
        /// NT-разделить путь сборки на имена частей и имена аргументов
        /// </summary>
        /// <param name="path">Входная строка пути из объекта Процедуры.</param>
        /// <returns>Функция возвращает массив имен частей пути сборки и имен аргументов.
        /// Первые три элемента - путь к сборке: сборка, класс, метод.
        /// Остальные элементы выходного массива - имена аргументов метода в порядке их следования.
        /// </returns>
        /// <exception cref="Exception">Выбрасывает исключение, если в path указан неправильный путь.</exception>
        internal static string[] ParseAssemblyCodePath(string path)
        {
            // тут не проверяем формат входной строки, так как регекс уже все проверил ранее в IsAssemblyCodePath()
            List<string> lis = new List<string>();
            String p = path.Trim();
            //TODO: следует перенести эти создаваемые массивы разделителей для Split() в приватные статические переменные, чтобы немного ускорить работу. 
            String[] sar1 = p.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries);
            String names = sar1[0];
            String args = sar1[1];

            String[] sar2 = names.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
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
            // TODO: Оптимизация: К Процедуре надо прицепить список названий аргументов,
            // чтобы не выполнять второй раз регекс для извлечения названий групп.
            // А брать их из этого списка в правильном порядке.
            // TODO: К Процедуре надо прицепить объекты регекса, чтобы не создавать их каждый раз, а использовать созданные в первый раз.
            // Тогда нужно разработать один объект-контейнер для этих объектов регекса и прицепить его к Процедуре при ее загрузке из БД.
            // Но делать это после завершения портирования текущего проекта, как обновление.
            // Чтобы не усложнять и не запутывать портирование проекта сейчас.

            // rx = ^my app.exe -t -d(?<arg1>.+)%[56*4765] -c"(?<arg2>.+)"$
            // cmd = my app.exe -t -dhttps://www.google.com%[56*4765] -c"udaff.com"

            ArgumentCollection args = null;
            //// тут надо распарсить строку запроса, выделив аргументы.
            //LinkedList<String> groupNames = getGroupNames(regex);
            //// ВАЖНО: регекс не работает с группами, названия которых на русском языке - выдает исключение
            //Pattern p = Pattern.compile(regex, Pattern.UNICODE_CHARACTER_CLASS | Pattern.CASE_INSENSITIVE);// Cannot move to static now! Но можно прицепить к
            //                                                                                               // процедуре каждый Pattern как объект.
            //Matcher m = p.matcher(query);// А этот нельзя прицепить к Процедуре.
            //Эта Java-версия регекса заменена обратно на Шарп-версию, но возможно она все так же не работает с русскими группами. 
            //TODO: удалить этот закомментированный код выше после проверки и отладки процесса парсинга тут.
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(command);
            
            // Проверить что совпадение регекса и текста есть
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

        ///**
        // * NT-Извлечь из регекса все имена групп строго в порядке следования
        // * 
        // * @param regex
        // *            Нормальный регекс, содержащий имена групп.
        // * @return Функция возвращает список имен групп в порядке их следования в регексе.
        // */
        //private static LinkedList<String> getGroupNames(String regex)
        //{
        //    LinkedList<String> result = new LinkedList<String>();
        //    // в названии групп в регексе можно использовать только латинские символы.
        //    // Русские символы приводят к выбросу исключения далее в разборе выражения.
        //    // Поэтому надо на этапе преобразования регекса из простого в нормальный заменять русские имена на arg0 итп, строго по порядку появления в регексе.
        //    // И еще нужна функция, позволяющая определить, что название аргумента - группы содержит не-латинские символы, или начинается не с буквы.
        //    // Matcher m = Pattern.compile("\\(\\?<([a-zA-Z][a-zA-Z_0-9]+)>", Pattern.UNICODE_CHARACTER_CLASS ).matcher(regex);
        //    Matcher m = GGNPattern.matcher(regex);

        //    while (m.find())
        //    {
        //        result.add(m.group(1));
        //    }

        //    return result;
        //}


        #region *** Замена аргументов командной строки приложений ***
        //TODO: Эту старую Ява-функцию убрать после проверки работы. Вдруг логика изменилась...
 //       /**
 //* NT-Заменить аргументы в командной строке приложения их значениями.
 //* 
 //* @param cmdline
 //*            Командная строка с аргументами в стиле Простой регекс.
 //* @param args
 //*            Коллекция аргументов Процедуры
 //* @return Функция возвращает командную строку с параметрами вместо аргументов.
 //* @throws Exception
 //*             Аргумент %s не найден! Вероятно, неправильная командная строка Процедуры.
 //*/
 //       public static String ConvertApplicationCommandString(
 //               String cmdline,
 //               ArgumentCollection args)
 //       {
 //           // распарсить строку вида: my app.exe -t -d%arg1%[56*4765] -c"%arg2"
 //           // Аргумент начинается с % и содержит буквы или цифры, но не знаки или пробелы.
 //           // Тут надо заменить аргументы в строке Простого регекса их значениями.
 //           // - это текст поля объекта Процедуры - пути и командной строки приложения, запускаемого в качестве Процедуры.
 //           // Командная строка может содержать ключи и аргументы в формате Простого регекса.
 //           // А в формате Нормального регекса - очень сложно ее задавать и применять.

 //           // TODO: кавычки в командной строке: Исправление от 10.07.2019 : если значение аргумента содержит пробелы, заключать его в кавычки.
 //           // Это только для аргументов командной строки.
 //           // Решено: все места, где могут быть такие случаи, в строке шаблона в кавычки заключать, а не здесь аватоматически!
 //           // Поскольку лучше в самой командной строке их в кавычки заключать - вдруг приложение не поддерживает кавычки,
 //           // а ведь пользователь не сможет их отменить без перекомпиляции кода Оператор.
 //           // - это глобальная фича - всю БД команд надо будет переписывать, чтобы отменить/применить эту фичу.

 //           // 1. найти аргументы в исходной строке
 //           // 2. заменить аргументы на значения
 //           // 3. вернуть получившуюся строку

 //           String query = cmdline.Trim();

 //           Matcher m = MNRPattern.matcher(query);
 //           StringBuilder sb = new StringBuilder();
 //           int lastEnd = 0;
 //           // int argCounter = 0; - сейчас не используется, но, возможно, потребуется...
 //           int start, end;
 //           String argname, part;
 //           // iterate matches
 //           while (m.find())
 //           {
 //               // find match
 //               argname = m.group();
 //               start = m.start();
 //               end = m.end();// next after end
 //                             // add part of query before match
 //               part = query.substring(lastEnd, start);
 //               sb.append(part);
 //               // аргумент приходит из выражения со знаком % первым символом
 //               // а в коллекции имена аргументов без %,
 //               // поэтому надо первый символ убрать из имени аргумента из выражения.
 //               // добавить значение простого аргумента по его индексу
 //               FuncArgument fa = args.getByName(argname.substring(1));
 //               // если аргумента нет в словаре, здесь будет выброшено исключение Null reference.
 //               // такое может быть если изначально задан неправильный шаблон команды
 //               // или строка запуска приложения
 //               if (fa == null)
 //                   throw new Exception(String.format("Аргумент \"%s\" не найден! Неправильная командная строка Процедуры.", argname));
 //               // write argument value
 //               sb.append(fa.get_ArgumentValue());
 //               // set new lastend
 //               lastEnd = end;
 //               // //increment loop counter
 //               // argCounter++;
 //           }
 //           // add last part after last match
 //           // if no matches found, then lastEnd = 0, all query be copied to result.
 //           sb.append(query.substring(lastEnd));

 //           return sb.toString();
 //       }


        /// <summary>
        /// Словарь аргументов для вставки в командную строку.
        /// </summary>
        private static ArgumentCollection m_AppMatchArguments;
        /// <summary>
        /// NT-Заменить аргументы в командной строке приложения их значениями.
        /// </summary>
        /// <param name="cmdline">Командная строка с аргументами в стиле Простой регекс.</param>
        /// <param name="arguments">Коллекция аргументов Процедуры</param>
        /// <returns>Функция возвращает командную строку с параметрами вместо аргументов.</returns>
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
            if (argValue.Contains(" "))
                argValue = '"' + argValue + '"';
            return argValue;
            //если аргумента нет в словаре, здесь будет выброшено исключение.
            //такое может быть если изначально задан неправильный шаблон команды или строка запуска приложения
        }
        #endregion

        /// <summary>
        /// NT-Разделить строку запуска приложения на путь приложения и аргументы.
        /// Поддерживаются только exe и com расширения файлов.
        /// </summary>
        /// <param name="cmdline">Строка запуска приложения</param>
        /// <returns>Функция возвращает массив строк: [0]= Путь к файлу приложения [1]= Строка аргументов приложения</returns>
        internal static string[] ParseCommandLine(string cmdline)
        {
            //Можно было лучше сделать - последовательно брать куски с пробелами и проверять, существует ли такой путь и файл.
            //Если существует, то значит это и есть приложение.
            //Но пока сделаем так.
            //Если надо запустить файл не exe, то можно его переименовать в exe
            //А вот еще надо cmd файлы запускать тоже.
            //И они могут заканчиваться кавычками, если путь содержит пробелы, надо и эти кавычки включать в путь.
            //и пробел в паттернах важен, см ниже.

            String[] sar = new String[2];
            String[] patterns = new String[] { ".exe ", ".exe\"", ".com ", ".com\"", ".bat ", ".bat\"", ".cmd ", ".cmd\"" };
            int position;
            foreach (string pat in patterns)
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
            if (File.Exists(cmdline))
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

    }
}
