using System;
using Engine.OperatorEngine;
using Engine.SettingSubsystem;

namespace Engine.Utility
{
    /// <summary>
    /// NT-Класс-контейнер для объектов, передаваемых в функции и обратно.
    /// </summary>
    public class InOutArgument
    {
        //*** Constants and  Fields ***
        /**
         * Object value
         */
        private Object m_value;

        //*** Constructors ***
        /**
         * Default constructor
         */
        public InOutArgument()
        {
            this.m_value = null;
        }
        /**
         * Parameter constructor
         * @param val Initial value
         */
        public InOutArgument(Object val)
        {
            this.m_value = val;
        }
        //*** Properties ***
        /**
         * Check this value is null
         * @return Returns true if value is null, returns false otherwise,
         */
        public bool isNull()
        {
            return (this.m_value == null);
        }
        /**
         * Set value
         * @param ob value
         */
        public void setValue(Object ob)
        {
            this.m_value = ob;
        }
        /**
         * Get value as String
         * @return Function returns value as string
         */
        public String getValueString()
        {
            return (String)this.m_value;
        }
        /**
         * Get value as Double
         * @return Function returns value as Double
         */
        public Double getValueDouble()
        {
            return (Double)this.m_value;
        }
        /**
         * Get value as Integer
         * @return Function returns value as Integer
         */
        public int getValueInteger()
        {
            return (int)this.m_value;
        }
        /**
         * Get value as Procedure
         * @return Function returns value as Procedure
         */
        public Procedure getValueProcedure()
        {
            return (Procedure)this.m_value;
        }

        /**
         * Get value as Place
         * @return Function returns value as Place
         */
        public Place getValuePlace()
        {
            return (Place)this.m_value;
        }

        /**
         * Get value as SettingItem
         * @return Function returns value as SettingItem
         */
        public SettingItem getValueSettingItem()
        {
            return (SettingItem)this.m_value;
        }

        /**
         * Return string representation of object.
         * @see java.lang.Object#toString()
         */

        public override String ToString()
        {
            if (this.m_value == null) return "Null";
            else return m_value.ToString();
        }

        //*** Service  functions ***

        //*** End of file ***



    }
}
