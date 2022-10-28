using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Engine.Utility
{
    
    /// <summary>
    /// NR - File utility functions
    /// </summary>
    public class FileUtility
    {
        //TODO: port this class from Java

        /// <summary>
        /// NT-Get file extension from file title string
        /// </summary>
        /// <param name="s">Filename without path</param>
        /// <returns>
        /// Function returns file extension without leading dot.
        /// Function returns empty string if filename has not extension.
        /// </returns>
        public static String getFileExtension(String s)
        {
            //TODO: Заменить все вызовы функции после портирования проекта.
            return Path.GetExtension(s);
        }

        /// <summary>
        /// NT- Get filename without last extension par and dot.
        /// </summary>
        /// <param name="s">Filename without path</param>
        /// <returns>Function returns filename without last extension and dot.</returns>
        public static String getFilenameWithoutExtension(String s)
        {
            //TODO: Заменить все вызовы функции после портирования проекта.
            return Path.GetFileNameWithoutExtension(s);
        }

        /// <summary>
        /// NT-Заменить недопустимые символы в названии файла на указанный символ
        /// </summary>
        /// <param name="title">Название файла без расширения</param>
        /// <param name="p">Символ-замена.</param>
        /// <returns>Возвращает безопасное название файла</returns>
        public static String ReplaceInvalidPathChars(String title, char p)
        {
            Char[] unsafeChars = Path.GetInvalidFileNameChars();
            StringBuilder sb = new StringBuilder(title);
            foreach (char c in unsafeChars)
                sb.Replace(c, p);

            return sb.ToString();
        }




        /// <summary>
        /// NT-Remove ending dot's from file title string
        /// </summary>
        /// <param name="title">file title string</param>
        /// <returns>Fumction returns title without ending dot's</returns>
        public static String RemoveEndingDots(String title)
        {
            return title.TrimEnd(' ', '.');
        }

        /// <summary>
        /// Static array of web path prefixes for speed optimization
        /// </summary>
        private static String[] webPrefixes = { "http:", "https:", "www.", "ftp:", "sftp:" };

        /// <summary>
        /// NT-проверить, что строка это веб-ссылка
        /// </summary>
        /// <param name="addr">Проверяемая строка</param>
        /// <returns>Возвращает True, если строка это веб-ссылка, False в противном случае.</returns>
        public static bool IsWebUri(String addr)
        {
            foreach (String s in webPrefixes)
            {
                if (addr.StartsWith(s))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// NT-проверить, что строка это сетевой путь файла
        /// </summary>
        /// <param name="addr">Проверяемая строка</param>
        /// <returns>Возвращает True, если строка это веб-ссылка, False в противном случае.</returns>
        public static bool isFileUri(String addr)
        {
            return (addr.StartsWith("file:"));
        }

        /** 
         * NR-проверить, что строка это локальный путь файла
         * @param addr Проверяемая строка
         * @return Возвращает True, если строка это веб-ссылка, False в противном случае.
         */
        public static Boolean isLocalFile(String addr)
        {
            Boolean result = true;

            //if (String.IsNullOrEmpty(addr)) return false;
            ////если это локальный путь ФС, то он должен начинаться с / или ~
            //if (addr.StartsWith("/") || addr.StartsWith("~"))
            //    return true;
            ////тут конструктор File() подставит текущий (или рабочий?) каталог приложения (.Operator), если addr не содержит путь ФС.
            ////так что оно сможет запускать все файлы, что находятся внутри каталога Оператор, по относительным путям.
            //try
            //{
            //    File f = new File(addr);
            //    result = f.exists();
            //}
            //catch (Exception ex)
            //{
            //    result = false;
            //}

            return result;
        }


        /// <summary>
        /// RT-Убедиться что файловый путь является абсолютным
        /// </summary>
        /// <param name="p">Проверяемый файловый путь, не сетевой.</param>
        /// <returns></returns>
        public static bool IsAbsolutePath(string p)
        {
            if (p == null) throw new ArgumentException("Path is null", "p");
            //если путь - пустая строка то это точно локальный путь.
            if (p == String.Empty)
                return false;
            //проверяем
            String vol = Path.GetPathRoot(p);
            //returns "" or "\" for relative path, and "C:\" for absolute path
            if (vol.Length != 3)
                return false;
            //первый символ должен быть буквой дискового тома
            return (Char.IsLetter(vol, 0));
        }













    }
}
