using System;
using System.Reflection;
using UnityEngine;

namespace Utilities.Reflections {
    public static class AttributeExtensions {
        public static T require_attribute<T>(this MethodInfo m) where T : Attribute {
            var attribute = m.GetCustomAttribute<T>();
            Debug.Assert(attribute != null);
            return attribute;
        } 
        
        public static bool try_get_attribute<T>(this MethodInfo m, out T attribute) where T : Attribute {
            attribute = m.GetCustomAttribute<T>();
            return attribute != null;
        }

        public static bool has_attribute<T>(this MethodInfo m) where T : Attribute => m.GetCustomAttribute<T>() != null;
        public static bool has_attribute<T>(this Type t) where T : Attribute => t.GetCustomAttribute<T>() != null;
    }
}