using Unity.Mathematics;
using UnityEngine;
using static Hyperway.entity_type.type;
using static Hyperway.hyperway;

namespace Hyperway.unity {
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
            
            ref var type = ref _entities[figure];

            var min_pos = new float2(1, 1) * -range;
            var max_pos = new float2(1, 1) *  range;
            var min_vel = new float2(1, 1) *  min_speed;
            var max_vel = new float2(1, 1) *  max_speed;

            var random = new rand(seed);
            type.make_random_figures(ref random, count, min_pos, max_pos, min_vel, max_vel);
            
            for (var i = 0; i < count; i++)
                type.transform.Add(Instantiate(prefab).transform);
        }
    }
}