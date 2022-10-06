using System;
using System.IO;

namespace Engine.Utility
{
    /// <summary>
    /// NR - Менеджер информации о операционной системе
    /// </summary>
    internal static class SystemInfoManager
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

        /// <summary>
        /// NR-Get Operating system name
        /// </summary>
        /// <returns>Operating system name</returns>
        public static String GetOsTitle()
        {
            return Environment.OSVersion.Platform.ToString();//TODO: выяснить что тут такое
        }

        /// <summary>
        /// NR-Get Operating system architecture
        /// </summary>
        /// <returns>Operating system architecture</returns>
        public static String GetOsArchTitle()
        {
            return Environment.OSVersion.Platform.ToString();//TODO: выяснить что тут такое
        }

        /// <summary>
        /// Get Operating system version
        /// </summary>
        /// <returns>Operating system version</returns>
        public static String GetOsVersionString()
        {
            return Environment.OSVersion.Version.ToString();
        }

        /// <summary>
        /// Get Operating system version
        /// </summary>
        /// <returns>Operating system version</returns>
        public static String GetOsVersionString2()
        {
            return Environment.OSVersion.VersionString;
        }

        /// <summary>
        /// NT-Get Line separator ("\n" on UNIX)
        /// </summary>
        /// <returns>Line separator ("\n" on UNIX)</returns>
        public static String GetLineSeparator()
        {
            return Environment.NewLine;
        }



        /// <summary>
        /// NT-Get File separator ("/" on UNIX)
        /// </summary>
        /// <returns>File separator ("/" on UNIX)</returns>
        public static String GetFileSeparator()
        {
            return new string(Path.PathSeparator, 1);
        }

        /// <summary>
        /// NR-Get User's account name
        /// </summary>
        /// <returns>User's account name</returns>
        public static String GetUserTitle()
        {
            //TODO: get User account name
            //return System.getProperty("user.name");
            return null;
        }


        /// <summary>
        /// Get User's home directory path
        /// </summary>
        /// <returns>User's home directory</returns>
        public static String GetUserHomeFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        /// <summary>
        /// Get User's current working directory
        /// </summary>
        /// <returns>User's current working directory</returns>
        public static String GetUserCurrentDirectory()
        {
            return Environment.CurrentDirectory;
        }

        ///**
        // * Get Java Runtime Environment version
        // * 
        // * @return Java Runtime Environment version
        // */
        //public static String GetJREVersion()
        //{
        //    return System.getProperty("user.dir");
        //}





    }
}
