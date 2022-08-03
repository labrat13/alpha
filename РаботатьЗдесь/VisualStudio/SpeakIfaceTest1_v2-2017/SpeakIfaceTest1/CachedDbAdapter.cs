using System;
using System.Collections.Generic;
using System.Text;

namespace Operator
{
    /// <summary>
    /// Адаптер БД с кешем хранящихся элементов
    /// </summary>
    public class CachedDbAdapter: SqliteDbAdapter
    {
        /// <summary>
        /// Список процедур, все процедуры держим здесь в памяти. Они примерно будут занимать до 1 мб на 1000 процедур.
        /// </summary>
        private ProcedureCollection m_procedures;

        /// <summary>
        /// Список мест, все места держим здесь в памяти. Они будут мало памяти занимать, наверно..
        /// </summary>
        private PlacesCollection m_places;


        public CachedDbAdapter()//: base()
        {
            this.m_places = new PlacesCollection();
            this.m_procedures = new ProcedureCollection();
        }

        /// <summary>
        /// Список процедур, все процедуры держим здесь в памяти. Они примерно будут занимать до 1 мб на 1000 процедур.
        /// </summary>
        public ProcedureCollection Procedures
        {
            get { return m_procedures; }
            set { m_procedures = value; }
        }

        /// <summary>
        /// Список мест, все места держим здесь в памяти. Они будут мало памяти занимать, наверно..
        /// </summary>
        public PlacesCollection Places
        {
            get { return m_places; }
            set { m_places = value; }
        }

        #region Main functions
        /// <summary>
        /// NT-Открыть ранее закрытое соединение
        /// И перезагрузить кеши элементов
        /// </summary>
        /// <remarks>
        /// Это перегруженная функция, и она вызывается и из кода родительского класса тоже.
        /// </remarks>
        public override void Open()
        {
            this.m_places.Clear();
            this.m_procedures.Clear();
            base.Open();
            reloadProcedures();
            reloadPlaces();

            return;
        }

        /// <summary>
        /// NT-Закрыть текущее соединение
        /// И сбросить кеши элементов
        /// </summary>
        public override void Close()
        {
            this.m_places.Clear();
            this.m_procedures.Clear();
            base.Close();

            return;
        }

        /// <summary>
        /// NT-Добавить новое место и обновить кеш мест
        /// </summary>
        /// <param name="p">Добавляемое место</param>
        public new void AddPlace(Place p)
        {
            base.AddPlace(p);
            //update cache
            reloadPlaces();

            return;
        }

        /// <summary>
        /// NT-Добавить несколько Мест в БД и обновить кеш мест
        /// </summary>
        /// <param name="places">Список заполненных Мест</param>
        public void AddPlace(List<Place> places)
        {
            //Добавить объекты в БД
            foreach (Place p in places)
                base.AddPlace(p);
            //update cache
            reloadPlaces();

            return;
        }


        public void RemovePlace(Place p)
        {
            this.RemovePlace(p.TableId);
            this.reloadPlaces();
        }


        /// <summary>
        /// NT-Добавить Процедуру в БД и обновить кеш процедур
        /// </summary>
        /// <param name="p">Заполненный объект</param>
        public new void AddProcedure(Procedure p)
        {
            //Добавить объект в БД
            base.AddProcedure(p);
            //reload cache
            reloadProcedures();

            return;
        }

        /// <summary>
        /// NT-Добавить несколько Процедур в БД и обновить кеш процедур
        /// </summary>
        /// <param name="procedures">Список заполненных Процедур</param>
        public void AddProcedure(List<Procedure> procedures)
        {
            //Добавить объект в БД
            foreach (Procedure p in procedures)
                base.AddProcedure(p);
            //reload cache
            reloadProcedures();

            return;
        }


        public void RemoveProcedure(Procedure p)
        {
            this.RemoveProcedure(p.TableId);
            this.reloadProcedures();
        }

        #endregion

        #region Service functions
        /// <summary>
        /// NT-Перезагрузить кеш-коллекции мест данными из БД
        /// Чтобы они соответствовали содержимому БД, если она была изменена.
        /// </summary>
        private void reloadPlaces()
        {
            //тут заполнить коллекцию мест данными мест.
            this.m_places.Clear();
            this.m_places.FillFromDb(GetAllPlaces());

            return;
        }

        /// <summary>
        /// NT-Перезагрузить кеш-коллекции процедур данными из БД
        /// Чтобы они соответствовали содержимому БД, если она была изменена.
        /// </summary>
        private void reloadProcedures()
        {
            //тут заполнить коллекцию процедур данными процедур. И не забыть их отсортировать по весу.
            this.m_procedures.Clear();
            this.m_procedures.FillFromDb(GetAllProcedures());

            return;
        }

        #endregion


    }
}
