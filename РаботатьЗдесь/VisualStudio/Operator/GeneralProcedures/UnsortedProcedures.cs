using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralProcedures
{
    internal class UnsortedProcedures
    {
    }

    /**
 * Класс тестовых Процедур для Оператор.
 * 
 * @author Селяков Павел
 *
 */
    @OperatorProcedure(State = ImplementationState.NotTested,
        Title = "Unsorted Procedures class",
        Description = "Разные процедуры для команд.")
public class UnsortedProcedures
    {
        // Для всех операций с Процедурами и Местами из кода Процедур использовать класс ElementCacheManager, а не БД итп.
        // engine.get_ECM().AddPlace(p);

        // Памятка: Если аннотация @OperatorProcedure не указана, Движок будет выдавать исключение, что соответствующий элемент не помечен аннотацией, и не будет
        // исполнять Процедуру.
        // Если @OperatorProcedure.State = ImplementationState.NotRealized, Движок будет выдавать исключение, что соответствующий элемент не реализован, и не будет
        // исполнять Процедуру.
        // Если @OperatorProcedure.State = ImplementationState.NotTested, Движок будет выдавать сообщение, что соответствующий элемент не тестирован, и будет
        // исполнять Процедуру.
        // Если @OperatorProcedure.State = ImplementationState.Ready, Движок будет исполнять Процедуру.
        // Следовательно:
        // - Если вы недописали Процедуру, пометьте ее ImplementationState.NotRealized, чтобы Оператор не пытался исполнять эту Процедуру.
        // - Если вы дописали но не протестировали Процедуру, пометьте ее ImplementationState.NotTested, чтобы Оператор выводил предупреждение, что запускаемая
        // Процедура не проверена.
        // - Если вы протестировали Процедуру, и она правильно работает, пометьте ее ImplementationState.Ready, чтобы Оператор не выводил ненужные более
        // предупреждения.



        /**
        * NT-Обработчик процедуры Открыть терминал.
        * 
        * 
        * @param engine
        *            Ссылка на объект Движка Оператор для доступа к консоли, логу, БД итп.
        * @param manager
        *            Ссылка на объект Менеджера Библиотеки Процедур для доступа к инициализированным ресурсам библиотеки.
        * @param query
        *            Текст исходного запроса пользователя для возможной дополнительной обработки.
        * @param args
        *            Массив аргументов Процедуры, соответствующий запросу.
        * @return Функция возвращает результат как одно из значений EnumProcedureResult:
        *         EnumProcedureResult.Success если Процедура выполнена успешно;
        *         EnumProcedureResult.WrongArguments если аргументы не подходят для запуска Процедуры;
        *         EnumProcedureResult.Error если произошла ошибка при выполнении Процедуры;
        *         EnumProcedureResult.CancelledByUser если выполнение Процедуры прервано Пользователем;
        *         EnumProcedureResult.Exit если после выполнения Процедуры требуется завершить работу Оператор;
        *         EnumProcedureResult.ExitAndLogoff если после выполнения Процедуры требуется завершить сеанс пользователя;
        *         EnumProcedureResult.ExitAndHybernate если после выполнения Процедуры требуется перевести компьютер в спящий режим;
        *         EnumProcedureResult.ExitAndSleep если после выполнения Процедуры требуется перевести компьютер в спящий режим;
        *         EnumProcedureResult.ExitAndReload если после выполнения Процедуры требуется перезагрузить компьютер;
        *         EnumProcedureResult.ExitAndShutdown если после выполнения Процедуры требуется выключить компьютер;
        */
        @OperatorProcedure(State = ImplementationState.Ready,
             Title = "Открыть терминал",
             Description = "Открытие терминала по пути из ФайлНастроекОператора или ТаблицаНастроекОператора.")
  public static EnumProcedureResult OpenTerminal(
       Engine engine,
       LibraryManagerBase manager,
       UserQuery query,
       ArgumentCollection args)
        {
            /*
             * 07042022 - Добавлена возможность внутри Процедуры изменять текст запроса,
             * чтобы применить новый текст запроса к дальнейшему поиску Процедур.
             * Изменение запроса не перезапускает поиск Процедур (в текущей версии Оператора).
             * Поэтому изменять запрос следует только в хорошо продуманных случаях.
             * 
             * Пример вызова функции переопределения запроса, с выводом в лог старого и нового значений.
             * Example: query.ChangeQuery(engine, "New query text");
             */
            EnumProcedureResult result = EnumProcedureResult.Success;
            //указать здесь полный путь как название процедуры для вывода на экран.    
            String currentProcedureTitle = "GeneralProcedures.UnsortedProcedures.OpenTerminal";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                //вывести это тестовое сообщение о начале процедуры - в лог!
                String str = "Начата процедура " + currentProcedureTitle + "()";
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);
                engine.get_OperatorConsole().PrintEmptyLine();

                //1. извлечь из настроек команду открытия терминала: сначала из ФайлНастроекОператора, потом из ТаблицаНастроекОператора.
                //2. если пути нет, вывести сообщение об ошибке  и вернуть WrongArguments, чтобы Оператор искал другую Процедуру для этого запроса.
                EnumProcedureResult epr = engine.StartAloneTerminal();
                if (epr == EnumProcedureResult.Error)
                    result = EnumProcedureResult.WrongArguments;
                else if (epr == EnumProcedureResult.Success)
                {
                    result = epr;
                    //вывести сообщение о результате операции: успешно
                    engine.get_OperatorConsole().PrintTextLine("Команда успешно завершена", EnumDialogConsoleColor.Успех);
                }
                else
                    result = epr;
            }
            catch (Exception ex)
            {
                engine.PrintExceptionMessageToConsoleAndLog("Ошибка в процедуре " + currentProcedureTitle + "()", ex);
                result = EnumProcedureResult.Error;
            }

            // вернуть флаг продолжения работы
            return result;
        }


        /**
        * NT-команда создать заметку НазваниеЗаметки
        * 
        * 
        * @param engine
        *            Ссылка на объект Движка Оператор для доступа к консоли, логу, БД итп.
        * @param manager
        *            Ссылка на объект Менеджера Библиотеки Процедур для доступа к инициализированным ресурсам библиотеки.
        * @param query
        *            Текст исходного запроса пользователя для возможной дополнительной обработки.
        * @param args
        *            Массив аргументов Процедуры, соответствующий запросу.
        * @return Функция возвращает результат как одно из значений EnumProcedureResult:
        *         EnumProcedureResult.Success если Процедура выполнена успешно;
        *         EnumProcedureResult.WrongArguments если аргументы не подходят для запуска Процедуры;
        *         EnumProcedureResult.Error если произошла ошибка при выполнении Процедуры;
        *         EnumProcedureResult.CancelledByUser если выполнение Процедуры прервано Пользователем;
        *         EnumProcedureResult.Exit если после выполнения Процедуры требуется завершить работу Оператор;
        *         EnumProcedureResult.ExitAndLogoff если после выполнения Процедуры требуется завершить сеанс пользователя;
        *         EnumProcedureResult.ExitAndHybernate если после выполнения Процедуры требуется перевести компьютер в спящий режим;
        *         EnumProcedureResult.ExitAndSleep если после выполнения Процедуры требуется перевести компьютер в спящий режим;
        *         EnumProcedureResult.ExitAndReload если после выполнения Процедуры требуется перезагрузить компьютер;
        *         EnumProcedureResult.ExitAndShutdown если после выполнения Процедуры требуется выключить компьютер;
        */
        @OperatorProcedure(State = ImplementationState.Ready,
             Title = "Создать заметку",
             Description = "Создание заметки на рабочем столе.")
  public static EnumProcedureResult CommandCreateNotepadNote(
       Engine engine,
       LibraryManagerBase manager,
       UserQuery query,
       ArgumentCollection args)
        {

            EnumProcedureResult result = EnumProcedureResult.Success;
            // название текущей процедуры для лога итп.   
            String currentProcedureTitle = "GeneralProcedures.UnsortedProcedures.CommandCreateNotepadNote";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                //1. извлечь название заметки 
                //извлечь название файла: берем из аргумента сырую строку запроса, так как Места тут не нужны, даже если движок подставил одно из них.
                String title = args.getByIndex(0).get_ArgumentQueryValue().Trim();

                //копируем название заметки для последующей записи в файл.
                String titleAsContent = new String(title);
                //вывести это тестовое сообщение о начале процедуры - в лог!
                String msg1 = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, title);
                engine.AddMessageToConsoleAndLog(msg1, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);
                engine.get_OperatorConsole().PrintEmptyLine();

                //Если в конце названия заметки стоит точка, то из имени файла ее надо убрать, а то она туда попадает и получается неправильно: заметка..txt
                title = Utility.RemoveEndingDots(title).Trim();
                //TODO: если этот цикл переписать с использованием File как основы для хранения пути файла, 
                // то он будет немного проще и быстрее, и потом URI из него получать будет быстрее.
                Boolean ФайлУжеСуществует = false;
                String fpath = null;
                //label_1:
                while (true)
                {
                    //удалить из названия неправильные символы - заменить на _
                    title = Utility.ReplaceInvalidPathChars(title, "_");


                    //2 если файл уже существует, запросить новое имя для заметки
                    fpath = FileSystemManager.getUserDesktopFolderPath() + title + ".txt";
                    ФайлУжеСуществует = FileSystemManager.isFileExists(fpath);
                    if (ФайлУжеСуществует == true)
                    {
                        //вывести сообщение что файл с таким названием заметки уже существует
                        engine.get_OperatorConsole().SureConsoleCursorStart();
                        engine.get_OperatorConsole().PrintTextLine(String.format("Файл заметки \"%s\" уже существует.", title), EnumDialogConsoleColor.Предупреждение);
                        //вывести запрос о смене названия заметки
                        engine.get_OperatorConsole().SureConsoleCursorStart();
                        EnumSpeakDialogResult sdr = engine.get_OperatorConsole().PrintДаНетОтмена("Изменить название заметки?");
                        //если Да, запросить новое название заметки и перейти к label_1
                        if (sdr.isДа())
                        {
                            engine.get_OperatorConsole().SureConsoleCursorStart();
                            String str = engine.get_OperatorConsole().PrintQuestionAnswer(EnumSpeakDialogResult.Отмена, "Введите новое название заметки:", true, true);
                            if (Dialogs.этоОтмена(str))
                                return EnumProcedureResult.CancelledByUser;
                            //else
                            title = Utility.StringCopy(str);
                            continue; //goto label_1;
                        }
                        //если Отмена, то прервать выполнение процедуры
                        else if (sdr.isОтмена())
                            return EnumProcedureResult.CancelledByUser;
                        //если Нет, то прервать цикл запроса и открыть существующий файл для добавления
                        break;
                    }
                    //если файла нет, то прервать цикл запроса и создать новый файл для текста.
                    else break;
                }
                //тут мы окажемся, если файл уже существует или нужно создать новый

                //2 создать на рабочем столе текстовый файл в кодировке utf-8 
                BufferedWriter writer = Utility.FileWriterOpenOrCreate(fpath, "UTF-8");

                //если файл новый, то вывести в него строку названия заметки как заголовок
                //и добавить пустую строку как разделитель.
                if (ФайлУжеСуществует == false)
                {
                    writer.write(titleAsContent);
                    writer.newLine();
                    writer.write(Utility.DateTimeNowToString());
                    writer.newLine();
                    writer.newLine();
                }
                writer.close();

                //3 открыть этот файл в блокноте через командную строку.   
                //3.1 извлечь из настроек команду открытия терминала: сначала из ФайлНастроекОператора, потом из ТаблицаНастроекОператора.
                //3.2 если пути нет, вывести сообщение об ошибке  и вернуть WrongArguments, чтобы Оператор искал другую Процедуру для этого запроса.

                // тут лучше весь путь к файлу превратить в URI типа "file:///" стандартными средствами явы.
                String fpath2 = Utility.MakeUriFromFilePath(fpath);
                EnumProcedureResult epr = engine.StartShellExecute(fpath2);
                if (epr == EnumProcedureResult.Success)
                {
                    result = epr;
                    //вывести сообщение о результате операции: успешно
                    String msgT;
                    if (ФайлУжеСуществует)
                        msgT = "Существующая заметка \"%s\" открыта для изменения.";
                    else
                        msgT = "Новая заметка \"%s\" создана.";

                    engine.get_OperatorConsole().PrintTextLine(String.format(msgT, title), EnumDialogConsoleColor.Успех);
                }
                else
                {
                    result = epr;
                    engine.get_OperatorConsole().PrintTextLine(String.format("Невозможно открыть  созданную заметку \"%s\" ", title), EnumDialogConsoleColor.Предупреждение);
                }
            }
            catch (Exception ex)
            {
                engine.PrintExceptionMessageToConsoleAndLog("Ошибка в процедуре " + currentProcedureTitle + "()", ex);
                result = EnumProcedureResult.Error;
            }

            // вернуть флаг продолжения работы
            return result;
        }


        /**
         * NT-Обработчик процедуры Открыть НазваниеМеста.
         * 
         * 
         * @param engine
         *            Ссылка на объект Движка Оператор для доступа к консоли, логу, БД итп.
         * @param manager
         *            Ссылка на объект Менеджера Библиотеки Процедур для доступа к инициализированным ресурсам библиотеки.
         * @param query
         *            Текст исходного запроса пользователя для возможной дополнительной обработки.
         * @param args
         *            Массив аргументов Процедуры, соответствующий запросу.
         * @return Функция возвращает результат как одно из значений EnumProcedureResult:
         *         EnumProcedureResult.Success если Процедура выполнена успешно;
         *         EnumProcedureResult.WrongArguments если аргументы не подходят для запуска Процедуры;
         *         EnumProcedureResult.Error если произошла ошибка при выполнении Процедуры;
         *         EnumProcedureResult.CancelledByUser если выполнение Процедуры прервано Пользователем;
         *         EnumProcedureResult.Exit если после выполнения Процедуры требуется завершить работу Оператор;
         *         EnumProcedureResult.ExitAndLogoff если после выполнения Процедуры требуется завершить сеанс пользователя;
         *         EnumProcedureResult.ExitAndHybernate если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndSleep если после выполнения Процедуры требуется перевести компьютер в спящий режим;
         *         EnumProcedureResult.ExitAndReload если после выполнения Процедуры требуется перезагрузить компьютер;
         *         EnumProcedureResult.ExitAndShutdown если после выполнения Процедуры требуется выключить компьютер;
         */
        @OperatorProcedure(State = ImplementationState.Ready,   // TODO: заменить на актуальное
                Title = "Команда Открыть НазваниеМеста",
                Description = "Команда Открыть место через ShellExecute.")
    public static EnumProcedureResult CommandOpen(
            Engine engine,
            LibraryManagerBase manager,
            UserQuery query,
            ArgumentCollection args)
        {

            EnumProcedureResult result = EnumProcedureResult.Success;
            // указать здесь полный путь как название процедуры для вывода на экран.
            String currentProcedureTitle = "GeneralProcedures.UnsortedProcedures.CommandOpen";
            // выброшенное тут исключение будет заменено на Reflection исключение и его текст потеряется.
            // Поэтому надо здесь его перехватить, вывести в лог и на консоль, и погасить, вернув EnumProcedureResult.Error.
            try
            {
                String str = String.format("Начата процедура %s(\"%s\")", currentProcedureTitle, args.getByIndex(0).get_ArgumentValue());
                // вывести это тестовое сообщение о начале процедуры - в лог!
                engine.AddMessageToConsoleAndLog(str, EnumDialogConsoleColor.Сообщение, EnumLogMsgClass.SubsystemEvent_Procedure, EnumLogMsgState.OK);
                // получить путь из подставленного места
                String addr = args.getByIndex(0).get_ArgumentValue();

                // если это веб-ссылка, открыть ее в шелл
                if (OperatorEngine.Utility.isWebUri(addr))
                    engine.StartShellExecute(addr);
                // если это сетевой путь файла, открыть его в шелл
                else if (OperatorEngine.Utility.isFileUri(addr))
                    engine.StartShellExecute(addr);
                // если это локальный путь файла, превратить его в сетевой и открыть в шелл
                else if (OperatorEngine.Utility.isLocalFile(addr))
                {
                    addr = OperatorEngine.Utility.MakeUriFromFilePath(addr);
                    engine.StartShellExecute(addr);
                }
                // если это не вышеперечисленное, то исполнить как команду.
                //TODO: эта функция все еще не работает как требуется!  
                else engine.StartCommandTerminalExecute(addr);

                // вывести сообщение о результате операции: успешно
                engine.get_OperatorConsole().PrintTextLine("Команда успешно завершена.", EnumDialogConsoleColor.Успех);
            }
            catch (Exception ex)
            {
                engine.PrintExceptionMessageToConsoleAndLog("Ошибка в процедуре " + currentProcedureTitle + "()", ex);
                result = EnumProcedureResult.Error;
            }

            // вернуть флаг продолжения работы
            return result;
        }


        //*** End of file ***
    }
