namespace MiP.ObjectDump.Reflection
{
    public class CyclicReference : DObject
    {
        public string Reference { get; }
        public string TypeHeader { get; }

        public CyclicReference(string typeHeader, string reference)
        {
            Reference = reference;
            TypeHeader = typeHeader;
        }
    }
}