using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.OperatorEngine
{
    /// <summary>
    /// NT-Класс констант названий неймспейсов.
    /// </summary>
    public class NamespaceConstants
    {

        //*** Constants and  Fields ***

        //TODO: Добавить сюда новые неймспейсы для использования в БиблиотекаПроцедурОператор.

        /// <summary>
        /// Пространство служебных Сущностей Оператор.
        /// </summary>
        public const String NsService = "Service";

        /// <summary>
        /// Пространство служебных Сущностей - Команд Оператор.
        /// </summary>
        public const String NsService_Command = "Service.Command";

        /// <summary>
        ///  Пространство служебных Сущностей - Мест Оператор.
        /// </summary>
        public const String NsService_Place = "Service.Place";

        /// <summary>
        /// Пространство служебных Сущностей - Настроек Оператор.
        /// </summary>
        public const String NsService_Setting = "Service.Setting";

        /// <summary>
        /// Пространство по умолчанию для Сущностей.
        /// </summary>
        public const String NsDefault = "Default";


    }
}
