using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Engine.LexiconSubsystem
{
    /// <summary>
    /// NT - Анализатор команды - для исполнения сложных и составных команд.
    /// Менеджер всей подсистемы Lexicon.
    /// </summary>
    internal class BCSA : Engine.OperatorEngine.EngineSubsystem
    {
        // BigCommandSemanticAnalyser - такое длинное название

        /// <summary>
        /// NR - Конструктор
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        public BCSA(OperatorEngine.Engine engine) : base(engine)
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
            // init array Dialogs.ExitAppCommands from settings file string
            // это не потребуется очищать при завершении данной подсистемы.
            String words = this.m_Engine.get_EngineSettings().getValue(EnumSettingKey.ExitAppCommands);
            if (Utility.StringUtility.StringIsNullOrEmpty(words))
            {
                String msg = String.Format("Ошибка! В файле настроек отсутствует необходимое поле \"{0}\" или значение для него", EnumSettingKey.ExitAppCommands.getTitle());
                throw new Exception(msg);
            }
            String[] sar = Utility.StringUtility.SplitCommaDelimitedString(words);
            Dialogs.ExitAppCommands = sar;

            // set ready flag
            this.m_Ready = true;

            return;
        }

        /// <summary>
        /// NR - De-initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected override void onClose()
        {
            // Cleanup log subsystem here
            if (this.m_Ready == true)
            {
                // TODO: add cleanup code here
            }
            // clear ready flag
            this.m_Ready = false;

            return;
        }
        #endregion




        //    /**
        //     * NT-Разобрать входной запрос команды, построить граф исполнения и
        //     * исполнить команду
        //     * 
        //     * @param engine
        //     *            Объект движка
        //     * @param query
        //     *            Текст исходного запроса
        //     * @return Функция возвращает код результата исполнения процедуры
        //     * @throws Exception
        //     *             Error on DoQuery()
        //     */
        //    public static EnumProcedureResult ProcessQuery(Engine engine, String query)
        //            throws Exception
        //    {
        //        // сейчас тупо исполним весь запрос целиком
        //        EnumProcedureResult result = engine.DoQuery(query);
        //
        //        // а вообще это неправильно - для составных команд надо:
        //        // 1. попытаться найти для нее процедуру-исполнителя как есть. Но найти
        //        // и не исполнять пока!
        //        // 2. если не нашлось, то пытаться разделить запрос на части по смыслу,
        //        // и пытаться найти для них процедуру-исполнителя. Но найти и не
        //        // исполнять пока!
        //        // 3. если не нашлось процедуры хотя бы для одной из частей запроса,
        //        // исполнять команду нельзя!
        //        // В итоге, это все не вписывается в существующую архитектуру системы.
        //        // Надо все переделывать для поддержки составных команд.
        //        // а тут - пока - можно приводить глаголы в команде к первичной форме:
        //        // спи = спать, найди = найти.
        //        // Вот и вся интеллектуальная обработка тут.
        //
        //        return result;
        //
        //    }

        #region Функции для Русского языка

        /// <summary>
        /// Русский язык для конверсий и зависимых от языка операций
        /// </summary>  
        /// <remarks>
        /// Настройки пользователя локальной системы перекрывают типовую культуру.
        /// Если нужно игнорировать пользовательские изменения культуры, замените true на false.
        /// </remarks>
        public static CultureInfo RuCulture = new CultureInfo("ru-RU", true);


        /// <summary>
        /// Буквы русского алфавита маленькие (Строчные)
        /// </summary>
        private const String RussianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        /// <summary>
        /// Буквы русского алфавита большие (Прописные)
        /// </summary>
        private const String RussianAlphabetShift = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";



        /// <summary>
        /// NT-Вернуть True, если первые три символа - не русскоязычные.
        /// </summary>
        /// <param name="query">Текст запроса</param>
        /// <returns> Функция возвращает True, если первые три символа - не русскоязычные.</returns>    
        public static bool IsNotRussianFirst(String query)
        {
            // Это функция для распознавания русскоязычной команды. Против текстов
            // для командной строки, подобных "wget -c -nc www.xxx.com"
            // Если команда русскоязычная, она должна начинаться с глагола или
            // подобного русского слова.
            // Значит, если первые три символа - русские, то весь текст -
            // русскоязычная команда.
            // А если нет - то это команда для терминала.
            // Но я точно не представляю себе эту ситуацию, так что почему два
            // символа, а не один или три?
            String q = query.Trim();
            if (q.Length == 1)
                return !IsRussianLetter(q[0]);
            else if (q.Length > 1)
                return (!(IsRussianLetter(q[0]) && IsRussianLetter(q[1])));
            else return true;// вернуть флаг, что буквы не русские

        }

        /// <summary>
        /// NT-Возвращает True если проверяемый символ - русская буква
        /// </summary>
        /// <param name="p">Проверяемый символ</param>
        /// <returns>Возвращает True если проверяемый символ - русская буква</returns>
        private static bool IsRussianLetter(char p)
        {
            return ((RussianAlphabet.IndexOf(p) != -1) || (RussianAlphabetShift.IndexOf(p) != -1));
        }
        #endregion



    }
}
