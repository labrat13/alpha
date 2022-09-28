using System;
using System.Collections.Generic;
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

        /**
 * NT-Get file extension from file title string
 * 
 * @param s
 *            Filename without path
 * @return Function returns file extension without leading dot.
 *         Function returns empty string if filename has not extension.
 */
        public static String getFileExtension(String s)
        {
            int pos = s.lastIndexOf(".");
            if (pos == -1)
                return "";
            // если точка - последняя в строке
            if (s.length() == (pos + 1))
                return "";
            // else
            String result = s.substring(pos + 1);
            return result;
        }


        /**
 * NT- Get filename without last extension par and dot.
 * 
 * @param s
 *            Filename without path
 * @return Function returns filename without last extension and dot.
 */
        public static String getFilenameWithoutExtension(String s)
        {
            int pos = s.lastIndexOf(".");
            if (pos == -1)
                return s;
            // else
            String result = s.substring(0, pos);
            return result;

        }

        /**
         * NT-Заменить недопустимые символы в названии файла на указанный символ
         * @param title Название файла без расширения
         * @param p Символ-замена.
         * @return Возвращает безопасное название файла
         */
        public static String ReplaceInvalidPathChars(String title, String p)
        {
            //TODO: перенести эту функцию в более правильное место по семантике.
            String result = title.replaceAll("[\\\\/:*?\"<>|]", p);

            return result;
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


        /** NT-Remove ending dot's from file title string
         * @param title file title string
         * @return Fumction returns title without ending dot's
         */
        public static String RemoveEndingDots(String title)
        {
            String s = title;
            while (s.endsWith("."))
                s = s.substring(0, s.length() - 1);

            return s;
        }

        /** 
       * NT-проверить, что строка это веб-ссылка
       * @param addr Проверяемая строка
       * @return Возвращает True, если строка это веб-ссылка, False в противном случае.
       */
        public static boolean isWebUri(String addr)
        {
            return (addr.startsWith("http:")
                    || addr.startsWith("https:")
                    || addr.startsWith("www.")
                    || addr.startsWith("ftp:")
                    || addr.startsWith("sftp:"));
        }

        /** 
         * NT-проверить, что строка это сетевой путь файла
         * @param addr Проверяемая строка
         * @return Возвращает True, если строка это веб-ссылка, False в противном случае.
         */
        public static boolean isFileUri(String addr)
        {
            return (addr.startsWith("file:"));
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
