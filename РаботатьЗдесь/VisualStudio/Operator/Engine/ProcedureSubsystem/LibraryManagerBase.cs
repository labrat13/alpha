using System;
using Engine.OperatorEngine;

namespace Engine.ProcedureSubsystem
{
    /// <summary>
    /// NR-Базовый класс для менеджеров библиотек Процедур.
    /// </summary>
    public class LibraryManagerBase : Engine.OperatorEngine.EngineSubsystem
    {
        //Этот класс не помечать атрибутом OperatorProcedureAttribute, поскольку он только задает интерфейс для производных классов!

        //TODO: портировать код с явы


        // TODO: добавить сюда поле пути к папке утилит этой библиотеки Процедур. И функцию его заполнения тоже.
        // так как в каждой библиотеке путь свой, то придется подставлять этот путь во время сборки массива Процедур для передачи в Оператор.
        // Задача эта выглядит сложной пока что..

        // TODO: в производном классе переопределить методы onInit() и onExit() для реализации новых ресурсов данного класса

        #region *** Fields and constants ***

        //унаследованы от родительского класса: 
        //protected Engine m_Engine;
        //protected bool m_Initialized;

        /// <summary>
        /// Static Version string for current library
        /// </summary>
        protected static String m_VersionString = "1.0.0.0";

        /// <summary>
        /// Путь к файлу библиотеки Процедур.
        /// </summary>
        protected String m_LibraryPath;

        /// <summary>
        /// Название библиотеки Процедур как идентификатор Библиотеки.
        /// </summary>
        protected String m_LibraryTitle;

        #endregion

        #region *** Constructors ***

        /// <summary>
        /// NT-Конструктор
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        /// <param name="title">Library title</param>
        /// <param name="libPath">Library DLL file path</param>
        public LibraryManagerBase(Engine.OperatorEngine.Engine engine, string title, string libPath) : base(engine)
        {
            //this.m_Engine = engine; - done by parent constructor
            //this.m_Ready = false; - done by parent constructor
            this.m_LibraryPath = libPath;
            this.m_LibraryTitle = title;

            return;
        }

        #endregion

        #region *** Member access functions ***

        /// <summary>
        /// Current library file path
        /// </summary>
        public string LibraryPath
        {
            get { return this.m_LibraryPath; }
            set { this.m_LibraryPath = value; }
        }

        /// <summary>
        /// Current library title
        /// </summary>
        public string LibraryTitle
        {
            get { return this.LibraryTitle; }
            set { this.LibraryTitle = value; }
        }

        #endregion

        #region  *** Skip Overriding this from EngineSubsystem parent class ***
        /// <summary>
        /// NR - Initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onOpen()
        {
            throw new Exception("Function must be overridden");
        }

        /// <summary>
        /// NR - De-initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onClose()
        {
            throw new Exception("Function must be overridden");
        }
        #endregion

        #region *** Public API ***

        //TODO: решить, следует ли перенести поле версии подсистемы в базовый класс, сделав его обязательным для всех подсистем Оператор.
        //А как его будут переопределять производные классы?

        /// <summary>
        /// NT-Get library version
        /// </summary>
        public static Version getLibraryVersion()
        {
            return new Version(m_VersionString);
        }

        /// <summary>
        /// NR- Get Places collection from this library.  This function must be overriden in child class.
        /// </summary>
        /// <returns>Function returns array of Places, defined in this library.</returns>
        /// <exception cref="Exception">Error in processing.</exception>
        public virtual Place[] getLibraryPlaces()
        {
            // if (this.m_Ready == false)
            // {
            //
            // }
            // else
            // {
            //
            // }
            throw new Exception("Function must be overridden");
        }

        /// <summary>
        /// NR-Get Procedures collection from this library. This function must be overriden in child class.
        /// </summary>
        /// <returns>Function returns array of Procedures, defined in this library.</returns>
        /// <exception cref="Exception">Error in processing.</exception>
        public virtual Procedure[] getLibraryProcedures()
        {
            // if (this.m_Ready == false)
            // {
            //
            // }
            // else
            // {
            //
            // }
            throw new Exception("Function must be overridden");
        }

        /// <summary>
        /// NT- Get Setting collection from this library. This function must be overriden in child class.
        /// </summary>
        /// <returns>Function returns array of SettingItem, defined in this library.</returns>
        /// <exception cref="Exception">Error in processing.</exception>
        public virtual SettingSubsystem.SettingItem[] getLibrarySettings()
        {
            // if (this.m_Ready == false)
            // {
            //
            // }
            // else
            // {
            //
            // }
            throw new Exception("Function must be overridden");
        }
        #endregion


        #region *** Статические функции загрузки классов ***

        //TODO: переделать все это на виндовс-версию, используя код виндовс-примера.
        // - сначала написать код менеджера и тестового метода в GeneralProcedures, чтобы правильно совместить код там и здесь.

        /// <summary>
        /// NR-Try get library manager from specified DLL file.
        /// </summary>
        /// <param name="engine">Operator engine reference.</param>
        /// <param name="title">Library title.</param>
        /// <param name="path">Library DLL file pathname.</param>
        /// <returns>Function returns library manager object or null if it is not found.</returns>
        /// <exception cref="Exception">Error on loading.</exception>
        public static LibraryManagerBase loadLibraryManager(Engine.OperatorEngine.Engine engine, String title, String path)
        {

            //    // throw exceptions if any error
            //    // return null if no manager in that file
            //    // TODO: не забыть вписать в объект менеджера путь к его файлу path!
            //    LibraryManagerBase result = null;
            //    URLClassLoader loader = null;
            //    String classTitle = title + ".LibraryManager";

            //        try
            //        {
            //        if (Utility.StringIsNullOrEmpty(path) || (FileSystemManager.isFileExists(path) == false))
            //            throw new Exception("Не найдена библиотека Процедур: " + title);

            //        // 2. load class from specified JarFile
            //        URL classUrl = new URL("file://" + path);
            //        URL[] classUrls = new URL[] { classUrl };
            //        loader = new URLClassLoader(classUrls, engine.getClass().getClassLoader());
            //        // Если класслоадер потребуется более одного раза, следует как-то сделать его постоянным?
            //        Class <?> cls = null;
            //        // load class, return null if not exists
            //        cls = loader.loadClass(classTitle);

            //        // 3. Package annotation check
            //        Package pg = cls.getPackage();
            //        // не может не быть корневого пакета
            //        if (pg == null)
            //            throw new Exception("Корневой пакет не найден в библиотеке Процедур " + path);
            //        OperatorProcedure packageAnnot = pg.getAnnotation(OperatorProcedure.class);
            //if (packageAnnot == null)
            //    throw new Exception(String.format("Корневой пакет в библиотеке Процедур \"%s\" не помечен аннотацией OperatorProcedure.", path));
            //if (packageAnnot.State() == ImplementationState.NotRealized)
            //    throw new Exception(String.format("Корневой пакет в библиотеке Процедур \"%s\" помечен как NotRealized.", path));

            //// 4. if class not annotated, stop work
            //OperatorProcedure annot = cls.getAnnotation(OperatorProcedure.class);
            //if (annot == null)
            //    throw new Exception(String.format("Класс LibraryManager в библиотеке Процедур \"%s\" не помечен аннотацией OperatorProcedure.", path));
            //// 5. find method
            //ImplementationState state = annot.State();
            //// String class_title = annot.Title();
            //// String class_description = annot.Description();
            //if (state == ImplementationState.NotRealized)
            //    throw new Exception(String.format("Класс LibraryManager в библиотеке Процедур \"%s\" помечен как NotRealized.", path));
            //if (state == ImplementationState.NotTested)
            //{
            //    // print warning "This Procedure method class %s marked as not tested"
            //    String msg1 = String.format("Класс LibraryManager в библиотеке Процедур \"%s\" помечен как NotTested", path);
            //    engine.get_OperatorConsole().PrintTextLine(msg1, EnumDialogConsoleColor.Предупреждение);
            //    // add msg to log
            //    engine.getLogManager().AddMessage(EnumLogMsgClass.Default, EnumLogMsgState.Default, msg1);
            //}
            //// 6. create object
            //Constructor <?> cs = cls.getConstructor(Engine.class, String.class, String.class);
            //result = (LibraryManagerBase)cs.newInstance(engine, title, path);

            //        }
            //        finally
            //{
            //    // close loader before exit
            //    if (loader != null)
            //        loader.close();
            //}
            //return result;

            return null;
        }






        /// <summary>
        /// NR-Invoke procedure method.
        /// </summary>
        /// <param name="p">Procedure for execution.</param>
        /// <param name="names">assembly.class.method titles array.</param>
        /// <param name="jarFilePath">Path to library DLL file.</param>
        /// <param name="engine">Engine object reference.</param>
        /// <param name="manager">Library manager object reference.</param>
        /// <param name="command">Query object.</param>
        /// <param name="args">Procedure arguments array.</param>
        /// <returns>Function returns Procedure result code.</returns>
        /// <exception cref="Exception">"Не найдена библиотека Процедур" и другие.</exception>
        public static EnumProcedureResult invokeProcedure(
                Procedure p,
                String[] names,
                String jarFilePath,
                Engine.OperatorEngine.Engine engine,
                LibraryManagerBase manager,
                UserQuery command,
                ArgumentCollection args)
        {
            //    URLClassLoader loader = null;
            //    EnumProcedureResult result = EnumProcedureResult.Error;

            //        try
            //        {

            //        String AssemblyTitle = names[0];
            //        String ClassTitle = AssemblyTitle + "." + names[1]; // = "AssemblyTitle.ClassTitle"
            //        String MethodTitle = names[2];
            //        String printMethodTitle = ClassTitle + "." + MethodTitle; // = "AssemblyTitle.ClassTitle.MethodTitle"

            //        // 1. Получить абсолютный путь к JAR файлу сборки.
            //        if (Utility.StringIsNullOrEmpty(jarFilePath) || (FileSystemManager.isFileExists(jarFilePath) == false))
            //            throw new Exception("Не найдена библиотека Процедур: " + AssemblyTitle);
            //        // а далее - по коду из прототипа

            //        // 2. load class from specified JarFile
            //        URL classUrl = new URL("file://" + jarFilePath);
            //        URL[] classUrls = new URL[] { classUrl };
            //        loader = new URLClassLoader(classUrls, engine.getClass().getClassLoader());
            //        Class <?> cls = loader.loadClass(ClassTitle);

            //        // 3. Package annotation check
            //        Package pg = cls.getPackage();
            //        // не может не быть корневого пакета
            //        if (pg == null)
            //            throw new Exception("Корневой пакет не найден в библиотеке Процедур " + jarFilePath);
            //        OperatorProcedure packageAnnot = pg.getAnnotation(OperatorProcedure.class);
            //if (packageAnnot == null)
            //    throw new Exception(String.format("Корневой пакет в библиотеке Процедур \"%s\" не помечен аннотацией OperatorProcedure.", jarFilePath));
            //if (packageAnnot.State() == ImplementationState.NotRealized)
            //    throw new Exception(String.format("Корневой пакет в библиотеке Процедур \"%s\" помечен как NotRealized.", jarFilePath));
            //if (packageAnnot.State() == ImplementationState.NotTested)
            //{
            //    // print warning "This Procedure method class %s marked as not tested"
            //    String msg1 = "Корневой пакет библиотеки \"" + AssemblyTitle + "\", содержащий данную Процедуру, помечен как NotTested";
            //    engine.get_OperatorConsole().PrintTextLine(msg1, EnumDialogConsoleColor.Предупреждение);
            //    // add msg to log
            //    engine.getLogManager().AddMessage(EnumLogMsgClass.Default, EnumLogMsgState.Default, msg1);
            //}

            //// 4. if class not annotated, stop work
            //OperatorProcedure annot = cls.getAnnotation(OperatorProcedure.class);
            //if (annot == null)
            //    throw new Exception("Класс \"" + ClassTitle + "\", содержащий данную Процедуру, не помечен аннотацией OperatorProcedure.");
            //// 5. find method
            //ImplementationState state = annot.State();
            //// String class_title = annot.Title();
            //// String class_description = annot.Description();
            //if (state == ImplementationState.NotRealized)
            //    throw new Exception("Класс \"" + ClassTitle + "\", содержащий данную Процедуру, помечен как NotRealized.");
            //if (state == ImplementationState.NotTested)
            //{
            //    // print warning "This Procedure method class %s marked as not tested"
            //    String msg1 = "Класс \"" + ClassTitle + "\", содержащий данную Процедуру, помечен как NotTested";
            //    engine.get_OperatorConsole().PrintTextLine(msg1, EnumDialogConsoleColor.Предупреждение);
            //    // add msg to log
            //    engine.getLogManager().AddMessage(EnumLogMsgClass.Default, EnumLogMsgState.Default, msg1);
            //}
            //// 6. get method
            //Method m = cls.getMethod(MethodTitle, Engine.class, LibraryManagerBase.class, UserQuery.class, ArgumentCollection.class);
            //// throw NoSuchMethodException if cannot find method

            //// 7. check method annotation
            //OperatorProcedure annot2 = m.getAnnotation(OperatorProcedure.class);

            //if (annot2 == null)
            //    throw new Exception("Указанный метод \"" + printMethodTitle + "\" не помечен аннотацией OperatorProcedure.");
            //ImplementationState state2 = annot2.State();
            //// String method_title = annot2.Title();
            //// String method_description = annot2.Description();
            //if (state2 == ImplementationState.NotRealized)
            //    throw new Exception("Указанный метод \"" + printMethodTitle + "\" помечен как NotRealized.");
            //// запросить у пользователя подтверждение на запуск процедуры, помеченной как требующая отладки.
            //if (state2 == ImplementationState.NotTested)
            //{
            //    String question = String.format("Исполняемый метод \"%s\" помечен как NotTested. Продолжить выполнение?", printMethodTitle);
            //    EnumSpeakDialogResult esdr = engine.get_OperatorConsole().PrintДаНетОтмена(question);
            //    // если пользователь ответил не "Да", то отменить исполнение Процедуры.
            //    if (!esdr.isДа())
            //    {
            //        // TODO: тут надо прервать выполнение и вернуть результат EnumProcedureResult.CancelledByUser
            //        // Я накидал сейчас что попало, это не то, что нужно.
            //        result = EnumProcedureResult.CancelledByUser;
            //        // throw new Exception("Процедура прервана пользователем.");
            //    }
            //}

            //// 8. execute method if not previous cancelled by user
            //if (result != EnumProcedureResult.CancelledByUser)
            //{
            //    Object returned = m.invoke(null, engine, manager, command, args);
            //    // 8. return result
            //    if (returned == null)
            //        throw new Exception("Ошибка: Метод \"" + printMethodTitle + "\" вернул null.");

            //    result = (EnumProcedureResult)returned;
            //}
            //        }
            //        // ловить тут ничего не будем - все вызывающему коду передаем.
            //        finally
            //{
            //    // close class loader
            //    if (loader != null)
            //        loader.close();
            //}
            //            return result;

            return EnumProcedureResult.Unknown;
        }

        #endregion
    }
}
