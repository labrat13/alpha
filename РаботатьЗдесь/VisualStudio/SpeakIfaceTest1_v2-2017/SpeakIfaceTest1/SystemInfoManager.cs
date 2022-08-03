
using System;
namespace Operator
{
    /// <summary>
    /// Менеджер информации о операционной системе
    /// </summary>
    public static class SystemInfoManager
    {
        /// <summary>
        /// List of all operating systems
        /// </summary>
        public enum OsType
        {
            /// <summary>
            /// Windows 95/98, NT4.0, unknown OS
            /// </summary>
            Unknown,
            ///<summary>
            ///  Windows 2000
            ///</summary>
            Windows2000,
            ///<summary>
            /// Windows XP x86
            ///</summary>
            WindowsXP,
            ///<summary>
            /// Windows XP x64
            ///</summary>
            WindowsXP64,
            ///<summary>
            /// Windows Vista
            ///</summary>
            WindowsVista,
            ///<summary>
            /// Windows 7
            ///</summary>
            Windows7,
            ///<summary>
            /// Windows 8
            ///</summary>
            Windows8,
            ///<summary>
            /// Windows 8
            ///</summary>
            Windows81,
            ///<summary>
            /// Windows 10
            ///</summary>
            Windows10,
            ///<summary>
            /// Windows 2003 Server
            ///</summary>
            Windows2003,
            ///<summary>
            /// Windows 2003 R2 Server
            ///</summary>
            Windows2003R2,
            ///<summary>
            /// Windows 2008 Server
            ///</summary>
            Windows2008,
            ///<summary>
            /// Windows 2008 R2 Server
            ///</summary>
            Windows2008R2,
            ///<summary>
            /// Windows 2012 Server
            ///</summary>
            Windows2012,
            ///<summary>
            /// Windows 2012 R2 Server
            ///</summary>
            Windows2012R2,
            ///<summary>
            /// Windows 2016 Server
            ///</summary>
            Windows2016
        }

        /// <summary>
        /// NT-Определить версию ОС.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Функция приблизительно определяет версию ОС, не различает десктопную и серверную версии.
        /// Но для Оператора этого должно быть достаточно.
        /// </remarks>
        public static OsType detectOsType()
        {
            int major = Environment.OSVersion.Version.Major;
            int minor = Environment.OSVersion.Version.Minor;

            if (major < 5) return OsType.Unknown;
            else if (major == 5)
            {
                if (minor == 0) return OsType.Windows2000;
                else if (minor == 1) return OsType.WindowsXP;
                else if (minor == 2) return OsType.Windows2003;
                else return OsType.Unknown;
            }
            else if (major == 6)
            {
                if (minor == 0) return OsType.WindowsVista;
                else if (minor == 1) return OsType.Windows7;
                else if (minor == 2) return OsType.Windows8;
                else if (minor == 3) return OsType.Windows81;
                else return OsType.Unknown;
            }
            else if (major == 10)
                return OsType.Windows10;

            else return OsType.Unknown;
        }


        /// <summary>
        /// Функция возвращает True если операционная система - Windows XP.
        /// </summary>
        /// <returns></returns>
        public static bool isWindowsXP()
        {
            if ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor == 1))
                return true;
            else return false;
        }
        /// <summary>
        /// Функция возвращает True если операционная система 64-битная.
        /// </summary>
        /// <returns></returns>
        public static bool is64bitProcess()
        {
            return (IntPtr.Size == 8); 
        }

    }

}
