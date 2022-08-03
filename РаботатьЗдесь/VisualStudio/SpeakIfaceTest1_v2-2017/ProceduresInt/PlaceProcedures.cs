using System;
using System.Collections.Generic;
using System.Text;
using Operator;
using Operator.Lexicon;

namespace ProceduresInt
{
    //Чтобы отлаживать эти процедуры через отладчик студии, скопируйте файлы dll и pdb в каталог \x86\Debug основного приложения.
    //и запустите отладку основного приложения. Все другие варианты тут не работают.
    
    /// <summary>
    /// Встроенные в Оператор процедуры команд работы с Местами
    /// </summary>
    public static class PlaceProcedures
    {
        #region Константы
        /// <summary>
        /// Массив строк вопросов для ввода падежных форм
        /// </summary>
        private static String[] ВопросыПадежныхФорм = { "И.п. Это что?", "Р.п. Владелец чего?", "Д.п. Дали чему?", "В.п. Обвинили что?", "Тв.п. Творимый чем?", "Пр.п. Рассказ о чем?" };
        
        #endregion

        ///// <summary>
        ///// Пример функции процедуры обработчика команды
        ///// </summary>
        ///// <param name="engine">Механизм исполнения команд</param>
        ///// <param name="cmdline">Текст запроса для возможной дополнительной обработки</param>
        ///// <param name="args">Список аргументов</param>
        ///// <returns>
        ///// Вернуть ProcedureResult.Success в случае успешного выполнения команды.
        ///// Вернуть ProcedureResult.WrongArguments если аргументы не подходят для запуска команды.
        ///// Вернуть ProcedureResult.Error если произошла ошибка при выполнении операции
        ///// Вернуть ProcedureResult.ExitXXX если нужно завершить работу текущего приложения, выключить или перезагрузить компьютер.
        ///// </returns>
        //[ProcedureAttribute(ImplementationState.NotTested)]//изменить состояние на подходящее после отладки функции
        //public static ProcedureResult CommandHandlerExample(Engine engine, string cmdline, ArgumentCollection args)
        //{
        //    //вернуть флаг продолжения работы
        //    return ProcedureResult.Success;
        //}

        /// <summary>
        /// Обработчик процедуры создания новой Сущности (Места)
        /// </summary>
        /// <param name="engine">Механизм исполнения команд</param>
        /// <param name="cmdline">Текст запроса для возможной дополнительной обработки</param>
        /// <param name="args">Список аргументов</param>
        /// <returns>
        /// Вернуть ProcedureResult.Success в случае успешного выполнения команды.
        /// Вернуть ProcedureResult.WrongArguments если аргументы не подходят для запуска команды.
        /// Вернуть ProcedureResult.Error если произошла ошибка при выполнении операции
        /// Вернуть ProcedureResult.ExitXXX если нужно завершить работу текущего приложения, выключить или перезагрузить компьютер.
        /// </returns>
        [ProcedureAttribute(ImplementationState.NotTested)]
        public static ProcedureResult CommandCreatePlace(Engine engine, string query, ArgumentCollection args)
        {
            //содержимое списка аргументов
            //args[0].name = "место" - название аргумента в строке регекса команды
            //args[0].value = "картон" - значение аргумента - название создаваемого места
            //args[0].type = "" - тип места - сейчас не указывается, так как аргумент не сопоставлен существующему месту, 
            // а никак иначе тип определить нельзя. В строке запроса тип аргументов не указывается ведь.


            //TODO: вывести это тестовое сообщение о начале процедуры - в лог!
            engine.OperatorConsole.PrintTextLine(String.Format("Начата процедура CommandCreatePlace(\"{0}\")", args.Arguments[0].ArgumentValue), DialogConsoleColors.Сообщение);

            String str = String.Empty;
            Place p = new Place();
            
            //TODO: тут код недостаточен. Но уже слишком усложнен из-за циклов, которые нужны для возврата после проверок свойств.
            //Надо перепроектировать всю функцию, разбив ее на отдельные подфункции-запросы.

            //Как создать процедуру создания места?
            //1 извлечь из аргументов название Места, если оно есть

            //TODO: ПроблемаАвтоподстановкиМест. Если место уже существует, движок подставляет его в этот запрос в качестве аргумента.
            //Поэтому надо проверять поле типа аргумента. Если оно пустое, то аргумент из командной строки пришел как слово.
            //(Поэтому нельзя типы аргументов при разборе строки назначать, если это не подставленное Место)
            //А если тип есть, то это было место подставленное движком. Тогда ArgumentName = название аргумента из регекса, ArgumentValue=ПутьМеста
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("1. Название Места", DialogConsoleColors.Сообщение);
            
            //извлечь название места из аргумента
            FuncArgument arg = args.Arguments[0];
            str = arg.ArgumentQueryValue.Trim();//берем сырой текст аргумента из запроса
            engine.OperatorConsole.PrintTextLine(String.Format("Название нового Места: {0}", str), DialogConsoleColors.Сообщение);

            //проверить признак того, что место уже существует и было подставлено в запрос при разборе запроса движком.
            if (arg.АвтоподстановкаМеста == true)
            {
                engine.OperatorConsole.PrintTextLine("Место с таким названием уже существует:", DialogConsoleColors.Предупреждение);
                engine.OperatorConsole.PrintPlaceShortLine(arg.AssociatedPlace);
                return ProcedureResult.Error;
            }

            //2 проверить что в БД нет Места с таким названием, без учета регистра символов
            //TODO: этот код надо переделать - эта процедура обычно не вызывается без названия места
            //а неуникальное название места - оригинальное место подставляется движком, и выпиливается вышележащей проверкой
            //Но! иногда в БД есть Места, синонимы которых отличаются от значения свойства Название Места.
            //Вот они-то и попадают в такой оборот, как здесь.
            //Это места, названия которые совпадают с аргументом процедуры, но не с синонимами Места. Редкий случай.
            bool notUnicalPlace = false;
            while (true)
            {
                //если название места - пустая строка, вывести сообщение и перейти к приему нового названия места
                if (String.IsNullOrEmpty(str))
                    engine.OperatorConsole.PrintTextLine("Пустая строка недопустима для названия Места!", DialogConsoleColors.Предупреждение);
                else
                {
                    //проверить что в БД нет Места с таким названием, без учета регистра символов
                    List<Place> lip = engine.DbGetPlacesByTitle(str);
                    notUnicalPlace = (lip.Count > 0);//временный флаг для упрощения проверок позже
                    if (notUnicalPlace)
                    {
                        //тут вывести пользователю найденные места с тем же названием
                        engine.OperatorConsole.PrintTextLine("Места с таким названием уже существуют:", DialogConsoleColors.Предупреждение);
                        foreach (Place pp in lip)
                        {
                            engine.OperatorConsole.PrintPlaceShortLine(lip[0]);
                        }
                        engine.OperatorConsole.PrintTextLine("Дубликаты Мест недопустимы!", DialogConsoleColors.Предупреждение);
                        lip.Clear();//очистить временный список, поскольку он в цикле 
                    }
                }
                if((str == String.Empty) || (notUnicalPlace == true))
                {
                    //Раз есть такие Места, пользователь должен сменить название Места прямо тут же 
                    //или же завершить диалог Отменой создания места
                    str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите новое название Места:", false, true);
                    if (Dialogs.этоОтмена(str))
                        return ProcedureResult.CancelledByUser;
                }
                else
                    break;//end while loop
            }//while loop
            //Тут мы окажемся, если название Места уникальное
            p.Title = str;

            //3 ввести синонимы названия Места
            //именно сейчас. Так как если они не уникальные, то, возможно, придется сменить также и название места.
            if(ВводитьСловоформыМеста(engine, p) == false)
                return ProcedureResult.CancelledByUser;
            //4 TODO: проверить уникальность синонимов названия Места - они должны быть уникальными для всех Мест.
            //если один из синонимов не уникален, то надо предложить пользователю изменить название Места прямо тут же
            //Если пользователь введет Отмена, завершить диалог Отменой создания места

            //5 Ввести описание места одной строкой. Нажатие Enter завершит ввод описания.
            //Поэтому описание места должно быть коротким и емким.
            //Если пользователь введет Отмена, завершить диалог Отменой создания места
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("3. Описание Места", DialogConsoleColors.Сообщение);//пункт плана
            str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите краткое описание Места:", true, true);
            if (Dialogs.этоОтмена(str))
                return ProcedureResult.CancelledByUser;
            p.Description = str;

            //6 Ввести путь к Месту, веб-путь к Сущности, файловый путь к файлу.
            //Если пользователь введет Отмена, завершить диалог Отменой создания места
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("4. Адрес Места", DialogConsoleColors.Сообщение);//пункт плана
            str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите адрес Места:", true, true);
            if (Dialogs.этоОтмена(str))
                return ProcedureResult.CancelledByUser;
            p.Path = str;

            //7 Ввести строку описания класса Сущности. Она имеет специальный формат.
            //пользователю потребуется дополнительно справочник по этим классам.
            //Если пользователь введет Отмена, завершить диалог Отменой создания места
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("5. Класс Места", DialogConsoleColors.Сообщение);//пункт плана
            str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите класс Места:", true, true);
            if (Dialogs.этоОтмена(str))
                return ProcedureResult.CancelledByUser;
            p.PlaceTypeExpression = str;
            //7.1 TODO: Проверить что введенное выражение не имеет ошибок, его можно распарсить в дерево классов
            //если это не так, предложить повторить ввод выражения.
            //Если пользователь введет Отмена, завершить диалог Отменой создания места


            ////TODO: заполним объект тестовыми данными только для проверки возможности создания места
            //p.Title = args.Arguments[0].ArgumentValue;
            //p.Synonim = "корова,коровы,корове,корову,коровой,корове";
            //p.Description = "Тестовое место для отладки создания мест";
            //p.Path = "C:\\Temp";
            //p.PlaceTypeExpression = "Каталог<Каталог, Файл>";
            
            //8 Вроде все свойства должны быть заполнены, теперь надо вывести все их в форме
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("6. Подтвердите создание Места", DialogConsoleColors.Сообщение);//пункт плана
            engine.OperatorConsole.PrintPlaceForm(p);
            //и запросить подтверждение пользователя, что он желает создать Место
            //Если пользователь ответит Да, надо создать место.
            //Если пользователь ответит Нет или Отмена, отменить операцию.
            SpeakDialogResult sdr = engine.OperatorConsole.PrintДаНетОтмена("Желаете создать новое Место?");
            if ((sdr == SpeakDialogResult.Отмена) || (sdr == SpeakDialogResult.Нет))
                return ProcedureResult.CancelledByUser;
            //9 заполнить объект Места и создать новое место в БД
            engine.DbInsertPlace(p);
            //10 вывести сообщение о результате операции: успешно
            engine.OperatorConsole.PrintTextLine(String.Format("Место \"{0}\" создано успешно", p.Title), DialogConsoleColors.Успех);
            
            //вернуть флаг продолжения работы
            return ProcedureResult.Success;
        }

        /// <summary>
        /// NT-принять от пользователя синонимы и словоформы места
        /// </summary>
        /// <param name="engine">Объект движка</param>
        /// <param name="p">Объект места</param>
        /// <returns>Возвращает True если словоформы собраны, False если пользователь отказался продолжать создание места</returns>
        private static bool ВводитьСловоформыМеста(Engine engine, Place p)
        {
            //str = engine.PrintQuestionAnswer(SpeakDialogResult.Отмена, "2. Введите Словоформы Места:", true, true);
            //if (Dialogs.этоОтмена(str))
            //    return ProcedureResult.CancelledByUser;
            String syno = String.Empty;
            //1 вывести текст с описанием формата поля
            engine.OperatorConsole.SureConsoleCursorStart();
            engine.OperatorConsole.PrintEmptyLine();//разделительная пустая строка перед новым пунктом
            engine.OperatorConsole.PrintTextLine("2. Словоформы Места", DialogConsoleColors.Сообщение);

label_start://метка для goto - хотя я отвык от него 
            engine.OperatorConsole.SureConsoleCursorStart();
            engine.OperatorConsole.PrintTextLine("Нужно ввести все падежные формы названия места для всех синонимов.", DialogConsoleColors.Сообщение);
            SpeakDialogResult sdr = engine.OperatorConsole.PrintДаНетОтмена("Желаете ввести сразу все словоформы одной строкой?");
            if (sdr == SpeakDialogResult.Да)
            {
                engine.OperatorConsole.PrintTextLine("Нужно ввести падежные формы (И,Р,Д,В,Т,П) разделяя их запятыми.", DialogConsoleColors.Сообщение);
                engine.OperatorConsole.PrintTextLine("Пример: место, места, месту, места, местом, месте", DialogConsoleColors.Сообщение);
                syno = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите строку словоформ названия Места:", true, true);
                //результат ввода ?
                if (Dialogs.этоОтмена(syno))
                {
                    //возможно, пользователь решил вводить словоформы раздельно.
                    goto label_start;
                }
                //выходим из свитча
            }
            else if (sdr == SpeakDialogResult.Нет)
            {
                //вводить словоформы по отдельности - длинная функция
                syno = ВводитьСловоформыПоОтдельности(engine);
                if (String.IsNullOrEmpty(syno))//если пользователь решил отменить процесс
                    return false;
                //выходим из свитча
            }
            else //введено Отмена
                return false;

            //Тут словоформы надо проверить и записать в объект места
            if (Place.checkSynonimString(syno) == false)
            {
                engine.OperatorConsole.SureConsoleCursorStart();
                engine.OperatorConsole.PrintTextLine("Ошибка! Строка словоформ имеет неправильный формат.", DialogConsoleColors.Предупреждение);
                goto label_start;
            }
            p.Synonim = syno;//проверим их позже

            return true;
        }

        /// <summary>
        /// NT-Вводить словоформы Места по отдельности
        /// </summary>
        /// <param name="engine">Объект Движка</param>
        /// <returns>Возвращает строку словоформ или пустую строку при ошибке или отмене</returns>
        private static string ВводитьСловоформыПоОтдельности(Engine engine)
        {
            String result = String.Empty;//тут собирается строка словоформ, после сбора всех словоформ.
            List<String> synoList = new List<string>();//тут собираются введенные словоформы
            

        label_start:    
            //начинаем опрос
            engine.OperatorConsole.SureConsoleCursorStart();
            engine.OperatorConsole.PrintTextLine("Введите словоформы по одной согласно падежу:", DialogConsoleColors.Сообщение);
            //запрос падежных форм по очереди
            foreach (String quest in PlaceProcedures.ВопросыПадежныхФорм)
            {
                engine.OperatorConsole.SureConsoleCursorStart();
                String wordform = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, quest, false, true);
                if (Dialogs.этоОтмена(wordform))
                    goto label_doExit;//на выход, пользователю надоело вводить словоформы.
                else
                    synoList.Add(wordform);
            }
            //собираем строку словоформ и показываем ее пользователю
            result = String.Join(", ", synoList.ToArray());
            engine.OperatorConsole.SureConsoleCursorStart();
            engine.OperatorConsole.PrintTextLine(String.Format("Строка словоформ: {0}", result), DialogConsoleColors.Сообщение);
            //переход на следующий синоним места?
            SpeakDialogResult sdr = engine.OperatorConsole.PrintДаНетОтмена("Желаете ввести словоформы еще одного синонима места?");
            if (sdr == SpeakDialogResult.Да)
                goto label_start;//снова собираем словоформы для нового синонима места
            else if (sdr == SpeakDialogResult.Нет)
                return result;//выходим со строкой словоформ
            else
                return String.Empty;//выходим с флагом отмены процедуры

        label_doExit:
            //выходим с последней собранной строкой, если она была собрана. Или с флагом отмены, если строка не была собрана
            //то есть, выходим с строкой, собранной из последнего полного комплекта ответов синонима, если они вообще были.
            return result;
        }

        /// <summary>
        /// NT-Обработчик процедуры вывода пользователю списка Мест
        /// </summary>
        /// <param name="engine">Механизм исполнения команд</param>
        /// <param name="cmdline">Текст запроса для возможной дополнительной обработки</param>
        /// <param name="args">Список аргументов</param>
        /// <returns>
        /// Вернуть ProcedureResult.Success в случае успешного выполнения команды.
        /// Вернуть ProcedureResult.WrongArguments если аргументы не подходят для запуска команды.
        /// Вернуть ProcedureResult.Error если произошла ошибка при выполнении операции
        /// Вернуть ProcedureResult.ExitXXX если нужно завершить работу текущего приложения, выключить или перезагрузить компьютер.
        /// </returns>
        [ProcedureAttribute(ImplementationState.NotTested)]
        public static ProcedureResult CommandListPlaces(Engine engine, string query, ArgumentCollection args)
        {
            //TODO: вывести это тестовое сообщение о начале процедуры - в лог!
            engine.OperatorConsole.PrintTextLine("Начата процедура CommandListPlaces()", DialogConsoleColors.Сообщение);
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("Список всех Мест Оператора", DialogConsoleColors.Сообщение);
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintListOfPlaces();
            engine.OperatorConsole.PrintEmptyLine();
            //вывести сообщение о результате операции: успешно
            engine.OperatorConsole.PrintTextLine("Выведен список Мест", DialogConsoleColors.Успех);

            //вернуть флаг продолжения работы
            return ProcedureResult.Success;
        }



    }
}
