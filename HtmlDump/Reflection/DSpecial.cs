namespace MiP.ObjectDump.Reflection
{
    public class DSpecial : DObject
    {
        public string Value { get; }

        /// <summary>
        /// Kind of the special value... Code, Constant, ...
        /// </summary>
        public string Type { get; }

        public DSpecial(string type, string value)
        {
            Value = value;
            Type = type;
        }
    }
}
