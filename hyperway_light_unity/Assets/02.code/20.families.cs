using System;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using UnityEngine;
using static Hyperway.hyperway.entity_type_props;

namespace Hyperway {
    using bit_arr = NativeBitArray;
    using u16 = UInt16;

    public static partial class hyperway {
        public partial struct entity_type {
            [savefile] public bit_arr occupied_arr; // is the house currently occupied
        
            public void family_fields() => req(houses, ref occupied_arr);
            public void inspect_families(entity_id id) {
                GUILayout.Label("Families");
                draw(nameof(occupied_arr), occupied_arr, id);
            }
            
            public void add_occupied(ref u16 result) {
                if (props.all(houses)) {} else return;
                result += (u16)occupied_arr.CountBits(0, count);
            }

            public bool is_occupied(entity_id id) => occupied_arr.IsSet(id);
            public void reset_occupied(entity_id id) => occupied_arr.Set(id, false);
        }

        public partial struct stats {
            public u16 total_families;

            void calculate_families() {
                total_families = 0;
                _entities.for_each(ref total_families, (ref u16 result, ref entity_type type) => type.add_occupied(ref result));
            }
        }
    }
}