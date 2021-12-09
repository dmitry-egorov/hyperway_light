using System.Collections.Generic;
using UnityEngine;
using Utilities.Collections;

namespace Utilities {
    public class pool<T> where T: class, new() {
        public static T acquire() {
            var resource = released.is_empty() ? new T() : released.remove_last();
            #if UNITY_ASSERTIONS
            acquired.Add(resource);
            #endif
            return resource;
        }

        public static void release(T resource) {
            #if UNITY_ASSERTIONS
            Debug.Assert(acquired.Contains(resource), "Releasing a path, that wasn't acquired");
            acquired.Remove(resource);
            #endif
            released.Add(resource);
        }

        #if UNITY_ASSERTIONS
        static readonly HashSet<T> acquired = new HashSet<T>();
        #endif
        static readonly List<T> released = new List<T>();
    }
}