using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralProcedures
{
    internal class ProcedureProcedures
    {
    }

    // DONE: Класс методов для Процедур должен быть помечен аннотацией OperatorProcedure с ImplementationState = NotTested либо Ready, чтобы его методы можно было
    // вызывать в качестве процедур.
    // Класс может также содержать любые элементы, необходимые для методов Процедур.

    /**
     * NT-Класс операций с Процедурами в БазаДанныхОператора.
     * 
     * @author Селяков Павел
     *
     */

    @OperatorProcedure(State = ImplementationState.Ready,
            Title = "Класс операций Процедур.",
            Description = "Класс операций с Процедурами в БазаДанныхОператора.")
public class ProcedureProcedures
    {

        // Для всех операций с Процедурами и Местами из кода Процедур использовать класс ElementCacheManager, а не БД итп.
        // engine.get_ECM().AddPlace(p);

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




        /**
         * RT- Обработчик процедуры Создать команду НазваниеКоманды.
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
         * @throws Exception
         *             Ошибка при исполнении Процедуры.
         */
        @OperatorProcedure(State = ImplementationState.Ready,
                Title = "Создать Команду",
                Description = "Создать Процедуру в БазаДанныхОператора.")
    public static EnumProcedureResult CommandCreateProcedure(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args) throws Exception
        {

            // содержимое списка аргументов
            // args[0].name = "команда" - название аргумента в строке регекса команды
            // args[0].value = "Скачать файл ХХХ" - значение аргумента - название создаваемого места
            // args[0].type = "" - тип аргумента - сейчас не указывается, так как мне лень думать

            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.
            String currentProcedureTitle = "GeneralProcedures.ProcedureProcedures.CommandCreateProcedure";
        // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
        // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
        try
        {
                // 1. Извлечь из аргумента название команды
                String procedureTitle;
                FuncArgument arg = args.getByIndex(0);
                procedureTitle = arg.get_ArgumentQueryValue().trim();// берем сырой текст аргумента из запроса


                String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, procedureTitle);
                // DONE: вывести это тестовое сообщение о начале процедуры - в лог!
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);

                Procedure proc = new Procedure();// новый пустой объект для заполнения

                // 1 извлечь из аргументов название Процедуры, если оно есть
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("1. Название Команды", EnumDialogConsoleColor.Сообщение);

                // извлечь название процедуры из аргумента
                engine.get_OperatorConsole().PrintTextLine(String.format("Название новой Команды: \"%s\"", procedureTitle), EnumDialogConsoleColor.Сообщение);

                // TODO: проверить признак того, что вместо названия процедуры движком было подставлено название зарегистрированного места
                if (arg.get_АвтоподстановкаМеста() == true)
                {
                    ; // TODO: обработать тут случай автоподстановки места вместо названия команды
                      // если он возникнет
                }

                // 2 проверить что в БД нет Процедуры с таким названием, без учета регистра символов
                InOutArgument outResult;  // temp var reference
                EnumProcedureResult epr; // temp var reference

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readProcedureTitleForNew(engine, outResult, procedureTitle);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set regex value
                    str = outResult.getValueString();
                    proc.set_Title(str);
                }

                // 2 Пользователь должен ввести описание процедуры
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("2. Описание Команды", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readProcedureDescription(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set regex value
                    str = outResult.getValueString();
                    proc.set_Description(str);
                }

                // 3. Пользователь должен ввести регекс процедуры
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("3. Регекс Команды", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readProcedureRegex(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set regex value
                    str = outResult.getValueString();
                    proc.set_Regex(str);
                }

                // 4. Пользователь должен ввести путь к процедуре
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("4. Адрес Процедуры Команды", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readProcedureAdres(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set namespace value
                    str = outResult.getValueString();
                    proc.set_Path(str);
                }

                // 5. Пользователь должен ввести Вес процедуры
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("5. Вес Команды", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readProcedureVes(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set ves value
                    Double ves = outResult.getValueDouble();
                    proc.set_Ves(ves);
                }

                // 6. Пользователь должен ввести неймспейс для Процедуры.
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("6. Категория для Команды", EnumDialogConsoleColor.Сообщение);

                outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
                epr = readProcedureCategory(engine, outResult);
                if (epr == EnumProcedureResult.CancelledByUser)
                    return epr;
                else
                {
                    // set namespace value
                    str = outResult.getValueString();
                    proc.set_Namespace(str.trim());
                }

                // 7. Вывести свойства Процедуры и запросить подтверждение создания процедуры.
                // сначала добавить название хранилища в объект Процедуры - согласно концепту Библиотек Процедур Оператора.
                proc.set_Storage(Procedure.StorageKeyForDatabaseItem);
                // Вроде все свойства должны быть заполнены, теперь надо вывести все их в форме
                engine.get_OperatorConsole().PrintEmptyLine();
                engine.get_OperatorConsole().PrintTextLine("7. Подтвердите создание Команды", EnumDialogConsoleColor.Сообщение);// пункт плана
                engine.get_OperatorConsole().PrintProcedureForm(proc);
                // и запросить подтверждение пользователя, что он желает создать Место
                // Если пользователь ответит Да, надо создать место.
                // Если пользователь ответит Нет или Отмена, отменить операцию.
                EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете создать новую Команду?");
                if (sdr.isНет() || sdr.isОтмена())
                    return EnumProcedureResult.CancelledByUser;

                // 8. заполнить объект Процедуры и создать новую процедуру в БД
                engine.get_ECM().AddProcedure(proc);

                // 9. вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine(String.format("Команда \"%s\" создана успешно", proc.get_Title()), EnumDialogConsoleColor.Успех);
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
 * RT- Обработчик процедуры Показать команды
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
        Title = "Показать список Команд.",
        Description = "Вывести на экран список доступных Команд Оператора.")
    public static EnumProcedureResult CommandListProcedures(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
{
    EnumProcedureResult result = EnumProcedureResult.Success;
    // название текущей процедуры для лога итп.
    String currentProcedureTitle = "GeneralProcedures.ProcedureProcedures.CommandListProcedures";
    // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
    // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
    try
    {
        // вывести это тестовое сообщение о начале процедуры на консоль и в лог
        String str = String.format("Начата процедура %s()", currentProcedureTitle);
        engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("Список всех Команд Оператора:", EnumDialogConsoleColor.Сообщение);
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintListOfProcedures();
        engine.get_OperatorConsole().PrintEmptyLine();
        // вывести сообщение о результате операции: успешно
        engine.get_OperatorConsole().PrintTextLine("Выведен список Команд", EnumDialogConsoleColor.Успех);
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
 * RT-Обработчик процедуры Удалить команду НазваниеКоманды
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
        Title = "Удалить команду НазваниеКоманды",
        Description = "Удалить указанную команду из БазаДанныхОператор.")
    public static EnumProcedureResult CommandDeleteProcedure(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
{
    EnumProcedureResult result = EnumProcedureResult.Success;
    // название текущей процедуры для лога итп.
    // DONE: указать здесь полный путь как название процедуры для вывода на экран.
    String currentProcedureTitle = "GeneralProcedures.ProcedureProcedures.CommandDeleteProcedure";
    // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
    // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
    try
    {
        // 1. Извлечь из аргумента название команды
        String procedureTitle;
        FuncArgument arg = args.getByIndex(0);
        procedureTitle = arg.get_ArgumentQueryValue().trim();// берем сырой текст аргумента из запроса

        // DONE: вывести это тестовое сообщение о начале процедуры - в лог!
        String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, procedureTitle);
        engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);

        // 1. Извлечь из аргумента название команды
        engine.get_OperatorConsole().PrintTextLine(String.format("Название удаляемой Команды: \"%s\"", procedureTitle), EnumDialogConsoleColor.Сообщение);

        // TODO: проверить признак того, что вместо названия команды движком было подставлено название зарегистрированного места
        if (arg.get_АвтоподстановкаМеста() == true)
        {
            ; // TODO: обработать тут случай автоподстановки места вместо названия команды
              // если он возникнет
        }

        // 2. извлечь из БД список команд с таким названием и показать пользователю.
        // без учета регистра символов
        Procedure proc = null;

        InOutArgument outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
        EnumProcedureResult epr = selectProcedure(engine, procedureTitle, outResult);
        if (epr == EnumProcedureResult.CancelledByUser)
            return epr;
        // если процедура не найдена, selectProcedure возвращает Error, а эта процедура должна вернуть Success,
        // так как не ее проблема, что нечего удалять.
        else if (epr == EnumProcedureResult.Error)
            return EnumProcedureResult.Success;
        else
        {
            // set Procedure object
            proc = outResult.getValueProcedure();
        }
        // Тут в объекте proc уже должна быть выбранная команда.

        // 3. выбранную пользователем команду - проверить, что она может быть удалена
        // - она может быть удалена, если она находится в БД. Сейчас БиблиотекиПроцедур не позволяют удалять или редактировать свои Процедуры и Места.
        // - надо добавить в объект Процедуры и Места (Item) флаг ReadOnly, для БД он сброшен, для Библиотек Процедур он установлен.
        // - Флаг не должен храниться в БД, только в памяти.
        // - TODO: решено позже добавить в объект Item ссылку на объект Хранилища, унифицированный для БД и БиблиотекаПроцедур,
        // и из него запрашивать возможность удаления и изменения итемов.
        if (proc.isItemCanRemoved() == false)
        {
            // Если указанная Команда не может быть удалена, вывести сообщение об этом и завершить текущую Процедуру успешно.
            String st2 = String.format("Выбранная команда \"%s\" не может быть удалена из ее хранилища \"%s\"", procedureTitle, proc.get_Storage());
            engine.get_OperatorConsole().PrintTextLine(st2, EnumDialogConsoleColor.Предупреждение);
            return EnumProcedureResult.Success;
        }
        // else

        // 4. Показать пользователю свойства выбранной команды и запросить подтверждение удаления.
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("Подтвердите удаление Команды", EnumDialogConsoleColor.Сообщение);
        engine.get_OperatorConsole().PrintProcedureForm(proc);
        // и запросить подтверждение пользователя, что он желает удалить Команду.
        // Если пользователь ответит Да, надо удалить Команду.
        // Если пользователь ответит Нет или Отмена, отменить операцию.
        EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете удалить Команду?");
        if (sdr.isНет() || sdr.isОтмена())
            return EnumProcedureResult.CancelledByUser;
        // 5. Удалить команду.
        engine.get_ECM().RemoveProcedure(proc);

        // 6. вывести сообщение о результате операции: успешно
        String msg6 = String.format("Команда \"%s\" успешно удалена.", procedureTitle);
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
 * RT-Обработчик процедуры Изменить команду НазваниеКоманды.
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
        Title = "Изменить команду НазваниеКоманды",
        Description = "Изменить указанную команду Оператора.")
    public static EnumProcedureResult CommandChangeProcedure(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
{

    EnumProcedureResult result = EnumProcedureResult.Success;
    // название текущей процедуры для лога итп.
    // указать здесь полный путь как название процедуры для вывода на экран.
    String currentProcedureTitle = "GeneralProcedures.ProcedureProcedures.CommandChangeProcedure";
    // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
    // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
    String stmp; // temp string
    EnumSpeakDialogResult esdr; // temp dialog result
    InOutArgument outResult; // temp in-out argument object
    EnumProcedureResult epr; // temp procedure result
    try
    {
        // DONE: вывести это тестовое сообщение о начале процедуры - в лог!
        stmp = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, args.getByIndex(0).get_ArgumentValue());
        engine.AddMessageToConsoleAndLog(stmp, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);

        // 1. Извлечь из аргумента название процедуры
        String procedureTitle;
        FuncArgument arg = args.getByIndex(0);
        procedureTitle = arg.get_ArgumentQueryValue().trim();// берем сырой текст аргумента из запроса
        engine.get_OperatorConsole().PrintTextLine(String.format("Название изменяемой Команды: \"%s\"", procedureTitle), EnumDialogConsoleColor.Сообщение);

        // TODO: проверить признак того, что вместо названия процедуры движком было подставлено название зарегистрированного места
        if (arg.get_АвтоподстановкаМеста() == true)
        {
            ; // TODO: обработать тут случай автоподстановки места вместо названия команды
              // если он возникнет
        }

        // 2. извлечь из БД список процедур с таким названием и показать пользователю.
        // без учета регистра символов

        Procedure proc = null;

        outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
        epr = selectProcedure(engine, procedureTitle, outResult);
        if (epr == EnumProcedureResult.CancelledByUser)
            return epr;
        // если процедура не найдена, selectProcedure возвращает Error, а эта процедура должна вернуть Success,
        // так как не ее проблема, что нечего изменять.
        else if (epr == EnumProcedureResult.Error)
            return EnumProcedureResult.Success;
        else
        {
            // set Procedure object
            proc = outResult.getValueProcedure();
        }

        // Тут в объекте proc уже должна быть выбранная команда.

        // 3. выбранную пользователем процедуру - проверить, что она может быть изменена.
        // - она может быть изменена, если она находится в БД. Сейчас БиблиотекиПроцедур не позволяют удалять или редактировать свои Процедуры и Места.
        // - надо добавить в объект Процедуры и Места (Item) флаг ReadOnly, для БД он сброшен, для Библиотек Процедур он установлен.
        // Флаг не хранится в БД, только в памяти.
        // - TODO: решено позже добавить в объект Item ссылку на объект Хранилища, унифицированный для БД и БиблиотекаПроцедур,
        // и из него запрашивать возможность удаления и изменения итемов.
        if (proc.isItemCanChanged() == false)
        {
            // Если указанная Команда не может быть удалена, вывести сообщение об этом и завершить текущую Процедуру успешно.
            stmp = String.format("Выбранная команда \"%s\" не может быть изменена.", procedureTitle);
            engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Предупреждение);
            return EnumProcedureResult.Success;
        }
        // else
        //создадим копию, чтобы не перезаписывать оригинал из коллекции Команд
        Procedure procNew = new Procedure(proc);
        // 4. По каждому свойству Команды:
        // - показывать текущее значение свойства и спрашивать, желает ли пользователь его изменить. (Да-Нет-Отмена-Пропустить)
        // Если не желает, перейти к следующему свойству.
        // Если желает, вывести справку, запросить новое значение, проверить его формат и вписать в объект Процедуры.
        // Если отменил, то отменить операцию.

        // 4.1 Название команды
        // - вывести заголовок
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("1. Название Команды", EnumDialogConsoleColor.Сообщение);
        // - вывести текущее значение
        stmp = String.format("Текущее название: \"%s\"", proc.get_Title());
        engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
        // - запросить изменение
        // Если пользователь ответит Да, надо запросить новое значение.
        // Если пользователь ответит Нет, надо пропустить это поле.
        // Если пользователь ответит Отмена, прервать операцию.
        esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить название Команды?");
        if (esdr.isОтмена())
            return EnumProcedureResult.CancelledByUser;
        else if (esdr.isДа())
        {
            outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
            epr = readProcedureTitleForExisting(engine, outResult, proc);
            if (epr == EnumProcedureResult.CancelledByUser)
                return epr;
            else
            {
                // set regex value
                stmp = outResult.getValueString();
                procNew.set_Title(stmp);
            }
        }
        // else Нет - перейти к следующему свойству Команды.

        // 4.2 Описание команды
        // - вывести заголовок
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("2. Описание Команды", EnumDialogConsoleColor.Сообщение);
        // - вывести текущее значение
        stmp = String.format("Текущее описание: \"%s\"", proc.get_Description());
        engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
        // - запросить изменение
        // Если пользователь ответит Да, надо запросить новое значение.
        // Если пользователь ответит Нет, надо пропустить это поле.
        // Если пользователь ответит Отмена, прервать операцию.
        esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить описание Команды?");
        if (esdr.isОтмена())
            return EnumProcedureResult.CancelledByUser;
        else if (esdr.isДа())
        {
            outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
            epr = readProcedureDescription(engine, outResult);
            if (epr == EnumProcedureResult.CancelledByUser)
                return epr;
            else
            {
                // set regex value
                stmp = outResult.getValueString();
                procNew.set_Description(stmp);
            }
        }
        // else Нет - перейти к следующему свойству Команды.

        // 4.3 Регекс команды
        // - вывести заголовок
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("3. Регекс Команды", EnumDialogConsoleColor.Сообщение);
        // - вывести текущее значение
        stmp = String.format("Текущий регекс: \"%s\"", proc.get_Regex());
        engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
        // - запросить изменение
        // Если пользователь ответит Да, надо запросить новое значение.
        // Если пользователь ответит Нет, надо пропустить это поле.
        // Если пользователь ответит Отмена, прервать операцию.
        esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить регекс Команды?");
        if (esdr.isОтмена())
            return EnumProcedureResult.CancelledByUser;
        else if (esdr.isДа())
        {
            outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
            epr = readProcedureRegex(engine, outResult);
            if (epr == EnumProcedureResult.CancelledByUser)
                return epr;
            else
            {
                // set regex value
                stmp = outResult.getValueString();
                procNew.set_Regex(stmp);
            }
        }
        // else Нет - перейти к следующему свойству Команды.

        // 4.4 Адрес команды
        // - вывести заголовок
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("4. Адрес Процедуры Команды", EnumDialogConsoleColor.Сообщение);
        // - вывести текущее значение
        stmp = String.format("Текущий адрес: \"%s\"", proc.get_Path());
        engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
        // - запросить изменение
        // Если пользователь ответит Да, надо запросить новое значение.
        // Если пользователь ответит Нет, надо пропустить это поле.
        // Если пользователь ответит Отмена, прервать операцию.
        esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить адрес Команды?");
        if (esdr.isОтмена())
            return EnumProcedureResult.CancelledByUser;
        else if (esdr.isДа())
        {
            outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
            epr = readProcedureAdres(engine, outResult);
            if (epr == EnumProcedureResult.CancelledByUser)
                return epr;
            else
            {
                // set namespace value
                stmp = outResult.getValueString();
                procNew.set_Path(stmp);
            }
        }
        // else Нет - перейти к следующему свойству Команды.

        // 4.5 Вес команды
        // - вывести заголовок
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("5. Вес Команды", EnumDialogConsoleColor.Сообщение);
        // - вывести текущее значение
        stmp = String.format("Текущий вес: \"%s\"", proc.get_Ves());
        engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
        // - запросить изменение
        // Если пользователь ответит Да, надо запросить новое значение.
        // Если пользователь ответит Нет, надо пропустить это поле.
        // Если пользователь ответит Отмена, прервать операцию.
        EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить вес Команды?");
        if (sdr.isОтмена())
            return EnumProcedureResult.CancelledByUser;
        else if (sdr.isДа())
        {
            outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
            epr = readProcedureVes(engine, outResult);
            if (epr == EnumProcedureResult.CancelledByUser)
                return epr;
            else
            {
                // set ves value
                Double ves = outResult.getValueDouble();
                procNew.set_Ves(ves);
            }
        }
        // else Нет - перейти к следующему свойству Команды.

        // 4.6 Категория команды
        // - вывести заголовок
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("6. Категория для Команды", EnumDialogConsoleColor.Сообщение);
        // - вывести текущее значение
        stmp = String.format("Текущая категория: \"%s\"", proc.get_Namespace());
        engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Сообщение);
        // - запросить изменение
        // Если пользователь ответит Да, надо запросить новое значение.
        // Если пользователь ответит Нет, надо пропустить это поле.
        // Если пользователь ответит Отмена, прервать операцию.
        esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить категорию Команды?");
        if (esdr.isОтмена())
            return EnumProcedureResult.CancelledByUser;
        else if (esdr.isДа())
        {
            outResult = new InOutArgument();// оболочка для строки-результата, передаваемого как аргумент.
            epr = readProcedureCategory(engine, outResult);
            if (epr == EnumProcedureResult.CancelledByUser)
                return epr;
            else
            {
                // set namespace value
                stmp = outResult.getValueString();
                procNew.set_Namespace(stmp.trim());
            }
        }
        // else Нет - перейти к следующему свойству Команды.

        // 5. Вывести общий набор свойств Процедуры и запросить пользователя, желает ли он сохранить эти данные.(Да-Нет-Отмена)
        // - если пользователь ответил да, изменить процедуру через ECM функцию.
        // - при любом другом ответе пользователя отменить операцию.
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("Подтвердите изменение Команды", EnumDialogConsoleColor.Сообщение);
        engine.get_OperatorConsole().PrintProcedureForm(procNew);
        // и запросить подтверждение пользователя, что он желает удалить Команду.
        // Если пользователь ответит Да, надо удалить Команду.
        // Если пользователь ответит Нет или Отмена, отменить операцию.
        esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете изменить Команду?");
        if (esdr.isНет() || esdr.isОтмена())
            return EnumProcedureResult.CancelledByUser;
        // записать команду
        engine.get_ECM().UpdateProcedure(procNew);

        // 6. вывести сообщение о результате операции: успешно
        engine.get_OperatorConsole().PrintTextLine(String.format("Команда %s успешно изменена", procedureTitle), EnumDialogConsoleColor.Успех);
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
 * RT- Обработчик процедуры Удалить все команды
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
 * @throws Exception
 *             Ошибка при исполнении Процедуры.
 */
@OperatorProcedure(State = ImplementationState.Ready,
        Title = "Удалить все команды",
        Description = "Удалить все Команды из БазаДанныхОператора.")
    public static EnumProcedureResult CommandDeleteAllProcedures(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args) throws Exception
{

    // содержимое списка аргументов
    // args[0].name = "команда" - название аргумента в строке регекса команды
    // args[0].value = "Скачать файл ХХХ" - значение аргумента - название создаваемого места
    // args[0].type = "" - тип аргумента - сейчас не указывается, так как мне лень думать

    EnumProcedureResult result = EnumProcedureResult.Success;
    // название текущей процедуры для лога итп.
    String currentProcedureTitle = "GeneralProcedures.ProcedureProcedures.CommandDeleteAllProcedures";
        // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
        // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
        try
        {
        //1. Запросить от пользователя подтверждение о удалении всех команд из БД. Да - Нет и Отмена.
        //если пользователь ответил Да - удалить все команды из БД
        //если пользователь ответил Нет или Отмена - прервать операцию. 
        engine.get_OperatorConsole().PrintEmptyLine();
        engine.get_OperatorConsole().PrintTextLine("1. Подтвердите удаление всех команд из БазаДанныхОператора", EnumDialogConsoleColor.Сообщение);
        EnumSpeakDialogResult esdr = engine.get_OperatorConsole().PrintДаНетОтмена("Желаете удалить все команды?");
        if (esdr.isНет() || esdr.isОтмена())
            return EnumProcedureResult.CancelledByUser;
        // выполнить удаление всех Команд из БД Оператора
        engine.get_ECM().RemoveAllProceduresFromDatabase();
        // вывести сообщение о результате операции: успешно
        engine.get_OperatorConsole().PrintTextLine("Удаление всех Команд завершено успешно", EnumDialogConsoleColor.Успех);
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
 *            Procedure title.
 * @param outResult
 *            Result Procedure in shell object.
 * @return Function returns EnumProcedureResult.Success if success, Error if procedure not found, CancelledByUser if cancelled.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult selectProcedure(
        Engine engine,
        String title,
        InOutArgument outResult) throws Exception
{
    Procedure proc = null;
    LinkedList<Procedure> lip = engine.get_ECM().get_ProcedureCollection().getByTitle(title);
        int lipSize = lip.size();
        if (lipSize == 0)
        {
        // тут вывести сообщение, что указанная команда не найдена и завершить процедуру успешно.
        String stmp = String.format("Указанная команда \"%s\" не найдена", title);
        engine.get_OperatorConsole().PrintTextLine(stmp, EnumDialogConsoleColor.Предупреждение);
        return EnumProcedureResult.Error;
    }
        else if (lipSize == 1)
        {
        proc = lip.get(0);
    }
        else // if(lipSize > 1)
        {
        // тут вывести пользователю найденные команды с тем же названием
        engine.get_OperatorConsole().PrintTextLine("Существующие команды с таким названием:", EnumDialogConsoleColor.Предупреждение);
        // Если в списке более одной команды, то пользователь должен выбрать одну из них для удаления.
        // для этого в списке Команд нужно показать уникальный ИД команды, источник-хранилище, категорию, название и описание.
        // И флаг, возможно ли удаление данной команды - это определяется Хранилищем.
        // Для выбора из нескольких Команд надо запросить у пользователя ИД команды, но проблема в том, что ид сейчас есть только у объектов из БД.
        // - можно вывести порядковый номер в этом списке и запросить у пользователя его.
        engine.get_OperatorConsole().PrintProcedureFormNumberedList(lip);
        // запросить у пользователя порядковый номер элемента списка
        int lip_index = engine.get_OperatorConsole().InputListIndex(lipSize);
        // обработать указанный пользователем индекс
        if (lip_index < 1)
        {
            // Если пользователь отказался выбрать элемент списка, вывести сообщение об этом и завершить текущую Процедуру Отменено пользователем.
            engine.get_OperatorConsole().PrintTextLine("Операция отменена, поскольку пользователь не смог выбрать Команду из списка.", EnumDialogConsoleColor.Сообщение);
            return EnumProcedureResult.CancelledByUser;
        }
        else proc = lip.get(lip_index);// выбранный пользователем объект команды.
    }

    // return
    outResult.setValue(proc);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Procedure Title value from User, for newly create Procedure.
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
private static EnumProcedureResult readProcedureTitleForNew(
        Engine engine,
        InOutArgument outResult,
        String oldTitle) throws Exception
{
    // вывести справку по созданию названия для команды.
    printHelpProcedureTitleProp(engine);
    // temp string with old title
    String str = new String(oldTitle);
// 2 проверить что в БД нет Процедуры с таким названием, без учета регистра символов
boolean notUnicalProcedure = false;
while (true)
{
    // если название команды - пустая строка, вывести сообщение и перейти к приему нового названия команды
    if (OperatorEngine.Utility.StringIsNullOrEmpty(str))
        engine.get_OperatorConsole().PrintTextLine("Пустая строка недопустима для названия Команды!", EnumDialogConsoleColor.Предупреждение);
    else
    {
        // проверить что в БД нет Процедуры с таким названием, без учета регистра символов
        LinkedList<Procedure> lip = engine.get_ECM().get_ProcedureCollection().getByTitle(str);
        notUnicalProcedure = (lip.size() > 0);// временный флаг для упрощения проверок позже
        if (notUnicalProcedure)
        {
            // тут вывести пользователю найденные команды с тем же названием
            engine.get_OperatorConsole().PrintTextLine("Команды с таким названием уже существуют:", EnumDialogConsoleColor.Предупреждение);
            for (Procedure pp : lip)
            {
                engine.get_OperatorConsole().PrintProcedureShortLine(pp);
            }
            engine.get_OperatorConsole().PrintTextLine("Дубликаты Команд недопустимы!", EnumDialogConsoleColor.Предупреждение);
            lip.clear();// очистить временный список, поскольку он в цикле
        }
    }
    if (str.isEmpty() || (notUnicalProcedure == true))
    {
        // Раз есть такие Процедуры, пользователь должен сменить название Процедуры прямо тут же
        // или же завершить диалог Отменой создания команды
        str = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите новое название Команды:", false, true);
        if (Dialogs.этоОтмена(str))
            return EnumProcedureResult.CancelledByUser;
    }
    else break;// end while loop
}// while loop
 // Тут мы окажемся, если название Процедуры уникальное
 // return
outResult.setValue(str);
return EnumProcedureResult.Success;
    }

    /**
     * NT-Read Procedure Title value from User, for change or rename Procedure.
     * 
     * @param engine
     *            Engine object.
     * @param outResult
     *            Result title string shell object.
     * @param proc
     *            Change procedure object.
     * @return Function returns EnumProcedureResult.Success if success; EnumProcedureResult.CancelledByUser if cancelled.
     * @throws Exception
     *             Error occured.
     */
    private static EnumProcedureResult readProcedureTitleForExisting(
            Engine engine,
            InOutArgument outResult,
            Procedure proc) throws Exception
{
    // вывести справку по созданию названия для команды.
    printHelpProcedureTitleProp(engine);
    // temp string with old title
    String str = new String(proc.get_Title());
// 2 проверить что в БД нет Процедуры с таким названием, без учета регистра символов, кроме текущей.
boolean notUnicalProcedure = false;
while (true)
{
    // 1 введите новое название, пустые строки не принимаюся.
    // Раз есть такие Процедуры, пользователь должен сменить название Процедуры прямо тут же
    // или же завершить диалог Отменой создания команды
    // no empty answer's allowed by arg4! str cannot be empty here.
    str = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите новое название Команды:", false, true);

    // 3 если отмена - выйти с отменой
    if (Dialogs.этоОтмена(str))
        return EnumProcedureResult.CancelledByUser;

    // 4 проверить уникальность процедуры
    // если процедура уникальна, то выйти с названием процедуры
    // иначе - сообщить что команда уже существует и перейти на новый цикл.

    // проверить что в БД нет Процедуры с таким названием, без учета регистра символов
    // кроме текущей Процедуры - она же есть.
    LinkedList<Procedure> lip = engine.get_ECM().get_ProcedureCollection().getByTitle(str);
    // удалить текущий объект Процедуры из этого списка
    if (lip.contains(proc))
    {
        lip.remove(proc);
    }
    notUnicalProcedure = (lip.size() > 0);// временный флаг для упрощения проверок позже
    if (notUnicalProcedure)
    {
        // тут вывести пользователю найденные команды с тем же названием
        engine.get_OperatorConsole().PrintTextLine("Команды с таким названием уже существуют:", EnumDialogConsoleColor.Предупреждение);
        for (Procedure pp : lip)
        {
            engine.get_OperatorConsole().PrintProcedureShortLine(pp);
        }
        engine.get_OperatorConsole().PrintTextLine("Дубликаты Команд недопустимы!", EnumDialogConsoleColor.Предупреждение);
        lip.clear();// очистить временный список, поскольку он в цикле
        continue;
    }
    else break;// break while loop

}// while loop
 // Тут мы окажемся, если название Процедуры уникальное
 // return
outResult.setValue(str);
return EnumProcedureResult.Success;
    }

    /**
     * NT-Read Procedure Description value from User
     * 
     * @param engine
     *            Engine object
     * @param outResult
     *            Result string shell object
     * @return Function returns EnumProcedureResult.Success if success, or other values.
     * @throws Exception
     *             Error occured.
     */
    private static EnumProcedureResult readProcedureDescription(
            Engine engine,
            InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpProcedureDescriptionProp(engine);
    // - запросить новое значение
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите краткое описание Команды:", true, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;

    // return
    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Procedure Regex value from User
 * 
 * @param engine
 *            Engine object
 * @param outResult
 *            Result string shell object
 * @return Function returns EnumProcedureResult.Success if success, or other values.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult readProcedureRegex(
        Engine engine,
        InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpProcedureRegexProp(engine);
    // - запросить новое значение
    // - проверить новое значение
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите регекс для Команды:", true, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;
    // TODO: я пока не знаю, как проверить регекс в этом месте. Поэтому просто поверим, что пользователь все сделал правильно.

    // return
    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Procedure Adres value from User
 * 
 * @param engine
 *            Engine object
 * @param outResult
 *            Result string shell object
 * @return Function returns EnumProcedureResult.Success if success, or other values.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult readProcedureAdres(
        Engine engine,
        InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpProcedureAdresProp(engine);
    // - запросить новое значение
    // - проверить новое значение
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите адрес Процедуры для Команды:", true, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;
    // TODO: надо проверить что файл или сборка, класс, функция - существуют, и если нет - сообщить об этом пользователю.
    // но это не критичная ошибка - пользователь может заново ввести или применить это значение, несмотря на.
    // return
    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Procedure Ves value from User
 * 
 * @param engine
 *            Engine object
 * @param outResult
 *            Result string shell object
 * @return Function returns EnumProcedureResult.Success if success, or other values.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult readProcedureVes(
        Engine engine,
        InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpProcedureVesProp(engine);
    // - запросить новое значение
    boolean isValidValue = false;
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите Вес Команды:", true, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;
        do
        {
        // надо проверить что введенное значение конвертируется в Double, больше 0 и меньше 1.
        isValidValue = Procedure.IsValidVesFormat(stmp);
        // если значение веса неправильное, то сообщить об этом и запросить новое значение, в цикле, пока пользователь не отменит всю команду.
        if (isValidValue == false)
        {
            engine.get_OperatorConsole().PrintTextLine("Это значение Веса является недопустимым!", EnumDialogConsoleColor.Предупреждение);
            stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите новое значение Веса для Команды:", true, true);
            if (Dialogs.этоОтмена(stmp))
                return EnumProcedureResult.CancelledByUser;
        }
    }
        while (isValidValue == false);

    Double ves = Double.valueOf(stmp);
    // return
    outResult.setValue(ves);
        return EnumProcedureResult.Success;
}

/**
 * NT-Read Procedure Category value from User
 * 
 * @param engine
 *            Engine object
 * @param outResult
 *            Result string shell object
 * @return Function returns EnumProcedureResult.Success if success, or other values.
 * @throws Exception
 *             Error occured.
 */
private static EnumProcedureResult readProcedureCategory(
        Engine engine,
        InOutArgument outResult) throws Exception
{
    // - вывести справку
    printHelpProcedureNsProp(engine);
    // - запросить новое значение
    // показать существующие неймспейсы, пока только для Процедур.
    String existingNsChain = engine.get_ECM().getNamespacesChainString(true, false, false);
    engine.get_OperatorConsole().PrintTextLine(" - Можно придумать новую категорию для этой Команды, или использовать уже существующую:", EnumDialogConsoleColor.Сообщение);
    // сдвинуть на 3 позиции вправо для выравнивания на экране.
    engine.get_OperatorConsole().PrintTextLine("   " + existingNsChain, EnumDialogConsoleColor.Сообщение);
    engine.get_OperatorConsole().PrintEmptyLine();
    // пользователь должен ввести название пространства имен для новой Процедуры
    String stmp = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите название категории для Команды:", false, true);
        if (Dialogs.этоОтмена(stmp))
            return EnumProcedureResult.CancelledByUser;
    // else return
    outResult.setValue(stmp);
        return EnumProcedureResult.Success;
}

/**
 * NT-вывести на консоль справку по свойству команды Название.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpProcedureTitleProp(Engine engine)
{
    String[] titleDescr = new String[] {
                " - Название команды идентифицирует команду для пользователя.",
                " - Название команды должно быть коротким, понятным, уникальным.",
                " - Для удобства пользователя, название команды должно совпадать с текстом команды, но начинаться с заглавной буквы.",
                " - Если команда предполагает аргументы, они могут быть включены в Название команды в порядке их следования.",
                "   Например: Создать команду НазваниеКоманды",
                "   Или: Скачать файл и сохранить как НазваниеФайла",
                " - Для подробной информации о названиях команд обратитесь к документации Оператор.",
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
private static void printHelpProcedureDescriptionProp(Engine engine)
{
    String[] descr = new String[] {
                " - Описание команды описывает смысл и действие команды для пользователя.",
                " - Описание должно быть коротким, однострочным но понятным.",
                " - Описание  должно начинаться с заглавной буквы и оканчиваться точкой.",
                " - Описание должно позволить пользователю различать схожие по названию команды.",
                " - Для подробной информации о описаниях команд обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(descr, EnumDialogConsoleColor.Сообщение);
    return;

}

/**
 * NT-вывести на консоль справку по свойству команды Категория.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpProcedureNsProp(Engine engine)
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
 * NT-вывести на консоль справку по свойству команды Вес.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpProcedureVesProp(Engine engine)
{
    String[] vesDescr = new String[] {
                " - Вес определяет порядок выбора для исполнения одной из Команд, подходящих по Регексу",
                " - Команда с наибольшим Весом будет выполнена последней",
                " - Вес должен быть больше 0.0 и меньше 1.0",
                " - Для новой Команды рекомендуется значение 0.5",
                " - Для подробной информации о весе команды обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(vesDescr, EnumDialogConsoleColor.Сообщение);
    return;
}

/**
 * NT-вывести на консоль справку по свойству команды Адрес.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpProcedureAdresProp(Engine engine)
{
    String[] adresDescr = new String[] {
                " - Описывает командную строку исполняемого файла или путь к Процедуре Команды в Сборке Процедур",
                " - Для исполняемых файлов, вызываемых в качестве Процедур, путь может содержать аргументы.",
                "   Пример вызова без аргументов: \"shutdown -h now\"",
                "   Пример вызова с аргументами: \"/home/username/firefox/firefox.sh %www\"",
                "   Аргументы идентифицируются по своим именам, заданным в Регексе Команды.",
                " - Для Процедур из СборкиПроцедур прописывается путь в формате БиблиотекаПроцедур.Класс.Функция().",
                "   Аргументы идентифицируются по своим именам, заданным в Регексе Команды.",
                "   Например: ProceduresInt.ProcedureProcedures.CommandCreateProcedure()",
                " - Для подробной информации о адресе процедуры команды обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(adresDescr, EnumDialogConsoleColor.Сообщение);
    return;
}

/**
 * NT-вывести на консоль справку по свойству команды Регекс.
 * 
 * @param engine
 *            Ссылка на объект движка Оператор.
 */
private static void printHelpProcedureRegexProp(Engine engine)
{
    String[] regexDescr = new String[] {
                " - Команда будет выбрана для исполнения, если ее Регекс опознает текст, введенный Пользователем",
                " - Простой Регекс содержит текст Команды и аргументы. Аргумент обозначается словом с знаком % перед ним.",
                " - Название аргумента должно содержать только латинские буквы, цифры, знак_подчеркивания.",
                " - Например: Открыть сайт %arg_1",
                " - Простой регекс может быть и без аргументов, например: выключить компьютер",
                " - Сложный Регекс это специально форматированный текст. ",
                " - Для подробной информации о регексе команды обратитесь к документации Оператор.",
                "" };
    engine.get_OperatorConsole().PrintTextLines(regexDescr, EnumDialogConsoleColor.Сообщение);
    return;
}

}
