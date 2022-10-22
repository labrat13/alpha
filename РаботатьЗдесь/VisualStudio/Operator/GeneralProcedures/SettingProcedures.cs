using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralProcedures
{
    internal class SettingProcedures
    {
    }

    /**
 * Класс тестовых Процедур для Оператор.
 * 
 * @author Селяков Павел
 *
 */
    @OperatorProcedure(State = ImplementationState.Ready,
            Title = "SettingProcedures",
            Description = "Класс содержит Процедуры для операций с Настройками Оператора.")
public class SettingProcedures
    {
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
         * NT-Обработчик процедуры Создать настройку НазваниеНастройки.
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
        @OperatorProcedure(State = ImplementationState.Ready,   //заменить на актуальное
                Title = "Создать настройку НазваниеНастройки",
                Description = "Создать Настройку в БазаДанныхОператора.")
    public static EnumProcedureResult CommandCreateSetting(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {

            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.SettingProcedures.CommandCreateSetting";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                // 1. Извлечь из аргумента название Настройки
                String settingTitle;
                FuncArgument arg = args.getByIndex(0);
                settingTitle = arg.get_ArgumentQueryValue().Trim();// берем сырой текст аргумента из запроса

                String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, settingTitle);
                // DONE: вывести это тестовое сообщение о начале процедуры - в лог!
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);

                SettingItem proc = new SettingItem();// новый пустой объект для заполнения

                // 1 извлечь из аргументов название Настройки, если оно есть
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("1. Название Настройки", EnumDialogConsoleColor.Сообщение);

                // извлечь название Настройки из аргумента
                engine.get_OperatorConsole().PrintTextLine(String.format("Название новой Настройки: \"%s\"", settingTitle), EnumDialogConsoleColor.Сообщение);

                //проверить признак того, что вместо названия Настройки движком было подставлено название зарегистрированного места
                if (arg.get_АвтоподстановкаМеста() == true)
                {
                    ; // TODO: обработать тут случай автоподстановки места вместо названия команды
                      // если он возникнет
                }

                // 2 проверить что в БД нет Настройки с таким названием, без учета регистра символов
                InOutArgument outResult;  // temp var reference
                EnumProcedureResult epr; // temp var reference

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readSettingTitleForNew(engine, outResult, settingTitle);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set regex value
                    str = outResult.getValueString();
                    proc.set_Title(str);
                }

                // 2 Пользователь должен ввести описание Настройки
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("2. Описание Настройки", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readSettingDescription(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set regex value
                    str = outResult.getValueString();
                    proc.set_Description(str);
                }

                // 4. Пользователь должен ввести путь к Настройки
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("3. Значение Настройки", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readSettingAdres(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set namespace value
                    str = outResult.getValueString();
                    proc.set_Path(str);
                }

                // 6. Пользователь должен ввести неймспейс для Настройки.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("4. Категория для Настройки", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readSettingCategory(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set namespace value
                    str = outResult.getValueString();
                    proc.set_Namespace(str.Trim());
                }

                // 7. Вывести свойства Настройки и запросить подтверждение создания Настройки.
                // сначала добавить название хранилища в объект Настройки - согласно концепту Библиотек Процедур Оператора.
                proc.set_Storage(Item.StorageKeyForDatabaseItem);
                // Вроде все свойства должны быть заполнены, теперь надо вывести все их в форме
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("5. Подтвердите создание Настройки", EnumDialogConsoleColor.Сообщение);// пункт плана
                engine.get_OperatorConsole().PrintSettingForm(proc);
                // и запросить подтверждение пользователя, что он желает создать Настройку
                // Если пользователь ответит Да, надо создать Настройку.
                // Если пользователь ответит Нет или Отмена, отменить операцию.
                EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете создать новую Настройку?");
                if (sdr.isНет() || sdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;

                // 8. заполнить объект Настройки и создать новую Настройку в БД
                engine.get_ECM().AddSetting(proc);

                // 9. вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine(String.format("Настройка \"%s\" создана успешно", proc.get_Title()), EnumDialogConsoleColor.Успех);
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
         * NT-Обработчик процедуры Показать настройки.
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
        @OperatorProcedure(State = ImplementationState.Ready,   //заменить на актуальное
                Title = "Показать настройки",
                Description = "Вывести на экран список Настроек Оператора")
    public static EnumProcedureResult CommandListSettings(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {

            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.SettingProcedures.CommandListSettings";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                // вывести это тестовое сообщение о начале процедуры на консоль и в лог
                String str = String.format("Начата процедура %s()", currentProcedureTitle);
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("Список всех Настроек Оператора:", EnumDialogConsoleColor.Сообщение);
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintListOfSettings();
                engine.get_OperatorConsole().PrintEmptyLine();
                // вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine("Выведен список Настроек", EnumDialogConsoleColor.Успех);
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
         * NT-Обработчик процедуры Удалить Настройку НазваниеНастройки.
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
        @OperatorProcedure(State = ImplementationState.Ready,   //заменить на актуальное
                Title = "Удалить настройку НазваниеНастройки",
                Description = "Удалить указанную Настройку из БазаДанныхОператора")
    public static EnumProcedureResult CommandDeleteSetting(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {

            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.SettingProcedures.CommandDeleteSetting";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                // 1. Извлечь из аргумента название Настройки
                String settingTitle;
                FuncArgument arg = args.getByIndex(0);
                settingTitle = arg.get_ArgumentQueryValue().Trim();// берем сырой текст аргумента из запроса

                // DONE: вывести это тестовое сообщение о начале процедуры - в лог!
                String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, settingTitle);
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);

                // 1. Извлечь из аргумента название Настройки
                engine.get_OperatorConsole().PrintTextLine(String.format("Название удаляемой Настройки: \"%s\"", settingTitle), EnumDialogConsoleColor.Сообщение);

                // проверить признак того, что вместо названия Настройки движком было подставлено название зарегистрированного места
                if (arg.get_АвтоподстановкаМеста() == true)
                {
                    ; // TODO: обработать тут случай автоподстановки места вместо названия Настройки
                      // если он возникнет
                }

                // 2. извлечь из БД список Настроек с таким названием и показать пользователю.
                // без учета регистра символов
                // DONE: весь этот пункт 2 надо превратить в отдельную функцию выбора Настройки по ее названию, чтобы использовать ее во многих местах этого класса.

                SettingItem sett = null;

                InOutArgument outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                EnumProcedureResult epr = selectSetting(engine, settingTitle, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                // если Настройка не найдена, selectSetting возвращает Error, а эта процедура должна вернуть Success,
                // так как не ее проблема, что нечего удалять.
                else if (epr == EnumProcedureResult.Error)
                    return EnumProcedureResult.Success;
                else
                {
                    // set SettingItem object
                    sett = outResult.getValueSettingItem();
                }
                // Тут в объекте sett уже должна быть выбранная Настройка.

                // 3. выбранную пользователем Настройку - проверить, что она может быть удалена
                // - она может быть удалена, если она находится в БД. Сейчас БиблиотекиПроцедур не позволяют удалять или редактировать свои Процедуры и Места.
                // - надо добавить в объект Процедуры и Места (Item) флаг ReadOnly, для БД он сброшен, для Библиотек Процедур он установлен.
                // - Флаг не должен храниться в БД, только в памяти.
                // - TODO: решено позже добавить в объект Item ссылку на объект Хранилища, унифицированный для БД и БиблиотекаПроцедур,
                // и из него запрашивать возможность удаления и изменения итемов.
                if (sett.isItemCanRemoved() == false)
                {
                    // Если указанная Настройка не может быть удалена, вывести сообщение об этом и завершить текущую Процедуру успешно.
                    String st2 = String.format("Выбранная настройка \"%s\" не может быть удалена из ее хранилища \"%s\"", settingTitle, sett.get_Storage());
                    engine.get_OperatorConsole().PrintTextLine(st2, EnumDialogConsoleColor.Предупреждение);
                    return EnumProcedureResult.Success;
                }
                // else

                // 4. Показать пользователю свойства выбранной Настройки и запросить подтверждение удаления.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("Подтвердите удаление Настройки", EnumDialogConsoleColor.Сообщение);
                engine.get_OperatorConsole().PrintSettingForm(sett);
                // и запросить подтверждение пользователя, что он желает удалить Настройку.
                // Если пользователь ответит Да, надо удалить Настройку.
                // Если пользователь ответит Нет или Отмена, отменить операцию.
                EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете удалить Настройку?");
                if (sdr.isНет() || sdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                // 5. Удалить команду.
                engine.get_ECM().RemoveSetting(sett);

                // 6. вывести сообщение о результате операции: успешно
                String msg6 = String.format("Настройка \"%s\" успешно удалена.", settingTitle);
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
         * NT-Обработчик процедуры Изменить настройку НазваниеНастройки.
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
                Title = "Изменить настройку НазваниеНастройки",
                Description = "Изменить указанную Настройку Оператора")
    public static EnumProcedureResult CommandChangeSetting(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {

            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.SettingProcedures.CommandChangeSetting";

            String stmp; // temp string
            EnumSpeakDialogResult esdr; // temp dialog result
            InOutArgument outResult; // temp in-out argument object
            EnumProcedureResult epr; // temp procedure result
                                     // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
                                     // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {

                // 1. Извлечь из аргумента название Настройки
                String settingTitle;
                FuncArgument arg = args.getByIndex(0);
                settingTitle = arg.get_ArgumentQueryValue().Trim();// берем сырой текст аргумента из запроса
                                                                   // вывести в лог тестовое сообщение о начале процедуры
                String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, settingTitle);
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);

                engine.get_OperatorConsole().PrintTextLine(String.format("Название изменяемой Настройки: \"%s\"", settingTitle), EnumDialogConsoleColor.Сообщение);

                //проверить признак того, что вместо названия Настройки движком было подставлено название зарегистрированного места
                if (arg.get_АвтоподстановкаМеста() == true)
                {
                    ; // TODO: обработать тут случай автоподстановки места вместо названия команды
                      // если он возникнет
                }

                // 2. извлечь из БД список Настроек с таким названием и показать пользователю.
                // без учета регистра символов
                SettingItem sett = null;

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = selectSetting(engine, settingTitle, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                // если процедура не найдена, selectProcedure возвращает Error, а эта процедура должна вернуть Success,
                // так как не ее проблема, что нечего изменять.
                else if (epr == EnumProcedureResult.Error)
                    return EnumProcedureResult.Success;
                else
                {
                    // set SettingItem object
                    sett = outResult.getValueSettingItem();
                }

                // Тут в объекте proc уже должна быть выбранная Настройка.

                // 3. выбранную пользователем Настройку - проверить, что она может быть изменена.
                // - она может быть изменена, если она находится в БД. Сейчас БиблиотекиПроцедур не позволяют удалять или редактировать свои Настройки, Процедуры и Места.
                // - надо добавить в объект Настройки, Процедуры и Места (Item) флаг ReadOnly, для БД он сброшен, для Библиотек Процедур он установлен.
                // Флаг не хранится в БД, только в памяти.
                // - TODO: решено позже добавить в объект Item ссылку на объект Хранилища, унифицированный для БД и БиблиотекаПроцедур,
                // и из него запрашивать возможность удаления и изменения итемов.
                if (sett.isItemCanChanged() == false)
                {
                    // Если указанная Настройка не может быть удалена, вывести сообщение об этом и завершить текущую Процедуру успешно.
                    stmp = String.format("Выбранная Настройка \"%s\" не может быть изменена.", settingTitle);
                    engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Предупреждение);
                    return EnumProcedureResult.Success;
                }
                // else
                //создадим копию, чтобы не перезаписывать оригинал из коллекции Настроек.
                SettingItem settNew = new SettingItem(sett);
                // 4. По каждому свойству Настройки:
                // - показывать текущее значение свойства и спрашивать, желает ли пользователь его изменить. (Да-Нет-Отмена-Пропустить)
                // Если не желает, перейти к следующему свойству.
                // Если желает, вывести справку, запросить новое значение, проверить его формат и вписать в объект Настройки.
                // Если отменил, то отменить операцию.

                // 4.1 Название Настройки
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("1. Название Настройки", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущее название: \"%s\"", sett.get_Title());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить название Настройки?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readSettingTitleForExisting(engine, outResult, sett);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set regex value
                        stmp = outResult.getValueString();
                        settNew.set_Title(stmp);
                    }
                }
                // else Нет - перейти к следующему свойству Настройки.

                // 4.2 Описание Настройки
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("2. Описание Настройки", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущее описание: \"%s\"", sett.get_Description());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить описание Настройки?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readSettingDescription(engine, outResult);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set regex value
                        stmp = outResult.getValueString();
                        settNew.set_Description(stmp);
                    }
                }
                // else Нет - перейти к следующему свойству Команды.

                // 4.4 Адрес Настройки
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("3. Значение Настройки", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущий адрес: \"%s\"", sett.get_Path());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить значение Настройки?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readSettingAdres(engine, outResult);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set namespace value
                        stmp = outResult.getValueString();
                        settNew.set_Path(stmp);
                    }
                }
                // else Нет - перейти к следующему свойству Команды.

                // 4.6 Категория Настройки
                // - вывести заголовок
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("4. Категория для Настройки", EnumDialogConsoleColor.Сообщение);
                // - вывести текущее значение
                stmp = String.format("Текущая категория: \"%s\"", sett.get_Namespace());
                engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
                // - запросить изменение
                // Если пользователь ответит Да, надо запросить новое значение.
                // Если пользователь ответит Нет, надо пропустить это поле.
                // Если пользователь ответит Отмена, прервать операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить категорию Настройки?");
                if (esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                else if (esdr.isДа())
                {
                    outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                    epr = readSettingCategory(engine, outResult);
                    if (epr == EnumProcedureResult.CancelledByUser)
                        return epr;
                    else
                    {
                        // set namespace value
                        stmp = outResult.getValueString();
                        settNew.set_Namespace(stmp.Trim());
                    }
                }
                // else Нет - перейти к следующему свойству Настройки.

                // 5. Вывести общий набор свойств Настройки и запросить пользователя, желает ли он сохранить эти данные.(Да-Нет-Отмена)
                // - если пользователь ответил да, изменить Настройку через ECM функцию.
                // - при любом другом ответе пользователя отменить операцию.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("5. Подтвердите изменение Настройки", EnumDialogConsoleColor.Сообщение);
                engine.get_OperatorConsole().PrintSettingForm(settNew);
                // и запросить подтверждение пользователя, что он желает удалить Команду.
                // Если пользователь ответит Да, надо удалить Команду.
                // Если пользователь ответит Нет или Отмена, отменить операцию.
                esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить Настройку?");
                if (esdr.isНет() || esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                // записать команду
                engine.get_ECM().UpdateSetting(settNew);

                // 6. вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine(String.format("Настройка %s успешно изменена", settingTitle), EnumDialogConsoleColor.Успех);
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
         * NT-Обработчик процедуры Удалить все настройки.
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
        @OperatorProcedure(State = ImplementationState.Ready,   //заменить на актуальное
                Title = "Удалить все настройки",
                Description = "Удалить все Настройки из БазаДанныхОператора")
    public static EnumProcedureResult CommandDeleteAllSettings(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {

            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.SettingProcedures.CommandDeleteAllSetting";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                // 1. Запросить от пользователя подтверждение о удалении всех Настроек из БД. Да - Нет и Отмена.
                // если пользователь ответил Да - удалить все Настройки из БД
                // если пользователь ответил Нет или Отмена - прервать операцию.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("1. Подтвердите удаление всех Настроек из БазаДанныхОператора", EnumDialogConsoleColor.Сообщение);
                EnumSpeakDialogResult esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете удалить все настройки?");
                if (esdr.isНет() || esdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;
                // выполнить удаление всех Настроек из БД Оператора
                engine.get_ECM().RemoveAllSettingsFromDatabase();
                // вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine("Удаление всех Настроек завершено успешно", EnumDialogConsoleColor.Успех);
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
         * NT-Select Procedure by Title.
         * 
         * @param engine
         *            Engine object.
         * @param title
         *            Setting title.
         * @param outResult
         *            Result SettingItem in shell object.
         * @return Function returns EnumProcedureResult.Success if success, Error if procedure not found, CancelledByUser if cancelled.
         * @throws Exception
         *             Error occured.
         */
        private static EnumProcedureResult selectSetting(
                Engine engine,
                String title,
                InOutArgument outResult) throws Exception
        {
            SettingItem p = null;
            LinkedList<SettingItem> lip = engine.get_ECM().get_SettingCollection().getByTitle(title);
        int lipSize = lip.size();
        if (lipSize == 0)
        {
            // тут вывести сообщение, что указанная Настройка не найдена и завершить процедуру успешно.
            String stmp = String.format("Указанная Настройка \"%s\" не найдена.", title);
        engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Предупреждение);
            return EnumProcedureResult.Error;
        }
        else if (lipSize == 1)
        {
            p = lip.get(0);
        }
        else // if(lipSize > 1)
{
    // тут вывести пользователю найденные Настройки с тем же названием
    engine.get_OperatorConsole().PrintTextLine("Существующие Настройки с таким названием:", EnumDialogConsoleColor.Предупреждение);
    // Если в списке более одной Настройки, то пользователь должен выбрать одну из них для удаления.
    // для этого в списке Команд нужно показать уникальный ИД Настройки, источник-хранилище, категорию, название и описание.
    // И флаг, возможно ли удаление данной Настройки - это определяется Хранилищем.
    // Для выбора из нескольких Настроек надо запросить у пользователя ИД Настройки, но проблема в том, что ид сейчас есть только у объектов из БД.
    // - можно вывести порядковый номер в этом списке и запросить у пользователя его.
    engine.get_OperatorConsole().PrintSettingFormNumberedList(lip);
    // запросить у пользователя порядковый номер элемента списка
    int lip_index = engine.get_OperatorConsole().InputListIndex(lipSize);
    // обработать указанный пользователем индекс
    if (lip_index < 1)
    {
        // Если пользователь отказался выбрать элемент списка, вывести сообщение об этом и завершить текущую Процедуру Отменено пользователем.
        engine.get_OperatorConsole().PrintTextLine("Операция отменена, поскольку пользователь не смог выбрать Настройку из списка.", EnumDialogConsoleColor.Сообщение);
        return EnumProcedureResult.CancelledByUser;
    }
    else p = lip.get(lip_index);// выбранный пользователем объект команды.
}

// return
outResult.setValue(p);
return EnumProcedureResult.Success;
    }

    /**
     * NT-Read Setting Title value from User, for newly create Setting.
     * 
     * @param engine
     *            Engine object.
     * @param outResult
     *            Result string shell object.
     * @param oldTitle
     *            Current setting title string.
     * @return Function returns EnumProcedureResult.Success if success; EnumProcedureResult.CancelledByUser if cancelled.
     * @throws Exception
     *             Error occured.
     */
    private static EnumProcedureResult readSettingTitleForNew(
            Engine engine,
            InOutArgument outResult,
            String oldTitle) throws Exception
{
    // вывести справку по созданию названия для Настройки.
    printHelpSettingTitleProp(engine);
    // temp string with old title
    String str = new String(oldTitle);

// Поскольку имена настроек не уникальные, тут просто надо вывести имеющиеся настройки с этим именем, их описание и значения.
// затем предложить использовать это название настройки (ДаНетОтмена)
// Если пользователь выберет Да, то выйти успешно с названием настройки.
// Если пользователь выберет Отмена, то выйти с отменой.
// Если пользователь выберет Нет, то запросить новое название Настройки в диалоге.
// и перейти снова в начало цикла, чтобы вывести имеющиеся Настройки с этим именем и продолжить цикл.
Boolean notUnicalSetting = false;
while (true)
{
    // если название Настройки - пустая строка, вывести сообщение и перейти к приему нового названия Настройки
    if (OperatorEngine.Utility.StringIsNullOrEmpty(str))
        engine.get_OperatorConsole().PrintTextLine("Пустая строка недопустима для названия Настройки!", EnumDialogConsoleColor.Предупреждение);
    else
    {
        // вывести все Настройки с таким названием, без учета регистра символов
        LinkedList<SettingItem> lip = engine.get_ECM().get_SettingCollection().getByTitle(str);
        notUnicalSetting = (lip.size() > 0);// временный флаг для упрощения проверок позже
        if (notUnicalSetting)
        {
            // тут вывести пользователю найденные Настройки с тем же названием
            engine.get_OperatorConsole().PrintTextLine("Настройки с таким названием уже существуют:", EnumDialogConsoleColor.Предупреждение);
            for (SettingItem pp : lip)
            {
                engine.get_OperatorConsole().PrintSettingShortLine(pp);
            }
            lip.clear();// очистить временный список, поскольку он в цикле
        }
    }
    if (!OperatorEngine.Utility.StringIsNullOrEmpty(str))
    {
        EnumSpeakDialogResult esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Использовать это название для создаваемой Настройки?");
        if (esdr.isДа())
        {
            //выйти успешно с названием настройки.
            break;
        }
        else if (esdr.isОтмена())
        {
            //выйти с отменой.
            return EnumProcedureResult.CancelledByUser;
        }
        //else (isНет)
    }
    //тут мы окажемся, если была введена пустая строка или если пользователь выбрал Нет
    //запросить новое название Настройки в диалоге.
    str = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите новое название Настройки:", false, true);
    if (Dialogs.этоОтмена(str))
        return EnumProcedureResult.CancelledByUser;
    //и перейти снова в начало цикла, чтобы вывести имеющиеся Настройки с этим именем и продолжить цикл.
}// while loop
 // Тут мы окажемся, если название Настройки введено.
 // return
outResult.setValue(str);
return EnumProcedureResult.Success;
    }
    /**
     * NT-Read Setting Title value from User, for change or rename Setting.
     * 
     * @param engine
     *            Engine object.
     * @param outResult
     *            Result title string shell object.
     * @param sett
     *            Change Setting object.
     * @return Function returns EnumProcedureResult.Success if success; EnumProcedureResult.CancelledByUser if cancelled.
     * @throws Exception
     *             Error occured.
     */
    private static EnumProcedureResult readSettingTitleForExisting(
            Engine engine,
            InOutArgument outResult,
            SettingItem sett) throws Exception
{
    // вывести справку по созданию названия для Настройки.
    printHelpSettingTitleProp(engine);
    // temp string with old title
    String str = new String(sett.get_Title());

// Поскольку имена настроек не уникальные, тут просто надо вывести имеющиеся настройки с этим именем, их описание и значения.
// затем предложить использовать это название настройки (ДаНетОтмена)
// Если пользователь выберет Да, то выйти успешно с названием настройки.
// Если пользователь выберет Отмена, то выйти с отменой.
// Если пользователь выберет Нет, то запросить новое название Настройки в диалоге.
// и перейти снова в начало цикла, чтобы вывести имеющиеся Настройки с этим именем и продолжить цикл.
Boolean notUnicalSetting = false;
while (true)
{
    // если название Настройки - пустая строка, вывести сообщение и перейти к приему нового названия Настройки
    if (OperatorEngine.Utility.StringIsNullOrEmpty(str))
        engine.get_OperatorConsole().PrintTextLine("Пустая строка недопустима для названия Настройки!", EnumDialogConsoleColor.Предупреждение);
    else
    {
        // вывести все Настройки с таким названием, без учета регистра символов
        //кроме текущей редактируемой
        LinkedList<SettingItem> lip = engine.get_ECM().get_SettingCollection().getByTitle(str);
        lip.remove(sett);
        notUnicalSetting = (lip.size() > 0);// временный флаг для упрощения проверок позже
        if (notUnicalSetting)
        {
            // тут вывести пользователю найденные Настройки с тем же названием
            engine.get_OperatorConsole().PrintTextLine("Настройки с таким названием уже существуют:", EnumDialogConsoleColor.Предупреждение);
            for (SettingItem pp : lip)
            {
                engine.get_OperatorConsole().PrintSettingShortLine(pp);
            }
            lip.clear();// очистить временный список, поскольку он в цикле
        }
    }
    if (!OperatorEngine.Utility.StringIsNullOrEmpty(str))
    {
        EnumSpeakDialogResult esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Использовать это название для изменяемой Настройки?");
        if (esdr.isДа())
        {
            //выйти успешно с названием настройки.
            break;
        }
        else if (esdr.isОтмена())
        {
            //выйти с отменой.
            return EnumProcedureResult.CancelledByUser;
        }
        //else (isНет)
    }
    //тут мы окажемся, если была введена пустая строка или если пользователь выбрал Нет
    //запросить новое название Настройки в диалоге.
    str = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите новое название Настройки:", false, true);
    if (Dialogs.этоОтмена(str))
        return EnumProcedureResult.CancelledByUser;
    //и перейти снова в начало цикла, чтобы вывести имеющиеся Настройки с этим именем и продолжить цикл.
}// while loop
 // Тут мы окажемся, если название Настройки введено.
 // return
outResult.setValue(str);
return EnumProcedureResult.Success;
    }
    
    /**
     * NT-Read Setting Category value from User
     * 
     * @param engine
     *            Engine object
     * @param outResult
     *            Result string shell object
     * @return Function returns EnumProcedureResult.Success if success, or other values.
     * @throws Exception
     *             Error occured.
     */
    private static EnumProcedureResult readSettingCategory(
            Engine engine,
            InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpSettingNsProp(engine);
    // - запросить новое значение
    // показать существующие неймспейсы, пока только для Настроек.
    String existingNsChain = engine.get_ECM().getNamespacesChainString(false, false, true);
    engine.get_OperatorConsole().PrintTextLine(" - Можно придумать новую категорию для этой Настройки, или использовать уже существующую:", EnumDialogConsoleColor.Сообщение);
    // сдвинуть на 3 позиции вправо для выравнивания на экране.
    engine.get_OperatorConsole().PrintTextLine("   " + existingNsChain, EnumDialogConsoleColor.Сообщение);
    engine.get_OperatorConsole().PrintEmptyLine();
    // пользователь должен ввести название пространства имен для новой Процедуры
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите название категории для Настройки:", false, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;
    // else return
    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Setting Value value from User
 * 
 * @param engine
 *            Engine object
 * @param outResult
 *            Result string shell object
 * @return Function returns EnumProcedureResult.Success if success, or other values.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult readSettingAdres(
        Engine engine,
        InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpSettingAdresProp(engine);
    // - запросить новое значение
    // - проверить новое значение
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите значение для Настройки:", true, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;

    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Setting Description value from User
 * 
 * @param engine
 *            Engine object
 * @param outResult
 *            Result string shell object
 * @return Function returns EnumProcedureResult.Success if success, or other values.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult readSettingDescription(
        Engine engine,
        InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpSettingDescriptionProp(engine);
    // - запросить новое значение
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите краткое описание Настройки:", true, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;

    // return
    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-вывести на консоль справку по свойству Настройки Категория.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpSettingNsProp(Engine engine)
{
    String[] namespaceDescr = new String[] {
                " - Категория группирует Настройки для удобства отображения в списке.",
                " - Название категории должно быть очень коротким, не должно содержать пробелы, может разделяться точками.",
                "   Например: \"Service.Settings\" как Категория.Подкатегория.",
                " - Если вы затрудняетесь выбрать категорию, введите текст \"Default\" (без кавычек)",
                " - Для подробной информации о категориях обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(namespaceDescr, EnumDialogConsoleColor.Сообщение);
    return;

}

/**
 * NT-вывести на консоль справку по свойству Настройки Значение.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpSettingAdresProp(Engine engine)
{
    String[] adresDescr = new String[] {
                " - Значение Настроек используется в некоторых Процедурах Команд Оператор.",
                " - Для подробной информации о Настройках обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(adresDescr, EnumDialogConsoleColor.Сообщение);
    return;

}

/**
 * NT-вывести на консоль справку по свойству Настройки Описание.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpSettingDescriptionProp(Engine engine)
{
    String[] descr = new String[] {
                " - Описание Настройки описывает смысл и применение Настройки для пользователя.",
                " - Описание должно быть коротким, однострочным но понятным.",
                " - Описание  должно начинаться с заглавной буквы и оканчиваться точкой.",
                " - Описание должно позволить пользователю различать схожие по названию Настройки.",
                " - Для подробной информации о описаниях Настроек обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(descr, EnumDialogConsoleColor.Сообщение);

    return;
}

/**
 * NT-вывести на консоль справку по свойству Настройки Название.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpSettingTitleProp(Engine engine)
{
    String[] titleDescr = new String[] {
                " - Название Настройки идентифицирует Настройку для пользователя и для кода Оператор.",
                " - Название Настройки должно быть коротким, понятным, уникальным.",
                " - Под одним названием может храниться несколько Настроек.",
                "   В этом случае они либо обрабатываются все, как список,",
                "    либо обрабатывается только первая попавшаяся из них.",
                "   Это зависит от кода, работающего с данной Настройкой.",
                " - Для подробной информации о названиях Настроек обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(titleDescr, EnumDialogConsoleColor.Сообщение);
    return;
}

    
    // *** End of file ***
}
