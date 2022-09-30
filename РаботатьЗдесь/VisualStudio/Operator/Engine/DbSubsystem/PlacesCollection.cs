using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.DbSubsystem
{
    /// <summary>
    /// Коллекция мест для быстрого доступа из кода
    /// </summary>
    internal class PlacesCollection
    {
        /**
     * Словарь синонимов сущностей мест
     */
        private HashMap<String, Place> m_places;

        /**
         * Default constructor
         */
        public PlacesCollection()
        {
            this.m_places = new HashMap<String, Place>();
        }

        // Properties ====================
        /**
         * Словарь синонимов сущностей мест
         * 
         * @return the places
         */
        public HashMap<String, Place> get_Places()
        {
            return m_places;
        }

        /**
         * Словарь синонимов сущностей мест
         * 
         * @param places
         *            the places to set
         */
        public void set_Places(HashMap<String, Place> places)
        {
            this.m_places = places;
        }

        // Functions ====================
        /**
         * NT-Очистить коллекцию
         */
        public void Clear()
        {
            this.m_places.clear();
        }

        /**
         * NT-Get count of collection items.
         * 
         * @return Returns count of collection items.
         */
        public int getCount()
        {
            return this.m_places.size();
        }

        /**
         * NT-Добавить в словарь синонимы для указанного места
         * 
         * @param p
         *            Объект места
         * @throws Exception
         *             Ошибка: Синоним Места уже существует в словаре мест.
         */
        public void Add(Place p) throws Exception
        {
            // Если такое словосочетание уже есть в словаре, будет выброшено
            // исключение,
            // но часть синонимов места останется в словаре и не даст повторно его
            // добавить.
            // Надо при обнаружении конфликта ключей откатывать состояние к
            // предыдущему,
            // удалив все уже добавленные элементы.
            // Их можно найти по одинаковому для всех них значению - объекту Места в
            // паре ключ-значение.
            // Но все равно, одинаковые имена Мест - это проблема, которую надо
            // учитывать
            // при проектировании содержимого системы.
            // Это проблема методологии. Возможно, ее можно решить с помощью
            // Контекста Задачи.
            // Это можно решить проще - перебором всех синонимов проверить что они
            // отсутствуют в словаре.
            // А потом уже добавлять их в словарь смело.

            // LinkedList<String> syno = p.GetSynonims();
            // for (String s : syno)
            // {
            // if (m_places.containsKey(s))
            // {
            // // тут может оказаться что синоним в словаре уже существует и
            // // ассоциирован с другим местом.
            // // нельзя просто пропустить такой ключ, хоть их и много еще в
            // // списке - нужно сообщить об этом пользователю
            // // и вообще откатить всю процедуру добавления места
            // // в данном случае - удалить из списка все ключи текущего места
            // Remove(p);
            // // и выдать исключение с сообщением о проблеме
            // throw new Exception(String.format("Ошибка: Синоним %s места %s уже существует в словаре мест.", s, p.get_Title()));
            // }
            // else this.m_places.put(s, p);
            // }

            // заменено на новый код с предварительной проверкой
            // Функция p.GetSynonims() занимает относительно много времени, поэтому ее однократно вызывать надо.
            LinkedList<String> syno = p.GetSynonims();
        // если синонимов нет, все скидать в словарь, а если есть - выбросить исключение.
        if (this.ContainsPlaceSynonims(syno) == false)
        {
            for (String s : syno)
                this.m_places.put(s, p);
        }
        else
        {
            throw new Exception(String.format("Ошибка: Один из синонимов места %s уже существует в словаре мест.", p.get_Title()));
        }
        return;
        }

        /**
         * NT-Удалить место из коллекции мест
         * 
         * @param p
         *            Удаляемое место
         */
        public void Remove(Place p)
        {
            // удалить все пары ключ-значение, где значением является указанное
            // место.
            LinkedList<String> lis = new LinkedList<String>();
            // получим список всех ключей, к которым привязан этот объект
            // это может оказаться сравнительно долгим процессом, если в системе
            // много мест - боее 10000.
            // но все равно нужен полный перебор всех элементов словаря, и ничего
            // тут не ускорить.
            for (Map.Entry<String, Place> kvp : m_places.entrySet())
            {
                if (kvp.getValue() == p)
                    lis.add(kvp.getKey());
            }
            // Теперь удалим из словаря все эти ключи и их данные
            for (String s : lis)
                m_places.remove(s);
            // тут вроде все.
            lis.clear();

            return;
        }

        /**
         * NT-Определить, есть ли такой синоним в коллекции. Это для упрощения
         * вызывающего кода
         * 
         * @param synonim
         *            Проверяемый синоним Места.
         * @return Возвращает True если имя есть в коллекции, иначе возвращает
         *         False.
         */
        public boolean ContainsPlaceBySynonim(String synonim)
        {
            return this.m_places.containsKey(synonim);
        }

        /**
         * NT-Определить, существует ли в коллекции хотя бы один синоним из данного Места.
         * 
         * @param p
         *            Объект Места
         * @return
         *         Возвращает True, если хотя бы один синоним данного Места уже существует в коллекции.
         *         Возвращает False в противном случае.
         */
        public boolean ContainsPlaceSynonims(Place p)
        {
            LinkedList<String> syno = p.GetSynonims();

            return this.ContainsPlaceSynonims(syno);
        }

        /**
         * NT-Определить, существует ли в коллекции хотя бы один синоним из данного Места.
         * 
         * @param syno
         *            Список синонимов из объекта Места.
         * @return
         *         Возвращает True, если хотя бы один синоним данного Места уже существует в коллекции.
         *         Возвращает False в противном случае.
         */
        public boolean ContainsPlaceSynonims(LinkedList<String> syno)
        {
            for (String s : syno)
            {
                if (this.m_places.containsKey(s))
                    return true;
            }
            return false;
        }



        /**
         * NT-Получить объект места по синониму (тексту аргумента).
         * 
         * @param synonim
         *            Синоним места
         * @return Возвращает объект Места или null если место отсутствует в
         *         коллекции.
         */
        public Place GetPlace(String synonim)
        {
            String t = synonim.trim();
            if (this.m_places.containsKey(t))
            {
                return this.m_places.get(t);
            }
            else return null;
        }

        /**
         * NT-Заполнить коллекцию местами из списка
         * 
         * @param list
         *            Список Мест
         * @throws Exception
         *             Ошибка: Синоним Места уже существует в словаре мест.
         */
        public void Fill(LinkedList<Place> list) throws Exception
        {
        for (Place p : list)
        {
            // p.ParseEntityTypeString(); TODO: проверить, что это уже сделано при чтении из БД
            this.Add(p);
    }
        return;
    }

/**
 * NR-Выбрать из БД Места по названию, без учета регистра символов
 * 
 * @param placeTitle
 *            Название места
 * @return Возвращает список мест с указанным названием
 */
public LinkedList<Place> getByTitle(String placeTitle)
{
    // словарь имеет ключ - синоним, так что в словаре по 6 штук одних и тех
    // же объектов мест.
    // поэтому создадим еще один словарь - временный для уникализации
    // объектов мест.
    LinkedList<Place> result = new LinkedList<Place>();
    HashMap<Integer, Place> tdic = new HashMap<Integer, Place>();

    for (Place p : this.m_places.values())
            if (Utility.StringEqualsOrdinalIgnoreCase(p.get_Title(), placeTitle))
{
    // TODO: check: переделал уникальный ключ Места с tableid на hashcode,
    // поскольку tableid теперь не всегда уникальный, поскольку часть Мест
    // теперь хранятся в Бибилиотеках Процедур и имеют tableid = -1.
    int key = p.hashCode();
    if (!tdic.containsKey(key))
        tdic.put(key, p);
}
// тут перенесем содержимое словаря в выходной список и уничтожим
// словарь
result.addAll(tdic.values());
tdic.clear();
tdic = null;

return result;
    }

    /**
     * NT-Выбрать из коллекции все Места.
     * 
     * @return Возвращает список всех имеющихся в коллекции Мест.
     */
    public LinkedList<Place> getPlacesAsList()
{
    // словарь имеет ключ - синоним, так что в словаре по 6 штук одних и тех же объектов мест.
    // поэтому создадим еще один словарь - временный для уникализации объектов мест.
    LinkedList<Place> result = new LinkedList<Place>();
    HashMap<Integer, Place> tdic = new HashMap<Integer, Place>();
    for (Place p : this.m_places.values())
        {
    if (p != null)
    {
        //будем использовать хешкод объекта как уникальный ключ.
        int key = p.hashCode();
        if (!tdic.containsKey(key))
        {
            tdic.put(key, p);
        }
    }
}
//тут перенесем содержимое словаря в выходной список и уничтожим словарь
result.addAll(tdic.values());
tdic.clear();
tdic = null;

return result;
    }

    /**
     * NT-Получить множество уникальных названий неймспейсов элементов коллекции.
     * 
     * @return Функция возвращает множество уникальных названий неймспейсов элементов коллекции.
     */
    public HashSet<String> getNamespaces()
{
    HashSet<String> set = new HashSet<String>();
    // add existing item namespaces
    for (Place p : this.m_places.values())
            set.add(Utility.GetStringTextNull(p.get_Namespace()));

return set;
    }
    }
}
