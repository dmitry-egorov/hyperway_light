using UnityEngine;
using Utilities.Assertions;
using Utilities.Maths;
using static Hyperway.res_type_ext;

namespace Hyperway {
    using trans = Transform;
    
    public partial struct entity_type {
        public void add_building(Transform trans, fixed_batch stored, building_type_id type) {
            var i = count;
            count++;
            (count <= this.capacity).else_fail();
            
            transform_arr.Add(trans);
            
            curr_position_arr[i] = trans.localPosition.xz();
            building_type_arr[i] = type;

            foreach (var res_type in all_res_types) {
                resources[res_type].stored_arr  [i] = stored  [res_type];
            }
        }
    }
}