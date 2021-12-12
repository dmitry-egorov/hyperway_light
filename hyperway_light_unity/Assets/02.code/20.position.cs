using Common.spaces;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using static Hyperway.hyperway.entity_type_props;

namespace Hyperway {
    using  p2_arr = NativeArray< point2>;

    public static partial class hyperway {
        public partial struct entity_type {
            [savefile] public p2_arr curr_pos_arr; // current position
        
            public void position_fields() => 
                req(positioned, ref curr_pos_arr);
        }
    }
}