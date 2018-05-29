using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MiP.ObjectDump.Reflection
{
    public class Reflector
    {
        private readonly Dictionary<object, string> _knownReferences = new Dictionary<object, string>(new ObjectReferenceEqualityComparer());

        public DObject GetDObject(object item, int depth)
        {
            try
            {
                if (item is null)
                    return DObject.Null;

                if (!(item is ValueType))
                {
                    // TODO: change checking for cyclic: always create objects on current object first, then members of children.
                    if (_knownReferences.ContainsKey(item))
                    {
                        string typeHeader = item.GetType().ToString();
                        return new CyclicReference(typeHeader, _knownReferences[item]);
                    }

                    _knownReferences.Add(item, Guid.NewGuid().ToString());
                }

                if (IsSimpleType(item))
                    return GetSimpleValue(item);

                if (depth == 0)
                    return new DError($"Object nesting too deep for [{item}]"); // TODO: better object string

                if (IsArrayType(item, out IEnumerable<object> items))
                    return GetArray(items, item.GetType(), depth - 1);

                return GetComplex(item, depth - 1);
            }
            catch(TargetInvocationException ex)
            {
                return new DError(ex.InnerException);
            }
            catch (Exception ex)
            {
                return new DError(ex);
            }
        }

        private DObject GetComplex(object item, int depth)
        {
            Type itemType = item.GetType();

            string stringified = null;
            Type declaringTypeOfToString = itemType.GetMethod(nameof(ToString), new Type[0]).DeclaringType;
            if (declaringTypeOfToString != typeof(object) && declaringTypeOfToString != typeof(ValueType))
            {
                // TODO: create extension point here for providing formatting
                stringified = item.ToString();
            }

            // TODO: provide nicer type formatting
            var complex = new DComplex(itemType.ToString(), stringified);

            var properties = GetMembers(item, itemType, depth);

            foreach (var property in properties)
            {
                complex.AddProperty(property.Key, property.Value);
            }

            return complex;
        }

        protected IDictionary<string, DObject> GetMembers(object item, Type itemType, int depth)
        {
            var fields = itemType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            var properties = itemType.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

            Dictionary<string, DObject> result = new Dictionary<string, DObject>();

            foreach (var field in fields)
            {
                string name = field.Name;
                object value = field.GetValue(item);
                result.Add(name, GetDObject(value, depth));
            }

            foreach (var property in properties)
            {
                string name = property.Name;

                try
                {
                    object value = property.GetValue(item);
                    result.Add(name, GetDObject(value, depth));
                }
                catch (TargetInvocationException ex)
                {
                    result.Add(name, new DError(ex.InnerException));
                }
                catch (Exception ex)
                {
                    result.Add(name, new DError(ex));
                }
            }

            return result;
        }

        private DArray GetArray(IEnumerable<object> arrayObject, Type arrayType, int depth)
        {
            var array = new DArray();

            object[] list = arrayObject.ToArray();
            string type = arrayType.Name;

            array.TypeHeader = $"{type} ({list.Length} items)";

            foreach (var item in list)
            {
                DObject arrayItem = GetDObject(item, depth);
                array.Add(arrayItem);

                if (arrayItem is DComplex complex)
                {
                    array.AddColumns(complex.Properties.Select(p => p.Name));
                }
            }

            return array;
        }

        private bool IsArrayType(object item, out IEnumerable<object> enumerable)
        {
            if (item is IEnumerable list)
            {
                enumerable = list.Cast<object>();
                return true;
            }

            enumerable = null;
            return false;
        }

        private DValue GetSimpleValue(object item)
        {
            return new DValue(item.ToString()); // create extension points here to provide formatting, maybe use IConvertible
        }

        private readonly Type[] _simpleTypes =
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
            return _simpleTypes.Any(t => t == item?.GetType());
        }
    }
}
