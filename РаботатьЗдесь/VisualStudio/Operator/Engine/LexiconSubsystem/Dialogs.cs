using System;
using System.Collections.Generic;
using System.Text;
using Engine.Utility;

namespace Engine.LexiconSubsystem
{
    /// <summary>
    /// NT - Функции для обслуживания речевого диалога
    /// </summary>
    internal class Dialogs
    {

        #region Стандартные слова - константы

        // Есть соблазн сделать диалоги более живыми, добавив сюда кучу синонимов.
        // Но это надо делать осторожно.
        // Лучше синонимы обрабатывать в другом месте, а тут это жестко прошито в
        // коде и никак не изменить потом.

        // Сейчас стандартные ответы это: Да, Нет, Отмена. - они находятся и распознаются в качестве ответов.
        // Есть и другие стандартные ответы в SpeakDialogResult 
        // TODO: они пока не распознаются в качестве ответов.
        // Но потом добавятся и другие?

        // Первое слово в массиве будет выводиться пользователю в качестве образца стандартного ответа.
        // Поэтому оно должно быть с заглавной буквы, и быть кратким и емким названием.
        // Это необходимо для работы функции makeСтрокаОжидаемыхОтветов()

        /// <summary>
        /// Константа - массив стандартных слов Да
        /// </summary>
        protected internal static String[] СтандартныеДа = { "Да" };

        /// <summary>
        ///  Константа - массив стандартных слов Нет
        /// </summary>
        protected internal static String[] СтандартныеНет = { "Нет" };

        /// <summary>
        /// Константа - массив стандартных слов Отмена
        /// </summary>
        protected internal static String[] СтандартныеОтмена = { "Отмена", "отмени", "отменить" };

        /// <summary>
        /// Константа - массив стандартных слов Отложить
        /// </summary>
        protected internal static String[] СтандартныеОтложить = { "Отложить", "Отложи" };

        /// <summary>
        /// Константа - массив стандартных слов Прервать
        /// </summary>
        protected internal static String[] СтандартныеПрервать = { "Прервать", "Прерви" };

        /// <summary>
        /// Константа - массив стандартных слов Повторить
        /// </summary>
        protected internal static String[] СтандартныеПовторить = { "Повтор", "Повторить", "Повтори" };

        /// <summary>
        /// Константа - массив стандартных слов Пропустить
        /// </summary>
        protected internal static String[] СтандартныеПропустить = { "Пропуск", "Пропустить", "Пропусти" };

        /// <summary>
        /// NT-Сформировать строку напоминания стандартных ответов для вывода их пользователю.
        /// </summary>
        /// <param name="wanted">Комбинация (флагов) ожидаемых стандартных ответов</param>
        /// <returns>
        /// Возвращает строку напоминания стандартных ответов, например: [Да/Нет/Отмена].
        /// Возвращает пустую строку, если никаких стандартных ответов не выбрано.
        /// </returns>
        public static String makeСтрокаОжидаемыхОтветов(SpeakDialogResult wanted)
        {
            //check wanted valueset
            SDRHelper.CheckValue(wanted);

            // 0 если флаги не установлены, возвращаем пустую строку
            if (wanted == SpeakDialogResult.Unknown)
                return String.Empty; 

            // 1 собираем список ожидаемых ответов
            List<String> lis = new List<String>();
            // все варианты перебираем в коде

            //if(SDRHelper.HasFlag(wanted, SpeakDialogResult.Да)) - слишком много проверок получится, зато надежность выше.
            if ((wanted & SpeakDialogResult.Да) != 0)
                lis.Add(Dialogs.СтандартныеДа[0]);

            if ((wanted & SpeakDialogResult.Нет) != 0)
                lis.Add(Dialogs.СтандартныеНет[0]);

            if ((wanted & SpeakDialogResult.Отмена) != 0)
                lis.Add(Dialogs.СтандартныеОтмена[0]);

            if ((wanted & SpeakDialogResult.Отложить) != 0)
                lis.Add(Dialogs.СтандартныеОтложить[0]);

            if ((wanted & SpeakDialogResult.Прервать) != 0)
                lis.Add(Dialogs.СтандартныеПрервать[0]);

            if ((wanted & SpeakDialogResult.Повторить) != 0)
                lis.Add(Dialogs.СтандартныеПовторить[0]);

            if ((wanted & SpeakDialogResult.Пропустить) != 0)
                lis.Add(Dialogs.СтандартныеПропустить[0]);

            // если флагов не найдено, возвращаем пустую строку
            if (lis.Count == 0)
                return String.Empty;

            // 2 формируем строку ожидаемых ответов
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            //добавляем строки и разделяем их разделительным символом
            int count = lis.Count - 1;
            for (int i = 0; i < lis.Count; i++)
            {
                sb.Append(lis[i]);
                //добавим разделитель
                if (i < count)
                    sb.Append('/');
            }
            //    TODO: ранее было заменено на join(), но будет ли это быстрее? надо проверить?
            //    String[] sar = lis.ToArray();
            //String s = String.Join("/", sar);
            //sb.Append(s);

            sb.Append(']');

            lis.Clear();

            return sb.ToString();
        }
        #endregion

        #region Функции простых диалогов

        #endregion

        #region Функции Детекторы стандартных ответов

        /// <summary>
        /// NT-Проверить, что пользователь ввел ответ Да
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя</param>
        /// <returns>Функция возвращает True, если пользователь ввел положительный ответ, False во всех других случаях.</returns>
        public static bool этоДа(String текстОтветаПользователя)
        {
            return StringUtility.arrayContainsString(Dialogs.СтандартныеДа, текстОтветаПользователя.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// NT-Проверить, что пользователь ввел ответ Нет
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя</param>
        /// <returns>Функция возвращает True, если пользователь ввел отрицательный ответ, False во всех других случаях.</returns>
        public static bool этоНет(String текстОтветаПользователя)
        {
            return StringUtility.arrayContainsString(Dialogs.СтандартныеНет, текстОтветаПользователя.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// NT-Проверить, что пользователь ввел команду Отменить в качестве ответа
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя.</param>
        /// <returns>Функция возвращает True, если пользователь ввел команду Отменить вместо ответа, False во всех других случаях.</returns>
        public static bool этоОтмена(String текстОтветаПользователя)
        {
            return StringUtility.arrayContainsString(Dialogs.СтандартныеОтмена, текстОтветаПользователя.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        // Функции этоОтложить, этоПрервать, этоПовторить, этоПропустить и их управляющие слова сейчас не используются, но запланированы на будущее. 

        /// <summary>
        /// NT-Проверить, что пользователь ввел команду Отложить в качестве ответа
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя.</param>
        /// <returns>Функция возвращает True, если пользователь ввел команду Отложить, False во всех других случаях.</returns>
        public static bool этоОтложить(String текстОтветаПользователя)
        {
            return StringUtility.arrayContainsString(Dialogs.СтандартныеОтложить, текстОтветаПользователя.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// NT-Проверить, что пользователь ввел команду Прервать в качестве ответа
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя.</param>
        /// <returns>Функция возвращает True, если пользователь ввел команду Прервать, False во всех других случаях.</returns>
        public static bool этоПрервать(String текстОтветаПользователя)
        {
            return StringUtility.arrayContainsString(Dialogs.СтандартныеПрервать, текстОтветаПользователя.Trim(), StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// NT-Проверить, что пользователь ввел команду Повторить в качестве ответа
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя.</param>
        /// <returns>Функция возвращает True, если пользователь ввел команду Повторить, False во всех других случаях.</returns>
        public static bool этоПовторить(String текстОтветаПользователя)
        {
            return StringUtility.arrayContainsString(Dialogs.СтандартныеПовторить, текстОтветаПользователя.Trim(), StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// NT-Проверить, что пользователь ввел команду Пропустить в качестве ответа
        /// </summary>
        /// <param name="текстОтветаПользователя">Текст ответа пользователя.</param>
        /// <returns>Функция возвращает True, если пользователь ввел команду Пропустить, False во всех других случаях.</returns>
        public static bool этоПропустить(String текстОтветаПользователя)
        {
            return StringUtility.arrayContainsString(Dialogs.СтандартныеПропустить, текстОтветаПользователя.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// NT-Ответ это один из стандартных ответов Да Нет Отмена
        /// </summary>
        /// <param name="текстОтвета">Текст ответа от пользователя</param>
        /// <returns>
        /// Функция возвращает SpeakDialogResult код стандартного ответа, если ответ можно привести к одному из стандартных ответов.
        /// Функция возвращает код SpeakDialogResult.Unknown, если ответ нельзя привести к стандартному ответу.
        /// </returns>
        public static SpeakDialogResult этоДаНетОтмена(String текстОтвета)
        {
            // триммим текст ответа на всякий случай
            String otvet = текстОтвета.Trim();
            // если ответ один из стандартных, возвращаем соответствующий код ответа
            if (Dialogs.этоДа(otvet))
                return SpeakDialogResult.Да;
            else if (Dialogs.этоНет(otvet))
                return SpeakDialogResult.Нет;
            else if (Dialogs.этоОтмена(otvet))
                return SpeakDialogResult.Отмена;
            else
                // иначе возвращает неопределенное значение
                return SpeakDialogResult.Unknown;
        }

        /// <summary>
        /// NT-Конвертировать ответ в SpeakDialogResult
        /// </summary>
        /// <param name="text">Текст ответа от пользователя</param>
        /// <returns>
        /// Функция возвращает SpeakDialogResult код стандартного ответа, если ответ можно привести к одному из стандартных ответов.
        /// Функция возвращает код SpeakDialogResult.Unknown, если ответ нельзя привести к стандартному ответу.
        /// </returns>
        public static SpeakDialogResult ToSpeakDialogResult(String text)
        {
            // триммим текст ответа на всякий случай
            String otvet = text.Trim();
            // если ответ один из стандартных, возвращаем соответствующий код ответа
            if (Dialogs.этоДа(otvet))
                return SpeakDialogResult.Да;
            else if (Dialogs.этоНет(otvet))
                return SpeakDialogResult.Нет;
            else if (Dialogs.этоОтмена(otvet))
                return SpeakDialogResult.Отмена;
            else if (Dialogs.этоОтложить(otvet))
                return SpeakDialogResult.Отложить;
            else if (Dialogs.этоПовторить(otvet))
                return SpeakDialogResult.Повторить;
            else if (Dialogs.этоПрервать(otvet))
                return SpeakDialogResult.Прервать;
            else if (Dialogs.этоПропустить(otvet))
                return SpeakDialogResult.Пропустить;
            else
                // иначе возвращает неопределенное значение
                return SpeakDialogResult.Unknown;
        }

        #endregion

        #region *** Exit control functions ***

        /// <summary>
        /// Команды завершить Оператор.
        /// </summary>
        internal static String[] ExitAppCommands;
        // инициализируется в BCSA.Init(), поскольку надо загружать слова из файла настроек Оператора.
        //Остальные встроенные команды, ранее обрабатывавшиеся здесь, теперь загружаются из настроек Оператора из файла или БД.
        //Поскольку они в разных ОС разными командами выполняются;
        //Но их нельзя реализовать как обычные команды, поскольку они не должны запускать затем пост-обработку - они и есть пост-обработка. 

        //TODO: проверить, что в настройках есть слова команды завершения приложения.  

        /// <summary>
        /// NT-Обработать команду завершения работы приложения.
        /// </summary>
        /// <param name="query">Строка команды</param>
        /// <returns>
        /// Возвращает True для завершения работы приложения.
        /// </returns>
        public static bool isExitAppCommand(String query)
        {
            return StringUtility.arrayContainsString(Dialogs.ExitAppCommands, query.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        #endregion

    }
}
