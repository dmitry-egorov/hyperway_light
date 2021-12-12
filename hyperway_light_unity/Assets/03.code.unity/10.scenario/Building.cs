using System;
using Common;
using Lanski.Utilities.assertions;
using UnityEngine;
using Utilities.Assertions;
using Utilities.Collections;
using Utilities.Maths;
using static Hyperway.hyperway.entity_type_id;
using static Hyperway.hyperway;

namespace Hyperway {
    using err = ApplicationException;
    using trans = Transform;
    
    using u16_8 = fixed_arr_8<ushort>;

    [after(typeof(BuildingType))]
    public class Building: MonoBehaviour {
        public ResourceLoad[] initial_resources;

        public BuildingType building_type;
        
        public void Start() {
            var btype = building_type;
            (btype != null || initial_resources.Length == 0).assert();
            
            ref var type = ref _entities[btype.houses_people ? house : building];
            var storage_id = btype == null ? storage_spec_id.none : btype.id;
            var prod_id    = btype == null || btype.production_type == null ? prod_spec_id.none : btype.production_type.id;
            
            var entity_id = type.add_building(transform, storage_id, prod_id, btype.houses_people);
            foreach (var load in initial_resources) {
                var overflow = type.add(entity_id, load);
                (overflow == 0).assert("Initial resources exceed capacity");
            }
        }
    }

    public static partial class hyperway {
        public partial struct entity_type {
            public entity_id add_building(Transform trans, storage_spec_id bspec, prod_spec_id pspec, bool houses) {
                var i = count;
                count++;
                (count <= capacity).else_fail();
            
                trans_arr.Add(trans);
            
                curr_pos_arr          [i] = trans.localPosition.xz();
                storage_spec_arr      [i] = bspec;
                storage_slots_type_arr.@ref(i).set(res_id.none);

                if (prod_spec_id_arr.IsCreated) {
                    prod_spec_id_arr[i] = pspec;
                }

                if (houses) {
                    occupied_arr.Set(i, true);
                }

                return i;
            }
        }
        
    }
}