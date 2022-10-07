using System;
using System.IO;
using Engine.DbSubsystem;
using Engine.SettingSubsystem;
using Engine.Utility;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - Всевозможные операции с файлами и каталогами
    /// </summary>
    internal static class FileSystemManager
    {

        /// <summary>
        /// User home directory
        /// </summary>
        public static String UserFolderPath = SystemInfoManager.GetUserHomeFolderPath();
            
        /// <summary>
        /// NT-Создать каталог Оператора с правильной структурой файлов и папок
        /// </summary>
        /// <exception cref="Exception">Cannot create some directories.</exception>
        public static void CreateOperatorFolder()
        {
            // TODO: Добавить код создания папок и файлов и копировать сюда все
            // нужные файлы.
            // Этот код для общего понимания устройства структуры каталогов
            // Оператора.
            // 1. Create main folder
            String appFolderPath = FileSystemManager.getAppFolderPath();
            bool result = false;
            result = FileSystemManager.CreateDirectory(appFolderPath);
            if (result == false)
                throw new Exception("Error: cannot create application folder " + appFolderPath);
            // 2. Create log folder
            String logFolderPath = FileSystemManager.getAppLogFolderPath();
            result = FileSystemManager.CreateDirectory(logFolderPath);
            if (result == false)
                throw new Exception("Error: cannot create log folder " + logFolderPath);
            // 3. Create assemblies folder
            String asmFolderPath = FileSystemManager.getAssembliesFolderPath();
            result = FileSystemManager.CreateDirectory(asmFolderPath);
            if (result == false)
                throw new Exception("Error: cannot create assemblies folder " + asmFolderPath);
            // 4. Create procedure apps folder
            String procFolderPath = FileSystemManager.getProcedureAppsFolderPath();
            result = FileSystemManager.CreateDirectory(procFolderPath);
            if (result == false)
                throw new Exception("Error: cannot create procedure application folder " + procFolderPath);
            // 5. TODO: Create other folders here

            return;
        }

        /// <summary>
        /// NT- Get application folder path string.
        /// </summary>
        /// <returns>Return application folder path string.</returns>
        public static String getAppFolderPath()
        {
            return Path.Combine(SystemInfoManager.GetUserHomeFolderPath(), "." + Engine.ApplicationTitle);
        }

        /// <summary>
        /// NT-Check application folder exists
        /// </summary>
        /// <returns>Returns True if folder exists, returns False otherwise.</returns>
        public static bool isAppFolderExists()
        {
            string afp = getAppFolderPath();

            return Directory.Exists(afp);
        }

        /// <summary>
        /// NT- Get application settings file pathname string.
        /// </summary>
        /// <returns>Return application settings file pathname string.</returns>
        public static String getAppSettingsFilePath()
        {
            return Path.Combine(getAppFolderPath(), ApplicationSettingsBase.AppSettingsFileName);
        }

        /// <summary>
        /// NT-Check app settings file existing
        /// </summary>
        /// <returns>Returns True if file exists, returns False otherwise.</returns>
        public static bool isAppSettingsFileExists()
        {
            string asf = FileSystemManager.getAppSettingsFilePath();

            return File.Exists(asf);
        }

        /// <summary>
        /// NT-Get path of user directory Documents.
        /// </summary>
        /// <returns>Function returns User directory Documents path string.</returns>
        public static String getUserDocumentsFolderPath()
        {
            //тут надо автоматически получать папку документов. Она для русской версии ОС называется Документы, для английской - Documents.
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        /// <summary>
        /// NT-Get path of user directory Desktop.
        /// </summary>
        /// <returns>Function returns User directory Desktop path string.</returns>
        public static String getUserDesktopFolderPath()
        {
            //тут надо автоматически получать папку рабочего стола. Она для русской версии ОС называется Рабочий стол, для английской - Desktop.
            return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        /// <summary>
        /// NT-Get application log folder path string.
        /// </summary>
        /// <returns>Return application log folder path string.</returns>
        public static String getAppLogFolderPath()
        {
            return Path.Combine(getAppFolderPath(), "logs");
        }

        /// <summary>
        /// NT-Get application database file pathname string.
        /// </summary>
        /// <returns>Return application database file pathname string.</returns>
        public static String getAppDbFilePath()
        {
            return Path.Combine(getAppFolderPath(), OperatorDbAdapter.AppDbFileName);
        }

        /// <summary>
        /// NT-Check app database file existing
        /// </summary>
        /// <returns>Returns True if file exists, returns False otherwise.</returns>
        public static bool isAppDbFileExists()
        {
            string dbf = getAppDbFilePath();

            return File.Exists(dbf);
        }

        /// <summary>
        /// NT-Get procedure application folder path string.
        /// Возвращает путь к папке, которая должна содержать подпапки программ, используемых в Процедурах, определенных пользователем.
        /// </summary>
        /// <returns>Return procedure application folder path string.</returns>
        public static String getProcedureAppsFolderPath()
        {
            return Path.Combine(getAppFolderPath(), "apps");
        }

        /// <summary>
        /// NT-Получить путь к папке, которая должна содержать подпапки Сборок Процедур.
        /// </summary>
        /// <returns>Return assemblies folder path string.</returns>
        public static String getAssembliesFolderPath()
        {
            return Path.Combine(getAppFolderPath(), "proc");
        }

        /// <summary>
        /// NT- Создать новые каталоги в указанном пути - сразу всю цепочку каталогов.
        /// </summary>
        /// <param name="path">Path to new directory</param>
        /// <returns>Returns True if success, false if errors</returns>
        public static bool CreateDirectory(String path)
        {
            DirectoryInfo di = Directory.CreateDirectory(path);

            return di.Exists;
        }

        ///**
        // * RT-Remove directory with subdirectories recursively
        // * 
        // * @param dir
        // *            Directory to remove
        // * @return Returns True if success, false if errors
        // */
        //public static boolean RemoveDirectory(File dir)
        //{
        //    if (dir.isDirectory())
        //    {
        //        File[] files = dir.listFiles();
        //        if (files != null && files.length > 0)
        //        {
        //            for (File aFile : files)
        //                {
        //    RemoveDirectory(aFile);
        //}
        //            }
        //        }

        //        return dir.delete();
        //    }

        //    /**
        //     * NT-Clean specified directory
        //     * 
        //     * @param dir
        //     *            Directory to clean
        //     */
        //    public static void CleanDirectory(File dir)
        //{
        //    if (dir.isDirectory())
        //    {
        //        File[] files = dir.listFiles();
        //        if (files != null && files.length > 0)
        //        {
        //            for (File aFile : files)
        //                {
        //    RemoveDirectory(aFile);
        //}
        //            }
        //        }

        //        return;
        //    }

        ///**
        //    * NR-Get current user temporary folder path string
        //    * @return Function returns current user temporary filder path string.
        //    * @throws Exception Функция не реализована.
        //    */
        //public static String getTempFolderPath() throws Exception
        //{
        //    // TODO: add path to User temp folder
        //      throw new Exception();
        //}

        ///**
        // * NT-Получить список дисковых томов данного компьютера
        // * 
        // * @return Возвращает массив объектов дисковых томов компьютера или null при
        // *         неудаче.
        // */
        //public static DriveInfo[] GetDrives()
        //{
        //        return DriveInfo.GetDrives();
        //}

        ///**
        // * NT-Получить общий объем дискового тома.
        // * 
        // * @param volume
        // *            Дисковый том.
        // * @return Возвращает объем дискового тома.
        // */
        //public static long GetVolumeSpace(File volume)
        //{
        //    return volume.getTotalSpace();
        //}

        ///**
        // * NT-Получить объем свободного места на дисковом томе.
        // * 
        // * @param volume
        // *            Дисковый том.
        // * @return Возвращает объем свободного места на дисковом томе.
        // */
        //public static long GetVolumeFreeSpace(File volume)
        //{
        //    return volume.getFreeSpace();
        //}



        //    /**
        //     * NT- Get from top directory all files with specified extensions.
        //     * 
        //     * @param dir
        //     *            Directory
        //     * @param exts
        //     *            Array of filename endings or extensions: .htm
        //     * @return Returns array of File objects
        //     */
        //    public static File[] getDirectoryFiles(File dir, String[] exts)
        //{
        //    FilenameFilter filter = new FilenameFilter()
        //    {

        //            public boolean accept(File file, String name)
        //    {
        //        // check filename ends
        //        for (String ext : exts)
        //                    if (name.endsWith(ext))
        //    return true;
        //return false;
        //            }
        //        };
        //File[] files = dir.listFiles(filter);

        //return files;
        //    }

        //    /**
        //     * NT-Get directory files
        //     * 
        //     * @param dir
        //     *            Directory object.
        //     * @param exts
        //     *            Array of file endings (extension).
        //     * @param recursive
        //     *            If True, read all subdirectories recursive. Else read specified directory only.
        //     * @return Function returns array of founded files.
        //     */
        //    public static File[] getDirectoryFiles(
        //            File dir,
        //            String[] exts,
        //            boolean recursive)
        //{
        //    LinkedList<File> result = new LinkedList<File>();
        //    getDirectoryFilesRecurse(result, dir, exts, recursive);

        //    return result.toArray(new File[result.size()]);
        //}

        ///**
        // * NT - Get directory files recursively.
        // * 
        // * @param result
        // *            Result list of founded File objects
        // * @param dir
        // *            Directory object.
        // * @param exts
        // *            Array of file endings (extension).
        // * @param recursive
        // *            If True, read all subdirectories recursive. Else read specified directory only.
        // */
        //private static void getDirectoryFilesRecurse(
        //        LinkedList<File> result,
        //        File dir,
        //        String[] exts,
        //        boolean recursive)
        //{
        //    File[] files = dir.listFiles();
        //    if (files != null && files.length > 0)
        //    {
        //        for (File f : files)
        //            {
        //    if (f.isDirectory() && (recursive == true))
        //        getDirectoryFilesRecurse(result, f, exts, recursive);
        //    else if (f.isFile() && (checkFileExt(f, exts) == true))
        //        result.add(f);
        //}
        //        }

        //        return;
        //    }

        //    /**
        //     * NT-Check file extension matched array contents.
        //     * 
        //     * @param f
        //     *            File or Directory object.
        //     * @param exts
        //     *            Array of name ending string's.
        //     * @return Function returns True if file title matched with any of specified title ending's.
        //     *         Function returns False otherwise.
        //     */
        //    private static boolean checkFileExt(File f, String[] exts)
        //{
        //    // check filename ends
        //    String name = f.getAbsolutePath();
        //    for (String ext : exts)
        //            if (name.endsWith(ext))
        //    return true;

        //return false;
        //    }


    }
}
