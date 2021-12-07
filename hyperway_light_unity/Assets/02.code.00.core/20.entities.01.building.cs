using UnityEngine;
using Utilities.Assertions;
using Utilities.Maths;

namespace Hyperway {
    using trans = Transform;
    
    public partial struct entity_type {
        public void add_mine(trans trans, batch cap, batch amount) {
            var i = count;
            count++;
            (count <= capacity).else_fail();
            
            transform.Add(trans);
            
                 curr_position[i] = trans.localPosition.xz();
            resources_capacity[i] = cap;
            resources_amount  [i] = amount;
        }
    }
}