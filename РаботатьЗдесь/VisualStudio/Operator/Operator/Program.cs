using System;
using System.IO;
using System.Reflection;
using Engine.Utility;

namespace Operator
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Сейчас тут уже построен каркас Оператор для запуска движка.
            // Тесты вставлять только в виде вызовов функций тестирования!

            Engine.OperatorEngine.Engine engine = null;
            try
            {

                // 1. Первым делом, попытаться определить, запущен ли уже Оператор,
                // и если да, завершить работу и передать фокус ввода более старой копии
                //TODO: фокус ввода передать не могу - способ есть, но делать лень - надо совместить оба способа SingleInstance.
                bool toExit = checkPreviousInstance();
                if (toExit == false)
                {
                    // 2. TODO: приложение не умеет перехватывать свое завершение, поэтому не обрабатывает закрытие окна.
                    //Так что в лог не выводится запись, завершающая сеанс работы.
                    //БД поэтому реализована так, что постоянно закрыта.
                    Engine.Utility.WindowUtility.DisableConsoleCloseButton();
                    //это выключает кнопку Close на окне, и теперь приложение нельзя закрыть через системное меню.
                    //И по alt+F4 консольные приложения не закрываются никакие.
                    //Закрываются по Ctrl+C или Ctrl+Break

                    //Заблокировать Ctrl+C оставить только Ctrl+Break
                    //Console.TreatControlCAsInput = true; - я пробовал включить это, но консоль команды странно обрабатывает - виснет на них.
                    Console.TreatControlCAsInput = false;
                    Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

                    //можно добавить поддержку ввода клавиш команд Да Нет Отмена,
                    //есть идеи в ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/fxref_mscorlib/html/bcf70b80-2b4b-d7d2-05ed-15ff0ceab52f.htm
                    //Для этого надо сначала запрашивать первый символ через Console.readKey(true), 
                    //а потом, если это не командная клавиша, выводить ее на экран и собирать остальной ввод как обычную строку.
                    //Esc = Отмена; + = Да; - = Нет;
                    //Решено: не делать подобные фичи - использовать только обычный текст!

                    // 3. create engine object
                    engine = new Engine.OperatorEngine.Engine();
                    // 4. init engine object
                    engine.Open();

                    // 5. запускаем цикл приема запросов, а когда он завершится, закрываем все приложение.
                    engine.CommandLoop();

                    // TODO: разобраться с исключениями в engine.Close()
                    engine.Close();
                    engine = null;
                }
            }
            catch (Exception e)
            {
                // если лог работоспособен, то вывести сообщение в него
                // if(Engine.isLogReady(engine))
                Engine.OperatorEngine.Engine.LoggingException(engine, e);

                // print exception
                PrintExceptionWithoutEngine(e);
            }
            finally
            {
                // close engine object
                // записываем в лог сообщение о завершении работы и завершаем работу
                // приложения.
                // TODO: разобраться с исключениями в engine.Close()
                // if(engine != null)
                // engine.Close();
                // engine = null;

                //завершить детектор запущенных копий приложения в самом конце работы приложения.
                finalPreviousInstance();
            }

            return;
        }

        /// <summary>
        /// Handles the CancelKeyPress event of the Console control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ConsoleCancelEventArgs"/> instance containing the event data.</param>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            //TODO: переделать код так, чтобы Оператор завершался при нажатии только Ctrl+Break, и завершался корректно!
            
            if (e.SpecialKey == ConsoleSpecialKey.ControlC)
                e.Cancel = true;
            else if (e.SpecialKey == ConsoleSpecialKey.ControlBreak)
            {
                e.Cancel = false;
                ////
                //m_engine.Exit();
                //logWriter.WriteLine("Close by Ctrl+Break {0}", DateTime.Now.ToString());
                ////записываем в лог сообщение о завершении работы и завершаем работу приложения.
                //logWriter.WriteLine("ENDSESSION {0}", DateTime.Now.ToString());
            }

            return;
        }

        #region *** Operator version ***
        /// <summary>
        /// NT-Получить строку версии сборки Оператора
        /// </summary>
        /// <remarks>Возвращает версию сборки, в которой находится данная функция.</remarks>
        /// <returns></returns>
        internal static string getOperatorVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// NT-Получить версию сборки Оператора
        /// </summary>
        /// <returns></returns>
        internal static Version getOperatorVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }
        #endregion





/// <summary>
/// NT-Освободить ресурсы детектора запущенных копий приложения.
/// </summary>
        private static void finalPreviousInstance()
        {
            try
            {
                SingleAppInstance.unlockInstance();
            }
            catch (Exception e)
            {
                PrintExceptionWithoutEngine(e);
            }

            return;
        }

        /// <summary>
        /// NT-Запустить детектор запущенных копий приложения.
        /// </summary>
        /// <returns>Функция возвращает true, если предыдущая копия уже запущена и текущую копию надо завершить, false в противном случае.</returns>
        private static bool checkPreviousInstance()
        {
            bool toExit = false;
            try
            {
                //create locking file path and start locking
                String lockfilepath = Path.Combine(  Engine.OperatorEngine.FileSystemManager.getAppFolderPath(), SingleAppInstance.LockingFileName);
                SingleAppInstance.lockInstance(lockfilepath);
                //check flags
                if (SingleAppInstance.hasDuplicate() == true)
                {
                    toExit = true;
                    AnotherCopyOnWork();
                }
                else if (SingleAppInstance.needRestoreData() == true)
                {
                    toExit = false;
                    needRestoreData();
                }
            }
            catch (Exception e)
            {
                PrintExceptionWithoutEngine(e);

                //to exit because errors
                finalPreviousInstance();
                toExit = true;
            }

            return toExit;
        }


        
/// <summary>
/// NT-Обработчик состояния "Возможно, требуется восстановление данных после аварийного завершения Оператор".
/// </summary>
        private static void needRestoreData()
        {
            String msg = "Возможно, требуется восстановление данных после аварийного завершения предыдущего экземпляра Оператор.";
            PrintMessageWithoutEngine(msg);

            // TODO Тут запустить проверку состояния и восстановление данных Оператора

            return;
        }

       
/// <summary>
/// NT-обработчик состояния "Другая копия Оператор уже запущена".
/// </summary>
        private static void AnotherCopyOnWork()
        {
            String msg = "Другая копия Оператор уже запущена. Эта копия будет закрыта.";
            PrintMessageWithoutEngine(msg);

            // TODO тут найти другую копию и передать ей фокус ввода.  

            return;
        }

        /// <summary>
        /// NT-Вывести сообщение об исключении без использования Движка Оператор. 
        /// </summary>
        /// <param name="e">Объект исключения.</param>
        private static void PrintExceptionWithoutEngine(Exception e)
        {
            // print exception
            Console.WriteLine();
            Console.WriteLine(e.GetType().ToString());
            Console.WriteLine(e.ToString());
            Console.WriteLine();
        }

        /// <summary>
        /// NT-Вывести сообщение без использования Движка Оператор.
        /// </summary>
        /// <param name="msg">Текст сообщения.</param>
        private static void PrintMessageWithoutEngine(String msg)
        {
            // print message
            Console.WriteLine();
            Console.WriteLine(msg);
            Console.WriteLine();
        }
    }
}
