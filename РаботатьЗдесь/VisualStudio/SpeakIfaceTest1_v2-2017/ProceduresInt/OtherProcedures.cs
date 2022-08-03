using System;
using System.Collections.Generic;
using System.Text;
using Operator;
using System.IO;
using System.Diagnostics;
using Operator.Lexicon;

namespace ProceduresInt
{
    //Чтобы отлаживать эти процедуры через отладчик студии, скопируйте файлы dll и pdb в каталог \x86\Debug основного приложения.
    //и запустите отладку основного приложения. Все другие варианты тут не работают.
    
    /// <summary>
    /// Прочие разные процедуры, пока их деть некуда
    /// </summary>
    public static class OtherProcedures
    {
        /// <summary>
        /// Обработчик процедуры создания заметки Блокнота на Рабочем столе
        /// </summary>
        /// <param name="engine">Механизм исполнения команд</param>
        /// <param name="cmdline">Текст запроса для возможной дополнительной обработки</param>
        /// <param name="args">Список аргументов</param>
        /// <returns>
        /// Вернуть ProcedureResult.Success в случае успешного выполнения команды.
        /// Вернуть ProcedureResult.WrongArguments если аргументы не подходят для запуска команды.
        /// Вернуть ProcedureResult.Error если произошла ошибка при выполнении операции
        /// Вернуть ProcedureResult.ExitXXX если нужно завершить работу текущего приложения, выключить или перезагрузить компьютер.
        /// </returns>
        [ProcedureAttribute(ImplementationState.NotTested)]
        public static ProcedureResult CommandCreateNotepadNote(Engine engine, string query, ArgumentCollection args)
        {
            //команда: создать заметку НазваниеЗаметки
            
            //TODO: вывести это тестовое сообщение о начале процедуры - в лог!
            engine.OperatorConsole.PrintTextLine(String.Format("Начата процедура CommandCreateNotepadNote(\"{0}\")", args.Arguments[0].ArgumentValue), DialogConsoleColors.Сообщение);

            //не прокатит: если НазваниеЗаметки совпадает с названием места, то вместо него будет подставлено место,  поэтому не получится получить название заметки.
            //TODO: вот это проблема: Места блокируют аргументы во всех запросах, невзирая на ожидаемые классы аргументов.
            //и как это разрулить? Вынести этот вопрос в концепцию в вики!

            //1 извлечь название заметки
            //извлечь название места из аргумента
            FuncArgument arg = args.Arguments[0];
            String title;
            //извлечь название файла: берем сырую строку запроса, так как Места тут не нужны, даже если движок подставил одно из них.
            title = arg.ArgumentQueryValue.Trim();//берем сырое значение аргумента из запроса.

            bool ФайлУжеСуществует = false;

        label_1:

            //удалить из названия неправильные символы - заменить на _
            title = ReplaceInvalidPathChars(title, '_');
            
            //2 если файл уже существует, запросить новое имя для заметки
            String fpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), title + ".txt");
            ФайлУжеСуществует = File.Exists(fpath);
            if (ФайлУжеСуществует == true)
            {
                //вывести сообщение что файл с таким названием заметки уже существует
                engine.OperatorConsole.SureConsoleCursorStart();
                engine.OperatorConsole.PrintTextLine(String.Format("Файл заметки {0} уже существует.", title), DialogConsoleColors.Предупреждение);
                //вывести запрос о смене названия заметки
                engine.OperatorConsole.SureConsoleCursorStart();
                SpeakDialogResult sdr = engine.OperatorConsole.PrintДаНетОтмена("Изменить название заметки?");
                //если Да, запросить новое название заметки и перейти к label_1
                if (sdr == SpeakDialogResult.Да)
                {
                    engine.OperatorConsole.SureConsoleCursorStart();
                    String str = engine.OperatorConsole.PrintQuestionAnswer(SpeakDialogResult.Отмена, "Введите новое название заметки:", true, true);
                    if (Dialogs.этоОтмена(str))
                        return ProcedureResult.CancelledByUser;
                    title = String.Copy(str);
                    goto label_1;
                }
                //если Отмена, то прервать выполнение процедуры
                else 
                    if (sdr == SpeakDialogResult.Отмена)
                    return ProcedureResult.CancelledByUser;
                //если Нет, то открыть существующий файл для добавления
                //просто ничего не делаем тут
            }

            //2 создать на рабочем столе текстовый файл в кодировке 1251 
            StreamWriter sw = new StreamWriter(fpath, true, Encoding.GetEncoding(1251));
            //если файл новый, то вывести в него строку названия заметки как заголовок
            if (ФайлУжеСуществует == false)
            {
                sw.WriteLine(title);
                sw.WriteLine();
            }
            sw.Close();
            //3 открыть этот файл в блокноте через командную строку.
            //а можно получить путь к блокноту из места Блокнот? Надо это тоже обсужиь в вики: можно ли использовать Места из кода процедур?
            Process.Start(fpath);
            //вывести сообщение о результате операции: успешно
            engine.OperatorConsole.PrintTextLine(String.Format("Заметка \"{0}\" создана", title), DialogConsoleColors.Успех);

            //вернуть флаг продолжения работы
            return ProcedureResult.Success;
        }

        /// <summary>
        /// Заменить неправильные символы в названии файла на указанный символ
        /// </summary>
        /// <param name="title">Название файла</param>
        /// <param name="p">Замена</param>
        /// <returns>Возвращает безопасное название файла</returns>
        private static string ReplaceInvalidPathChars(string title, char p)
        {
            //TODO: перенести эту функцию в более правильное место по семантике.
            StringBuilder sb = new StringBuilder(title);
            Char[] inpc = Path.GetInvalidFileNameChars();
            foreach (Char c in inpc)
                sb = sb.Replace(c, p);

            return sb.ToString();
        }

        /// <summary>
        /// Обработчик процедуры открытия заметки Блокнота на Рабочем столе
        /// </summary>
        /// <param name="engine">Механизм исполнения команд</param>
        /// <param name="cmdline">Текст запроса для возможной дополнительной обработки</param>
        /// <param name="args">Список аргументов</param>
        /// <returns>
        /// Вернуть ProcedureResult.Success в случае успешного выполнения команды.
        /// Вернуть ProcedureResult.WrongArguments если аргументы не подходят для запуска команды.
        /// Вернуть ProcedureResult.Error если произошла ошибка при выполнении операции
        /// Вернуть ProcedureResult.ExitXXX если нужно завершить работу текущего приложения, выключить или перезагрузить компьютер.
        /// </returns>
        [ProcedureAttribute(ImplementationState.NotTested)]
        public static ProcedureResult CommandOpenNotepadNote(Engine engine, string query, ArgumentCollection args)
        {
            //команда: открыть заметку НазваниеЗаметки

            //TODO: вывести это тестовое сообщение о начале процедуры - в лог!
            engine.OperatorConsole.PrintTextLine(String.Format("Начата процедура CommandOpenNotepadNote(\"{0}\")", args.Arguments[0].ArgumentValue), DialogConsoleColors.Сообщение);

            //1 извлечь название заметки
            //извлечь название места из аргумента
            FuncArgument arg = args.Arguments[0];
            String title;
            //извлечь название файла: берем сырую строку запроса, так как Места тут не нужны, даже если движок подставил одно из них.
            title = arg.ArgumentQueryValue.Trim();//берем сырое значение аргумента из запроса.

            //2 удалять неправильные символы будем? Да, хотя я же не должен ввести неправильное название заметки
            //удалить из названия неправильные символы - заменить на _
            title = ReplaceInvalidPathChars(title, '_');
            //Название заметки - с заглавной буквы? Обычно Да, но не всегда. Лучше не менять, чтобы случаи вроде "eSIM" не портились. 

            //если файл не существует, вывести предупреждение Заметка не найдена
            //создавать новую не надо!
            String fpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), title + ".txt");
            if (File.Exists(fpath) == false)
            {
                //вывести сообщение что файл с таким названием заметки не существует
                engine.OperatorConsole.SureConsoleCursorStart();
                engine.OperatorConsole.PrintTextLine(String.Format("Файл заметки {0} не найден на Рабочем столе.", title), DialogConsoleColors.Предупреждение);
                return ProcedureResult.Error;//завершим процедуру с флагом ошибки?
            }

            //3 открыть этот файл в блокноте через командную строку.
            //а можно получить путь к блокноту из места Блокнот? Надо это тоже обсужиь в вики: можно ли использовать Места из кода процедур?
            Process.Start(fpath);

            //вывести сообщение о результате операции: успешно
            engine.OperatorConsole.PrintTextLine(String.Format("Заметка \"{0}\" открыта", title), DialogConsoleColors.Успех);

            //вернуть флаг продолжения работы
            return ProcedureResult.Success;
        }





    }
}
