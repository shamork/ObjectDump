using System;

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

        public DError(Exception exception)
        {
#if DEBUG
                Error = exception.ToString();
#else
                Error = exception.Message;
#endif
        }
    }
}
