using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;

namespace Engine.SettingSubsystem
{
    /// <summary>
    /// NT - Представляет элемент данных в файле настроек приложения.
    /// </summary>
    /// <remarks>
    /// Элемент данных содержит название-ключ, текстовое значение и текстовое описание.
    /// Текстовое описание должно выводиться на одной строке комментария, если оно однострочное, или на нескольких строках, если оно многострочное.
    /// На следующей строке должно выводиться пара ключ-значение и затем следующая строка должна быть пустой.
    /// Допускается иметь несколько элементов данных с одинаковым ключом, в этом случае они образуют список значений.
    /// </remarks>
    public class SettingItem : OperatorEngine.Item
    {

        // TODO: убедиться, что при извлечении из файла настроек итемы получают источник = ФайлНастроек
        // TODO: убедиться, что при извлечении из БД итемы получают источник = Database

        /// <summary>
        /// NT-Default constructor
        /// </summary>
        public SettingItem() : base()
        {
        }

        /// <summary>
        /// NT-Конструктор копирования.
        /// </summary>
        /// <param name="p">Копируемый объект.</param>         
        public SettingItem(SettingItem p)
        {
            this.m_descr = (p.m_descr);
            this.m_namespace = (p.m_namespace);
            this.m_path = (p.m_path);
            this.m_storage = (p.m_storage);
            this.m_title = (p.m_title);
            this.m_tableid = p.m_tableid;
            this.m_readOnly = (p.m_readOnly);

            return;
        }

        /// <summary>
        /// NT - Constructor from EnumSettingKey.
        /// </summary>
        /// <param name="key">SettingKey member.</param>
        /// <param name="value">Setting value as string.</param>
        public SettingItem(SettingKey key, String value)
        {
            this.m_tableid = 0;
            this.m_title = key.Title;
            this.m_descr = key.Description;
            this.m_path = value;
            this.m_namespace = key.Namespace;

            return;
        }

        /// <summary>
        /// NT-Parameter constructor - not for Database Item.
        /// </summary>
        /// <param name="group">Setting item namespace as group.</param>
        /// <param name="title">item title</param>
        /// <param name="value">item value</param>
        /// <param name="descr">item description text</param>
        public SettingItem(String group, String title, String value, String descr)
        {
            this.m_tableid = 0;
            this.m_path = value;
            this.m_descr = descr;
            this.m_title = title;
            this.m_namespace = group;

            return;
        }

        /// <summary>
        /// NT-Parameter constructor - for Database Item.
        /// </summary>
        /// <param name="id">item table id or 0 if not.</param>
        /// <param name="group">Setting item namespace as group.</param>
        /// <param name="title">item title</param>
        /// <param name="value">item value</param>
        /// <param name="descr">item description text</param>
        /// <param name="storage">item storage keyword</param>
        public SettingItem(int id,
                String group,
                String title,
                String value,
                String descr,
                String storage)
        {
            this.m_tableid = id;
            this.m_namespace = group;
            this.m_path = value;
            this.m_descr = descr;
            this.m_title = title;
            this.m_storage = storage;

            return;
        }

        /// <summary>
        /// NT-Return string for debug
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return base.getSingleLineProperties();
        }

        /// <summary>
        /// NT-получить однострочное описание Настройки.
        /// </summary>
        /// <returns>Функция возвращает однострочное описание Настройки в формате "Команда "Значение" из "Хранилище"."Название": Описание."</returns>        
        public String toSingleDescriptionString()
        {
            return String.Format("Команда \"{0}\" из \"{1}\".\"{2}\": {3}.", this.m_path.Trim(), this.m_storage.Trim(), this.m_title.Trim(), this.m_descr.Trim());
        }

        /// <summary>
        /// NT- Get value.
        /// </summary>
        /// <returns>Returns Value as String.</returns>
        public String getValueAsString()
        {
            return this.m_path;
        }

        /// <summary>
        /// NT- Set value.
        /// </summary>
        /// <param name="val">Value as String.</param>
        public void setValue(String val)
        {
            this.m_path = val;
        }

        /// <summary>
        /// NT- Get value
        /// </summary>
        /// <returns>Returns Value as Integer; returns null if Value has invalid format.</returns>
        public Int32? getValueAsInteger()
        {
            return StringUtility.tryParseInteger(this.m_path);
        }

        /// <summary>
        /// Gets the value as Boolean.
        /// </summary>
        /// <returns>Returns Value as Boolean; returns null if Value has invalid format.</returns>
        public Boolean? getValueAsBoolean()
        {
            return StringUtility.tryParseBoolean(this.m_path);
        }

        /// <summary>
        /// NT-Settings value as Boolean
        /// </summary>
        /// <param name="value">the value to set</param>
        public void setValue(bool value)
        {
            this.m_path = value.ToString();
        }

        /// <summary>
        /// NT-Settings value as Int32
        /// </summary>
        /// <param name="value">the value to set</param>
        public void setValue(Int32 value)
        {
            this.m_path = value.ToString();
        }

    }
}
