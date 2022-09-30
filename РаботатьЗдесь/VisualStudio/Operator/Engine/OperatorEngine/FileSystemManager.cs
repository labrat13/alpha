using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - Всевозможные операции с файлами и каталогами
    /// </summary>
    internal static class FileSystemManager
    {
        //TODO: port this class from Java
        
        // Вынесен сюда, поскольку в JDK путаница с этими сепараторами.
        /**
         * File path separator as string
         */
        public final static String FileSeparator = File.separator;

        /**
         * User home directory
         */
        public final static String UserFolderPath = SystemInfoManager.GetUserHomeFolderPath();

        /**
         * NT-Создать каталог Оператора с правильной структурой файлов и папок
         * 
         * @throws Exception
         *             Cannot create some directories.
         */
        public static void CreateOperatorFolder() throws Exception
        {
            // TODO: Добавить код создания папок и файлов и копировать сюда все
            // нужные файлы.
            // Этот код для общего понимания устройства структуры каталогов
            // Оператора.
            // 1. Create main folder
            String appFolderPath = FileSystemManager.getAppFolderPath();
        boolean result = false;
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

    /**
     * NT- Get application folder path string.
     * 
     * @return Return application folder path string.
     */
    public static String getAppFolderPath()
    {
        return SystemInfoManager.GetUserHomeFolderPath() + FileSeparator + "." + Engine.ApplicationTitle;
    }

    /**
     * NT-Check application folder exists
     * 
     * @return Returns True if folder exists, returns False otherwise.
     */
    public static boolean isAppFolderExists()
    {
        return isFileExists(getAppFolderPath());
    }

    /**
     * NT- Get application settings file pathname string.
     * 
     * @return Return application settings file pathname string.
     */
    public static String getAppSettingsFilePath()
    {
        return getAppFolderPath() + FileSeparator + ApplicationSettingsBase.AppSettingsFileName;
    }

    /**
     * NT-Check app settings file existing
     * 
     * @return Returns True if file exists, returns False otherwise.
     */
    public static boolean isAppSettingsFileExists()
    {
        return FileSystemManager.isFileExists(FileSystemManager.getAppSettingsFilePath());
    }

    /**
     * NT-Get path of user directory Documents. 
     * @return Function returns User directory Documents path string.
     */
    public static String getUserDocumentsFolderPath()
    {
        //TODO: тут надо автоматически получать папку документов. Она для русской версии ОС называется Документы, для английской - Documents.
        return SystemInfoManager.GetUserHomeFolderPath() + FileSeparator + "Documents" + FileSeparator;
    }

    /**
     * NT-Get path of user directory Desktop. 
     * @return Function returns User directory Desktop path string.
     */
    public static String getUserDesktopFolderPath()
    {
        //TODO: тут надо автоматически получать папку рабочего стола. Она для русской версии ОС называется Рабочий стол, для английской - Desktop.
        return SystemInfoManager.GetUserHomeFolderPath() + FileSeparator + "Desktop" + FileSeparator;
    }

    /**
     * NT-Check file exists
     * 
     * @param filepath
     *            File pathname string
     * @return Returns True if file exists, returns False otherwise.
     */
    public static boolean isFileExists(String filepath)
    {
        File f = new File(filepath);

        return f.exists();
    }

    /**
     * NT-Get application log folder path string.
     * 
     * @return Return application log folder path string.
     */
    public static String getAppLogFolderPath()
    {
        return getAppFolderPath() + File.separator + "logs";
    }

    /**
     * NT-Get application database file pathname string.
     * 
     * @return Return application database file pathname string.
     */
    public static String getAppDbFilePath()
    {
        return getAppFolderPath() + File.separator + OperatorDbAdapter.AppDbFileName;
    }

    /**
     * NT-Check app database file existing
     * 
     * @return Returns True if file exists, returns False otherwise.
     */
    public static boolean isAppDbFileExists()
    {
        return isFileExists(getAppDbFilePath());
    }

    /**
     * NT-Get procedure application folder path string.
     * Возвращает путь к папке, которая должна содержать подпапки программ, используемых в Процедурах, определенных пользователем.
     * 
     * @return Return procedure application folder path string.
     */
    public static String getProcedureAppsFolderPath()
    {
        return getAppFolderPath() + File.separator + "apps";
    }

    /**
     * NT-Получить путь к папке, которая должна содержать подпапки Сборок Процедур.
     * 
     * @return Return assemblies folder path string.
     */
    public static String getAssembliesFolderPath()
    {
        return getAppFolderPath() + File.separator + "proc";
    }

    /**
     * NR-Get current user temporary folder path string
     * @return Function returns current user temporary filder path string.
     * @throws Exception Функция не реализована.
     */
    public static String getTempFolderPath() throws Exception
    {
        // TODO: add path to User temp folder
        
        throw new Exception();
}

/**
 * NT-Получить список дисковых томов данного компьютера
 * 
 * @return Возвращает массив объектов дисковых томов компьютера или null при
 *         неудаче.
 */
public static File[] GetDrives()
{
    File[] drives = File.listRoots();

    return drives;
}

/**
 * NT-Получить общий объем дискового тома.
 * 
 * @param volume
 *            Дисковый том.
 * @return Возвращает объем дискового тома.
 */
public static long GetVolumeSpace(File volume)
{
    return volume.getTotalSpace();
}

/**
 * NT-Получить объем свободного места на дисковом томе.
 * 
 * @param volume
 *            Дисковый том.
 * @return Возвращает объем свободного места на дисковом томе.
 */
public static long GetVolumeFreeSpace(File volume)
{
    return volume.getFreeSpace();
}

/**
 * NT- Создать новые каталоги в указанном пути - сразу всю цепочку
 * каталогов.
 * 
 * @param path
 *            Path to new directory
 * @return Returns True if success, false if errors
 */
public static boolean CreateDirectory(String path)
{
    File newDir = new File(path);

    return newDir.mkdirs();
}

/**
 * RT-Remove directory with subdirectories recursively
 * 
 * @param dir
 *            Directory to remove
 * @return Returns True if success, false if errors
 */
public static boolean RemoveDirectory(File dir)
{
    if (dir.isDirectory())
    {
        File[] files = dir.listFiles();
        if (files != null && files.length > 0)
        {
            for (File aFile : files)
                {
    RemoveDirectory(aFile);
}
            }
        }

        return dir.delete();
    }

    /**
     * NT-Clean specified directory
     * 
     * @param dir
     *            Directory to clean
     */
    public static void CleanDirectory(File dir)
{
    if (dir.isDirectory())
    {
        File[] files = dir.listFiles();
        if (files != null && files.length > 0)
        {
            for (File aFile : files)
                {
    RemoveDirectory(aFile);
}
            }
        }

        return;
    }

    /**
     * NT- Get from top directory all files with specified extensions.
     * 
     * @param dir
     *            Directory
     * @param exts
     *            Array of filename endings or extensions: .htm
     * @return Returns array of File objects
     */
    public static File[] getDirectoryFiles(File dir, String[] exts)
{
    FilenameFilter filter = new FilenameFilter()
    {

            public boolean accept(File file, String name)
    {
        // check filename ends
        for (String ext : exts)
                    if (name.endsWith(ext))
    return true;
return false;
            }
        };
File[] files = dir.listFiles(filter);

return files;
    }

    /**
     * NT-Get directory files
     * 
     * @param dir
     *            Directory object.
     * @param exts
     *            Array of file endings (extension).
     * @param recursive
     *            If True, read all subdirectories recursive. Else read specified directory only.
     * @return Function returns array of founded files.
     */
    public static File[] getDirectoryFiles(
            File dir,
            String[] exts,
            boolean recursive)
{
    LinkedList<File> result = new LinkedList<File>();
    getDirectoryFilesRecurse(result, dir, exts, recursive);

    return result.toArray(new File[result.size()]);
}

/**
 * NT - Get directory files recursively.
 * 
 * @param result
 *            Result list of founded File objects
 * @param dir
 *            Directory object.
 * @param exts
 *            Array of file endings (extension).
 * @param recursive
 *            If True, read all subdirectories recursive. Else read specified directory only.
 */
private static void getDirectoryFilesRecurse(
        LinkedList<File> result,
        File dir,
        String[] exts,
        boolean recursive)
{
    File[] files = dir.listFiles();
    if (files != null && files.length > 0)
    {
        for (File f : files)
            {
    if (f.isDirectory() && (recursive == true))
        getDirectoryFilesRecurse(result, f, exts, recursive);
    else if (f.isFile() && (checkFileExt(f, exts) == true))
        result.add(f);
}
        }

        return;
    }

    /**
     * NT-Check file extension matched array contents.
     * 
     * @param f
     *            File or Directory object.
     * @param exts
     *            Array of name ending string's.
     * @return Function returns True if file title matched with any of specified title ending's.
     *         Function returns False otherwise.
     */
    private static boolean checkFileExt(File f, String[] exts)
{
    // check filename ends
    String name = f.getAbsolutePath();
    for (String ext : exts)
            if (name.endsWith(ext))
    return true;

return false;
    }
    }
}
