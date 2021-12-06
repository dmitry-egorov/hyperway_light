using System;

namespace Lanski.Utilities.attributes {
    public static class attributes_ext {
        public static T attr<T>(this Enum value) where T: Attribute {
            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }
    }
}