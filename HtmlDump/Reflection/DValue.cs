using System;

namespace MiP.ObjectDump.Reflection
{
    /// <summary>
    /// Represents a value which can be represented by a simple string.
    /// This can be a string, but also numbers, floats, datetime, timespan or boolean.
    /// </summary>
    public class DValue : DObject
    {
        public string Value { get; }

        public DValue(string value)
        {
            Value = value;
            ValueType = typeof(string);
        }
        public DValue(string value, Type valueType)
        {
            Value = value;
            this.ValueType = valueType;
        }
        public Type ValueType { get; set; }
    }
}
