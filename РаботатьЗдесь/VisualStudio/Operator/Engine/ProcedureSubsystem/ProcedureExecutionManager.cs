using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ProcedureSubsystem
{
    /// <summary>
    /// NR-Менеджер подсистемы исполнения Процедур
    /// </summary>
    public class ProcedureExecutionManager : Engine.OperatorEngine.EngineSubsystem
    {
        //TODO: портировать код с явы

        //        // Тут хранить объекты менеджеров LibraryManager.
        //        // А пути к сборкам процедур хранить в этих объектах.
        //        // Тогда и объекты в памяти остаются - и данные инициализации в памяти
        //        // остаются на время хранения объекта.
        //        // - пока непонятно, что там должно храниться в памяти, и как что
        //        // выгружается сборщиком мусора.


        /// <summary>
        /// NR - Конструктор
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        public ProcedureExecutionManager(OperatorEngine.Engine engine) : base(engine)
        {
            //TODO: Add code here
        }


        #region  *** Override this from EngineSubsystem parent class ***
        /// <summary>
        /// NR - Initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onOpen()
        {
            throw new Exception("Function must be overridden");//TODO: Add code here
        }

        /// <summary>
        /// NR - De-initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onClose()
        {
            throw new Exception("Function must be overridden");//TODO: Add code here
        }
        #endregion

        //        /**
        //         * Словарь (Сборка,Путь) для путей к библиотекам.
        //         */
        //        protected HashMap<String, LibraryManagerBase> m_Libraries;

        //        /**
        //         * NT-Constructor
        //         * 
        //         * @param engine
        //         *            Engine backreference.
        //         */
        //        public ProcedureExecutionManager(Engine engine)
        //        {
        //            this.m_Engine = engine;
        //            this.m_Libraries = null;
        //        }

        //        /**
        //         * NT-Initialize Execution manager
        //         * 
        //         * @throws Exception
        //         *             Exception on errors.
        //         */
        //        public void Open() throws Exception
        //        {
        //        // Заполнить HashMap
        //        this.m_Libraries = loadLibraries();// TODO: как обрабатывать исключения в Open() всех менеджеров?
        //        // init all libraries
        //        this.initLibraries();

        //        return;
        //    }

        //    /**
        //     * NT-Close execution manager
        //     */
        //    public void Close()
        //    {
        //        // free all libraries
        //        this.сloseLibraries();
        //        // clear dictionary
        //        this.m_Libraries.clear();

        //        return;
        //    }

        //    /**
        //     * NT-Получить объект менеджера библиотеки по названию библиотеки
        //     * 
        //     * @param key
        //     *            Название библиотеки.
        //     * @return Функция возвращает объект менеджера библиотеки.
        //     */
        //    private LibraryManagerBase getManager(String key)
        //    {
        //        return this.m_Libraries.get(key);
        //    }

        //    /**
        //     * NT-initialize all libraries
        //     */
        //    private void initLibraries()
        //    {
        //        Set<Entry<String, LibraryManagerBase>> values = this.m_Libraries.entrySet();
        //        for (Entry<String, LibraryManagerBase> en : values)
        //        {
        //            try
        //            {
        //                en.getValue().Init();
        //            }
        //            catch (Exception ex)
        //            {
        //                // тут вывести сообщение о ошибке инициализации библиотеки
        //                // на консоль и в лог
        //                String msg = String.format("Error on init library \"%s\" (path=\"%s\")", en.getKey(), en.getValue().m_LibraryPath);
        //                this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
        //            }
        //        }

        //        return;
        //    }

        //    /**
        //     * NT-deinitialize all libraries
        //     */
        //    private void сloseLibraries()
        //    {
        //        Set<Entry<String, LibraryManagerBase>> values = this.m_Libraries.entrySet();
        //        for (Entry<String, LibraryManagerBase> en : values)
        //        {
        //            try
        //            {
        //                LibraryManagerBase lmb = en.getValue();
        //                if (lmb.get_Initialized() == true)
        //                    lmb.Exit();
        //            }
        //            catch (Exception ex)
        //            {
        //                // тут вывести сообщение о ошибке инициализации библиотеки
        //                // на консоль и в лог
        //                String msg = String.format("Error on exit library \"%s\" (path=\"%s\")", en.getKey(), en.getValue().m_LibraryPath);
        //                this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
        //            }
        //        }

        //        return;
        //    }

        //    /**
        //     * NT-Load LibraryManager object from each of founded Procedure Library JAR file.
        //     * 
        //     * @return Function returns HashMap[title, manager]
        //     * @throws Exception
        //     *             Error on loading.
        //     */
        //    private HashMap<String, LibraryManagerBase> loadLibraries() throws Exception
        //    {
        //        // создать словарь название библиотеки - объект менеджера.
        //        HashMap<String, LibraryManagerBase> result = new HashMap<String, LibraryManagerBase>();
        //        // получить путь к корневой папке хранилища библиотек
        //        String libraryFolder = FileSystemManager.getAssembliesFolderPath();
        //    File libFolder = new File(libraryFolder);
        //    // собираем файлы только в указанном каталоге, но не в подкаталогах.
        //    File[] files = FileSystemManager.getDirectoryFiles(libFolder, new String[] {
        //                ".jar", ".JAR" }, true);
        //        // найденные файлы проверить и обработать.
        //        // здесь исключение не должно прерывать общий процесс загрузки библиотек,
        //        // но должно выводиться на консоль и в лог.
        //        for (File f : files)
        //        {
        //            try
        //            {
        //                // извлечь имя файла без расширения и путь к файлу
        //                String filetitle = f.getName();
        //    filetitle = Utility.getFilenameWithoutExtension(filetitle);
        //                String path = f.getAbsolutePath();
        //    // get library manager object from class loader
        //    LibraryManagerBase manager = LibraryManagerBase.loadLibraryManager(this.m_Engine, filetitle, path);
        //                // поместить объект в словарь только если файл является правильным файлом библиотеки
        //                if (manager != null)
        //                    result.put(filetitle, manager);
        //            }
        //            catch (Exception e)
        //{
        //    String title = "Исключение при загрузке библиотеки Процедур " + f.getAbsolutePath();
        //    this.m_Engine.PrintExceptionMessageToConsoleAndLog(title, e);
        //}
        //        }

        //        return result;
        //    }

        //    // ==============================================
        //    /**
        //     * NT-Получить все объекты Places
        //     * 
        //     * @return Функция возвращает список объектов Places
        //     * @throws Exception
        //     *             Ошибка
        //     */
        //    public LinkedList<Place> GetAllPlaces() throws Exception
        //{
        //    LinkedList<Place> result = new LinkedList<Place>();
        //Set<Entry<String, LibraryManagerBase>> values = this.m_Libraries.entrySet();

        //for (Entry<String, LibraryManagerBase> en : values)
        //{
        //    try
        //    {
        //        LibraryManagerBase lmb = en.getValue();
        //        if (lmb.get_Initialized() == true)
        //        {
        //            Place[] par = lmb.getLibraryPlaces();
        //            // add to result list
        //            for (Place p : par)
        //                result.add(p);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // тут вывести сообщение о ошибке библиотеки на консоль и в лог
        //        String msg = String.format("Error on GetAllPlaces() for library \"%s\" (path=\"%s\")", en.getKey(), en.getValue().m_LibraryPath);
        //        this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
        //    }
        //}

        //return result;
        //    }

        //    /**
        //     * NT-Получить все объекты Процедур
        //     * 
        //     * @return Функция возвращает список объектов Процедур.
        //     * @throws Exception
        //     *             Ошибка .
        //     */
        //    public LinkedList<Procedure> GetAllProcedures() throws Exception
        //{
        //    LinkedList<Procedure> result = new LinkedList<Procedure>();
        //Set<Entry<String, LibraryManagerBase>> values = this.m_Libraries.entrySet();

        //for (Entry<String, LibraryManagerBase> en : values)
        //{
        //    try
        //    {
        //        LibraryManagerBase lmb = en.getValue();
        //        if (lmb.get_Initialized() == true)
        //        {
        //            Procedure[] par = lmb.getLibraryProcedures();
        //            // add to result list
        //            for (Procedure p : par)
        //                result.add(p);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // тут вывести сообщение о ошибке библиотеки на консоль и в лог
        //        String msg = String.format("Error on GetAllProcedures() for library \"%s\" (path=\"%s\")", en.getKey(), en.getValue().m_LibraryPath);
        //        this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
        //    }
        //}

        //return result;
        //    }

        //    /**
        //     * NT-Получить все объекты Настроек
        //     * 
        //     * @return Функция возвращает список объектов Настроек.
        //     * @throws Exception
        //     *             Ошибка .
        //     */
        //    public LinkedList<SettingItem> GetAllSettings() throws Exception
        //{
        //    LinkedList<SettingItem> result = new LinkedList<SettingItem>();
        //Set<Entry<String, LibraryManagerBase>> values = this.m_Libraries.entrySet();

        //for (Entry<String, LibraryManagerBase> en : values)
        //{
        //    try
        //    {
        //        LibraryManagerBase lmb = en.getValue();
        //        if (lmb.get_Initialized() == true)
        //        {
        //            SettingItem[] par = lmb.getLibrarySettings();
        //            // add to result list
        //            for (SettingItem p : par)
        //                result.add(p);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // тут вывести сообщение о ошибке библиотеки на консоль и в лог
        //        String msg = String.format("Error on GetAllSettings() for library \"%s\" (path=\"%s\")", en.getKey(), en.getValue().m_LibraryPath);
        //        this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
        //    }
        //}

        //return result;
        //    }

        //    /**
        //     * NT- Запустить процедуру на исполнение
        //     * 
        //     * @param p
        //     *            Объект Процедуры.
        //     * @param names
        //     *            Массив частей пути Процедуры.
        //     * @param command
        //     *            Объект запроса пользователя.
        //     * @param engine
        //     *            Ссылка на Движок Оператора.
        //     * @param args
        //     *            Мвассив аргументов для Процедуры.
        //     * @return Функция возвращает код результата исполнения Процедуры.
        //     * @throws Exception
        //     *             Error on execution.
        //     */
        //    public EnumProcedureResult invokeProcedure(
        //            Procedure p,
        //            String[] names,
        //            UserQuery command,
        //            Engine engine,
        //            ArgumentCollection args) throws Exception
        //{
        //    // get manager reference
        //    LibraryManagerBase manager = this.getManager(names [0]);
        //    // get library file path
        //    String jarFilePath = manager.get_LibraryPath();
        //        // invoke procedure and return result
        //        if (manager.get_Initialized() == false)
        //            throw new Exception("Library manager not initialized for " + jarFilePath);
        //        else return LibraryManagerBase.invokeProcedure(p, names, jarFilePath, engine, manager, command, args);
        //    }

        //    /**
        //     * NT-Запустить приложение и немедленно выйти из функции.
        //     * @param cmdline Командная строка для исполнения
        //     * @param workDirectory Путь к рабочему каталогу, должен существовать.
        //     * @return Возвращает значение 0.
        //     * @throws Exception Исключение при запуске процесса.
        //     */
        //    public int ExecuteApplicationSimple(String cmdline, String workDirectory) throws Exception
        //{
        //    File wd = new File(workDirectory);
        //@SuppressWarnings("unused")
        //        Process p = Runtime.getRuntime().exec(cmdline, null, wd);

        //return 0;
        //    }

        //    /**
        //     * NT-Запустить приложение и немедленно выйти из функции.
        //     * 
        //     * @param app
        //     *            Application path
        //     * @param args
        //     *            Argument string
        //     * @param workDirectory
        //     *            Application working directory, must exists.
        //     * @param logging Print environment variables to Operator console and log.
        //     * @return Возвращает значение 0.
        //     * @throws Exception
        //     *             Исключение при запуске процесса.
        //     */
        //    public int ExecuteApplication(
        //            String app,
        //            String args,
        //            String workDirectory,
        //            Boolean logging
        //            ) throws Exception
        //{
        //    //ProcessBuilder принимает именно так: список из пути приложения и аргументов.
        //    //Есть способ: Runtime.getRuntime().exec(cmd, null, File(workDirectory))
        //    //Он тоже работает, но переопределить свойства Process не дает. 
        //    //Сейчас используем ProcessBuilder, чтобы потом проще было переделывать.

        //    List<String> command = new ArrayList<String>();
        //command.add(app);
        //command.add(args);
        ////create process builder
        //ProcessBuilder builder = new ProcessBuilder(command);

        ////get environment variables
        //if (logging == true)
        //{
        //    //get map of variables key-value
        //    Map<String, String> environ = builder.environment();
        //    this.m_Engine.AddMessageToConsoleAndLog("------------", EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
        //    this.m_Engine.AddMessageToConsoleAndLog("Дамп ProcessBuilder Environment variables: title -> value", EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
        //    this.m_Engine.AddMessageToConsoleAndLog("------------", EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
        //    for (Map.Entry<String, String> ent : environ.entrySet())
        //    {
        //        String s = String.format("\"%s\" -> \"%s\"", ent.getKey(), ent.getValue());
        //        this.m_Engine.AddMessageToConsoleAndLog(s, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
        //    }
        //    this.m_Engine.AddMessageToConsoleAndLog("------------", EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
        //}
        //// set directory
        //builder.directory(new File(workDirectory));
        //// startup
        //@SuppressWarnings("unused")
        //        final Process process = builder.start();

        //// InputStream is = process.getInputStream();
        //// InputStreamReader isr = new InputStreamReader(is);
        //// BufferedReader br = new BufferedReader(isr);
        //// String line;
        //// while ((line = br.readLine()) != null)
        //// {
        //// System.out.println(line);
        //// }
        //// System.out.println("Program terminated!");

        //return 0;
        //    }


        //    // *** Java Reflection debug functions *** TODO: перенести эти функции в Utility или другое подходящее место

        //    /**
        //     * RT-Вывести на экран информацию о классе.
        //     * 
        //     * @param c
        //     *            Объект класса.
        //     */
        //    @SuppressWarnings("unused")
        //    private static void printClassInfo(Class<?> c)
        //{
        //    if (c == null)
        //    {
        //        System.out.println("PrintClassInfo: Class = null");
        //        return;
        //    }
        //    System.out.println("Class information:");
        //    System.out.println("getCanonicalName(): " + c.getCanonicalName());
        //    System.out.println("getName():" + c.getName());
        //    System.out.println("getSimpleName():" + c.getSimpleName());
        //    System.out.println("getTypeName():" + c.getTypeName());
        //    System.out.println("toGenericString():" + c.toGenericString());
        //    System.out.println("toString():" + c.toString());
        //    printAnnotations(c.getAnnotations());
        //    printMethodsInfo(c.getMethods());
        //    printPackageInfo(c.getPackage());

        //    return;
        //}

        ///**
        // * RT-Вывести на экран информацию о методах класса.
        // * 
        // * @param mar
        // *            Массив объектов методов класса.
        // */
        //private static void printMethodsInfo(Method[] mar)
        //{
        //    System.out.println("Methods information:");
        //    if (mar.length == 0)
        //        System.out.println("   No methods.");
        //        else
        //    {
        //        for (Method m : mar)
        //            {
        //    System.out.println("getName(): " + m.getName());
        //    System.out.println("toString(): " + m.toString());
        //    printAnnotations(m.getAnnotations());
        //    System.out.println("");
        //}
        //        }

        //        return;
        //    }

        //    /**
        //     * RT-Вывести на экран информацию о пакете.
        //     * 
        //     * @param p
        //     *            Объект пакета.
        //     */
        //    private static void printPackageInfo(Package p)
        //{
        //    if (p == null)
        //    {
        //        System.out.println("PrintPackageInfo: Package = null");
        //        return;
        //    }
        //    System.out.println("Package information:");
        //    System.out.println("getName(): " + p.getName());
        //    System.out.println("getImplementationTitle(): " + p.getImplementationTitle());
        //    System.out.println("getImplementationVendor(): " + p.getImplementationVendor());
        //    System.out.println("getImplementationVersion(): " + p.getImplementationVersion());
        //    System.out.println("getSpecificationTitle(): " + p.getSpecificationTitle());
        //    System.out.println("getSpecificationVendor(): " + p.getSpecificationVendor());
        //    System.out.println("getSpecificationVersion(): " + p.getSpecificationVersion());
        //    System.out.println("toString(): " + p.toString());
        //    printAnnotations(p.getAnnotations());
        //    System.out.println("");
        //    return;
        //}

        ///**
        // * RT-Вывести на экран массив аннотаций
        // * 
        // * @param annotations
        // *            Массив аннотаций элемента
        // */
        //private static void printAnnotations(Annotation[] annotations)
        //{
        //    System.out.println("Annotation information:");
        //    if (annotations.length == 0)
        //        System.out.println("   No annotations.");
        //        else
        //    {
        //        for (Annotation a : annotations)
        //                System.out.println(a.toString());
        //        }
        //        System.out.println("");
        //return;
        //    }


    }
}
