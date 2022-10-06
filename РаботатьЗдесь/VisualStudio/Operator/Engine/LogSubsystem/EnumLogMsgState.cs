namespace Engine.LogSubsystem
{
    /// <summary>
    /// Код состояния события лога
    /// </summary>
    public enum EnumLogMsgState
    {
        /// <summary>
        /// Default (0) or Unknown
        /// </summary>
        Default = 0,
        /// <summary>
        /// Fail state
        /// </summary>
        Fail = 1,
        /// <summary>
        /// Success state
        /// </summary>
        OK = 2
    }
}