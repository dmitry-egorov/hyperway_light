using UnityEngine;
using static Hyperway.entity_type_id;
using static Hyperway.hyperway;
using static Hyperway.building_type_id;

namespace Hyperway.unity {
    public class Building: MonoBehaviour {
        public batch_u16[] initial_resources;

        public BuildingType building_type;
        
        public void Start() {
            ref var type = ref _entities[building];
            var pt = building_type == null ? none : building_type.id;
            type.add_building(transform, initial_resources, pt);
        }
    }
}