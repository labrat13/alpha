using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NT - User query text as object.
    /// </summary>
    public class UserQuery
    {
        // Поскольку класс используется в Процедурах, то важно минимизировать число связанных с ним классов.
        // Например, RegexManager не следует использовать внутри данного класса - следует реализовать операции с ним вне класса.

        #region *** Constants and Fields ***
        /// <summary>
        /// Текст запроса
        /// </summary>
        protected String m_Query;

        #endregion

        /// <summary>
        /// RT- default constructor
        /// </summary>
        /// <param name="q">Текст запроса.</param>
        public UserQuery(String q)
        {
            this.m_Query = q;
        }

        #region *** Properties ***
        /// <summary>
        /// RT- Query text
        /// </summary>
        public String Query
        {
            get { return this.m_Query; }
            internal set { this.m_Query = value; }
        }

        #endregion

        #region *** Service functions ***
        /// <summary>
        /// RT-Returns string representation of object.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return GetNullOrString(this.m_Query);
        }

        /// <summary>
        /// RT-Get String with null as String.
        /// </summary>
        /// <param name="val">String object.</param>
        /// <returns>String object.</returns>
        public static String GetNullOrString(String val)
        {
            if (val == null)
                return "[Null]";
            else return val;
        }

        /// <summary>
        /// NT- Trim user query text
        /// </summary>
        public void Trim()
        {
            if (this.m_Query != null)
                this.m_Query = this.m_Query.Trim();

            return;
        }

        /// <summary>
        /// RT-Return True if query is null.
        /// </summary>
        /// <returns>Return True if query is null.</returns>
        public bool isNull()
        {
            return (this.m_Query == null);
        }

        /// <summary>
        /// RT-Return true if query is nullor empty string.
        /// </summary>
        /// <returns>Return true if query is nullor empty string.</returns>
        public bool isNullOrEmpty()
        {
            return String.IsNullOrEmpty(this.m_Query);
        }

        /// <summary>
        /// NT- Change query text and write message to log.
        /// </summary>
        /// <param name="en">Current Engine object reference.</param>
        /// <param name="q">New text of query.</param>
        /// <exception cref="Exception">Error on log writing</exception>
        public void ChangeQuery(Engine en, String q)
        {
            //// TODO: patch code here
            // записать в лог старое значение и новое значение запроса
            String old = GetNullOrString(this.m_Query);
            StringBuilder sb = new StringBuilder();
            sb.Append("Изменен текст запроса: \"");
            sb.Append(old);
            sb.Append("\" заменен на \"");
            sb.Append(q);
            sb.Append("\".");
            // записать в лог сообщение
            en.LogManager.AddMessage(LogSubsystem.EnumLogMsgClass.QueryReplaced, LogSubsystem.EnumLogMsgState.OK, sb.ToString());
            // изменить значение запроса
            this.m_Query = q;

            return;
        }

        #endregion
    }
}
