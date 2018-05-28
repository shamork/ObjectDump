using System.Collections.Generic;

namespace MiP.ObjectDump.Reflection
{
    public class DArray : DObject
    {
        private readonly List<DObject> _items = new List<DObject>();
        public IReadOnlyList<DObject> Items => _items;

        private Dictionary<string, int> _columns = new Dictionary<string, int>();
        private int _columnCount;
        public IReadOnlyDictionary<string, int> Columns => _columns;

        /// <summary>
        /// Contains a string like 'List{int} (4 items)'.
        /// </summary>
        public string TypeHeader { get; set; }

        public void Add(DObject item)
        {
            _items.Add(item);
        }

        public void AddColumns(IEnumerable<string> columnNames)
        {
            foreach (var name in columnNames)
            {
                if (_columns.ContainsKey(name))
                    continue;

                _columns[name] = _columnCount++;
            }
        }
    }
}
