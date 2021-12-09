using System;
using static System.Reflection.BindingFlags;

namespace Lanski.Utilities.attributes {
    public static class attributes_ext {
        public static a attr<a>(this Enum value) where a: Attribute {
            var memberInfos = value.GetType().GetMembers(Public | Static);
            var memInfo     = memberInfos[Convert.ToInt32(value)];
            var attributes  = memInfo.GetCustomAttributes(typeof(a), false);
            return attributes.Length > 0 ? (a)attributes[0] : null;
        }
    }
}