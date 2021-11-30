using Unity.Mathematics;
using UnityEngine;
using static Cities.city;

namespace Cities {
    using go = GameObject;
    
    public class figure_spawner : MonoBehaviour {
        public ushort count;
        public float range = 10.0f;
        public float min_speed = 0.1f;
        public float max_speed = 0.2f;
        public go prefab;

        public void Start() {
            var figure = new entity { arch_index = 0 };
            ref var archetype = ref instance.archetypes[figure.arch_index];
            archetype.make_figure_archetype(count);

            var min_pos = new float2(1, 1) * -range;
            var max_pos = new float2(1, 1) *  range;
            var min_vel = new float2(1, 1) *  min_speed;
            var max_vel = new float2(1, 1) *  max_speed;
            
            archetype.make_random_figures(min_pos, max_pos, min_vel, max_vel);
            for (var i = 0; i < count; i++) 
                archetype.transform[i] = Instantiate(prefab).transform;
        }
    }
}