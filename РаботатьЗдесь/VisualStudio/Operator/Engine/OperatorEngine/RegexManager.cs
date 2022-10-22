using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - Класс-контейнер для регекс-операций
    /// </summary>
    internal class RegexManager
    {
        //TODO: port from Java code this class

        // TODO: надо написать тесты по функциям регексов здешних - чтобы все протестировать и уверенно работать дальше.

        // Статические переменные для регекса
        /**
         * Символы, которые необходимо экранировать при переработке простого регекса
         * в нормальный регекс
         */
        protected final static String unsafeRegexChars = ".$%[](?+*:^\\|{}";

        // если указан флаг Pattern.UNICODE_CHARACTER_CLASS, то \\w обозначает все
        // Unicode символы. А без флага - только [a-zA-Z_0-9]

        /**
         * Pattern string for isAssemblyCodePath() function
         */
        protected final static String ACPPatternString = "^\\w+\\.\\w+\\.\\w+\\([\\w,\\s]*\\)\\w*$";

        /**
         * Pattern constant regex object for isAssemblyCodePath() function -
         * optimization
         */
        protected static Pattern ACPPattern = Pattern.compile(ACPPatternString);

        /**
         * Pattern string for makeNormalRegex() function
         */
        protected final static String MNRPatternString = "%\\w+";

        /**
         * Pattern constant regex object for makeNormalRegex() function -
         * optimization
         */
        protected static Pattern MNRPattern = Pattern.compile(MNRPatternString, Pattern.UNICODE_CHARACTER_CLASS);

        /**
         * Pattern string for getGroupNames() function
         */
        protected final static String GGNPatternString = "\\(\\?<([\\w]+)>";

        /**
         * Pattern constant regex object for getGroupNames() function - optimization
         */
        protected static Pattern GGNPattern = Pattern.compile(GGNPatternString, Pattern.UNICODE_CHARACTER_CLASS);

        /**
         * Pattern string for makeNewArgName() function
         */
        protected final static String MNANPatternString = "[a-zA-Z][a-zA-Z_0-9]*";

        /**
         * Pattern constant regex object for makeNewArgName() function -
         * optimization
         */
        protected static Pattern MNANPattern = Pattern.compile(MNANPatternString);

        /**
         * NT-Определить тип регекса, содержащегося в переданной строке
         * 
         * @param pattern
         *            Строка, содержащая регекс
         * @return Возвращает одно из RegexType значений.
         */
        public static EnumRegexType determineRegexType(String pattern)
        {
            // проверка наличия регекса в процедуре
            if (Utility.StringIsNullOrEmpty(pattern))
                return EnumRegexType.Empty;
            // проверка наличия ^ и $
            Boolean b1 = (pattern.charAt(0) == '^');
            Boolean b2 = (pattern.charAt(pattern.length() - 1) == '$');
            if ((b1 & b2) == true)
                return EnumRegexType.NormalRegex; // это сложный регекс
            else if ((b1 | b2) == false)
                return EnumRegexType.SimpleString; // это простой регекс
            else return EnumRegexType.Invalid; // это ни то ни другое
        }

        /**
         * NT-Конвертировать простой шаблон в регекс, сохраняя названия аргументов.
         * 
         * @param simpleRx
         *            Строка Простой регекс.
         * @return Функция возвращает строку Нормальный регекс.
         */
        public static String ConvertSimpleToRegex2(String simpleRx)
        {
            // тут надо распарсить строку запроса, выделив аргументы.
            // затем заменить аргументы на код групп, с учетом количества
            // аргументов.
            // И еще - экранировать слешами все служебные символы, которые
            // встретятся в регексе
            // иначе они испортят регекс и он не будет ничего находить.
            // Затем добавить символы ^ и $ для нормального регекса

            // Пример простого регекса:
            // Копировать %файл в %папка
            String query = simpleRx.Trim();

            Matcher m = RegexManager.MNRPattern.matcher(query);
            StringBuilder sb = new StringBuilder();
            // тут надо найти совпадение, получить его границы и текст
            // до начала совпадения поместить в выходной буфер после санобработки,
            // а текст совпадения заменить на правильный текст группы,
            // и тоже поместить в выходной буфер.
            // пример: *****1111*****222**
            int lastEnd = 0;// end of previous matched string
            int argCounter = 0;// counter of arguments
            int start, end;
            String part, partS, argname, newArgName;
            // iterate matches
            while (m.find())
            {
                // find match
                argname = m.group();
                start = m.start();
                end = m.end();// next after end
                              // получить часть текста до группы или между группами
                part = query.substring(lastEnd, start);
                // Экранировать служебные символы регекса до группы или между
                // группами.
                partS = makeSafeRegexChars(part);
                sb.append(partS);
                // добавить замену простого аргумента и удалить первый символ "%" из
                // имени аргумента
                // если имя аргумента содержит символы кроме [a-zA-Z_0-9], то
                // заменить все имя на arg_#, где # - порядковый номер аргумента в
                // выражении.
                sb.append("(?<");
                newArgName = makeNewArgName(argname, argCounter);
                sb.append(newArgName);
                sb.append(">.+)");
                // set new lastend
                lastEnd = end;
                // increment loop counter
                argCounter++;
            }
            // add last part after last match
            // if no matches found, then lastEnd = 0, all query be copied to result.
            part = query.substring(lastEnd);
            partS = makeSafeRegexChars(part);
            sb.append(partS);
            // добавить символы ^ и $ для нормального регекса
            return "^" + sb.toString() + "$";
            // TODO: Оптимизация: эти символы впихать в этот билдер сразу в
            // правильных местах, чтобы выкинуть операцию +.
        }

        /**
         * NT-Создать правильное название аргумента для нормального регекса
         * 
         * @param argname
         *            Исходное название аргумента из Простого регекса
         * @param argCounter
         *            Порядковый номер аргумента
         * @return Функция возвращает правильное название аргумента для нормального
         *         регекса
         */
        private static String makeNewArgName(String argname, int argCounter)
        {
            // удалить первый символ аргумента простого регекса = %
            String newName = argname.Trim().substring(1);
            // использовать регекс
            Matcher m = RegexManager.MNANPattern.matcher(newName);
            // все ли название состоит из допустимых символов?
            Boolean valid = m.find();
            if (valid)
                return newName;
            else return "arg_" + Integer.toString(argCounter);

        }

        /**
         * NT-Экранировать символы простого регекса, используемые в нормальном
         * регексе, чтобы они не нарушали разбор.
         * 
         * @param part
         *            Входной текст
         * @return Функция возвращает текст с экранированными служебными символами
         *         регекса.
         */
        private static String makeSafeRegexChars(String part)
        {
            // Список символов, которые надо экранировать - перенесен в статические
            // члены класса.

            StringBuilder sb = new StringBuilder();
            for (char ch : part.toCharArray())
            {
                // если символ из входной строки есть в списке символов, которые
                // нужно экранировать, то
                if (RegexManager.unsafeRegexChars.indexOf((int)ch) != -1)
                    sb.append("\\");// добавить
                                    // экранирующий
                                    // слеш
                                    // Добавить сам символ
                sb.append(ch);
            }

            return sb.toString();
        }

        /**
         * NT-Проверить, что это путь к методу в сборке кода, а не что-либо другое.
         * 
         * @param path
         *            Assembly code path
         * @return Функция возвращает True, если путь соответствует формату. False в
         *         противном случае.
         */
        public static Boolean IsAssemblyCodePath(String path)
        {
            // проверить что путь это путь к функции сборки
            // имясборки.имякласса.имяфункции()
            // имясборки.имякласса.имяфункции(арг1)
            // имясборки.имякласса.имяфункции(арг1, арг2, арг3 )

            // неправильные случаи:
            // имясборки.имякласса.имяфункции (арг1) - пробел перед скобкой
            // имясборки.имякласса.имяфункции( арг1) - пробел после скобки
            // return Regex.IsMatch(path, pattern);
            Matcher m = ACPPattern.matcher(path);
            // Проверить полное совпадение с паттерном
            Boolean found = m.matches();

            return found;
        }

        /**
         * NT-разделить путь сборки на имена частей и имена аргументов
         * 
         * @param path
         *            Входная строка пути из объекта Процедуры.
         * @return Функция возвращает массив имен частей пути сборки и имен
         *         аргументов.
         *         Первые три элемента - путь к сборке: сборка, класс, метод.
         *         Остальные элементы выходного массива - имена аргументов метода в
         *         порядке их следования.
         * @throws Exception
         *             Выбрасывает исключение, если в path указан неправильный путь.
         */
        public static String[] ParseAssemblyCodePath(String path) throws Exception
        {
            // тут не проверяем формат входной строки, так как регекс
            // уже все проверил ранее в IsAssemblyCodePath()
            LinkedList<String> lis = new LinkedList<String>();
        String p = path.Trim();
        // p.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries);
        String[] sar1 = Utility.StringSplit(p, "\\(", true);
        // TODO: Оптимизация: можно ли обойтись без регекса? Он наверно медленнее поиска в строке?

        String names = sar1[0];
        String args = sar1[1];

        // names.Split( new char[] { '.'},
        // StringSplitOptions.RemoveEmptyEntries);
        String[] sar2 = Utility.StringSplit(names, "\\.", true);
        // TODO: Оптимизация: можно ли обойтись без регекса?

        lis.add(sar2[0].Trim());// assembly name
        lis.add(sar2[1].Trim());// class name
        lis.add(sar2[2].Trim());// func name
        // TODO: можно ли уместить путь к методу в Java в 3 элемента?

        // отсечь все что после закрывающей скобки
        int pos = args.indexOf(")");
        if (pos< 0)
            throw new Exception(String.format("Неправильный путь: %s", path));
        // но этого не может быть - ведь мы уже проверили формат пути ранее

        args = args.substring(0, pos).Trim(); // как .Remove(pos);? - проверить!
        // если там еще что-то есть, это должны быть аргументы
        if (args.length() > 0)
        {
            // разделить на аргументы
            // String[] sar4 = args.Split(new char[] { ',' },
            // StringSplitOptions.RemoveEmptyEntries);
            String[] sar4 = Utility.StringSplit(args, "\\,", true);
            // TODO: Оптимизация: можно ли обойтись без регекса?

            for (String s : sar4)
            {
                lis.add(s.Trim());
            }
}
return lis.toArray(new String[lis.size()]);
    }

    /**
     * NT-Извлечь аргументы из текста команды, если она совпала с шаблоном.
     * 
     * @param query
     *            Текст команды (запроса пользователя)
     * @param regex
     *            Регекс команды
     * @return Функция возвращает коллекцию аргументов или нуль если не было совпадения.
     */
    public static ArgumentCollection ExtractArgumentsFromCommand(
            String query,
            String regex)
{
    ArgumentCollection args = null;
    // TODO: Оптимизация: К Процедуре надо прицепить список названий аргументов,
    // чтобы не выполнять второй раз регекс для извлечения названий групп.
    // А брать их из этого списка в правильном порядке.
    // TODO: К Процедуре надо прицепить объекты регекса, чтобы не создавать их каждый раз, а использовать созданные в первый раз.
    // Тогда нужно разработать один объект-контейнер для этих объектов регекса и прицепить его к Процедуре при ее загрузке из БД.
    // Но делать это после завершения портирования текущего проекта, как обновление.
    // Чтобы не усложнять и не запутывать портирование проекта сейчас.

    // rx = ^my app.exe -t -d(?<arg1>.+)%[56*4765] -c"(?<arg2>.+)"$
    // cmd = my app.exe -t -dhttps://www.google.com%[56*4765] -c"udaff.com"

    // тут надо распарсить строку запроса, выделив аргументы.
    LinkedList<String> groupNames = getGroupNames(regex);
    // ВАЖНО: регекс не работает с группами, названия которых на русском языке - выдает исключение
    Pattern p = Pattern.compile(regex, Pattern.UNICODE_CHARACTER_CLASS | Pattern.CASE_INSENSITIVE);// Cannot move to static now! Но можно прицепить к
                                                                                                   // процедуре каждый Pattern как объект.
    Matcher m = p.matcher(query);// А этот нельзя прицепить к Процедуре.
                                 // Проверить что совпадение регекса и текста есть
    if (m.matches() == true)
    {
        // создаем коллекцию аргументов как флаг успеха сравнения
        args = new ArgumentCollection();
        // делаем из совпадений регекса аргументы и вносим в список аргументов
        // Извлечь имена и значения групп тут
        for (String name : groupNames)
            {
    String value = m.group(name);
    args.Add(new FuncArgument(name, "", value, value));
}
        }
        // очистить список имен групп для порядка
        groupNames.clear();

// возвращаем коллекцию аргументов, если матч успешный и нуль,если матч неуспешный
return args;
    }

    /**
     * NT-Извлечь из регекса все имена групп строго в порядке следования
     * 
     * @param regex
     *            Нормальный регекс, содержащий имена групп.
     * @return Функция возвращает список имен групп в порядке их следования в регексе.
     */
    private static LinkedList<String> getGroupNames(String regex)
{
    LinkedList<String> result = new LinkedList<String>();
    // в названии групп в регексе можно использовать только латинские символы.
    // Русские символы приводят к выбросу исключения далее в разборе выражения.
    // Поэтому надо на этапе преобразования регекса из простого в нормальный заменять русские имена на arg0 итп, строго по порядку появления в регексе.
    // И еще нужна функция, позволяющая определить, что название аргумента - группы содержит не-латинские символы, или начинается не с буквы.
    // Matcher m = Pattern.compile("\\(\\?<([a-zA-Z][a-zA-Z_0-9]+)>", Pattern.UNICODE_CHARACTER_CLASS ).matcher(regex);
    Matcher m = GGNPattern.matcher(regex);

    while (m.find())
    {
        result.add(m.group(1));
    }

    return result;
}

/**
 * NT-Заменить аргументы в командной строке приложения их значениями.
 * 
 * @param cmdline
 *            Командная строка с аргументами в стиле Простой регекс.
 * @param args
 *            Коллекция аргументов Процедуры
 * @return Функция возвращает командную строку с параметрами вместо аргументов.
 * @throws Exception
 *             Аргумент %s не найден! Вероятно, неправильная командная строка Процедуры.
 */
public static String ConvertApplicationCommandString(
        String cmdline,
        ArgumentCollection args) throws Exception
{
    // распарсить строку вида: my app.exe -t -d%arg1%[56*4765] -c"%arg2"
    // Аргумент начинается с % и содержит буквы или цифры, но не знаки или пробелы.
    // Тут надо заменить аргументы в строке Простого регекса их значениями.
    // - это текст поля объекта Процедуры - пути и командной строки приложения, запускаемого в качестве Процедуры.
    // Командная строка может содержать ключи и аргументы в формате Простого регекса.
    // А в формате Нормального регекса - очень сложно ее задавать и применять.

    // TODO: кавычки в командной строке: Исправление от 10.07.2019 : если значение аргумента содержит пробелы, заключать его в кавычки.
    // Это только для аргументов командной строки.
    // Решено: все места, где могут быть такие случаи, в строке шаблона в кавычки заключать, а не здесь аватоматически!
    // Поскольку лучше в самой командной строке их в кавычки заключать - вдруг приложение не поддерживает кавычки,
    // а ведь пользователь не сможет их отменить без перекомпиляции кода Оператор.
    // - это глобальная фича - всю БД команд надо будет переписывать, чтобы отменить/применить эту фичу.

    // 1. найти аргументы в исходной строке
    // 2. заменить аргументы на значения
    // 3. вернуть получившуюся строку

    String query = cmdline.Trim();

    Matcher m = MNRPattern.matcher(query);
    StringBuilder sb = new StringBuilder();
int lastEnd = 0;
// int argCounter = 0; - сейчас не используется, но, возможно, потребуется...
int start, end;
String argname, part;
// iterate matches
while (m.find())
{
    // find match
    argname = m.group();
    start = m.start();
    end = m.end();// next after end
                  // add part of query before match
    part = query.substring(lastEnd, start);
    sb.append(part);
    // аргумент приходит из выражения со знаком % первым символом
    // а в коллекции имена аргументов без %,
    // поэтому надо первый символ убрать из имени аргумента из выражения.
    // добавить значение простого аргумента по его индексу
    FuncArgument fa = args.getByName(argname.substring(1));
    // если аргумента нет в словаре, здесь будет выброшено исключение Null reference.
    // такое может быть если изначально задан неправильный шаблон команды
    // или строка запуска приложения
    if (fa == null)
        throw new Exception(String.format("Аргумент \"%s\" не найден! Неправильная командная строка Процедуры.", argname));
    // write argument value
    sb.append(fa.get_ArgumentValue());
    // set new lastend
    lastEnd = end;
    // //increment loop counter
    // argCounter++;
}
// add last part after last match
// if no matches found, then lastEnd = 0, all query be copied to result.
sb.append(query.substring(lastEnd));

return sb.toString();
    }

    /**
     * NR-Разделить строку запуска приложения на путь приложения и аргументы.
     * 
     * @param cmdline
     *            Строка запуска приложения
     * @return Функция возвращает массив строк: [0]= Путь к файлу приложения [1]= Строка аргументов приложения
     */
    public static String[] ParseCommandLine(String cmdline)
{
    // TODO: Надо изучать тему файлов приложений и запуска файлов приложений.
    // Регекс пока нельзя применять - неясно, что куда зачем и чего ожидать при применении функции.

    // Старый код для виндовс-версии этой функции

    // // Можно было лучше сделать - последовательно брать куски с пробелами и
    // // проверять, существует ли такой путь и файл.
    // // Если существует, то значит это и есть приложение.
    // // Но пока сделаем так.
    // // Если надо запустить файл не exe, то можно его переименовать в exe
    // // А вот еще надо cmd файлы запускать тоже.
    //
    // String[] sar = new String[2];
    // String[] patterns = new String[] { ".exe ", ".exe\"", ".com ", ".com\"",
    // ".bat ", ".bat\"", ".cmd ", ".cmd\"" };// TODO:
    // // расширения
    // // в
    // // Линукс
    // // не
    // // используются!
    // int position;
    // for (String pat : patterns)
    // {
    // position = cmdline.IndexOf(pat, StringComparison.OrdinalIgnoreCase);
    // if (position >= 0)
    // {
    // position += pat.Length;
    // sar[0] = cmdline.Substring(0, position); // app path
    // sar[1] = cmdline.Substring(position);// app arguments
    // return sar;
    // }
    // }
    // // не нашли ничего
    // // может быть, там нет аргументов и нет пробела после расширения?
    // if (System.IO.File.Exists(cmdline))
    // {
    // sar[0] = String.Copy(cmdline);
    // sar[1] = "";
    // return sar;
    // }
    // // может быть, это URI?
    // sar[0] = String.Copy(cmdline);
    // sar[1] = "";
    // return sar;
    //
    // // пока нечем проверить что это - файловый путь или веб-адрес или я хз
    // // что,
    // // так что просто пробуем выполнить.
    // // throw new Exception(String.Format("Неправильная строка запуска
    // // приложения: {0}", cmdline));
    return null;// for debug only
}

    }
}
