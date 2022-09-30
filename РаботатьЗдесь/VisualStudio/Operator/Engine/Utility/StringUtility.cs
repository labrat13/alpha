using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Utility
{
    /// <summary>
    /// NR - Text utility functions 
    /// </summary>
    public class StringUtility
    {
        //TODO: port this class from Java

        /// <summary>
        /// NT-проверяет что список не содержит такой строки
        /// </summary>
        /// <param name="lis">Проверяемый список.</param>
        /// <param name="sss">Искомая строка.</param>
        /// <returns>Функция возвращает true, если искомая строка присутствует в списке, false в противном случае.</returns>
        /// <remarks>
        /// Использован именно список, а не словарь, чтобы сравнивать слова без учета регистра символов.
        /// Следует вынести эту функцию отсюда в общую библиотеку, так как она явно общего назначения. 
        /// Или как расширение списка: List'string'.Contains(string s, StringComparison mode)
        /// </remarks>
        public static bool ListNotContains(List<string> lis, string sss, StringComparison mode)
        {
            foreach (String s in lis)
                if (String.Equals(s, sss, mode)) //StringComparison.OrdinalIgnoreCase))
                    return false;
            return true;
        }

        /// <summary>
        /// NT-Проверить на допустимость значение Double, введенное пользователем.
        /// </summary>
        /// <param name="str">Текстовое значение веса</param>
        /// <param name="cultureInfo">Информация о языке</param>
        /// <returns>Возвращает true, если значение допустимо в качестве Double, false в противном случае.</returns>
        public static bool IsValidFloatFormat(string str, System.Globalization.CultureInfo cultureInfo)
        {
            //bool result = false;
            //try
            //{
            //    //это должно парситься в Double
            //    double d = Double.Parse(str, cultureInfo);
            //        result = true;
            //}
            //catch
            //{
            //    result = false;
            //}
            //return result;

            Double t;
            return Double.TryParse(str, out t);
        }

        /// <summary>
        /// Compare two strings ignore case
        /// </summary>
        /// <param name="s1">String</param>
        /// <param name="s2">String</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool StringEqualsOrdinalIgnoreCase(string s1, string s2)
        {
            return String.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// NT-Return string value or [Null] if string is null.
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Return string value or [Null] if string is null.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static String GetStringTextNull(string s)
        {
            if (s == null)
                return "[Null]";
            else return s;
        }



      /// <summary>
      /// NT-Check String.IsNullOrEmpty()
      /// </summary>
      /// <param name="s">string object</param>
      /// <returns>Returns true if string is null or empty. Returns false otherwise</returns>
        public static bool StringIsNullOrEmpty(String s)
        {
            //TODO: заменить это во всех местах, где оно встречается, эту функцию удалить.
            return String.IsNullOrEmpty(s);
        }


         /// <summary>
         /// Compare two strings
         /// </summary>
         /// <param name="s1">String</param>
         /// <param name="s2">String</param>
         /// <returns>Returns True if strings are equal, returns False otherwise.</returns>
        public static bool StringEquals(String s1, String s2)
        {
            //TODO: заменить это во всех местах, где оно встречается, эту функцию удалить.
            return s1.Equals(s2, StringComparison.Ordinal);
        }

        /**
         * NT-Create copy of specified string
         * 
         * @param s
         *            Образец для копирования.
         * @return Функция возвращает копию образца.
         */
        public static String StringCopy(String s)
        {
            String result = new String(s);
            return result;
        }

        /**
 * NT-Return formatted string for DateTime.Now
 * 
 * @return Return formatted string for DateTime.Now
 */

        public static String DateTimeNowToString()
        {
            LocalDateTime dt = LocalDateTime.now();

            return DateTimeToString(dt);
        }

        /**
         * NT-Return formatted string for specified LocalDateTime object.
         * 
         * @param dt
         *            LocalDateTime object.
         * @return Return formatted string for specified LocalDateTime object.
         */
        public static String DateTimeToString(LocalDateTime dt)
        {
            DateTimeFormatter dtf = DateTimeFormatter.ofPattern("dd.MM.yyyy HH:mm:ss", BCSA.RuCulture);
            return dtf.format(dt);
        }

        /**
 * NT-Return part of filename string for specified LocalDateTime object.
 * 
 * @param dt
 *            LocalDateTime object.
 * @return Return part of filename for specified LocalDateTime object.
 */
        public static String DateTimeToFileNameString(LocalDateTime dt)
        {
            DateTimeFormatter dtf = DateTimeFormatter.ofPattern("yyMMdd_HHmmss", BCSA.RuCulture);
            return dtf.format(dt);
        }

        /**
         * NT-Получить строку версии сборки Оператора
         * 
         * @return Returns Operator version string
         */
        public static String getOperatorVersionString()
        {
            return Engine.EngineVersionString;
        }

        /**
         * NT-Remove empty string items from source array
         * 
         * @param sar
         *            Source array with empty or null items
         * @return Result array without empty or null items
         */
        public static String[] RemoveEmptyItems(String[] sar)
        {
            int count = 0;
            // find size of result array
            for (String s : sar)
                if (!Utility.StringIsNullOrEmpty(s))
                    count++;
            // create result array
            String[] result = new String[count];
            // fill array
            count = 0;
            for (String s : sar)
                if (!Utility.StringIsNullOrEmpty(s))
                {
                    result[count] = s;
                    count++;
                }

            return result;
        }


        /**
         * NT-Split string by regex and optional remove empty elements from result
         * array
         * 
         * @param text
         *            Source string
         * @param regex
         *            regex string as described in String.split() function
         *            documentation.
         *            " " result " ";
         *            "k" "m " result "[km]" and so on...
         * @param RemoveEmptyItems
         *            if True - remove empty items from result array.
         * @return Returns array of string's
         */
        public static String[] StringSplit(
                String text,
                String regex,
                boolean RemoveEmptyItems)
        {
            String[] sar = text.split(regex);

            if (RemoveEmptyItems)
                return Utility.RemoveEmptyItems(sar);
            else return sar;
        }

        /**
         * NT-Разделить строку ключевых слов на отдельные слова по , и ;
         * 
         * @param text
         *            Входная строка
         * @return Возвращает массив ключевых слов, очищенных от разделителей и пробельных символов по краям.
         */
        public static String[] SplitCommaDelimitedString(String text)
        {
            // 1. split text to array
            String[] sar = text.split("[,;]");
            // 2. trim each string in array
            LinkedList<String> li = new LinkedList<String>();
            String t;
            for (String s : sar)
            {
                // 3. put each string in array to output list
                if (s == null)
                    continue;
                t = s.trim();
                if (t.isEmpty())
                    continue;
                li.add(t);
            }
            // 4. return list as array
            return li.toArray(new String[li.size()]);
        }

        /**
 * NT-Разделить строку ключевых слов на отдельные слова по , и ;
 * 
 * @param text
 *            Входная строка
 * @return Возвращает список ключевых слов, очищенных от разделителей и пробельных символов по краям.
 */
        public static LinkedList<String> SplitCommaDelimitedString2(String text)
        {
            // 1. split text to array
            String[] sar = text.split("[,;]");
            // 2. trim each string in array
            LinkedList<String> li = new LinkedList<String>();
            String t;
            for (String s : sar)
            {
                // 3. put each string in array to output list
                if (s == null)
                    continue;
                t = s.trim();
                if (t.isEmpty())
                    continue;
                li.add(t);
            }
            // 4. return list as array
            return li;
        }

        /**
         * NT-Faster split string at first match delimiter string
         * 
         * @param text
         *            Source string
         * @param delimiter
         *            Delimiter string as "="
         * @return Returns array of 2 parts: before and after delimiter. Returns null if delimiter not found.
         */
        public static String[] StringSplitFirstMatch(String text, String delimiter)
        {
            String[] result = null;

            int start = text.indexOf(delimiter);
            int delimiterLength = delimiter.length();
            if (start >= 0)
            {
                result = new String[2];
                result[0] = text.substring(0, start);
                result[1] = text.substring(start + delimiterLength);
            }
            // else if(start == 0)
            // {
            // result = new String[2];
            // result[0] = "";
            // result[1] = text.substring(start+delimiterLength);
            // }
            else result = null;

            return result;
        }





        /**
    * NT-Проверить что указанный массив содержит указанную строку.
    * 
    * @param array
    *            Массив строк.
    * @param sample
    *            Строка-образец для поиска.
    * @param ignoreCase
    *            Игнорировать регистр символов строки.
    * @return Возвращает True, если массив содержит указанную строку; False в
    *         противном случае.
    */
        public static boolean arrayContainsStringOrdinal(
                String[] array,
                String sample,
                boolean ignoreCase)
        {
            for (String s : array)// as foreach
                if (ignoreCase == true)
                {
                    if (sample.equalsIgnoreCase(s))
                        return true;
                }
                else
                {
                    if (sample.compareTo(s) == 0)
                        return true;
                }

            return false;
        }





        /** 
         * NT-Превратить логическое значение в русскоязычное Да или Нет.
         * @param flag Логическое значение.
         * @return Функция возвращает русскоязычное значение логического значения.
         */
        public static String BoolToДаНет(boolean flag)
        {
            if (flag == true) return "Да";
            else return "Нет";
        }

        /** NT-Распарсить строку в целое число.
         * @param str входная строка
         * @return Функция возвращает объект числа, если удалось его распарсить.
         * Функция возвращает null при любой ошибке парсинга.
         */
        public static Integer tryParseInteger(String str)
        {
            Integer result = null;
            try
            {
                result = new Integer(str);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }


      

        /////////////////////////////////////////
        ///

   









    }
}
