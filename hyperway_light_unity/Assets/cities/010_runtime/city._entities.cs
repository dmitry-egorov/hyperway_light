using System;

namespace Cities {
    using save = SerializableAttribute;

    public partial struct city {
        [save] public partial struct archetype {
            //TODO: assuming that the count check allows skipping the bounds check on the array
            //TODO: that is likely not the case
            bool use(Array a1) => a1 != null && count <= a1.Length; 
            bool use(Array a1, Array a2) => use(a1) && use(a2);
            bool use(Array a1, Array a2, Array a3) => use(a1) && use(a2) && use(a3);
            bool use(Array a1, Array a2, Array a3, Array a4) => use(a1) && use(a2) && use(a3) && use(a4);
        }

        [save] public partial struct entity {
            public ushort index;
            public ushort arch_index;
        }
    }
}