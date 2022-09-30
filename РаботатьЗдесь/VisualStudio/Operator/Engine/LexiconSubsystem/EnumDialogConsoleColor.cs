using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.LexiconSubsystem
{
    //TODO: заменить на значения из старого Оператор.
    /// <summary>
    /// NR - Выбранные цвета для текстовых диалогов консоли. 
    /// </summary>
    public enum EnumDialogConsoleColor
    {
        /// <summary>
    ///Неопределенный.
     /// </summary>
        Unknown=0,

        /// <summary>
        ///Цвет обычного текста консоли.
         /// </summary>
        Сообщение=TerminalMode.COLOR_GRAY,

        /// <summary>
        ///Цвет текстов, введенных пользователем.
         /// </summary>
        ВводПользователя=TerminalMode.COLOR_BRIGHT_GRAY,

        /// <summary>
        ///Цвет текста выводимых в консоли предупреждений.
         /// </summary>
        Предупреждение=TerminalMode.COLOR_MAGENTA,

        /// <summary>
        ///Цвет текста выводимых в консоли вопросов пользователю.
         /// </summary>
        Вопрос=TerminalMode.COLOR_BRIGHT_YELLOW,

        /// <summary>
        ///Цвет текста выводимых в консоли подтверждений.
         /// </summary>
        Успех=TerminalMode.COLOR_GREEN,

        /// <summary>
        ///Цвет текста выводимых в консоли сообщений о критических ошибках.
         /// </summary>
        Критический=TerminalMode.COLOR_BRIGHT_RED,

    }
}