using System;
using System.Collections.Generic;
using System.Text;

namespace Operator
{
    /// <summary>
    /// Оценка типа регекса
    /// </summary>
    public enum RegexType
    {
        /// <summary>
        /// Неизвестно
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Обычный регекс
        /// </summary>
        NormalRegex = 1,
        /// <summary>
        /// Простая строка
        /// </summary>
        SimpleString = 2,
        /// <summary>
        /// Пустое поле
        /// </summary>
        Empty = 3,
        /// <summary>
        /// Неправильный формат
        /// </summary>
        Invalid = 4,
    }
}
