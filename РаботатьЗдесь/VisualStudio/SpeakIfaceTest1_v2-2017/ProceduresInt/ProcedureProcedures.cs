using System;
using System.Collections.Generic;
using System.Text;
using Operator;
using Operator.Lexicon;

namespace ProceduresInt
{
    /// <summary>
    /// Встроенные в Оператор процедуры команд работы с Процедурами
    /// </summary>
    public static class ProcedureProcedures
    {
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
        /// Обработчик процедуры создания новой процедуры
        /// </summary>
        /// <param name="engine">Механизм исполнения команд</param>
        /// <param name="cmdline">Текст запроса для возможной дополнительной обработки</param>
        /// <param name="args">Список аргументов</param>
        /// <returns></returns>
        [ProcedureAttribute(ImplementationState.NotTested)]
        public static ProcedureResult CommandCreateProcedure(Engine engine, string query, ArgumentCollection args)
        {
            //содержимое списка аргументов
            //args[0].name = "команда" - название аргумента в строке регекса команды
            //args[0].value = "Скачать файл ХХХ" - значение аргумента - название создаваемого места
            //args[0].type = "" - тип аргумента - TODO: сейчас не указывается, так как мне лень думать 

            String str = String.Format("Начата процедура CommandCreateProcedure(\"{0}\")", args.Arguments[0].ArgumentValue);
            //TODO: вывести это тестовое сообщение о начале процедуры - в лог!
            engine.OperatorConsole.PrintTextLine(str, DialogConsoleColors.Сообщение);

            Procedure proc = new Procedure();//новый пустой объект для заполнения
  
            //Как создать процедуру создания Процедуры?
            //1 извлечь из аргументов название Процедуры, если оно есть
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("1. Название Команды", DialogConsoleColors.Сообщение);

            //извлечь название процедуры из аргумента
            FuncArgument arg = args.Arguments[0];
            str = arg.ArgumentQueryValue.Trim();//берем сырой текст аргумента из запроса
            engine.OperatorConsole.PrintTextLine(String.Format("Название новой Команды: {0}", str), DialogConsoleColors.Сообщение);

            //TODO: проверить признак того, что вместо названия процедуры движком было подставлено название зарегистрированного места
            if (arg.АвтоподстановкаМеста == true)
            {
                ; //TODO: обработать тут случай автоподстановки места вместо названия команды
                //если он возникнет
            }

            //2 проверить что в БД нет Процедуры с таким названием, без учета регистра символов
            //TODO: я просто скопировал этот код из похожей процедуры создания мест, и не знаю, подойдет ли он.

            bool notUnicalProcedure = false;
            while (true)
            {
                //если название команды - пустая строка, вывести сообщение и перейти к приему нового названия команды
                if (String.IsNullOrEmpty(str))
                    engine.OperatorConsole.PrintTextLine("Пустая строка недопустима для названия Команды!", DialogConsoleColors.Предупреждение);
                else
                {
                    //проверить что в БД нет Процедуры с таким названием, без учета регистра символов
                    List<Procedure> lip = engine.DbGetProceduresByTitle(str);
                    notUnicalProcedure = (lip.Count > 0);//временный флаг для упрощения проверок позже
                    if (notUnicalProcedure)
                    {
                        //тут вывести пользователю найденные команды с тем же названием
                        engine.OperatorConsole.PrintTextLine("Команды с таким названием уже существуют:", DialogConsoleColors.Предупреждение);
                        foreach (Procedure pp in lip)
                        {
                            engine.OperatorConsole.PrintProcedureShortLine(lip[0]);
                        }
                        engine.OperatorConsole.PrintTextLine("Дубликаты Команд недопустимы!", DialogConsoleColors.Предупреждение);
                        lip.Clear();//очистить временный список, поскольку он в цикле 
                    }
                }
                if ((str == String.Empty) || (notUnicalProcedure == true))
                {
                    //Раз есть такие Процедуры, пользователь должен сменить название Процедуры прямо тут же 
                    //или же завершить диалог Отменой создания команды
                    str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите новое название Команды:", false, true);
                    if (Dialogs.этоОтмена(str))
                        return ProcedureResult.CancelledByUser;
                }
                else
                    break;//end while loop
            }//while loop
            //Тут мы окажемся, если название Процедуры уникальное
            proc.Title = str;



            //2 Пользователь должен ввести описание процедуры
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("2. Описание Команды", DialogConsoleColors.Сообщение);
            str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите краткое описание Команды:", true, true);
            if (Dialogs.этоОтмена(str))
                return ProcedureResult.CancelledByUser;
            proc.Description = str;


            //3. Пользователь должен ввести регекс процедуры
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("3. Регекс Команды", DialogConsoleColors.Сообщение);
            //TODO: для Пользователя нужно вывести краткую справку с примерами регекса
            string[] regexDescr = new String[] {" - Команда будет выбрана для исполнения, если ее Регекс опознает текст, введенный Пользователем", 
                " - Простой Регекс содержит текст Команды и аргументы. Аргумент обозначается словом с знаком % перед ним.",
                "   Например: Открыть сайт %Аргумент", 
                " - Сложный Регекс это специально форматированный текст. ",
                " - Обратитесь к документации, чтобы узнать больше о Регексе Команды",
            String.Empty };
            engine.OperatorConsole.PrintTextLines(regexDescr, DialogConsoleColors.Сообщение);

            //простым и сложным форматом регекса. Целую краткую инструкцию  и примеры
            str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите регекс для Команды:", true, true);
            if (Dialogs.этоОтмена(str))
                return ProcedureResult.CancelledByUser;
            //TODO: я пока не знаю, как проверить регекс в этом месте. Поэтому просто поверим, что пользователь все сделал правильно.
            proc.Regex = str;


            //4. Пользователь должен ввести путь к процедуре
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("4. Адрес Процедуры Команды", DialogConsoleColors.Сообщение);
            //TODO: для Пользователя нужно вывести краткую справку с примерами путей
            string[] adresDescr = new String[] {" - Описывает командную строку исполняемого файла или путь к Процедуре Команды в Сборке Процедур", 
                " - Для исполняемых файлов, используемых в качестве Процедур, путь может содержать аргументы.",
                "   Например: C:\\Program Files\\Windows Media Player\\wmplayer.exe %место",
                "   Аргументы идентифицируются по своим именам, заданным в Регексе Команды.",
                " - Для Процедур из СборкиПроцедур прописывается путь в формате СборкаПроцедур.Класс.Функция().",
                "   Аргументы идентифицируются внутри кода функции Процедуры, по своим именам, заданным в Регексе Команды.",
                "   Например: ProceduresInt.ProcedureProcedures.CommandCreateProcedure()",
                " - Обратитесь к документации, чтобы узнать больше о Адресе Процедуры Команды",
            String.Empty };
            engine.OperatorConsole.PrintTextLines(adresDescr, DialogConsoleColors.Сообщение);

            str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите адрес Процедуры для Команды:", true, true);
            if (Dialogs.этоОтмена(str))
                return ProcedureResult.CancelledByUser;
            //TODO: надо проверить что файл или сборка, класс, функция - существуют, и если нет - сообщить об этом пользователю.
            //  но это не критичная ошибка - пользователь может заново ввести или применить это значение, несмотря на. 
            proc.Path = str;


            //5. Пользователь должен ввести Вес процедуры
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("5. Вес Команды", DialogConsoleColors.Сообщение);
            // для Пользователя нужно вывести краткую справку по Вес процедуры
            string[] vesDescr = new String[] {" - Вес определяет порядок выбора для исполнения одной из Команд, подходящих по Регексу", 
                " - Команда с наибольшим Весом будет выполнена последней", 
                " - Вес должен быть больше 0,0 и меньше 1,0",
                " - Для новой Команды рекомендуется значение 0,5",
            String.Empty };
            engine.OperatorConsole.PrintTextLines(vesDescr, DialogConsoleColors.Сообщение);
            //ввод значения
            bool isValidValue = false;
                str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите Вес Команды:", true, true);
                if (Dialogs.этоОтмена(str))
                    return ProcedureResult.CancelledByUser;
            do 
            {
                //TODO: надо проверить что введенное значение конвертируется в Double, больше 0 и меньше 1.
                isValidValue = Procedure.IsValidVesFormat(str, engine.OperatorConsole.RuCulture);
                //если значение веса неправильное, то сообщить об этом и запросить новое значение, в цикле, пока пользователь не отменит всю команду.
                if (isValidValue == false)
                {
                    engine.OperatorConsole.PrintTextLine("Это значение Веса является недопустимым!", DialogConsoleColors.Предупреждение);
                    str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите новое значение Веса для Команды:", true, true);
                    if (Dialogs.этоОтмена(str))
                        return ProcedureResult.CancelledByUser;
                }
            }
            while(isValidValue == false);
            
            proc.Ves = Double.Parse(str, engine.OperatorConsole.RuCulture);

            //6. Вывести свойства Процедуры и запросить подтверждение создания процедуры.
            //Вроде все свойства должны быть заполнены, теперь надо вывести все их в форме
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("6. Подтвердите создание Команды", DialogConsoleColors.Сообщение);//пункт плана
            engine.OperatorConsole.PrintProcedureForm(proc);
            //и запросить подтверждение пользователя, что он желает создать Место
            //Если пользователь ответит Да, надо создать место.
            //Если пользователь ответит Нет или Отмена, отменить операцию.
            SpeakDialogResult sdr = engine.OperatorConsole.PrintДаНетОтмена("Желаете создать новую Команду?");
            if ((sdr == SpeakDialogResult.Отмена) || (sdr == SpeakDialogResult.Нет))
                return ProcedureResult.CancelledByUser;

            //9 заполнить объект Процедуры и создать новую процедуру в БД
            engine.DbInsertProcedure(proc);
            //10 вывести сообщение о результате операции: успешно
            engine.OperatorConsole.PrintTextLine(String.Format("Команда \"{0}\" создана успешно", proc.Title), DialogConsoleColors.Успех);

            //вернуть флаг продолжения работы
            return ProcedureResult.Success;
        }

        /// <summary>
        /// NR-Обработчик процедуры вывода пользователю списка Команд
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
        public static ProcedureResult CommandListProcedures(Engine engine, string query, ArgumentCollection args)
        {
            //TODO: вывести это тестовое сообщение о начале процедуры - в лог!
            engine.OperatorConsole.PrintTextLine("Начата процедура CommandListProcedures()", DialogConsoleColors.Сообщение);
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintTextLine("Список всех Команд Оператора:", DialogConsoleColors.Сообщение);
            engine.OperatorConsole.PrintEmptyLine();
            engine.OperatorConsole.PrintListOfProcedures();
            engine.OperatorConsole.PrintEmptyLine();
            //вывести сообщение о результате операции: успешно
            engine.OperatorConsole.PrintTextLine("Выведен список Команд", DialogConsoleColors.Успех);
            //вернуть флаг продолжения работы
            return ProcedureResult.Success;
        }

    }
}
