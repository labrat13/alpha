using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - Движок исполнения Процедур.
    /// </summary>
    public class Engine : EngineSubsystem
    {
        /// <summary>
        /// NR - Конструктор
        /// </summary>
        public Engine() : base(null)
        {
            //TODO: Add code here
        }

        #region  *** Override this from EngineSubsystem parent class ***
        /// <summary>
        /// NR - Initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onOpen()
        {
            throw new Exception("Function must be overridden");//TODO: Add code here
        }

        /// <summary>
        /// NR - De-initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onClose()
        {
            throw new Exception("Function must be overridden");//TODO: Add code here
        }
        #endregion

    }
}
