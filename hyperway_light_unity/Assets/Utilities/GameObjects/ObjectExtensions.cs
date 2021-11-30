using System;
using Object = UnityEngine.Object;

namespace Utilities.GameObjects {
    public static class ObjectExtensions {
        public static TValue or_default<T, TValue>(this T o, Func<T, TValue> selector) where T : Object => o == null ? default : selector(o);
        public static TValue or_default<T, TValue>(this T o, Func<T, TValue> selector, TValue default_value) where T : Object => o == null ? default_value : selector(o);

        public static T declare<T>(this T o, out T val) {
            val = o;
            return o;
        }


        public static bool not_null<T>(this T o, out T not_null_object) where T : class { 
            if (o != null) {
                not_null_object = o;
                return true;
            }

            not_null_object = null;
            return false;
        }

        public static bool exists<T>(this T o, out T not_null_object) where T: Object {
            if (o != null) {
                not_null_object = o;
                return true;
            }

            not_null_object = null;
            return false;
        }
    }
}