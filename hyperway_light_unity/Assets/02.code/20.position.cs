using Common.spaces;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using Utilities.Collections;
using static Hyperway.hyperway.entity_type_props;
using static UnityEngine.GUILayout;

namespace Hyperway {
    using  p2_arr = NativeArray< point2>;

    public static partial class hyperway {
        public partial struct entity_type {
            [savefile] public p2_arr curr_pos_arr; // current position
        
            public void position_fields() => 
                req(positioned, ref curr_pos_arr);
            
            public void inspect_position(entity_id id) {
                Label("Position");
                draw(nameof(curr_pos_arr), curr_pos_arr, id);
            }


            public ref point2 get_position_ref(entity_id entity) => ref curr_pos_arr.@ref(entity);
            public     point2 get_position    (entity_id entity) =>     curr_pos_arr[entity];
        }
    }
}