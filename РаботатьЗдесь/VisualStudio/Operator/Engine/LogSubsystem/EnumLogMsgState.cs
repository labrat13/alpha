using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.LogSubsystem
{
    /// <summary>
    /// NR-Код состояния события лога
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