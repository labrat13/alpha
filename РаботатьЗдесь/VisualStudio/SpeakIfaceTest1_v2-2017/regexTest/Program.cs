using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace regexTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestRegex1();
            //ParseStringArguments("my app.exe -t -d%arg1%[56*4765] -c\"%arg2\"");

            //System.Diagnostics.Process.Start("\"C:\\Program Files\\Windows NT\\Accessories\\wordpad.exe\" C:\\Temp\\1.txt");


        }

        private static void TestRegex1()
        {
            String command = "копировать кошку в резиновую кошку";
            String pattern = "^копировать (?<file>.+) в (?<folder>.+)$";
            //String pattern = "^копировать .+$";

            bool result = Regex.IsMatch(command, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (result == true)
                Console.WriteLine("Startup success");

            Regex r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(command);
            //проверить что выполнение было успешным 
            if (m.Success)
            {
                //foreach (Capture c in m.Groups)
                //{
                //    //первый элемент - весь текст целиком. Остальные - капчи, но без имени. Имена капчей надо брать из паттерна.
                //    //если капчей нет в паттерне, то только первый элемент в списке содержится.
                //    Console.WriteLine(String.Format("{0} {1} {2}", c.Index, c.Value, c.Length));
                //}
                ////это тест получения капчей по имени капчи.
                ////если такого имени нет, возвращается пустая строка.
                //Console.WriteLine("file = " + m.Groups["file"].Value);
                //Console.WriteLine("folder = " + m.Groups["folder"].Value);

                //более компактный код, но хорошо работает только с одиночным совпадением.
                //если в тексте два и более совпадения, надо делать другой код.
                for (int i = 0; i < m.Groups.Count; i++)
                {
                    Console.WriteLine(r.GroupNameFromNumber(i) + " = " + m.Groups[i].Value);


                }

            }
            else
                Console.WriteLine("Match failed");

            //как получить имена капчей? По индексу группы
            //если такого индекса нет, возвращается пустая строка.
            String name = r.GroupNameFromNumber(0); //"0"
            name = r.GroupNameFromNumber(1);  //"file"
            name = r.GroupNameFromNumber(2);  //"folder"

            while (true)
            {
                Console.WriteLine("Input command:");
                command = Console.ReadLine();
                result = Regex.IsMatch(command, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (result == true)
                    Console.WriteLine("Match success");
                else
                    Console.WriteLine("No matches");
            }
        }

        internal static string ParseStringArguments(string query)
        {
            //распарсить строку вида: my app.exe -t -d%arg1%[56*4765] -c"%arg2"
            //аргумент начинается с % и содержит буквы или цифры, но не знаки или пробелы
            //надо найти позиции начала и конца для замены на другие.
            //или сразу разделить и заменить

            MatchEvaluator myEvaluator = new MatchEvaluator(myMatchEvaluator);

            String result = Regex.Replace(query, "%\\w+", myEvaluator);

            return result;
        }

        public static int i = 0;
        public static string myMatchEvaluator(Match match)
        {
            Console.WriteLine(match.Value);
            i++;
            return "test" + i.ToString();
        }




    }
}
