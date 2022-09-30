using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NR - 
    /// </summary>
    internal class EntityTypesCollection
    {
        //TODO: Port this class from Java
        
        // #region *** Fields ***

        /**
         * Словарь типов сущностей первого уровня
         */
        private HashMap<String, EntityType> m_EntityTypes;
        // #endregion

        /**
         * Default constructor
         */
        public EntityTypesCollection()
        {
            this.m_EntityTypes = new HashMap<String, EntityType>();
        }
        // #region *** Properties ***

        /**
         * Словарь типов сущностей первого уровня
         * 
         * @return Словарь типов сущностей первого уровня
         */
        public HashMap<String, EntityType> get_EntityTypes()
        {
            return this.m_EntityTypes;
        }

        /**
         * Словарь типов сущностей первого уровня
         * 
         * @param value
         *            Словарь типов сущностей первого уровня
         */
        public void set_EntityTypes(HashMap<String, EntityType> value)
        {
            this.m_EntityTypes = value;
        }

        // #endregion

        /**
         * NT-Ищет запись типа по его названию. Если тип не упомянут, возвращается null
         * 
         * @param nameOfType
         *            Название типа
         * @return Возвращает первый найденный объект записи типа или null если объект не найден
         */
        public EntityType ContainsType(String nameOfType)
        {
            if (m_EntityTypes.containsKey(nameOfType))
                return m_EntityTypes.get(nameOfType);
            else
            {
                for (Map.Entry<String, EntityType> kvp : m_EntityTypes.entrySet())
                {
                    EntityType res = kvp.getValue().ContainsType(nameOfType);
                    if (res != null)
                        return res;
                }
                return null;
            }
        }

        /**
         * NT-Распарсить строку описания классов сущности в дерево классов
         * 
         * @param expression
         *            Выражение описания классов сущности
         * @throws Exception
         *             Exception from EntityTypesCollection.ParseExpression()
         */
        public void ParseExpression(String expression) throws Exception
        {
            // Это образец формата для разработки парсинга.
            // выражение представляет собой список выражений типов через ;
            // Мои места:: Коллекция музыки<Файл::ФайлМузыки>;Файловая система
            // ::Папка < Файловая система::Папка,Файл>;

            // тут надо бы исключение перехватывать и выводить boolean результат
            // парсинга.

            // парсим
            // 1) удаляем пробелы с начала и конца выражения
            String exp = expression.trim();
        // 2) разделяем на элементы по ;
        // String[] sar = exp.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        String[] sar = Utility.StringSplit(exp, ";", true);
        // тут мы получим несколько элементов:
        // [0] Мои места:: Коллекция музыки<Файл::ФайлМузыки>
        // [1] Файловая система ::Папка < Файловая система::Папка,Файл>
        // [2] элемент после последней ; должен уже быть удален как пустой
        // элемент
        // 3) передаем каждое выражение для парсинга в объект типа
        for (String s : sar)
        {
            EntityType t = new EntityType(); // создаем новый объект
        t.ParseExpression(s); // отправляем строку на парсинг
            this.m_EntityTypes.put(t.get_Title(), t); // добавляем объект в коллекцию.
        }

        return;
    }
/**
 * Check expression parsing
 * @param exp Выражение описания классов сущности
 * @return Возвращает true, если выражение распарсено, false в противном случае.
 */
public static boolean TryParsingExpression(String exp)
{
    boolean result = true;
    try
    {
        EntityTypesCollection t = new EntityTypesCollection();
        t.ParseExpression(exp);
    }
    catch (Exception ex)
    {
        result = false;
    }

    return result;
}
    }
}
