using System;
using System.Text;

namespace Utilities.Reflections {
    public static class TypeExtensions {
        public static bool IsStatic(this Type type) => type.IsAbstract && type.IsSealed;
        public static string RealName(this Type type) {
            if (!type.IsGenericType && type.DeclaringType == null)
                return type.Name;

            var sb = new StringBuilder(); 
            sb.append_real_name(type, true);
            return sb.ToString();
        }

        public static void append_real_name(this StringBuilder sb, Type type, bool is_leaf) {
            var declaring_type = type.DeclaringType;
            if (declaring_type != null) {
                sb.append_real_name(declaring_type, false);
                sb.Append('.');
            }

            var name = type.Name;
            if (!type.IsGenericType || !is_leaf) {
                sb.Append(name);
                return;
            }
            
            var index_of = name.IndexOf('`');
            if (index_of < 0) {
                sb.Append(name);
                return; // parent type was generic
            }

            sb.Append(name.Substring(0, index_of));
            sb.Append('<');
            var appendComma = false;
            foreach (var arg in type.GetGenericArguments()) {
                if (appendComma) sb.Append(',');
                sb.append_real_name(arg, true);
                appendComma = true;
            }

            sb.Append('>');
        }
    }
}