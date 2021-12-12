using System;
using Unity.Collections;
using static Hyperway.hyperway.entity_type_props;

namespace Hyperway {
    using save = SerializableAttribute;
    
    using u16 = UInt16;
    using u32 = UInt16;
    using arr_u32 = NativeArray<uint>;

    public static partial class hyperway {
        public static stats _stats;

        [save] public partial struct stats {
            public void init  () => init_stored();

            public void start () => calculate();
            public void update() => calculate();

            void calculate() {
                calculate_families();
                count_stored_resources();
            }
        }
    }
}