using Common;
using Unity.Mathematics;
using UnityEngine;
using static Hyperway.hyperway;
using Random = Unity.Mathematics.Random;

namespace Hyperway.unity {
    using go = GameObject;
    
    public class SceneryGenerator : MonoBehaviour {
        public ushort count;
        public   uint seed;
        public  float range = 10.0f;
        
        public go prefab;

        public void Start() {
            ref var type = ref entities.new_entity_type();
            type.make_scenery_type(count);

            var min_pos = new float2(1, 1) * -range;
            var max_pos = new float2(1, 1) *  range;

            for (var i = 0; i < count; i++)
                type.transform[i] = Instantiate(prefab).transform;

            var random = new Random(seed);
            type.make_random_sceneries(ref random, min_pos, max_pos);
        }
    }
}