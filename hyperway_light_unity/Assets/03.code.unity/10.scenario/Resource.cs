using System;
using Sirenix.OdinInspector;
using UnityEngine;
using static Hyperway.hyperway;

namespace Hyperway {
    using no_save = NonSerializedAttribute;
    
    public class Resource : MonoBehaviour {
        public res_id id;
        public bool isFood;

        void Start() {
            _resources.add_resource(id, isFood);
        }

        public static implicit operator res_id    (Resource rt) => rt.id;
        public static implicit operator res_filter(Resource rt) => rt.id;
    }

    public static partial class hyperway {
        public partial struct resources {
            public void add_resource(res_id res_id, bool id_food) {
                _resources.name_arr[res_id] = res_id.name;
                _resources.is_food_arr.Set(res_id, id_food);
            }
        }
    }

    public static partial class hyperway {
        [InlineProperty(LabelWidth = 1)] public partial struct res_id { }
    }
}