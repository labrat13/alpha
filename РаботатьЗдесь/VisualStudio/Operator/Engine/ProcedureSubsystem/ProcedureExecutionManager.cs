using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using Engine.LexiconSubsystem;
using Engine.LogSubsystem;
using Engine.OperatorEngine;
using Engine.SettingSubsystem;
using Engine.Utility;

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

        /// <summary>
        /// NT-Load LibraryManager object from each of founded Procedure Library DLL file.
        /// </summary>
        /// <returns>Function returns Dictionary[titleAsKey, manager]</returns>
        private Dictionary<String, LibraryManagerBase> loadLibraries()
        {
            // создать словарь название библиотеки - объект менеджера.
            Dictionary<String, LibraryManagerBase> result = new Dictionary<String, LibraryManagerBase>();

            //файлы сборок библиотек процедур должны лежать в подпапках внутри папки хранилища библиотек.
            // собираем файлы DLL в подкаталогах указанного каталога 
            // функция возвращает объекты доступа для любых найденных файлов DLL, а не только сборок Процедур.
            List<FileInfo> dlls = FileSystemManager.getDllFilesFromProcedureLibraryFolder();
            // найденные файлы проверить и обработать.
            // здесь исключение не должно прерывать общий процесс загрузки библиотек,
            // но должно выводиться на консоль и в лог.
            foreach (FileInfo f in dlls)
            {
                try
                {
                    // извлечь имя файла без расширения и путь к файлу
                    String filetitle = Utility.FileUtility.getFilenameWithoutExtension(f.Name);
                    String path = f.FullName;
                    // get library manager object from class loader
                    // это заодно отфильтрует прочие файлы DLL, которые не содержат класс МенеджерБиблиотекиПроцедур.
                    LibraryManagerBase manager = LibraryManagerBase.loadLibraryManager(this.m_Engine, filetitle, path);
                    // поместить объект в словарь только если файл является правильным файлом библиотеки
                    //TODO: здесь учитиывается регистр символов для ключа filetitle! 
                    if (manager != null)
                        result.Add(filetitle, manager);
                }
                catch (Exception e)
                {
                    String title = "Исключение при загрузке библиотеки Процедур " + f.ToString();
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
                        result.AddRange(par);
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
                        Procedure[] par = lmb.getLibraryProcedures();//TODO: переделать на List для упрощения                                               // add to result list
                        result.AddRange(par);
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
                        result.AddRange(par);
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

        
        /// <summary>
        /// NT- Запустить процедуру на исполнение
        /// </summary>
        /// <param name="p">Объект Процедуры.</param>
        /// <param name="names">Массив частей пути Процедуры.</param>
        /// <param name="command">Объект запроса пользователя.</param>
        /// <param name="engine">Ссылка на Движок Оператора.</param>
        /// <param name="args">Массив аргументов для Процедуры.</param>
        /// <returns>Функция возвращает код результата исполнения Процедуры.</returns>
        /// <exception cref="Exception">Error on execution.</exception>
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
            String dllFilePath = manager.LibraryPath;
            // invoke procedure and return result
            //TODO: почему не вызывается эта же функция из самого класса менеджера библиотеки?
            //ведь она могла бы содержать локальные уточнения?
            if (manager.isReady == false)
                throw new Exception("Library manager not initialized for " + dllFilePath);
            else return LibraryManagerBase.invokeProcedure(p, names, dllFilePath, engine, manager, command, args);
        }



        #region *** Run application by command line ***
        //TODO: Сократить количество функций со схожим функционалом.

        /// <summary>
        /// NT-Запустить приложение и немедленно выйти из функции.
        /// </summary>
        /// <param name="cmdline">Командная строка для исполнения</param>
        /// <param name="workDirectory">Путь к рабочему каталогу, должен существовать.</param>
        /// <returns>Возвращает значение 0.</returns>
        public int ExecuteApplicationSimple(String cmdline, String workDirectory)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            //тут надо разделить входную cmdline на приложение и аргументы.
            String[] sar = RegexManager.ParseCommandLine(cmdline);
            psi.FileName = sar[0];
            psi.Arguments = sar[1];

            //использовать профиль текущего пользователя.
            psi.LoadUserProfile = false;
            //показывать диалог сообщения об ошибке в запущенной программе.
            //psi.ErrorDialog = true;
            //psi.ErrorDialogParentHandle = Process.GetCurrentProcess().MainWindowHandle;
            //использовать ShellExecute ? 
            psi.UseShellExecute = true;
            //psi.StandardOutputEncoding = ?
            psi.WindowStyle = ProcessWindowStyle.Normal;
            //сложности с рабочим каталогом:
            //- если UseShellExecute = false, WorkingDirectory используется в работе процесса.
            //UseShellExecute must be false if the UserName property is not null or an empty string,
            // or an InvalidOperationException will be thrown when the Process.Start(ProcessStartInfo) method is called.
            //- если UseShellExecute = true, WorkingDirectory должна указывать на каталог с запускаемым файлом.
            //  Если это пустая строка, вместо WorkingDirectory используется текущий каталог текущего приложения.
            //UseShellExecute must be true if you set the ErrorDialog property to true.
            psi.WorkingDirectory = workDirectory;
            //start process
            Process.Start(psi);

            return 0;
        }

        /// <summary>
        /// NT-Запустить приложение и немедленно выйти из функции.
        /// </summary>
        /// <param name="cmdline"></param>
        /// <param name="workDirectory">Application working directory, must exists.</param>
        /// <param name="logging">rint environment variables to Operator console and log.</param>
        /// <returns>Возвращает значение 0.</returns>
        public int ExecuteApplication(
            String cmdline,
            String workDirectory,
            Boolean logging
            )
        {
            //тут надо разделить входную cmdline на приложение и аргументы.
            String[] sar = RegexManager.ParseCommandLine(cmdline);
            
            //запустить основную функцию
            return ExecuteApplication(sar[0], sar[1], workDirectory, logging);
        }

        /// <summary>
        /// NT-Запустить приложение и немедленно выйти из функции.
        /// </summary>
        /// <param name="app">Application path</param>
        /// <param name="args">Argument string</param>
        /// <param name="workDirectory">Application working directory, must exists.</param>
        /// <param name="logging">Print environment variables to Operator console and log.</param>
        /// <returns>Возвращает значение 0.</returns>
        public int ExecuteApplication(
                    String app,
                    String args,
                    String workDirectory,
                    Boolean logging
                    )
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = app;
            psi.Arguments = args;
            //использовать профиль текущего пользователя.
            psi.LoadUserProfile = false;
            //показывать диалог сообщения об ошибке в запущенной программе.
            //psi.ErrorDialog = true;
            //psi.ErrorDialogParentHandle = Process.GetCurrentProcess().MainWindowHandle;
            //использовать ShellExecute ?
            psi.UseShellExecute = true;
            //psi.StandardOutputEncoding = ?
            psi.WindowStyle = ProcessWindowStyle.Normal;
            //сложности с рабочим каталогом:
            //- если UseShellExecute = false, WorkingDirectory используется в работе процесса.
            //UseShellExecute must be false if the UserName property is not null or an empty string,
            // or an InvalidOperationException will be thrown when the Process.Start(ProcessStartInfo) method is called.
            //- если UseShellExecute = true, WorkingDirectory должна указывать на каталог с запускаемым файлом.
            //  Если это пустая строка, вместо WorkingDirectory используется текущий каталог текущего приложения.
            //UseShellExecute must be true if you set the ErrorDialog property to true.
            psi.WorkingDirectory = workDirectory;
            //get environment variables
            if (logging == true)
            {                
                PrintAndLogEnvironmentVariables(psi.EnvironmentVariables);
            }

            //start process
            Process.Start(psi);

            return 0;
        }

        /// <summary>
        /// NT-Print EnvironmentVariables
        /// </summary>
        /// <param name="ev">EnvironmentVariables collection from ProcessStartInfo.EnvironmentVariables property.</param>
        private void PrintAndLogEnvironmentVariables(StringDictionary ev)
        {
            this.m_Engine.AddMessageToConsoleAndLog("------------", DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
            this.m_Engine.AddMessageToConsoleAndLog("Дамп ProcessStartInfo Environment variables: title -> value", DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
            this.m_Engine.AddMessageToConsoleAndLog("------------", DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);

            foreach(DictionaryEntry di in ev)
            {
                String s = String.Format("\"{0}\" -> \"{1}\"", di.Key, StringUtility.GetStringTextNull(di.Value));
                this.m_Engine.AddMessageToConsoleAndLog(s, DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);
            }
            this.m_Engine.AddMessageToConsoleAndLog("------------", DialogConsoleColor.Сообщение, EnumLogMsgClass.DebugLoggingMessage, EnumLogMsgState.OK);

            return;
        }
        #endregion

        #region *** Java Reflection debug functions *** TODO: перенести эти функции в Utility или другое подходящее место

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
        #endregion

    }
}
