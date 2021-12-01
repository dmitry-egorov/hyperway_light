using Cities.editors;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Cities {
    using go = GameObject;
    
    public class FigureGenerator : MonoBehaviour {
        public ushort    count;
        public uint       seed;
        public float     range = 10.0f;
        public float min_speed = 0.1f;
        public float max_speed = 0.2f;
        
        public go prefab;

        public void Start() {
            ref var archetype = ref ArchetypeConstructor.archetypes.add(default);
            archetype.make_figure_archetype(count);

            var min_pos = new float2(1, 1) * -range;
            var max_pos = new float2(1, 1) *  range;
            var min_vel = new float2(1, 1) *  min_speed;
            var max_vel = new float2(1, 1) *  max_speed;

            var random = new Random(seed);
            archetype.make_random_figures(ref random, min_pos, max_pos, min_vel, max_vel);
            for (var i = 0; i < count; i++)
                archetype.transform[i] = Instantiate(prefab).transform;
        }
    }
}