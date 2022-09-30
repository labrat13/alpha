using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.LogSubsystem
{
    /// <summary>
    /// NT-Менеджер лога Оператор with XML writer.
    /// </summary>
    /// <remarks> 
    /// Более медленная версия с экранированием недопустимых символов.
    /// Немного другой формат сообщения - текст расположен в элементе иначе.</remarks>
    internal class LogManager2 :LogManager
    {
        /**
    * Log writer object
    */
        protected XMLStreamWriter m_Writer;

        /**
         * NT-Param constructor.
         * @param en Engine reference.
         */
        public LogManager2(Engine en)
        {
            super(en);
        }

        /**
         * NT-Initialize log subsystem here and open log session
         * 
         * @throws Exception
         *             "Session already exists" or "Error on writing to log file."
         * 
         */
        @Override
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
        // //OutputStreamWriter osw = new OutputStreamWriter(os, "UTF-8");
        // // И вывести стандартный заголовок XML файла: <?xml version="1.0"
        // // encoding="UTF-8" standalone="yes" ?>
        // // 5. write log file header
        // osw.write("<?xml version=\"1.0\" encoding=\"UTF-8\"
        // standalone=\"yes\" ?>");
        // osw.write(LineSeparator);
        XMLOutputFactory output = XMLOutputFactory.newInstance();
        XMLStreamWriter writer = output.createXMLStreamWriter(os);
        writer.writeStartDocument("utf-8", "1.0");
        writer.writeCharacters(LineSeparator);
        // // Вывести открытие сессии <session> как корень документа.
        // osw.write("<session>");
        // osw.write(LineSeparator);
        writer.writeStartElement("session");
        writer.writeCharacters(LineSeparator);
        // store to class field
        this.m_Writer = writer;
        // set ready flag
        this.m_Ready = true;
        // write session start message
        this.AddMessage(EnumLogMsgClass.SessionStarted, EnumLogMsgState.OK, "Session opened");

        return;
    }

    /**
     * NT-Close log session
     * 
     * @throws XMLStreamException
     *             Error on writing to log file.
     * 
     * @throws IOException
     *             Error on writing to log file
     */
    @Override
    public void Close() throws IOException, XMLStreamException
    {
        // Cleanup log subsystem here
        if (this.m_Ready == true)
        {
        // write session finish message
        this.AddMessage(EnumLogMsgClass.SessionFinished, EnumLogMsgState.OK, "Session closed");
        // Вывести закрытие сессии </session>.
        this.m_Writer.writeEndDocument();
        this.m_Writer.flush();

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
     *             Throw exception if error on writing.
     */
    public void AddMessage(LogMessage msg)
            throws IOException, XMLStreamException
    {
        // skip writing if subsystem not ready
        if (this.m_Ready == false)
            return;
    // write msg line
    msg.WriteXmlWriter(this.m_Writer);
        this.m_Writer.writeCharacters(LineSeparator);// add new line
        // flush to file
        this.m_Writer.flush();

        return;
    }

}
}
