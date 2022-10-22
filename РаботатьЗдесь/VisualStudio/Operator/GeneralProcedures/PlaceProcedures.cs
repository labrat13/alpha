using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralProcedures
{
    internal class PlaceProcedures
    {
    }

    /**
 * Класс тестовых Процедур для Оператор.
 * 
 * @author Селяков Павел
 *
 */
    @OperatorProcedure(State = ImplementationState.Ready,
            Title = "PlaceProcedures",
            Description = "Класс содержит Процедуры для операций с Местами.")
public class PlaceProcedures
    {

        // #region Константы
        /**
         * Массив строк вопросов для ввода падежных форм
         */
        private final static String[] ВопросыПадежныхФорм = { "И.п. Это что?",
            "Р.п. Владелец чего?", "Д.п. Дали чему?", "В.п. Обвинили что?",
            "Тв.п. Творимый чем?", "Пр.п. Рассказ о чем?" };

        // #endregion

        // Для всех операций с Процедурами и Местами из кода Процедур использовать класс ElementCacheManager, а не БД итп.
        // Пример: engine.get_ECM().AddPlace(p);

        // Памятка: Если аннотация @OperatorProcedure не указана, Движок будет выдавать исключение, что соответствующий элемент не помечен аннотацией, и не будет
        // исполнять Процедуру.
        // Если @OperatorProcedure.State = ImplementationState.NotRealized, Движок будет выдавать исключение, что соответствующий элемент не реализован, и не будет
        // исполнять Процедуру.
        // Если @OperatorProcedure.State = ImplementationState.NotTested, Движок будет выдавать сообщение, что соответствующий элемент не тестирован, и будет
        // исполнять Процедуру.
        // Если @OperatorProcedure.State = ImplementationState.Ready, Движок будет исполнять Процедуру.
        // Следовательно:
        // - Если вы недописали Процедуру, пометьте ее ImplementationState.NotRealized, чтобы Оператор не пытался исполнять эту Процедуру.
        // - Если вы дописали но не протестировали Процедуру, пометьте ее ImplementationState.NotTested, чтобы Оператор выводил предупреждение, что запускаемая
        // Процедура не проверена.
        // - Если вы протестировали Процедуру, и она правильно работает, пометьте ее ImplementationState.Ready, чтобы Оператор не выводил ненужные более
        // предупреждения.

        // Заготовка функции обработчика Процедуры - скопируйте в нужный класс и выполните тодо.

        // /**
        // * NR-Обработчик процедуры Шаблон обработчика процедуры.
        // *
        // *
        // * @param engine
        // * Ссылка на объект Движка Оператор для доступа к консоли, логу, БД итп.
        // * @param manager
        // * Ссылка на объект Менеджера Библиотеки Процедур для доступа к инициализированным ресурсам библиотеки.
        // * @param query
        // * Текст исходного запроса пользователя для возможной дополнительной обработки.
        // * @param args
        // * Массив аргументов Процедуры, соответствующий запросу.
        // * @return Функция возвращает результат как одно из значений EnumProcedureResult:
        // * EnumProcedureResult.Success если Процедура выполнена успешно;
        // * EnumProcedureResult.WrongArguments если аргументы не подходят для запуска Процедуры;
        // * EnumProcedureResult.Error если произошла ошибка при выполнении Процедуры;
        // * EnumProcedureResult.CancelledByUser если выполнение Процедуры прервано Пользователем;
        // * EnumProcedureResult.Exit если после выполнения Процедуры требуется завершить работу Оператор;
        // * EnumProcedureResult.ExitAndLogoff если после выполнения Процедуры требуется завершить сеанс пользователя;
        // * EnumProcedureResult.ExitAndHybernate если после выполнения Процедуры требуется перевести компьютер в спящий режим;
        // * EnumProcedureResult.ExitAndSleep если после выполнения Процедуры требуется перевести компьютер в спящий режим;
        // * EnumProcedureResult.ExitAndReload если после выполнения Процедуры требуется перезагрузить компьютер;
        // * EnumProcedureResult.ExitAndShutdown если после выполнения Процедуры требуется выключить компьютер;
        // */
        // @OperatorProcedure(State = ImplementationState.NotRealized, // TODO: заменить на актуальное
        // Title = "Название команды", // TODO: заменить название команды на актуальное
        // Description = "Однострочное описание команды.") // TODO: заменить описание команды на актуальное
        // public static EnumProcedureResult НазваниеШаблона(
        // Engine engine,
        // LibraryManagerBase manager,
        // UserQuery query,
        // ArgumentCollection args)
        // {
        // /*
        // * 07042022 - Добавлена возможность внутри Процедуры изменять текст запроса,
        // * чтобы применить новый текст запроса к дальнейшему поиску Процедур.
        // * Изменение запроса не перезапускает поиск Процедур (в текущей версии Оператора).
        // * Поэтому изменять запрос следует только в хорошо продуманных случаях.
        // *
        // * Пример вызова функции переопределения запроса, с выводом в лог старого и нового значений.
        // * Example: query.ChangeQuery(engine, "New query text");
        // */
        //
        // // TODO: Не забудьте добавить эту Процедуру в LibraryManager.getLibraryProcedures() функцию, чтобы она была добавлена в Оператор.
        //
        // EnumProcedureResult result = EnumProcedureResult.Success;
        // // название текущей процедуры для лога итп.
        // // TODO: указать здесь полный путь как название процедуры для вывода на экран.
        // String currentProcedureTitle = "НазваниеБиблиотеки.НазваниеКласса.НазваниеФункции";
        // // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
        // // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
        // try
        // {
        // // вывести в лог тестовое сообщение о начале процедуры
        // String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, args.getByIndex(0).get_ArgumentValue());
        // engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);
        //
        // // TODO: код алгоритма добавить здесь
        //
        // // TODO: вывести сообщение о результате операции: успешно
        // engine.get_OperatorConsole().PrintTextLine("Команда успешно завершена.", EnumDialogConsoleColor.Успех);
        // }
        // catch (Exception ex)
        // {
        // engine.PrintExceptionMessageToConsoleAndLog("Ошибка в процедуре " + currentProcedureTitle + "()", ex);
        // result = EnumProcedureResult.Error;
        // }
        //
        // // вернуть флаг продолжения работы
        // return result;
        // }

        /**
         * NT-Обработчик процедуры Создать место НазваниеМеста.
         * 
         * 
         * @param engine
         *            Ссылка на объект Движка Оператор для доступа к консоли, логу, БД итп.
         * @param manager
         *            Ссылка на объект Менеджера Библиотеки Процедур для доступа к инициализированным ресурсам библиотеки.
         * @param query
         *            Текст исходного запроса пользователя для возможной дополнительной обработки.
         * @param args
         *            Массив аргументов Процедуры, соответствующий запросу.
         * @return Функция возвращает результат как одно из значений EnumProcedureResult:
         *         EnumProcedureResult.Success если Процедура выполнена успешно;
         *         EnumProcedureResult.WrongArguments если аргументы не подходят для запуска Процедуры;
         *         EnumProcedureResult.Error если произошла ошибка при выполнении Процедуры;
         *         EnumProcedureResult.CancelledByUser если выполнение Процедуры прервано Пользователем;
         *         EnumProcedureResult.Exit если после выполнения Процедуры требуется завершить работу Оператор;
         *         EnumProcedureResult.ExitAndLogoff если после выполнения Процедуры требуется завершить сеанс пользователя;
         *         EnumProcedureResult.ExitAndHybernate если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndSleep если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndReload если после выполнения Процедуры требуется перезагрузить компьютер;
         *         EnumProcedureResult.ExitAndShutdown если после выполнения Процедуры требуется выключить компьютер;
         */
        @OperatorProcedure(State = ImplementationState.Ready,
                Title = "Создать место НазваниеМеста",
                Description = "Создать Место в БазаДанныхОператора.")
    public static EnumProcedureResult CommandCreatePlace(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {
            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога и вывода на экран.
            String currentProcedureTitle = "GeneralProcedures.PlaceProcedures.CommandCreatePlace";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                // 1. Извлечь из аргумента название команды
                String placeTitle;
                FuncArgument arg = args.getByIndex(0);
                placeTitle = arg.get_ArgumentQueryValue().Trim();// берем сырой текст аргумента из запроса

                // вывести это тестовое сообщение о начале процедуры в лог
                String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, placeTitle);
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);

                Place place = new Place();// новый пустой объект для заполнения

                // 1 извлечь из аргументов название Места, если оно есть

                // ПроблемаАвтоподстановкиМест. Если место уже существует, движок подставляет его в этот запрос в качестве аргумента.
                // Поэтому надо проверять поле типа аргумента. Если оно пустое, то аргумент из командной строки пришел как слово.
                // (Поэтому нельзя типы аргументов при разборе строки назначать, если это не подставленное Место)
                // А если тип есть, то это было место подставленное движком. Тогда ArgumentName = название аргумента из регекса, ArgumentValue=ПутьМеста
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("1. Название Места", EnumDialogConsoleColor.Сообщение);

                // извлечь название места из аргумента
                engine.get_OperatorConsole().PrintTextLine(String.format("Название нового Места: \"%s\"", placeTitle), EnumDialogConsoleColor.Сообщение);

                // проверить признак того, что место уже существует и было подставлено в запрос при разборе запроса движком.
                if (arg.get_АвтоподстановкаМеста() == true)
                {
                    // обработать тут случай автоподстановки места вместо названия команды, если он возникнет
                    engine.get_OperatorConsole().PrintTextLine("Место с таким названием уже существует:", EnumDialogConsoleColor.Предупреждение);
                    engine.get_OperatorConsole().PrintPlaceShortLine(arg.get_AssociatedPlace());
                    return EnumProcedureResult.Error;
                }

                // 2 проверить что в БД нет Места с таким названием, без учета регистра символов
                // эта процедура обычно не вызывается без названия места
                // а неуникальное название места - оригинальное место подставляется движком, и выпиливается вышележащей проверкой
                // Но! иногда в БД есть Места, синонимы которых отличаются от значения свойства Название Места.
                // Вот они-то и попадают в такой оборот, как здесь.
                // Это места, названия которые совпадают с аргументом процедуры, но не с синонимами Места. Редкий случай.
                InOutArgument outResult;  // temp var reference
                EnumProcedureResult epr; // temp var reference

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readPlaceTitleForNew(engine, outResult, placeTitle);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set regex value
                    str = outResult.getValueString();
                    place.set_Title(str);
                }

                // 3 ввести синонимы названия Места
                // именно сейчас. Так как если они не уникальные, то, возможно, придется сменить также и название места.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("2. Словоформы Места", EnumDialogConsoleColor.Сообщение);
                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readPlaceSynonims(engine, outResult, placeTitle, place);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set regex value
                    str = outResult.getValueString();
                    place.set_Synonim(str);
                }

                // 4 Ввести описание места одной строкой. Нажатие Enter завершит ввод описания.
                // Поэтому описание места должно быть коротким и емким.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("3. Описание Места", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readPlaceDescription(engine, outResult);
                // Если пользователь введет Отмена, завершить диалог Отменой создания места
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set regex value
                    str = outResult.getValueString();
                    place.set_Description(str);
                }

                // 6 Ввести путь к Месту, веб-путь к Сущности, файловый путь к файлу.
                // Если пользователь введет Отмена, завершить диалог Отменой создания места
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("4. Адрес Места", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readPlaceAdres(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set namespace value
                    str = outResult.getValueString();
                    place.set_Path(str);
                }

                // 7 Ввести строку описания класса Сущности. Она имеет специальный формат.
                // пользователю потребуется дополнительно справочник по этим классам.
                // Если пользователь введет Отмена, завершить диалог Отменой создания места
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("5. Класс Места", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readPlaceClass(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set namespace value
                    str = outResult.getValueString();
                    place.set_PlaceTypeExpression(str);
                }

                // 6. Пользователь должен ввести неймспейс для Места.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("6. Категория для Места", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readPlaceCategory(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set namespace value
                    str = outResult.getValueString();
                    place.set_Namespace(str.Trim());
                }

                // 7. Вывести свойства Места и запросить подтверждение создания Места.
                // Вроде все свойства должны быть заполнены, теперь надо вывести все их в форме

                // сначала добавить название хранилища в объект Места - согласно концепту Библиотек Процедур Оператора.
                place.set_Storage(Procedure.StorageKeyForDatabaseItem);

                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("7. Подтвердите создание Места", EnumDialogConsoleColor.Сообщение);// пункт плана
                engine.get_OperatorConsole().PrintPlaceForm(place);
                // и запросить подтверждение пользователя, что он желает создать Место
                // Если пользователь ответит Да, надо создать место.
                // Если пользователь ответит Нет или Отмена, отменить операцию.
                EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете создать новое Место?");
                if (sdr.isНет() || sdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;

                // 8. заполнить объект Места и создать новое Место в БД
                engine.get_ECM().AddPlace(place);

                // 9. вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine(String.format("Место \"%s\" создано успешно", place.get_Title()), EnumDialogConsoleColor.Успех);

                // вернуть флаг успеха - уже установлен в начале функции, поэтому не будем здесь.
            }
            catch (Exception ex)
            {
                engine.PrintExceptionMessageToConsoleAndLog("Ошибка в процедуре " + currentProcedureTitle + "()", ex);
                result = EnumProcedureResult.Error;
            }

            // вернуть флаг продолжения работы
            return result;
        }

        /**
         * NT-Обработчик процедуры Показать Места.
         * 
         * 
         * @param engine
         *            Ссылка на объект Движка Оператор для доступа к консоли, логу, БД итп.
         * @param manager
         *            Ссылка на объект Менеджера Библиотеки Процедур для доступа к инициализированным ресурсам библиотеки.
         * @param query
         *            Текст исходного запроса пользователя для возможной дополнительной обработки.
         * @param args
         *            Массив аргументов Процедуры, соответствующий запросу.
         * @return Функция возвращает результат как одно из значений EnumProcedureResult:
         *         EnumProcedureResult.Success если Процедура выполнена успешно;
         *         EnumProcedureResult.WrongArguments если аргументы не подходят для запуска Процедуры;
         *         EnumProcedureResult.Error если произошла ошибка при выполнении Процедуры;
         *         EnumProcedureResult.CancelledByUser если выполнение Процедуры прервано Пользователем;
         *         EnumProcedureResult.Exit если после выполнения Процедуры требуется завершить работу Оператор;
         *         EnumProcedureResult.ExitAndLogoff если после выполнения Процедуры требуется завершить сеанс пользователя;
         *         EnumProcedureResult.ExitAndHybernate если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndSleep если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndReload если после выполнения Процедуры требуется перезагрузить компьютер;
         *         EnumProcedureResult.ExitAndShutdown если после выполнения Процедуры требуется выключить компьютер;
         */
        @OperatorProcedure(State = ImplementationState.Ready,   // заменить на актуальное
                Title = "Показать места",
                Description = "Вывести на экран список Мест Оператор")
    public static EnumProcedureResult CommandListPlaces(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {
            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.PlaceProcedures.CommandListPlaces";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                // вывести это тестовое сообщение о начале процедуры на консоль и в лог
                String str = String.format("Начата процедура %s()", currentProcedureTitle);
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("Список всех Мест Оператора:", EnumDialogConsoleColor.Сообщение);
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintListOfPlaces();
                engine.get_OperatorConsole().PrintEmptyLine();
                // вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine("Выведен список Мест", EnumDialogConsoleColor.Успех);
            }
            catch (Exception ex)
            {
                engine.PrintExceptionMessageToConsoleAndLog("Ошибка в процедуре " + currentProcedureTitle + "()", ex);
                result = EnumProcedureResult.Error;
            }

            // вернуть флаг продолжения работы
            return result;
        }

        /**
         * NT-Обработчик процедуры Удалить место НазваниеМеста.
         * 
         * 
         * @param engine
         *            Ссылка на объект Движка Оператор для доступа к консоли, логу, БД итп.
         * @param manager
         *            Ссылка на объект Менеджера Библиотеки Процедур для доступа к инициализированным ресурсам библиотеки.
         * @param query
         *            Текст исходного запроса пользователя для возможной дополнительной обработки.
         * @param args
         *            Массив аргументов Процедуры, соответствующий запросу.
         * @return Функция возвращает результат как одно из значений EnumProcedureResult:
         *         EnumProcedureResult.Success если Процедура выполнена успешно;
         *         EnumProcedureResult.WrongArguments если аргументы не подходят для запуска Процедуры;
         *         EnumProcedureResult.Error если произошла ошибка при выполнении Процедуры;
         *         EnumProcedureResult.CancelledByUser если выполнение Процедуры прервано Пользователем;
         *         EnumProcedureResult.Exit если после выполнения Процедуры требуется завершить работу Оператор;
         *         EnumProcedureResult.ExitAndLogoff если после выполнения Процедуры требуется завершить сеанс пользователя;
         *         EnumProcedureResult.ExitAndHybernate если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndSleep если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndReload если после выполнения Процедуры требуется перезагрузить компьютер;
         *         EnumProcedureResult.ExitAndShutdown если после выполнения Процедуры требуется выключить компьютер;
         */
        @OperatorProcedure(State = ImplementationState.Ready,   // заменить на актуальное
                Title = "Удалить место НазваниеМеста",
                Description = "Удалить указанное Место из БазаДанныхОператор")
    public static EnumProcedureResult CommandDeletePlace(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {
            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.PlaceProcedures.CommandDeletePlace";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                // 1. Извлечь из аргумента название Места
                String placeTitle;
                FuncArgument arg = args.getByIndex(0);
                placeTitle = arg.get_ArgumentQueryValue().Trim();// берем сырой текст аргумента из запроса

                // вывести это тестовое сообщение о начале процедуры в лог
                String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, placeTitle);
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);

                // 1. Извлечь из аргумента название команды
                engine.get_OperatorConsole().PrintTextLine(String.format("Название удаляемого Места: \"%s\"", placeTitle), EnumDialogConsoleColor.Сообщение);

                // 2. извлечь из БД список команд с таким названием и показать пользователю.
                // без учета регистра символов
                // DONE: весь этот пункт 2 надо превратить в отдельную функцию выбора процедуры по ее названию, чтобы использовать ее во многих местах этого класса.
                Place place = null;

                InOutArgument outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                EnumProcedureResult epr = selectPlace(engine, placeTitle, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                // если Место не найдено, selectPlace возвращает Error, а текущая процедура должна вернуть Success,
                // так как не ее проблема, что нечего удалять.
                else if (epr == EnumProcedureResult.Error)
                    return EnumProcedureResult.Success;
                else
                {
                    // set Place object
                    place = outResult.getValuePlace();
                }
                // Тут в объекте place уже должна быть выбранное Место.

                // 3. выбранную пользователем Место - проверить, что оно может быть удалено
                // - она может быть удалена, если она находится в БД. Сейчас БиблиотекиПроцедур не позволяют удалять или редактировать свои Процедуры и Места.
                // - надо добавить в объект Процедуры и Места (Item) флаг ReadOnly, для БД он сброшен, для Библиотек Процедур он установлен.
                // - Флаг не должен храниться в БД, только в памяти.
                // - TODO: решено позже добавить в объект Item ссылку на объект Хранилища, унифицированный для БД и БиблиотекаПроцедур,
                // и из него запрашивать возможность удаления и изменения итемов. А пока только НазваниеХранилища для этого есть.
                if (place.isItemCanRemoved() == false)
                {
                    // Если указанное Место не может быть удалено, вывести сообщение об этом и завершить текущую Процедуру успешно.
                    String st2 = String.format("Выбранное Место \"%s\" не может быть удалено из его хранилища \"%s\"", placeTitle, place.get_Storage());
                    engine.get_OperatorConsole().PrintTextLine(st2, EnumDialogConsoleColor.Предупреждение);
                    return EnumProcedureResult.Success;
                }
                // else

                // 4. Показать пользователю свойства выбранного Места и запросить подтверждение удаления.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("Подтвердите удаление Места", EnumDialogConsoleColor.Сообщение);
                engine.get_OperatorConsole().PrintPlaceForm(place);
                // и запросить подтверждение пользователя, что он желает удалить Место.
                // Если пользователь ответит Да, надо удалить Место.
                // Если пользователь ответит Нет или Отмена, отменить операцию.
                EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете удалить Место?");
                if (sdr.isНет() || sdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                // 5. Удалить команду.
                engine.get_ECM().RemovePlace(place);

                // 6. вывести сообщение о результате операции: успешно
                String msg6 = String.format("Место \"%s\" успешно удалено.", placeTitle);
                engine.get_OperatorConsole().PrintTextLine(msg6, EnumDialogConsoleColor.Успех);
            }
            catch (Exception ex)
            {
                engine.PrintExceptionMessageToConsoleAndLog("Ошибка в процедуре " + currentProcedureTitle + "()", ex);
                result = EnumProcedureResult.Error;
            }

            // вернуть флаг продолжения работы
            return result;
        }

        /**
         * NT-Обработчик процедуры Изменить место НазваниеМеста.
         * 
         * 
         * @param engine
         *            Ссылка на объект Движка Оператор для доступа к консоли, логу, БД итп.
         * @param manager
         *            Ссылка на объект Менеджера Библиотеки Процедур для доступа к инициализированным ресурсам библиотеки.
         * @param query
         *            Текст исходного запроса пользователя для возможной дополнительной обработки.
         * @param args
         *            Массив аргументов Процедуры, соответствующий запросу.
         * @return Функция возвращает результат как одно из значений EnumProcedureResult:
         *         EnumProcedureResult.Success если Процедура выполнена успешно;
         *         EnumProcedureResult.WrongArguments если аргументы не подходят для запуска Процедуры;
         *         EnumProcedureResult.Error если произошла ошибка при выполнении Процедуры;
         *         EnumProcedureResult.CancelledByUser если выполнение Процедуры прервано Пользователем;
         *         EnumProcedureResult.Exit если после выполнения Процедуры требуется завершить работу Оператор;
         *         EnumProcedureResult.ExitAndLogoff если после выполнения Процедуры требуется завершить сеанс пользователя;
         *         EnumProcedureResult.ExitAndHybernate если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndSleep если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndReload если после выполнения Процедуры требуется перезагрузить компьютер;
         *         EnumProcedureResult.ExitAndShutdown если после выполнения Процедуры требуется выключить компьютер;
         */
        @OperatorProcedure(State = ImplementationState.Ready,   // заменить на актуальное
                Title = "Изменить место НазваниеМеста",
                Description = "Изменить указанное Место Оператора")
    public static EnumProcedureResult CommandChangePlace(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {
            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.PlaceProcedures.CommandChangePlace";

            String stmp; // temp string
            EnumSpeakDialogResult esdr; // temp dialog result
            InOutArgument outResult; // temp in-out argument object
            EnumProcedureResult epr; // temp procedure result
                                     // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
                                     // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                // вывести в лог тестовое сообщение о начале процедуры
                String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, args.getByIndex(0).get_ArgumentValue());
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);

                // 1. Извлечь из аргумента название Места
                String placeTitle;
                FuncArgument arg = args.getByIndex(0);
                placeTitle = arg.get_ArgumentQueryValue().Trim();// берем сырой текст аргумента из запроса
                engine.get_OperatorConsole().PrintTextLine(String.format("Название изменяемого Места: \"%s\"", placeTitle), EnumDialogConsoleColor.Сообщение);
                // 2. извлечь из БД список Мест с таким названием и показать пользователю.
                // без учета регистра символов
                Place place = null;

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = selectPlace(engine, placeTitle, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                // если Место не найдено, selectPlace возвращает Error, а эта процедура должна вернуть Success,
                // так как не ее проблема, что нечего изменять.
                else if (epr == EnumProcedureResult.Error)
                    return EnumProcedureResult.Success;
                else
                {
                    // set Place object
                    place = outResult.getValuePlace();
                }

                // Тут в объекте place уже должна быть выбранное Место.

                // 3. выбранную пользователем Место - проверить, что оно может быть изменено.
                // - она может быть изменена, если она находится в БД. Сейчас БиблиотекиПроцедур не позволяют удалять или редактировать свои Процедуры и Места.
                // - надо добавить в объект Процедуры и Места (Item) флаг ReadOnly, для БД он сброшен, для Библиотек Процедур он установлен.
                // Флаг не хранится в БД, только в памяти.
                // - TODO: решено позже добавить в объект Item ссылку на объект Хранилища, унифицированный для БД и БиблиотекаПроцедур,
                // и из него запрашивать возможность удаления и изменения итемов.
                if (place.isItemCanChanged() == false)
                {
                    // Если указанное Место не может быть изменено, вывести сообщение об этом и завершить текущую Процедуру успешно.
                    stmp = String.format("Выбранное Место \"%s\" не может быть изменено.", placeTitle);
                    engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Предупреждение);
                    return EnumProcedureResult.Success;
                }
                // else
                //создадим копию, чтобы не перезаписывать оригинал из коллекции Мест
                Place placeNew = new Place(place);

                // 4. По каждому свойству Места:
                // - показывать текущее значение свойства и спрашивать, желает ли пользователь его изменить. (Да-Нет-Отмена-Пропустить)
                // Если не желает, перейти к следующему свойству.
                // Если желает, вывести справку, запросить новое значение, проверить его формат и вписать в объект Места.
                // Если отменил, то отменить операцию.

                // 4.1 Название Места
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("1. Название Места", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущее название: \"%s\"", place.get_Title());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить название Места?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readPlaceTitleForExisting(engine, outResult, place);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set regex value
                        stmp = outResult.getValueString();
                        placeNew.set_Title(stmp);
                    }
                }
                // else Нет - перейти к следующему свойству Места.

                // 4.2 Синонимы Места
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("2. Словоформы Места", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущие словоформы: \"%s\"", place.get_Synonim());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить словоформы Места?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readPlaceSynonims(engine, outResult, placeTitle, place);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set regex value
                        stmp = outResult.getValueString();
                        placeNew.set_Synonim(stmp);
                    }
                }
                // else Нет - перейти к следующему свойству Места.

                // 4.2 Описание Места
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("3. Описание Места", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущее описание: \"%s\"", place.get_Description());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить описание Места?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readPlaceDescription(engine, outResult);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set regex value
                        stmp = outResult.getValueString();
                        placeNew.set_Description(stmp);
                    }
                }
                // else Нет - перейти к следующему свойству Места.

                // 4.4 Адрес Места
                // Ввести путь к Месту, веб-путь к Сущности, файловый путь к файлу.
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("4. Адрес Места", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущий адрес: \"%s\"", place.get_Path());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить адрес Места?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readPlaceAdres(engine, outResult);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set namespace value
                        stmp = outResult.getValueString();
                        placeNew.set_Path(stmp);
                    }
                }
                // else Нет - перейти к следующему свойству Места.

                // 4.6 Ввести строку описания класса Сущности. Она имеет специальный формат.
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("5. Класс Места", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущий класс: \"%s\"", place.get_PlaceTypeExpression());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить класс Места?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readPlaceClass(engine, outResult);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set namespace value
                        stmp = outResult.getValueString();
                        placeNew.set_PlaceTypeExpression(stmp.Trim());
                    }
                }
                // else Нет - перейти к следующему свойству Места.

                // 4.6 Категория Места
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("6. Категория для Места", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущая категория: \"%s\"", place.get_Namespace());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить категорию Места?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readPlaceCategory(engine, outResult);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set namespace value
                        stmp = outResult.getValueString();
                        placeNew.set_Namespace(stmp.Trim());
                    }
                }
                // else Нет - перейти к следующему свойству Места.

                // 5. Вывести общий набор свойств Места и запросить пользователя, желает ли он сохранить эти данные.(Да-Нет-Отмена)
                // - если пользователь ответил да, изменить процедуру через ECM функцию.
                // - при любом другом ответе пользователя отменить операцию.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("Подтвердите изменение Места", EnumDialogConsoleColor.Сообщение);
                engine.get_OperatorConsole().PrintPlaceForm(placeNew);
                // и запросить подтверждение пользователя, что он желает изменить Место.
                // Если пользователь ответит Да, надо изменить Место.
                // Если пользователь ответит Нет или Отмена, отменить операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить Место?");
                if (esdr.isНет() || esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                // записать команду
                engine.get_ECM().UpdatePlace(placeNew);

                // 6. вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine(String.format("Место %s успешно изменено", placeTitle), EnumDialogConsoleColor.Успех);
            }
            catch (Exception ex)
            {
                engine.PrintExceptionMessageToConsoleAndLog("Ошибка в процедуре " + currentProcedureTitle + "()", ex);
                result = EnumProcedureResult.Error;
            }

            // вернуть флаг продолжения работы
            return result;
        }

        /**
         * NT-Обработчик процедуры Удалить все места.
         * 
         * 
         * @param engine
         *            Ссылка на объект Движка Оператор для доступа к консоли, логу, БД итп.
         * @param manager
         *            Ссылка на объект Менеджера Библиотеки Процедур для доступа к инициализированным ресурсам библиотеки.
         * @param query
         *            Текст исходного запроса пользователя для возможной дополнительной обработки.
         * @param args
         *            Массив аргументов Процедуры, соответствующий запросу.
         * @return Функция возвращает результат как одно из значений EnumProcedureResult:
         *         EnumProcedureResult.Success если Процедура выполнена успешно;
         *         EnumProcedureResult.WrongArguments если аргументы не подходят для запуска Процедуры;
         *         EnumProcedureResult.Error если произошла ошибка при выполнении Процедуры;
         *         EnumProcedureResult.CancelledByUser если выполнение Процедуры прервано Пользователем;
         *         EnumProcedureResult.Exit если после выполнения Процедуры требуется завершить работу Оператор;
         *         EnumProcedureResult.ExitAndLogoff если после выполнения Процедуры требуется завершить сеанс пользователя;
         *         EnumProcedureResult.ExitAndHybernate если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndSleep если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndReload если после выполнения Процедуры требуется перезагрузить компьютер;
         *         EnumProcedureResult.ExitAndShutdown если после выполнения Процедуры требуется выключить компьютер;
         */
        @OperatorProcedure(State = ImplementationState.Ready,   // заменить на актуальное
                Title = "Удалить все места",
                Description = "Удалить все Места из БазаДанныхОператора.")
    public static EnumProcedureResult CommandDeleteAllPlaces(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {
            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.PlaceProcedures.CommandDeleteAllPlaces";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                // 1. Запросить от пользователя подтверждение о удалении всех Мест из БД. Да - Нет и Отмена.
                // если пользователь ответил Да - удалить все Места из БД
                // если пользователь ответил Нет или Отмена - прервать операцию.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("1. Подтвердите удаление всех мест из БазаДанныхОператора", EnumDialogConsoleColor.Сообщение);
                EnumSpeakDialogResult esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете удалить все Места?");
                if (esdr.isНет() || esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                // выполнить удаление всех Мест из БД Оператора
                engine.get_ECM().RemoveAllPlacesFromDatabase();
                // вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine("Удаление всех Мест завершено успешно", EnumDialogConsoleColor.Успех);
            }
            catch (Exception ex)
            {
                engine.PrintExceptionMessageToConsoleAndLog("Ошибка в процедуре " + currentProcedureTitle + "()", ex);
                result = EnumProcedureResult.Error;
            }

            // вернуть флаг продолжения работы
            return result;
        }

        // *** Вспомогательные процедуры ***

        /**
         * NT-Select Place by Title.
         * 
         * @param engine
         *            Engine object.
         * @param title
         *            Place title.
         * @param outResult
         *            Result Place in shell object.
         * @return Function returns EnumProcedureResult.Success if success, Error if procedure not found, CancelledByUser if cancelled.
         * @throws Exception
         *             Error occured.
         */
        private static EnumProcedureResult selectPlace(
                Engine engine,
                String title,
                InOutArgument outResult) throws Exception
        {
            Place p = null;
            LinkedList<Place> lip = engine.get_ECM().get_PlaceCollection().getByTitle(title);
        int lipSize = lip.size();
        if (lipSize == 0)
        {
            // тут вывести сообщение, что указанное Место не найдено и завершить процедуру успешно.
            String stmp = String.format("Указанное Место \"%s\" не найдено.", title);
        engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Предупреждение);
            return EnumProcedureResult.Error;
        }
        else if (lipSize == 1)
        {
            p = lip.get(0);
        }
        else // if(lipSize > 1)
{
    // тут вывести пользователю найденные Места с тем же названием
    engine.get_OperatorConsole().PrintTextLine("Существующие Места с таким названием:", EnumDialogConsoleColor.Предупреждение);
    // Если в списке более одного Места, то пользователь должен выбрать одно из них для удаления.
    // для этого в списке Мест нужно показать уникальный ИД Места, источник-хранилище, категорию, название и описание.
    // И флаг, возможно ли удаление данного Места - это определяется Хранилищем.
    // Для выбора из нескольких Мест надо запросить у пользователя ИД Места, но проблема в том, что ид сейчас есть только у объектов из БД.
    // - можно вывести порядковый номер в этом списке и запросить у пользователя его.
    engine.get_OperatorConsole().PrintPlaceFormNumberedList(lip);
    // запросить у пользователя порядковый номер элемента списка
    int lip_index = engine.get_OperatorConsole().InputListIndex(lipSize);
    // обработать указанный пользователем индекс
    if (lip_index < 1)
    {
        // Если пользователь отказался выбрать элемент списка, вывести сообщение об этом и завершить текущую Процедуру Отменено пользователем.
        engine.get_OperatorConsole().PrintTextLine("Операция отменена, поскольку пользователь не смог выбрать Место из списка.", EnumDialogConsoleColor.Сообщение);
        return EnumProcedureResult.CancelledByUser;
    }
    else p = lip.get(lip_index);// выбранный пользователем объект команды.
}

// return
outResult.setValue(p);
return EnumProcedureResult.Success;
    }

    /**
     * NT-Read Place Title value from User, for newly create Place.
     * 
     * @param engine
     *            Engine object.
     * @param outResult
     *            Result string shell object.
     * @param oldTitle
     *            Current procedure title string.
     * @return Function returns EnumProcedureResult.Success if success; EnumProcedureResult.CancelledByUser if cancelled.
     * @throws Exception
     *             Error occured.
     */
    private static EnumProcedureResult readPlaceTitleForNew(
            Engine engine,
            InOutArgument outResult,
            String oldTitle) throws Exception
{
    // вывести справку по созданию названия для команды.
    printHelpPlaceTitleProp(engine);

    // temp string with old title
    String str = new String(oldTitle);
// 2 проверить что в БД нет Процедуры с таким названием, без учета регистра символов
Boolean notUnicalPlace = false;
while (true)
{
    // если название места - пустая строка, вывести сообщение и перейти к приему нового названия места
    if (OperatorEngine.Utility.StringIsNullOrEmpty(str))
        engine.get_OperatorConsole().PrintTextLine("Пустая строка недопустима для названия Места!", EnumDialogConsoleColor.Предупреждение);
    else
    {
        // проверить что в БД нет Места с таким названием, без учета регистра символов
        LinkedList<Place> lip = engine.get_ECM().get_PlaceCollection().getByTitle(str);
        notUnicalPlace = (lip.size() > 0);// временный флаг для упрощения проверок позже
        if (notUnicalPlace)
        {
            // тут вывести пользователю найденные места с тем же названием
            engine.get_OperatorConsole().PrintTextLine("Места с таким названием уже существуют:", EnumDialogConsoleColor.Предупреждение);
            for (Place pp : lip)
            {
                engine.get_OperatorConsole().PrintPlaceShortLine(pp);
            }
            engine.get_OperatorConsole().PrintTextLine("Дубликаты Мест недопустимы!", EnumDialogConsoleColor.Предупреждение);
            lip.clear();// очистить временный список, поскольку он в цикле
        }
    }
    if (str.isEmpty() || (notUnicalPlace == true))
    {
        // Раз есть такие Места, пользователь должен сменить название Места прямо тут же
        // или же завершить диалог Отменой создания места
        str = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите новое название Места:", false, true);
        if (Dialogs.этоОтмена(str))
            return EnumProcedureResult.CancelledByUser;
    }
    else break;// end while loop
}// while loop
 // Тут мы окажемся, если название Места уникальное
 // return
outResult.setValue(str);
return EnumProcedureResult.Success;
    }

    /**
     * NT-Read Place Title value from User, for change or rename Place.
     * 
     * @param engine
     *            Engine object.
     * @param outResult
     *            Result title string shell object.
     * @param place
     *            Change Place object.
     * @return Function returns EnumProcedureResult.Success if success; EnumProcedureResult.CancelledByUser if cancelled.
     * @throws Exception
     *             Error occured.
     */
    private static EnumProcedureResult readPlaceTitleForExisting(
            Engine engine,
            InOutArgument outResult,
            Place place) throws Exception
{
    // вывести справку по созданию названия для Места.
    printHelpPlaceTitleProp(engine);
    // temp string with old title
    String str = new String(place.get_Title());
// 2 проверить что в БД нет Места с таким названием, без учета регистра символов, кроме текущей.
Boolean notUnicalPlace = false;
while (true)
{
    // 1 введите новое название, пустые строки не принимаются.
    // Раз есть такие Места, пользователь должен сменить название Места прямо тут же
    // или же завершить диалог Отменой создания Места
    // no empty answer's allowed by arg4! str cannot be empty here.
    str = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите новое название Места:", false, true);

    // 3 если отмена - выйти с отменой
    if (Dialogs.этоОтмена(str))
        return EnumProcedureResult.CancelledByUser;

    // 4 проверить уникальность названия Места
    // если Место уникальное, то выйти с названием Места
    // иначе - сообщить что Место уже существует и перейти на новый цикл.

    // проверить что в БД нет Места с таким названием, без учета регистра символов
    // кроме текущего Места - оно же есть.
    LinkedList<Place> lip = engine.get_ECM().get_PlaceCollection().getByTitle(str);
    // удалить текущий объект Места из этого списка
    if (lip.contains(place))
    {
        lip.remove(place);
    }
    notUnicalPlace = (lip.size() > 0);// временный флаг для упрощения проверок позже
    if (notUnicalPlace)
    {
        // тут вывести пользователю найденные Места с тем же названием
        engine.get_OperatorConsole().PrintTextLine("Места с таким названием уже существуют:", EnumDialogConsoleColor.Предупреждение);
        for (Place pp : lip)
        {
            engine.get_OperatorConsole().PrintPlaceShortLine(pp);
        }
        engine.get_OperatorConsole().PrintTextLine("Дубликаты Мест недопустимы!", EnumDialogConsoleColor.Предупреждение);
        lip.clear();// очистить временный список, поскольку он в цикле
        continue;
    }
    else break;// break while loop

}// while loop
 // Тут мы окажемся, если название Места уникальное
 // return
outResult.setValue(str);
return EnumProcedureResult.Success;
    }

    /**
     * NT-Read Place Description value from User
     * 
     * @param engine
     *            Engine object
     * @param outResult
     *            Result string shell object
     * @return Function returns EnumProcedureResult.Success if success, or other values.
     * @throws Exception
     *             Error occured.
     */
    private static EnumProcedureResult readPlaceDescription(
            Engine engine,
            InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpPlaceDescriptionProp(engine);
    // - запросить новое значение
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите краткое описание Места:", true, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;

    // return
    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Place class value from User
 * 
 * @param engine
 *            Engine object
 * @param outResult
 *            Result string shell object
 * @return Function returns EnumProcedureResult.Success if success, or other values.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult readPlaceClass(
        Engine engine,
        InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpPlaceClassProp(engine);
    //
    String str = null;
        while (true)
        {
        // - запросить новое значение
        str = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите выражение класса для Места:", true, true);
        if (Dialogs.этоОтмена(str))
            return EnumProcedureResult.CancelledByUser;

        // Проверить что введенное выражение не имеет ошибок, его можно распарсить в дерево классов
        // если это не так, предложить повторить ввод выражения.
        // Если пользователь введет Отмена, завершить диалог Отменой создания места
        if (EntityTypesCollection.TryParsingExpression(str) == false)
        {
            engine.get_OperatorConsole().PrintTextLine("Неправильное выражение, введите правильное выражение класса для этого Места.", EnumDialogConsoleColor.Предупреждение);
            continue;
        }
        else break;
    }
    // return
    outResult.setValue(str);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Place Category value from User
 * 
 * @param engine
 *            Engine object
 * @param outResult
 *            Result string shell object
 * @return Function returns EnumProcedureResult.Success if success, or other values.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult readPlaceCategory(
        Engine engine,
        InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpPlaceNsProp(engine);
    // - запросить новое значение
    // показать существующие неймспейсы, пока только для Процедур и Мест.
    String existingNsChain = engine.get_ECM().getNamespacesChainString(true, true, false);
    engine.get_OperatorConsole().PrintTextLine(" - Можно придумать новую категорию для этого Места, или использовать уже существующую:", EnumDialogConsoleColor.Сообщение);
    // сдвинуть на 3 позиции вправо для выравнивания на экране.
    engine.get_OperatorConsole().PrintTextLine("   " + existingNsChain, EnumDialogConsoleColor.Сообщение);
    engine.get_OperatorConsole().PrintEmptyLine();
    // пользователь должен ввести название пространства имен для нового Места
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите название категории для Места:", false, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;
    // else return
    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Place Adres value from User
 * 
 * @param engine
 *            Engine object
 * @param outResult
 *            Result string shell object
 * @return Function returns EnumProcedureResult.Success if success, or other values.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult readPlaceAdres(
        Engine engine,
        InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpPlaceAdresProp(engine);
    // - запросить новое значение
    // - проверить новое значение
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите адрес Места:", true, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;
    // TODO: как убедиться, что указанный путь правильный и указанный объект существует?
    // return
    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-Вводить словоформы Места по отдельности
 * 
 * @param engine
 *            Объект Движка
 * @return Возвращает строку словоформ или пустую строку при ошибке или отмене.
 * @throws Exception
 *             Ошибка.
 */
private static String ВводитьСловоформыПоОтдельности(Engine engine)
            throws Exception
{
    String result = "";// тут собирается строка словоформ, после сбора всех словоформ.
    LinkedList<String> synoList = new LinkedList<String>();// тут собираются введенные словоформы

while (true)
{
    // начинаем опрос
    engine.get_OperatorConsole().SureConsoleCursorStart();
    engine.get_OperatorConsole().PrintTextLine("Введите словоформы по одной согласно падежу:", EnumDialogConsoleColor.Сообщение);
    // запрос падежных форм по очереди
    for (String quest : PlaceProcedures.ВопросыПадежныхФорм)
    {
        engine.get_OperatorConsole().SureConsoleCursorStart();
        String wordform = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, quest, false, true);
        if (Dialogs.этоОтмена(wordform))
            break;// на выход, пользователю надоело вводить словоформы.
        else synoList.add(wordform);
    }
    // собираем строку словоформ и показываем ее пользователю
    result = String.join(", ", synoList);
    engine.get_OperatorConsole().SureConsoleCursorStart();
    engine.get_OperatorConsole().PrintTextLine("Строка словоформ: " + result, EnumDialogConsoleColor.Сообщение);

    // переход на следующий синоним места?
    EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете ввести словоформы еще одного синонима места?");
    if (sdr.isДа())
        continue;// снова собираем словоформы для нового синонима места
    else if (sdr.isНет())
        break;// выходим со строкой словоформ
    else
    {
        result = "";// выходим с флагом отмены процедуры
        break;
    }
}

// выходим с последней собранной строкой, если она была собрана. Или с флагом отмены, если строка не была собрана
// то есть, выходим с строкой, собранной из последнего полного комплекта ответов синонима, если они вообще были.
return result;
    }

    /**
     * NT-принять от пользователя синонимы и словоформы места
     * 
     * @param engine
     *            Объект движка
     * @param outResult
     *            Класс-оболочка для результата
     * @param placeTitle
     *            Название Места
     * @param place
     *            Place Объект Места для исключения из сравнения.
     * @return Function returns EnumProcedureResult.Success if success, or other values.
     * @throws Exception
     *             Ошибка.
     */
    private static EnumProcedureResult readPlaceSynonims(
            Engine engine,
            InOutArgument outResult,
            String placeTitle,
            Place place) throws Exception
{
    String syno = null;

        while (true)
        {
        engine.get_OperatorConsole().SureConsoleCursorStart();
        engine.get_OperatorConsole().PrintTextLine("Нужно ввести все падежные формы названия места для всех синонимов.", EnumDialogConsoleColor.Сообщение);
        EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете ввести сразу все словоформы одной строкой?");
        if (sdr.isДа())
        {
            engine.get_OperatorConsole().PrintTextLine("Нужно ввести падежные формы (И,Р,Д,В,Т,П), разделяя их запятыми.", EnumDialogConsoleColor.Сообщение);
            engine.get_OperatorConsole().PrintTextLine("Пример: место, места, месту, места, местом, месте", EnumDialogConsoleColor.Сообщение);
            syno = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите строку словоформ названия Места:", true, true);
            // результат ввода ?
            if (Dialogs.этоОтмена(syno))
            {
                // возможно, пользователь решил вводить словоформы раздельно.
                continue;
            }
            // else выходим из свитча
        }
        else if (sdr.isНет())
        {
            // вводить словоформы по отдельности - длинная функция
            syno = ВводитьСловоформыПоОтдельности(engine);
            if (OperatorEngine.Utility.StringIsNullOrEmpty(syno))// если пользователь решил отменить процесс
                return EnumProcedureResult.CancelledByUser;
            // else выходим из свитча
        }
        else // введено Отмена
            return EnumProcedureResult.CancelledByUser;

        // Тут словоформы надо проверить и записать в объект места
        if (Place.checkSynonimString(syno) == false)
        {
            engine.get_OperatorConsole().SureConsoleCursorStart();
            engine.get_OperatorConsole().PrintTextLine("Ошибка! Строка словоформ имеет неправильный формат.", EnumDialogConsoleColor.Предупреждение);
            continue;
        }
        // 4 проверить уникальность синонимов названия Места - они должны быть уникальными для всех Мест.
        // если один из синонимов не уникален, то надо предложить пользователю изменить название Места прямо тут же
        // Если пользователь введет Отмена, завершить диалог Отменой создания места
        // пока что это делается при загрузке Места из БД, а надо вот прямо сейчас.
        // Иначе в БД будут добавлены эти неуникальные синонимы, и загрузка Мест из БД будет давать исключения.
        else if (checkPlaceSynonimUnicality(engine, syno, place) == false)
        {
            engine.get_OperatorConsole().SureConsoleCursorStart();
            engine.get_OperatorConsole().PrintTextLine("Ошибка! Некоторые словоформы Места не уникальны!", EnumDialogConsoleColor.Предупреждение);
            continue;
        }

        else break;

    }
    outResult.setValue(syno);// проверим их на уникальность позже
        // return success
        return EnumProcedureResult.Success;
}

/**
 * NT-проверить уникальность синонимов Места
 * 
 * @param engine
 *            Ссылка на движок.
 * @param syno
 *            Строка синонимов места.
 * @param place
 *            Объект Места для исключения из сравнения.
 * @return Функция возвращает True, если все синонимы уникальные, False в противном случае.
 */
private static Boolean checkPlaceSynonimUnicality(
        Engine engine,
        String syno,
        Place place)
{
    Boolean result = true;
    // 1. получить список синонимов
    LinkedList<String> sy = OperatorEngine.Utility.SplitCommaDelimitedString2(syno);
    // 2. для каждого синонима получить список или 1 объект места.
    for (String s : sy)
        {
    Place p = engine.get_ECM().get_PlaceCollection().GetPlace(s);
    // 3. если места нет (= null), или место = place, то перейти к следующему синониму
    if ((p == null) || (p == place))
        continue;
    // иначе установить флаг результата - false и вывести на экран найденное Место.
    else
    {
        result = false;
        String msg = String.format("Неуникальный синоним \"%s\" Места \"%s\"", s, place.getSingleLineProperties());
        engine.get_OperatorConsole().PrintTextLine(msg, EnumDialogConsoleColor.Предупреждение);
    }
    // 4. Если синонимы закончились, то вернуть true.
    // а если флаг установлен, вернуть false.
}

return result;
    }

    /**
     * NT-вывести на консоль справку по свойству Места Категория.
     * 
     * @param engine
     *            Ссылка на объект движка Оператор.
     */
    private static void printHelpPlaceNsProp(Engine engine)
{
    String[] namespaceDescr = new String[] {
                " - Категория группирует Команды и Места для удобства отображения в списке.",
                " - Название категории должно быть очень коротким, не должно содержать пробелы, может разделяться точками.",
                "   Например: \"Service.Procedure\" как Категория.Подкатегория.",
                " - Если вы затрудняетесь выбрать категорию, введите текст \"Default\" (без кавычек)",
                " - Для подробной информации о категориях обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(namespaceDescr, EnumDialogConsoleColor.Сообщение);
    return;

}

/**
 * NT-вывести на консоль справку по свойству Места Название.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpPlaceTitleProp(Engine engine)
{
    String[] titleDescr = new String[] {
                " - Название места идентифицирует Место для пользователя.",
                " - Название места должно быть коротким, понятным, уникальным.",
                " - Название места должно начинаться с заглавной буквы.",
                "   Например, для программы: ФайловыйМенеджер",
                "   Или, для веб-сайта: Яндекс",
                "   Или, для папки: МоиДокументы",
                " - Для подробной информации о названиях Мест обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(titleDescr, EnumDialogConsoleColor.Сообщение);
    return;

}

/**
 * NT-вывести на консоль справку по свойству команды Описание
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpPlaceDescriptionProp(Engine engine)
{
    String[] descr = new String[] {
                " - Описание Места описывает смысл Места для пользователя.",
                " - Описание должно быть коротким, однострочным но понятным.",
                " - Описание  должно начинаться с заглавной буквы и оканчиваться точкой.",
                " - Описание должно позволить пользователю различать схожие по названию Места.",
                " - Для подробной информации о описаниях Мест обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(descr, EnumDialogConsoleColor.Сообщение);

    return;
}

/**
 * NT-вывести на консоль справку по свойству Места Класс.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpPlaceClassProp(Engine engine)
{
    String[] descr = new String[] {
                " - Класс Места описывает тип Места для использования в Оператор.",
                " - Если не знаете, что вводить, введите (без кавычек):",
                "   - для приложения - \"Программа\"",
                "   - для файла - \"Файл\"",
                "   - для папки - \"Каталог\"",
                "   - для веб-сайта - \"Сайт\"",
                " - Для подробной информации о классах Мест обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(descr, EnumDialogConsoleColor.Сообщение);

    return;
}

/**
 * NT-вывести на консоль справку по свойству команды Адрес.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpPlaceAdresProp(Engine engine)
{
    String[] adresDescr = new String[] {
                " - Описывает путь к каталогу или файлу или веб-адрес сайта",
                "   Например, для каталога: file:/home/username/Documents",
                "   Например, для приложения: file:/home/username/firefox/firefox",
                "   Например, для сайта: https://yandex.ru",
                " - Для подробной информации о адресе Места обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(adresDescr, EnumDialogConsoleColor.Сообщение);

    return;
}

    // *** End of file ***
}
