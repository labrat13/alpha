using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralProcedures
{
    public class LibraryManager
    {
    }

    /**
 * Класс менеджера библиотеки Процедур.
 * Выполняем инициализацию библиотеки, завершение работы библиотеки, загрузку Процедур и Мест в Оператор.
 * 
 * @author Селяков Павел
 *
 */
    @OperatorProcedure(State = ImplementationState.Ready,
            Title = "GeneralProcedures library manager",
            Description = "Library manager for GeneralProcedures library")
public class LibraryManager extends LibraryManagerBase
    {
    // Памятка: переменные родительского класса LibraryManagerBase:
    // * Backreference to Operator Engine object
    // protected Engine m_Engine;
    // * Флаг что менеджер был инициализирован
    // protected Boolean m_Initialized;
    // * Путь к файлу библиотеки Процедур.
    // protected String m_LibraryPath;
    // * Название библиотеки Процедур как идентификатор Библиотеки.
    // protected String m_LibraryTitle;

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
         * Static Version string for current library
         */
    protected static String m_VersionString = "1.0.0.0";// TODO: add valid library version here

    /**
     * NT-Конструктор
     * 
     * @param engine
     *            Backreference for Engine
     * @param title
     *            Library title
     * @param libPath
     *            Library JAR file path
     */
    public LibraryManager(Engine engine, String title, String libPath)
    {
        super(engine, title, libPath);

        // Добавлять код только для новых ресурсов.

        return;
    }

    /**
     * NT- Initialize library manager.
     * This function must be overriden in child class.
     * 
     * @throws Exception
     *             Error in processing.
     */
    @Override
    protected void onInit() throws Exception
    {
        // TODO: Добавить код инициализации ресурсов библиотеки процедур здесь.
        return;
    }

    /**
     * NT-Deinitialize library manager.
     * This function must be overriden in child class.
     * 
     * @throws Exception
     *             Error in processing.
     */
    @Override
    protected void onExit() throws Exception
    {
        // TODO: Добавить код завершения ресурсов библиотеки процедур здесь.
        return;
    }
    /**
     * NT- Get Setting collection from this library.
     * This function must be overriden in child class.
     * 
     * @return Function returns array of SettingItem, defined in this library.
     * @throws Exception
     *             Error in processing.
     */
    @Override
    public SettingItem[] getLibrarySettings() throws Exception
    {
        LinkedList<SettingItem> result = new LinkedList<SettingItem>();
      //Это шаблон, образец, не трогать его!
      //TODO: Описать правила заполнения полей объекта, привести ссылки на форматы, документацию.
      
//      SettingItem s;
//    //заполнить вручную поля
//      s = new SettingItem();
//      //Уникальный идентификатор элемента или первичный ключ таблицы. 0 по умолчанию, здесь можно не указывать.
//      //s.set_TableId(0);
//      //Краткое однострочное название сущности Настройки.
//      s.set_Title("НазваниеНастройки");
//      //Краткое однострочное описание Настройки, не обязателен.
//      s.set_Description("Описание Настройки");
//      //установить категорию для Настройки.
//      s.set_Namespace(NamespaceConstants.NsDefault);
//      //Значение настройки как String Integer Boolean
//      //s.set_Path("Значение Настройки"); хранится в поле Item.Path как строка.
//      s.setValue("Значение Настройки");   
//      //указать название источника как название текущей библиотеки.
//      s.set_Storage(this.m_LibraryTitle);
//      //добавить объект Настройки в выходной массив
//      result.add(s);
      
      
      // вернуть выходной массив
      return result.toArray(new SettingItem[result.size()]);
    }

/**
 * NT- Get Places collection from this library.
 * This function must be overriden in child class.
 * 
 * @return Function returns array of Places, defined in this library.
 * @throws Exception
 *             Error in processing.
 */
@Override
    public Place[] getLibraryPlaces() throws Exception
{
    LinkedList<Place> result = new LinkedList<Place>();// TODO: add library places here

//Это шаблон, образец, не трогать его!
//TODO: Описать правила заполнения полей объекта, привести ссылки на форматы, документацию.

//         Place p;

//         //заполнить вручную поля
//         p = new Place();
//         //Уникальный идентификатор элемента или первичный ключ таблицы. 0 по умолчанию, здесь можно не указывать.
//         //p.set_TableId(0);
//         //Краткое однострочное название сущности места.
//         p.set_Title("title");
//         //Краткое однострочное описание Сущности, не обязателен.
//         p.set_Description("descr");
//         //Веб-путь или файловый путь к месту
//         p.set_Path("path");
//         //Перечисление типов сущности по моей методике.
//         //TODO: добавить сюда ссылку на документацию по методике классов Мест.
//         p.set_PlaceTypeExpression("Приложение::ТекстовыйРедактор<Файл::ТекстовыйФайл>;");
//         //указать название источника как название текущей библиотеки.
//         p.set_Storage(this.m_LibraryTitle);
//         //установить неймспейс для Места.
//         p.set_Namespace(NamespaceConstants.NsDefault);
//         //Список синонимов названия сущности, должны быть уникальными в системе.
//         p.set_Synonim("слон, слона, слону, слоне, слоном");
//         //Распарсить список типов сущностей
//         p.ParseEntityTypeString();
//         //добавить объект Места в выходной массив
//         result.add(p);

// вернуть выходной массив
return result.toArray(new Place[result.size()]);
    }

    /**
     * NT-Get Procedures collection from this library.
     * This function must be overriden in child class.
     * 
     * @return Function returns array of Procedures, defined in this library.
     * @throws Exception
     *             Error in processing.
     */
    @Override
    public Procedure[] getLibraryProcedures() throws Exception
{

    // Тут надо вернуть вызывающему коду массив объектов Процедур, реализованных в этой библиотеке Процедур.
    // Пока просто создаем их в коде, но если Процедур очень много, то следует внести заранее их в XML файл,
    // добавить его в jar-файл этой библиотеки и тут загрузить их из файла XML.
    // Это сэкономит немного памяти VM.

    // TODO: добавить ссылку на текущую библиотеку в каждый объект здесь.

    // создать выходной массив
    LinkedList<Procedure> result = new LinkedList<Procedure>();

// создать объект Процедуры и добавить в выходной массив
Procedure p = new Procedure();
// Уникальный идентификатор элемента или первичный ключ таблицы. 0 по умолчанию, здесь можно не указывать.
// p.set_TableId(0);
// Краткое однострочное название сущности места.
p.set_Title("Тест хеловорд");
// Краткое однострочное описание Сущности, не обязателен.
p.set_Description("Тестовая процедура: выводит на консоль helloworld  и звуковой сигнал.");
// Веб-путь или командная строкас аргументами или путь к процедуре в библиотеке процедур.
p.set_Path("GeneralProcedures.TestProcedures.testHelloWorld()");// Скобки обязательны!
                                                                // Регекс процедуры
                                                                //имена аргументов в регексе и простом регексе - только латинские и цифры! Поскольку русские буквы не поддерживаются в именах групп регекса.
p.set_Regex("тест хеловорд");
// вес процедуры надо подобрать более точно, он зависит от общего набора Процедур.
// TODO: придумать, как динамически изменять и определять вес Процедуры.
// - но в данном случае его все равно нельзя изменить, он зафиксирован в коде тут.
p.set_Ves(0.5);
// добавить название источника как название текущей библиотеки.
p.set_Storage(this.m_LibraryTitle);
//установить неймспейс для команды
p.set_Namespace(NamespaceConstants.NsService);
// Добавить Процедуру в выходной массив
result.add(p);

//*** Add Procedures from PowerProcedures class ***
p = new Procedure();
p.set_Title("Выключить компьютер");
p.set_Description("Выключить компьютер");
p.set_Path("GeneralProcedures.PowerProcedures.DummyShutdown()");//имена аргументов только латинские и цифры.
p.set_Regex("выключить компьютер");//простой регекс, имена аргументов только латинские и цифры.
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsDefault);
result.add(p);

p = new Procedure();
p.set_Title("Перезагрузить компьютер");
p.set_Description("Перезагрузить компьютер");
p.set_Path("GeneralProcedures.PowerProcedures.DummyReload()");//имена аргументов только латинские и цифры.
p.set_Regex("перезагрузить компьютер");//простой регекс, имена аргументов только латинские и цифры.
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsDefault);
result.add(p);

//*** Add Procedures from ProcedureProcedures class ***
p = new Procedure();
p.set_Title("Создать команду НазваниеКоманды");
p.set_Description("Создать команду в БазаДанныхОператор");
p.set_Path("GeneralProcedures.ProcedureProcedures.CommandCreateProcedure(procedureTitle)");//имена аргументов только латинские и цифры.
p.set_Regex("создать команду %cmd");//простой регекс, имена аргументов только латинские и цифры.
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Command);
result.add(p);

p = new Procedure();
p.set_Title("Показать команды");
p.set_Description("Вывести на экран список доступных команд.");
p.set_Path("GeneralProcedures.ProcedureProcedures.CommandListProcedures()");
p.set_Regex("показать команды");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Command);
result.add(p);

p = new Procedure();
p.set_Title("Удалить команду НазваниеКоманды");
p.set_Description("Удалить указанную команду Оператора.");
p.set_Path("GeneralProcedures.ProcedureProcedures.CommandDeleteProcedure(procedureTitle)");
p.set_Regex("удалить команду %cmd");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Command);
result.add(p);

p = new Procedure();
p.set_Title("Изменить команду НазваниеКоманды");
p.set_Description("Изменить указанную команду Оператора.");
p.set_Path("GeneralProcedures.ProcedureProcedures.CommandChangeProcedure()");
p.set_Regex("изменить команду %cmd");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Command);
result.add(p);

p = new Procedure();
p.set_Title("Редактировать команду НазваниеКоманды");
p.set_Description("Синоним к Изменить команду НазваниеКоманды.");
p.set_Path("GeneralProcedures.ProcedureProcedures.CommandChangeProcedure()");
p.set_Regex("редактировать команду %cmd");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Command);
result.add(p);

p = new Procedure();
p.set_Title("Удалить все команды");
p.set_Description("Удалить все Команды из БазаДанныхОператора.");
p.set_Path("GeneralProcedures.ProcedureProcedures.CommandDeleteAllProcedures()");
p.set_Regex("удалить все команды");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Command);
result.add(p);

//*** Add Procedures from UnsortedProcedures class ***

p = new Procedure();
p.set_Title("Открыть терминал");
p.set_Description("Открытие терминала по пути из ФайлНастроекОператора или ТаблицаНастроекОператора.");
p.set_Path("GeneralProcedures.UnsortedProcedures.OpenTerminal()");
p.set_Regex("открыть терминал");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsDefault);
result.add(p);

p = new Procedure();
p.set_Title("Открыть консоль");
p.set_Description("Открытие терминала по пути из ФайлНастроекОператора или ТаблицаНастроекОператора.");
p.set_Path("GeneralProcedures.UnsortedProcedures.OpenTerminal()");
p.set_Regex("открыть консоль");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsDefault);
result.add(p);

p = new Procedure();
p.set_Title("Создать заметку НазваниеЗаметки");
p.set_Description("Создание текстовой заметки на рабочем столе.");
p.set_Path("GeneralProcedures.UnsortedProcedures.CommandCreateNotepadNote(noteTitle)");
p.set_Regex("создать заметку %title");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsDefault);
result.add(p);

p = new Procedure();
p.set_Title("Открыть НазваниеМеста");
p.set_Description("Открыть указанное Место");
p.set_Path("GeneralProcedures.UnsortedProcedures.CommandOpen(place)");
p.set_Regex("открыть %place");
p.set_Ves(0.9);//вес большой, чтобы команда выбиралась последней в списке команд.
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsDefault);
result.add(p);


//*** Add Procedures from PlaceProcedures class ***

p = new Procedure();
p.set_Title("Создать место НазваниеМеста");
p.set_Description("Создать Место в БазаДанныхОператора.");
p.set_Path("GeneralProcedures.PlaceProcedures.CommandCreatePlace()");
p.set_Regex("создать место %place");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Place);
result.add(p);

p = new Procedure();
p.set_Title("Изменить место НазваниеМеста");
p.set_Description("Изменить Место в БазаДанныхОператора.");
p.set_Path("GeneralProcedures.PlaceProcedures.CommandChangePlace()");
p.set_Regex("изменить место %place");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Place);
result.add(p);

p = new Procedure();
p.set_Title("Редактировать место НазваниеМеста");
p.set_Description("Синоним для Изменить место НазваниеМеста.");
p.set_Path("GeneralProcedures.PlaceProcedures.CommandChangePlace()");
p.set_Regex("редактировать место %place");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Place);
result.add(p);

p = new Procedure();
p.set_Title("Показать места");
p.set_Description("Вывести на экран список доступных Мест.");
p.set_Path("GeneralProcedures.PlaceProcedures.CommandListPlaces()");
p.set_Regex("показать места");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Place);
result.add(p);

p = new Procedure();
p.set_Title("Удалить место НазваниеМеста");
p.set_Description("Удалить указанное Место из БазаДанныхОператор.");
p.set_Path("GeneralProcedures.PlaceProcedures.CommandDeletePlace()");
p.set_Regex("удалить место %place");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Place);
result.add(p);

p = new Procedure();
p.set_Title("Удалить все места");
p.set_Description("Удалить все Места из БазаДанныхОператора.");
p.set_Path("GeneralProcedures.PlaceProcedures.CommandDeleteAllPlaces()");
p.set_Regex("удалить все места");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Place);
result.add(p);

//*** Add Procedures from SettingProcedures class ***

p = new Procedure();
p.set_Title("Показать настройки");
p.set_Description("Вывести на экран список доступных Настроек.");
p.set_Path("GeneralProcedures.SettingProcedures.CommandListSettings()");
p.set_Regex("показать настройки");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Setting);
result.add(p);

p = new Procedure();
p.set_Title("Создать настройку НазваниеНастройки");
p.set_Description("Создать Настройку в БазаДанныхОператора.");
p.set_Path("GeneralProcedures.SettingProcedures.CommandCreateSetting()");
p.set_Regex("создать настройку %setting");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Setting);
result.add(p);

p = new Procedure();
p.set_Title("Изменить настройку НазваниеНастройки");
p.set_Description("Изменить указанную Настройку Оператора");
p.set_Path("GeneralProcedures.SettingProcedures.CommandChangeSetting()");
p.set_Regex("изменить настройку %setting");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Setting);
result.add(p);

p = new Procedure();
p.set_Title("Редактировать настройку НазваниеНастройки");
p.set_Description("Синоним для Изменить настройку НазваниеНастройки");
p.set_Path("GeneralProcedures.SettingProcedures.CommandChangeSetting()");
p.set_Regex("редактировать настройку %setting");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Setting);
result.add(p);

p = new Procedure();
p.set_Title("Удалить настройку НазваниеНастройки");
p.set_Description("Удалить указанную Настройку из БазаДанныхОператора.");
p.set_Path("GeneralProcedures.SettingProcedures.CommandDeleteSetting()");
p.set_Regex("удалить настройку %setting");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Setting);
result.add(p);

p = new Procedure();
p.set_Title("Удалить все настройки");
p.set_Description("Удалить все Настройки из БазаДанныхОператора.");
p.set_Path("GeneralProcedures.SettingProcedures.CommandDeleteAllSettings()");
p.set_Regex("удалить все настройки");
p.set_Ves(0.5);
p.set_Storage(this.m_LibraryTitle);
p.set_Namespace(NamespaceConstants.NsService_Setting);
result.add(p);


// вернуть выходной массив
return result.toArray(new Procedure[result.size()]);
    }

}
