using System;
using System.Collections.Generic;
using Engine.OperatorEngine;

namespace Engine.DbSubsystem
{
    /// <summary>
    /// NT-Коллекция мест для быстрого доступа из кода
    /// </summary>
    internal class PlacesCollection
    {
        //DONE: ported from Java to CS

        /// <summary>
        /// Словарь синонимов сущностей мест
        /// </summary>
        private Dictionary<String, Place> m_places;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlacesCollection()
        {
            this.m_places = new Dictionary<String, Place>();
        }

        #region *** Properties ***

        /// <summary>
        /// Словарь синонимов сущностей мест.
        /// </summary>
        public Dictionary<String, Place> Places
        {
            get { return m_places; }
            set { this.m_places = value; }
        }

        #endregion

        #region *** Functions ***


        /// <summary>
        /// NT-Очистить коллекцию
        /// </summary>
        public void Clear()
        {
            this.m_places.Clear();
        }


        /// <summary>
        /// NT-Get count of collection items.
        /// </summary>
        public int Count
        {
            get { return this.m_places.Count; }
        }

        /// <summary>
        /// NT-Добавить в словарь синонимы для указанного места
        /// </summary>
        /// <param name="p"> Объект места</param>
        /// <exception cref="Exception">Ошибка: Синоним Места уже существует в словаре мест.</exception>
        public void Add(Place p)
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
            List<String> syno = p.GetSynonims();
            // если синонимов нет, все скидать в словарь, а если есть - выбросить исключение.
            if (this.ContainsPlaceSynonims(syno) == false)
            {
                foreach (String s in syno)
                    this.m_places.Add(s, p);
            }
            else
            {
                throw new Exception(String.Format("Ошибка: Один из синонимов места {0} уже существует в словаре мест.", p.Title));
            }
            return;
        }


        /// <summary>
        /// NT-Удалить место из коллекции мест
        /// </summary>
        /// <param name="p">Удаляемое место</param>
        public void Remove(Place p)
        {
            // удалить все пары ключ-значение, где значением является указанное
            // место.
            List<String> lis = new List<String>();
            // получим список всех ключей, к которым привязан этот объект
            // это может оказаться сравнительно долгим процессом, если в системе
            // много мест - более 10000.
            // но все равно нужен полный перебор всех элементов словаря, и ничего
            // тут не ускорить.
            foreach (KeyValuePair<String, Place> kvp in this.m_places)
            {
                if (kvp.Value == p)
                    lis.Add(kvp.Key);
            }
            // Теперь удалим из словаря все эти ключи и их данные
            foreach (String s in lis)
                m_places.Remove(s);
            // тут вроде все.
            lis.Clear();

            return;
        }

        /// <summary>
        /// NT-Определить, есть ли такой синоним в коллекции. Это для упрощения вызывающего кода
        /// </summary>
        /// <param name="synonim">Проверяемый синоним Места.</param>
        /// <returns>Возвращает True если имя есть в коллекции, иначе возвращает False.</returns>
        public bool ContainsPlaceBySynonim(String synonim)
        {
            return this.m_places.ContainsKey(synonim);
        }


        /// <summary>
        /// NT-Определить, существует ли в коллекции хотя бы один синоним из данного Места.
        /// </summary>
        /// <param name="p">Объект Места</param>
        /// <returns>
        /// Возвращает True, если хотя бы один синоним данного Места уже существует в коллекции. 
        /// Возвращает False в противном случае.
        /// </returns>
        public bool ContainsPlaceSynonims(Place p)
        {
            List<String> syno = p.GetSynonims();

            return this.ContainsPlaceSynonims(syno);
        }

        /// <summary>
        /// NT-Определить, существует ли в коллекции хотя бы один синоним из данного Места.
        /// </summary>
        /// <param name="syno">Список синонимов из объекта Места.</param>
        /// <returns>
        /// Возвращает True, если хотя бы один синоним данного Места уже существует в коллекции.
        /// Возвращает False в противном случае.
        /// </returns>
        public bool ContainsPlaceSynonims(List<String> syno)
        {
            foreach (String s in syno)
            {
                if (this.m_places.ContainsKey(s))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// NT-Получить объект места по синониму (тексту аргумента).
        /// </summary>
        /// <param name="synonim">Синоним места</param>
        /// <returns>Возвращает объект Места или null если место отсутствует в коллекции.</returns>
        public Place GetPlace(String synonim)
        {
            String t = synonim.Trim();
            if (this.m_places.ContainsKey(t))
            {
                return this.m_places[t];
            }
            else return null;
        }


        /// <summary>
        /// NT-Заполнить коллекцию местами из списка.
        /// </summary>
        /// <param name="list">Список Мест</param>
        /// <exception cref="Exception">Ошибка: Синоним Места уже существует в словаре мест.</exception>
        public void Fill(LinkedList<Place> list)
        {
            foreach (Place p in list)
            {
                // p.ParseEntityTypeString(); TODO: проверить, что это уже сделано при чтении из БД
                this.Add(p);
            }
            return;
        }

        /// <summary>
        /// NR-Выбрать из БД Места по названию, без учета регистра символов
        /// </summary>
        /// <param name="placeTitle">Название места</param>
        /// <returns>Возвращает список мест с указанным названием</returns>
        public List<Place> getByTitle(String placeTitle)
        {
            // словарь имеет ключ - синоним, так что в словаре по 6 штук одних и тех
            // же объектов мест.
            // поэтому создадим еще один словарь - временный для уникализации
            // объектов мест.
            List<Place> result = new List<Place>();
            Dictionary<int, Place> tdic = new Dictionary<int, Place>();

            foreach (Place p in this.m_places.Values)
                if (Utility.StringUtility.StringEqualsOrdinalIgnoreCase(p.Title, placeTitle))
                {
                    // TODO: check: переделал уникальный ключ Места с tableid на hashcode,
                    // поскольку tableid теперь не всегда уникальный, поскольку часть Мест
                    // теперь хранятся в Бибилиотеках Процедур и имеют tableid = -1.
                    int key = p.GetHashCode();
                    if (!tdic.ContainsKey(key))
                        tdic.Add(key, p);
                }
            // тут перенесем содержимое словаря в выходной список и уничтожим
            // словарь
            result.AddRange(tdic.Values);
            tdic.Clear();
            tdic = null;

            return result;
        }

        /// <summary>
        /// NT-Выбрать из коллекции все Места.
        /// </summary>
        /// <returns>Возвращает список всех имеющихся в коллекции Мест.</returns>
        public List<Place> getPlacesAsList()
        {
            // словарь имеет ключ - синоним, так что в словаре по 6 штук одних и тех же объектов мест.
            // поэтому создадим еще один словарь - временный для уникализации объектов мест.
            List<Place> result = new List<Place>();
            Dictionary<int, Place> tdic = new Dictionary<int, Place>();
            foreach (Place p in this.m_places.Values)
            {
                if (p != null)
                {
                    //будем использовать хешкод объекта как уникальный ключ.
                    int key = p.GetHashCode();
                    if (!tdic.ContainsKey(key))
                    {
                        tdic.Add(key, p);
                    }
                }
            }
            //тут перенесем содержимое словаря в выходной список и уничтожим словарь
            result.AddRange(tdic.Values);
            tdic.Clear();
            tdic = null;

            return result;
        }

        /// <summary>
        /// NT-Получить множество уникальных названий неймспейсов элементов коллекции.
        /// </summary>
        /// <returns>Функция возвращает множество уникальных названий неймспейсов элементов коллекции.</returns>
        public HashSet<String> getNamespaces()
        {
            HashSet<String> set = new HashSet<String>();
            // add existing item namespaces
            foreach (Place p in this.m_places.Values)
                set.Add(Utility.StringUtility.GetStringTextNull(p.Namespace));

            return set;
        }

        #endregion
    }
}
