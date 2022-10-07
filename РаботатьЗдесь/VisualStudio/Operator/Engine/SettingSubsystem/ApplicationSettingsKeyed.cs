using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.SettingSubsystem
{
    /// <summary>
    /// NR - Класс настроек приложения, использующий EnumSettingKey в качестве ключей настроек.
    /// </summary>
    public class ApplicationSettingsKeyed :ApplicationSettingsBase
    {
        // TODO: тут может быть неиспользуемых функций несколько. Следует ли их закомментировать после релиза?
        // А то что-то сейчас совсем не думается, так я набор функций и не проработал в подсистеме настроек этой.

        // TODO: Настройки в файле настроек разбросаны хаотично по всему файлу.
        // Надо или группировать их в именованные секции, или сортировать по алфавиту при выводе в файл.

        // Добавление настроек:
        // Новые настройки добавлять в EnumSettingKey как название и описание,
        // и потом сюда в функцию Reset() вместе с значениями по умолчанию.

        // *** Constants and Fields ***

        /**
         * Settings file comment symbol as string('#')
         */
        protected final static String commentChar = "#";

        /**
         * Строка-маркер окончания файла настроек. Заодно проверяет сохранность русскоязычного текста файла.
         */
        protected final static String EndOfSettingsFile = "Конец файла настроек";

        // *** Constructors ***

        /**
         * Paramether constructor.
         * 
         * @param engine
         *            Engine object backreference.
         */
        public ApplicationSettingsKeyed(Engine engine)
        {
            super(engine);

            return;
        }
        // *** Properties ***

        // *** Service functions ***

        /**
         * NT-Get string representation of object for debug
         * 
         * @see Settings.ApplicationSettingsBase#toString()
         */
        @Override
    public String toString()
        {
            return super.toString();
        }
        // *** Work functions ***

        /**
         * NT-Reset settings to default values
         */
        @Override
    public void Reset()
        {
            // clear dictionary
            this.m_Items.Clear();
            // TODO: store default values
            // operator version
            // engine version
            this.addItem(EnumSettingKey.EngineVersion, Engine.EngineVersionString);
            // TODO: Добавить сюда все настройки из енума
            // TODO: установить правильные значения настроек по умолчанию
            this.addItem(EnumSettingKey.LoneTerminal, "exo-open --launch TerminalEmulator");
            this.addItem(EnumSettingKey.ForCommandTerminal, "xfce4-terminal -x ");
            this.addItem(EnumSettingKey.ForProcedureTerminal, "xfce4-terminal -x ");
            this.addItem(EnumSettingKey.DefaultWorkingDirectory, FileSystemManager.getUserDocumentsFolderPath());
            this.addItem(EnumSettingKey.ShellExecuteCommand, "exo-open ");
            // тексты встроенных команд Оператор
            this.addItem(EnumSettingKey.ExitAppCommands, "выход, выйти, закрыть, quit, close, exit");
            // Команды или Процедуры стартапа и финиша Оператор.
            this.addItem(EnumSettingKey.CmdStartup, "");
            this.addItem(EnumSettingKey.CmdFinish, "");
            this.addItem(EnumSettingKey.IgnoreStartup, "false");
            // Команды или Процедуры результата исполнения Процедур.
            this.addItem(EnumSettingKey.CmdLogoff, "");
            this.addItem(EnumSettingKey.CmdReload, "shutdown -r now");
            this.addItem(EnumSettingKey.CmdShutdown, "shutdown -h now");
            this.addItem(EnumSettingKey.CmdSleep, "");
            this.addItem(EnumSettingKey.CmdHybernate, "");
            //TODO: добавить сюда пути папок пользователя
            // енумы для него уже созданы, если они потребуются.
            //TODO: выяснить, как можно получать эти пути из Java-фреймворка, чтобы не прописывать их здесь.
            // - а может, все же, прописывать тут? Вдруг пользователь предпочитает хранить файлы не в стандартном аккаунте?
            //  - тогда, если путь в настройках не переопределен, то формировать путь для стандартного аккаунта.
            //  - а код для этого формирования разместить в FileSystemManager, как сейчас.
            // - этот процесс переопределения настроек надо документировать, а то я забуду. 

            // TODO: определить, нужно ли тут сбрасывать флаг modified? this.m_Items.setModified()
            return;
        }

        /**
         * NT-Load or reload settings from file
         * 
         * @param filepath
         *            Settings file path
         * @throws Exception
         *             Error's on read settings file.
         */
        @Override
    public void Load(String filepath) throws Exception
        {

            /*
             * Стандартный формат файла:
             * Комментарий Заголовок с копирайтом
             * Пустая строка
             * [группа настроек]
             * Пустая строка
             * Комментарий Описание настройки
             * Строка ключ=значение настройки
             * Пустая строка
             * Комментарий Описание настройки №2
             * Строка ключ=значение настройки №2
             * Пустая строка
             * [группа настроек]
             * Пустая строка
             * Комментарий Описание настройки
             * Строка ключ=значение настройки
             * Пустая строка
             * и так далее...
             * Запись о окончании файла настроек EndOfSettingsFile
             * - чтобы обнаружить, когда он не до конца записался.
             */

            /*
             * Порядок разбора:
             * Читаем файл по одной строке
             * В строке уже нет символов конца строки - их удалил BufferedReader.
             * А) Если строка начинается с символа комментария - добавить ее в буфер
             * комментариев после удаления символа комментария и тримминга.
             * Если за строкой комментария следующей идет совершенно пустая строка,
             * то буфер комментариев очистить. Так как это не строка описания итема
             * настроек, то она не пригодится.
             * Б) Если строка не комментарий, и содержит символ =, то это строка
             * ключ=значение.
             * Тогда триммим строку и делим по =. Первой частью будет Ключ,
             * второй частью будет Значение. Их тоже триммим и вместе с описанием из
             * буфера комментариев добавляем в объект итема настроек, который
             * добавляем в словарь настроек.
             * И очистить буфер комментариев, так как он уже не нужен.
             * В) Если строка - пустая, то тут либо она после комментария, либо она
             * после строки ключ=значение.
             * В любом случае буфер комментариев уже не нужен, его надо очистить.
             */

            // 1. open specified file and read all items to dictionary
            FileInputStream fi = new FileInputStream(filepath);
        InputStreamReader isr = new InputStreamReader(fi, "UTF-8");
        // при нормальной работе ридер закрывается, а при исключении все приложение
        // закрывается, так что утечки дескриптора не должно образоваться.
        @SuppressWarnings("resource")
        BufferedReader reader = new BufferedReader(isr);

        String line, line2, title, value, descr;
        StringBuilder descriptionLines = new StringBuilder();
        boolean hasEndOfSettingsFile = false;// флаг, что был прочитан маркер окончания файла настроек.
        String groupTitle = "";
        // read file lines
        while ((line = reader.readLine()) != null)
        {
            line = line.trim();
            // if line is group header - set it as current group title
            if (isGroupLine(line))
            {
                groupTitle = conditeGroupTitle(line);
    }
            // else if line has comment char - add it to description buffer
            else if (isCommentLine(line))
            {
                // удалить знак комментария и триммить остальной текст
                line2 = conditeComment(line);
                // это или комментарий, или маркер завершения файла
                if (isEndOfSettingsMarker(line2))
                    hasEndOfSettingsFile = true;// установить флаг
                else this.appendDescriptionLine(descriptionLines, line2);
    }
            // descriptionLines.append(makeCommentLine(line));
            // if line is empty - reset description buffer
            else if (line.isEmpty())
                descriptionLines.setLength(0); // clear string builder
            // if line has = then it key-value pair, process it and clear
            // description buffer
            else if (isKeyValueLine(line))
            {
                String[] sar = Utility.StringSplitFirstMatch(line, "=");
                if (sar == null)
                    throw new Exception(String.format("Invalid settings file line format: %s at %s", filepath, line));
                title = sar[0].trim();
                // выбросить исключение, если название итема настроек - пустое.
                // а вот значение может быть и пустой строкой.
                if (title.isEmpty())
                    throw new Exception(String.format("Invalid settings title: %s at %s", filepath, line));
                value = sar[1].trim();
    // extract description from buffer
    descr = descriptionLines.toString();
                descriptionLines.setLength(0);// clear string builder
                // add item
                this.addItem(groupTitle, title, value, descr);
    }
            // else line is wrong format and file is invalid
            else throw new Exception("Invalid settings file format: " + filepath);
}

// close all
reader.close();
if (hasEndOfSettingsFile == false)
    throw new Exception("Invalid end of settings file: " + filepath);
// 2. set specified file as current file
this.m_filepath = filepath;
// 3. modified flag clear
this.m_Items.setModified(false);

return;
    }

    /**
     * NT-Очистить название группы от символов разметки файла настроек.
     * 
     * @param line
     *            Строка со знаком комментария.
     * @return Функция возвращает строку без знака комментария.
     */
    private String conditeGroupTitle(String line)
{
    int len = line.length();
    return line.substring(1, len - 1).trim();
}

/**
 * NT-Проверить, что строка это Группа: [groupname]
 * 
 * @param line
 *            Проверяемая строка.
 * @return Функция возвращает True, если строка является строкой названия группы настроек.
 *         Функция возвращает False в противном случае.
 */
private boolean isGroupLine(String line)
{
    if (line.isEmpty())
        return false;
    //
    char begin = line.charAt(0);
    int len = line.length();
    char end = line.charAt(len - 1);
    if ((begin == '[') && (end == ']'))
        return true;

    return false;
}

/**
 * NT-Очистить текст описания от знака комментария, итп.
 * 
 * @param line
 *            Строка со знаком комментария.
 * @return Функция возвращает строку без знака комментария.
 */
private String conditeComment(String line)
{
    // первым символом строки комментария должен быть символ комментария
    if (line.length() > 1)
        return line.substring(1).trim();
    else return "";
}

/**
 * NT-Проверить, что строка это маркер конца файла настроек.
 * 
 * @param line
 *            Проверяемая строка
 * @return Функция возвращает True, если строка полностью совпадает с маркером конца файла настроек.
 *         Функция возвращает False в противном случае.
 */
private boolean isEndOfSettingsMarker(String line)
{
    return Utility.StringEquals(line, ApplicationSettingsKeyed.EndOfSettingsFile);
}

/**
 * NT-Проверить, содержит ли входная строка пару ключ=значение
 * 
 * @param line
 *            Проверяемая строка
 * @return Функция возвращает true, если переданная строка содержит пару ключ=значение, false  в противном случае.
 */
private boolean isKeyValueLine(String line)
{
    // если строка содержит = то, скорее всего, это строка ключ=значение
    int pos = line.indexOf("=");
    return (pos >= 0);
}

/**
 * NT- Add comment line to description buffer.
 * 
 * @param buf
 *            Existing StringBuilder object as description buffer.
 * @param line
 *            Trimmed comment line.
 */
private void appendDescriptionLine(StringBuilder buf, String line)
{
    // add to buffer as line with line end
    // if new text is empty, skip it
    if (!line.isEmpty())
    {
        // if buf not empty, add line separator, then add new text
        if (buf.length() > 0)
            buf.append(lineSeparator);
        buf.append(line);
    }

    return;
}

/**
 * NT- Проверить что переданная строка является строкой комментария
 * 
 * @param line
 *            Строка
 * @return Функция возвращает true, если переданная строка является строкой комментария, false  в противном случае.
 */
private boolean isCommentLine(String line)
{
    // первым символом строки комментария должен быть символ комментария
    //        int pos = line.indexOf(commentChar);
    //        return (pos == 0);
    return line.startsWith(commentChar);

}

/**
 * NT- Write settings to file
 * 
 * @param filepath
 *            Settings file path
 * @throws Exception
 *             Error on writing
 */
@Override
    public void Store(String filepath) throws Exception
{
    // 1. open specified file, write all items from dictionary to file and
    // close file.
    FileOutputStream os = new FileOutputStream(filepath, false);
OutputStreamWriter writer = new OutputStreamWriter(os, "UTF-8");
this.WriteCommentLines(writer, "Application settings file");
writer.write(ApplicationSettingsBase.lineSeparator);
// write group sorted items
ItemDictionaryByNamespace itbn = new ItemDictionaryByNamespace();
itbn.addSettingItems(this.m_Items.getAllItems());
String[] keys = itbn.getKeys(true);
for (String group : keys)
{
    this.WriteGroupHeader(writer, group);
    LinkedList<Item> items = itbn.getItems(group, true);
    for (Item it : items)
    {
        this.WriteSettingBlock(writer, it);
    }
}
// close file
this.WriteCommentLines(writer, EndOfSettingsFile);
writer.close();
// 2. do not set specified file as current file
// 3. clear modified flag
this.m_Items.setModified(false);

return;
    }

    /**
     * NT-Write group header line
     * 
     * @param writer
     *            File writer.
     * @param group
     *            Group title to write.
     * @throws IOException
     *             Error on writing.
     */
    private void WriteGroupHeader(OutputStreamWriter writer, String group)
            throws IOException
{
    // write empty line
    writer.write(ApplicationSettingsBase.lineSeparator);
    // write group line
    writer.write("[");
    writer.write(group.trim());
    writer.write("]");
    writer.write(ApplicationSettingsBase.lineSeparator);

        return;
}

/**
 * NT-Write setting item
 * 
 * @param writer
 *            File writer
 * @param item
 *            Item to write.
 * @throws IOException
 *             Error on writing.
 */
private void WriteSettingBlock(OutputStreamWriter writer, Item item)
            throws IOException
{
    // write empty line
    writer.write(ApplicationSettingsBase.lineSeparator);
    // write description
    String d = item.get_Description();
        this.WriteCommentLines(writer, d);
    // write key-value pair
    // 1. check title and value
    // 2. print title=value
    writer.write(Utility.GetStringTextNull(item.get_Title()));
    writer.write(" = ");
    writer.write(Utility.GetStringTextNull(item.get_Path()));
    writer.write(ApplicationSettingsBase.lineSeparator);
    // write empty line
    writer.write(ApplicationSettingsBase.lineSeparator);

        return;
}


/**
 * NT-Write item description as multiline comments
 * 
 * @param writer
 *            File writer
 * @param s
 *            Description text
 * @throws IOException
 *             Error on writing
 */
private void WriteCommentLines(OutputStreamWriter writer, String s)
            throws IOException
{
        // 1. check null or empty = not print
        if (Utility.StringIsNullOrEmpty(s))
            return;
    // 2. split to lines and print each line as comment
    String []
    sar = Utility.StringSplit(s, ApplicationSettingsBase.lineSeparator, true);
        for (String r : sar)
        {
        writer.write(commentChar);
        writer.write(" ");
        writer.write(r);
        writer.write(ApplicationSettingsBase.lineSeparator);
    }

        return;
}

// *** Collection functions ***

// ключи для объектов ФайлНастроекОператора должны быть заданы енумом EnumSettingKey.

/**
 * NT-Check setting is present
 * 
 * @param key
 *            Setting key
 * @return Returns true if setting present in collection, false otherwise.
 */
public boolean hasSetting(EnumSettingKey key)
{
    return super.hasSetting(key.getTitle());
}

/**
 * NT-Get settings item array by title
 * 
 * @param key
 *            Setting item key
 * @param sorted
 *            Sort items.
 * @return Returns SettingsItem[] array, or returns null if title not exists in
 *         collection.
 */
public SettingItem[] getItems(EnumSettingKey key, boolean sorted)
{
    return super.getItems(key.getTitle(), sorted);
}

/**
 * NT-Add new or replace existing settings item in collection.
 * 
 * @param key
 *            Setting item key.
 * @param value
 *            Setting item value as String.
 */
public void addItem(EnumSettingKey key, String value)
{
    // super.addItem(key.getTitle(), value, key.getDescription());
    super.addItem(new SettingItem(key, value));
}

/**
 * NT-Add new or replace existing settings item in collection.
 * 
 * @param key
 *            Setting item key.
 * @param value
 *            Setting item value as Integer.
 */
public void addItem(EnumSettingKey key, Integer value)
{
    super.addItem(key.getNamespace(), key.getTitle(), value, key.getDescription());
}

/**
 * NT-Add new or replace existing settings item in collection.
 * 
 * @param key
 *            Setting item key.
 * @param value
 *            Setting item value as Boolean.
 */
public void addItem(EnumSettingKey key, Boolean value)
{
    super.addItem(key.getNamespace(), key.getTitle(), value, key.getDescription());
}

/**
 * NT-Get single value by key
 * 
 * @param key
 *            Setting key
 * @return Function returns first SettingItem.value as String, or returns null if nothing found.
 */
public String getValue(EnumSettingKey key)
{
    SettingItem[] sar = super.getItems(key.getTitle(), false);
    if (sar == null)
        return null;
    if (sar.length == 0)
        return null;

    return sar[0].getValueAsString();// get value as Item.Path
}

/**
 * NT-Get single value by key
 * 
 * @param key
 *            Setting key
 * @return Function returns first SettingItem.value as Boolean, or returns null if nothing found.
 */
public Boolean getValueAsBoolean(EnumSettingKey key)
{
    SettingItem[] sar = super.getItems(key.getTitle(), false);
    if (sar == null)
        return null;
    if (sar.length == 0)
        return null;

    return sar[0].getValueAsBoolean();
}

/**
 * NT-Get single value by key
 * 
 * @param key
 *            Setting key
 * @return Function returns first SettingItem.value as Integer, or returns null if nothing found.
 */
public Integer getValueAsInteger(EnumSettingKey key)
{
    SettingItem[] sar = super.getItems(key.getTitle(), false);
    if (sar == null)
        return null;
    if (sar.length == 0)
        return null;

    return sar[0].getValueAsInteger();
}
    // *** End of file ***

    }
}
