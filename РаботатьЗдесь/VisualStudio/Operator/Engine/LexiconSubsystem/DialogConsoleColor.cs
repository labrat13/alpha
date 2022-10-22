using System;

namespace Engine.LexiconSubsystem
{
    /// <summary>
    /// NT - Выбранные цвета для текстовых диалогов консоли. 
    /// </summary>
    public enum DialogConsoleColor : uint
    {
        /// <summary>
        /// Значение ошибки
        /// </summary>
        Unknown = 0,
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
        /// <summary>
        ///Цвет текста выводимых в консоли сообщений о критических ошибках.
        /// </summary>
        Критический = ConsoleColor.Red,
    }
}