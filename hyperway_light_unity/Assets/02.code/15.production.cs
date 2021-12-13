using System;
using Lanski.Plugins.Persistance;
using Lanski.Utilities.assertions;
using Unity.Collections;
using Utilities.Collections;
using static Hyperway.hyperway.prod_spec_id;
using static Hyperway.hyperway.entity_type_props;
using static Hyperway.hyperway;
using static Lanski.Utilities.constants.consts;

namespace Hyperway {
    using spec_id = prod_spec_id;
    using slot_id = storage_slot_id;
    using u16 = UInt16;
    using slot_types_8 = fixed_arr_8<res_id>;
    using arr_rt_8 = NativeArray<fixed_arr_8<res_id>>;
    using save    =  SerializableAttribute;

    using ps_arr   = NativeArray<prod_spec_id>;
    using u16_arr  = NativeArray<ushort>;

    public static partial class hyperway {
        public static prod_specs _prod_specs;

        [save] public partial struct prod_spec_id {
            public byte value;
            
            public static readonly spec_id none     = u8_max;
            public static readonly int    max_count = u8_count;
            
            public static implicit operator byte(spec_id i) => i.value;
            public static implicit operator spec_id(byte b) => new spec_id {value = b};
        }
        
        [save] public partial struct prod_specs {
            public byte count;

            [scenario] public res_multi_8_arr out_resources_arr;
            [scenario] public res_multi_8_arr  in_resources_arr;
            [scenario] public u16_arr    required_ticks_arr;

            public void init() {
                init(ref   in_resources_arr);
                init(ref  out_resources_arr);
                init(ref required_ticks_arr);
            }

            void init<t>(ref NativeArray<t> arr) where t : struct => arr.init(max_count);
            void init   (ref res_multi_8_arr arr) { arr.loads.init(max_count); arr.counts.init(max_count); }
        }
        
        public partial struct entity_type {
            [scenario] public ps_arr  prod_spec_id_arr;             // production spec id
            [savefile] public u16_arr prod_ticks_arr;            // ticks till the production finishes
            
            public void production_fields() => req(produces, ref prod_spec_id_arr, ref prod_ticks_arr);

            public void produce() {
                if (all(produces)) {} else return;

                var    in_arr = _prod_specs.  in_resources_arr;
                var ticks_arr = _prod_specs.required_ticks_arr;
                var   out_arr = _prod_specs. out_resources_arr;

                for (ushort entity = 0; entity < count; entity++) {
                    var spec_ud  = prod_spec_id_arr[entity];
                    if (spec_ud != spec_id.none) {} else continue;

                    ref var remaining = ref prod_ticks_arr.@ref(entity);
                    if (remaining > 1) { // progress production
                        remaining -= 1;
                        continue;
                    }

                        var out_count =     out_arr.counts    [spec_ud];
                    ref var out_loads = ref out_arr.loads.@ref(spec_ud);
                    if (remaining == 1) { // finish production
                        var overflow = add(entity, out_loads, out_count);
                        overflow.all(0).assert();
                        
                        remaining = 0;
                        continue;
                    }

                    (remaining == 0).assert(); { // check required resources and start production
                            var in_count =     in_arr.counts    [spec_ud];
                        ref var in_loads = ref in_arr.loads.@ref(spec_ud);
                        
                        if (has_space(entity, out_loads, out_count)) {} else continue; // don't start production if there's no empty space for the output
                        if (try_sub  (entity,  in_loads,  in_count)) {} else continue; // don't start production if there's not enough resources

                        remaining = ticks_arr[spec_ud];
                    }
                }
            }
        }
    }
}