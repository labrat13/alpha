using System;
using System.IO;
using System.Text;
using System.Xml;
using Engine.Utility;

namespace Engine.LogSubsystem
{

    // Для каждого сеанса лога - свой файл лога, а не все в один файл лога.
    // А более старые файлы лога удалять при превышении их количества?
    // TODO: Это не отработано в виндовс-прототипе!

    /// <summary>
    /// NT-Менеджер лога Оператор with XML writer.
    /// </summary>
    /// <remarks> 
    /// Более медленная версия с экранированием недопустимых символов.
    /// Немного другой формат сообщения - текст расположен в элементе иначе.</remarks>
    public class LogManager : Engine.OperatorEngine.EngineSubsystem
    {
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

        #region *** Constants and Fields ***
        /// <summary>
        /// Log writer object
        /// </summary>
        protected XmlWriter m_Writer;

        /// <summary>
        /// Application log folder path
        /// </summary>
        protected static String AppLogFolderPath = Engine.OperatorEngine.FileSystemManager.getAppLogFolderPath();
        // SystemInfoManager.GetUserHomeFolderPath() + File.separator + "Operator" + File.separator + "logs";
        // = FileSystemManager.AppLogFolderPath; - заменено на время отладки,
        // поскольку Engine класс не готов.
        #endregion

        /// <summary>
        /// NT-Param constructor.
        /// </summary>
        /// <param name="engine">Ссылка на объект движка</param>
        public LogManager(Engine.OperatorEngine.Engine engine) : base(engine)
        {
            this.m_Writer = null;
        }


        #region  *** Override this from EngineSubsystem parent class ***
        /// <summary>
        /// NT-Initialize log subsystem here and open log session
        /// </summary>
        /// <exception cref="Exception">"Session already exists" or "Error on writing to log file."</exception>
        protected override void onOpen()
        {
            // Если каталог лога не найден - создать новый каталог лога и файл лога
            // в нем.
            // 1. create file if not exists
            // - if log folder not exists, try create it.
            // - if log folder not writable, throw exception.
            DirectoryInfo logFolder = new DirectoryInfo(LogManager.AppLogFolderPath);
            if (!logFolder.Exists)
                logFolder.Create();
            // 2. create filename as log-datetime.xml
            String filename = Path.Combine(AppLogFolderPath, this.makeNewFileName());// session_timestamp.xml
            FileInfo logfile = new FileInfo(filename);
            // если файл сессии уже существует, выбросить исключение об этом
            if (logfile.Exists)
                throw new Exception("Session already exists");

            // open or create file with StreamWriter with UTF-8 encoding and path Operator/logs directory/
            // 4. create writer object
            // И вывести стандартный заголовок XML файла: <?xml version="1.0" encoding="UTF-8" standalone="yes" ?>
            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true;
            sett.CloseOutput = true;
            XmlWriter writer = XmlWriter.Create(filename, sett);

            //  5. write log file header
            // writer.WriteStartDocument(); - не требуется?
            writer.WriteComment("Operator Log file");
            // Вывести открытие сессии <session> как корень документа.
            writer.WriteStartElement("session");
            // store to class field
            this.m_Writer = writer;
            //// set ready flag
            //this.m_Ready = true; - set by caller function
            // write session start message
            this.AddMessage(EnumLogMsgClass.SessionStarted, EnumLogMsgState.OK, "Session opened");

            return;
        }

        /// <summary>
        /// NT-Close log session
        /// </summary>
        protected override void onClose()
        {
            // Cleanup log subsystem here
            if (this.m_Ready == true)
            {
                // write session finish message
                this.AddMessage(EnumLogMsgClass.SessionFinished, EnumLogMsgState.OK, "Session closed");
                // Вывести закрытие сессии </session>.
                this.m_Writer.WriteEndElement();
                //this.m_Writer.WriteEndDocument(); - не требуется?
                this.m_Writer.Flush();
            }
            //// clear ready flag - done by caller function
            //this.m_Ready = false;
            // Закрыть StreamWriter
            if (this.m_Writer != null)
            {
                this.m_Writer.Close();
                this.m_Writer = null;
            }

            return;
        }
        #endregion


        /// <summary>
        /// NT-Append new message object to log
        /// </summary>
        /// <param name="msg">New message object.</param>
        public void AddMessage(LogMessage msg)
        {
            // skip writing if subsystem not ready
            if (this.m_Ready == false)
                return;
            // write msg line
            this.m_Writer.WriteStartElement("msg");
            this.m_Writer.WriteAttributeString("t", StringUtility.DateTimeToString(msg.MsgTimestamp));
            this.m_Writer.WriteAttributeString("c", msg.MsgClass.ToString());
            this.m_Writer.WriteAttributeString("s", msg.MsgState.ToString());
            this.m_Writer.WriteString(msg.MsgText);
            this.m_Writer.WriteEndElement();
            //this.m_Writer.writeCharacters(LineSeparator);// TODO: add new line
            // flush to file
            this.m_Writer.Flush();

            return;
        }

        /// <summary>
        ///  NT-append new message object to log
        /// </summary>
        /// <param name="c">Event class code</param>
        /// <param name="s">Event state code</param>
        /// <param name="text">Event text description</param>
        /// <exception cref="IOException"> Error on writing to log file.</exception>
        public void AddMessage(EnumLogMsgClass c, EnumLogMsgState s, String text)
        {
            LogMessage msg = new LogMessage(c, s, text);
            this.AddMessage(msg);

            return;
        }

        /// <summary>
        /// NT-Create new log file name for new session
        /// </summary>
        /// <returns>Function returns new log file name</returns>
        protected String makeNewFileName()
        {
            DateTime dt = DateTime.Now;
            String p = StringUtility.DateTimeToFileNameString(dt);

            return "session_" + p + ".xml";
        }

        /// NT-Add log message about exception. This function not thrown (suppress) any exceptions.
        /// </summary>
        /// <param name="e">Exception object to log
        /// <summary></param>
        public void AddExceptionMessage(Exception e)
        {
            try
            {
                LogMessage msg = new LogMessage(EnumLogMsgClass.ExceptionRaised, EnumLogMsgState.Fail, e.ToString());
                this.AddMessage(msg);
            }
            catch (Exception ex)
            {
                ;// add debug breakpoint here;
            }
            return;
        }

        /// <summary>
        /// NT-Add log message about exception. This function not thrown (suppress) any exceptions.
        /// </summary>
        /// <param name="title">Вводный текст сообщения. Если пустая строка, то используется "Ошибка".</param>
        /// <param name="ex">Объект исключения.</param>
        public void AddExceptionMessage(String title, Exception ex)
        {
            try
            {
                //Текст сообщения: Если пустая строка, то используется "Ошибка".
                String t = "Ошибка: ";
                if (!String.IsNullOrEmpty(title))
                    t = title;
                StringBuilder sb = new StringBuilder(t);
                // добавим разделительный пробел
                sb.Append(' ');
                // добавим название исключения
                sb.Append(ex.GetType().Name);
                // добавим текст исключения
                sb.Append(": ");
                sb.Append(ex.ToString());
                //
                LogMessage msg = new LogMessage(EnumLogMsgClass.ExceptionRaised, EnumLogMsgState.Fail, sb.ToString());
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
