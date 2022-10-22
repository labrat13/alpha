using System;
using System.Collections.Generic;
using System.Text;
using Engine.OperatorEngine;
using Engine.SettingSubsystem;
using Engine.Utility;

namespace Engine.LexiconSubsystem
{
    /// <summary>
    /// NT-Представляет консоль Оператора для всех процедур и внутренних функций движка и всего приложения.
    /// </summary>
    public class DialogConsole : Engine.OperatorEngine.EngineSubsystem
    {

        /// <summary>
        /// Дефолтовый цвет текста
        /// </summary>
        private ConsoleColor m_defaultColor;

        /// <summary>
        /// NR - Конструктор
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        public DialogConsole(OperatorEngine.Engine engine) : base(engine)
        {
            this.m_defaultColor = (ConsoleColor)DialogConsoleColor.Сообщение;
            //TODO: Add code here
            return;
        }

        #region  *** Override this from EngineSubsystem parent class ***
        /// <summary>
        /// NR - Initialize Subsystem. 
        /// </summary>
        protected override void onOpen()
        {
            //настроить консоль приложения
            Console.ForegroundColor = this.m_defaultColor;
            //Console.Title = new window title

            //TODO: Add init code here
            return;
        }

        /// <summary>
        /// NR - De-initialize Subsystem. 
        /// </summary>
        protected override void onClose()
        {

            //TODO: Add de-init code here
            return;
        }
        #endregion

        #region *** Служебные функции ***

        /// <summary>
        /// NT-Установить обычный цвет текста консоли
        /// </summary>
        private void ResetColors()
        {
            Console.ForegroundColor = this.m_defaultColor;
        }

        /// <summary>
        /// NT-Установить указанный цвет текста консоли.
        /// Консоль должна печатать этим цветом весь последующий текст, включая промпт и ввод пользователя.
        /// </summary>
        /// <param name="color">Цвет для текста консоли</param>
        private void SetTextColor(DialogConsoleColor color)
        {
            Console.ForegroundColor = (ConsoleColor)color;

            return;
        }

        /// <summary>
        /// NT-Подать короткий звуковой сигнал
        /// </summary>
        public void Beep()
        {
            Console.Beep();
            return;
        }

        /// <summary>
        /// NT-Поместить курсор консоли в начало новой строки, если он не там.
        /// </summary>
        public void SureConsoleCursorStart()
        {
            if (Console.CursorLeft > 0)
                Console.WriteLine();

            return;
        }

        /// <summary>
        /// NT-вывести на консоль текст (Console.Write())
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        public void PrintText(String text, DialogConsoleColor color)
        {
            this.SetTextColor(color);
            Console.Write(text);
            this.ResetColors();

            return;
        }

        /// <summary>
        /// NT-Вывести на консоль пустую строку
        /// </summary>
        public void PrintEmptyLine()
        {
            Console.WriteLine();

            return;
        }

        /// <summary>
        /// NT-вывести на консоль строку текста
        /// (Console.WriteLine())
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        public void PrintTextLine(String text, DialogConsoleColor color)
        {
            // если текст - пустая строка, то незачем цвета переключать
            if (String.IsNullOrEmpty(text))
                Console.WriteLine();
            else
            {
                this.SetTextColor(color);
                Console.WriteLine(text);
                this.ResetColors();
            }

            return;
        }

        /// <summary>
        /// NT-вывести массив строк сообщения.
        /// </summary>
        /// <remarks>
        /// Это типа оптимизация - экономит переключения цвета текста консоли
        /// </remarks>
        /// <param name="lines">Массив строк сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        public void PrintTextLines(string[] lines, DialogConsoleColor color)
        {
            this.SetTextColor(color);
            foreach (string s in lines)
                Console.WriteLine(s);
            this.ResetColors();

            return;
        }

        /// <summary>
        /// NT-Получить строку ввода с консоли
        /// </summary>
        /// <returns>Функция возвращает введенный с консоли текст</returns>
        public String ReadLine()
        {
            this.SetTextColor(DialogConsoleColor.ВводПользователя);
            String result = Console.ReadLine();
            this.ResetColors();

            return result;
        }

        #endregion

        #region *** Основные функции ***
        /// <summary>
        /// NT-вывести на консоль вопрос и принять ответ
        /// </summary>
        /// <param name="keys">Набор флагов ожидаемых субкоманд</param>
        /// <param name="question">Текст вопроса пользователю</param>
        /// <param name="newLine">True - Начинать ответ с новой строки, False - в той же строке.</param>
        /// <param name="noEmptyAnswer">True - требовать повторный ввод, если ответ пустая строка; False - принимать пустые ответы</param>
        /// <returns>Возвращает строку, введенную пользователем.</returns>
        public String PrintQuestionAnswer(
                SpeakDialogResult keys,
                String question,
                Boolean newLine,
                Boolean noEmptyAnswer)
        {
            this.SureConsoleCursorStart();// убедиться что курсор находится в начале строки
                                          // установим цвет вопросов в консоли
            this.SetTextColor(DialogConsoleColor.Вопрос);
            Console.Write(question);
            Console.Write(' ');// разделитель для секции []
            Console.Write(Dialogs.makeСтрокаОжидаемыхОтветов(keys));
            this.ResetColors();
            // если указан флаг, вывести символ конца строки
            if (newLine)
                Console.WriteLine();
            String result = String.Empty;
            do
            {
                result = this.ReadLine();
                if (result == null)
                    result = String.Empty;//случается, если нажата Ctrl+C
                else result = result.Trim();// удалить пробелы и всякие там случайности
                                            // no empty answers
                if ((noEmptyAnswer == false) && String.IsNullOrEmpty(result))
                    break;// answer can be empty
            }
            while (String.IsNullOrEmpty(result));

            return result;
        }

        /// <summary>
        /// NT-Диалог Да-Нет-Отменить. Другие ответы не принимаются.
        /// </summary>
        /// <param name="question">Текст вопроса пользователю, без символа перевода строки в конце!</param>
        /// <returns>
        /// Функция возвращает <c>SpeakDialogResult</c> код стандартного ответа Да, Нет или Отмена.
        /// </returns>
        public SpeakDialogResult PrintДаНетОтмена(String question)
        {
            this.SureConsoleCursorStart();// убедиться что курсор находится в начале строки
            // установим цвет вопросов в консоли
            this.SetTextColor(DialogConsoleColor.Вопрос);
            Console.Write(question);
            Console.Write(' ');// разделитель для секции []
            Console.Write(Dialogs.makeСтрокаОжидаемыхОтветов(SpeakDialogResult.ДаНетОтмена));
            this.ResetColors();

            String result = String.Empty;
            do
            {
                result = this.ReadLine();
                if (result == null)
                    result = String.Empty;//случается, если нажата Ctrl+C
                                          //no empty answers
                if (Dialogs.этоДа(result))
                    return SpeakDialogResult.Да;
                else if (Dialogs.этоНет(result))
                    return SpeakDialogResult.Нет;
                else if (Dialogs.этоОтмена(result))
                    return SpeakDialogResult.Отмена;
                else
                {
                    this.SureConsoleCursorStart();// убедиться что курсор находится в начале строки
                    this.PrintTextLine("Принимаются только ответы Да, Нет или Отмена!", DialogConsoleColor.Предупреждение);
                }
            }
            while (true);

            // unreachable code: return SpeakDialogResult.Unknown;
        }

        /// <summary>
        /// NT-Запросить у пользователя ввести индекс [1..n] элемента из ранее выведенного нумерованного списка.
        /// </summary>
        /// <param name="listSize">Размер ранее выведенного списка, для проверки границ значений введенного пользователем индекса..</param>
        /// <returns>Функция возвращает введенный пользователем индекс элемента списка, либо -1 при отказе пользователя.</returns>
        public int InputListIndex(int listSize)
        {
            int result = -1;
            String res = null;
            try
            {
                while (true)
                {
                    //get integer number string from user
                    res = this.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите номер (без №) элемента списка:", false, true);
                    //если введен Отмена, вернуть значение -1.
                    if (Dialogs.этоОтмена(res))
                    {
                        result = -1;
                        break;
                    }
                    //если введен не номер, то повторять запрос.
                    int? ir = StringUtility.tryParseInteger(res);
                    //если введен допустимый номер, вернуть его значение.
                    //если введен номер менее 1 или более listSize, то вывести сообщение и снова запросить номер элемента.
                    if ((ir != null) && ir.HasValue && (ir.Value > 0) && (ir.Value <= listSize))
                    {
                        result = ir.Value;
                        break;
                    }
                    else
                    {
                        this.SureConsoleCursorStart();// убедиться что курсор находится в начале строки
                        this.PrintTextLine(String.Format("Принимаются только числа от 1 до {0} и Отмена", listSize), DialogConsoleColor.Предупреждение);
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

        /// <summary>
        /// NT- вывести сообщение об исключении
        /// </summary>
        /// <param name="title">Вводный текст сообщения. Если пустая строка, то используется "Ошибка"</param>
        /// <param name="ex">Объект исключения</param>
        public void PrintExceptionMessage(string title, Exception ex)
        {
            StringBuilder sb = new StringBuilder(title);
            //добавим разделительный пробел
            sb.Append(' ');
            //добавим название исключения
            sb.Append(ex.GetType().ToString());
            //добавим текст исключения
            sb.Append(": ");
            sb.Append(ex.Message);
            //выведем текст на консоль 
            this.PrintTextLine(sb.ToString(), DialogConsoleColor.Предупреждение);

            return;
        }

        /// <summary>
        /// NT- вывести сообщение об исключении
        /// </summary>
        /// <param name="ex">Объект исключения</param>
        public void PrintExceptionMessage(Exception ex)
        {
            this.PrintExceptionMessage("Ошибка", ex);

            return;
        }
        
        #endregion

        #region *** Функции вывода свойств Мест ***
        
        // TODO: перенести их в правильное место. И код их не должен работать с консолью напрямую!

        /// <summary>
        /// NT-Вывести на консоль короткое описание места в одну строку
        /// </summary>
        /// <param name="place"></param>
        public void PrintPlaceShortLine(Place place)
        {
            this.SureConsoleCursorStart();
            this.PrintTextLine(place.getSingleLineProperties(), DialogConsoleColor.Сообщение);

            return;
        }

        /// <summary>
        /// NT-вывести на консоль свойства места подробно, как форму
        /// </summary>
        /// <param name="p">Место</param>
        public void PrintPlaceForm(Place p)
        {
            this.SureConsoleCursorStart();
            // тут надо вывести описание свойств Места в виде многострочной формы
            // или списка свойств.
            String[] sar = new String[10];
            sar[0] = String.Format("Свойства Места \"{0}\":", p.Title);
            sar[1] = String.Format("Название:  {0}", p.Title);
            sar[2] = String.Format("Категория: {0}", p.Namespace);
            sar[3] = String.Format("Класс:     {0}", p.PlaceTypeExpression);
            sar[4] = String.Format("Адрес:     {0}", p.Path);
            sar[5] = String.Format("Синонимы:  {0}", p.Synonim);
            sar[6] = String.Format("Описание:  {0}", p.Description);
            sar[7] = String.Format("ID:        {0}", p.TableId.ToString());
            sar[8] = String.Format("ТолькоЧтение: {0}", StringUtility.BoolToДаНет(!p.isItemCanChanged()));
            sar[9] = "";// пустая строка-разделитель
                        // print array of lines
            this.PrintTextLines(sar, DialogConsoleColor.Сообщение);

            return;
        }

        /// <summary>
        /// NT-Вывести на экран список существующих Мест - только названия и описания мест.
        /// </summary>
        /// <returns></returns>
        public void PrintListOfPlaces()
        {
            this.SureConsoleCursorStart();
            // получить список мест
            List<Place> places = this.m_Engine.ECM.getPlacesAsList();

            // сортировать список мест по алфавиту и неймспейсам.
            // вывести на экран одни только названия и описания мест
            // TODO: Удобство - если строк много, вывести порциями по 20 штук с перерывом на Enter

            // write namespace sorted items
            ItemDictionaryByNamespace itbn = new ItemDictionaryByNamespace();
            itbn.addPlaceItems(places);
            String[] keys = itbn.getKeys(true);
            foreach (String group in keys)
            {
                // this.WriteGroupHeader(writer, group);
                this.PrintTextLine("[" + group + "]", DialogConsoleColor.Сообщение);
                // write procedure lines
                List<Item> items = itbn.getItems(group, true);
                foreach (Item it in items)
                {
                    Place p = (Place)it;
                    String shortline = "   " + p.GetShortInfo();
                    this.PrintTextLine(shortline, DialogConsoleColor.Сообщение);
                }
                this.PrintEmptyLine();
            }

            return;
        }

        /// <summary>
        /// NT-Вывести на консоль нумерованный список Мест в виде многострочной формы с разделителями.
        /// </summary>
        /// <param name="list">Список Мест.</param>
        /// <returns></returns>
        public void PrintPlaceFormNumberedList(List<Place> list)
        {
            String tmp;
            Place p;
            for (int i = 0; i < list.Count; i++)
            {
                p = list[i];
                tmp = String.Format("№{0} Место \"{1}\":", i, p.Title);
                this.PrintTextLine(tmp, DialogConsoleColor.Сообщение);
                //
                this.PrintPlaceForm(p);
                //тут возможно нужен разделитель?
            }

            return;
        }

        #endregion

        #region *** Функции вывода свойств Процедур ***

        /// <summary>
        /// NT-Вывести на экран список существующих Процедур - только названия процедур
        /// </summary>
        public void PrintListOfProcedures()
        {
            this.SureConsoleCursorStart();
            // получить список процедур
            List<Procedure> procedures = this.m_Engine.ECM.getProceduresAsList();

            // сортировать список мест по алфавиту и неймспейсам.
            // вывести на экран одни только названия и описания процедур
            // TODO: Удобство - если строк много, вывести порциями по 20 штук с
            // перерывом на Enter

            // write namespace sorted items
            ItemDictionaryByNamespace itbn = new ItemDictionaryByNamespace();
            itbn.addProcedureItems(procedures);
            String[] keys = itbn.getKeys(true);
            foreach (String group in keys)
            {
                // this.WriteGroupHeader(writer, group);
                this.PrintTextLine("[" + group + "]", DialogConsoleColor.Сообщение);
                // write procedure lines
                List<Item> items = itbn.getItems(group, true);
                foreach (Item it in items)
                {
                    Procedure p = (Procedure)it;
                    String shortline = "   " + p.GetShortInfo();
                    this.PrintTextLine(shortline, DialogConsoleColor.Сообщение);
                }
                this.PrintEmptyLine();
            }

            return;
        }

        /// <summary>
        /// NT-Вывести на консоль короткое описание Процедуры в одну строку
        /// </summary>
        /// <param name="p">Объект Процедуры</param>
        public void PrintProcedureShortLine(Procedure p)
        {
            this.SureConsoleCursorStart();
            this.PrintTextLine(p.getSingleLineProperties(), DialogConsoleColor.Сообщение);

            return;
        }

        /// <summary>
        /// NT-вывести на консоль свойства Процедуры подробно, как форму
        /// </summary>
        /// <param name="p">Объект Процедуры.</param>
        public void PrintProcedureForm(Procedure p)
        {
            this.SureConsoleCursorStart();
            // тут надо вывести описание свойств Процедуры в виде
            // многострочной формы или списка свойств.
            String[] sar = new String[10];
            sar[0] = String.Format("Свойства Команды \"{0}\":", p.Title);
            sar[1] = String.Format("Название:  {0}", p.Title);
            sar[2] = String.Format("Категория: {0}", p.Namespace);
            sar[3] = String.Format("Описание:  {0}", p.Description);
            sar[4] = String.Format("Регекс:    {0}", p.Regex);
            sar[5] = String.Format("Адрес:     {0}", p.Path);
            sar[6] = String.Format("Вес:       {0}", p.Ves.ToString());
            sar[7] = String.Format("ID:        {0}", p.TableId.ToString());
            sar[8] = String.Format("ТолькоЧтение: {0}", StringUtility.BoolToДаНет(!p.isItemCanChanged()));
            sar[9] = "";// пустая строка-разделитель
                        // print array of lines
            this.PrintTextLines(sar, DialogConsoleColor.Сообщение);

            return;
        }

        /// <summary>
        /// NT-Вывести на консоль нумерованный список процедур в виде многострочной формы с разделителями.
        /// </summary>
        /// <param name="list">Список Процедур.</param>
        /// <returns></returns>
        public void PrintProcedureFormNumberedList(List<Procedure> list)
        {
            String tmp;
            Procedure p;
            for (int i = 0; i < list.Count; i++)
            {
                p = list[i];
                tmp = String.Format("№{0} Команда \"{1}\":", i, p.Title);
                this.PrintTextLine(tmp, DialogConsoleColor.Сообщение);
                //
                this.PrintProcedureForm(p);
                //тут возможно нужен разделитель?
            }

            return;
        }

        #endregion

        #region *** Функции вывода свойств Настроек ***

        /// <summary>
        /// NT-Вывести на экран список существующих Настроек - только названия и описания настроек.
        /// </summary>
        public void PrintListOfSettings()
        {
            this.SureConsoleCursorStart();
            // получить список процедур
            List<SettingItem> settings = this.m_Engine.ECM.getSettingsAsList();

            // сортировать список мест по алфавиту и неймспейсам.
            // вывести на экран одни только названия и описания процедур
            // TODO: Удобство - если строк много, вывести порциями по 20 штук с перерывом на Enter

            // write namespace sorted items
            ItemDictionaryByNamespace itbn = new ItemDictionaryByNamespace();
            itbn.addSettingItems(settings);
            String[] keys = itbn.getKeys(true);
            foreach (String group in keys)
            {
                // this.WriteGroupHeader(writer, group);
                this.PrintTextLine("[" + group + "]", DialogConsoleColor.Сообщение);
                // write procedure lines
                List<Item> items = itbn.getItems(group, true);
                foreach (Item it in items)
                {
                    SettingItem p = (SettingItem)it;
                    String shortline = "   " + p.GetShortInfo();
                    this.PrintTextLine(shortline, DialogConsoleColor.Сообщение);
                }
                this.PrintEmptyLine();
            }

            return;
        }

        /// <summary>
        /// NT-Вывести на консоль короткое описание Настройки в одну строку
        /// </summary>
        /// <param name="p">Объект Настройки.</param>
        /// <returns></returns>
        public void PrintSettingShortLine(SettingItem p)
        {
            this.SureConsoleCursorStart();
            this.PrintTextLine(p.getSingleLineProperties(), DialogConsoleColor.Сообщение);

            return;
        }

        /// <summary>
        /// NT-вывести на консоль свойства Настройки подробно, как форму
        /// </summary>
        /// <param name="p">Объект Настройки.</param>
        public void PrintSettingForm(SettingItem p)
        {
            this.SureConsoleCursorStart();
            // тут надо вывести описание свойств Настройки в виде
            // многострочной формы или списка свойств.
            String[] sar = new String[8];
            sar[0] = String.Format("Свойства Настройки \"{0}\":", p.Title);
            sar[1] = String.Format("Название:  {0}", p.Title);
            sar[2] = String.Format("Категория: {0}", p.Namespace);
            sar[3] = String.Format("Описание:  {0}", p.Description);
            sar[4] = String.Format("Значение:  {0}", p.Path);
            sar[5] = String.Format("ID:        {0}", p.TableId.ToString());
            sar[6] = String.Format("ТолькоЧтение: {0}", StringUtility.BoolToДаНет(!p.isItemCanChanged()));
            sar[7] = "";// пустая строка-разделитель
                        // print array of lines
            this.PrintTextLines(sar, DialogConsoleColor.Сообщение);

            return;
        }

        /// <summary>
        /// NT-Вывести на консоль нумерованный список Настроек в виде многострочной формы с разделителями.
        /// </summary>
        /// <param name="list">Список Настроек.</param>
        public void PrintSettingFormNumberedList(List<SettingItem> list)
        {
            String tmp;
            SettingItem p;
            for (int i = 0; i < list.Count; i++)
            {
                p = list[i];
                tmp = String.Format("№{0} Настройка \"{0}\":", i, p.Title);
                this.PrintTextLine(tmp, DialogConsoleColor.Сообщение);
                //
                this.PrintSettingForm(p);
                //тут возможно нужен разделитель?
            }

            return;
        }

        #endregion
    }
}
