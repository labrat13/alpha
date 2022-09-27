using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ProcedureSubsystem
{
    /// <summary>
    /// Состояние реализации методов, помеченных данным атрибутом.
    /// </summary>
    public enum ImplementationState
    {
        /// <summary>
        /// Метод не реализован (NR).
        /// </summary>
        NotRealized,
        /// <summary>
        /// Метод, класс, сборка реализован, но не тестирован (NT).
        /// </summary>
        NotTested,
        /// <summary>
        /// Метод, класс, сборка реализован, тестирован, готов к применению (RT).
        /// </summary>
        Ready
    }
}