namespace MiP.ObjectDump.Reflection
{
    /// <summary>
    /// Represents a null value.
    /// </summary>
    public class DNull : DObject
    {
        internal DNull()
        {
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == typeof(DNull);
        }

        public override int GetHashCode()
        {
            return typeof(DNull).GetHashCode();
        }
    }
}
