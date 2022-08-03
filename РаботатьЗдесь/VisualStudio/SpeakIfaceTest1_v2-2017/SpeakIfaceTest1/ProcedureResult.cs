using System;
using System.Collections.Generic;
using System.Text;

namespace Operator
{
    /// <summary>
    /// Значение, возвращаемое процедурой в цикл обработки запроса.
    /// </summary>
    public enum ProcedureResult
    {
        /// <summary>
        /// Неизвестно
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Неправильный выбор процедуры, неподходящие аргументы.
        /// </summary>
        /// <remarks>
        /// Это значит, процедура не соответствует запросу. Цикл выборки процедур должен найти и попробовать другую процедуру.
        /// </remarks>
        WrongArguments = 1,
        /// <summary>
        /// Успех - процедура выполнена успешно
        /// </summary>
        /// <remarks>
        /// Это значит, процедура соответствует запросу. Цикл выборки процедур должен завершиться.
        /// </remarks>
        Success = 2,
        /// <summary>
        /// Завершение приложения Оператор
        /// </summary>
        Exit = 3,
        /// <summary>
        /// Завершение и сон компьютера
        /// </summary>
        ExitAndSleep = 4,
        /// <summary>
        /// Завершение и перезагрузка компьютера
        /// </summary>
        ExitAndReload = 5,
        /// <summary>
        /// Завершение и выключение компьютера
        /// </summary>
        ExitAndShutdown = 6,
        /// <summary>
        /// Завершение и выход пользователя
        /// </summary>
        ExitAndLogoff = 7,
        /// <summary>
        /// Завершение и легкий сон компьютера
        /// </summary>
        ExitAndHybernate = 8,
        /// <summary>
        /// Ошибка при выполнении процедуры
        /// </summary>
        Error = 9,
        /// <summary>
        /// Выполнение процедуры отменено пользователем
        /// </summary>
        CancelledByUser = 10,

    }
}
