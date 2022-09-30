using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Engine.Utility
{
    internal class WindowUtility
    {
        #region Console titlebar CloseButton disable
        //07072019 добавлено в Operator.CWindowProcessor 

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, int bRevert);

        [DllImport("user32.dll")]
        private static extern Boolean DeleteMenu(IntPtr hMenu, int uPosition, int uFlags);

        /// <summary>
        /// Выключить кнопку Close на титлебаре окна консольного приложения
        /// </summary>
        public static void DisableConsoleCloseButton()
        {
            //1 получить описатель окна приложения
            Process p = Process.GetCurrentProcess();
            IntPtr hwnd = p.MainWindowHandle;
            //2 получить служебное меню окна
            IntPtr sysMenu = GetSystemMenu(hwnd, 0);
            //3 выключить пункт Закрыть
            DeleteMenu(sysMenu, 6, 1024);

            return;
        }
        #endregion

        #region switch to previous Operator application
        //Это только для консольного приложения годится.
        //Это очень криво и наспех сделано, но работает пока, под аккаунтом администратора
        //отладить не получится, так как приложение под отладчиком запускается в оболочке vshost
        //и это портит всю схему отладки - приложение не находит свою копию.
        //Но проверить можно, запуская обе копии без отладчика, просто из папки.
        //старое приложение разворачивается, а новое - исчезает в момент запуска.


        [DllImport("user32.dll")]
        private static extern Boolean SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, int nCmdShow);
        //#define SW_HIDE             0
        //#define SW_SHOWNORMAL       1
        //#define SW_NORMAL           1
        //#define SW_SHOWMINIMIZED    2
        //#define SW_SHOWMAXIMIZED    3
        //#define SW_MAXIMIZE         3
        //#define SW_SHOWNOACTIVATE   4
        //#define SW_SHOW             5
        //#define SW_MINIMIZE         6
        //#define SW_SHOWMINNOACTIVE  7
        //#define SW_SHOWNA           8
        //#define SW_RESTORE          9
        //#define SW_SHOWDEFAULT      10
        //#define SW_FORCEMINIMIZE    11
        //#define SW_MAX              11

        /// <summary>
        /// NT-выявить что приложение - дубликат и активировать более раннее приложение
        /// </summary>
        /// <returns>Возвращает True, если текущее приложение надо закрыть, False в противном случае.</returns>
        internal static bool DoubleApplication()
        {
            //получить текущий процесс
            Process curProcess = Process.GetCurrentProcess();
            //string appPath = curProcess.MainModule.FileName;
            string title = curProcess.MainWindowTitle;
            //искать дубликат процесса
            //Process[] dublicates = Process.GetProcessesByName(curProcess.ProcessName, curProcess.MachineName);
            Process oldProcess = null;
            foreach (Process p in Process.GetProcesses())
            {
                //если нашли дубликат текущего процесса
                if (!isProcessCanAccessed(p))
                    continue;
                if ((!p.HasExited) && (p.MainWindowTitle == title))
                {
                    if (p.Id != curProcess.Id)
                    {
                        oldProcess = p;
                        break;
                    }
                }
            }
            //если нашли дубликат процесса
            //передаем ему фокус ввода и возвращаем флаг завершения работы приложения.
            //TODO: если приложение свернуто, оно не разворачивается. Тут еще надо работать.
            if (oldProcess != null)
            {
                IntPtr hWnd = oldProcess.MainWindowHandle;
                if (hWnd != null)
                {
                    ShowWindow(hWnd, 1);//разворачивает на передний план, но если уже развернуто, не выводит на передний план.
                    SetForegroundWindow(hWnd);//выводит на передний план если развернуто, но не разворачивает
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// RT-некоторые процессы нельзя прочитать - выбрасывается исключение Недостаточно прав доступа
        /// Перехватываем такие процессы и гасим исключения.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool isProcessCanAccessed(Process p)
        {
            bool result = false;

            try
            {
                result = !p.HasExited;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        #endregion

        /// <summary>
        /// NT-Получить строку версии сборки Оператора
        /// </summary>
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

    }
}
