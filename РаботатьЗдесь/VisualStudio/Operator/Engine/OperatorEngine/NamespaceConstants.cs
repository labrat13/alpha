using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NT-Список констант названий неймспейсов.
    /// </summary>
    public class NamespaceConstants
    {

        //TODO: Port this class from Java code

        //*** Constants and  Fields ***

        //Добавить сюда новые неймспейсы для использования в БиблиотекаПроцедурОператор.

        /**
         * Пространство служебных Сущностей Оператор.
         */
        public static final String NsService = "Service";
    
    /**
     * Пространство служебных Сущностей - Команд Оператор.
     */
    public static final String NsService_Command = "Service.Command";
    
    /**
     * Пространство служебных Сущностей - Мест Оператор.
     */
    public static final String NsService_Place = "Service.Place";
    
    /**
     * Пространство служебных Сущностей - Настроек Оператор.
     */
    public static final String NsService_Setting = "Service.Setting";
    /**
     * Пространство по умолчанию для Сущностей.  
     */
    public static final String NsDefault = "Default";
    

    }
}
