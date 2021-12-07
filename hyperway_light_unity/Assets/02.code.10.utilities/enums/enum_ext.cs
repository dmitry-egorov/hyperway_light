using System;

namespace Lanski.Utilities.enums {
    public static class enum_ext {
        public static uint to_flag_u32<t>(this t e) where t : Enum, IConvertible => 1u << e.ToInt32(null);
    }
}