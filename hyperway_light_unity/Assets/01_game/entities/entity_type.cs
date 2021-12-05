using System;
using static Hyperway.entity_type;

namespace Hyperway {
    public partial struct entity_type {
        //TODO: assuming that the count check allows skipping the bounds check on the array
        //TODO: that is likely not the case
        bool try_use(Array a1) => a1 != null && count <= a1.Length;
        bool try_use(Array a1, Array a2) => try_use(a1) && try_use(a2);
        bool try_use(Array a1, Array a2, Array a3) => try_use(a1) && try_use(a2) && try_use(a3);
        bool try_use(Array a1, Array a2, Array a3, Array a4) => try_use(a1) && try_use(a2) && try_use(a3) && try_use(a4);
    }
    
    public static class execution_flags_ext {
        public static bool all(this entity_type_flags src, entity_type_flags check) => (src & check) == check;
        public static bool any(this entity_type_flags src, entity_type_flags check) => (src & check) != 0;
    }
}