using System.Collections.Generic;

namespace MiP.ObjectDump.Reflection
{
    public class DComplex : DObject
    {
        private readonly List<DProperty> _properties = new List<DProperty>();
        public IReadOnlyList<DProperty> Properties => _properties;

        /// <summary>
        /// Contains the type of the object in a readable form.
        /// </summary>
        public string TypeHeader { get; }

        /// <summary>
        /// Contains the .ToString() of the original object, or null, if the object's type didn't override .ToString()
        /// </summary>
        public string InstanceHeader { get; }

        public DComplex(string typeHeader, string instanceHeader)
        {
            TypeHeader = typeHeader;
            InstanceHeader = instanceHeader;
        }

        public void AddProperty(string name, DObject value)
        {
            _properties.Add(new DProperty(name, value));
        }
    }
}
