using System;
using System.Collections.Generic;
using System.Text;
using Operator;
using Operator.Lexicon;


namespace FirstProcedures
{
    /// <summary>
    /// Класс должен содержать реализации процедур для команд Оператора
    /// </summary>
    public static class Procedures 
    {
        ///// <summary>
        ///// Пример функции процедуры обработчика команды
        ///// </summary>
        ///// <param name="engine">Механизм исполнения команд</param>
        ///// <param name="query">Текст запроса для возможной дополнительной обработки</param>
        ///// <param name="args">Список аргументов</param>
        ///// <returns>
        ///// Вернуть ProcedureResult.Success в случае успешного выполнения команды.
        ///// Вернуть ProcedureResult.WrongArguments если аргументы не подходят для запуска команды.
        ///// Вернуть ProcedureResult.Error если произошла ошибка при выполнении операции
        ///// Вернуть ProcedureResult.ExitXXX если нужно завершить работу текущего приложения, выключить или перезагрузить компьютер.
        ///// </returns>
        //[ProcedureAttribute(ImplementationState.NotTested)]//изменить состояние ImplementationState на подходящее после отладки функции
        //public static ProcedureResult CommandHandlerExample(Engine engine, string query, ArgumentCollection args)
        //{
        //    //вернуть флаг продолжения работы
        //    return ProcedureResult.Success;
        //}

        /// <summary>
        /// Пример функции процедуры обработчика команды
        /// </summary>
        /// <param name="engine">Механизм исполнения команд</param>
        /// <param name="query">Текст запроса для возможной дополнительной обработки</param>
        /// <param name="args">Список аргументов</param>
        /// <returns></returns>
        [ProcedureAttribute(ImplementationState.NotTested)]
        public static ProcedureResult CommandHandlerExample(Engine engine, string query, ArgumentCollection args)
        {
            //вывести сообщение на консоль Оператора
            engine.OperatorConsole.PrintTextLine("Message from command handler function", DialogConsoleColors.Сообщение);
            //вернуть флаг продолжения работы
            return ProcedureResult.Success;
        }

    }
}
