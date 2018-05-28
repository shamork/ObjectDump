namespace MiP.ObjectDump.Reflection
{
    public class DProperty
    {
        public DProperty(string name, DObject value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public DObject Value { get; }
    }
}
