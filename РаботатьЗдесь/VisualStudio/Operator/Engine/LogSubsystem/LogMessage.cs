using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.LogSubsystem
{
    /// <summary>
    /// NT-Объект сообщения лога
    /// </summary>
    internal class LogMessage
    {
        /**
    * Event timestamp
    */
        protected LocalDateTime m_MsgStamp;

        /**
         * Event class code
         */
        protected EnumLogMsgClass m_MsgClass;

        /**
         * Event state code
         */
        protected EnumLogMsgState m_MsgState;

        /**
         * Event description text
         */
        protected String m_MsgText;

        /**
         * Default constructor
         */
        public LogMessage()
        {
            this.m_MsgClass = EnumLogMsgClass.Default;
            this.m_MsgState = EnumLogMsgState.Default;
            this.m_MsgText = "";// or buffered - Utility.StringCopy(text);
            this.m_MsgStamp = LocalDateTime.now();
        }

        /**
         * NT-Parameter constructor
         * 
         * @param c
         *            Event class code
         * @param s
         *            Event state code
         * @param text
         *            Event text description
         */
        public LogMessage(EnumLogMsgClass c, EnumLogMsgState s, String text)
        {
            this.m_MsgClass = c;
            this.m_MsgState = s;
            this.m_MsgText = text;// or buffered - Utility.StringCopy(text);
            this.m_MsgStamp = LocalDateTime.now();

            return;
        }

        /**
         * NT-Parameter constructor
         * 
         * @param t
         *            Event timestamp as come from LocalDateTime.now()
         * @param c
         *            Event class code
         * @param s
         *            Event state code
         * @param text
         *            Event text description
         */
        public LogMessage(LocalDateTime t,
                EnumLogMsgClass c,
                EnumLogMsgState s,
                String text)
        {
            this.m_MsgClass = c;
            this.m_MsgState = s;
            this.m_MsgText = text;// or buffered - Utility.StringCopy(text);
            this.m_MsgStamp = t;

            return;
        }

        /**
         * NT-Get event description text
         * 
         * @return event description text
         */
        public String getMsgText()
        {
            return m_MsgText;
        }

        /**
         * NT-Set event description text
         * 
         * @param msgText
         *            event description text
         */
        public void setMsgText(String msgText)
        {
            this.m_MsgText = msgText;
        }

        /**
         * NT-Get event state code
         * 
         * @return event state code
         */
        public EnumLogMsgState getMsgState()
        {
            return m_MsgState;
        }

        /**
         * NT-Set event state code
         * 
         * @param msgState
         *            the event state code to set
         */
        public void setMsgState(EnumLogMsgState msgState)
        {
            this.m_MsgState = msgState;
        }

        /**
         * NT-Get event class code
         * 
         * @return the event class code
         */
        public EnumLogMsgClass getMsgClass()
        {
            return m_MsgClass;
        }

        /**
         * NT-Set event class code
         * 
         * @param msgClass
         *            the event class code to set
         */
        public void setMsgClass(EnumLogMsgClass msgClass)
        {
            this.m_MsgClass = msgClass;
        }

        /**
         * NT-Get event timestamp value
         * 
         * @return the event timestamp value
         */
        public LocalDateTime getMsgStamp()
        {
            return m_MsgStamp;
        }

        /**
         * NT-Set event timestamp value
         * 
         * @param msgStamp
         *            the event timestamp value to set
         */
        public void setMsgStamp(LocalDateTime msgStamp)
        {
            this.m_MsgStamp = msgStamp;
        }

        /**
         * NT- String for debugging
         * 
         * @see java.lang.Object#toString()
         */
        @Override
    public String toString()
        {
            StringBuilder builder = new StringBuilder();
            builder.append("LogMessage [m_MsgStamp=");
            builder.append(this.m_MsgStamp);
            builder.append(", m_MsgClass=");
            builder.append(this.m_MsgClass);
            builder.append(", m_MsgState=");
            builder.append(this.m_MsgState);
            builder.append(", m_MsgText=");
            builder.append(this.m_MsgText);
            builder.append("]");

            return builder.toString();
        }

        /**
         * NT-Create xml string to write to log file
         * 
         * @return Function returns Xml string without /n
         */
        public String ToXmlString()
        {
            // sample line: <msg t="msg stamp" c="0" s="0" text="msg text" />
            StringBuilder builder = new StringBuilder();
            builder.append("<msg t=\"");
            builder.append(Utility.DateTimeToString(this.m_MsgStamp));
            builder.append("\" c=\"");
            builder.append(this.m_MsgClass.toString());
            builder.append("\" s=\"");
            builder.append(this.m_MsgState.toString());
            builder.append("\" text=\"");
            builder.append(this.m_MsgText);
            builder.append("\" />");

            return builder.toString();
        }

        /**
         * NT-Write message to specified writer as xml element.
         * 
         * @param writer
         *            Log writer to write to.
         * @throws XMLStreamException
         *             Throw exception if error on writing.
         */
        public void WriteXmlWriter(XMLStreamWriter writer) throws XMLStreamException
        {
            writer.writeStartElement("msg");
        writer.writeAttribute("t", Utility.DateTimeToString(this.m_MsgStamp));
        writer.writeAttribute("c", this.m_MsgClass.toString());
        writer.writeAttribute("s", this.m_MsgState.toString());
        writer.writeCharacters(this.m_MsgText);
        writer.writeEndElement();

        return;
    }


}
}
