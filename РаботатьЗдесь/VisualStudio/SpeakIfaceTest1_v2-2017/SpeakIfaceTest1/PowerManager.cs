using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Operator
{
    /// <summary>
    /// Содержит функции управления питанием компьютера
    /// </summary>
    public class PowerManager
    {

        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        /// <summary>
        /// Выполнить операцию согласно коду операции
        /// </summary>
        /// <param name="exitcode"></param>
        public static void ProcessExitCode(ProcedureResult exitcode)
        {
            switch (exitcode)
            {
                case ProcedureResult.ExitAndShutdown:
                    DoShutdown();
                    break;
                case ProcedureResult.ExitAndReload:
                    DoReload();
                    break;
                case ProcedureResult.ExitAndLogoff:
                    DoLogoff();
                    break;
                case ProcedureResult.ExitAndSleep:
                    DoSleep();
                    break;
                case ProcedureResult.ExitAndHybernate:
                    DoHybernate();
                    break;
                default:
                    break;
            }
            return;
        }
        /// <summary>
        /// Гибернация машины
        /// </summary>
        public static void DoHybernate()
        {
            // Hibernate
            SetSuspendState(true, true, true);
            //System.Windows.Forms.Application.SetSuspendState(PowerState.Hibernate, true, true);
        }
        /// <summary>
        /// Спящий режим машины
        /// </summary>
        public static void DoSleep()
        {
            // Standby
            SetSuspendState(false, true, true);
            //System.Windows.Forms.Application.SetSuspendState(PowerState.Suspend, true, true);

        }
        /// <summary>
        /// NT-Завершение сеанса текущего поьзователя
        /// </summary>
        public static void DoLogoff()
        {
            // if OS is Windows7,  command args will be " -l"
            //if OS is WindowsXP, command args will be " -l -t 00"
            String args = " -l";
            if(SystemInfoManager.isWindowsXP())
                args = " -l -t 00";
            Process.Start("Shutdown.exe", args);

            return;
        }
        /// <summary>
        /// Перезагрузка машины
        /// </summary>
        public static void DoReload()
        {
            Process.Start("Shutdown.exe", "-r -t 00");

        }
        /// <summary>
        /// Выключение машины
        /// </summary>
        public static void DoShutdown()
        {
            Process.Start("Shutdown.exe", "-s -t 00");

        }

        /// <summary>
        /// NT-Запустить приложение 
        /// </summary>
        /// <param name="cmdline">Командная строка с аргументами приложения</param>
        public static void ExecuteApplication(String cmdline)
        {
            String[] sar = RegexManager.ParseCommandLine(cmdline);
            //Process.Start(sar[0], sar[1]);
            ExecuteApplication(sar[0], sar[1]);
            return;
        }

        /// <summary>
        /// NT-Запустить приложение - улучшенная функция
        /// </summary>
        /// <param name="app">Путь исполняемого файла</param>
        /// <param name="args">Аргументы или пустая строка</param>
        /// <remarks>
        /// Приложение запускается с нормальным окном и с рабочим каталогом=Мои документы.
        /// </remarks>
        public static void ExecuteApplication(string app, string args)
        {
            ProcessStartInfo psi = new ProcessStartInfo(app, args);
            //psi.StandardOutputEncoding = ?
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Process.Start(psi);
            
            return;
        }
    }
}
