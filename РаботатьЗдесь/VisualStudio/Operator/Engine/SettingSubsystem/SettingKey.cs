using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.SettingSubsystem;

namespace Engine.SettingSubsystem
{
    /// <summary>
    /// NT-Енум стандартных ключей словаря настроек. Содержит члены-ключи с названием и описанием.
    /// </summary>
    public class SettingKey
    {
        #region *** Constants *** 
        
        //Это статические объекты в качестве констант - членов енума, ранее они были Java Enum

        /// <summary>
        /// Default (0) or Unknown
        /// </summary>
        public static SettingKey Default = new SettingKey("Default", "Default", "Default or Unknown.");
        
        // *** Settings version ***
        
        /// <summary>
        /// Engine version string
        /// </summary>
        public static SettingKey EngineVersion = new SettingKey("General", "EngineVersion", "Engine version string.");

        /// <summary>
        /// This settings file version string
        /// </summary>
        public static SettingKey SettingsFileVersion = new SettingKey("General", "SettingsFileVersion", "This settings file version string");

        // *** Terminal command line templates ***

        /// <summary>
        /// Командная строка для запуска пустого Терминала
        /// </summary>
        public static SettingKey LoneTerminal = new SettingKey("Terminal", "LoneTerminal", "Командная строка для запуска пустого Терминала.");

        /// <summary>
        /// Командная строка для запуска англоязычной команды в Терминале.
        /// </summary>
        public static SettingKey ForCommandTerminal = new SettingKey("Terminal", "ForCommandTerminal", "Командная строка для запуска англоязычной команды в Терминале.");

        /// <summary>
        /// Командная строка для запуска Процедуры в Терминале.
        /// </summary>
        public static SettingKey ForProcedureTerminal = new SettingKey("Terminal", "ForProcedureTerminal", "Командная строка для запуска Процедуры в Терминале.");

        /// <summary>
        /// Путь к рабочему каталогу для любых команд Терминала.
        /// </summary>
        public static SettingKey DefaultWorkingDirectory = new SettingKey("Terminal", "DefaultWorkingDirectory", "Путь к рабочему каталогу для любых команд Терминала.");

        /// <summary>
        /// Команда для запуска подобно клику на ярлыке программы или документа.
        /// </summary>
        public static SettingKey ShellExecuteCommand = new SettingKey("ShellExecute", "ShellExecuteCommand", "Команда для запуска подобно клику на ярлыке программы или документа.");

        // *** Startup and finish settings ***

        /// <summary>
        /// Текст команды или путь Процедуры, исполняемой при запуске Оператора.
        /// </summary>
        public static SettingKey CmdStartup = new SettingKey("Startup", "CmdStartup", "Текст команды или путь Процедуры, исполняемой при запуске Оператора.");

        /// <summary>
        /// Текст команды или путь Процедуры, исполняемой при завершении Оператора.
        /// </summary>
        public static SettingKey CmdFinish = new SettingKey("Startup", "CmdFinish", "Текст команды или путь Процедуры, исполняемой при завершении Оператора.");

        /// <summary>
        /// Логическое значение true или false, игнорировать ли настройки файла настроек и БД при загрузке и завершении Оператора.
        /// </summary>
        public static SettingKey IgnoreStartup = new SettingKey("Startup", "IgnoreStartup", "Логическое значение, игнорировать ли команды старта и завершения Оператор из файла настроек и БД при загрузке и завершении Оператора.");

        // *** Internal command text's ***

        /// <summary>
        ///  Перечиcление через запятую кодовых слов для встроенной команды Выход из Оператора.
        /// </summary>
        public static SettingKey ExitAppCommands = new SettingKey("Commands", "ExitAppCommands", "Перечисление через запятую кодовых слов для встроенной команды Выход из Оператора");

        // *** Procedure result code processing settings ***

        /// <summary>
        /// Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndSleep.
        /// </summary>
        public static SettingKey CmdSleep = new SettingKey("AfterCommands", "CmdSleep", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndSleep.");

        /// <summary>
        ///  Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndReload.
        /// </summary>
        public static SettingKey CmdReload = new SettingKey("AfterCommands", "CmdReload", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndReload");

        /// <summary>
        /// Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndShutdown.
        /// </summary>
        public static SettingKey CmdShutdown = new SettingKey("AfterCommands", "CmdShutdown", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndShutdown");

        /// <summary>
        /// Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndLogoff.
        /// </summary>
        public static SettingKey CmdLogoff = new SettingKey("AfterCommands", "CmdLogoff", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndLogoff");

        /// <summary>
        /// Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndHybernate.
        /// </summary>
        public static SettingKey CmdHybernate = new SettingKey("AfterCommands", "CmdHybernate", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndHybernate");

        // Пути к каталогам пользовательского аккаунта ***

        /// <summary>
        /// Путь к каталогу документов пользовательского аккаунта.
        /// </summary>
        public static SettingKey UserFolderDocuments = new SettingKey("UserDirectories", "UserFolderDocuments", "Путь к каталогу документов пользовательского аккаунта");

        /// <summary>
        /// Путь к каталогу рабочего стола пользовательского аккаунта.
        /// </summary>
        public static SettingKey UserFolderDesktop = new SettingKey("UserDirectories", "UserFolderDesktop", "Путь к каталогу рабочего стола пользовательского аккаунта");

        /// <summary>
        /// Путь к каталогу загрузок пользовательского аккаунта.
        /// </summary>
        public static SettingKey UserFolderDownloads = new SettingKey("UserDirectories", "UserFolderDownloads", "Путь к каталогу загрузок пользовательского аккаунта");

        /// <summary>
        /// Путь к каталогу музыки пользовательского аккаунта.
        /// </summary>
        public static SettingKey UserFolderMusic = new SettingKey("UserDirectories", "UserFolderMusic", "Путь к каталогу музыки пользовательского аккаунта");

        /// <summary>
        /// Путь к каталогу изображений пользовательского аккаунта.
        /// </summary>
        public static SettingKey UserFolderPictures = new SettingKey("UserDirectories", "UserFolderPictures", "Путь к каталогу изображений пользовательского аккаунта");

        /// <summary>
        /// Путь к каталогу публикаций пользовательского аккаунта.
        /// </summary>
        public static SettingKey UserFolderPublic = new SettingKey("UserDirectories", "UserFolderPublic", "Путь к каталогу публикаций пользовательского аккаунта");

        /// <summary>
        /// Путь к каталогу шаблонов пользовательского аккаунта.
        /// </summary>
        public static SettingKey UserFolderTemplates = new SettingKey("UserDirectories", "UserFolderTemplates", "Путь к каталогу шаблонов пользовательского аккаунта");

        /// <summary>
        /// Путь к каталогу видеороликов пользовательского аккаунта.
        /// </summary>
        public static SettingKey UserFolderVideos = new SettingKey("UserDirectories", "UserFolderVideos", "Путь к каталогу видеороликов пользовательского аккаунта");

        #endregion

        #region *** Fields ***
        /// <summary>
        /// Setting namespace string
        /// </summary>
        private String m_Namespace;
        /// <summary>
        /// Key title string
        /// </summary>
        private String m_Title;
        /// <summary>
        /// Key description string
        /// </summary>
        private String m_Description;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingKey"/> class.
        /// </summary>
        /// <param name="ns">Key namespace or group title</param>
        /// <param name="title">New key title</param>
        /// <param name="description">New key description multiline text</param>
        public SettingKey(String ns, String title, String description)
        {
            this.m_Namespace = ns;
            this.m_Title = title;
            this.m_Description = description;

            return;
        }

        #region *** Properties ***
        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>
        /// The namespace.
        /// </value>
        public string Namespace { get => this.m_Namespace; set => this.m_Namespace = value; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get => this.m_Title; set => this.m_Title = value; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get => this.m_Description; set => this.m_Description = value; }

        #endregion

//        // *** Массив ключей настроек ***
//        /**
//         * Статический массив имен ключей элементов енума - для оптимизации доступа к ним.
//         * Если = null, то надо вызвать getKeyArray() для создания и заполнения массива.
//         */
//        protected static String[] KeysArray = null;

//        /**
//         * NT-Get array of used keynames.
//         * 
//         * @return Function returns array of used keyname strings.
//         */
//        public static String[] getKeyArray()
//        {
//            // Если массив не сгенерирован ранее, создать и заполнить его.
//            if (KeysArray == null)
//            {
//                EnumSettingKey[] members = EnumSettingKey.class.getEnumConstants();
//        int len = members.length;
//        String[] result = new String[len];

//            for (int i = 0; i<len; i++)
//                result[i] = members[i].getTitle();
//        // и вписать созданный массив в статическую переменную класса.
//        KeysArray = result;
//        }
//// вернуть массив строк ключей енума.
//return KeysArray;
//    }

///**
// * NT-Check keyname is in enum keynames collection. Ignore letter case.
// * 
// * @param keyname
// *            Keyname string.
// * @return Returns true if specified string already used as some keyname here.
// */
//public static boolean IsKeynameExists(String keyname)
//{
//    String[] keys = EnumSettingKey.getKeyArray();
//    return OperatorEngine.Utility.arrayContainsStringOrdinal(keys, keyname, true);
//}






    }
}
