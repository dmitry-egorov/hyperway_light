using System;
using Lanski.Utilities.enums;

namespace Lanski.Utilities.collections {
    using save = SerializableAttribute;

    [save] public struct flags_u32<t> where t: Enum, IConvertible {
        public uint flags;

        public bool has(t value) => (flags & value.to_flag_u32()) > 0;
        
        public static implicit operator flags_u32<t>(t value) => new flags_u32<t> { flags = value.to_flag_u32() };
    }
}