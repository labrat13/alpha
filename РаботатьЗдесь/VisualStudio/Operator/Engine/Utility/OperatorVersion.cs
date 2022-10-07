using System;

namespace Engine.Utility
{
    /// <summary>
    /// NT - Самодельный класс версии
    /// </summary>
    public class OperatorVersion
    {

        /// <summary>
        /// Major version number
        /// </summary>
        protected int m_Version;

        /// <summary>
        /// Minor version number
        /// </summary>
        protected int m_SubVersion;

        /// <summary>
        /// Revision number
        /// </summary>
        protected int m_Revision;

        /// <summary>
        /// Build number
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

        /// <summary>
        /// NT-Constructor
        /// </summary>
        /// <param name="v">NET Framework Version object</param>
        public OperatorVersion(Version v)
        {
            this.m_Version = v.Major;
            this.m_SubVersion = v.Minor;
            this.m_Revision = v.Revision;
            this.m_Build = v.Build;
        }

        #region *** Properties ***
        /// <summary>
        /// Major version number
        /// </summary>
        public int Version { get => this.m_Version; set => this.m_Version = value; }
        /// <summary>
        /// Minor version number
        /// </summary>
        public int SubVersion { get => this.m_SubVersion; set => this.m_SubVersion = value; }
        /// <summary>
        /// Revision number
        /// </summary>
        public int Revision { get => this.m_Revision; set => this.m_Revision = value; }
        /// <summary>
        /// Build number
        /// </summary>
        public int Build { get => this.m_Build; set => this.m_Build = value; }

        #endregion

        /// <summary>
        /// NT- Полное сравнение двух объектов версий по всем полям
        /// </summary>
        /// <param name="obj">OperatorVersion object for comparing</param>
        /// <returns> Returns 1..0..-1</returns>        
        public int compareTo(OperatorVersion obj)
        {

            if (obj == null)
                return 1;
            // else
            int result = this.m_Version.CompareTo(obj.m_Version);
            if (result != 0)
                return result;
            result = this.m_SubVersion.CompareTo(obj.m_SubVersion);
            if (result != 0)
                return result;
            result = this.m_Revision.CompareTo(obj.m_Revision);
            if (result != 0)
                return result;
            result = this.m_Build.CompareTo(obj.m_Build);
            return result;

        }

        /// <summary>
        /// NT- Частичное сравнение двух объектов версий по полям MajorVersion и MinorVersion
        /// </summary>
        /// <param name="obj">OperatorVersion object for comparing</param>
        /// <returns>Returns 1..0..-1</returns>
        public int comparePartial(OperatorVersion obj)
        {
            if (obj == null)
                return 1;
            // else
            int result = this.m_Version.CompareTo(obj.m_Version);
            if (result != 0)
                return result;
            result = this.m_SubVersion.CompareTo(obj.m_SubVersion);

            return result;
        }

        /// <summary>
        /// RT-Return typical version string like "0.18.16.29997"
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return this.m_Version.ToString() + "." + this.m_SubVersion.ToString() + "." + this.m_Revision.ToString() + "." + this.m_Build.ToString();
        }

        /// <summary>
        /// RT-Get version object from string.
        /// </summary>
        /// <param name="s">version string.</param>
        /// <returns>Returns version object.</returns>
        /// <exception cref="Exception">Входная строка имеет неправильный формат.</exception>
        public static OperatorVersion parse(String s)
        {
            //TODO: тут заменить исключения на правильный тип и передать входную строку  с ним.
            int[] v = new int[4];
            v[0] = 0;
            v[1] = 0;
            v[2] = 0;
            v[3] = 0;
            OperatorVersion result;
            try
            {
                // 1. split by '.'
                String[] sar = s.Split('.');
                if ((sar.Length < 1) || (sar.Length > 4))
                    throw new Exception("Invalid format of OperatorVersion string");
                // 2. convert string array to int array
                for (int i = 0; i < sar.Length; i++)
                    v[i] = Int32.Parse(sar[i]);
                // 3. put to Version
                result = new OperatorVersion(v[0], v[1], v[2], v[3]);
            }
            catch (Exception ex)
            {
                //re-throw exception to caller
                throw new Exception("Invalid format of OperatorVersion string", ex);
            }
            return result;
        }

        /// <summary>
        /// NT-Попытка преобразовать входную строку в объект OperatorVersion
        /// </summary>
        /// <param name="s">Входная строка</param>
        /// <returns>Возвращает объект OperatorVersion при успехе или null при неудаче парсинга.</returns>
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
