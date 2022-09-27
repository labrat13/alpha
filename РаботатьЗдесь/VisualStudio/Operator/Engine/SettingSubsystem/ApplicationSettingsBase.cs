using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.SettingSubsystem
{
    /// <summary>
    /// NR - Представляет базовый класс файла настроек приложения.
    /// </summary>
    public class ApplicationSettingsBase : Engine.OperatorEngine.EngineSubsystem
    {

        /// <summary>
        /// NR - Конструктор
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        public ApplicationSettingsBase(OperatorEngine.Engine engine) : base(engine)
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
