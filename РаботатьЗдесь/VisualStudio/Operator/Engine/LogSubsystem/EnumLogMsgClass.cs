namespace Engine.LogSubsystem
{
    /// <summary>
    /// Код класса события лога
    /// </summary>
    public enum EnumLogMsgClass
    {

        /// <summary>
        /// Default (0) or Unknown
        /// </summary>
        Default = 0,
        /// <summary>
        /// Session start message
        /// </summary>
        SessionStarted = 1,
        /// <summary>
        /// Session finish message
        /// </summary>
        SessionFinished = 2,
        /// <summary>
        /// User input query text
        /// </summary>
        QueryStarted = 3,
        /// <summary>
        /// Query text has been changed
        /// </summary>
        QueryReplaced = 4,
        /// <summary>
        /// User query processing finished
        /// </summary>
        QueryFinished = 5,
        /// <summary>
        /// Exception raised to break execution of Operator
        /// </summary>
        ExceptionRaised = 6,
        /// <summary>
        /// Exception suppressed or processed by engine
        /// </summary>
        ExceptionSuppressed = 7,
        /// <summary>
        /// Startup Operator procedure events
        /// </summary>
        StartupExecution = 8,
        /// <summary>
        /// Finish Operator procedure execution
        /// </summary>
        FinishExecution = 9,
        /// <summary>
        /// Общесистемное событие для лога.
        /// </summary>
        SubsystemEvent_General = 10,
        /// <summary>
        /// Событие подсистемы БД для лога.
        /// </summary>
        SubsystemEvent_Database = 11,
        /// <summary>
        /// Событие подсистемы терминала для лога.
        /// </summary>
        SubsystemEvent_Terminal = 12,
        /// <summary>
        /// Событие подсистемы Семантической обработки для лога.
        /// </summary>
        SubsystemEvent_Lexicon = 13,
        /// <summary>
        /// Событие подсистемы Движка для лога.
        /// </summary>
        SubsystemEvent_Engine = 14,
        /// <summary>
        /// Событие подсистемы Процедур для лога.
        /// </summary>
        SubsystemEvent_Procedure = 15,
        /// <summary>
        /// Событие подсистемы Настроек для лога.
        /// </summary>
        SubsystemEvent_Settings = 16,
        /// <summary>
        /// Событие подсистемы утилит для лога.
        /// </summary>
        SubsystemEvent_Utility = 17,
        /// <summary>
        /// Сообщение дампа данных для отладки
        /// </summary>
        DebugLoggingMessage = 18,

        // TODO: add new enum member here
        // TODO: Добавить новые классы событий лога здесь.

        /// <summary>
        /// Максимальное значение енума для упрощения добавления новых членов.
        /// </summary>
        MaxValue = 16384,
    }
}