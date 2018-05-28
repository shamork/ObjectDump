namespace MiP.ObjectDump.Reflection
{
    /// <summary>
    /// An error happened while reading a property value.
    /// </summary>
    public class DError : DObject
    {
        public string Error { get; }

        public DError(string error)
        {
            Error = error;
        }
    }
}
