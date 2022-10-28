using System;
using Engine.DbSubsystem;
using Engine.LexiconSubsystem;
using Engine.LogSubsystem;
using Engine.ProcedureSubsystem;
using Engine.SettingSubsystem;
using Engine.Utility;

namespace Engine.OperatorEngine
{
    // Консоль Оператора:
    // Функции доступа к консоли из сборок процедур сейчас перенесены в класс Operator.Lexicon.DialogConsole.
    // Для вывода сообщений на консоль использовать только! объект engine.OperatorConsole.
    // так как я планирую вынести консоль совсем отдельно, то надо уже сейчас ее использование ограничить.

    /// <summary>
    /// NT - Движок исполнения Процедур.
    /// </summary>
    public class Engine : EngineSubsystem
    {

        #region *** Constants and Fields ***

        /// <summary>
        /// Строка названия Оператора для платформы Windows
        /// </summary>
        public const String ApplicationTitle = "Operator";

        /// <summary>
        /// Строка версии Оператора для платформы Windows
        /// </summary>
        public const String EngineVersionString = "1.1.0.0";

        /// <summary>
        /// Статический объект версии движка для платформы Windows 
        /// </summary>
        public static OperatorVersion EngineVersion = OperatorVersion.tryParse(EngineVersionString);

        /// <summary>
        /// Менеджер подсистемы лога
        /// </summary>
        private LogManager m_logman;

        /// <summary>
        /// Объект адаптера БД Оператора.
        /// </summary>
        private OperatorDbAdapter m_db;
        // TODO: Объект реализован частично. Исправить весь код для него.

        /// <summary>
        /// Объект консоли Оператора. Выделен чтобы упорядочить код работающий с консолью, так как
        /// он вызывается из сборок процедур, создаваемых сторонними разработчиками.
        /// </summary>
        private DialogConsole m_OperatorConsole;
        // TODO: Объект реализован частично. Исправить весь код для него.

        /// <summary>
        /// Объект настроек Оператора
        /// </summary>
        private ApplicationSettingsKeyed m_Settings;

        /// <summary>
        /// Объект менеджера исполнения Процедур
        /// </summary>
        private ProcedureExecutionManager m_PEM;

        /// <summary>
        ///  Объект Менеджера кэша Мест и Процедур Оператора
        /// </summary>
        private ElementCacheManager m_ECM;

        /// <summary>
        /// Объект семантического анализатора
        /// </summary>
        private BCSA m_BCSA;
        #endregion

        /// <summary>
        /// NT-Стандартный конструктор
        /// </summary>
        public Engine() : base(null)
        {
            // create log manager object
            this.m_logman = new LogManager(this);
            // создать объект консоли Оператора
            this.m_OperatorConsole = new DialogConsole(this);
            // create engine settings object
            this.m_Settings = new ApplicationSettingsKeyed(this);
            // create database adapter object
            this.m_db = new OperatorDbAdapter(this);
            // create execution manager
            this.m_PEM = new ProcedureExecutionManager(this);
            // create cache manager object - after DB and PEM only.
            this.m_ECM = new ElementCacheManager(this, this.m_db, this.m_PEM);
            // create semantic analyzer object
            this.m_BCSA = new BCSA(this);

            return;
        }

        #region *** Properties ***

        ///<summary>
        ///NT- Get log manager object
        /// </summary> 
        public LogManager LogManager
        {
            get { return this.m_logman; }
        }

        /// <summary>
        /// NT-Получить объект адаптера БД Оператора.
        /// </summary>
        public OperatorDbAdapter Database
        {
            get { return this.m_db; }
        }

        /// <summary>
        /// NT-Получить объект консоли Оператора. Должен быть доступен из сторонних сборок.
        /// </summary>
        public DialogConsole OperatorConsole
        {
            get { return this.m_OperatorConsole; }
        }

        /// <summary>
        /// NT-Получить объект настроек движка Оператора.
        /// </summary>
        public ApplicationSettingsKeyed EngineSettings
        {
            get { return this.m_Settings; }
        }

        /// <summary>
        ///  NT-Получить объект кеш-коллекции Процедур и Мест Оператора.
        /// </summary>
        internal ElementCacheManager ECM
        {
            get { return this.m_ECM; }
        }

        /// <summary>
        ///  NT-Получить объект Менеджера Библиотек Процедур.
        /// </summary>
        public ProcedureExecutionManager PEM
        {
            get { return this.m_PEM; }
        }

        /// <summary>
        /// NT- получить объект семантического анализатора запросов.
        /// </summary>
        internal BCSA BCSA
        {
            get { return this.m_BCSA; }
        }
        #endregion

        // Функции инициализации и завершения движка =================

        #region  *** Override this from EngineSubsystem parent class ***
        /// <summary>
        /// NR - Initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onOpen()
        {
            // this.m_OperatorConsole.PrintTextLine("Operator loading...", EnumDialogConsoleColor.Сообщение);

            // 1. check operator folder exist
            //TODO: если каталог не найден, предложить создать новый с ответами Да и Нет.
            //и вывести путь для каталога Оператора. (Путь прописан в коде, его нельзя изменить. Но можно создать все прямо в текущем каталоге приложения)
            //Если Нет, то  завершить работу приложения.
            //Если Да, то создать новый каталог Оператор
            if (FileSystemManager.isAppFolderExists() == false)
            {
                String msg1 = "Ошибка: Каталог Оператор не найден: " + FileSystemManager.getAppFolderPath();
                String msg2 = "Будет создан новый каталог Оператор с настройками по умолчанию.";
                this.m_OperatorConsole.PrintTextLine(msg1, DialogConsoleColor.Предупреждение);
                this.m_OperatorConsole.PrintTextLine(msg2, DialogConsoleColor.Предупреждение);
                FileSystemManager.CreateOperatorFolder();
            }

            // 2. open engine log session
            // this.m_logman.AddMessage(new
            // LogMessage(EnumLogMsgClass.SessionStarted, EnumLogMsgState.OK,
            // "Session opened"));
            // - это уже сделано в this.m_logman.Open();
            this.m_logman.Open();

            // 3. load engine settings
            // Если файл настроек не обнаружен, вывести сообщение об этом и
            // создать новый файл настроек с дефолтовыми значениями.
            //TODO: файл настроек управляется подсистемой настроек, и путь к нему должен создаваться там, и проверяется наличие файла  - там.
            //Но ему нужен каталог, в котором хранится файл, вот каталог надо брать из FileSystemManager и здесь отправлять его в подсистему настроек
            //как часть открытия сеанса подсистемы настроек и сеанса Движка.
            String settingsFilePath = FileSystemManager.getAppSettingsFilePath();
            if (!FileSystemManager.isAppSettingsFileExists())
            {
                String msg3 = "Файл настроек " + settingsFilePath + " не найден! Будет создан файл с настройками по умолчанию.";
                this.m_logman.AddMessage(EnumLogMsgClass.Default, EnumLogMsgState.Fail, msg3);
                this.m_OperatorConsole.PrintTextLine(msg3, DialogConsoleColor.Предупреждение);
                this.m_Settings.Reset();
                this.m_Settings.Store(settingsFilePath);
            }
            else this.m_Settings.Load(settingsFilePath);

            // 4. init database
            // заполнить кеш-коллекции процедур и мест данными из БД
            // CachedDbAdapter делает это сам

            // если новой бд нет в каталоге приложения, создаем ее.
            String dbFile = FileSystemManager.getAppDbFilePath();
            String connectionString = OperatorDbAdapter.CreateConnectionString(dbFile, false);
            if (FileSystemManager.isAppDbFileExists() == false)
            {
                // print warning about database
                String msg4 = "Файл базы данных " + dbFile + " не найден! Будет создан новый пустой файл базы данных.";
                this.m_logman.AddMessage(EnumLogMsgClass.Default, EnumLogMsgState.Fail, msg4);
                this.m_OperatorConsole.PrintTextLine(msg4, DialogConsoleColor.Предупреждение);
                // TODO: тут лучше попытаться загрузить бекап-копию БД, после
                // предупреждения о отсутствии основного файла.
                // Но эту копию надо сначала создать.

                // create new database here. Open, write, close.
                OperatorDbAdapter.CreateNewDatabase(this, dbFile);
                // но работать с пустой БД - начинать все сначала.
            }
            // else
            // open existing database and close - as test
            this.m_db.Open(connectionString);
            this.m_db.Close();
            // Использовать адаптер так, чтобы он открывал БД только на
            // время чтения или записи, а не держал ее постоянно открытой.
            // Так меньше вероятность повредить бд при глюках OS.

            // 5. Open PEM
            // TODO: дополнить код здесь полезными проверками
            this.AddMessageToConsoleAndLog("Загрузка Библиотек Процедур..", DialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.Default);
            this.m_PEM.Open();// TODO: this function not completed now.
            this.AddMessageToConsoleAndLog("Загрузка Библиотек Процедур завершена.", DialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.Default);
            this.m_OperatorConsole.PrintEmptyLine();
            // 6. Open ECM
            // TODO: дополнить код здесь полезными проверками
            this.m_ECM.Open();// TODO: this function not completed now.

            // 7. Open BCSA
            this.m_BCSA.Open();// TODO: this function not completed now.

            return;
        }

        /// <summary>
        /// NR - De-initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onClose()
        {
            // закрыть BCSA
            if (this.m_BCSA != null)
                this.m_BCSA.Close();
            // закрыть ECM
            if (this.m_ECM != null)
                this.m_ECM.Close();
            // закрыть PEM
            if (this.m_PEM != null)
                this.m_PEM.Close();
            // Закрыть БД если еще не закрыта
            if (this.m_db != null)
                this.m_db.Close();
            // close settings object
            this.m_Settings.StoreIfModified();
            // close log session - последним элементом
            this.m_logman.Close();
            this.m_logman = null;

            return;
        }
        #endregion

        #region *** Safe access to Log ***

        //TODO: эту подсистему лога надо перепроектировать - и свойства событий неправильные, и доступ к ней тоже кривой.

        /// <summary>
        /// NT-append new message object to log
        /// </summary>
        /// <param name="c">Event class code</param>
        /// <param name="s">Event state code</param>
        /// <param name="text">Event text description</param>
        protected void safeAddLogMsg(
                EnumLogMsgClass c,
                EnumLogMsgState s,
                String text)
        {
            // проверить существование движка и лога, и затем добавить сообщение в лог.
            if (OperatorEngine.Engine.isLogReady(this))
                this.LogManager.AddMessage(c, s, text);

            return;
        }

        /// <summary>
        /// NT-Write exception to engine log if available
        /// </summary>
        /// <param name="en">Engine object</param>
        /// <param name="e">Exception object</param>
        public static void LoggingException(Engine en, Exception e)
        {
            try
            {
                // get log manager
                LogManager l = en.LogManager;
                // check log ready and write exception object
                if (l.isReady)
                    l.AddExceptionMessage(e);
            }
            catch (Exception e2)
            {
                ;// add breakpoint here
            }

            return;
        }

        /// <summary>
        /// NT-Check log is available
        /// </summary>
        /// <param name="en">Engine object</param>
        /// <returns>
        ///   Function returns True if log writing is available, returns False otherwise.
        /// </returns>
        public static bool isLogReady(Engine en)
        {
            if (en == null)
                return false;
            // get log manager
            LogManager l = en.LogManager;
            if (l == null)
                return false;
            // check log ready
            return l.isReady;
        }

        /// <summary>
        /// NT-Add exception message to Log and Console.
        /// </summary>
        /// <param name="msg">Message title.</param>
        /// <param name="ex">Exception object.</param>
        public void PrintExceptionMessageToConsoleAndLog(String msg, Exception ex)
        {
            this.m_OperatorConsole.PrintExceptionMessage(msg, ex);
            this.m_logman.AddExceptionMessage(msg, ex);

            return;
        }

        /// <summary>
        /// NT-Вывести сообщение на консоль и в лог.
        /// </summary>
        /// <param name="text">Текст сообщения.</param>
        /// <param name="color">Класс сообщения Консоли.</param>
        /// <param name="cls">Класс сообщения Лога.</param>
        /// <param name="state">Состояние сообщения Лога.</param>
        public void AddMessageToConsoleAndLog(
                String text,
                DialogConsoleColor color,
                EnumLogMsgClass cls,
                EnumLogMsgState state)
        {
            this.m_OperatorConsole.PrintTextLine(text, color);
            this.m_logman.AddMessage(cls, state, text);
        }
        #endregion

        #region *** Извлечение Настроек ***

        /// <summary>
        /// NT-Извлечь значение Настройки из ФайлНастроекОператора или ТаблицаНастроекОператора, иначе вывести сообщение о ее отсутствии.
        /// </summary>
        /// <param name="setting">Ключ Настройки.</param>
        /// <param name="operation">Название выполняемой операции для использования в текстах сообщений.</param>
        /// <returns>Функция возвращает значение настройки или null.</returns>
        /// <remarks>
        /// Эта функция может использоваться в коде Процедур из Библиотек Процедур.
        /// </remarks>
        public String getSettingOrMessage(SettingKey setting, String operation)
        {
            // TODO: эта функция извлекает только первую настройку, а нужно извлекать все элементы с таким ключом.
            // Но это требует перепроектировать весь код, использующий настройки.
            String val = this.m_Settings.getValue(setting);
            string title = setting.Title;//key cached

            if (String.IsNullOrEmpty(val))
            {
                String msg = String.Format("Невозможно выполнить {0}, поскольку настройка {1} не найдена в ФайлНастроекОператора.", operation, title);
                this.AddMessageToConsoleAndLog(msg, DialogConsoleColor.Предупреждение, EnumLogMsgClass.SubsystemEvent_Settings, EnumLogMsgState.Fail);
                //then get from database
                val = this.m_ECM.getSettingFirstValue(title);
                if (String.IsNullOrEmpty(val))
                {
                    String msg2 = String.Format("Невозможно выполнить {0}, поскольку настройка {1} не найдена в ТаблицаНастроекОператора.", operation, title);
                    this.AddMessageToConsoleAndLog(msg2, DialogConsoleColor.Предупреждение, EnumLogMsgClass.SubsystemEvent_Settings, EnumLogMsgState.Fail);
                }
            }
            return val;
        }

        /// <summary>
        /// NT - Получить значение настройки из ФайлНастроекОператора или ТаблицаНастроекОператора.
        /// </summary>
        /// <param name="setting">Ключ - название настройки.</param>
        /// <returns>
        /// Функция возвращает значение настройки из ФайлНастроекОператора или ТаблицаНастроекОператора.
        /// Функция возвращает null, если поля настройки не найдено.
        /// Функция возвращает пустую строку, если значение настройки не указано.
        /// </returns>
        /// <remarks>
        /// Эта функция может использоваться в коде Процедур из Библиотек Процедур.
        /// </remarks>
        public String getSettingFromFileOrTable(SettingKey setting)
        {
            // Файл настроек всегда должен проверяться раньше, чем таблица настроек.
            String result = this.m_Settings.getValue(setting);
            if (String.IsNullOrEmpty(result) == true)
            {
                //then get from database
                result = this.m_ECM.getSettingFirstValue(setting.Title);
            }

            return result;
        }

        #endregion

        #region *** Основной цикл исполнения механизма ***

        /// <summary>
        /// NT-Основной цикл исполнения механизма
        /// </summary>
        public void CommandLoop()
        {

            // TODO: работать здесь!!! Такая свалка недоделок получилась, бардак в проекте нарастает.
            // Надо расчищать эти завалы скорее, пока я помню, что тут как и зачем.

            // выводим приветствие и описание программы
            this.m_OperatorConsole.PrintEmptyLine();
            this.m_OperatorConsole.PrintTextLine("Консоль речевого интерфейса. Версия " + Engine.EngineVersionString, DialogConsoleColor.Сообщение);
            this.m_OperatorConsole.PrintTextLine("Для завершения работы приложения введите слово выход или quit", DialogConsoleColor.Сообщение);
            this.m_OperatorConsole.PrintTextLine("Сегодня " + StringUtility.CreateLongDatetimeString(DateTime.Now), DialogConsoleColor.Сообщение);
            this.m_OperatorConsole.PrintEmptyLine();

            // 3.1 выполнить СтартоваяПроцедура.
            int StartResult = CommandStartupProcedure();
            // 3.2 вывести приглашение пользователю.
            EnumProcedureResult result = EnumProcedureResult.Unknown;
            // запускаем цикл приема запросов
            while (true)
            {
                this.m_OperatorConsole.PrintTextLine("", DialogConsoleColor.Сообщение);
                this.m_OperatorConsole.PrintTextLine("Введите ваш запрос:", DialogConsoleColor.Сообщение);
                // 3.3 ожидать ввод запроса от пользователя.
                String query = this.m_OperatorConsole.ReadLine();
                // если был нажат CTRL+C, query может быть null
                // пока я не знаю, что делать в этом случае, просто перезапущу цикл
                // приема команды
                // и при пустой строке тоже просто перезапустить цикл приема команды

                // Операторы: return закрывает Оператор, а continue - переводит на
                // следующий цикл приема команды

                if (query == null)
                    continue;
                // триммим из запроса пробелы всякие лишние сразу же
                // если строка пустая, начинаем новый цикл приема команды
                query = query.Trim();
                // query теперь может оказаться пустой строкой
                if (String.IsNullOrEmpty(query))
                    continue;
                // 3.4 TODO: событие С10 Поступил новый запрос от пользователя.
                // TODO: добавить событие С10 в лог вместе с текстом запроса.

                // TODO: Отложить это все до готовности остальных частей проекта: БД, настроек, остального.

                // Тут упакуем все исполнение запроса и эту функцию, потому
                // что она нужна еще и в CommandStartupProcedure(), где она должна
                // исполнить либо Процедуру, если указан путь к ней, либо
                // прокрутить выборку и исполнение Процедуры для запроса, если это запрос.

                // а семантическую обработку запроса - упустили! Я ее потом приделаю, когда весь Оператор заработает.
                // EnumProcedureResult result = Lexicon.BCSA.ProcessQuery(this, query);

                result = DoCommandExecution(query);

                // Описать результат исполнения Процедуры.
                this.describeProcedureResult(result);

                // завершение работы Оператор
                if (result == EnumProcedureResult.Exit)
                    break;

                // -3.6 если С11 Исполнение запроса успешно завершено.
                // -3.7 если С12, то переход на следующую итерацию цикла.
                // -3.8 если С13, то завершение работы Оператор.
                // -3.9 если С14, то переход на следующую итерацию цикла.

                // -3.10 конец цикла обработки запроса
            }// end while

            // -3.11 выполнить ФинишнаяПроцедура.
            // switch(result)
            // {
            // case ExitAndHybernate:
            // case ExitAndLogoff:
            // case ExitAndReload:
            // case ExitAndShutdown:
            // case ExitAndSleep:
            // //тут не запускаем финишную процедуру, так как можем не успеть до завершения приложения.
            // break;
            // case CancelledByUser:
            // case Error:
            // case Exit:
            // case Success:
            // case Unknown:
            // case WrongArguments:
            // default:
            // //тут надо запустить финишную процедуру
            // break;
            // }
            int FinishResult = CommandFinishProcedure(result);

            return;
        }

        /// <summary>
        /// NT- Запустить исполнение запроса и вернуть результат
        /// </summary>
        /// <param name="query">Строка запроса или путь к Процедуре.</param>
        /// <returns>Функция возвращает код результата исполнения Процедуры.</returns>
        private EnumProcedureResult DoCommandExecution(String query)
        {
            // TODO: Отложить это все до готовности остальных частей проекта: БД, настроек, остального.

            // TODO: этот код должен перехватывать все исключения и возвращать только EnumProcedureResult.Error при любой ошибке.

            // Создать объект запроса пользователя для использования в алгоритме и Процедурах.
            UserQuery userQuery = new UserQuery(query);
            // если это строка запроса, то запустить цикл выбора и исполнения Процедур.
            // если это путь к Процедуре, то запустить эту выбранную Процедуру только.

            // вернуть - яхз пока что возвращать. Наверно, Код результата процедуры?
            EnumProcedureResult exitCode = EnumProcedureResult.Success;
            // 3.5 исполнение запроса
            // 3.5.1 Выполнить ПредОбработкаЗапроса.
            exitCode = DoPreProcessing(userQuery);// готово
            if (exitCode != EnumProcedureResult.Exit)
            {
                // 3.5.2 Выполнить ИсполнениеЗапроса.
                // если запрос это путь к Процедуре, то:
                if (RegexManager.IsAssemblyCodePath(userQuery.Query) == true)
                {
                    // выполнить Процедуру без аргументов, по ее пути.
                    // потом выполнить пост-обработку, если она нужна.
                    exitCode = DoSimpleProcedureExecution(userQuery);// готово
                }
                else
                {
                    // если запрос - англоязычный, то перенаправить его в Терминал
                    if (BCSA.IsNotRussianFirst(userQuery.Query) == true)
                    {
                        exitCode = DoCommandEnglishTerminal(userQuery);// готово
                                                                       // TODO: всегда возвращает EnumProcedureResult.Success ?
                    }
                    else
                    {
                        // иначе - запустить цикл выборки Процедур для Запроса и исполнить Процедуру.
                        exitCode = DoProcedureLoopExecution(userQuery);// TODO: работать здесь!!!
                    }
                }
                // 3.5.3 Выполнить ПостОбработкаЗапроса.
                exitCode = DoPostProcessing(exitCode);// готово
                // 3.6 если С11 Исполнение запроса успешно завершено.
                // 3.7 если С12, то переход на следующую итерацию цикла.
                // 3.8 если С13, то завершение работы Оператор.
                // 3.9 если С14, то переход на следующую итерацию цикла.
            }
            // else - DoPreProcessing() returns Exit - нужно завершить работу приложения в вызывающем коде.
            return exitCode;
        }

        /// <summary>
        /// NT-Пре-процессинг введенного пользователем запроса.
        /// </summary>
        /// <param name="userQuery">Объект введенного пользователем запроса.</param>
        /// <returns>Функция возвращает код результата исполнения Процедуры, выбранной как пред-обработка поступившего запроса.</returns>
        private EnumProcedureResult DoPreProcessing(UserQuery userQuery)
        {
            // Тут если текст запроса = тексту одной из встроенных команд, то
            // заменить текст запроса на текст соответствующей Процедуры или команды из ФайлНастроекОператора.

            // 1. Проверяем команду Выход
            if (LexiconSubsystem.Dialogs.isExitAppCommand(userQuery.Query) == true)
                return EnumProcedureResult.Exit;
            // если будут еще встроенные команды, то для них надо извлечь из настроек текст запроса и заменить его в userQuery объекте.
            // - и эта замена должна быть записана в Лог (TODO: выяснить, делается ли это уже автоматически?).

            // возвращаем код продолжения разбора запроса.
            return EnumProcedureResult.Success;
        }

        /// <summary>
        /// NT-Выполнить пост-обработку результата исполнения Процедуры.
        /// </summary>
        /// <param name="code">Код результата исполнения предыдущей Процедуры.</param>
        /// <returns>Функция возвращает код результата исполнения Процедуры, выбранной как пост-обработка результата предыдущей Процедуры.</returns>
        private EnumProcedureResult DoPostProcessing(EnumProcedureResult code)
        {
            // Превратить exitCode в путь Процедуры или текст Запроса
            String newQuery = null;
            String codename = Enum.GetName(typeof(EnumProcedureResult), code);

            switch (code)
            {
                // вызываем вспомогательную функцию, которая и настройку извлекает, и сообщение об отсутствии настройки выводит.
                case EnumProcedureResult.ExitAndHybernate:
                    newQuery = getSettingOrMessage(SettingKey.CmdHybernate, codename);
                    break;
                case EnumProcedureResult.ExitAndLogoff:
                    newQuery = getSettingOrMessage(SettingKey.CmdLogoff, codename);
                    break;
                case EnumProcedureResult.ExitAndReload:
                    newQuery = getSettingOrMessage(SettingKey.CmdReload, codename);
                    break;
                case EnumProcedureResult.ExitAndShutdown:
                    newQuery = getSettingOrMessage(SettingKey.CmdShutdown, codename);
                    break;
                case EnumProcedureResult.ExitAndSleep:
                    newQuery = getSettingOrMessage(SettingKey.CmdSleep, codename);
                    break;
                case EnumProcedureResult.CancelledByUser:
                case EnumProcedureResult.Error:
                case EnumProcedureResult.Exit:
                case EnumProcedureResult.Success:
                case EnumProcedureResult.Unknown:
                case EnumProcedureResult.WrongArguments:
                default:
                    // ничего не делаем.
                    break;
            }
            // run query execution if query has been assigned.
            if (String.IsNullOrEmpty(newQuery) == false)
                return DoCommandExecution(newQuery);
            else return code;
        }

        /// <summary>
        /// NT-Исполнить запрос через ЦиклПеребораПроцедур.
        /// </summary>
        /// <param name="userQuery">Текущий текст запроса.</param>
        /// <returns>Функция возвращает код результата исполнения Процедуры.</returns>
        private EnumProcedureResult DoProcedureLoopExecution(UserQuery userQuery)
        {
            // запустить цикл выборки Процедур для Запроса и исполнить Процедуру.
            // TODO: образец: public EnumProcedureResult DoQuery(String query)

            EnumProcedureResult result = EnumProcedureResult.Success;
            String regex = null;

            try
            {
                // для каждой процедуры из списка процедур из кеша элементов:
                foreach (Procedure p in this.m_ECM.Procedures.Items)
                {
                    // собрать нормальный регекс для процедуры
                    // TODO: optimization - можно же это сделать после загрузки регекса
                    // из БД как часть процесса распаковки данных, записав в объект
                    // Процедуры как служебное поле.
                    // а не при каждом исполнении команды от пользователя.
                    regex = MakeNormalRegex(p);
                    // выполнить регекс и, заодно, определить, является ли процедура пригодной для исполнения
                    ArgumentCollection args = RegexManager.ExtractArgumentsFromCommand(userQuery.Query, regex);
                    if (args != null)
                    {
                        // Тут запускаем процедуру. Она должна теперь проверить свои аргументы и все условия,
                        // и если они не подходят, то завершиться с флагом ProcedureResult.WrongArguments.
                        result = DoProcedureExecute(userQuery, regex, p, args);
                        if (result != EnumProcedureResult.WrongArguments)
                            return result;
                    }
                }
                // Тут состояние "Не удалось подобрать процедуру для исполнения запроса"
                // Вынесем его в функцию-обработчик, чтобы модифицировать обработку
                // этого события.
                EventCommandNotExecuted();
                result = EnumProcedureResult.Success;
            }
            catch (Exception e)
            {
                this.PrintExceptionMessageToConsoleAndLog("Ошибка", e);
                result = EnumProcedureResult.Error;
            }

            return result;
        }

        /// <summary>
        /// NT- Execute Procedure
        /// </summary>
        /// <param name="userQuery">User query object</param>
        /// <param name="regex">Procedure regex string</param>
        /// <param name="p">Procedure object</param>
        /// <param name="args">Procedure argument collection</param>
        /// <returns>Функция возвращает код результата исполнения Процедуры.</returns>
        private EnumProcedureResult DoProcedureExecute(
                UserQuery userQuery,
                String regex,
                Procedure p,
                ArgumentCollection args)
        {
            // и еще нужно этим аргументам сопоставить типы мест хотя бы
            TryAssignPlaces(args);

            // надо определить, путь исполнения это путь к процедуре или к
            // приложению.
            // TODO:оптимизация: сделать это при загрузке Процедуры из БД и
            // сохранить в служебном поле Процедуры
            Boolean isAssemblyCodePath = RegexManager.IsAssemblyCodePath(p.Path);
            if (isAssemblyCodePath == false)
            {
                // если к приложению, его надо запустить и вернуть стандартное
                // значение для продолжения работы.
                return DoShellExecute(p, args); // return RunShellExecute(p, args);

            }
            else
            {
                // если к процедуре, надо приготовить аргументы, найти сборку,
                // вызвать функцию, передать ей аргументы и вернуть результат.
                return DoLocalAssembly(userQuery, p, args);// return RunLocalAssembly(command, p, args);
            }
        }

        /// <summary>
        /// NT- Запустить Процедуру из локальной БиблиотекаПроцедурОператора.
        /// </summary>
        /// <param name="userQuery">Команда пользователя..</param>
        /// <param name="p">Объект процедуры.</param>
        /// <param name="args">Коллекция аргументов.</param>
        /// <returns> Функция возвращает результат выполнения процедуры.</returns>
        private EnumProcedureResult DoLocalAssembly(
                UserQuery userQuery,
                Procedure p,
                ArgumentCollection args)
        {
            // источник: private EnumProcedureResult RunLocalAssembly(..)

            EnumProcedureResult result = EnumProcedureResult.Success;
            try
            {
                // получить имена частей пути. в порядке: сборка, класс, функция,
                // аргументы по порядку следования если они есть
                String[] names = RegexManager.ParseAssemblyCodePath(p.Path);
                // тут аргументы уже должны быть заполнены значениями и типами
                // и местами и готовы к выполнению.
                result = this.m_PEM.invokeProcedure(p, names, userQuery, this, args);
                // Это выбрасывает исключения процесса запуска и исполнения Процедуры.
            }
            catch (Exception e)
            {
                // вызов исполнения не удался.
                // пока выведем сообщение об исключении в консоль.
                // TODO: вот надо завести в механизме статическую переменную
                // отладки, включаемую через отдельную процедуру, и по ней выводить
                // на экран эти отладочные данные.
                // TODO: надо вывести тут сообщение об исключении в общий лог.
                // если не выводить сообщение об ошибке, то непонятно, почему
                // команда не исполняется.
                // например, когда выключился спящий режим, команда спать просто
                // выводила сообщение я не умею.

                this.PrintExceptionMessageToConsoleAndLog("Ошибка: ", e);

                // вернуть флаг ошибки
                // TODO: Определить, что тут должна возвращать текущая функция, если во время исполнения Процедуры произошла ошибка.
                // Ошибка - это ошибка, а не несоответствие Запроса и Процедуры. А тут - наобум назначено возвращаемое значение.
                result = EnumProcedureResult.WrongArguments;
            }

            // возвращаем то что вернет процедура
            return result;
        }

        /// <summary>
        /// NT-Открыть пустой Терминал по пути LoneTerminal из ФайлНастроекОператора.
        /// </summary>
        /// <returns>
        /// Функция возвращает EnumProcedureResult.Success при успехе.
        /// Функция возвращает EnumProcedureResult.Error, если при запуске Терминала произошла ошибка.
        /// </returns>
        /// <exception cref="Exception">
        /// Не найдена команда запуска Терминала из настройки " + EnumSettingKey.LoneTerminal.getTitle()
        /// or
        /// Не найден путь к рабочему каталогу Терминала из настройки " + EnumSettingKey.DefaultWorkingDirectory.getTitle()
        /// </exception>
        public EnumProcedureResult StartAloneTerminal()
        {
            // Еще, для программ нужен рабочий каталог. Для wget подойдет каталог Downloads, для других программ - другие варианты желательны.
            // Но возможно указать только один, и в нем все последствия запускаемых программ будут оставаться.
            // Указать его в ФайлНастроекОператора - надо добавить поле для РабочийКаталог.
            // - Рабочий каталог - текущий каталог - часть контекста текущей работы пользователя.
            // Если запаковать работу пользователя в Проект, то текущий рабочий каталог - это будет элемент контекста проекта.
            // - а как сейчас - все приложения будут в одном этом каталоге исполняться? Там будет свалка.
            // - в итоге, надо пока что избегать этой проблемы с текущим каталогом, и собирать материал по ней.

            // вызвать PEM.ExecuteApplicationSimple() or PEM.ExecuteApplication()

            EnumProcedureResult result = EnumProcedureResult.Success;
            try
            {
                // 1. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение командной строки терминала
                String cmdterm = this.getSettingFromFileOrTable(SettingKey.LoneTerminal);
                if (String.IsNullOrEmpty(cmdterm))
                    throw new Exception("Не найдена команда запуска Терминала из настройки " + SettingKey.LoneTerminal.Title);
                // 2. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение рабочего каталога терминала
                String workDirectory = this.getSettingFromFileOrTable(SettingKey.DefaultWorkingDirectory);
                if (String.IsNullOrEmpty(workDirectory))
                    throw new Exception("Не найден путь к рабочему каталогу Терминала из настройки " + SettingKey.DefaultWorkingDirectory.Title);
                // 3. TODO: разделить командную строку терминала на приложение и аргументы в классе RegexManager.

                // 4. пока разделить нечем - вызвать PEM.ExecuteApplicationSimple()
                this.m_PEM.ExecuteApplicationSimple(cmdterm, workDirectory);
            }
            catch (Exception e)
            {
                this.PrintExceptionMessageToConsoleAndLog("Ошибка", e);
                result = EnumProcedureResult.Error;
            }
            return result;
        }

        /// <summary>
        /// NT-Открыть Терминал и перенаправить в него текущий текст запроса.
        /// </summary>
        /// <param name="userQuery">Текущий текст запроса</param>
        /// <returns>
        /// Функция возвращает EnumProcedureResult.Success при успехе.
        /// Функция возвращает EnumProcedureResult.Error, если при запуске Терминала произошла ошибка. 
        /// </returns>
        /// <exception cref="Exception">
        /// Не найдена команда запуска Терминала из настройки " + EnumSettingKey.ForCommandTerminal.getTitle()
        /// or
        /// Не найден путь к рабочему каталогу Терминала из настройки " + EnumSettingKey.DefaultWorkingDirectory.getTitle()
        /// </exception>
        private EnumProcedureResult DoCommandEnglishTerminal(UserQuery userQuery)
        {
            // образец: private EnumProcedureResult ExecuteWithTerminal(String query)
            // Состояние: код есть, тестовый прототип написан, но терминал не запускается - чего-то не хватает.
            // Еще, для программ нужен рабочий каталог. Для wget подойдет каталог Downloads, для других программ - другие варианты желательны.
            // Но возможно указать только один, и в нем все последствия запускаемых программ будут оставаться.
            // Указать его в ФайлНастроекОператора - надо добавить поле для РабочийКаталог.
            // - Рабочий каталог - текущий каталог - часть контекста текущей работы пользователя.
            // Если запаковать работу пользователя в Проект, то текущий рабочий каталог - это будет элемент контекста проекта.
            // - а как сейчас - все приложения будут в одном этом каталоге исполняться? Там будет свалка.
            // - в итоге, надо пока что избегать этой проблемы с текущим каталогом, и собирать материал по ней.

            // вызвать PEM.ExecuteApplicationSimple() or PEM.ExecuteApplication()

            EnumProcedureResult result = EnumProcedureResult.Success;
            try
            {
                // 1. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение командной строки терминала
                // TODO: EnumSettingKey.ForProcedureTerminal не используется сейчас, вместо него EnumSettingKey.ShellExecuteCommand.
                // - Надо проверить этот факт и решить, что делать с неиспользуемой настройкой.
                String cmdterm = this.getSettingFromFileOrTable(SettingKey.ForCommandTerminal);
                if (String.IsNullOrEmpty(cmdterm))
                    throw new Exception("Не найдена команда запуска Терминала из настройки " + SettingKey.ForCommandTerminal.Title);
                // 2. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение рабочего каталога терминала
                String workDirectory = this.getSettingFromFileOrTable(SettingKey.DefaultWorkingDirectory);
                if (String.IsNullOrEmpty(workDirectory))
                    throw new Exception("Не найден путь к рабочему каталогу Терминала из настройки " + SettingKey.DefaultWorkingDirectory.Title);
                // 3. TODO: разделить командную строку терминала на приложение и аргументы в классе RegexManager.

                // 4. пока разделить нечем - вызвать PEM.ExecuteApplicationSimple()
                String cmdline = cmdterm.Trim() + " " + userQuery.Query;
                this.m_PEM.ExecuteApplicationSimple(cmdline, workDirectory);
            }
            catch (Exception e)
            {
                this.PrintExceptionMessageToConsoleAndLog("Ошибка", e);
                result = EnumProcedureResult.Error;
            }
            return result;
        }


        /// <summary>
        /// NT-Исполнить ShellExecute по пути ShellExecuteCommand из ФайлНастроекОператора.
        /// </summary>
        /// <param name="arg">URI-путь к запускаемому объекту. Пример: file:///home/jsmith/Documents/Путь%20с%20пробелами.txt</param>
        /// <returns>
        /// Функция возвращает EnumProcedureResult.Success при успехе.
        /// Функция возвращает EnumProcedureResult.Error, если при запуске Терминала произошла ошибка. 
        /// </returns>
        /// <exception cref="Exception">
        /// Не найдена команда запуска Терминала из настройки " + EnumSettingKey.ShellExecuteCommand.getTitle()
        /// or
        /// Не найден путь к рабочему каталогу Терминала из настройки " + EnumSettingKey.DefaultWorkingDirectory.getTitle()
        /// </exception>
        public EnumProcedureResult StartShellExecute(String arg)
        {
            EnumProcedureResult result = EnumProcedureResult.Success;
            try
            {
                // 1. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение командной строки терминала
                String cmdterm = this.getSettingFromFileOrTable(SettingKey.ShellExecuteCommand);
                if (String.IsNullOrEmpty(cmdterm))
                    throw new Exception("Не найдена команда запуска Терминала из настройки " + SettingKey.ShellExecuteCommand.Title);
                // 2. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение рабочего каталога терминала
                String workDirectory = this.getSettingFromFileOrTable(SettingKey.DefaultWorkingDirectory);
                if (String.IsNullOrEmpty(workDirectory))
                    throw new Exception("Не найден путь к рабочему каталогу Терминала из настройки " + SettingKey.DefaultWorkingDirectory.Title);
                // 3. TODO: разделить командную строку терминала на приложение и аргументы в классе RegexManager.

                // 4. пока разделить нечем - вызвать PEM.ExecuteApplicationSimple()
                this.m_PEM.ExecuteApplicationSimple(cmdterm + " " + arg, workDirectory);
            }
            catch (Exception e)
            {
                this.PrintExceptionMessageToConsoleAndLog("Ошибка", e);
                result = EnumProcedureResult.Error;
            }
            return result;
        }

        /// <summary>
        /// NT-Исполнить команду по пути ForCommandTerminal из ФайлНастроекОператора.
        /// </summary>
        /// <param name="arg">URI-путь к запускаемому объекту. Пример: file:///home/jsmith/Documents/Путь%20с%20пробелами.txt</param>
        /// <returns>
        /// Функция возвращает EnumProcedureResult.Success при успехе.
        /// Функция возвращает EnumProcedureResult.Error, если при запуске Терминала произошла ошибка.
        /// </returns>
        /// <exception cref="Exception">
        /// Не найдена команда запуска Терминала из настройки " + EnumSettingKey.ForCommandTerminal.getTitle()
        /// or
        /// Не найден путь к рабочему каталогу Терминала из настройки " + EnumSettingKey.DefaultWorkingDirectory.getTitle()
        /// </exception>
        public EnumProcedureResult StartCommandTerminalExecute(String arg)
        {
            EnumProcedureResult result = EnumProcedureResult.Success;
            try
            {
                // 1. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение командной строки терминала
                String cmdterm = this.getSettingFromFileOrTable(SettingKey.ForCommandTerminal);
                if (String.IsNullOrEmpty(cmdterm))
                    throw new Exception("Не найдена команда запуска Терминала из настройки " + SettingKey.ForCommandTerminal.Title);
                // 2. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение рабочего каталога терминала
                String workDirectory = this.getSettingFromFileOrTable(SettingKey.DefaultWorkingDirectory);
                if (String.IsNullOrEmpty(workDirectory))
                    throw new Exception("Не найден путь к рабочему каталогу Терминала из настройки " + SettingKey.DefaultWorkingDirectory.Title);
                // 3. TODO: разделить командную строку терминала на приложение и аргументы в классе RegexManager.

                // 4. пока разделить нечем - вызвать PEM.ExecuteApplicationSimple()
                this.m_PEM.ExecuteApplicationSimple(cmdterm + " " + arg, workDirectory);
            }
            catch (Exception e)
            {
                this.PrintExceptionMessageToConsoleAndLog("Ошибка", e);
                result = EnumProcedureResult.Error;
            }
            return result;
        }

        /// <summary>
        /// NT-Запустить команду через механизм ShellExecute.
        /// </summary>
        /// <param name="p">Объект Процедуры</param>
        /// <param name="args">Коллекция аргументов.</param>
        /// <returns>Функция возвращает результат выполнения процедуры.</returns>
        /// <exception cref="Exception">
        /// Не найдена команда запуска из настройки " + SettingKey.ShellExecuteCommand.Title
        /// or
        /// Не найден путь к рабочему каталогу Терминала из настройки " + SettingKey.DefaultWorkingDirectory.Title
        /// </exception>
        private EnumProcedureResult DoShellExecute(
                Procedure p,
                ArgumentCollection args)
        {
            // вызвать PEM.ExecuteApplicationSimple() or PEM.ExecuteApplication()

            EnumProcedureResult result = EnumProcedureResult.Success;
            try
            {
                // 1. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение командной строки ShellExecute
                String cmdterm = this.getSettingFromFileOrTable(SettingKey.ShellExecuteCommand);
                if (String.IsNullOrEmpty(cmdterm))
                    throw new Exception("Не найдена команда запуска из настройки " + SettingKey.ShellExecuteCommand.Title);
                // 2. извлечь из ФайлНастроекОператора или ТаблицаНастроекОператора значение рабочего каталога терминала
                String workDirectory = this.getSettingFromFileOrTable(SettingKey.DefaultWorkingDirectory);
                if (String.IsNullOrEmpty(workDirectory))
                    throw new Exception("Не найден путь к рабочему каталогу Терминала из настройки " + SettingKey.DefaultWorkingDirectory.Title);
                // 3. вставить аргументы в командную строку приложения
                // TODO: непонятно тут - нужен ли путь Терминала для Процедуры тут, или вообщен не нужно ничего добавлять?
                // пока что ничего не будем добавлять.
                String cmdline = RegexManager.ConvertApplicationCommandString(p.Path, args);

                // 4. пока разделить нечем - вызвать PEM.ExecuteApplicationSimple()
                // String cmdline = cmdterm.Trim() + " " + cmdQuery; - ничего не будем добавлять в командную строку от Команды.
                this.m_PEM.ExecuteApplicationSimple(cmdline, workDirectory);
            }
            catch (Exception e)
            {
                // вызов исполнения не удался.
                // пока выведем сообщение об исключении в консоль.
                // TODO: вот надо завести в механизме статическую переменную
                // отладки, включаемую через отдельную процедуру/команду, и по ней
                // выводить на экран эти отладочные данные.
                // TODO: надо вывести тут сообщение об исключении в общий лог.
                // если не выводить сообщение об ошибке, то непонятно, почему
                // команда не исполняется.
                // например, когда выключился спящий режим, команда спать просто
                // выводила сообщение я не умею.
                // this.m_OperatorConsole.PrintExceptionMessage(e);

                this.PrintExceptionMessageToConsoleAndLog("Ошибка", e);

                // TODO: Флаг, что процедура не годится, выдается для исключения из функции
                // RegexManager.ConvertApplicationCommandString(..), она могла бы возвращать null как значение ошибки.
                // а то сейчас при любой ошибке Процедура считается непригодной и тихо пропускается.
                // А функция играет роль фильтра для отсева неподходящих пар регекс-командная строка из Команды.
                result = EnumProcedureResult.WrongArguments;// флаг что процедура не годится
            }

            return result;
        }

        /// <summary>
        ///  NT-Исполнить Процедуру без аргументов.
        /// </summary>
        /// <param name="userQuery">Путь к Процедуре.</param>
        /// <returns>Функция возвращает код результата исполнения Процедуры.</returns>
        /// <exception cref="Exception">Процедура не найдена в Коллекции Процедур Оператора.</exception>
        private EnumProcedureResult DoSimpleProcedureExecution(UserQuery userQuery)
        {
            EnumProcedureResult result = EnumProcedureResult.Success;

            try
            {
                // 1. извлечь из userQuery путь к Процедуре.
                String procedurePath = userQuery.Query;
                // 2. найти Процедуру с таким путем в ECM.
                Procedure p = this.m_ECM.Procedures.getByPath(procedurePath);
                if (p == null)
                    throw new Exception(String.Format("Процедура \"{0}\" не найдена в Коллекции Процедур Оператора.", procedurePath));
                // 3. Запустить Процедуру на исполнение - лучше из самого объекта Процедуры?
                // - без аргументов.
                // PEM.invokeProcedure(Procedure, names[], query, engine, args);
                // вот ее придется обернуть в более короткий вызов и подготовку всех ресурсов.
                // 1. Procedure - объект процедуры, не используется пока.
                // 2. String[] names - массив частей пути Процедуры:
                // [0] - library dll title
                // [1] - class title within library
                // [2] - function title within class
                // [3..n] - argument names, not used here
                // - этот массив формируется в функции RegexManager.ParseAssemblyCodePath(...);
                String[] names = RegexManager.ParseAssemblyCodePath(procedurePath);
                // 3. String query text - чего туда подавать?
                // - тут проблема, если Процедура проверяет текст запроса - запроса же нет.
                // - А) передавать что есть - путь к Процедуре.
                // - Б) передавать текст описания из объекта Процедуры.
                // 4. Engine object ref
                // 5. ArgumentCollection args - формируется в функции RegexManager.ExtractArgumentsFromCommand(query, regex);
                // - если регекс подходит, а аргументов нет, возвращается пустая коллекция аргументов.
                ArgumentCollection args = new ArgumentCollection();

                // TODO: вот тут можно предложить запрашивать необходимые Процедуре аргументы - у пользователя, через диалог.
                // Раз он сам такой путь Процедуры ввел в качестве команды, так пусть и аргументы укажет, если они есть.
                // - для этого надо где-то взять список аргументов с именами и описаниями. Где?
                // - в регексе Команды; либо в Пути Процедуры должны быть указаны имена аргументов. Но толку от их названий мало будет.
                // Но это - потом опробовать фичу, а сейчас - тупо запустить Процедуру без аргументов и не учитывая регекс.

                result = this.m_PEM.invokeProcedure(p, names, userQuery, this, args);
            }
            catch (Exception e)
            {
                this.PrintExceptionMessageToConsoleAndLog("Ошибка", e);
                result = EnumProcedureResult.Error;
            }

            // 4. Вернуть код результата Процедуры.
            return result;
        }

        /// <summary>
        /// NT- выполнить стартовую процедуру
        /// </summary>
        /// <returns>Вернуть код результата для правильного перехода в вызывающем алгоритме.</returns>
        private int CommandStartupProcedure()
        {
            // - TODO: С30 Событие начала стартапа.
            // Если не удалось найти пути для процедуры стартапа ни в файле настроек, ни в БД, то ничего не писать про процедуру стартапа.
            // - вывести сообщение о начале процедуры startUp.
            // если в ФайлНастроекОператора флаг ignore_startup = true, то вывести надпись о игнорировании стартапа.
            // и вывести в лог сообщение об игнорировании стартапа.
            // Иначе вывести надпись о начале процедуры стартапа.
            // и вывести в лог сообщение о начале процедуры стартапа.

            // читаем флаг из ФайлНастроекОператора
            Boolean? ignoreStartup = this.m_Settings.getValueAsBoolean(SettingKey.IgnoreStartup);
            // Вместо путей получаем объекты настроек, чтобы выводить на консоль информацию о запускаемых командах.
            SettingItem[] settings = null;
            // если флага нет, или он сброшен, то стартап запускать
            if ((ignoreStartup == null) || (ignoreStartup.HasValue == false) || (ignoreStartup.Value == false))
            {
                // читаем настройку из ФайлНастроекОператора
                // - если в ФайлНастроекОператор поле cmd_startup существует и не пустое, то:
                // - строка содержимого поля запускается на исполнение как команда либо как путь Процедуры.
                // - Если строка - путь процедуры, то запускается Процедура.
                // Иначе - считать строку командой и передать в МеханизмИсполненияКоманд Оператор.
                settings = this.m_Settings.getItems(SettingKey.CmdStartup, false);
                //TODO: а почему процедуры выбираются либо из файла настроек, либо из БД? А нельзя и оттуда и оттуда? Надо подумать, нужно ли это.
                if ((settings == null) || (settings.Length == 0))
                {
                    // - иначе:
                    // - выбрать из таблицы БД ТаблицаНастроекОператор
                    // записи по ключу cmd_startup - может быть несколько значений с одинаковым ключом.
                    // - строка содержимого поля value запускается на исполнение как команда либо как путь Процедуры.
                    // - Если строка - путь процедуры, то запускается Процедура.
                    // - todo: нужна функция для запуска Процедуры без аргументов по ее пути.
                    // Иначе - считать строку командой и передать в МеханизмИсполненияКоманд Оператор.

                    // читаем настройку из ТаблицаНастроекОператора. Она теперь в ECM выведена.
                    // Вместо путей получаем объекты настроек, чтобы выводить на консоль информацию о запускаемых командах.
                    settings = this.m_ECM.Settings.getItems(SettingKey.CmdStartup.Title, false);
                }
                // если массив не null, то исполняем команды из массива
                // а тут как выявить не-пустые значения массива?
                if ((settings == null) || (settings.Length == 0))
                {
                    // Вывести на консоль и в лог сообщение об отсутствии команд для стартапа.
                    this.AddMessageToConsoleAndLog("Нет заданий для процедуры старта Оператор.", DialogConsoleColor.Сообщение, EnumLogMsgClass.StartupExecution, EnumLogMsgState.OK);
                }
                else
                {
                    // Вывести на консоль и в лог сообщение о начале исполнения команд для стартапа.
                    this.AddMessageToConsoleAndLog("Исполнение процедуры старта Оператор начато:", DialogConsoleColor.Сообщение, EnumLogMsgClass.StartupExecution, EnumLogMsgState.OK);

                    foreach (SettingItem ca in settings)
                    {
                        // check null or empty
                        if (ca == null)
                            continue;
                        // get query text
                        String caQuery = ca.Path.Trim();
                        if (String.IsNullOrEmpty(caQuery))
                            continue;
                        // Вывести на консоль и в лог название и описание команды финиша. Предварительно вывести в консоль пустую строку как разделитель.
                        String msgcmd = ca.toSingleDescriptionString();
                        this.AddMessageToConsoleAndLog(msgcmd, DialogConsoleColor.Сообщение, EnumLogMsgClass.StartupExecution, EnumLogMsgState.OK);

                        // TODO: тут запустить Процедуру или Запрос на исполнение в обычном порядке.
                        // TODO: результат Процедуры нужно обработать, вдруг он вызовет перезагрузку компьютера посреди исполнения следующего запроса в списке?
                        EnumProcedureResult result = DoCommandExecution(caQuery);
                    }

                    // - вывести сообщение о завершении процедуры startUp.
                    this.AddMessageToConsoleAndLog("Исполнение процедуры старта Оператор завершено.", DialogConsoleColor.Сообщение, EnumLogMsgClass.StartupExecution, EnumLogMsgState.OK);
                }
                // - todo: пост-обработка для КодЗавершенияПроцедуры здесь не выполняется?
                // - TODO: С31 Событие завершения стартапа.

            }
            else
            {
                // вывести на консоль сообщение о игнорировании стартапа.
                // и вывести в лог сообщение об игнорировании стартапа.
                this.AddMessageToConsoleAndLog("Исполнение процедуры старта Оператор запрещено конфигурацией.", DialogConsoleColor.Сообщение, EnumLogMsgClass.StartupExecution, EnumLogMsgState.OK);
            }
            return 0;
        }

        /// <summary>
        /// NT-Выполнить процедуру завершения Оператор.
        /// </summary>
        /// <param name="pr">Код результата после завершения цикла исполнения команд..</param>
        /// <returns>Вернуть код результата для правильного перехода в вызывающем алгоритме.</returns>
        private int CommandFinishProcedure(EnumProcedureResult pr)
        {
            // - TODO: С35 Событие начала финиша.
            // Если не удалось найти пути для процедуры финиша ни в файле настроек, ни в БД, то ничего не писать про процедуру финиша.
            // - вывести сообщение о начале процедуры финиша.
            // если в ФайлНастроекОператора флаг ignore_startup = true, то вывести надпись о игнорировании финиша.
            // и вывести в лог сообщение об игнорировании финиша.
            // Иначе вывести надпись о начале процедуры финиша.
            // и вывести в лог сообщение о начале процедуры финиша.

            // читаем флаг из ФайлНастроекОператора
            Boolean? ignoreStartup = this.m_Settings.getValueAsBoolean(SettingKey.IgnoreStartup);
            // Вместо путей получаем объекты настроек, чтобы выводить на консоль информацию о запускаемых командах.
            SettingItem[]
            settings = null;
            // если флага нет, или он сброшен, то стартап запускать
            if ((ignoreStartup == null) || (ignoreStartup.HasValue == false) || (ignoreStartup.Value == false))
            {
                // читаем настройку из ФайлНастроекОператора
                // - если в ФайлНастроекОператор поле cmd_finish существует и не пустое, то:
                // - строка содержимого поля запускается на исполнение как команда либо как путь Процедуры.
                // - Если строка - путь процедуры, то запускается Процедура.
                // Иначе - считать строку командой и передать в МеханизмИсполненияКоманд Оператор.
                settings = this.m_Settings.getItems(SettingKey.CmdFinish, false);
                //TODO: а почему процедуры выбираются либо из файла настроек, либо из БД? А нельзя и оттуда и оттуда? Надо подумать, нужно ли это.
                if ((settings == null) || (settings.Length == 0))
                {
                    // - иначе:
                    // - выбрать из таблицы БД ТаблицаНастроекОператор
                    // записи по ключу cmd_finish - может быть несколько значений с одинаковым ключом.
                    // - строка содержимого поля value запускается на исполнение как команда либо как путь Процедуры.
                    // - Если строка - путь процедуры, то запускается Процедура.
                    // - todo: нужна функция для запуска Процедуры без аргументов по ее пути.
                    // Иначе - считать строку командой и передать в МеханизмИсполненияКоманд Оператор.

                    // читаем настройку из ТаблицаНастроекОператора. Она выведена в ECM.
                    // Вместо путей получаем объекты настроек, чтобы выводить на консоль информацию о запускаемых командах.
                    settings = this.m_ECM.Settings.getItems(SettingKey.CmdFinish.Title, false);
                }
                // если массив не null, то исполняем команды из массива
                // а тут как выявить не-пустые значения массива?
                if (settings == null)
                {
                    // Вывести на консоль и в лог сообщение об отсутствии команд для финиша.
                    this.AddMessageToConsoleAndLog("Исполнение процедуры завершения Оператор невозможно: задания не определены.", DialogConsoleColor.Сообщение, EnumLogMsgClass.FinishExecution, EnumLogMsgState.OK);
                }
                else
                {
                    // Вывести на консоль и в лог сообщение о начале исполнения команд для финиша.
                    this.AddMessageToConsoleAndLog("Исполнение процедуры завершения Оператор начато:", DialogConsoleColor.Сообщение, EnumLogMsgClass.FinishExecution, EnumLogMsgState.OK);

                    foreach (SettingItem ca in settings)
                    {
                        // check null or empty
                        if (ca == null)
                            continue;
                        // get query text
                        String caQuery = ca.Path.Trim();
                        if (String.IsNullOrEmpty(caQuery))
                            continue;
                        // Вывести на консоль и в лог название и описание команды финиша. Предварительно вывести в консоль пустую строку как разделитель.
                        String msgcmd = ca.toSingleDescriptionString();
                        this.AddMessageToConsoleAndLog(msgcmd, DialogConsoleColor.Сообщение, EnumLogMsgClass.FinishExecution, EnumLogMsgState.OK);

                        // TODO: тут запустить Процедуру или Запрос на исполнение в обычном порядке.
                        // TODO: результат Процедуры нужно обработать, вдруг он вызовет перезагрузку компьютера посреди исполнения следующего запроса в списке?
                        EnumProcedureResult result = DoCommandExecution(caQuery);
                    }
                }
                // - todo: пост-обработка для КодЗавершенияПроцедуры здесь не выполняется?
                // - TODO: С36 Событие завершения финиша.
                // - вывести сообщение о завершении процедуры finish.
                this.AddMessageToConsoleAndLog("Исполнение процедуры завершения Оператор завершено.", DialogConsoleColor.Сообщение, EnumLogMsgClass.FinishExecution, EnumLogMsgState.OK);

            }
            else
            {
                // вывести на консоль и в лог сообщение о игнорировании финиша.
                this.AddMessageToConsoleAndLog("Исполнение процедуры старта Оператор запрещено конфигурацией.", DialogConsoleColor.Сообщение, EnumLogMsgClass.FinishExecution, EnumLogMsgState.OK);
            }
            // return result code
            return 0;
        }

        // /// <summary>
        // /// NR-Обработчик события "Поступила новая команда"
        // /// </summary>
        // /// <param name="query">Текст запроса команды</param>
        // /// <returns></returns>
        // private EnumProcedureResult EventCommandArrived(String query) throws Exception
        // {
        // // сейчас тупо исполним весь запрос целиком
        // // result = DoQuery(query);
        //
        // EnumProcedureResult result = Lexicon.BCSA.ProcessQuery(this, query);
        //
        // return result;
        // }

        /// <summary>
        /// NT-вывести сообщение-подтверждение результата процедуры
        /// Играть звуковой сигнал, если результат - ошибка.
        /// Подтверждение не выводится, если завершение процедуры - успешное.
        /// </summary>
        /// <param name="result">Результат исполнения процедуры</param>
        private void describeProcedureResult(EnumProcedureResult result)
        {
            String msg = null;
            Boolean ErrorAndBeep = false;
            switch (result)
            {
                case EnumProcedureResult.CancelledByUser:
                    msg = "Процедура прервана пользователем.";
                    break;
                case EnumProcedureResult.Error:
                case EnumProcedureResult.Unknown:
                    msg = "Ошибка при исполнении процедуры.";
                    ErrorAndBeep = true;
                    break;
                case EnumProcedureResult.Exit:
                    msg = "Завершение программы...";
                    break;
                case EnumProcedureResult.ExitAndHybernate:
                case EnumProcedureResult.ExitAndSleep:
                    msg = "Переход в спящий режим...";
                    break;
                case EnumProcedureResult.ExitAndLogoff:
                    msg = "Завершение сеанса пользователя...";
                    break;
                case EnumProcedureResult.ExitAndReload:
                    msg = "Перезагрузка компьютера...";
                    break;
                case EnumProcedureResult.ExitAndShutdown:
                    msg = "Выключение компьютера...";
                    break;
                case EnumProcedureResult.WrongArguments:
                    msg = "Ошибка: неправильные аргументы.";
                    ErrorAndBeep = true;
                    break;
                case EnumProcedureResult.Success:
                    msg = "Процедура выполнена успешно.";
                    break;
                default:
                    break;
            }
            // выбрать цвет сообщения о результате процедуры
            // подать звуковой сигнал при ошибке
            DialogConsoleColor color;
            if (ErrorAndBeep == true)
            {
                this.m_OperatorConsole.Beep();
                color = DialogConsoleColor.Предупреждение;
            }
            else color = DialogConsoleColor.Сообщение;
            // выдать сообщение о результате процедуры
            // если курсор не в начале строки, начать сообщение с новой строки.
            this.m_OperatorConsole.SureConsoleCursorStart();
            this.m_OperatorConsole.PrintTextLine(msg, color);

            return;
        }

        /// <summary>
        /// NT-Обработать событие "Не удалось подобрать процедуру для исполнения запроса" 
        /// </summary>
        /// <returns></returns>
        private void EventCommandNotExecuted()
        {
            // TODO: добавить эту функцию -ивент в документацию по работе движка.
            // выводим сообщение что для запроса не удалось подобрать процедуру
            this.m_OperatorConsole.PrintTextLine("Я такое не умею", DialogConsoleColor.Сообщение);

            // вообще же тут можно выполнять другую обработку, наверно...

            return;
        }

        /// <summary>
        /// NT-Собрать нормальный регекс для процедуры
        /// </summary>
        /// <param name="p">Объект Процедуры</param>
        /// <returns>Функция возвращает Нормальный регекс для указанной Процедуры.</returns>
        /// <exception cref="Exception">Procedure has invalid regex string</exception>
        private String MakeNormalRegex(Procedure p)
        {
            //TODO: почему эта функция не в RegexManager ?
            String result = null;
            String procedureRegex = p.Regex;
            // получить тип регекса
            EnumRegexType rt = RegexManager.determineRegexType(procedureRegex);
            // конвертировать регекс в пригодный для исполнения
            if (rt == EnumRegexType.NormalRegex)
            {
                result = String.Copy(procedureRegex);
            }
            else if (rt == EnumRegexType.SimpleString)
            {
                // Тут Простой регекс превращается в Нормальный регекс, русские
                // названия аргументов заменяются на arg_#.
                result = RegexManager.ConvertSimpleToRegex2(procedureRegex);
            }
            else throw new Exception(String.Format("Procedure has invalid regex string: {0} in {1}", procedureRegex, p.Title));

            return result;
        }

        /// <summary>
        /// NT-Сопоставить данные аргументов и места из коллекции мест, насколько это возможно.
        /// </summary>
        /// <param name="args">Коллекция аргументов.</param>
        /// <returns></returns>
        private void TryAssignPlaces(ArgumentCollection args)
        {
            //если у аргумента название есть в словаре мест, то скопировать в аргумент значение этого места
            // пока без проверки типов и всего такого, так как это должна бы делать процедура.
            foreach (FuncArgument f in args.Arguments)
            {
                String name = f.ArgumentValue;
                if (this.m_ECM.Places.ContainsPlaceBySynonim(name))
                {
                    // извлечем место
                    Place p = this.m_ECM.Places.GetPlace(name);
                    // копируем свойства места в аргумент
                    f.ПодставитьМесто(p);
                }
            }

            return;
        }
        #endregion
    }
}
