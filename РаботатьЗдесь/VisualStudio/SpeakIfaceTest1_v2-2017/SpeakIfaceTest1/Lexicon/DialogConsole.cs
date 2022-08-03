using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Collections;


namespace Operator.Lexicon
{
    /// <summary>
    /// Представляет консоль Оператора для всех процедур и внутренних функций движка и всего приложения
    /// </summary>
    public class DialogConsole
    {
        /// <summary>
        /// Обратная ссылка на объект движка
        /// </summary>
        private Engine m_Engine;
        /// <summary>
        /// Дефолтовый цвет текста
        /// </summary>
        private ConsoleColor m_defaultColor;
        /// <summary>
        /// Русский язык для конверсий и зависимых от языка операций
        /// </summary>
        private CultureInfo m_RuCulture;

        /// <summary>
        /// Constructor
        /// </summary>
        public DialogConsole()
        {
            this.m_defaultColor = (ConsoleColor)DialogConsoleColors.Сообщение;
            Console.ForegroundColor = this.m_defaultColor;
            this.m_RuCulture = CultureInfo.CreateSpecificCulture("ru-RU");
            
            return;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="engine">Engine object</param>
        public DialogConsole(Engine engine)
        {
            this.m_Engine = engine;
            this.m_defaultColor = (ConsoleColor)DialogConsoleColors.Сообщение;
            Console.ForegroundColor = this.m_defaultColor;
            this.m_RuCulture = CultureInfo.CreateSpecificCulture("ru-RU");

            return;
        }

        #region Properties
        /// <summary>
        /// Русский язык для конверсий и зависимых от языка операций
        /// </summary>
        public CultureInfo RuCulture
        {
            get { return m_RuCulture; }
        }

        #endregion

        /// <summary>
        /// NT-Установить обычный цвет текста консоли
        /// </summary>
        public void ResetColor()
        {
            Console.ForegroundColor = this.m_defaultColor;
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
        /// NT-вывести на консоль текст
        /// (Console.Write())
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        public void PrintText(string text, DialogConsoleColors color)
        {
            Console.ForegroundColor = (ConsoleColor)color;
            Console.Write(text);
            this.ResetColor();

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
        public void PrintTextLine(string text, DialogConsoleColors color)
        {
            //если текст - пустая строка, то незачем цвета переключать
            if (String.IsNullOrEmpty(text))
                Console.WriteLine();
            else
            {
                Console.ForegroundColor = (ConsoleColor)color;
                Console.WriteLine(text);
                this.ResetColor();
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
        public void PrintTextLines(string[] lines, DialogConsoleColors color)
        {
            Console.ForegroundColor = (ConsoleColor)color;
            foreach (string s in lines)
                Console.WriteLine(s);
            this.ResetColor();

            return;
        }

        /// <summary>
        /// NT-Получить строку ввода с консоли
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            Console.ForegroundColor = (ConsoleColor)DialogConsoleColors.ВводПользователя;
            string result = Console.ReadLine();
            this.ResetColor();

            return result;
        }

        /// <summary>
        /// NT-вывести на консоль вопрос и принять ответ
        /// </summary>
        /// <param name="keys">Набор флагов ожидаемых субкоманд</param>
        /// <param name="question">Текст вопроса пользователю</param>
        /// <param name="newLine">True - Начинать ответ с новой строки, False - в той же строке.</param>
        /// <param name="noEmptyAnswer">True - требовать повторный ввод, если ответ пустая строка; False - принимать пустые ответы</param>
        /// <returns>Возвращает строку, введенную пользователем.</returns>
        public String PrintQuestionAnswer(SpeakDialogResult keys, string question, bool newLine, bool noEmptyAnswer)
        {
            this.SureConsoleCursorStart();
            Console.ForegroundColor = (ConsoleColor)DialogConsoleColors.Вопрос;//установим цвет вопросов в консоли
            Console.Write(question);
            Console.Write(' ');//разделитель для секции []
            Console.Write(Dialogs.makeСтрокаОжидаемыхОтветов(keys));
            this.ResetColor();
            //если указан флаг, вывести символ конца строки
            if (newLine) Console.WriteLine();
            String result = String.Empty;
            do
            {
                result = this.ReadLine();
                if (result == null)
                    result = String.Empty;//случается, если нажата Ctrl+C
                else
                    result = result.Trim();//удалить пробелы и всякие там случайности
                //no empty answers
                if ((noEmptyAnswer == false) && (String.IsNullOrEmpty(result)))
                    break;//answer can be empty
            } while (String.IsNullOrEmpty(result));

            return result;
        }

        /// <summary>
        /// NT-Диалог Да-Нет-Отменить. Другие ответы не принимаются.
        /// </summary>
        /// <param name="текстЗапроса">Текст вопроса пользователю, без символа перевода строки в конце!</param>
        /// <returns>
        /// Функция возвращает <c>SpeakDialogResult</c> код стандартного ответа Да, Нет или Отмена.
        /// </returns>
        public SpeakDialogResult PrintДаНетОтмена(string текстЗапроса)
        {
            this.SureConsoleCursorStart();//убедиться что курсор находится в начале строки
            Console.ForegroundColor = (ConsoleColor)DialogConsoleColors.Вопрос;//установим цвет вопросов
            Console.Write(текстЗапроса);
            Console.Write(' ');//разделитель для секции []
            Console.Write(Dialogs.makeСтрокаОжидаемыхОтветов(SpeakDialogResult.ДаНетОтмена));
            this.ResetColor();

            String result = String.Empty;
            do
            {
                result = this.ReadLine();
                if (result == null) result = String.Empty;//случается, если нажата Ctrl+C
                //no empty answers
                if (Dialogs.этоДа(result)) return SpeakDialogResult.Да;
                else if (Dialogs.этоНет(result)) return SpeakDialogResult.Нет;
                else if (Dialogs.этоОтмена(result)) return SpeakDialogResult.Отмена;
                else
                {
                    this.SureConsoleCursorStart();//убедиться что курсор находится в начале строки
                    this.PrintTextLine("Принимаются только ответы Да, Нет или Отмена!", DialogConsoleColors.Предупреждение);
                }
            } while (true);

            return SpeakDialogResult.Unknown;
        }
        /// <summary>
        /// NT-Форматировать дату и время в русской культуре
        /// Пример: воскресенье, 26 апреля 2020г. 01:03:18
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string CreateLongDatetimeString(DateTime dt)
        {
            //TODO: перенести эту функцию в семантику-лингвистику.
            return dt.ToString("dddd, d MMMM yyyyг. HH:mm:ss", this.m_RuCulture);
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
            this.PrintTextLine(sb.ToString(), DialogConsoleColors.Предупреждение);

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

        #region функции вывода свойств мест и процедур
        //TODO: перенести их в правильное место. Тут им нечего делать! И код их не должен работать с консолью напрямую!
        /// <summary>
        /// NT-Вывести на консоль короткое описание места в одну строку
        /// </summary>
        /// <param name="place"></param>
        public void PrintPlaceShortLine(Place place)
        {
            this.SureConsoleCursorStart();
            this.PrintTextLine(place.getSingleLineProperties(), DialogConsoleColors.Сообщение);

            return;
        }

        /// <summary>
        /// NT-вывести на консоль свойства места подробно, как форму
        /// </summary>
        /// <param name="p"></param>
        public void PrintPlaceForm(Place p)
        {
            this.SureConsoleCursorStart();
            //тут надо вывести описание свойств Места в виде многострочной формы или списка свойств.
            this.PrintTextLine(String.Format("Свойства Места \"{0}\":", p.Title), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Название: {0}", p.Title), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Класс:    {0}", p.PlaceTypeExpression), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Адрес:    {0}", p.Path), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Синонимы: {0}", p.Synonim), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Описание: {0}", p.Description), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("ID:       {0}", p.TableId.ToString()), DialogConsoleColors.Сообщение);
            this.PrintEmptyLine(); //пустая строка-разделитель

            return;
        }

        /// <summary>
        /// NT-Вывести на экран список существующих мест - только названия мест
        /// </summary>
        public void PrintListOfPlaces()
        {
            this.SureConsoleCursorStart();
            //получить список мест
            List<Place> places = this.m_Engine.Database.GetAllPlaces();
            //сортировать список мест по алфавиту
            places.Sort(Place.SortByTitle);
            //вывести на экран одни только названия мест
            foreach (Place p in places)
                this.PrintTextLine(string.Format("{0} [{1}]", p.Title, p.Path), DialogConsoleColors.Сообщение);

            return;
        }
        /// <summary>
        /// NT-Вывести на экран список существующих Процедур - только названия процедур
        /// </summary>
        public void PrintListOfProcedures()
        {
            this.SureConsoleCursorStart();
            //получить список процедур
            List<Procedure> procedures = this.m_Engine.Database.GetAllProcedures();
            //сортировать список мест по алфавиту
            procedures.Sort(Procedure.SortByTitle);
            //вывести на экран одни только названия процедур
            foreach (Procedure p in procedures)
                this.PrintTextLine(string.Format("{0} [{1}]", p.Title, p.Description), DialogConsoleColors.Сообщение);

            return;
        }

        /// <summary>
        /// NR-Вывести на консоль короткое описание Процедуры в одну строку
        /// </summary>
        /// <param name="place"></param>
        public void PrintProcedureShortLine(Procedure p)
        {
            this.SureConsoleCursorStart();
            this.PrintTextLine(p.getSingleLineProperties(), DialogConsoleColors.Сообщение);

            return;
        }

        /// <summary>
        /// NT-вывести на консоль свойства Процедуры подробно, как форму
        /// </summary>
        /// <param name="p"></param>
        public void PrintProcedureForm(Procedure p)
        {
            this.SureConsoleCursorStart();
            //TODO: тут надо вывести описание свойств Процедуры в виде многострочной формы или списка свойств.
            this.PrintTextLine(String.Format("Свойства Команды \"{0}\":", p.Title), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Название: {0}", p.Title), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Описание: {0}", p.Description), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Регекс:   {0}", p.Regex), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Адрес:    {0}", p.Path), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("Вес:      {0}", p.Ves), DialogConsoleColors.Сообщение);
            this.PrintTextLine(String.Format("ID:       {0}", p.TableId.ToString()), DialogConsoleColors.Сообщение);
            this.PrintEmptyLine(); //пустая строка-разделитель

        }

        #endregion



    }
}
