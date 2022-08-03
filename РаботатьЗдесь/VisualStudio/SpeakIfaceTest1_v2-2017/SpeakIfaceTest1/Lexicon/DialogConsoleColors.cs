using System;
using System.Collections.Generic;
using System.Text;

namespace Operator.Lexicon
{
    /// <summary>
    /// Выбранные цвета для текстовых диалогов консоли
    /// </summary>
    public enum DialogConsoleColors: uint
    {
        /// <summary>
        /// Цвет обычного текста консоли
        /// </summary>
        Сообщение = ConsoleColor.Gray,
        /// <summary>
        /// Цвет текстов, введенных пользователем
        /// </summary>
        ВводПользователя = ConsoleColor.White,
        /// <summary>
        /// Цвет выводимых в консоли предупреждений.
        /// </summary>
        Предупреждение = ConsoleColor.DarkMagenta,
        /// <summary>
        /// Цвет выводимых в консоли вопросов пользователю
        /// </summary>
        Вопрос = ConsoleColor.DarkYellow,
        /// <summary>
        /// Цвет выводимых в консоли подтверждений.
        /// </summary>
        Успех = ConsoleColor.DarkGreen,


    }
}
