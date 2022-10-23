using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.LexiconSubsystem;
using Engine.LogSubsystem;
using Engine.OperatorEngine;
using Engine.ProcedureSubsystem;
using Engine.SettingSubsystem;

namespace Engine.ProcedureSubsystem
{
    /// <summary>
    /// NR-Менеджер подсистемы исполнения Процедур
    /// </summary>
    public class ProcedureExecutionManager : Engine.OperatorEngine.EngineSubsystem
    {
        //TODO: портировать код с явы

        // Тут хранить объекты менеджеров LibraryManager.
        // А пути к сборкам процедур хранить в этих объектах.
        // Тогда и объекты в памяти остаются - и данные инициализации в памяти
        // остаются на время хранения объекта.
        // - пока непонятно, что там должно храниться в памяти, и как что
        // выгружается сборщиком мусора.

        //TODO: подсистема реализована криво. Функции открытия-закрытия сеанса недостаточно проработаны.

        /// <summary>
        /// Словарь (Сборка,Путь) для путей к библиотекам.
        /// </summary>
        protected Dictionary<String, LibraryManagerBase> m_Libraries;


        /// <summary>
        /// NT - Конструктор
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        public ProcedureExecutionManager(OperatorEngine.Engine engine) : base(engine)
        {
            //TODO: Add code here
            this.m_Libraries = null;
        }


        #region  *** Override this from EngineSubsystem parent class ***

        /// <summary>
        /// NT-Initialize Execution manager
        /// </summary>
        protected override void onOpen()
        {
            // Заполнить словарь библиотек
            this.m_Libraries = loadLibraries();
            // TODO: как обрабатывать исключения в Open() всех менеджеров?
            
            // init all libraries
            this.initLibraries();

            return;
        }

        /// <summary>
        /// NT-Close execution manager
        /// </summary>
        protected override void onClose()
        {
            // free all libraries
            this.сloseLibraries();
            // clear dictionary
            this.m_Libraries.Clear();

            return;
        }
        #endregion

/// <summary>
/// NT-Получить объект менеджера библиотеки по названию библиотеки
/// </summary>
/// <param name="titleKey">Название библиотеки.</param>
/// <returns>
/// Функция возвращает объект менеджера библиотеки.
/// Если название в словаре не найдено, функция выбрасывает исключение.
/// </returns>
    private LibraryManagerBase getManager(String titleKey)
    {
        return this.m_Libraries[titleKey];
    }

        /// <summary>
        /// NT-initialize all libraries
        /// </summary>
        private void initLibraries()
        {
            foreach (KeyValuePair<String, LibraryManagerBase> en in this.m_Libraries)
            {
                //init each LibraryManager in dictionary
                //if exception throwed, log and suppress, continue for all items.
                try
                {

                    en.Value.Open();
                }
                catch (Exception ex)
                {
                    // тут вывести сообщение о ошибке инициализации библиотеки
                    // на консоль и в лог
                    String msg = String.Format("Error on init library \"{0}\" (path=\"{1}\")", en.Key, en.Value.LibraryPath);
                    this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
                }
            }

            return;
        }

        /// <summary>
        /// NT-deinitialize all libraries
        /// </summary>
        private void сloseLibraries()
    {
            foreach (KeyValuePair<String, LibraryManagerBase> en in this.m_Libraries)
            {
            try
            {
                LibraryManagerBase lmb = en.Value;
                if (lmb.isReady == true)
                    lmb.Close();
            }
            catch (Exception ex)
            {
                // тут вывести сообщение о ошибке инициализации библиотеки
                // на консоль и в лог
                String msg = String.Format("Error on exit library \"{0}\" (path=\"{1}\")", en.Key, en.Value.LibraryPath);
                this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
            }
        }

        return;
    }

        /**
         * NT-Load LibraryManager object from each of founded Procedure Library JAR file.
         * 
         * @return Function returns HashMap[title, manager]
         * @throws Exception
         *             Error on loading.
         */
        private Dictionary<String, LibraryManagerBase> loadLibraries()
        {
            // создать словарь название библиотеки - объект менеджера.
            Dictionary<String, LibraryManagerBase> result = new Dictionary<String, LibraryManagerBase>();
            // получить путь к корневой папке хранилища библиотек
            String libraryFolder = FileSystemManager.getAssembliesFolderPath();
            File libFolder = new File(libraryFolder);
            // собираем файлы только в указанном каталоге, но не в подкаталогах.
            File[] files = FileSystemManager.getDirectoryFiles(libFolder, new String[] {
                        ".jar", ".JAR" }, true);
            // найденные файлы проверить и обработать.
            // здесь исключение не должно прерывать общий процесс загрузки библиотек,
            // но должно выводиться на консоль и в лог.
            for (File f : files)
            {
                try
                {
                    // извлечь имя файла без расширения и путь к файлу
                    String filetitle = f.getName();
                    filetitle = Utility.getFilenameWithoutExtension(filetitle);
                    String path = f.getAbsolutePath();
                    // get library manager object from class loader
                    LibraryManagerBase manager = LibraryManagerBase.loadLibraryManager(this.m_Engine, filetitle, path);
                    // поместить объект в словарь только если файл является правильным файлом библиотеки
                    if (manager != null)
                        result.put(filetitle, manager);
                }
                catch (Exception e)
                {
                    String title = "Исключение при загрузке библиотеки Процедур " + f.getAbsolutePath();
                    this.m_Engine.PrintExceptionMessageToConsoleAndLog(title, e);
                }
            }

            return result;
        }

        // ==============================================
        

        /// <summary>
        /// NT-Получить все объекты Places
        /// </summary>
        /// <returns>Функция возвращает список объектов Places</returns>
        public List<Place> GetAllPlaces()
{
    List<Place> result = new List<Place>();

foreach (KeyValuePair<String, LibraryManagerBase> en in this.m_Libraries)
{
    try
    {
        LibraryManagerBase lmb = en.Value;
        if (lmb.isReady == true)
        {
            Place[] par = lmb.getLibraryPlaces();//TODO: переделать на List для упрощения
                                                 // add to result list
                        foreach (Place p in par)
                result.Add(p);
        }
    }
    catch (Exception ex)
    {
        // тут вывести сообщение о ошибке библиотеки на консоль и в лог
        String msg = String.Format("Error on GetAllPlaces() for library \"{0}\" (path=\"{1}\")", en.Key, en.Value.LibraryPath);
        this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
    }
}

return result;
            }


/// <summary>
/// NT-Получить все объекты Процедур
/// </summary>
/// <returns>Функция возвращает список объектов Процедур.</returns>
            public List<Procedure> GetAllProcedures() 
{
    List<Procedure> result = new List<Procedure>();


foreach (KeyValuePair<String, LibraryManagerBase> en in this.m_Libraries)
{
    try
    {
        LibraryManagerBase lmb = en.Value;
        if (lmb.isReady == true)
        {
            Procedure[] par = lmb.getLibraryProcedures();//TODO: переделать на List для упрощения
                                                         // add to result list
                        foreach (Procedure p in par)
                result.Add(p);
        }
    }
    catch (Exception ex)
    {
        // тут вывести сообщение о ошибке библиотеки на консоль и в лог
        String msg = String.Format("Error on GetAllProcedures() for library \"{0}\" (path=\"{1}\")", en.Key, en.Value.LibraryPath);
        this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
    }
}

return result;
            }


/// <summary>
/// NT-Получить все объекты Настроек
/// </summary>
/// <returns>Функция возвращает список объектов Настроек.</returns>
            public List<SettingItem> GetAllSettings() 
{
    List<SettingItem> result = new List<SettingItem>();

foreach (KeyValuePair<String, LibraryManagerBase> en in this.m_Libraries)
{
    try
    {
        LibraryManagerBase lmb = en.Value;
        if (lmb.isReady == true)
        {
            SettingItem[] par = lmb.getLibrarySettings();//TODO: переделать на List для упрощения
            // add to result list
            foreach (SettingItem p in par)
                result.Add(p);
        }
    }
    catch (Exception ex)
    {
        // тут вывести сообщение о ошибке библиотеки на консоль и в лог
        String msg = String.Format("Error on GetAllSettings() for library \"{0}\" (path=\"{1}\")", en.Key, en.Value.LibraryPath);
        this.m_Engine.PrintExceptionMessageToConsoleAndLog(msg, ex);
    }
}

return result;
            }

            /**
             * NT- Запустить процедуру на исполнение
             * 
             * @param p
             *            Объект Процедуры.
             * @param names
             *            Массив частей пути Процедуры.
             * @param command
             *            Объект запроса пользователя.
             * @param engine
             *            Ссылка на Движок Оператора.
             * @param args
             *            Мвассив аргументов для Процедуры.
             * @return Функция возвращает код результата исполнения Процедуры.
             * @throws Exception
             *             Error on execution.
             */
            public EnumProcedureResult invokeProcedure(
                    Procedure p,
                    String[] names,
                    UserQuery command,
                    Engine.OperatorEngine.Engine engine,
                    ArgumentCollection args)
{
    // get manager reference
    LibraryManagerBase manager = this.getManager(names[0]);
    // get library file path
    String jarFilePath = manager.LibraryPath;
                // invoke procedure and return result
                if (manager.isReady == false)
                    throw new Exception("Library manager not initialized for " + jarFilePath);
                else return LibraryManagerBase.invokeProcedure(p, names, jarFilePath, engine, manager, command, args);
            }

            /**
             * NT-Запустить приложение и немедленно выйти из функции.
             * @param cmdline Командная строка для исполнения
             * @param workDirectory Путь к рабочему каталогу, должен существовать.
             * @return Возвращает значение 0.
             * @throws Exception Исключение при запуске процесса.
             */
            public int ExecuteApplicationSimple(String cmdline, String workDirectory) 
{
    File wd = new File(workDirectory);

                Process p = Runtime.getRuntime().exec(cmdline, null, wd);

return 0;
            }

            /**
             * NT-Запустить приложение и немедленно выйти из функции.
             * 
             * @param app
             *            Application path
             * @param args
             *            Argument string
             * @param workDirectory
             *            Application working directory, must exists.
             * @param logging Print environment variables to Operator console and log.
             * @return Возвращает значение 0.
             * @throws Exception
             *             Исключение при запуске процесса.
             */
            public int ExecuteApplication(
                    String app,
                    String args,
                    String workDirectory,
                    Boolean logging
                    ) 
{
    //ProcessBuilder принимает именно так: список из пути приложения и аргументов.
    //Есть способ: Runtime.getRuntime().exec(cmd, null, File(workDirectory))
    //Он тоже работает, но переопределить свойства Process не дает. 
    //Сейчас используем ProcessBuilder, чтобы потом проще было переделывать.

    List<String> command = new ArrayList<String>();
command.add(app);
command.add(args);
//create process builder
ProcessBuilder builder = new ProcessBuilder(command);

//get environment variables
if (logging == true)
{
    //get map of variables key-value
    Map<String, String> environ = builder.environment();
    this.m_Engine.AddMessageToConsoleAndLog("------------", DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
    this.m_Engine.AddMessageToConsoleAndLog("Дамп ProcessBuilder Environment variables: title -> value", DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
    this.m_Engine.AddMessageToConsoleAndLog("------------", DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
    for (Map.Entry<String, String> ent : environ.entrySet())
    {
        String s = String.Format("\"{0}\" -> \"{1}\"", ent.getKey(), ent.getValue());
        this.m_Engine.AddMessageToConsoleAndLog(s, DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
    }
    this.m_Engine.AddMessageToConsoleAndLog("------------", DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
}
// set directory
builder.directory(new File(workDirectory));
// startup
@SuppressWarnings("unused")
                final Process process = builder.start();

// InputStream is = process.getInputStream();
// InputStreamReader isr = new InputStreamReader(is);
// BufferedReader br = new BufferedReader(isr);
// String line;
// while ((line = br.readLine()) != null)
// {
// System.out.println(line);
// }
// System.out.println("Program terminated!");

return 0;
            }


        // *** Java Reflection debug functions *** TODO: перенести эти функции в Utility или другое подходящее место

        //            /**
        //             * RT-Вывести на экран информацию о классе.
        //             * 
        //             * @param c
        //             *            Объект класса.
        //             */
        //            @SuppressWarnings("unused")
        //            private static void printClassInfo(Class<?> c)
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
        //                else
        //    {
        //        for (Method m : mar)
        //                    {
        //    System.out.println("getName(): " + m.getName());
        //    System.out.println("toString(): " + m.toString());
        //    printAnnotations(m.getAnnotations());
        //    System.out.println("");
        //}
        //                }

        //                return;
        //            }

        //            /**
        //             * RT-Вывести на экран информацию о пакете.
        //             * 
        //             * @param p
        //             *            Объект пакета.
        //             */
        //            private static void printPackageInfo(Package p)
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
        //            System.out.println(a.toString());
        //    }
        //    System.out.println("");
        //    return;
        //}


    }
}
