using Common;
using Unity.Mathematics;
using UnityEngine;
using static Hyperway.hyperway;
using Random = Unity.Mathematics.Random;

namespace Hyperway.unity {
    using go = GameObject;
    
    public class FigureGenerator : MonoBehaviour {
        public ushort    count;
        public uint       seed;
        public float     range = 10.0f;
        public float min_speed = -0.2f;
        public float max_speed = 0.2f;
        
        public go prefab;

        public void Start() {
            if (count == 0) return;
            
            ref var type = ref new_entity_type();
            type.make_figure_type(count);

            var min_pos = new float2(1, 1) * -range;
            var max_pos = new float2(1, 1) *  range;
            var min_vel = new float2(1, 1) *  min_speed;
            var max_vel = new float2(1, 1) *  max_speed;

            for (var i = 0; i < count; i++)
                type.transform[i] = Instantiate(prefab).transform;

            var random = new Random(seed);
            type.make_random_figures(ref random, min_pos, max_pos, min_vel, max_vel);
        }
    }
}