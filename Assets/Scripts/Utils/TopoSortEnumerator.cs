using System;
using System.Collections.Generic;
using System.Linq;

namespace Steamwar.Utils
{
    public class TopologicalSorter<ValueType>
    {
        private class Relations
        {
            public int Dependencies = 0;
            public HashSet<ValueType> Dependents = new HashSet<ValueType>();
        }

        private Dictionary<ValueType, Relations> _map = new Dictionary<ValueType, Relations>();

        public void Add(ValueType obj)
        {
            if (!_map.ContainsKey(obj)) _map.Add(obj, new Relations());
        }

        public void Add(ValueType obj, ValueType dependency)
        {
            if (dependency.Equals(obj)) return;

            if (!_map.ContainsKey(dependency)) _map.Add(dependency, new Relations());

            var dependents = _map[dependency].Dependents;

            if (!dependents.Contains(obj))
            {
                dependents.Add(obj);

                if (!_map.ContainsKey(obj)) _map.Add(obj, new Relations());

                ++_map[obj].Dependencies;
            }
        }

        public void Add(ValueType obj, IEnumerable<ValueType> dependencies)
        {
            foreach (var dependency in dependencies) Add(obj, dependency);
        }

        public void Add(ValueType obj, params ValueType[] dependencies)
        {
            Add(obj, dependencies as IEnumerable<ValueType>);
        }

        public (IEnumerable<ValueType> sorted, IEnumerable<ValueType> cycled) Sort()
        {
            List<ValueType> sorted = new List<ValueType>(), cycled = new List<ValueType>();
            var map = _map.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            sorted.AddRange(map.Where(kvp => kvp.Value.Dependencies == 0).Select(kvp => kvp.Key));

            for (int idx = 0; idx < sorted.Count; ++idx) sorted.AddRange(map[sorted[idx]].Dependents.Where(k => --map[k].Dependencies == 0));

            cycled.AddRange(map.Where(kvp => kvp.Value.Dependencies != 0).Select(kvp => kvp.Key));

            return (sorted, cycled);
        }

        public void Clear()
        {
            _map.Clear();
        }
    }
}
