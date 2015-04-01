using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ObjectChanged
{
    public class ObjectComparer
    {
        /// <summary>
        ///     Detects changes in Properties/Fields decorated with the Changes attribute.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static bool HasChanged(object original, object current)
        {
            // short circuit
            if (original == current)
            {
                return false;
            }

            var members = GetMembersWithAttribute(current, typeof (ChangesAttribute));

            foreach (var m in members)
            {
                var origMember = GetMember(original, m.Name);

                if (origMember != null)
                {
                    if (m.Value != origMember.Value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool HasChanged(IEnumerable original, IEnumerable current)
        {
            // short circuit
            if (original == current)
            {
                return false;
            }

            // go through each item in the enumerable looking for changes...
            if (CompareItems(original, current))
            {
                return true;
            }

            return false;
        }
        
        public static bool HasChanged<T>(IEnumerable<T> original, IEnumerable<T> current)
        {
            // short circuit
            if (original == current)
            {
                return false;
            }

            if (original == null && current != null)
            {
                return true;
            }

            if (current == null && original != null)
            {
                return true;
            }

            if (original.Count() != current.Count())
            {
                return true;
            }

            if (CompareItems(original, current))
            {
                return true;
            }

            return false;
        }

        private static bool CompareItems(IEnumerable original, IEnumerable current)
        {
            // go through each item in the enumerable looking for changes...
            foreach (object c in current)
            {
                var members = GetMembersWithAttribute(c, typeof (ChangesIdAttribute));

                if (members.Count() != 1)
                {
                    throw new InvalidComparisionException(
                        "There must be one and only one field/property with the ChangesId attribute.");
                }

                var m = members.First();

                var idFound = false;
                // now we need to try to find a object with a matching id in the other enumerable
                foreach (var t in original)
                {
                    IEnumerable<Member> origMembers = GetMembersWithAttribute(t, typeof (ChangesIdAttribute));

                    if (origMembers.Count() != 1)
                    {
                        throw new InvalidComparisionException(
                            "There must be one and only one field/property with the ChangesId attribute.");
                    }

                    var origMember = origMembers.First();

                    if (m.Value.Equals(origMember.Value))
                    {
                        idFound = true;
                        // found it, now compare
                        if (HasChanged(t, current))
                        {
                            return true;
                        }
                    }
                }

                if (!idFound)
                {
                    // the id was not found, the enumerables aren't identical
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<Member> GetMembersWithAttribute(object o, Type attrType)
        {
            var oType = o.GetType();

            var properties = oType.GetProperties();

            foreach (var p in properties)
            {
                if (p.GetCustomAttributes(false)
                    .Any(a => a.GetType() == attrType))
                {
                    if (p.GetIndexParameters().Length > 0)
                    {
                        throw new InvalidComparisionException(
                            "Comparing Indexers is not supported. There is no practical way to determine valid indexes.");
                    }
                    
                    yield return new Member {Name = p.Name, Value = p.GetValue(o)};
                }
            }

            var fields = oType.GetFields();

            foreach (var f in fields)
            {
                if (f.GetCustomAttributes(false)
                    .Any(a => a.GetType() == attrType))
                {
                    var f1 = f;
                    yield return new Member {Name = f.Name, Value = f1.GetValue(o)};
                }
            }
        }

        private static Member GetMember(object o, string name)
        {
            var oType = o.GetType();

            var prop = oType.GetProperty(name);

            if (prop != null)
            {
                if (prop.GetIndexParameters().Length > 0)
                {
                    throw new InvalidComparisionException(
                        "Comparing Indexers is not supported. There is no practical way to determine valid indexes.");
                }

                return new Member {Name = prop.Name, Value = prop.GetValue(o)};
            }

            var field = oType.GetField(name);
            if (field != null)
            {
                return new Member {Name = field.Name, Value = field.GetValue(o)};
            }

            return null;
        }

        private class Member
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
    }
}