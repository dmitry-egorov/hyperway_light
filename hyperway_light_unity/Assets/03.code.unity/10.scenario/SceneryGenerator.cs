using Lanski.Utilities.assertions;
using Unity.Mathematics;
using UnityEngine;
using static Hyperway.hyperway.entity_type_id;
using Random = Unity.Mathematics.Random;

namespace Hyperway {
    using go = GameObject;
    
    public class SceneryGenerator : MonoBehaviour {
        public ushort count;
        public   uint seed;
        public  float range = 10.0f;
        
        public go prefab;

        public void Start() {
            ref var type = ref hyperway._entities[scenery];

            var min_pos = new float2(1, 1) * -range;
            var max_pos = new float2(1, 1) *  range;
            var random  = new Random(seed);
            type.make_random_sceneries(ref random, count, min_pos, max_pos);

            for (var i = 0; i < count; i++)
                type.trans_arr.Add(Instantiate(prefab).transform);
        }
    }

    public static partial class hyperway {
        public partial struct entity_type {
            public void make_random_sceneries(ref Random random, ushort count, float2 min_pos, float2 max_pos) {
                var new_count = this.count + count;
                (new_count <= capacity).assert();
            
                for (var i = this.count; i < new_count; i++)
                    curr_pos_arr[i] = random.next_position(min_pos, max_pos);

                this.count = (ushort) new_count;
            }
        }
        
    }
}