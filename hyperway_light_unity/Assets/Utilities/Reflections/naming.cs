using System;
using System.Text;

namespace Utilities.Reflections {
    public static class naming {
        public static string name_of(Type t) => t.RealName();
        public static string name_of<T>() => typeof(T).RealName();
        public static string names_of<T1, T2>() => names_of(typeof(T1), typeof(T2));
        public static string names_of<T1, T2, T3>() => names_of(typeof(T1), typeof(T2), typeof(T3));
        public static string names_of<T1, T2, T3, T4>() => names_of(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        public static string names_of<T1, T2, T3, T4, T5>() => names_of(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        public static string names_of<T1, T2, T3, T4, T5, T6>() => names_of(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        public static string names_of<T1, T2, T3, T4, T5, T6, T7>() => names_of(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));

        public static string names_of(params Type[] types) {
            if (types.Length == 0)
                return "";
            
            var sb = new StringBuilder();
            sb.append_real_name(types[0], true);

            for (var i = 1; i < types.Length; i++) {
                sb.Append(", ");
                sb.append_real_name(types[i], true);
            }

            return sb.ToString();
        }
    }
}