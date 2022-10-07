using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.LexiconSubsystem
{
    /// <summary>
    /// Представляет консоль Оператора для всех процедур и внутренних функций движка и всего приложения.
    /// </summary>
    public class DialogConsole
    {
        // TODO: Доделать и отладить функции Terminal терминала - а то сейчас они не все
        // работают.
        // TODO: Удалить лишние переменные и функции (закомментить пока), заменить
        // обратно видимость с public на package (вместо internal)

        /**
         * Обратная ссылка на объект движка
         */
        private Engine m_Engine;

        /**
         * Constructor
         */
        public DialogConsole()
        {
            this.m_Engine = null;
            return;
        }

        /**
         * Constructor
         * 
         * @param engine
         *            Engine object
         */
        public DialogConsole(Engine engine)
        {
            this.m_Engine = engine;

            return;
        }

        /**
         * NT-Установить обычный цвет текста консоли
         */
        private void ResetColors()
        {
            Terminal.ClearAttributeMode();
        }

        /**
         * NT-Установить указанный цвет текста консоли.
         * Консоль должна печатать этим цветом весь последующий текст, включая
         * промпт и ввод пользователя.
         * 
         * @param color
         *            Цвет для текста консоли
         */
        private void SetTextColor(EnumDialogConsoleColor color)
        {
            Terminal.SetAttributeMode(color.getValue());

            return;
        }

        /**
         * NT-Подать короткий звуковой сигнал
         */
        public void Beep()
        {
            Terminal.Beep();

            return;
        }

        /**
         * NT-Поместить курсор консоли в начало новой строки, если он не там.
         */
        public void SureConsoleCursorStart()
        {
            // //TODO: getCursorPos() needed
            // if (Console.CursorLeft > 0)
            // Console.WriteLine();

            // Поскольку Terminal.getCursorPos() не реализован ввиду проблем (см
            // выше), то просто переводим курсор на новую строку.
            // Это будет коряво, но работать будет.
            Terminal.WriteLine();

            return;
        }

        /**
         * NT-вывести на консоль текст (Console.Write())
         * 
         * @param text
         *            Текст сообщения
         * @param color
         *            Цвет сообщения
         */
        public void PrintText(String text, EnumDialogConsoleColor color)
        {
            this.SetTextColor(color);
            Terminal.Write(text);
            this.ResetColors();

            return;
        }

        /**
         * NT-Вывести на консоль пустую строку
         */
        public void PrintEmptyLine()
        {
            Terminal.WriteLine();

            return;
        }

        /**
         * NT-вывести на консоль строку текста (Console.WriteLine())
         * 
         * @param text
         *            Текст сообщения
         * @param color
         *            Цвет сообщения
         */
        public void PrintTextLine(String text, EnumDialogConsoleColor color)
        {
            // если текст - пустая строка, то незачем цвета переключать
            if (Utility.StringIsNullOrEmpty(text))
                Terminal.WriteLine();
            else
            {
                this.SetTextColor(color);
                Terminal.WriteLine(text);
                this.ResetColors();
            }

            return;
        }

        /// <summary>
        /// NT-вывести массив строк сообщения.
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <param name="lines">Массив строк сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        /**
         * NT-вывести массив строк сообщения. Это типа оптимизация - экономит
         * переключения цвета текста консоли
         * 
         * @param lines
         *            Массив строк сообщения
         * @param color
         *            Цвет сообщения
         */
        public void PrintTextLines(String[] lines, EnumDialogConsoleColor color)
        {
            this.SetTextColor(color);
            for (String text : lines)
                Terminal.WriteLine(text);
            this.ResetColors();

            return;
        }

        /**
         * NR-Получить строку ввода с консоли
         * 
         * @return Функция возвращает введенный с консоли текст
         */
        public String ReadLine()
        {
            this.SetTextColor(EnumDialogConsoleColor.ВводПользователя);
            String result = Terminal.ReadLine();
            this.ResetColors();

            return result;
        }

        /**
         * NT-вывести на консоль вопрос и принять ответ
         * 
         * @param keys
         *            Набор флагов ожидаемых субкоманд
         * @param question
         *            Текст вопроса пользователю
         * @param newLine
         *            True - Начинать ответ с новой строки, False - в той же строке.
         * @param noEmptyAnswer
         *            True - требовать повторный ввод, если ответ пустая строка;
         *            False - принимать пустые ответы
         * @return Возвращает строку, введенную пользователем.
         * @throws Exception
         *             Функция выбрасывает исключение, если параметр keys имеет неправильные значения.
         */
        public String PrintQuestionAnswer(
                EnumSpeakDialogResult keys,
                String question,
                boolean newLine,
                boolean noEmptyAnswer) throws Exception
        {
        this.SureConsoleCursorStart();// убедиться что курсор находится в начале
                                      // строки
        // установим цвет вопросов в консоли
        this.SetTextColor(EnumDialogConsoleColor.Вопрос);
        Terminal.Write(question);
        Terminal.Write(" ");// разделитель для секции []
        Terminal.Write(Dialogs.makeСтрокаОжидаемыхОтветов(keys));
        this.ResetColors();
        // если указан флаг, вывести символ конца строки
        if (newLine)
            Terminal.WriteLine();
        String result = ""; // String.Empty;
        do
        {
            result = this.ReadLine();
            if (result == null)
                result = ""; // String.Empty;//случается, если нажата Ctrl+C
            else result = result.trim();// удалить пробелы и всякие там
                                        // случайности
            // no empty answers
            if ((noEmptyAnswer == false) && (Utility.StringIsNullOrEmpty(result)))
                break;// answer
                      // can
                      // be
                      // empty
        }
        while (Utility.StringIsNullOrEmpty(result));

        return result;
    }

/**
 * NT-вывести на консоль вопрос и принять ответ.
 * Оболочка для упрощения кода диалогов, принимает комбинацию флагов как int.
 * 
 * @param SpeakDialogResultKeys
 *            Набор флагов ожидаемых субкоманд как int.
 * @param question
 *            Текст вопроса пользователю
 * @param newLine
 *            True - Начинать ответ с новой строки, False - в той же строке.
 * @param noEmptyAnswer
 *            True - требовать повторный ввод, если ответ пустая строка;
 *            False - принимать пустые ответы
 * @return Возвращает строку, введенную пользователем.
 * @throws Exception
 *             Функция выбрасывает исключение, если параметр keys имеет неправильные значения.
 */
public String PrintQuestionAnswer(
        int SpeakDialogResultKeys,
        String question,
        boolean newLine,
        boolean noEmptyAnswer) throws Exception
{
    EnumSpeakDialogResult esdr = new EnumSpeakDialogResult(SpeakDialogResultKeys);

return this.PrintQuestionAnswer(esdr, question, newLine, noEmptyAnswer);
    }

    /**
     * NT-Диалог Да-Нет-Отменить. Другие ответы не принимаются.
     * 
     * @param question
     *            Текст вопроса пользователю, без символа перевода строки в
     *            конце!
     * @return Функция возвращает SpeakDialogResult код стандартного ответа Да,
     *         Нет или Отмена.
     * @throws Exception
     *             Функция выбрасывает исключение от Dialogs.makeСтрокаОжидаемыхОтветов().
     */
    public EnumSpeakDialogResult PrintДаНетОтмена(String question)
            throws Exception
{
        this.SureConsoleCursorStart();// убедиться что курсор находится в начале
                                      // строки
        // установим цвет вопросов в консоли
        this.SetTextColor(EnumDialogConsoleColor.Вопрос);
    Terminal.Write(question);
    Terminal.Write(" ");// разделитель для секции []
    Terminal.Write(Dialogs.makeСтрокаОжидаемыхОтветов(new EnumSpeakDialogResult(EnumSpeakDialogResult.ДаНетОтмена)));
this.ResetColors();

String result = "";// String.Empty;
do
{
    result = this.ReadLine();
    if (result == null)
        result = ""; // String.Empty;//случается, если
                     // нажата Ctrl+C
                     // no empty answers
    if (Dialogs.этоДа(result))
        return new EnumSpeakDialogResult(EnumSpeakDialogResult.Да);
    else if (Dialogs.этоНет(result))
        return new EnumSpeakDialogResult(EnumSpeakDialogResult.Нет);
    else if (Dialogs.этоОтмена(result))
        return new EnumSpeakDialogResult(EnumSpeakDialogResult.Отмена);
    else
    {
        this.SureConsoleCursorStart();// убедиться что курсор находится
                                      // в начале строки
        this.PrintTextLine("Принимаются только ответы Да, Нет или Отмена!", EnumDialogConsoleColor.Предупреждение);
    }
}
while (true);

        // unreachable code: return new EnumSpeakDialogResult(
        // EnumSpeakDialogResult.Unknown);
    }

    /** 
     * NT-Запросить у пользователя ввести индекс [1..n] элемента из ранее выведенного нумерованного списка.
     * @param listSize Размер ранее выведенного списка, для проверки границ значений введенного пользователем индекса.
     * @return Функция возвращает введенный пользователем индекс элемента списка, либо -1 при отказе пользователя.
     */
    public int InputListIndex(int listSize)
{
    int result = -1;
    String res = null;
    try
    {
        while (true)
        {
            //get integer number string from user
            res = this.PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите номер (без №) элемента списка:", false, true);
            //если введен Отмена, вернуть значение -1.
            if (Dialogs.этоОтмена(res))
            {
                result = -1;
                break;
            }
            //если введен не номер, то повторять запрос.
            Integer ir = Utility.tryParseInteger(res);
            //если введен допустимый номер, вернуть его значение.
            //если введен номер менее 1 или более listSize, то вывести сообщение и снова запросить номер элемента.
            if ((ir != null) && (ir.intValue() > 0) && (ir.intValue() <= listSize))
            {
                result = ir.intValue();
                break;
            }
            else
            {
                this.SureConsoleCursorStart();// убедиться что курсор находится
                                              // в начале строки
                this.PrintTextLine(String.format("Принимаются только числа от 1 до %d и Отмена", listSize), EnumDialogConsoleColor.Предупреждение);
            }
        }
    }
    catch (Exception ex)
    {
        //TODO: add message to log here
        result = -1;
    }
    return result;
}

/**
 * NT- вывести сообщение об исключении
 * 
 * @param title
 *            Вводный текст сообщения. Если пустая строка, то используется
 *            "Ошибка"
 * @param ex
 *            Объект исключения
 */
public void PrintExceptionMessage(String title, Exception ex)
{
    String s;
    if (Utility.StringIsNullOrEmpty(title))
        s = "Ошибка";
    else s = title;
    //
    StringBuilder sb = new StringBuilder(s);
    // добавим разделительный пробел
    sb.append(' ');
    // добавим название исключения
    sb.append(ex.getClass().getName());
    // добавим текст исключения
    sb.append(": ");
    sb.append(ex.getMessage());
    // выведем текст на консоль
    this.PrintTextLine(sb.toString(), EnumDialogConsoleColor.Предупреждение);

    return;
}

/**
 * NT- вывести сообщение об исключении
 * 
 * @param ex
 *            Объект исключения
 */
public void PrintExceptionMessage(Exception ex)
{
    this.PrintExceptionMessage("Ошибка", ex);

    return;
}

// #region функции вывода свойств мест и процедур
// TODO: перенести их в правильное место. Тут им нечего делать! И код их не
// должен работать с консолью напрямую!

/**
 * NT-Вывести на консоль короткое описание Места в одну строку
 * 
 * @param place
 *            Место
 */
public void PrintPlaceShortLine(Place place)
{
    this.SureConsoleCursorStart();
    this.PrintTextLine(place.getSingleLineProperties(), EnumDialogConsoleColor.Сообщение);

    return;
}

/**
 * NT-вывести на консоль свойства Места подробно, как форму
 * 
 * @param p
 *            Место
 */
public void PrintPlaceForm(Place p)
{
    this.SureConsoleCursorStart();
    // тут надо вывести описание свойств Места в виде многострочной формы
    // или списка свойств.
    String[] sar = new String[10];
    sar[0] = String.format("Свойства Места \"%s\":", p.get_Title());
    sar[1] = String.format("Название:  %s", p.get_Title());
    sar[2] = String.format("Категория: %s", p.get_Namespace());
    sar[3] = String.format("Класс:     %s", p.get_PlaceTypeExpression());
    sar[4] = String.format("Адрес:     %s", p.get_Path());
    sar[5] = String.format("Синонимы:  %s", p.get_Synonim());
    sar[6] = String.format("Описание:  %s", p.get_Description());
    sar[7] = String.format("ID:        %d", p.get_TableId());
    sar[8] = String.format("ТолькоЧтение: %s", Utility.BoolToДаНет(!p.isItemCanChanged()));
    sar[9] = "";// пустая строка-разделитель
                // print array of lines
    this.PrintTextLines(sar, EnumDialogConsoleColor.Сообщение);

    return;
}

/**
 * NT-Вывести на экран список существующих Мест - только названия и описания мест.
 */
public void PrintListOfPlaces()
{
    this.SureConsoleCursorStart();
    // получить список мест
    LinkedList<Place> places = this.m_Engine.get_ECM().getPlacesAsList();

    // сортировать список мест по алфавиту и неймспейсам.
    // вывести на экран одни только названия и описания мест
    // TODO: Удобство - если строк много, вывести порциями по 20 штук с
    // перерывом на Enter

    // write namespace sorted items
    ItemDictionaryByNamespace itbn = new ItemDictionaryByNamespace();
    itbn.addPlaceItems(places);
    String[] keys = itbn.getKeys(true);
    for (String group : keys)
        {
    // this.WriteGroupHeader(writer, group);
    this.PrintTextLine("[" + group + "]", EnumDialogConsoleColor.Сообщение);
    // write procedure lines
    LinkedList<Item> items = itbn.getItems(group, true);
    for (Item it : items)
    {
        Place p = (Place)it;
        String shortline = "   " + p.GetShortInfo();
        this.PrintTextLine(shortline, EnumDialogConsoleColor.Сообщение);
    }
    this.PrintEmptyLine();
}

return;
    }

    /**
     * NT-Вывести на консоль нумерованный список Мест в виде многострочной формы с разделителями.
     * @param list Список Мест.
     */
    public void PrintPlaceFormNumberedList(LinkedList<Place> list)
{
    String tmp;
    Place p;
    for (int i = 0; i < list.size(); i++)
    {
        p = list.get(i);
        tmp = String.format("№%d Место \"%s\":", i, p.get_Title());
        this.PrintTextLine(tmp, EnumDialogConsoleColor.Сообщение);
        //
        this.PrintPlaceForm(p);
        //тут возможно нужен разделитель?
    }

    return;
}

//*** Процедуры ***
/**
 * NT-Вывести на экран список существующих Процедур - только названия и описания процедур.
 */
public void PrintListOfProcedures()
{
    this.SureConsoleCursorStart();
    // получить список процедур
    LinkedList<Procedure> procedures = this.m_Engine.get_ECM().getProceduresAsList();

    // сортировать список мест по алфавиту и неймспейсам.
    // вывести на экран одни только названия и описания процедур
    // TODO: Удобство - если строк много, вывести порциями по 20 штук с
    // перерывом на Enter

    // write namespace sorted items
    ItemDictionaryByNamespace itbn = new ItemDictionaryByNamespace();
    itbn.addProcedureItems(procedures);
    String[] keys = itbn.getKeys(true);
    for (String group : keys)
        {
    // this.WriteGroupHeader(writer, group);
    this.PrintTextLine("[" + group + "]", EnumDialogConsoleColor.Сообщение);
    // write procedure lines
    LinkedList<Item> items = itbn.getItems(group, true);
    for (Item it : items)
    {
        Procedure p = (Procedure)it;
        String shortline = "   " + p.GetShortInfo();
        this.PrintTextLine(shortline, EnumDialogConsoleColor.Сообщение);
    }
    this.PrintEmptyLine();
}

return;
    }

    /**
     * NT-Вывести на консоль короткое описание Процедуры в одну строку
     * 
     * @param p
     *            Объект Процедуры.
     */
    public void PrintProcedureShortLine(Procedure p)
{
    this.SureConsoleCursorStart();
    this.PrintTextLine(p.getSingleLineProperties(), EnumDialogConsoleColor.Сообщение);

    return;
}

/**
 * NT-вывести на консоль свойства Процедуры подробно, как форму
 * 
 * @param p
 *            Объект Процедуры.
 */
public void PrintProcedureForm(Procedure p)
{
    this.SureConsoleCursorStart();
    // тут надо вывести описание свойств Процедуры в виде
    // многострочной формы или списка свойств.
    String[] sar = new String[10];
    sar[0] = String.format("Свойства Команды \"%s\":", p.get_Title());
    sar[1] = String.format("Название:  %s", p.get_Title());
    sar[2] = String.format("Категория: %s", p.get_Namespace());
    sar[3] = String.format("Описание:  %s", p.get_Description());
    sar[4] = String.format("Регекс:    %s", p.get_Regex());
    sar[5] = String.format("Адрес:     %s", p.get_Path());
    sar[6] = String.format("Вес:       %s", p.get_Ves().toString());
    sar[7] = String.format("ID:        %d", p.get_TableId());
    sar[8] = String.format("ТолькоЧтение: %s", Utility.BoolToДаНет(!p.isItemCanChanged()));
    sar[9] = "";// пустая строка-разделитель
                // print array of lines
    this.PrintTextLines(sar, EnumDialogConsoleColor.Сообщение);

}

/**
 * NT-Вывести на консоль нумерованный список процедур в виде многострочной формы с разделителями.
 * @param list Список Процедур.
 */
public void PrintProcedureFormNumberedList(LinkedList<Procedure> list)
{
    String tmp;
    Procedure p;
    for (int i = 0; i < list.size(); i++)
    {
        p = list.get(i);
        tmp = String.format("№%d Команда \"%s\":", i, p.get_Title());
        this.PrintTextLine(tmp, EnumDialogConsoleColor.Сообщение);
        //
        this.PrintProcedureForm(p);
        //тут возможно нужен разделитель?
    }

    return;
}

//*** Настройки ***
/**
 * NT-Вывести на экран список существующих Настроек - только названия и описания настроек.
 */
public void PrintListOfSettings()
{
    this.SureConsoleCursorStart();
    // получить список процедур
    LinkedList<SettingItem> settings = this.m_Engine.get_ECM().getSettingsAsList();

    // сортировать список мест по алфавиту и неймспейсам.
    // вывести на экран одни только названия и описания процедур
    // TODO: Удобство - если строк много, вывести порциями по 20 штук с
    // перерывом на Enter

    // write namespace sorted items
    ItemDictionaryByNamespace itbn = new ItemDictionaryByNamespace();
    itbn.addSettingItems(settings);
    String[] keys = itbn.getKeys(true);
    for (String group : keys)
        {
    // this.WriteGroupHeader(writer, group);
    this.PrintTextLine("[" + group + "]", EnumDialogConsoleColor.Сообщение);
    // write procedure lines
    LinkedList<Item> items = itbn.getItems(group, true);
    for (Item it : items)
    {
        SettingItem p = (SettingItem)it;
        String shortline = "   " + p.GetShortInfo();
        this.PrintTextLine(shortline, EnumDialogConsoleColor.Сообщение);
    }
    this.PrintEmptyLine();
}

return;
    }
    
    /**
     * NT-Вывести на консоль короткое описание Настройки в одну строку
     * 
     * @param p
     *            Объект Настройки.
     */
    public void PrintSettingShortLine(SettingItem p)
{
    this.SureConsoleCursorStart();
    this.PrintTextLine(p.getSingleLineProperties(), EnumDialogConsoleColor.Сообщение);

    return;
}

/**
 * NT-вывести на консоль свойства Настройки подробно, как форму
 * 
 * @param p
 *            Объект Настройки.
 */
public void PrintSettingForm(SettingItem p)
{
    this.SureConsoleCursorStart();
    // тут надо вывести описание свойств Настройки в виде
    // многострочной формы или списка свойств.
    String[] sar = new String[8];
    sar[0] = String.format("Свойства Настройки \"%s\":", p.get_Title());
    sar[1] = String.format("Название:  %s", p.get_Title());
    sar[2] = String.format("Категория: %s", p.get_Namespace());
    sar[3] = String.format("Описание:  %s", p.get_Description());
    sar[4] = String.format("Значение:     %s", p.get_Path());
    sar[5] = String.format("ID:        %d", p.get_TableId());
    sar[6] = String.format("ТолькоЧтение: %s", Utility.BoolToДаНет(!p.isItemCanChanged()));
    sar[7] = "";// пустая строка-разделитель
                // print array of lines
    this.PrintTextLines(sar, EnumDialogConsoleColor.Сообщение);

}

/**
 * NT-Вывести на консоль нумерованный список Настроек в виде многострочной формы с разделителями.
 * @param list Список Настроек.
 */
public void PrintSettingFormNumberedList(LinkedList<SettingItem> list)
{
    String tmp;
    SettingItem p;
    for (int i = 0; i < list.size(); i++)
    {
        p = list.get(i);
        tmp = String.format("№%d Настройка \"%s\":", i, p.get_Title());
        this.PrintTextLine(tmp, EnumDialogConsoleColor.Сообщение);
        //
        this.PrintSettingForm(p);
        //тут возможно нужен разделитель?
    }

    return;
}


    
    // #endregion
    }
}
