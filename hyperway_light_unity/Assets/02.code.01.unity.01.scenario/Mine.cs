using UnityEngine;
using static Hyperway.entity_type.type;
using static Hyperway.hyperway;

namespace Hyperway.unity {
    public class Mine: MonoBehaviour {
        public storage_spec storage;
        
        public void Start() {
            ref var type = ref _entities[building];
            type.add_mine(transform, storage.capacity, storage.amount);
        }
    }
}