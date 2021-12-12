using Lanski.Utilities.assertions;
using Unity.Mathematics;
using UnityEngine;
using static Hyperway.hyperway.entity_type_id;

namespace Hyperway {
    using go = GameObject;
    using rand = Unity.Mathematics.Random;
    
    public class FigureGenerator : MonoBehaviour {
        public ushort    count;
        public uint       seed;
        public float     range = 10.0f;
        public float min_speed = -0.2f;
        public float max_speed =  0.2f;
        
        public go prefab;

        public void Start() {
            if (count == 0) return;
            
            ref var type = ref hyperway._entities[figure];

            var min_pos = new float2(1, 1) * -range;
            var max_pos = new float2(1, 1) *  range;
            var min_vel = new float2(1, 1) *  min_speed;
            var max_vel = new float2(1, 1) *  max_speed;

            var random = new rand(seed);
            type.make_random_figures(ref random, count, min_pos, max_pos, min_vel, max_vel);
            
            for (var i = 0; i < count; i++)
                type.trans_arr.Add(Instantiate(prefab).transform);
        }
    }

    public static partial class hyperway {
        public partial struct entity_type {
            public void make_random_figures(ref rand random, ushort count, float2 min_pos, float2 max_pos, float2 min_vel, float2 max_vel) {
                var new_count = this.count + count;
                (new_count <= capacity).assert();

                for (var i = this.count; i < new_count; i++) { 
                    prev_pos_arr[i] =
                        curr_pos_arr[i] = random.next_position(min_pos, max_pos);
                    curr_vel_arr[i] = random.next_velocity(min_vel, max_vel);
                }
            
                this.count = (ushort) new_count;
            }
        }
        
    }
}