using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.LexiconSubsystem;

namespace Engine.Utility
{
    /// <summary>
    /// NR - Text utility functions 
    /// </summary>
    public class StringUtility
    {
        //DONE: port this class from Java - но есть тодо и недоделки.

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

        /// <summary>
        /// NT-Create copy of specified string
        /// </summary>
        /// <param name="s">Образец для копирования.</param>
        /// <returns>Функция возвращает копию образца.</returns>
        public static String StringCopy(String s)
        {
            ////TODO: заменить все вызовы в коде проекта на String.Copy(s) после окончания портирования
            String result = String.Copy(s);
            return result;
        }


        #region *** DateTime formatting ***
        /// <summary>
        /// NT-Return formatted string for DateTime.Now
        /// </summary>
        /// <returns>Return formatted string for DateTime.Now</returns>
        public static String DateTimeNowToString()
        {
            DateTime dt = DateTime.Now;

            return DateTimeToString(dt);
        }
        /// <summary>
        ///  NT-Return formatted string for specified DateTime object.
        /// </summary>
        /// <param name="dt">LocalDateTime object.</param>
        /// <returns>Return formatted string for specified DateTime object.</returns>
        public static String DateTimeToString(DateTime dt)
        {
            return dt.ToString("dd.MM.yyyy HH:mm:ss", BCSA.RuCulture);
        }

        /// <summary>
        /// NT-Return part of filename string for specified LocalDateTime object.
        /// </summary>
        /// <param name="dt">DateTime object.</param>
        /// <returns>Return part of filename for specified DateTime object.</returns>
        public static String DateTimeToFileNameString(DateTime dt)
        {
            return dt.ToString("yyMMdd_HHmmss", BCSA.RuCulture);
        }

        /// <summary>
        /// RT-Форматировать дату и время в русской культуре.
        /// Пример: воскресенье, 26 апреля 2020г. 01:03:18
        /// </summary>
        /// <param name="dt">дата и время</param>
        /// <returns>Функция возвращает строку даты и времени.</returns>
        public static String CreateLongDatetimeString(DateTime dt)
        {
            return dt.ToString("dddd, d MMMM yyyy'г. 'HH:mm:ss", BCSA.RuCulture);
        }
        #endregion

        ///**
        // * NT-Получить строку версии сборки Оператора
        // * 
        // * @return Returns Operator version string
        // */
        //public static String getOperatorVersionString()
        //{
        //    return Engine.EngineVersionString;
        //}

        /// <summary>
        /// NT-Remove empty string items from source array
        /// </summary>
        /// <param name="sar">Source array with empty or null items</param>
        /// <returns>Result array without empty or null items</returns>
        public static String[] RemoveEmptyItems(String[] sar)
        {
            List<String> result = new List<string>();
            // create result array
            foreach (String s in sar)
                if (!String.IsNullOrEmpty(s))
                    result.Add(s);

            return result.ToArray();
        }

        //TODO: выбрать все использования и написать правильный код этой функции.
        ///**
        // * NT-Split string by regex and optional remove empty elements from result
        // * array
        // * 
        // * @param text
        // *            Source string
        // * @param regex
        // *            regex string as described in String.split() function
        // *            documentation.
        // *            " " result " ";
        // *            "k" "m " result "[km]" and so on...
        // * @param RemoveEmptyItems
        // *            if True - remove empty items from result array.
        // * @return Returns array of string's
        // */
        //public static String[] StringSplit(
        //        String text,
        //        String regex,
        //        boolean RemoveEmptyItems)
        //{
        //    String[] sar = text.split(regex);

        //    if (RemoveEmptyItems)
        //        return Utility.RemoveEmptyItems(sar);
        //    else return sar;
        //}


        /// <summary>
        /// Delimiter char static array for optimize speed
        /// </summary>
        private static char[] splitChars1 = { ',', ';' };
        /// <summary>
        /// NT-Разделить строку ключевых слов на отдельные слова по , и ;
        /// </summary>
        /// <param name="text">Входная строка</param>
        /// <returns>Возвращает список ключевых слов, очищенных от разделителей и пробельных символов по краям.</returns>
        public static List<String> SplitCommaDelimitedString2(String text)
        {
            // 1. split text to array with allow empty items
            String[] sar = text.Split(splitChars1, StringSplitOptions.None);
            // 2. trim each string in array
            List<String> li = new List<String>();
            String t;
            foreach (String s in sar)
            {
                // 3. put each string in array to output list if not empty
                t = s.Trim();
                if (t.Length == 0)
                    continue;
                li.Add(t);
            }
            // 4. return list as array
            return li;
        }

        //TODO: Заменить все случаи на вызовы SplitCommaDelimitedString2(text); после портирования проекта.
        /// <summary>
        /// NT-Разделить строку ключевых слов на отдельные слова по , и ;
        /// </summary>
        /// <param name="text">Входная строка</param>
        /// <returns>Возвращает массив ключевых слов, очищенных от разделителей и пробельных символов по краям.</returns>
        public static String[] SplitCommaDelimitedString(String text)
        {
            List<String> result = SplitCommaDelimitedString2(text);
            // 4. return list as array
            return result.ToArray();
        }


        /// <summary>
        /// NT-Faster split string at first match delimiter string
        /// </summary>
        /// <param name="text">Source string</param>
        /// <param name="delimiter">Delimiter string like "="</param>
        /// <param name="comp">String comparison mode</param>
        /// <returns>Returns array of 2 parts: before and after delimiter. Returns null if delimiter not found.</returns>
        public static String[] StringSplitFirstMatch(String text, String delimiter, StringComparison comp)
        {
            String[] result = null;

            int start = text.IndexOf(delimiter, comp);
            int delimiterLength = delimiter.Length;
            if (start >= 0)
            {
                result = new String[2];
                result[0] = text.Substring(0, start);
                result[1] = text.Substring(start + delimiterLength);
            }
            else result = null;

            return result;
        }






        /// <summary>
        /// NT-Проверить что указанный массив содержит указанную строку.
        /// </summary>
        /// <param name="array">Массив строк.</param>
        /// <param name="sample">Строка-образец для поиска.</param>
        /// <param name="ignoreCase">Игнорировать регистр символов строки.</param>
        /// <returns>Возвращает True, если массив содержит указанную строку; False в противном случае.</returns>
        public static bool arrayContainsStringOrdinal(
                String[] array,
                String sample,
                bool ignoreCase)
        {
            foreach (String s in array)
                if (String.Compare(s, sample, ignoreCase) == 0)
                    return true;

            return false;
        }





        //TODO: перенести в BCSA после портирования.
        /// <summary>
        /// NT-Превратить логическое значение в русскоязычное Да или Нет.
        /// </summary>
        /// <param name="flag">Логическое значение.</param>
        /// <returns>Функция возвращает русскоязычное значение логического значения.</returns>
        public static String BoolToДаНет(bool flag)
        {
            if (flag == true) return "Да";
            else return "Нет";
        }

        /// <summary>
        /// NT-Распарсить строку в целое число.
        /// </summary>
        /// <param name="str">входная строка</param>
        /// <returns>
        /// Функция возвращает Nullable(Int32) объект числа, если удалось его распарсить.
        /// Функция возвращает null при любой ошибке парсинга.
        /// </returns>
        public static Int32? tryParseInteger(String str)
        {
            Int32? result = null;
            try
            {
                result = Int32.Parse(str, BCSA.RuCulture);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// NT-Распарсить строку в логическое значение.
        /// </summary>
        /// <param name="str">входная строка</param>
        /// <returns>
        /// Функция возвращает Nullable(bool) объект значения, если удалось его распарсить.
        /// Функция возвращает null при любой ошибке парсинга.
        /// </returns>
        public static Boolean? tryParseBoolean(String str)
        {
            //TODO: добавить распознавание значений Да и Нет в любом регистре.
            Boolean? result = null;
            try
            {
                result = Boolean.Parse(str );
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

    }
}
