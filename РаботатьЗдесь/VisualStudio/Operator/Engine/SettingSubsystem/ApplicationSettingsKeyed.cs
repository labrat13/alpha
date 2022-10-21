using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Engine.OperatorEngine;
using Engine.Utility;

namespace Engine.SettingSubsystem
{
    /// <summary>
    /// NT - Класс настроек приложения, использующий простой текстовый формат ключ=значение и SettingKey в качестве ключей настроек.
    /// </summary>
    /// <remarks>
    /// TODO: добавить сюда описание формата файла настроек и особенности работы.
    /// </remarks>
    public class ApplicationSettingsKeyed : ApplicationSettingsBase
    {
        // TODO: тут может быть неиспользуемых функций несколько. Следует ли их закомментировать после релиза?
        // А то что-то сейчас совсем не думается, так я набор функций и не проработал в подсистеме настроек этой.

        // TODO: Настройки в файле настроек разбросаны хаотично по всему файлу.
        // Надо или группировать их в именованные секции, или сортировать по алфавиту при выводе в файл.

        // Добавление настроек:
        // Новые настройки добавлять в EnumSettingKey как название и описание,
        // и потом сюда в функцию Reset() вместе с значениями по умолчанию.

        #region *** Constants and Fields ***

        /// <summary>
        /// Settings file comment symbol as string '#' 
        /// </summary>
        protected const String commentChar = "#";

        /// <summary>
        /// Строка-маркер окончания файла настроек. Заодно проверяет сохранность русскоязычного текста файла.
        /// </summary>
        protected const String EndOfSettingsFile = "Конец файла настроек";

        #endregion

        #region *** Constructors ***

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsKeyed"/> class.
        /// </summary>
        /// <param name="engine">Ссылка на объект движка.</param>
        public ApplicationSettingsKeyed(Engine.OperatorEngine.Engine engine) : base(engine)
        {
            return;
        }

        #endregion
        
        // *** Properties ***

        #region *** Service functions ***

        /// <summary>
        /// NT-Get string representation of object for debug
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return base.ToString();
        }

        #endregion
        
        #region *** Read functions ***

        /// <summary>
        /// NR-Reset settings to default values
        /// </summary>
        /// <returns></returns>
        public override void Reset()
        {
            // clear dictionary
            this.m_Items.Clear();
            // TODO: store default values
            // operator version
            // engine version
            this.addItem(SettingKey.EngineVersion, Engine.OperatorEngine.Engine.EngineVersionString);
            // TODO: Добавить сюда все настройки из енума
            // TODO: установить правильные значения настроек по умолчанию
            this.addItem(SettingKey.LoneTerminal, "exo-open --launch TerminalEmulator");
            this.addItem(SettingKey.ForCommandTerminal, "xfce4-terminal -x ");
            this.addItem(SettingKey.ForProcedureTerminal, "xfce4-terminal -x ");
            this.addItem(SettingKey.DefaultWorkingDirectory, FileSystemManager.getUserDocumentsFolderPath());
            this.addItem(SettingKey.ShellExecuteCommand, "exo-open ");
            // тексты встроенных команд Оператор
            this.addItem(SettingKey.ExitAppCommands, "выход, выйти, закрыть, quit, close, exit");
            // Команды или Процедуры стартапа и финиша Оператор.
            this.addItem(SettingKey.CmdStartup, "");
            this.addItem(SettingKey.CmdFinish, "");
            this.addItem(SettingKey.IgnoreStartup, "false");
            // Команды или Процедуры результата исполнения Процедур.
            this.addItem(SettingKey.CmdLogoff, "");
            this.addItem(SettingKey.CmdReload, "shutdown -r now");
            this.addItem(SettingKey.CmdShutdown, "shutdown -h now");
            this.addItem(SettingKey.CmdSleep, "");
            this.addItem(SettingKey.CmdHybernate, "");
            //TODO: добавить сюда пути папок пользователя
            // енумы для него уже созданы, если они потребуются.
            //TODO: выяснить, как можно получать эти пути из фреймворка, чтобы не прописывать их здесь.
            // - а может, все же, прописывать тут? Вдруг пользователь предпочитает хранить файлы не в стандартном аккаунте?
            //  - тогда, если путь в настройках не переопределен, то формировать путь для стандартного аккаунта.
            //  - а код для этого формирования разместить в FileSystemManager, как сейчас.
            // - этот процесс переопределения настроек надо документировать, а то я забуду. 

            // TODO: определить, нужно ли тут сбрасывать флаг modified? this.m_Items.setModified()
            return;
        }

        /// <summary>
        /// NT-Load or reload settings from file
        /// </summary>
        /// <param name="filepath">Settings file path</param>
        /// <exception cref="Exception">Error's on read settings file.</exception>
        public override void Load(String filepath)
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
            // при нормальной работе ридер закрывается, а при исключении все приложение
            // закрывается, так что утечки дескриптора не должно образоваться.
            StreamReader reader = new StreamReader(filepath, Encoding.UTF8, false);

            String line, line2, title, value, descr;
            StringBuilder descriptionLines = new StringBuilder();
            bool hasEndOfSettingsFile = false;// флаг, что был прочитан маркер окончания файла настроек.
            String groupTitle = "";
            // read file lines
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
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
                else if (String.IsNullOrEmpty(line))
                    descriptionLines.Length = 0; // clear string builder
                                                 // if line has = then it key-value pair, process it and clear
                                                 // description buffer
                else if (isKeyValueLine(line))
                {
                    String[] sar = StringUtility.StringSplitFirstMatch(line, "=", StringComparison.Ordinal);
                    if (sar == null)
                        throw new Exception(String.Format("Invalid settings file line format: {0} at {1}", filepath, line));
                    title = sar[0].Trim();
                    // выбросить исключение, если название итема настроек - пустое.
                    // а вот значение может быть и пустой строкой.
                    if (String.IsNullOrEmpty(title))
                        throw new Exception(String.Format("Invalid settings title: {0} at {1}", filepath, line));
                    value = sar[1].Trim();
                    // extract description from buffer
                    descr = descriptionLines.ToString();
                    descriptionLines.Length = 0;// clear string builder
                                                // add item
                    this.addItem(groupTitle, title, value, descr);
                }
                // else line is wrong format and file is invalid
                else throw new Exception("Invalid settings file format: " + filepath);
            }

            // close all
            reader.Close();
            if (hasEndOfSettingsFile == false)
                throw new Exception("Invalid end of settings file: " + filepath);
            // 2. set specified file as current file
            this.m_filepath = filepath;
            // 3. modified flag clear
            this.m_Items.isModified = false;

            return;
        }


        /// <summary>
        /// NT-Очистить название группы от символов разметки файла настроек.
        /// </summary>
        /// <param name="line">Строка со знаком комментария.</param>
        /// <returns>Функция возвращает строку без знака комментария.</returns>
        private String conditeGroupTitle(String line)
        {
            int len = line.Length;
            return line.Substring(1, len - 1).Trim();
        }


        /// <summary>
        /// NT-Проверить, что строка это Группа: [groupname]
        /// </summary>
        /// <param name="line">Проверяемая строка.</param>
        /// <returns>
        ///   Функция возвращает True, если строка является строкой названия группы настроек.
        ///   Функция возвращает False в противном случае.
        /// </returns>
        private bool isGroupLine(String line)
        {
            if (String.IsNullOrEmpty(line))
                return false;
            //
            char begin = line[0];
            int len = line.Length;
            char end = line[len - 1];
            if ((begin == '[') && (end == ']'))
                return true;

            return false;
        }


        /// <summary>
        /// NT-Очистить текст описания от знака комментария, итп.
        /// </summary>
        /// <param name="line">Строка со знаком комментария.</param>
        /// <returns>Функция возвращает строку без знака комментария.</returns>
        private String conditeComment(String line)
        {
            // первым символом строки комментария должен быть символ комментария
            if (line.Length > 1)
                return line.Substring(1).Trim();
            else return String.Empty;
        }

        /// <summary>
        ///  NT-Проверить, что строка это маркер конца файла настроек.
        /// </summary>
        /// <param name="line">Проверяемая строка</param>
        /// <returns>
        ///   Функция возвращает True, если строка полностью совпадает с маркером конца файла настроек.
        ///   Функция возвращает False в противном случае.
        /// </returns>
        private bool isEndOfSettingsMarker(String line)
        {
            return String.Equals(line, ApplicationSettingsKeyed.EndOfSettingsFile, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// NT-Проверить, содержит ли входная строка пару ключ=значение
        /// </summary>
        /// <param name="line">Проверяемая строка</param>
        /// <returns>
        ///    Функция возвращает true, если переданная строка содержит пару ключ=значение, false  в противном случае.
        /// </returns>
        private bool isKeyValueLine(String line)
        {
            // если строка содержит = то, скорее всего, это строка ключ=значение
            int pos = line.IndexOf("=");
            return (pos >= 0);
        }


        /// <summary>
        /// NT- Add comment line to description buffer.
        /// </summary>
        /// <param name="buf">Existing StringBuilder object as description buffer.</param>
        /// <param name="line">Trimmed comment line.</param>
        private void appendDescriptionLine(StringBuilder buf, String line)
        {
            // add to buffer as line with line end
            // if new text is empty, skip it
            if (!String.IsNullOrEmpty(line))
            {
                // if buf not empty, add line separator, then add new text
                if (buf.Length > 0)
                    buf.Append(lineSeparator);
                buf.Append(line);
            }

            return;
        }

        /// <summary>
        /// NT- Проверить что переданная строка является строкой комментария
        /// </summary>
        /// <param name="line">Проверяемая строка</param>
        /// <returns>
        ///   Функция возвращает true, если переданная строка является строкой комментария, false  в противном случае.
        /// </returns>
        private bool isCommentLine(String line)
        {
            // первым символом строки комментария должен быть символ комментария
            //        int pos = line.indexOf(commentChar);
            //        return (pos == 0);
            return line.StartsWith(commentChar);
        }

        #endregion

        #region *** Store functions ***

        /// <summary>
        /// NT- Write settings to file
        /// </summary>
        /// <param name="filepath">Settings file path</param>
        /// <exception cref="Exception">Error on writing</exception>
        public override void Store(String filepath)
        {
            // 1. open specified file, write all items from dictionary to file and
            // close file.
            StreamWriter writer = new StreamWriter(filepath, false, Encoding.UTF8);
            this.WriteCommentLines(writer, "Application settings file");
            writer.Write(ApplicationSettingsBase.lineSeparator);
            // write group sorted items
            ItemDictionaryByNamespace itbn = new ItemDictionaryByNamespace();
            itbn.addSettingItems(this.m_Items.getAllItems());
            String[] keys = itbn.getKeys(true);
            foreach (String group in keys)
            {
                this.WriteGroupHeader(writer, group);
                List<Item> items = itbn.getItems(group, true);
                foreach (Item it in items)
                {
                    this.WriteSettingBlock(writer, it);
                }
            }
            // close file
            this.WriteCommentLines(writer, EndOfSettingsFile);
            writer.Close();
            // 2. do not set specified file as current file
            // 3. clear modified flag
            this.m_Items.isModified = false;

            return;
        }


        /// <summary>
        /// NT-Write group header line
        /// </summary>
        /// <param name="writer">File writer.</param>
        /// <param name="group">Group title to write.</param>
        private void WriteGroupHeader(StreamWriter writer, String group)

        {
            // write empty line
            writer.Write(ApplicationSettingsBase.lineSeparator);
            // write group line
            writer.Write("[");
            writer.Write(group.Trim());
            writer.Write("]");
            writer.Write(ApplicationSettingsBase.lineSeparator);

            return;
        }

        /// <summary>
        /// NT-Write setting item
        /// </summary>
        /// <param name="writer">File writer</param>
        /// <param name="item">Item to write</param>
        private void WriteSettingBlock(StreamWriter writer, Item item)

        {
            // write empty line
            writer.Write(ApplicationSettingsBase.lineSeparator);
            // write description
            String d = item.Description;
            this.WriteCommentLines(writer, d);
            // write key-value pair
            // 1. check title and value
            // 2. print title=value
            writer.Write(StringUtility.GetStringTextNull(item.Title));
            writer.Write(" = ");
            writer.Write(StringUtility.GetStringTextNull(item.Path));
            writer.Write(ApplicationSettingsBase.lineSeparator);
            // write empty line
            writer.Write(ApplicationSettingsBase.lineSeparator);

            return;
        }

        /// <summary>
        /// NT-Write item description as multiline comments
        /// </summary>
        /// <param name="writer">File writer</param>
        /// <param name="s">Description text</param>
        private void WriteCommentLines(StreamWriter writer, String s)
        {
            // 1. check null or empty = not print
            if (String.IsNullOrEmpty(s))
                return;
            // 2. split to lines and print each line as comment
            String[] sar = s.Split(new String[] { ApplicationSettingsBase.lineSeparator }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String r in sar)
            {
                writer.Write(commentChar);
                writer.Write(" ");
                writer.Write(r);
                writer.Write(ApplicationSettingsBase.lineSeparator);
            }

            return;
        }

        #endregion
        
        #region *** Collection by SettingKey functions ***

        // ключи для объектов ФайлНастроекОператора должны быть заданы енумом SettingKey.

        /// <summary>
        /// NT-Check setting is present
        /// </summary>
        /// <param name="key">Setting key.</param>
        /// <returns>
        ///   Returns true if setting present in collection, false otherwise.
        /// </returns>
        public bool hasSetting(SettingKey key)
        {
            return base.hasSetting(key.Title);
        }

        /// <summary>
        /// NT-Get settings item array by title
        /// </summary>
        /// <param name="key">Setting item key.</param>
        /// <param name="sorted">Sort items..</param>
        /// <returns>Returns SettingsItem[] array, or returns null if title not exists in collection.</returns>
        public SettingItem[] getItems(SettingKey key, bool sorted)
        {
            //TODO: check - sort by title - remove sorting
            return base.getItems(key.Title, sorted);
        }

        /// <summary>
        ///  NT-Add new or replace existing settings item in collection.
        /// </summary>
        /// <param name="key">Setting item key.</param>
        /// <param name="value">Setting item value as String.</param>
        public void addItem(SettingKey key, String value)
        {
            // super.addItem(key.getTitle(), value, key.getDescription());
            base.addItem(new SettingItem(key, value));
        }

        /// <summary>
        /// NT-Add new or replace existing settings item in collection.
        /// </summary>
        /// <param name="key">Setting item key.</param>
        /// <param name="value">Setting item value as Integer.</param>
        /// <returns></returns>
        public void addItem(SettingKey key, Int32 value)
        {
            //TODO: проверить, может ли эта функция заменить существующий итем коллекции - или надо исправить описание этой функции.
            //что вообще делает эта функция?
            base.addItem(key.Namespace, key.Title, value, key.Description);
        }

        /// <summary>
        /// NT-Add new or replace existing settings item in collection.
        /// </summary>
        /// <param name="key">Setting item key.</param>
        /// <param name="value">Setting item value as Boolean.</param>
        /// <returns></returns>
        public void addItem(SettingKey key, Boolean value)
        {
            base.addItem(key.Namespace, key.Title, value, key.Description);
        }

        /// <summary>
        /// NT-Get first item value by key
        /// </summary>
        /// <param name="key">Setting key</param>
        /// <returns>Function returns first SettingItem.value as String, or returns null if nothing found.</returns>
        public String getValue(SettingKey key)
        {
            SettingItem[] sar = base.getItems(key.Title, false);
            if (sar == null)
                return null;
            if (sar.Length == 0)
                return null;

            return sar[0].getValueAsString();// get value as Item.Path
        }

        /// <summary>
        /// NT-Get first item value by key
        /// </summary>
        /// <param name="key">Setting key</param>
        /// <returns>Function returns first SettingItem.value as Boolean, or returns null if nothing found.</returns>
        public Boolean? getValueAsBoolean(SettingKey key)
        {
            Boolean? result = null;
            SettingItem[] sar = base.getItems(key.Title, false);
            if (sar == null)
                return result;
            if (sar.Length == 0)
                return result;

            return sar[0].getValueAsBoolean();
        }

        /// <summary>
        /// NT-Get first item value by key
        /// </summary>
        /// <param name="key">Setting key.</param>
        /// <returns>Function returns first SettingItem.value as Int32?, or returns null if nothing found.</returns>
        public Int32? getValueAsInteger(SettingKey key)
        {
            Int32? result = null;
            SettingItem[] sar = base.getItems(key.Title, false);
            if (sar == null)
                return result;
            if (sar.Length == 0)
                return result;

            return sar[0].getValueAsInteger();
        }
        #endregion
        // *** End of file ***
    }
}
