using System;
using System.Collections.Generic;
using System.IO;
using Operator.Lexicon;

namespace Operator
{
    // Консоль Оператора:
    // Функции доступа к консоли из сборок процедур сейчас перенесены в класс Operator.Lexicon.DialogConsole.
    // Для вывода сообщений на консоль использовать только! объект engine.OperatorConsole.
    //  так как я планирую вынести консоль совсем отдельно, то надо уже сейчас ее использование ограничить. 
    
    /// <summary>
    /// основной класс механизма исполнения процедур
    /// </summary>
    public class Engine
    {

        /// <summary>
        /// Писатель файла лога
        /// </summary>
        private StreamWriter logWriter;
        /// <summary>
        /// Объект адаптера БД Оператора
        /// </summary>
        /// <remarks>
        /// Кешированный адаптер БД содержит коллекции элементов и сам их обслуживает
        /// </remarks>
        private CachedDbAdapter m_db;
        /// <summary>
        /// Объект консоли Оператора 
        /// </summary>
        /// <remarks>
        /// Выделен чтобы упорядочить код работающий с консолью, так как он вызывается из сборок процедур, создаваемых сторонними разработчиками
        /// </remarks>
        private DialogConsole m_OperatorConsole;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">Initialized Log writer</param>
        public Engine(StreamWriter log)
        {
            this.logWriter = log;
            //подцепить БД
            this.m_db = new CachedDbAdapter();
            //создать объект консоли Оператора
            this.m_OperatorConsole = new DialogConsole(this);

            return;
        }
        #region Properties
        /// <summary>
        /// Объект адаптера БД Оператора
        /// </summary>
        /// <remarks>
        /// Не должен быть доступен из сторонних сборок.
        /// </remarks>
        internal CachedDbAdapter Database
        {
            get { return m_db; }
        }

        /// <summary>
        /// Объект консоли Оператора 
        /// </summary>
        /// <remarks>Должен быть доступен из сторонних сборок.</remarks>
        public DialogConsole OperatorConsole
        {
            get { return m_OperatorConsole; }
        }
        #endregion

        #region Функции инициализации и завершения движка
        /// <summary>
        /// NR-Инициализация механизма
        /// </summary>
        public void Init()
        {
            //выводим приветствие и описание программы
            this.OperatorConsole.PrintTextLine("Консоль речевого интерфейса. Версия " + Utility.getOperatorVersionString(), DialogConsoleColors.Сообщение);
            this.OperatorConsole.PrintTextLine("Для завершения работы приложения введите слово выход или quit", DialogConsoleColors.Сообщение);
            this.OperatorConsole.PrintTextLine("Сегодня " + this.OperatorConsole.CreateLongDatetimeString(DateTime.Now), DialogConsoleColors.Сообщение);

            logWriter.WriteLine("SESSION {0}", DateTime.Now.ToString());

            ////init database
            ////заполнить кеш-коллекции процедур и мест данными из БД
            ////CachedDbAdapter делает это сам
            //m_db.Open(DbAdapter.CreateConnectionString("SIdb.mdb"));

            //если новой бд нет в каталоге приложения, создаем ее и копируем в нее все данные из старой БД.
            string str = "sidb.sqlite";
            string connectionString = SqliteDbAdapter.CreateConnectionString(str, false);
            SqliteDbAdapter sqliteDbAdapter = new SqliteDbAdapter();
            if (!File.Exists(str))
            {
                SqliteDbAdapter.DatabaseCreate(str);
                sqliteDbAdapter.Open(connectionString);
                sqliteDbAdapter.CreateDatabaseTables();
                sqliteDbAdapter.Close();
                if (File.Exists("SIdb.mdb"))
                {
                    OleDbAdapter oleDbAdapter = new OleDbAdapter();
                    oleDbAdapter.Open(OleDbAdapter.CreateConnectionString("SIdb.mdb"));
                    List<Place> allPlaces = oleDbAdapter.GetAllPlaces();
                    List<Procedure> allProcedures = oleDbAdapter.GetAllProcedures();
                    oleDbAdapter.Close();
                    sqliteDbAdapter.Open();
                    sqliteDbAdapter.TransactionBegin();
                    foreach (Place p in allPlaces)
                        sqliteDbAdapter.AddPlace(p);
                    sqliteDbAdapter.TransactionCommit();
                    sqliteDbAdapter.TransactionBegin();
                    foreach (Procedure p in allProcedures)
                        sqliteDbAdapter.AddProcedure(p);
                    sqliteDbAdapter.TransactionCommit();
                    sqliteDbAdapter.Close();
                }
            }
            this.m_db.Open(connectionString);


            //БД оставим открытой на весь сеанс работы Оператора
            return;
        }




        /// <summary>
        /// NT-Подготовка к завершению работы механизма
        /// </summary>
        internal void Exit()
        {
            //Закрыть БД если еще не закрыта
            if (this.m_db != null)
                this.m_db.Close();
            //но не обнулять ссылку, так как объект БД создается в конструкторе, а не в Init()
            //Хотя вряд ли объект будет еще раз инициализирован и использован, но не надо путать слои.
            return;
        }
        #endregion


        #region Основной цикл исполнения механизма
        /// <summary>
        /// NT-Основной цикл исполнения механизма
        /// </summary>
        public ProcedureResult ProcessLoop()
        {
            ProcedureResult result = ProcedureResult.Unknown;
            //запускаем цикл приема запросов
            while (true)
            {
                this.OperatorConsole.PrintTextLine(String.Empty, DialogConsoleColors.Сообщение);
                this.OperatorConsole.PrintTextLine("Введите ваш запрос:", DialogConsoleColors.Сообщение);
                String query = this.OperatorConsole.ReadLine();
                //если был нажат CTRL+C, query может быть null
                //пока я не знаю, что делать в этом случае, просто перезапущу цикл приема команды
                // и при пустой строке тоже просто перезапустить цикл приема команды

                //Операторы: return закрывает Оператор, а continue - переводит на следующий цикл приема команды

                //триммим из запроса пробелы всякие лишние сразу же
                //если строка пустая, начинаем новый цикл приема команды
                if (String.IsNullOrEmpty(query))
                    continue;
                query = query.Trim();//query теперь может оказаться пустой строкой
                if (String.IsNullOrEmpty(query))
                    continue;
                //а если нет - обрабатываем запрос
                logWriter.WriteLine("QUERY {0}", query);
                //Если запрос требует завершения работы, завершаем цикл приема запросов. 
                //Далее должно следовать сохранение результатов и закрытие приложения.
                if (Dialogs.isSleepCommand(query) == true) //спящий режим компьютера
                {
                    PowerManager.DoSleep();//запущенные приложения не закрываются, и Оператор - тоже
                    continue; //не return, так как return завершает работу Оператора!
                }
                else if (Dialogs.isExitAppCommand(query) == true) return ProcedureResult.Exit; //закрытие приложения
                else if (Dialogs.isExitShutdownCommand(query) == true) return ProcedureResult.ExitAndShutdown;//закрытие приложения и выключение машины
                else if (Dialogs.isExitReloadCommand(query) == true) return ProcedureResult.ExitAndReload;//закрытие приложения и перезагрузка машины
                else if (Dialogs.isExitLogoffCommand(query) == true) return ProcedureResult.ExitAndLogoff;//закрытие приложения и завершение сеанса пользователя
                //TODO: вообще-то при команде перезагрузки надо сначала запрашивать подтверждение
                //А это нужно делать внутри кода процедуры, а не здесь. Но пока мы просто тестируем возможность.
                //Да и пользователь должен решать, какие слова использовать для этих команд. А они сейчас прошиты в коде.
                
                //если функция вернет любой флаг выхода, завершаем цикл приема запросов 
                result = EventCommandArrived(query);

                //вывести сообщение-подтверждение результата процедуры
                describeProcedureResult(result);

                //TODO: Режимы сна: если прочие программы не завершаются при засыпании, то и завершать работу здесь не нужно.
                if ((result == ProcedureResult.Exit)
                    || (result == ProcedureResult.ExitAndReload) //перезагрузка компьютера
                    || (result == ProcedureResult.ExitAndShutdown)//выключение компьютера
                    || (result == ProcedureResult.ExitAndLogoff)) //выход пользователя - программы закрываются!
                    return result;

                //модификация запроса и перезапуск его обработки здесь не предполагается.
            }

            return ProcedureResult.Unknown;  //тут никогда мы не должны оказаться.
        }
        /// <summary>
        /// NT-Обработчик события "Поступила новая команда"
        /// </summary>
        /// <param name="query">Текст запроса команды</param>
        /// <returns></returns>
        private ProcedureResult EventCommandArrived(String query)
        {
            //сейчас тупо исполним весь запрос целиком
            //result = DoQuery(query);

            ProcedureResult result = CommandAnalyser.ProcessQuery(this, query);

            return result;
        }

        /// <summary>
        /// NT-вывести сообщение-подтверждение результата процедуры
        /// Играть звуковой сигнал, если результат - ошибка.
        /// Подтверждение не выводится, если завершение процедуры - успешное.
        /// </summary>
        /// <param name="result">Результат исполнения процедуры</param>
        private void describeProcedureResult(ProcedureResult result)
        {
            String msg = null;
            bool ErrorAndBeep = false;
            switch (result)
            {
                case ProcedureResult.CancelledByUser:
                    msg = "Процедура прервана пользователем";
                    break;
                case ProcedureResult.Error:
                case ProcedureResult.Unknown:
                    msg = "Ошибка при исполнении процедуры";
                    ErrorAndBeep = true;
                    break;
                case ProcedureResult.Exit:
                    msg = "Завершение программы...";
                    break;
                case ProcedureResult.ExitAndHybernate:
                case ProcedureResult.ExitAndSleep:
                    msg = "Переход в спящий режим...";
                    break;
                case ProcedureResult.ExitAndLogoff:
                    msg = "Завершение сеанса пользователя...";
                    break;
                case ProcedureResult.ExitAndReload:
                    msg = "Перезагрузка компьютера...";
                    break;
                case ProcedureResult.ExitAndShutdown:
                    msg = "Выключение компьютера...";
                    break;
                case ProcedureResult.WrongArguments:
                    msg = "Ошибка: неправильные аргументы";
                    ErrorAndBeep = true;
                    break;
                default:
                    break;
            }
            //выбрать цвет сообщения  о результате процедуры 
            //подать звуковой сигнал при ошибке
            DialogConsoleColors color;
            if (ErrorAndBeep == true)
            {
                this.OperatorConsole.Beep();
                color = DialogConsoleColors.Предупреждение;
            }
            else
                color = DialogConsoleColors.Сообщение;
            //выдать сообщение о результате процедуры    
            //если курсор не в начале строки, начать сообщение с новой строки.
            this.OperatorConsole.SureConsoleCursorStart();
            this.OperatorConsole.PrintTextLine(msg, color);

            return;
        }


        /// <summary>
        /// NT-обрабатываем запрос пользователя. 
        /// Возвращаем false для завершения работы приложения
        /// </summary>
        /// <param name="cmdline">Текст запроса</param>
        internal ProcedureResult DoQuery(string query)
        {
            //найти подходящую процедуру для запроса
            //перебором всех процедур.
            ProcedureResult result;
            String regex = null;

            //22052020 - фича: если запрос не русскоязычный, то передать его в терминал. Иначе - исполнять. 
            if (Utility.IsNotRussianFirst(query))
                return ExecuteWithTerminal(query);
            
            foreach (Procedure p in this.m_db.Procedures.Procedures)
            {
                //собрать нормальный регекс для процедуры
                //TODO: optimization - можно же это сделать после загрузки регекса из БД как часть процесса распаковки данных,
                // а не при каждом исполнении команды от пользователя. 
                regex = MakeNormalRegex(p);
                //выполнить регекс и определить, является ли процедура пригодной для исполнения
                //bool res = RegexManager.IsMatchQuery(rx, cmdline);
                //if (res == true)
                ArgumentCollection args = RegexManager.ExtractArgumentsFromCommand(query, regex);
                if (args != null)
                {
                    //Тут запускаем процедуру. Она должна теперь проверить свои аргументы и все условия, 
                    //и если они не подходят, то завершиться с флагом ProcedureResult.WrongArguments.
                    result = Execute(query, regex, p, args);
                    if (result != ProcedureResult.WrongArguments) return result;
                }
            }
            //Тут состояние "Не удалось подобрать процедуру для исполнения запроса"
            //Вынесем его в функцию-обработчик, чтобы модифицировать обработку этого события.
            EventCommandNotExecuted();

            return ProcedureResult.Success;
        }
        /// <summary>
        /// NT-Обработать событие "Не удалось подобрать процедуру для исполнения запроса"
        /// </summary>
        private void EventCommandNotExecuted()
        {
            //выводим сообщение что для запроса не удалось подобрать процедуру
            this.OperatorConsole.PrintTextLine("Я такое не умею", DialogConsoleColors.Сообщение);

            //вообще же тут можно выполнять другую обработку, наверно...

            return;
        }

        /// <summary>
        /// NT-Открыть запрос в Терминале Виндовс
        /// </summary>
        /// <param name="query">Строка запроса</param>
        /// <returns></returns>
        private ProcedureResult ExecuteWithTerminal(string query)
        {
            ProcedureResult result = ProcedureResult.Success;
            try
            {
                //cmd.exe /K query

                String app = "cmd.exe";
                String args = "/K " + query;
                PowerManager.ExecuteApplication(app, args);
            }
            catch (Exception e)
            {
                PrintExceptionToConsole(e);
                result = ProcedureResult.Error;//флаг что процедура не годится
            }

            return result;
        }

        /// <summary>
        /// NT- Собрать нормальный регекс для процедуры
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string MakeNormalRegex(Procedure p)
        {
            String rx = null;
            //получить тип регекса
            RegexType rt = RegexManager.determineRegexType(p.Regex);
            //конвертировать регекс в пригодный для исполнения
            if (rt == RegexType.NormalRegex)
            {
                rx = String.Copy(p.Regex);
            }
            else if (rt == RegexType.SimpleString)
            {
                rx = RegexManager.ConvertSimpleToRegex2(p.Regex);
            }
            else throw new Exception(String.Format("Invalid regex string: {0} in {1}", p.Regex, p.Title));

            return rx;
        }

        /// <summary>
        /// NT-Должна вернуть облом при неподходящих параметрах, успех при исполнении, выход если требуется завершение работы приложения или компьютера
        /// </summary>
        /// <param name="command">Текст команды пользователя</param>
        /// <param name="regex">Регулярное выражение, готовое для работы</param>
        /// <param name="p">Объект процедуры</param>
        /// <param name="args">Коллекция аргументов</param>
        /// <returns></returns>
        private ProcedureResult Execute(string command, string regex, Procedure p, ArgumentCollection args)
        {
            //и еще нужно этим аргументам сопоставить типы мест хотя бы
            TryAssignPlaces(args);

            //надо определить, путь исполнения это путь к процедуре или к приложению.
            bool AssemblyCodePath = RegexManager.IsAssemblyCodePath(p.Path);
            if (AssemblyCodePath == false)
            {
                //если к приложению, его надо запустить и вернуть стандартное значение для продолжения работы.
                return RunShellExecute(p, args);
            }
            else
            {
                //если к процедуре, надо приготовить аргументы, найти сборку, вызвать функцию, передать ей аргументы и вернуть результат.
                return RunLocalAssembly(command, p, args);
            }

        }
        /// <summary>
        /// RT- Запустить функцию из локальной сборки
        /// </summary>
        /// <param name="command">Команда пользователя</param>
        /// <param name="p">Объект процедуры</param>
        /// <param name="args">Коллекция аргументов</param>
        /// <returns>Возвращается результат выполнения процедуры</returns>
        private ProcedureResult RunLocalAssembly(string command, Procedure p, ArgumentCollection args)
        {
            ProcedureResult result = ProcedureResult.Success;
            try
            {
                //получить имена частей пути. в порядке: сборка, класс, функция, аргументы по порядку следования если они есть
                string[] names = RegexManager.ParseAssemblyCodePath(p.Path);
                //TODO: тут аргументы уже должны быть заполнены значениями и типами и местами и готовы к выполнению.
                result = p.invokeProcedure(command, names, this, args);
            }
            catch (Exception e)
            {
                //вызов исполнения не удался.
                //пока выведем сообщение об исключении в консоль.
                //TODO: вот надо завести в механизме статическую переменную отладки, включаемую через отдельную процедуру, и по ней выводить на экран эти отладочные данные. 
                //TODO: надо вывести тут сообщение об исключении в общий лог.
                //если не выводить сообщение об ошибке, то непонятно, почему команда не исполняется.
                //например, когда выключился спящий режим, команда спать просто выводила сообщение я не умею.

                PrintExceptionToConsole(e);

                result = ProcedureResult.WrongArguments;//флаг что процедура не годится
            }
            //возвращаем то что вернет процедура
            return result;
        }


        /// <summary>
        /// NT-Запустить команду через механизм ShellExecute
        /// </summary>
        /// <param name="p">Объект процедуры</param>
        /// <param name="args">Коллекция аргументов</param>
        /// <returns>Возвращается результат выполнения процедуры</returns>
        private ProcedureResult RunShellExecute(Procedure p, ArgumentCollection args)
        {
            //типы мест определять здесь не имеет смысла - они все равно не учитываются в запускаемом приложении
            ProcedureResult result = ProcedureResult.Success;
            try
            {
                //вставить аргументы в командную строку приложения
                String cmdline = RegexManager.ConvertApplicationCommandString(p.Path, args);
                //запустить приложение
                PowerManager.ExecuteApplication(cmdline);
            }
            catch (Exception e)
            {
                //вызов исполнения не удался.
                //пока выведем сообщение об исключении в консоль.
                //TODO: вот надо завести в механизме статическую переменную отладки, включаемую через отдельную процедуру, и по ней выводить на экран эти отладочные данные. 
                //TODO: надо вывести тут сообщение об исключении в общий лог.
                //если не выводить сообщение об ошибке, то непонятно, почему команда не исполняется.
                //например, когда выключился спящий режим, команда спать просто выводила сообщение я не умею.
                this.OperatorConsole.PrintExceptionMessage(e);
                result = ProcedureResult.WrongArguments;//флаг что процедура не годится
            }
            //вернуть результат
            return result;
        }
        /// <summary>
        /// NT-Сопоставить данные аргументов и места из коллекции мест, насколько это возможно.
        /// </summary>
        /// <param name="args"></param>
        private void TryAssignPlaces(ArgumentCollection args)
        {

            //тут надо если у аргумента название есть в словаре мест, то скопировать в аргумент значение этого места
            //пока без проверки типов и всего такого, так как это должна бы делать процедура.
            foreach (FuncArgument f in args.Arguments)
            {
                String name = f.ArgumentValue;
                if (this.m_db.Places.ContainsPlace(name))
                {
                    //извлечем место
                    Place p = this.m_db.Places.GetPlace(name);
                    //копируем свойства места в аргумент
                    //f.ArgumentType = String.Copy(p.PlaceTypeExpression);
                    //f.ArgumentValue = String.Copy(p.Path);
                    //заменены на 
                    f.ПодставитьМесто(p);
                }
            }

            return;
        }

        /// <summary>
        /// NT-Вывести на консоль информацию об исключении
        /// </summary>
        /// <param name="e"></param>
        private void PrintExceptionToConsole(Exception e)
        {
            //вложенное исключение выводить, если есть, вместо первого.
            //так как в процедурах сборок процедур они упаковываются в исключение механизма отражения
            if (e.InnerException != null)
                this.OperatorConsole.PrintExceptionMessage(e.InnerException);
            else
                this.OperatorConsole.PrintExceptionMessage(e);

            return;
        }


        #endregion


        #region Функции доступа к БД из сборок процедур
        //вынесены сюда, так как нельзя давать сторонним сборкам доступ к БД (не знаю пока, как получится)
        //Названия функций должны начинаться с Db...

        /// <summary>
        /// NT-Добавить Место в БД
        /// </summary>
        /// <param name="p">Заполненный объект</param>
        public void DbInsertPlace(Place p)
        {
            //Добавить объект в БД
            this.m_db.AddPlace(p);

            return;
        }

        /// <summary>
        /// NT-Добавить несколько Мест в БД
        /// </summary>
        /// <param name="places">Список заполненных Мест</param>
        public void DbInsertPlace(List<Place> places)
        {
            //Добавить объекты в БД
            this.m_db.AddPlace(places);

            return;
        }

        public void DbRemovePlace(Place p)
        {
            this.m_db.RemovePlace(p);
        }

        /// <summary>
        /// NT-Добавить Процедуру в БД
        /// </summary>
        /// <param name="p">Заполненный объект</param>
        public void DbInsertProcedure(Procedure p)
        {
            //Добавить объект в БД
            this.m_db.AddProcedure(p);

            return;
        }

        /// <summary>
        /// NT-Добавить несколько Процедур в БД
        /// </summary>
        /// <param name="procedures">Список заполненных Процедур</param>
        public void DbInsertProcedure(List<Procedure> procedures)
        {
            //Добавить объект в БД
            this.m_db.AddProcedure(procedures);

            return;
        }

        public void DbRemoveProcedure(Procedure p)
        {
            this.m_db.RemoveProcedure(p);
        }

        /// <summary>
        /// NT-Выбрать из БД Места по названию, без учета регистра символов
        /// </summary>
        /// <param name="placeTitle">Название места</param>
        /// <returns>Возвращает список мест с указанным названием</returns>
        public List<Place> DbGetPlacesByTitle(string placeTitle)
        {
            //проще всего перебрать названия мест в кеше в памяти, а не выбирать их из БД.
            //поэтому надо сделать выборку Мест из коллекции мест в БД, без учета регистра символов
            return this.m_db.Places.getByTitle(placeTitle);
        }

        /// <summary>
        /// NT-Выбрать из БД Процедуры по названию, без учета регистра символов
        /// </summary>
        /// <param name="title">Название Процедуры</param>
        /// <returns>Возвращает список Процедур с указанным названием</returns>
        public List<Procedure> DbGetProceduresByTitle(string title)
        {
            //проще всего перебрать названия процедур в кеше в памяти, а не выбирать их из БД.
            //поэтому надо сделать выборку Процедур из коллекции процедур в БД, без учета регистра символов
            return this.m_db.Procedures.getByTitle(title);
        }

        #endregion

        
        //


        ///// <summary>
        ///// Пример функции процедуры обработчика команды
        ///// </summary>
        ///// <param name="engine"></param>
        ///// <param name="cmdline"></param>
        ///// <param name="args"></param>
        ///// <returns></returns>
        //public static ProcedureResult CommandHandlerExample(Engine engine, string cmdline, FuncArgument[] args)
        //{


        //    //вернуть флаг продолжения работы
        //    return ProcedureResult.Success;
        //}















    }
}
