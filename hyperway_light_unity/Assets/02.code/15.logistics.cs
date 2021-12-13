using System;
using Common.spaces;
using Lanski.Plugins.Persistance;
using Lanski.Utilities.assertions;
using Unity.Collections;
using Utilities.Collections;
using static Hyperway.hyperway;
using static Hyperway.hyperway.entity_type_props;
using static UnityEngine.GUILayout;

namespace Hyperway {
    using u8 = Byte; using u16 = UInt16;
    using bit_arr = NativeBitArray; using u8_arr = NativeArray<byte>; using u16_arr = NativeArray<ushort>;
    
    using load = res_load<ushort>;
    using entity_arr = NativeArray<entity_id>;
    using search     = entity_type.warehouse_search_data;

    public static partial class hyperway {
        public static logistics _logistics;

        public partial struct logistics { 
            [config] public u16 teleport_cooldown;
        }
        
        public partial struct entity_type {
            [savefile] public bit_arr   export_scheduled_arr;
            [savefile] public u16_arr teleport_remaining_arr;

            public void logistics_fields() {
                req(produces, ref export_scheduled_arr); 
                req(produces, ref teleport_remaining_arr);
            }

            public void inspect_logistics(entity_id id) {
                Label("Logistics");
                draw(nameof(export_scheduled_arr),   export_scheduled_arr,   id);
                draw(nameof(teleport_remaining_arr), teleport_remaining_arr, id);
            }

            public void send_products() {
                if (all(produces)) { } else return;

                for (entity_id producer = 0; producer < count; producer++) {
                    // tick teleport remaining
                    ref var remaining = ref get_teleport_remaining_ref(producer);
                    if (remaining > 0) remaining -= 1;
                    
                    // export resources
                    if (remaining == 0 && export_is_scheduled(producer)) {} else continue;

                        var spec  =     get_prod_spec(producer);
                        var count =     get_out_count    (spec);
                    ref var loads = ref get_out_loads_ref(spec);

                    var all_products_sent = true;
                    for (u8 load_i = 0; load_i < count; load_i++) {
                        const byte batch_amount = 1;
                        
                        var load = loads[load_i];
                        load.amount = batch_amount;
                        if (has_amount(producer, load)) {} else continue; // resource not found
                        if (try_find_closest_warehouse_with_space_for(load, get_position(producer), out var warehouse)) {} else { all_products_sent = false; continue; } // warehouse with free space not found

                        var remainder = sub(producer , load);
                        var overflow  = add(warehouse, load);
                        (remainder == 0 && overflow == 0).assert();
                        
                        trigger_teleport_cooldown(producer);
                        
                        all_products_sent = load_i == count - 1; // last product was sent
                        break;
                    }
                    
                    if (all_products_sent)
                        reset_export(producer);
                }
            }

            public struct warehouse_search_data {
                public load load;
                public point2 source_position;
                public float min_dist_sq;
                public remote_entity_id result;
                public bool found;

                public warehouse_search_data(load load, point2 source_position) : this() {
                    this.load = load;
                    this.source_position = source_position;
                    
                    min_dist_sq = float.MaxValue;
                }
            }

            public bool try_find_closest_warehouse_with_space_for(load load, point2 source_position, out remote_entity_id result) {
                var search = new search(load, source_position);
                _entities.for_each(ref search, (ref search data, ref entity_type _) => _.find_closest_warehouse_with_space_for(ref data));

                if (search.found) {} else { result = default; return false; }
                { result = search.result; return true; }
            }

            public void find_closest_warehouse_with_space_for(ref search data) {
                if (all(accepts)) {} else return;

                for (entity_id warehouse = 0; warehouse < count; warehouse++) {
                    var pos = get_position(warehouse);

                    var distance_sq = data.source_position.distance_sq_to(pos);
                    if (distance_sq < data.min_dist_sq)  {} else continue; // too far
                    if (has_space(warehouse, data.load)) {} else continue; // no space

                    data.min_dist_sq = distance_sq;
                    data.result = remote_from(warehouse);
                    data.found  = true;
                }
            }

            ref u16 get_teleport_remaining_ref(entity_id entity) => ref teleport_remaining_arr.@ref(entity);
            void    trigger_teleport_cooldown (entity_id entity) => teleport_remaining_arr[entity] = _logistics.teleport_cooldown;
            
            bool export_is_scheduled(entity_id entity) => export_scheduled_arr.IsSet(entity);
            void schedule_export    (entity_id entity) => export_scheduled_arr.Set(entity, true);
            void reset_export       (entity_id entity) => export_scheduled_arr.Set(entity, false);
        }
    }
}