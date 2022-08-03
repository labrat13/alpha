using System;
using System.Collections.Generic;

namespace Operator
{
    /// <summary>
    /// Коллекция мест для быстрого доступа из кода
    /// </summary>
    public class PlacesCollection
    {
        /// <summary>
        /// Словарь синонимов сущностей мест
        /// </summary>
        private Dictionary<String, Place> m_places;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public PlacesCollection()
        {
            this.m_places = new Dictionary<string, Place>();
        }

        /// <summary>
        /// Словарь синонимов сущностей мест
        /// </summary>
        public Dictionary<String, Place> Places
        {
            get { return m_places; }
            set { m_places = value; }
        }

        /// <summary>
        /// NT-Добавить в словарь синонимы для указанного места
        /// </summary>
        /// <param name="p">Объект места</param>
        public void Add(Place p)
        {
            //Если такое словосочетание уже есть в словаре, будет выброшено исключение, 
            //но часть синонимов места останется в словаре и не даст повторно его добавить.
            //Надо при обнаружении конфликта ключей откатывать состояние к предыдущему, 
            //удалив все уже добавленные элементы.
            //Их можно найти по одинаковому для всех них значению - объекту Места в паре ключ-значение.
            //Но все равно, одинаковые имена Мест - это проблема, которую надо учитывать 
            //при проектировании содержимого системы.
            //Это проблема методологии. Возможно, ее можно решить с помощью Контекста Задачи.
            //Это можно решить проще - перебором всех синонимов проверить что они отсутствуют в словаре.
			//А потом уже добавлять их в словарь смело.
			List<String> syno = p.GetSynonims();
            foreach (String s in syno)
            {
                if (m_places.ContainsKey(s))
                {
                    //тут может оказаться что синоним в словаре уже существует и ассоциирован с другим местом.
                    //я не могу просто пропустить такой ключ, хоть их и много еще в списке - нужно сообщить об этом пользователю
                    //и вообще откатить всю процедуру добавления места
                    //в данном случае - удалить из списка все ключи текущего места
                    Remove(p);
                    //и выдать исключение с сообщением о проблеме
                    throw new Exception(String.Format("Ошибка: Синоним {0} места {1} уже существует в словаре мест.", s, p.Title));
                }
                else
                    this.m_places.Add(s, p);
            }
            return;
        }
        /// <summary>
        /// NT-Удалить место из коллекции мест
        /// </summary>
        /// <param name="p">Удаляемое место</param>
        public void Remove(Place p)
        {
            //удалить все пары ключ-значение, где значением является указанное место.
            List<string> lis = new List<string>();
            //получим список всех ключей, к которым привязан этот объект
            //это может оказаться сравнительно долгим процессом, если в системе много мест - боее 10000.
            //но все равно нужен полный перебор всех элементов словаря, и ничего тут не ускорить.
            foreach (KeyValuePair<string, Place> kvp in this.m_places)
            {
                if (kvp.Value == p)
                    lis.Add(kvp.Key);
            }
            //Теперь удалим из словаря все эти ключи и их данные
            foreach (String s in lis)
                m_places.Remove(s);
            //тут вроде все.
            lis.Clear();
            return;
        }


        /// <summary>
        /// NT-Определить, есть ли такое имя в коллекции.
        /// </summary>
        /// <remarks>Это для упрощения вызывающего кода</remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        internal bool ContainsPlace(string name)
        {
            return this.m_places.ContainsKey(name);
        }

        /// <summary>
        /// NT-Получить объект места по тексту аргумента.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Place GetPlace(String text)
        {
            String t = text.Trim();
            if (m_places.ContainsKey(t))
            {
                return m_places[t];
            }
            else return null;
        }


        /// <summary>
        /// NT-Заполнить эту коллекцию местами вручную, описав их в коде
        /// </summary>
        internal void FillHardcodedPlaces()
        {
            //Тут можно добавить места, но лучше прямо добавить их в БД.
 
            //DONE: Заполнить эту коллекцию местами вручную, описав их в коде
            //TODO: Описать правила заполнения полей объекта, привести ссылки на форматы, документацию.

            //Place p;
            
            //Это шаблон, образец, не трогать его!

            //p = new Place();
            ////заполнить вручную поля
            //p.Title = "";//Название сущности места, не обязательно, в работе механизма не используется
            //p.Synonim = "";//Список синонимов названия сущности, должны быть уникальными в системе.
            //p.Description = "";//Текст описания сущности, не обязателен, в работе механизма не используется
            //p.Path = "";//Веб-путь или файловый путь к месту
            //p.PlaceTypeExpression = "";//Перечисление типов сущности
            ////добавление в коллекцию 
            //p.ParseEntityTypeString();//Распарсить список типов сущностей
            //this.AddPlace(p);//добавить строку

            //////////////////////////////////////////////////////////////////////////////////////////

            return;

        }


        /// <summary>
        /// NT-Заполнить коллекцию местами из списка
        /// </summary>
        /// <param name="list"></param>
        internal void FillFromDb(List<Place> list)
        {
            foreach (Place p in list)
            {
                //p.ParseEntityTypeString(); это уже сделано при чтении из БД
                this.Add(p);
            }
            return;
        }
        ///// <summary>
        ///// for first time only
        ///// </summary>
        ///// <param name="m_db"></param>
        //internal void FillDbTest(DbAdapter db)
        //{
        //    this.FillHardcodedPlaces();
        //    foreach (Place p in m_places.Values)
        //        db.AddPlace(p);
        //    return;
        //}

        /// <summary>
        /// NT-Очистить коллекцию
        /// </summary>
        internal void Clear()
        {
            this.m_places.Clear();
        }


        /// <summary>
        /// NT-Выбрать из БД Места по названию, без учета регистра символов
        /// </summary>
        /// <param name="placeTitle">Название места</param>
        /// <returns>Возвращает список мест с указанным названием</returns>
        internal List<Place> getByTitle(string placeTitle)
        {
            //словарь имеет ключ - синоним, так что в словаре по 8 штук одних и тех же объектов мест.
            //поэтому создадим еще один словарь - временный для уникализации объектов мест. 
            List<Place> result = new List<Place>();
            Dictionary<int, Place> tdic = new Dictionary<int, Place>();
            foreach (Place p in this.m_places.Values)
                if (String.Equals(p.Title, placeTitle, StringComparison.OrdinalIgnoreCase))
                {
                    if (!tdic.ContainsKey(p.TableId))
                        tdic.Add(p.TableId, p);
                }
            //тут перенесем содержимое словаря в выходной список и уничтожим словарь
            result.AddRange(tdic.Values);
            tdic.Clear();
            tdic = null;

            return result;
        }
    }
}
