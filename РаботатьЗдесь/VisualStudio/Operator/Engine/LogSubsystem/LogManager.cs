using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.LogSubsystem
{
    /*
 * Для каждого сеанса лога - свой файл лога, а не все в один файл лога.
 * А более старые файлы лога удалять при превышении их количества?
 * TODO: Это не отработано в виндовс-прототипе!
 */

    /// <summary>
    /// NR - Менеджер лога Оператор
    /// </summary>
    /// <remarks>Простой менеджер быстро создает XML-теги текстом без экранирования символов.</remarks>
    public class LogManager : Engine.OperatorEngine.EngineSubsystem
    {

        /// <summary>
        /// NR - Конструктор
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        public LogManager(OperatorEngine.Engine engine) : base(engine)
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

        // TODO: Надо описать подсистему лога в документации проекта!
        // TODO: Надо добавить функцию регулирования размера каталога файлов лога.
        // - когда ее запускать? Подсчет размера каталога лога - долгое дело?
        // - предел размера каталога лога - задавать в настройках Оператор.
        // - - LogFolderSizeLimitMB = 1024
        // - - LogFileCountLimit = 4096
        // Нужна функция подсчета числа файлов в каталоге и их размера.
        // TODO: Надо добавить предельный размер файла лога, и создавать его продолжения при превышении предела.
        // - Но это уже немного другой процесс лога получается!
        // Придется переделывать подсистему лога.
        // - предел размера файла задаватьв настройках Оператор.
        // - - LogFileSizeLimitMB = 4096
        // Надо чтобы это все работало для производных классов, они и будут далее использоваться.

        /**
         * Application log folder path
         */
        protected final static String AppLogFolderPath = FileSystemManager.getAppLogFolderPath();
        // SystemInfoManager.GetUserHomeFolderPath() + File.separator + "Operator" + File.separator + "logs";
        // = FileSystemManager.AppLogFolderPath; - заменено на время отладки,
        // поскольку Engine класс не готов.

        /**
         * Line break symbol "/n"
         */
        protected final static String LineSeparator = System.lineSeparator();

        /**
         * Log writer object
         */
        protected OutputStreamWriter m_Writer;

        /**
         * Backreference to Engine object
         */
        protected Engine m_Engine;

        /**
         * Log subsystem is ready to serve
         */
        protected boolean m_Ready;

        /**
         * Default constructor
         * 
         * @param en
         *            Engine object reference
         */
        public LogManager(Engine en)
        {
            this.m_Engine = en;
            this.m_Writer = null;
            // log subsystem not ready
            this.m_Ready = false;
        }

        /**
         * RT-Log subsystem is ready to serve
         * 
         * @return Функция возвращает значение флага готовности.
         */
        public boolean isReady()
        {
            return this.m_Ready;
        }

        /**
         * NT-Initialize log subsystem and open log session
         * 
         * @throws Exception
         *             "Session already exists" or "Error on writing to log file."
         * 
         */
        public void Open() throws Exception
        {
            // Если каталог лога не найден - создать новый каталог лога и файл лога
            // в нем.
            // 1. create file if not exists
            // - if log folder not exists, try create it.
            // - if log folder not writable, throw exception.
            File logFolder = new File(LogManager.AppLogFolderPath);
        if (!logFolder.exists())
            logFolder.mkdir();
        // 2. create filename as log-datetime.xml
        String filename = logFolder.getPath() + FileSystemManager.FileSeparator + this.makeNewFileName();// session_timestamp.xml
        File logfile = new File(filename);
        // если файл сессии уже существует, выбросить исключение об этом
        if (logfile.exists())
            throw new Exception("Session already exists");
        // open or create file with StreamWriter with UTF-8 encoding and path
        // Operator/logs directory/
        // 4. create writer object
        FileOutputStream os = new FileOutputStream(logfile.getPath(), false);
        OutputStreamWriter osw = new OutputStreamWriter(os, "UTF-8");
        // И вывести стандартный заголовок XML файла: <?xml version="1.0"
        // encoding="UTF-8" standalone="yes" ?>
        // 5. write log file header
        osw.write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
        osw.write(LineSeparator);
        // Вывести открытие сессии <session> как корень документа.
        osw.write("<session>");
        osw.write(LineSeparator);
        // store to class field
        this.m_Writer = osw;
        // 6. write session start message
        this.AddMessage(EnumLogMsgClass.SessionStarted, EnumLogMsgState.OK, "Session opened");
        // set ready flag
        this.m_Ready = true;

        return;
    }

    /**
     * NT-Close log session
     * 
     * @throws IOException
     *             Error on writing to log file
     * @throws XMLStreamException
     *             Error on writing to log file.
     */
    public void Close() throws IOException, XMLStreamException
    {
        // Cleanup log subsystem here
        if (this.m_Ready == true)
        {
        // write session finish message
        this.AddMessage(EnumLogMsgClass.SessionFinished, EnumLogMsgState.OK, "Session closed");
        // Вывести закрытие сессии </session>.
        this.m_Writer.write("</session>");
        this.m_Writer.write(LineSeparator);
    }
        // clear ready flag
        this.m_Ready = false;
        // Закрыть StreamWriter
        if (this.m_Writer != null)
        {
        this.m_Writer.close();
        this.m_Writer = null;
    }

        return;
    }

    /**
     * NT-Append new message object to log
     * 
     * @param msg
     *            New message object.
     * @throws IOException
     *             Error on writing to log file.
     * @throws XMLStreamException
     *             Error on writing to log file.
     */
    public void AddMessage(LogMessage msg)
            throws IOException, XMLStreamException
    {
        String s = msg.ToXmlString();
        this.m_Writer.write(s);
        this.m_Writer.write(LineSeparator);
        this.m_Writer.flush();

        return;
    }

    /**
     * NT-append new message object to log
     * 
     * @param c
     *            Event class code
     * @param s
     *            Event state code
     * @param text
     *            Event text description
     * @throws IOException
     *             Error on writing to log file.
     * @throws XMLStreamException
     *             Error on writing to log file.
     */
    public void AddMessage(EnumLogMsgClass c, EnumLogMsgState s, String text)
            throws IOException, XMLStreamException
    {
        LogMessage msg = new LogMessage(c, s, text);
        this.AddMessage(msg);

        return;
    }

    /**
     * NT-Create new log file name for new session
     * 
     * @return Function returns new log file name/
     */
    protected String makeNewFileName()
    {
        LocalDateTime dt = LocalDateTime.now();
        String p = Utility.DateTimeToFileNameString(dt);

        return "session_" + p + ".xml";
    }

    /**
     * NT-Add log message about exception. This function not thrown (suppress) any exceptions.
     * 
     * @param e
     *            Exception object to log
     */
    public void AddExceptionMessage(Exception e)
    {
        try
        {
            LogMessage msg = new LogMessage(EnumLogMsgClass.ExceptionRaised, EnumLogMsgState.Fail, e.toString());
            this.AddMessage(msg);
        }
        catch (Exception ex)
        {
            ;// add debug breakpoint here;
        }
        return;
    }

    /**
     * NT-Add log message about exception. This function not thrown (suppress) any exceptions.
     * 
     * @param title
     *            Вводный текст сообщения. Если пустая строка, то используется "Ошибка".
     * @param ex
     *            Объект исключения.
     */
    public void AddExceptionMessage(String title, Exception ex)
    {
        try
        {
            StringBuilder sb = new StringBuilder(title);
            // добавим разделительный пробел
            sb.append(' ');
            // добавим название исключения
            sb.append(ex.getClass().getName());
            // добавим текст исключения
            sb.append(": ");
            sb.append(ex.toString());
            //
            LogMessage msg = new LogMessage(EnumLogMsgClass.ExceptionRaised, EnumLogMsgState.Fail, sb.toString());
            this.AddMessage(msg);
        }
        catch (Exception e)
        {
            ;// add debug breakpoint here;
        }
        return;
    }

}
}
