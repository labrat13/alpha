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
            //int pos = s.lastIndexOf(".");
            //if (pos == -1)
            //    return "";
            //// если точка - последняя в строке
            //if (s.length() == (pos + 1))
            //    return "";
            //// else
            //String result = s.substring(pos + 1);
            //return result;

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
            //int pos = s.lastIndexOf(".");
            //if (pos == -1)
            //    return s;
            //// else
            //String result = s.substring(0, pos);
            //return result;

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


        /** 
         * NT-Превратить путь к файлу в URI для вызова ShellExecute
         * 
         * @param path Путь к файлу.
         * @return Функция возвращает текстовый URI файла для использования в ShellExecute. 
         */
        public static String MakeUriFromFilePath(String path)
        {
            File f = new File(path);
            URI uri = f.toURI();
            //URL url = uri.toURL();
            String result = uri.toString();

            return result;
        }


        /// <summary>
        /// NT-Remove ending dot's from file title string
        /// </summary>
        /// <param name="title">file title string</param>
        /// <returns>Fumction returns title without ending dot's</returns>
        public static String RemoveEndingDots(String title)
        {
            //String s = title;
            //while (s.endsWith("."))
            //    s = s.substring(0, s.length() - 1);
            //return s;

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
         * NT-проверить, что строка это локальный путь файла
         * @param addr Проверяемая строка
         * @return Возвращает True, если строка это веб-ссылка, False в противном случае.
         */
        public static boolean isLocalFile(String addr)
        {
            boolean result = true;
            if (addr.isEmpty()) return false;
            //если это локальный путь ФС, то он должен начинаться с / или ~
            if (addr.startsWith("/") || addr.startsWith("~"))
                return true;
            //тут конструктор File() подставит текущий (или рабочий?) каталог приложения (.Operator), если addr не содержит путь ФС.
            //так что оно сможет запускать все файлы, что находятся внутри каталога Оператор, по относительным путям.
            try
            {
                File f = new File(addr);
                result = f.exists();
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }


        /**
* NT-Open or Create text file.
* 
* @param fpath
*            file pathname string
* @param encoding
*            Text file encoding, "UTF-8" as sample.
* @return Function returns BufferedWriter object. Close it on exit!
* @throws IOException
*             General IO error.
* @throws FileNotFoundException
*             File not found.
* @throws UnsupportedEncodingException
*             Unsupported encoding.
*/
        public static BufferedWriter FileWriterOpenOrCreate(
                String fpath,
                String encoding)
            throws IOException, FileNotFoundException,
            UnsupportedEncodingException
    {
        File f = new File(fpath);
        if (f.exists() == false)
        {
            f.createNewFile();
        }
    FileOutputStream os = new FileOutputStream(f, true);
    OutputStreamWriter osw = new OutputStreamWriter(os, encoding);
    BufferedWriter result = new BufferedWriter(osw);
        
        return result;
    }

/**
 * NT-Open Buffered Reader for read file with specified encoding.
 * 
 * @param filepath
 *            File pathname.
 * @param encoding
 *            File text encoding. For example: UTF-8 UTF-16.
 * @return Returns BufferedReader object ready for use.
 * @throws FileNotFoundException
 *             File not founded.
 * @throws UnsupportedEncodingException
 *             Wrong encoding title.
 */
public static BufferedReader openBufferedReader(String filepath, String encoding)
            throws FileNotFoundException, UnsupportedEncodingException
    {
        FileInputStream fis = new FileInputStream(filepath);
InputStreamReader isr = new InputStreamReader(fis, encoding);
BufferedReader result = new BufferedReader(isr);

return result;
    }
    
    /**
     * NT- Open Buffered Writer for write to file with specified encoding.
     * 
     * @param filepath
     *            File pathname.
     * @param encoding
     *            File text encoding. For example: UTF-8 UTF-16.
     * @return Returns BufferedWriter object ready for use.
     * @throws FileNotFoundException
     *             File cannot be created.
     * @throws UnsupportedEncodingException
     *             Wrong encoding title.
     */
    public static BufferedWriter openBufferedWriter(String filepath, String encoding)
            throws FileNotFoundException, UnsupportedEncodingException
    {
        FileOutputStream fos = new FileOutputStream(filepath);
OutputStreamWriter osw = new OutputStreamWriter(fos, encoding);
BufferedWriter result = new BufferedWriter(osw);

return result;
    }
    














    }
}
