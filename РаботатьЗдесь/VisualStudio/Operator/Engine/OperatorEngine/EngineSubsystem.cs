using System;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// Базовый класс подсистем Оператор
    /// </summary>
    /// <remarks>
    /// Этот класс должен применяться в качестве базового при условиях:
    /// 1. Производный класс представляет некоторую подсистему Движка, используемую во время работы Движка.
    /// 2. Эта подсистема выполняет некие действия внутри сеанса Движка. Например, работа с БД.
    /// </remarks>
    public class EngineSubsystem
    {
        //TODO: движок и все классы менеджеров подсистем произвести от этого класса.
        //Сейчас все кроме БД наследуют, но недописаны обработчики.
        
        //TODO: изменить эти функции на Open() Close(), чтобы все были единообразны с БД менеджером.
        //И заменить конструкторы на base(Engine e) здесь и в производных классах, для единообразия.
        
        
        /// <summary>
        /// Backreference to Operator Engine object
        /// </summary>
        protected Engine m_Engine;

        /// <summary>
        /// Флаг что данный объект был инициализирован
        /// </summary>
        protected bool m_Ready;

        /// <summary>
        /// NT-Конструктор
        /// </summary>
        public EngineSubsystem(Engine engine)
        {
            this.m_Ready = false;
            this.m_Engine = engine;

            return;
        }

        //TODO: посмотреть в документации, что происходит с наследованием классов и деструктором в них - надо ли переопределять деструкторы в производных классах и как быть вообще.
        /// <summary>
        /// NR-Деструктор объекта
        /// </summary>
        ~EngineSubsystem()
        {
            //вызвать завершение работы в деструкторе, если он не был нормально завершен ранее.
            Close();
        }

        /// <summary>
        /// NT- Получить флаг готовности подсистемы.
        /// </summary>
        public bool isReady
        {
            get { return this.m_Ready; }
        }

        /// <summary>
        /// NT- Initialize subsystem.
        /// </summary>
        public void Open()
        {
            if (this.m_Ready == false)
            {
                // тут вызвать метод инициализации подсистемы, переопределяемый в производных классах.
                this.onOpen();
                // mark object as initialized
                this.m_Ready = true;
            }

            return;
        }
        /// <summary>
        /// NT-Deinitialize subsystem.
        /// </summary>
        public void Close()
        {
            if (this.m_Ready == true)
            {
                // тут вызвать метод де-инициализации подсистемы, переопределяемый в производных классах.
                this.onClose();
                // clear init flag
                this.m_Ready = false;
            }

            return;
        }



        #region  *** Override this in child classes ***
        /// <summary>
        /// NT-Initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected virtual void onOpen()
        {
            throw new Exception("Function must be overridden");
        }

        /// <summary>
        /// NT-De-initialize Subsystem. This function must be overrided in child classes.
        /// </summary>
        /// <exception cref="Exception">Exception at Function must be overridden</exception>
        protected virtual void onClose()
        {
            throw new Exception("Function must be overridden");
        }
        #endregion

    }
}
