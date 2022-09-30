using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.LexiconSubsystem
{
    /// <summary>
    /// Представляет код результата стандартных диалогов. 
    /// Коды можно комбинировать по OR (Flags enum).
    /// </summary>
    [Flags]
    public enum EnumSpeakDialogResult
    {
        /**
     * Неопределенный ответ.
     * Обычно означает, что ответ нельзя отнести к определенному коду
     * результата.
     */
    public static final int Unknown = 0;

    /**
     * Положительный ответ (Yes, OK).
     * Используется в диалогах Да-Нет-Отмена для подтверждения операции
     */
    public static final int Да = 1;

    /**
     * Отрицательный ответ (No)
     * Используется в диалогах Да-Нет-Отмена для отрицания операции
     */
    public static final int Нет = 2;

    /**
     * Субкоманда Отмены (Cancel)
     * Используется в диалогах как субкоманда отмены всей процедуры, чтобы
     * прервать-отменить текущий диалог.
     */
    public static final int Отмена = 4;

    /**
     * Субкоманда Прервать операцию (Abort)
     * Используется в диалогах как субкоманда отмены всей процедуры, чтобы
     * прервать-отменить текущий диалог.
     * Синоним Отмена.
     */
    public static final int Прервать = 8;

    /**
     * Субкоманда Повторить операцию (Retry)
     * Вероятно, используется в диалогах как субкоманда для повтора операции
     * внутри диалога,
     * если логика работы позволяет повтор операции.
     * Непроработанная сущность.
     */
    public static final int Повторить = 16;

    /**
     * Субкоманда Игнорировать (Ignore)
     * Вероятно, используется в диалогах как субкоманда для игнорирования
     * события неудачи внутри диалога,
     * если логика работы позволяет пропуск операции.
     * Непроработанная сущность.
     */
    public static final int Пропустить = 32;

    /**
     * Субкоманда Отложить операцию на другое время.
     * Вероятно, используется в диалогах как субкоманда для того чтобы отложить
     * весь диалог на некоторое время.
     * Если логика работы позволяет отложить операцию и весь диалог на
     * неопределенное время.
     * Непроработанная сущность.
     */
    public static final int Отложить = 64;

    /**
     * Комбинация флагов субкоманд Да Нет Отмена - часто используемая, введена
     * для упрощения кода
     * 
     */
    public static final int ДаНетОтмена = EnumSpeakDialogResult.Да | EnumSpeakDialogResult.Нет | EnumSpeakDialogResult.Отмена;

    /**
     * Служебная комбинация всех флагов субкоманд для проверки максимально
     * допустимого значения
     */
    private static final int AllFlags = EnumSpeakDialogResult.Да | EnumSpeakDialogResult.Нет | EnumSpeakDialogResult.Отложить | EnumSpeakDialogResult.Отмена | EnumSpeakDialogResult.Повторить | EnumSpeakDialogResult.Прервать | EnumSpeakDialogResult.Пропустить;
    /**
     * Object value
     */
    private int m_Value;

    /**
     * Constructor
     */
    @SuppressWarnings("unused")
    private EnumSpeakDialogResult()
    {
        this.m_Value = EnumSpeakDialogResult.Unknown;

        return;
    }

    /**
     * Constructor
     * 
     * @param value
     *            New object value
     */
    public EnumSpeakDialogResult(int value)
    {
        this.m_Value = value;

        return;
    }

    /**
     * Get value
     * 
     * @return the value
     */
    public int getValue()
    {
        return m_Value;
    }

    /**
     * Set value
     * 
     * @param value
     *            the value to set
     * @throws Exception
     *             Функция выбрасывает исключение, если значение аргумента
     *             находится вне допустимых пределов.
     */
    public void setValue(int value) throws Exception
    {
        // проверить входное значение - это набор флагов или что-то случайное
        checkValue(value);

        this.m_Value = value;

        return;
    }

    /**
     * Проверить, что запрашиваемые флаги установлен(ы)
     * 
     * @param flag
     *            Один или несколько флагов EnumSpeakDialogResult
     * @return Функция возвращает true, если запрашиваемые флаги установлен(ы)
     * @throws Exception
     *             Функция выбрасывает исключение, если значение аргумента
     *             находится вне допустимых пределов.
     */
    public boolean hasFlag(int flag) throws Exception
    {
        // проверить входное значение - это набор флагов или что-то случайное
        checkValue(flag);

        return ((this.getValue() & flag) != 0);
    }

    /**
     * Проверить, что значение аргумента находится в допустимых пределах
     * 
     * @param value
     *            Значение аргумента как набора флагов EnumSpeakDialogResult
     * @throws Exception
     *             Функция выбрасывает исключение, если значение аргумента
     *             находится вне допустимых пределов.
     */
    private void checkValue(int value) throws Exception
    {
        if ((value < 0) || (value > EnumSpeakDialogResult.AllFlags))
            throw new Exception("Invalid argument value");

        return;
    }

/**
 * Проверить что значение аргумента совпадает с значением объекта
 * 
 * @param flag
 *            значение аргумента
 * @return Функция возвращает true, если значения совпадают.
 */
public boolean isEqualValue(int flag)
{
    return (this.m_Value == flag);
}

/**
 * Проверить что значение аргумента совпадает с значением объекта
 * 
 * @param esdr
 *            значение аргумента
 * @return Функция возвращает true, если значения совпадают.
 */
public boolean isEqualValue(EnumSpeakDialogResult esdr)
{
    return (this.m_Value == esdr.m_Value);
}

/**
 * NT-Результат это Да
 * 
 * @return Функция возвращает true, если значение = Да, false в остальных случаях.
 */
public boolean isДа()
{
    return this.m_Value == EnumSpeakDialogResult.Да;
}

/**
 * NT-Результат это Нет
 * 
 * @return Функция возвращает true, если значение = Нет, false в остальных случаях.
 */
public boolean isНет()
{
    return this.m_Value == EnumSpeakDialogResult.Нет;
}

/**
 * NT-Результат это Отмена
 * 
 * @return Функция возвращает true, если значение = Отмена, false в остальных случаях.
 */
public boolean isОтмена()
{
    return this.m_Value == EnumSpeakDialogResult.Отмена;
}

}
}