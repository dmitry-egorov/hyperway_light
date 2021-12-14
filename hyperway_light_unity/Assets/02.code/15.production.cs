using System;
using Lanski.Plugins.Persistance;
using Lanski.Utilities.assertions;
using Unity.Collections;
using Utilities.Collections;
using static Hyperway.hyperway.prod_spec_id;
using static Hyperway.hyperway.entity_type_props;
using static Hyperway.hyperway;
using static Lanski.Utilities.constants.consts;
using static UnityEngine.GUILayout;

namespace Hyperway {
    using save    =  SerializableAttribute;

    using spec_id = prod_spec_id;
    using slot_id = storage_slot_id;
    using slot_types_8 = fixed_arr_8<res_id>;
    using arr_rt_8 = NativeArray<fixed_arr_8<res_id>>;
    using load_8 = fixed_arr_8<res_load<ushort>>;

    using ps_arr   = NativeArray<prod_spec_id>;
    using u16_arr  = NativeArray<ushort>;
    using u16 = UInt16;
    using u8  = Byte;

    public static partial class hyperway {
        public static prod_specs _prod_specs;

        [save] public partial struct prod_spec_id {
            public byte value;
            
            public static readonly spec_id none      = u8_max;
            public static readonly     int max_count = u8_count;
            
            public static implicit operator byte(spec_id i) => i.value;
            public static implicit operator spec_id(byte b) => new spec_id {value = b};
            public override string ToString() => value == none ? "none" : _prod_specs.name_arr[value];
        }
        
        [save] public partial struct prod_specs {
            public byte count;

            [scenario] public string[]                 name_arr;
            [scenario] public res_multi_8_arr out_resources_arr;
            [scenario] public res_multi_8_arr  in_resources_arr;
            [scenario] public u16_arr        required_ticks_arr;

            public void init() {
                init(ref name_arr);
                init(ref   in_resources_arr);
                init(ref  out_resources_arr);
                init(ref required_ticks_arr);
            }

            void init<t>(ref NativeArray<t> arr) where t : struct => arr.init(max_count);
            void init   (ref res_multi_8_arr arr) { arr.loads.init(max_count); arr.counts.init(max_count); }
            void init<t>(ref t[] arr) { arr_ext.init(ref arr, max_count); }
        }
        
        public partial struct entity_type {
            [scenario] public ps_arr  prod_spec_id_arr;   // production spec id
            [savefile] public u16_arr prod_remaining_arr; // ticks till the production finishes
            
            public void production_fields() => req(produces, ref prod_spec_id_arr, ref prod_remaining_arr);

            public void inspect_production(entity_id id) {
                Label("Production");
                draw(nameof(prod_spec_id_arr  ), prod_spec_id_arr  , id);
                draw(nameof(prod_remaining_arr), prod_remaining_arr, id);
            }

            public void produce() {
                if (all(produces)) {} else return;

                for (ushort entity = 0; entity < count; entity++) {
                    var spec = get_prod_spec(entity);
                    if (spec != spec_id.none) {} else continue;

                    ref var remaining = ref get_remaining_ref(entity);
                    if (remaining > 1) { // progress production
                        remaining -= 1;
                        continue;
                    }

                        var out_count =     get_out_count    (spec);
                    ref var out_loads = ref get_out_loads_ref(spec);
                    if (remaining == 1) { // finish production
                        var overflow = add(entity, out_loads, out_count);
                        schedule_export(entity);
                        remaining = 0;
                        
                        overflow.all(0).assert();
                        continue;
                    }

                    (remaining == 0).assert(); { // check required resources and start production
                            var in_count =     get_in_count    (spec);
                        ref var in_loads = ref get_in_loads_ref(spec);
                        
                        if (has_space(entity, out_loads, out_count)) {} else continue; // no empty space for the output
                        if (try_sub  (entity,  in_loads,  in_count)) {} else continue; // not enough resources

                        remaining = get_ticks(spec);
                    }
                }
            }

            public    spec_id get_prod_spec    (entity_id entity) =>     prod_spec_id_arr       [entity];
            public ref    u16 get_remaining_ref(entity_id entity) => ref prod_remaining_arr.@ref(entity);
            
            public         u8 get_out_count    (spec_id spec    ) =>     _prod_specs.out_resources_arr.counts    [spec];
            public         u8 get_in_count     (spec_id spec    ) =>     _prod_specs. in_resources_arr.counts    [spec];
            public ref load_8 get_in_loads_ref (spec_id spec    ) => ref _prod_specs. in_resources_arr.loads.@ref(spec);
            public ref load_8 get_out_loads_ref(spec_id spec    ) => ref _prod_specs.out_resources_arr.loads.@ref(spec);
            public        u16 get_ticks        (spec_id spec    ) =>     _prod_specs.required_ticks_arr[spec];
        }
    }
}