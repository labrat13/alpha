using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralProcedures
{
    internal class PowerProcedures
    {
    }


    //Класс методов для Процедур должен быть помечен аннотацией OperatorProcedure с ImplementationState = NotTested либо Ready, чтобы его методы можно было
    //вызывать в качестве процедур.
    //Класс может также содержать любые элементы, необходимые для методов Процедур.

    /** NT-Класс Процедур управления питанием компьютера (Выключение, перезагрузка, сон, итп)
     * @author Селяков Павел
     *
     */
    @OperatorProcedure(State = ImplementationState.Ready,
    Title = "Power class",
    Description = "Power procedures class.")
public class PowerProcedures
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


        //Сейчас выключение и перезагрузку компьютера должна делать функция постобработки команды, чтобы корректно завершать Оператор.
        //Для этого надо на команды Выключить компьютер и Перезагрузить компьютер прицепить эти процедуры-заглушки, которые просто возвращают код для постобработки.
        //А постобработка использует команды из ФайлНастроекОператор для вызова процедур компьютера.

        /**
         * RT-Обработчик процедуры Вернуть флаг выключения.
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
                Title = "Вернуть флаг выключения",
                Description = "Вернуть флаг выключения для постобработки.")
    public static EnumProcedureResult DummyShutdown(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {

            return EnumProcedureResult.ExitAndShutdown;
        }

        /**
         * RT-Обработчик процедуры Вернуть флаг перезагрузки.
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
                Title = "Вернуть флаг перезагрузки",
                Description = "Вернуть флаг перезагрузки для постобработки.")
    public static EnumProcedureResult DummyReload(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {

            return EnumProcedureResult.ExitAndReload;
        }


    }


}
