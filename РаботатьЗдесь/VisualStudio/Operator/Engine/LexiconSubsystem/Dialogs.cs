using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.LexiconSubsystem
{
    /// <summary>
    /// NR - Функции для обслуживания речевого диалога
    /// </summary>
    internal class Dialogs
    {
        // TODO: пока класс недоопределен, в нем много лишних функций. Позже надо
        // будет их почистить.

        // #region Стандартные слова - константы
        // Есть соблазн сделать диалоги более живыми, добавив сюда кучу синонимов.
        // Но это надо делать осторожно.
        // Лучше синонимы обрабатывать в другом месте, а тут это жестко прошито в
        // коде и никак не изменить потом.

        // Сейчас стандартные ответы это: Да, Нет, Отмена. - они находятся и
        // распознаются в качестве ответов.
        // Есть и другие стандартные ответы в SpeakDialogResult
        // TODO: они пока не распознаются в качестве ответов.
        // Но потом добавятся и другие?

        // Первое слово в массиве будет выводиться пользователю в качестве образца
        // стандартного ответа.
        // Поэтому оно должно быть с заглавной буквы, и быть кратким и емким
        // названием.
        // Это необходимо для работы функции makeСтрокаОжидаемыхОтветов()

        /**
         * Константа - массив стандартных слов Да
         */
        protected static String[] СтандартныеДа = { "Да" };

        /**
         * Константа - массив стандартных слов Нет
         */
        protected static String[] СтандартныеНет = { "Нет" };

        /**
         * Константа - массив стандартных слов Отмена
         */
        protected static String[] СтандартныеОтмена = { "Отмена", "отмени", "отменить" };

        /**
         * Константа - массив стандартных слов Отложить
         */
        protected static String[] СтандартныеОтложить = { "Отложить", "Отложи" };

        /**
         * Константа - массив стандартных слов Прервать
         */
        protected static String[] СтандартныеПрервать = { "Прервать", "Прерви" };

        /**
         * Константа - массив стандартных слов Повторить
         */
        protected static String[] СтандартныеПовторить = { "Повтор", "Повторить", "Повтори" };

        /**
         * Константа - массив стандартных слов Пропустить
         */
        protected static String[] СтандартныеПропустить = { "Пропуск", "Пропустить", "Пропусти" };

        /**
         * NT-Сформировать строку напоминания стандартных ответов для вывода их
         * пользователю.
         * 
         * @param ожидаемыеОтветы
         *            Комбинация (флагов) ожидаемых стандартных ответов
         * @return Возвращает строку напоминания стандартных ответов, например:
         *         [Да/Нет/Отмена]. Возвращает пустую строку, если никаких
         *         стандартных ответов не выбрано.
         * @throws Exception
         *             Функция выбрасывает исключение, если EnumSpeakDialogResult
         *             имеет неправильные значения.
         */
        public static String makeСтрокаОжидаемыхОтветов(
                EnumSpeakDialogResult ожидаемыеОтветы) throws Exception
        {
        // 0 если флаги не установлены, возвращаем пустую строку
        if (ожидаемыеОтветы.isEqualValue(EnumSpeakDialogResult.Unknown))
            return ""; // as
                       // String.Empty

        // 1 собираем список ожидаемых ответов
        LinkedList<String> lis = new LinkedList<String>();
        // все варианты перебираем в коде
        if (ожидаемыеОтветы.hasFlag(EnumSpeakDialogResult.Да))
            lis.add(Dialogs.СтандартныеДа[0]);

        if (ожидаемыеОтветы.hasFlag(EnumSpeakDialogResult.Нет))
            lis.add(Dialogs.СтандартныеНет[0]);

        if (ожидаемыеОтветы.hasFlag(EnumSpeakDialogResult.Отмена))
            lis.add(Dialogs.СтандартныеОтмена[0]);

        if (ожидаемыеОтветы.hasFlag(EnumSpeakDialogResult.Отложить))
            lis.add(Dialogs.СтандартныеОтложить[0]);

        if (ожидаемыеОтветы.hasFlag(EnumSpeakDialogResult.Прервать))
            lis.add(Dialogs.СтандартныеПрервать[0]);

        if (ожидаемыеОтветы.hasFlag(EnumSpeakDialogResult.Повторить))
            lis.add(Dialogs.СтандартныеПовторить[0]);

        if (ожидаемыеОтветы.hasFlag(EnumSpeakDialogResult.Пропустить))
            lis.add(Dialogs.СтандартныеПропустить[0]);

        // если флагов не найдено, возвращаем пустую строку
        if (lis.size() == 0)
            return ""; // as String.Empty;

        // 2 формируем строку ожидаемых ответов
        StringBuilder sb = new StringBuilder();
        sb.append('[');
        // int count = lis.size() - 1;
        // for (int i = 0; i < lis.Count; i++)
        // {
        // sb.Append(lis[i]);
        // //добавим разделитель
        // if (i < count)
        // sb.Append('/');
        // }
        // заменено на join()
        String[] sar = lis.toArray(new String[lis.size()]);
        String s = String.join("/", sar);
        sb.append(s);
        sb.append(']');

        lis.clear();

        return sb.toString();
    }

    // #region Функции простых диалогов

    // #endregion

    // #region Функции Детекторы стандартных ответов

    /**
     * NT-Проверить, что пользователь ввел ответ Да
     * 
     * @param текстОтветаПользователя
     *            Текст ответа пользователя
     * @return Функция возвращает True, если пользователь ввел положительный
     *         ответ, False во всех других случаях.
     */
    public static boolean этоДа(String текстОтветаПользователя)
    {
        return arrayContainsStringOrdinalIgnoreCase(Dialogs.СтандартныеДа, текстОтветаПользователя.trim());
    }

    /**
     * NT-Проверить, что пользователь ввел ответ Нет
     * 
     * @param текстОтветаПользователя
     *            Текст ответа пользователя
     * @return Функция возвращает True, если пользователь ввел отрицательный
     *         ответ, False во всех других случаях.
     */
    public static boolean этоНет(String текстОтветаПользователя)
    {
        return arrayContainsStringOrdinalIgnoreCase(Dialogs.СтандартныеНет, текстОтветаПользователя.trim());
    }

    /**
     * NT-Проверить, что пользователь ввел команду Отменить в качестве ответа
     * 
     * @param текстОтветаПользователя
     *            Текст ответа пользователя
     * @return Функция возвращает True, если пользователь ввел команду Отменить
     *         вместо ответа, False во всех других случаях.
     */
    public static boolean этоОтмена(String текстОтветаПользователя)
    {
        return arrayContainsStringOrdinalIgnoreCase(Dialogs.СтандартныеОтмена, текстОтветаПользователя.trim());
    }

    // TODO: добавить тут функции этоОтложить, этоПрервать, этоПовторить,
    // этоПропустить
    // чтобы уже все это было сделано. Если они будут использоваться?

    /**
     * NT-Ответ это один из стандартных ответов
     * 
     * @param текстОтвета
     *            Текст ответа от пользователя
     * @return Функция возвращает SpeakDialogResult код стандартного ответа,
     *         если ответ можно привести к одному из стандартных ответов.
     *         Функция возвращает код SpeakDialogResult.Unknown, если ответ
     *         нельзя привести к стандартному ответу.
     */
    public static EnumSpeakDialogResult этоДаНетОтмена(String текстОтвета)
    {
        // триммим текст ответа на всякий случай
        String otvet = текстОтвета.trim();
        // если ответ один из стандартных, возвращаем соответствующий код ответа
        if (Dialogs.этоДа(otvet))
            return new EnumSpeakDialogResult(EnumSpeakDialogResult.Да);
        else if (Dialogs.этоНет(otvet))
            return new EnumSpeakDialogResult(EnumSpeakDialogResult.Нет);
        else if (Dialogs.этоОтмена(otvet))
            return new EnumSpeakDialogResult(EnumSpeakDialogResult.Отмена);
        else
            // иначе возвращает неопределенное значение
            return new EnumSpeakDialogResult(EnumSpeakDialogResult.Unknown);
    }

    // #endregion

    // #region Служебные функции

    /**
     * NT-Проверить что указанный массив содержит указанную строку.
     * Без учета регистра символов.
     * 
     * @param array
     *            Массив строк
     * @param sample
     *            Строка-образец для поиска
     * @return Возвращает True, если массив содержит указанную строку; False в
     *         противном случае.
     */
    private static boolean arrayContainsStringOrdinalIgnoreCase(
            String[] array,
            String sample)
    {
        for (String s : array)// as foreach
            if (sample.equalsIgnoreCase(s))
                return true;

        return false;
    }

    // #endregion

    // #region *** Exit control functions ***

    /**
     * Команды завершить Оператор.
     */
    static String[] ExitAppCommands;
    // инициализируется в BCSA.Init(), поскольку надо загружать слова из файла настроек Оператора.

    /**
     * NT-Обработать команду завершения работы приложения.
     * 
     * @param query
     *            Строка команды
     * @return Возвращает True для завершения работы приложения.
     */
    public static boolean isExitAppCommand(String query)
    {
        return arrayContainsStringOrdinalIgnoreCase(Dialogs.ExitAppCommands, query.trim());
    }

    // #endregion


}
}
