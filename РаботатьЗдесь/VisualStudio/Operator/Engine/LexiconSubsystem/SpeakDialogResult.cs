using System;

namespace Engine.LexiconSubsystem
{
    /// <summary>
    /// NT-Статический класс вспомогательных функций для енума SpeakDialogResult
    /// </summary>
    public static class SDRHelper
    {

        /// <summary>
        /// NT-Проверить, что запрашиваемые флаги установлены
        /// </summary>
        /// <param name="flag">Один или несколько флагов EnumSpeakDialogResult</param>
        /// <param name="val">Проверяемый набор флагов</param>
        public static bool HasFlag(SpeakDialogResult val, SpeakDialogResult flag)
        {
            CheckValue(val);
            CheckValue(flag);
            return ((val & flag) != 0);
        }
        /// <summary>
        /// NT-Проверить, что значение аргумента находится в допустимых пределах
        /// </summary>
        /// <param name="sdr">Значение аргумента как набора флагов EnumSpeakDialogResult</param>
        public static void CheckValue(SpeakDialogResult sdr)
        {
            if ((sdr < 0) || (sdr > SpeakDialogResult.AllFlags))
                throw new ArgumentOutOfRangeException("sdr", sdr, "Неправильное значение флага");
            else
                return;
        }

        /// <summary>
        /// NT-Результат это Да
        /// </summary>
        /// <param name="sdr">Проверяемое значение</param>
        /// <returns>
        ///   Функция возвращает true, если значение = Да, false в остальных случаях.
        /// </returns>
        public static bool IsYes(SpeakDialogResult sdr)
        {
            CheckValue(sdr);
            return sdr == SpeakDialogResult.Да;
        }

        /// <summary>
        /// NT-Результат это Нет
        /// </summary>
        /// <param name="sdr">Проверяемое значение</param>
        /// <returns>
        ///   Функция возвращает true, если значение = Нет, false в остальных случаях.
        /// </returns>
        public static bool IsNo(SpeakDialogResult sdr)
        {
            CheckValue(sdr);
            return sdr == SpeakDialogResult.Нет;
        }
        /// <summary>
        /// NT-Результат это Отмена
        /// </summary>
        /// <param name="sdr">Проверяемое значение</param>
        /// <returns>
        ///   Функция возвращает true, если значение = Отмена, false в остальных случаях.
        /// </returns>
        public static bool IsCancel(SpeakDialogResult sdr)
        {
            CheckValue(sdr);
            return sdr == SpeakDialogResult.Отмена;
        }
    }

    /// <summary>
    /// Представляет код результата стандартных диалогов
    /// Коды можно комбинировать по OR
    /// </summary>
    [Flags]
    public enum SpeakDialogResult
    {
        /// <summary>
        /// Неопределенный ответ
        /// </summary>
        /// <remarks>
        /// Обычно означает, что ответ нельзя отнести к определенному коду результата.
        /// </remarks>
        Unknown = 0,
        /// <summary>
        /// Положительный ответ (Yes, OK)
        /// </summary>
        /// <remarks>
        /// Используется в диалогах Да-Нет-Отмена для подтверждения операции
        /// </remarks>
        Да = 1,
        /// <summary>
        /// Отрицательный ответ (No)
        /// </summary>
        /// <remarks>
        /// Используется в диалогах Да-Нет-Отмена для отрицания операции
        /// </remarks>
        Нет = 2,
        /// <summary>
        /// Субкоманда Отмены (Cancel)
        /// </summary>
        /// <remarks>
        /// Используется в диалогах как субкоманда отмены всей процедуры, чтобы прервать-отменить текущий диалог.
        /// </remarks>
        Отмена = 4,
        /// <summary>
        /// Субкоманда Прервать операцию (Abort)
        /// </summary>
        /// <remarks>
        /// Используется в диалогах как субкоманда отмены всей процедуры, чтобы прервать-отменить текущий диалог.
        /// Синоним Отмена.
        /// </remarks>
        Прервать = 8,
        /// <summary>
        /// Субкоманда Повторить операцию (Retry)
        /// </summary>
        /// <remarks>
        /// Вероятно, используется в диалогах как субкоманда для повтора операции внутри диалога, если логика работы позволяет повтор операции.
        /// Непроработанная сущность.
        /// </remarks>
        Повторить = 16,
        /// <summary>
        /// Субкоманда Игнорировать (Ignore)
        /// </summary>
        /// <remarks>
        /// Вероятно, используется в диалогах как субкоманда для игнорирования события неудачи внутри диалога, если логика работы позволяет пропуск операции.
        /// Непроработанная сущность.
        /// </remarks>
        Пропустить = 32,
        /// <summary>
        /// Субкоманда Отложить операцию на другое время.
        /// </summary>
        /// <remarks>
        /// Вероятно, используется в диалогах как субкоманда для того чтобы отложить весь диалог на некоторое время. Если логика работы позволяет отложить операцию и весь диалог на неопределенное время.
        /// Непроработанная сущность.
        /// </remarks>
        Отложить = 64,
        /// <summary>
        /// Комбинация флагов субкоманд Да Нет Отмена - для упрощения кода
        /// </summary>
        ДаНетОтмена = SpeakDialogResult.Да | SpeakDialogResult.Нет | SpeakDialogResult.Отмена,
        /// <summary>
        /// Служебная комбинация всех флагов субкоманд для проверки максимально допустимого значения
        /// </summary>
        AllFlags = SpeakDialogResult.Да | SpeakDialogResult.Нет | SpeakDialogResult.Отложить | SpeakDialogResult.Отмена | SpeakDialogResult.Повторить | SpeakDialogResult.Прервать | SpeakDialogResult.Пропустить,

        //TODO: при добвлении новых элементов добавить их также в Dialogs.makeСтрокаОжидаемыхОтветов() и еще где-то используются. 

    }
}
