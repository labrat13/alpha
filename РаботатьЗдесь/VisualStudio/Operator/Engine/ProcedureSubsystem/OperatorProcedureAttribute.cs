using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ProcedureSubsystem
{
    /// <summary>
    /// NT - This attribute marks assemblies, classes and functions as usable in method execution process.
    /// If assembly, class, function has this attribute, user can view and select as method realization.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class OperatorProcedureAttribute : Attribute
    {
        //DONE: портировать код с явы

        //    // /**
        //    // * Состояние реализации методов, помеченных данным атрибутом.
        //    // */
        //    // public enum ImplementationState
        //    // {
        //    //  NotRealized,    //Метод не реализован (NR).
        //    //  NotTested,      //Метод, класс, сборка реализован, но не тестирован (NT). 
        //    //  Ready;          //Метод, класс, сборка реализован, тестирован, готов к применению (RT).
        //    // }
        //    /**


        /// <summary>
        /// Степень готовности помеченного элемента.
        /// </summary>
        private ImplementationState m_implementationState;

        /// <summary>
        /// Название Процедуры для просмотра пользователем.
        /// </summary>
        private String m_title;

        /// <summary>
        /// Однострочное описание действия Процедуры для просмотра пользователем при выборе Процедуры для Команды.
        /// </summary>
        private String m_description;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="implementationState">Степень готовности помечаемого элемента.</param>
        /// <param name="title">Название элемента для просмотра пользователем.</param>
        /// <param name="description">Однострочное описание элемента для просмотра пользователем при выборе Процедуры для Команды.</param>
        public OperatorProcedureAttribute(ImplementationState implementationState, string title, string description)
        {
            m_implementationState = implementationState;
            m_title = title;
            m_description = description;
        }
        /// <summary>
        /// Степень готовности помеченного элемента.
        /// </summary>
        public ImplementationState ImplementationState 
        {
            get => m_implementationState;
            //set => m_implementationState = value; 
        }
        /// <summary>
        /// Название Процедуры для просмотра пользователем.
        /// </summary>
        public string Title 
        { 
            get => m_title; 
            //set => m_title = value; 
        }
        /// <summary>
        /// Однострочное описание действия Процедуры для просмотра пользователем при выборе Процедуры для Команды.
        /// </summary>
        public string Description 
        { 
            get => m_description;
            //set => m_description = value; 
        }
    }
}
