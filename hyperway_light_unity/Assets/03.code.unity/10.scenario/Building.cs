using System;
using Common;
using Lanski.Utilities.assertions;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Collections;
using Utilities.Maths;
using static Hyperway.hyperway;
using static Hyperway.hyperway.entity_type_props;

namespace Hyperway {
    using former = FormerlySerializedAsAttribute;
    using err = ApplicationException;
    using trans = Transform;
    
    using u16_8 = fixed_arr_8<ushort>;

    [after(typeof(BuildingType))]
    public class Building: MonoBehaviour {
        [former("initial_resources")] 
        public ResourceLoad[] initialResources;
        [former("building_type")] 
        public BuildingType buildingType;
        
        public void Start() {
            var btype = buildingType;
            (btype != null || initialResources.Length == 0).assert();
            
            // @nocheckin @todo: warehouse
            var entity_type_id = btype.entity_type;
            ref var type = ref _entities[entity_type_id];
            
            var storage_id = btype.storage_spec_id;
            var prod_id    = btype.productionType == null ? prod_spec_id.none : btype.productionType.id;
            
            var entity_id = type.add_building(transform, storage_id, prod_id);
            foreach (var load in initialResources) {
                if (load.amount != 0) {} else continue;

                var overflow = type.add(entity_id, load);
                (overflow == 0).assert("Initial resources exceed capacity");
            }

            var view = gameObject.AddComponent<Entity>();
            view.id = type.remote_from(entity_id);
            view.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }
    }

    public static partial class hyperway {
        public partial struct entity_type {
            public entity_id add_building(Transform trans, storage_spec_id bspec, prod_spec_id pspec) {
                var i = count;
                count++;
                (count <= capacity).assert();
            
                trans_arr.Add(trans);
            
                curr_pos_arr    [i] = trans.localPosition.xz();
                storage_spec_arr[i] = bspec;
                storage_slots_type_arr.@ref(i).set(res_id.none);

                if (all(produces)) {
                    prod_spec_id_arr[i] = pspec;
                }

                if (all(houses)) {
                    occupied_arr.Set(i, true);
                }

                return i;
            }
        }
        
    }
}