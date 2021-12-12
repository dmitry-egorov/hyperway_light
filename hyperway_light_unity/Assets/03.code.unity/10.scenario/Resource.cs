using System;
using Sirenix.OdinInspector;
using UnityEngine;
using static Hyperway.hyperway;

namespace Hyperway {
    using no_save = NonSerializedAttribute;
    
    public class Resource : MonoBehaviour {
        public res_id id;

        void Start() {
            _resources.name_arr[id] = name;
        }

        public static implicit operator res_id(Resource rt) => rt.id;
    }

    public static partial class hyperway {
        [InlineProperty(LabelWidth = 1)] public partial struct res_id { }
    }
}