namespace MiP.ObjectDump.Reflection
{
    public class CyclicReference : DObject
    {
        public string Reference { get; }

        public CyclicReference(string reference)
        {
            Reference = reference;
        }
    }
}