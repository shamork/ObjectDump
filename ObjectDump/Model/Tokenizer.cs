using System;
using System.Collections.Generic;

namespace MiP.ObjectDump.Model
{
    /*
    public abstract class DToken
    {
        public Type Type { get; }
        public object Value { get; }

        public DToken(Type type, object value)
        {
            Value = value;
            Type = value?.GetType() ?? typeof(void);
        }
    }

    public class DValue : DToken
    {
        public DValue(Type type, object value) : base(type, value)
        {
        }
    }

    public class DArray : DToken
    {
        public IList<DToken> Items = new List<DToken>();

        public DArray(Type type) : base(type)
        {
        }
    }

    public class DObject : DToken
    {
        public IList<DToken> Properties = new List<DToken>();

        public DObject(Type type) : base(type)
        {
        }
    }

    public class Tokenizer
    {
        public DToken GetToken(object item)
        {
            if (ReferenceEquals(item, null))
                return DToken.Null;

            if (IsSimpleType(item))
                return GetToken(item);

            return null;
        }

        private Type[] _simpleTypes =
        {
            typeof(object), typeof(bool), typeof(string),
            typeof(byte), typeof(sbyte),
            typeof(short), typeof (ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),
            typeof(decimal), typeof(double), typeof(float),
            typeof(DateTime), typeof(TimeSpan),
        };

        private bool IsSimpleType(object item)
        {
            if ()
        }
    }
    */
}
