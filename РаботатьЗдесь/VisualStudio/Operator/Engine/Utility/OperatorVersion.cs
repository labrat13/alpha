using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Utility
{
    /// <summary>
    /// NR - Самодельный класс версии
    /// </summary>
    internal class OperatorVersion
    {
        //TODO: port from Java code this class
        // TODO: add get-set property here

        /// <summary>
        /// 
        /// </summary>
        protected int m_Version;

        /// <summary>
        /// 
        /// </summary>
        protected int m_SubVersion;

/// <summary>
/// 
/// </summary>
        protected int m_Revision;

/// <summary>
/// 
/// </summary>
        protected int m_Build;


          
         /// <summary>
         /// NT-Constructor
         /// </summary>
        public OperatorVersion()
        {
            this.m_Version = 0;
            this.m_SubVersion = 0;
            this.m_Revision = 0;
            this.m_Build = 0;
        }

        
            /// <summary>
            /// NT- Constructor
            /// </summary>
            /// <param name="ver">Major version number</param>
            /// <param name="sub">Minor version number</param>
            /// <param name="rev">Release number</param>
            /// <param name="bild">Build number</param>
            public OperatorVersion(int ver, int sub, int rev, int bild)
        {
            this.m_Version = ver;
            this.m_SubVersion = sub;
            this.m_Revision = rev;
            this.m_Build = bild;
        }

        /**
         * NT- Полное сравнение двух объектов версий по всем полям
         * 
         * @see java.lang.Comparable#compareTo(java.lang.Object)
         * @param obj
         *            Version object for comparing
         * @return Returns 1..0..-1
         */
    public int compareTo(OperatorVersion obj)
        {

            if (obj == null)
                return 1;
            // else
            int result = Integer.compare(this.m_Version, obj.m_Version);
            if (result != 0)
                return result;
            result = Integer.compare(this.m_SubVersion, obj.m_SubVersion);
            if (result != 0)
                return result;
            result = Integer.compare(this.m_Revision, obj.m_Revision);
            if (result != 0)
                return result;
            result = Integer.compare(this.m_Build, obj.m_Build);
            return result;

        }

        /**
         * NT- Частичное сравнение двух объектов версий по полям MajorVersion и MinorVersion
         * 
         * @see java.lang.Comparable#compareTo(java.lang.Object)
         * @param obj
         *            Version object for comparing
         * @return Returns 1..0..-1
         */
        public int comparePartial(Version obj)
        {
            if (obj == null)
                return 1;
            // else
            int result = Integer.compare(this.m_Version, obj.m_Version);
            if (result != 0)
                return result;
            result = Integer.compare(this.m_SubVersion, obj.m_SubVersion);

            return result;
        }

        /**
         * RT-Return typical version string like "0.18.16.29997"
         */
        
    public override String ToString()
        {
            return String.format("%d.%d.%d.%d", this.m_Version, this.m_SubVersion, this.m_Revision, this.m_Build);
        }

        /**
         * RT-Get version object from string.
         * 
         * @param s
         *            Version string.
         * @return Returns version object.
         * @throws Exception
         *             Ошибка при парсинге входной строки
         */
        public static OperatorVersion parse(String s) throws Exception
        {

        int[] v = new int[4];
        v[0] = 0;
        v[1] = 0;
        v[2] = 0;
        v[3] = 0;
        // 1. split by '.'
        String[] sar = Utility.StringSplit(s, "\\.", true);
        if ((sar.length< 1) || (sar.length > 4))
            throw new Exception("Invalid format input string");
        // 2. convert string array to int array
        for (int i = 0; i<sar.length; i++)
            v[i] = Integer.parseInt(sar[i]);
        // 3. put to Version
        Version result = new OperatorVersion(v[0], v[1], v[2], v[3]);

        return result;
    }

    /**
     * NT-Попытка преобразовать входную строку в объект Version
     * 
     * @param s
     *            Входная строка
     * @return Возвращает объект Version при успехе или null при неудаче парсинга.
     */
    public static OperatorVersion tryParse(String s)
    {
        OperatorVersion result = null;
        try
        {
            result = OperatorVersion.parse(s);
        }
        catch (Exception ex)
        {
            result = null;
        }
        return result;
    }
}
}
