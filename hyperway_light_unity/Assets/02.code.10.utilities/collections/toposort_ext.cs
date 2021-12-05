using System;
using System.Collections.Generic;

namespace Utilities.Collections {
    public static class toposort_ext {
        public static List<T> order_topologically<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> dependecies_getter) {
            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>();

            foreach (var item in source) 
                item.visit(dependecies_getter, sorted, visited);

            return sorted;
        }

        static void visit<T>(this T item, Func<T, IEnumerable<T>> dependecies_getter, List<T> sorted, Dictionary<T, bool> visited_map) {
            var already_visited = visited_map.TryGetValue(item, out var in_process);
            if (already_visited) {
                if (in_process)
                    throw new ArgumentException("Cyclic dependency found.");
                return;
            }

            visited_map[item] = true;

            var dependencies = dependecies_getter(item);
            foreach (var dependency in dependencies)
                dependency.visit(dependecies_getter, sorted, visited_map);

            visited_map[item] = false;
            sorted.Add(item);
        }
    }
}