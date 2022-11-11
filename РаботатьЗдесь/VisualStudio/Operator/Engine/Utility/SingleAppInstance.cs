using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Engine.Utility
{

    /// <summary>
    /// NT-Определить, работает ли уже другой экзмпляр приложения. И если нет, была ли его работа завершена некорректно.
    /// </summary>
    /// <remarks>
    ///  Механизм основан на создании пустого файла блокировки, обычно в рабочем каталоге приложения/проекта.
    /// - Если файл блокировки отсутствует, то нет ранее запущенного экземпляра приложения, и его работа была завершена корректно.
    /// - Если файл блокировки присутствует, то:
    ///   - Если файл блокирован от чтения-записи, то существует ранее запущенный экземпляр приложения. 
    ///      И он должен был восстановить данные, если они были повреждены.
    ///   - Если файл не блокирован от чтения-записи, то нет ранее запущенного экземпляра приложения. 
    ///      Работа предыдущего экземпляра приложения была завершена неожиданно.
    /// Верояно, потребуется восстановление данных приложения.
    /// </remarks>
    internal class SingleAppInstance
    {
        #region *** Constants and Fields ***
        /**
         * Имя файла блокировки.
         */
        public const String LockingFileName = "lockfile.lock";

        /**
         * Флаг, что предыдущая копия приложения уже запущена.
         */
        private static bool m_hasDuplicate = false;

        /**
         * Флаг, что предыдущая копия не была завершена корректно, и требуется восстановить данные приложения.
         */
        private static bool m_needRestoreData = false;

        /**
         * Путь к файлу блокировки.
         */
        private static String m_lockFilePathName = null;

        /**
         * Объект файла
         */
        private static RandomAccessFile m_raf = null;

        /**
         * Объект блокировки файла.
         */
        private static FileLock m_fl = null;
        #endregion

        #region *** Properties ***


        /// <summary>
        /// NT-Флаг, что предыдущая копия приложения уже запущена.
        /// </summary>
        /// <returns>Возвращает значение флага.</returns>
        public static bool hasDuplicate()
        {
            return m_hasDuplicate;
        }

        /// <summary>
        /// NT-Флаг, что нет запущенных копий, но предыдущая копия не была завершена корректно, и требуется восстановить данные приложения.
        /// </summary>
        /// <returns>Возвращает значение флага.</returns>
        public static bool needRestoreData()
        {
            return m_needRestoreData;
        }

        // *** Service functions ***
        /**
         * NR-Try lock application before start application routine.
         * 
         * @param lockfilepath Locking file pathname.
         * @throws IOException Error on locking application.
         * @throws InterruptedException Unknown error.
         */
        public static void lockInstance(String lockfilepath)
        {
            bool isExists = false;
            bool isLocked = false;
            //wait random time 50..3000ms
            waitRandom(3000);

                    FileInfo fi = new FileInfo(lockfilepath);
                    //store filepath to variable
                    m_lockFilePathName = String.Copy(lockfilepath);
                    // set flags
                    isExists = true;
                    if (fi.Exists == false)
                   {
                        isExists = false;
                        fi.createNewFile();
                    }
            //    // set lock
            //    m_raf = new RandomAccessFile(fi, "rw");
            //    m_fl = m_raf.getChannel().tryLock();
            //        if (m_fl == null)
            //        {
            //            isLocked = true;
            //        }
            //        else
            //{
            //    isLocked = false;
            //}
            //// calculate
            //if (isExists == false)
            //{
            //    m_hasDuplicate = false;
            //    m_needRestoreData = false;
            //}
            //else
            //{
            //    // flag isExists = true
            //    if (isLocked == false)
            //    {
            //        m_hasDuplicate = false;
            //        m_needRestoreData = true;
            //    }
            //    else
            //    {
            //        m_hasDuplicate = true;
            //        m_needRestoreData = false;
            //    }

            //}

            return;
        }


        /// <summary>
        /// NT - Current thread sleep for 50..maxMs milliseconds.
        /// </summary>
        /// <param name="maxMs">Maximum amount of milliseconds for sleep.</param>
        private static void waitRandom(int maxMs)
        {
            Random r = new Random(Environment.TickCount);
            int ms = r.Next(50, maxMs);

            Thread.Sleep(ms);

            return;
        }

        /**
         * NT-Unlock application at exit application routine.
         * 
         * @throws IOException
         *             Error on unlocking
         */
        public static void unlockInstance() 
        {
        if (m_fl != null)
        {
        m_fl.release();
        m_fl = null;
    }
        
        if (m_raf != null)
        {
        m_raf.close();
        m_raf = null;
    }
//delete lock file at exit application
File f = new File(m_lockFilePathName);
if (f.exists())
    f.delete();

m_lockFilePathName = null;

return;
    }
#endregion


    }
}
