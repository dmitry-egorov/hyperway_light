using System;

namespace Lanski.Utilities.constants {
    using u8  = Byte;
    using u16 = UInt16;
    public static class consts {
        public const u8  u8_max   = u8.MaxValue;
        public const int u8_count = u8_max + 1;
        
        public const u16 u16_max   = u16.MaxValue;
        public const int u16_count = u16_max + 1;
    }
}