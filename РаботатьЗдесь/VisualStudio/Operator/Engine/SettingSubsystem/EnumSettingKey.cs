using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.SettingSubsystem
{
    /// <summary>
    /// Енум ключей словаря настроек. Содержит члены-ключи с названием и описанием.
    /// </summary>
    public enum EnumSettingKey
    {
        // *** Settings version ***

        /**
         * Default (0) or Unknown
         */
        Default("Default", "Default", "Default or Unknown."),
        /**
         * Engine version string
         */
        EngineVersion("General", "EngineVersion", "Engine version string."),
        /**
         * This settings file version string
         */
        SettingsFileVersion("General", "SettingsFileVersion", "This settings file version string"),

        // *** Terminal command line templates ***

        /**
         * Командная строка для запуска пустого Терминала
         */
        LoneTerminal("Terminal", "LoneTerminal", "Командная строка для запуска пустого Терминала."),
        /**
         * Командная строка для запуска англоязычной команды в Терминале.
         */
        ForCommandTerminal("Terminal", "ForCommandTerminal", "Командная строка для запуска англоязычной команды в Терминале."),
        /**
         * Командная строка для запуска Процедуры в Терминале.
         */
        ForProcedureTerminal("Terminal", "ForProcedureTerminal", "Командная строка для запуска Процедуры в Терминале."),
        /**
         * Путь к рабочему каталогу для любых команд Терминала.
         */
        DefaultWorkingDirectory("Terminal", "DefaultWorkingDirectory", "Путь к рабочему каталогу для любых команд Терминала."),
        /**
         * Команда для запуска подобно клику на ярлыке программы или документа.
         */
        ShellExecuteCommand("ShellExecute", "ShellExecuteCommand", "Команда для запуска подобно клику на ярлыке программы или документа."),

        // *** Startup and finish settings ***

        /**
         * Текст команды или путь Процедуры, исполняемой при запуске Оператора.
         */
        CmdStartup("Startup", "CmdStartup", "Текст команды или путь Процедуры, исполняемой при запуске Оператора."),
        /**
         * Текст команды или путь Процедуры, исполняемой при завершении Оператора.
         */
        CmdFinish("Startup", "CmdFinish", "Текст команды или путь Процедуры, исполняемой при завершении Оператора."),
        /**
         * Логическое значение true или false, игнорировать ли настройки файла настроек и БД при загрузке и завершении Оператора.
         */
        IgnoreStartup("Startup", "IgnoreStartup", "Логическое значение, игнорировать ли команды старта и завершения Оператор из файла настроек и БД при загрузке и завершении Оператора."),

        // *** Internal command text's ***

        /**
         * Перечиcление через запятую кодовых слов для встроенной команды Выход из Оператора.
         */
        ExitAppCommands("Commands", "ExitAppCommands", "Перечисление через запятую кодовых слов для встроенной команды Выход из Оператора"),

        // *** Procedure result code processing settings ***

        /**
         * Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndSleep..
         */
        CmdSleep("AfterCommands", "CmdSleep", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndSleep."),

        /**
         * Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndReload.
         */
        CmdReload("AfterCommands", "CmdReload", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndReload"),

        /**
         * Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndShutdown.
         */
        CmdShutdown("AfterCommands", "CmdShutdown", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndShutdown"),

        /**
         * Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndLogoff.
         */
        CmdLogoff("AfterCommands", "CmdLogoff", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndLogoff"),

        /**
         * Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndHybernate.
         */
        CmdHybernate("AfterCommands", "CmdHybernate", "Текст команды или путь Процедуры, исполняемой при EnumProcedureResult.ExitAndHybernate"),

        //*** Пути к каталогам пользовательского аккаунта ***
        /**
         * Путь к каталогу документов пользовательского аккаунта.
         */
        UserFolderDocuments("UserDirectories", "UserFolderDocuments", "Путь к каталогу документов пользовательского аккаунта"),
        /**
         * Путь к каталогу рабочего стола пользовательского аккаунта.
         */
        UserFolderDesktop("UserDirectories", "UserFolderDesktop", "Путь к каталогу рабочего стола пользовательского аккаунта"),
        /**
         * Путь к каталогу загрузок пользовательского аккаунта.
         */
        UserFolderDownloads("UserDirectories", "UserFolderDownloads", "Путь к каталогу загрузок пользовательского аккаунта"),
        /**
         * Путь к каталогу музыки пользовательского аккаунта.
         */
        UserFolderMusic("UserDirectories", "UserFolderMusic", "Путь к каталогу музыки пользовательского аккаунта"),
        /**
         * Путь к каталогу изображений пользовательского аккаунта.
         */
        UserFolderPictures("UserDirectories", "UserFolderPictures", "Путь к каталогу изображений пользовательского аккаунта"),
        /**
         * Путь к каталогу публикаций пользовательского аккаунта.
         */
        UserFolderPublic("UserDirectories", "UserFolderPublic", "Путь к каталогу публикаций пользовательского аккаунта"),
        /**
         * Путь к каталогу шаблонов пользовательского аккаунта.
         */
        UserFolderTemplates("UserDirectories", "UserFolderTemplates", "Путь к каталогу шаблонов пользовательского аккаунта"),
        /**
         * Путь к каталогу видеороликов пользовательского аккаунта.
         */
        UserFolderVideos("UserDirectories", "UserFolderVideos", "Путь к каталогу видеороликов пользовательского аккаунта");


    //*** *** *** *** *** *** *** *** *** *** *** *** *** *** *** ***

        // *** Enum members ***
        /**
         * Setting namespace string
         */
    private String m_Namespace;

    /**
     * Key title string
     */
    private String m_Title;

    /**
     * Key description string
     */
    private String m_Description;

    /**
     * Get key title
     * 
     * @return Returns key title string.
     */
    public String getTitle()
    {
        return this.m_Title;
    }

    /**
     * Get key description multiline string
     * 
     * @return Returns key description string.
     */
    public String getDescription()
    {
        return this.m_Description;
    }
    /**
     * NT-Get key namespace string
     * @return Returns key namespace string/
     */
    public String getNamespace()
    {
        return this.m_Namespace;
    }

    // *** Массив ключей настроек ***
    /**
     * Статический массив имен ключей элементов енума - для оптимизации доступа к ним.
     * Если = null, то надо вызвать getKeyArray() для создания и заполнения массива.
     */
    protected static String[] KeysArray = null;

    /**
     * NT-Get array of used keynames.
     * 
     * @return Function returns array of used keyname strings.
     */
    public static String[] getKeyArray()
    {
        // Если массив не сгенерирован ранее, создать и заполнить его.
        if (KeysArray == null)
        {
            EnumSettingKey[] members = EnumSettingKey.class.getEnumConstants();
    int len = members.length;
    String[] result = new String[len];

            for (int i = 0; i<len; i++)
                result[i] = members[i].getTitle();
    // и вписать созданный массив в статическую переменную класса.
    KeysArray = result;
        }
// вернуть массив строк ключей енума.
return KeysArray;
    }

    /**
     * NT-Check keyname is in enum keynames collection. Ignore letter case.
     * 
     * @param keyname
     *            Keyname string.
     * @return Returns true if specified string already used as some keyname here.
     */
    public static boolean IsKeynameExists(String keyname)
{
    String[] keys = EnumSettingKey.getKeyArray();
    return OperatorEngine.Utility.arrayContainsStringOrdinal(keys, keyname, true);
}

// *** Конструктор членов енума ***
/**
 * Constructor
 * 
 * @param ns
 * Key namespace or group title
 * @param title
 *            New key title
 * @param description
 *            New key description multiline text
 */
private EnumSettingKey(String ns, String title, String description)
{
    this.m_Namespace = ns;
    this.m_Title = title;
    this.m_Description = description;
}
    }
}