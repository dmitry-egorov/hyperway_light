using System;
using Common;
using UnityEngine;
using UnityEngine.Serialization;
using static Hyperway.hyperway;

namespace Hyperway.unity {
    using no_save = NonSerializedAttribute;
    
    [after(typeof(ScenarioConfig))]
    public class BuildingType : MonoBehaviour {
        public batch_u16[] storage_capacity;
        public batch2_u16 production_cost;
        public batch2_u16 production_product;
        [FormerlySerializedAs("required_ticks")] public ushort production_ticks;

        [no_save] public building_type_id id; // set by ScenarioConfig

        void Start() {
            //TODO: use all the costs and products
            _buildings.storage_capacity_arr [id] = storage_capacity;
            _buildings.production_input_arr [id] = production_cost;
            _buildings.production_output_arr[id] = production_product;
            _buildings.production_ticks_arr [id] = production_ticks;
        }
    }
}