using System;
using System.Collections.Generic;
using System.Text;

namespace Operator.Lexicon
{
    /// <summary>
    /// Функции для обслуживания речевого диалога
    /// </summary>
    public class Dialogs
    {
        //TODO: пока класс еще придумывается, в нем много лишних функций. Позже надо будет их почистить.
        #region Стандартные слова - константы
        //Есть соблазн сделать диалоги более живыми, добавив сюда кучу синонимов. Но это надо делать осторожно.
        //Лучше синонимы обрабатывать в другом месте, а тут это жестко прошито в коде и никак не изменить потом.

        //Сейчас стандартные ответы это: Да, Нет, Отмена. - они находятся и распознаются в качестве ответов.
        //Есть и другие стандартные ответы в SpeakDialogResult -TODO: они пока не распознаются  в качестве ответов.
        //Но потом добавятся и другие?

        //Первое слово в массиве будет выводиться пользователю в качестве образца стандартного ответа.
        //Поэтому оно должно быть с заглавной буквы, и быть кратким и емким названием.
        //Это необходимо для работы функции makeСтрокаОжидаемыхОтветов()

        /// <summary>
        /// Константа - массив стандартных слов Да
        /// </summary>
        protected static string[] СтандартныеДа = {"Да" };
        /// <summary>
        /// Константа - массив стандартных слов Нет
        /// </summary>
        protected static string[] СтандартныеНет = { "Нет"};
        /// <summary>
        /// Константа - массив стандартных слов Отмена
        /// </summary>
        protected static string[] СтандартныеОтмена = { "Отмена", "отмени", "отменить" };

        /// <summary>
        /// Константа - массив стандартных слов Отложить
        /// </summary>
        protected static string[] СтандартныеОтложить = { "Отложить", "Отложи" };

        /// <summary>
        /// Константа - массив стандартных слов Прервать
        /// </summary>
        protected static string[] СтандартныеПрервать = { "Прервать", "Прерви" };

        /// <summary>
        /// Константа - массив стандартных слов Повторить
        /// </summary>
        protected static string[] СтандартныеПовторить = { "Повтор", "Повторить", "Повтори" };

        /// <summary>
        /// Константа - массив стандартных слов Пропустить
        /// </summary>
        protected static string[] СтандартныеПропустить = { "Пропуск", "Пропустить", "Пропусти" };


        #endregion

        /// <summary>
        /// NT-Сформировать строку напоминания стандартных ответов для вывода их пользователю.
        /// </summary>
        /// <param name="ожидаемыеОтветы">Комбинация (флагов) ожидаемых стандартных ответов</param>
        /// <returns>
        /// Возвращает строку напоминания стандартных ответов, например: [Да/Нет/Отмена].
        /// Возвращает пустую строку, если никаких стандартных ответов не выбрано.
        /// </returns>
        public static String makeСтрокаОжидаемыхОтветов(SpeakDialogResult ожидаемыеОтветы)
        {
            //0 если флаги не установлены, возвращаем пустую строку
            if (ожидаемыеОтветы == SpeakDialogResult.Unknown)
                return String.Empty;

            //1 собираем список ожидаемых ответов
            List<string> lis = new List<string>();
            //все варианты перебираем в коде
            if ((ожидаемыеОтветы & SpeakDialogResult.Да) != 0)
                lis.Add(Dialogs.СтандартныеДа[0]);

            if ((ожидаемыеОтветы & SpeakDialogResult.Нет) != 0)
                lis.Add(Dialogs.СтандартныеНет[0]);

            if ((ожидаемыеОтветы & SpeakDialogResult.Отмена) != 0)
                lis.Add(Dialogs.СтандартныеОтмена[0]);

            if ((ожидаемыеОтветы & SpeakDialogResult.Отложить) != 0)
                lis.Add(Dialogs.СтандартныеОтложить[0]);

            if ((ожидаемыеОтветы & SpeakDialogResult.Прервать) != 0)
                lis.Add(Dialogs.СтандартныеПрервать[0]);

            if ((ожидаемыеОтветы & SpeakDialogResult.Повторить) != 0)
                lis.Add(Dialogs.СтандартныеПовторить[0]);

            if ((ожидаемыеОтветы & SpeakDialogResult.Пропустить) != 0)
                lis.Add(Dialogs.СтандартныеПропустить[0]);

            //если флагов не найдено, возвращаем пустую строку
            if (lis.Count == 0)
                return String.Empty; 

            //2 формируем строку ожидаемых ответов
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            int count = lis.Count - 1;
            for (int i = 0; i < lis.Count; i++)
            {
                sb.Append(lis[i]);
                //добавим разделитель
                if (i < count)
                    sb.Append('/');
            }
            sb.Append(']');
            lis.Clear();

            return sb.ToString();
        }


        #region Функции простых диалогов


        #endregion

        #region Функции Детекторы стандартных ответов
        /// <summary>
        /// NT-Проверить, что пользователь ввел ответ Да
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя</param>
        /// <returns>
        /// Функция возвращает True, если пользователь ввел положительный ответ, False во всех других случаях.
        /// </returns>
        public static bool этоДа(string текстОтветаПользователя)
        {
            return arrayContainsStringOrdinalIgnoreCase(Dialogs.СтандартныеДа, текстОтветаПользователя.Trim());
        }

        /// <summary>
        /// NT-Проверить, что пользователь ввел ответ Нет
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя</param>
        /// <returns>
        /// Функция возвращает True, если пользователь ввел отрицательный ответ, False во всех других случаях.
        /// </returns>
        public static bool этоНет(string текстОтветаПользователя)
        {
            return arrayContainsStringOrdinalIgnoreCase(Dialogs.СтандартныеНет, текстОтветаПользователя.Trim());
        }

        /// <summary>
        /// NT-Проверить, что пользователь ввел команду Отменить в качестве ответа
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя</param>
        /// <returns>
        /// Функция возвращает True, если пользователь ввел команду Отменить вместо ответа, False во всех других случаях.
        /// </returns>
        public static bool этоОтмена(string текстОтветаПользователя)
        {
            return arrayContainsStringOrdinalIgnoreCase(Dialogs.СтандартныеОтмена, текстОтветаПользователя.Trim());
        }


        /// <summary>
        /// NT-Ответ это один из стандартных ответов
        /// </summary>
        /// <param name="текстОтвета">Текст ответа от пользователя</param>
        /// <returns>
        /// Функция возвращает <c>SpeakDialogResult</c> код стандартного ответа, если ответ можно привести к одному из стандартных ответов.
        /// Функция возвращает код <c>SpeakDialogResult.Unknown</c>, если ответ нельзя привести к стандартному ответу.
        /// </returns>
        public static SpeakDialogResult этоДаНетОтмена(string текстОтвета)
        {
            //триммим текст ответа на всякий случай
            String otvet = текстОтвета.Trim();
            //если ответ один из стандартных, возвращаем соответствующий код ответа
            if (Dialogs.этоДа(otvet))
                return SpeakDialogResult.Да;
            else if (Dialogs.этоНет(otvet))
                return SpeakDialogResult.Нет;
            else if (Dialogs.этоОтмена(otvet))
                return SpeakDialogResult.Отмена;

            //иначе возвращает неопределенное значение
            return SpeakDialogResult.Unknown;
        }

        #endregion

        #region Служебные функции
        /// <summary>
        /// NT-Проверить что указанный массив содержит указанную строку.
        /// Без учета регистра символов.
        /// </summary>
        /// <param name="array">Массив строк</param>
        /// <param name="sample">Строка-образец для поиска</param>
        /// <returns>Возвращает True, если массив содержит указанную строку; False в противном случае.</returns>
        internal static bool arrayContainsStringOrdinalIgnoreCase(string[] array, string sample)
        {
            foreach (String s in array)
                if (String.Equals(s, sample, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }


        #endregion

        #region *** Exit control functions ***
        /// <summary>
        /// Команды завершить Оператор
        /// </summary>
        private static string[] ExitAppCommands = { "выход", "выйти", "закрыть", "quit", "close", "exit" };
        /// <summary>
        /// Команды перейти в спящий режим
        /// </summary>
        private static string[] ExitSleepCommands = { "спать", "спи", "sleep" };
        /// <summary>
        /// Команды перезагрузить компьютер
        /// </summary>
        private static string[] ExitReloadCommands = {"перезагрузить", "перезагрузись", "перезагрузка", "перезагрузить компьютер", "reload", "restart" };
        /// <summary>
        /// Команды выключить компьютер
        /// </summary>
        private static string[] ExitShutdownCommands = {"выключись", "выключайся", "выключить компьютер", "poweroff", "shutdown" };
        /// <summary>
        /// Команды завершить сеанс текущего пользователя
        /// </summary>
        private static string[] ExitLogoffCommands = {"завершить сеанс", "завершение сеанса", "logoff" };

        /// <summary>
        /// NT-Обработать команду завершения работы приложения.
        /// </summary>
        /// <param name="cmdline">Строка команды</param>
        /// <returns>Возвращает True для завершения работы приложения.</returns>
        internal static bool isExitAppCommand(string query)
        {
            return arrayContainsStringOrdinalIgnoreCase(Dialogs.ExitAppCommands, query.Trim());
        }

        /// <summary>
        /// NT-Обработать команду завершения работы машины.
        /// </summary>
        /// <param name="cmdline">Строка команды</param>
        /// <returns>Возвращает True для завершения работы приложения.</returns>
        internal static bool isSleepCommand(string query)
        {
            return arrayContainsStringOrdinalIgnoreCase(Dialogs.ExitSleepCommands, query.Trim());
        }

        /// <summary>
        /// NT-Обработать команду завершения работы машины.
        /// </summary>
        /// <param name="cmdline">Строка команды</param>
        /// <returns>Возвращает True для завершения работы приложения.</returns>
        internal static bool isExitReloadCommand(string query)
        {
            return arrayContainsStringOrdinalIgnoreCase(Dialogs.ExitReloadCommands, query.Trim());
        }

        /// <summary>
        /// NT-Обработать команду завершения работы машины.
        /// </summary>
        /// <param name="cmdline">Строка команды</param>
        /// <returns>Возвращает True для завершения работы приложения.</returns>
        internal static bool isExitShutdownCommand(string query)
        {
            return arrayContainsStringOrdinalIgnoreCase(Dialogs.ExitShutdownCommands, query.Trim());
        }

        /// <summary>
        /// NT-Обработать команду завершения работы машины.
        /// </summary>
        /// <param name="cmdline">Строка команды</param>
        /// <returns>Возвращает True для завершения работы приложения.</returns>
        internal static bool isExitLogoffCommand(string query)
        {
            return arrayContainsStringOrdinalIgnoreCase(Dialogs.ExitLogoffCommands, query.Trim());
        }
        #endregion

    }
}
