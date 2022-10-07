using System;
using System.Text;
using Engine.Utility;

namespace Engine.LogSubsystem
{
    /// <summary>
    /// NT-Объект сообщения лога
    /// </summary>
    public class LogMessage
    {
        #region *** Constants and Fields ***
        /// <summary>
        /// Event timestamp
        /// </summary>
        protected DateTime m_MsgStamp;


        /// <summary>
        /// Event class code
        /// </summary>
        protected EnumLogMsgClass m_MsgClass;

        /// <summary>
        /// Event state code
        /// </summary>
        protected EnumLogMsgState m_MsgState;

        /// <summary>
        /// Event description text
        /// </summary>
        protected String m_MsgText;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogMessage()
        {
            this.m_MsgClass = EnumLogMsgClass.Default;
            this.m_MsgState = EnumLogMsgState.Default;
            this.m_MsgText = "";
            this.m_MsgStamp = DateTime.Now;
        }

        /// <summary>
        /// NT-Parameter constructor
        /// </summary>
        /// <param name="c">Event class code</param>
        /// <param name="s">Event state code</param>
        /// <param name="text">Event text description</param>
        public LogMessage(EnumLogMsgClass c, EnumLogMsgState s, String text)
        {
            this.m_MsgClass = c;
            this.m_MsgState = s;
            this.m_MsgText = String.Copy(text);
            this.m_MsgStamp = DateTime.Now;

            return;
        }

        /// <summary>
        /// NT-Parameter constructor
        /// </summary>
        /// <param name="t">Event timestamp as come from DateTime.Now</param>
        /// <param name="c">Event class code</param>
        /// <param name="s">Event state code</param>
        /// <param name="text">Event text description</param>
        public LogMessage(DateTime t,
                EnumLogMsgClass c,
                EnumLogMsgState s,
                String text)
        {
            this.m_MsgClass = c;
            this.m_MsgState = s;
            this.m_MsgText = String.Copy(text);
            this.m_MsgStamp = t;

            return;
        }

        #region *** Properties ***
        /// <summary>
        /// Event timestamp
        /// </summary>
        public DateTime MsgTimestamp
        {
            get { return this.m_MsgStamp; }
            set { this.m_MsgStamp = value; }
        }
        /// <summary>
        /// Event class code
        /// </summary>
        public EnumLogMsgClass MsgClass
        {
            get { return this.m_MsgClass; }
            set { this.m_MsgClass = value; }
        }
        /// <summary>
        /// Event state code
        /// </summary>
        public EnumLogMsgState MsgState
        {
            get { return this.m_MsgState; }
            set { this.m_MsgState = value; }
        }
        /// <summary>
        /// Event description text
        /// </summary>
        public String MsgText
        {
            get { return this.m_MsgText; }
            set { this.m_MsgText = value; }
        }
        #endregion

        /// <summary>
        /// NT- String for debugging
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("LogMessage [MsgStamp=");
            builder.Append(this.m_MsgStamp);
            builder.Append(", MsgClass=");
            builder.Append(this.m_MsgClass);
            builder.Append(", MsgState=");
            builder.Append(this.m_MsgState);
            builder.Append(", MsgText=");
            builder.Append(this.m_MsgText);
            builder.Append("]");

            return builder.ToString();
        }

        /// <summary>
        ///  NT-Create xml string to write to log file
        /// </summary>
        /// <returns>Function returns Xml string without /n</returns>
        public String ToXmlString()
        {
            // sample line: <msg t="msg stamp" c="0" s="0" text="msg text" />
            StringBuilder builder = new StringBuilder();
            builder.Append("<msg t=\"");
            builder.Append(StringUtility.DateTimeToString(this.m_MsgStamp));
            builder.Append("\" c=\"");
            builder.Append(this.m_MsgClass.ToString());
            builder.Append("\" s=\"");
            builder.Append(this.m_MsgState.ToString());
            builder.Append("\" text=\"");
            builder.Append(this.m_MsgText);
            builder.Append("\" />");

            return builder.ToString();
        }




    }
}
