using System;
using Lanski.Plugins.Persistance;
using Lanski.Utilities.assertions;
using Unity.Collections;
using UnityEngine;
using Utilities.Collections;
using static Lanski.Utilities.constants.consts;
using static Unity.Mathematics.math;
using static Hyperway.hyperway;
using static Hyperway.hyperway.entity_type_props;

namespace Hyperway {
    using save =  SerializableAttribute;
    using name = InspectorNameAttribute;
    
    using out_of_range = ArgumentOutOfRangeException;
    
    using slot_id_8 = fixed_arr_8<storage_slot_id>;
    using  filter_8 = fixed_arr_8<res_filter>;
    using  res_id_8 = fixed_arr_8<res_id>;
    using      u8_8 = fixed_arr_8<ushort>;
    using    load_8 = fixed_arr_8<res_load<ushort>>;
    using   slot_id = storage_slot_id;
    using   spec_id = storage_spec_id;
    using    filter = res_filter;

    using res_id_8_arr = NativeArray<fixed_arr_8<res_id>>;
    using filter_8_arr = NativeArray<fixed_arr_8<res_filter>>;
    using u8_8_arr     = NativeArray<fixed_arr_8<ushort>>;
    using spec_arr     = NativeArray<storage_spec_id>;

    using  u8_arr = NativeArray<byte>;
    using u32_arr = NativeArray<uint>;

    using u8  = Byte;
    using u16 = UInt16;

    public static partial class hyperway {
        public static storage_specs _storage_specs;

        [save] public partial struct storage_spec_id {
            public byte value;
            
            public static readonly spec_id none      = u8_max;
            public static readonly     int max_count = u8_count;
            
            public static implicit operator byte(spec_id i) => i.value;
            public static implicit operator spec_id(byte b) => new spec_id {value = b};
        }

        [save] public partial struct storage_specs {
            public byte count;

            [scenario] public u8_arr       slots_count_arr;  // number of storage slots
            [scenario] public filter_8_arr slots_filter_arr; // resource type filter per slot
            [scenario] public u8_8_arr     slots_cap_arr;    // capacity per slot

            public void init() {
                var max = spec_id.max_count;
                slots_count_arr .init(max);
                slots_filter_arr.init(max);
                slots_cap_arr   .init(max);
            }

            public           u8 get_slots_count(spec_id spec_id              ) =>     slots_count_arr      [spec_id];
            public ref filter_8 get_filters_ref(spec_id spec_id              ) => ref slots_filter_arr.@ref(spec_id);
            public       filter get_filter     (spec_id spec_id, slot_id slot) =>     slots_filter_arr.@ref(spec_id)[slot];
            public          u16 get_cap        (spec_id spec_id, slot_id slot) =>     slots_cap_arr   .@ref(spec_id)[slot];
        }
        
        public partial struct entity_type {
            [scenario] public spec_arr     storage_spec_arr;         // storage spec id
            [savefile] public res_id_8_arr storage_slots_type_arr;   //   type of stored resource per slot
            [savefile] public u8_8_arr     storage_slots_amount_arr; // amount of stored resource per slot

            public void storage_fields() {
                req(stores, ref storage_spec_arr, ref storage_slots_type_arr, ref storage_slots_amount_arr);
            }
            
            public           u8 get_slots_count(entity_id id              ) =>     _storage_specs.get_slots_count(storage_spec_arr[id]);
            public ref filter_8 get_filters_ref(entity_id id              ) => ref _storage_specs.get_filters_ref(storage_spec_arr[id]);
            public       filter get_filter     (entity_id id, slot_id slot) =>     _storage_specs.get_filter     (storage_spec_arr[id], slot);
            public          u16 get_cap        (entity_id id, slot_id slot) =>     _storage_specs.get_cap        (storage_spec_arr[id], slot);
            public ref   res_id get_res_ref    (entity_id id, slot_id slot) => ref storage_slots_type_arr  .@ref(id).@ref(slot);
            public       res_id get_res        (entity_id id, slot_id slot) =>     storage_slots_type_arr  .@ref(id)[slot];
            public ref      u16 get_amount_ref (entity_id id, slot_id slot) => ref storage_slots_amount_arr.@ref(id).@ref(slot);
            public          u16 get_amount     (entity_id id, slot_id slot) =>     storage_slots_amount_arr.@ref(id)[slot];
                                
            public u16 get_total_space(entity_id id, res_id res) {
                (res != res_id.none).assert();
                
                    var   space = (u16) 0;
                    var   count =     get_slots_count(id);
                ref var filters = ref get_filters_ref(id); 

                for (slot_id slot_i = 0; slot_i < count; slot_i++)
                    if (filters[slot_i].matches(res)) {
                        var type = get_res(id, slot_i);
                        if (type == res_id.none)
                            space += get_cap(id, slot_i);
                        else if (type == res) {
                            var slot_space = get_cap(id, slot_i) - get_amount(id, slot_i);
                            (slot_space > 0).assert();
                            space += (u16)slot_space;
                        }
                    }

                return space;
            }
                                
            public u16 get_total_cap(entity_id id, res_id res) {
                (res != res_id.none).assert();
                
                    var     cap = (u16)0;
                    var   count =     get_slots_count(id);
                ref var filters = ref get_filters_ref(id); 

                for (slot_id slot_i = 0; slot_i < count; slot_i++)
                    if (filters[slot_i].matches(res)) {
                        var type = get_res(id, slot_i);
                        if (type == res_id.none || type == res) 
                            cap += get_cap(id, slot_i);
                    }

                return cap;
            }
            
            public u16 get_total_amount(entity_id id, filter filter) {
                (filter != filter.none).assert();
                
                var amount = (u16)0;
                var count  = get_slots_count(id);

                for (slot_id slot_i = 0; slot_i < count; slot_i++)
                    if (filter.matches(get_res(id, slot_i)))
                        amount += get_amount(id, slot_i);

                return amount;
            }

            public bool has_space (entity_id id, res_id res, u16 amount) {
                (amount != 0 && res != res_id.none).assert();
                
                return get_total_space(id, res) >= amount;
            }

            public bool has_amount(entity_id id, filter filter, u16 amount) {
                (amount != 0 && filter != filter.none).assert();
                
                return get_total_amount(id, filter) >= amount;
            }

            public res_id try_find(entity_id id, filter filter) {
                (filter != filter.none).assert();
                
                var count = get_slots_count(id);
                for (slot_id slot_i = 0; slot_i < count; slot_i++) {
                    var type = get_res(id, slot_i);
                    if (filter.matches(type))
                        return type;
                }

                return res_id.none;
            }

            public u16 /* remainder */ sub(entity_id id, slot_id slot, res_id res, u16 amount) {
                (amount != 0 && res != res_id.none).assert();

                ref var res_ref = ref get_res_ref(id, slot);
                if (res_ref == res) {} else return amount;
                
                ref var amount_ref = ref get_amount_ref(id, slot);
                var new_amount = amount_ref - amount;

                if (new_amount <= 0) {
                    res_ref = res_id.none;
                    amount_ref = 0;
                    return (u16)(-new_amount);
                }

                amount_ref = (u16)new_amount;
                return 0;
            }

            public u16 /* overflow */ add(entity_id id, slot_id slot, res_id res, u16 amount) {
                (amount != 0 && res != res_id.none).assert();
                
                ref var res_ref = ref get_res_ref(id, slot);
                if (res_ref == res_id.none && get_filter(id, slot).matches(res)) res_ref = res;
                if (res_ref == res) {} else return amount;
                
                ref var amount_ref = ref get_amount_ref(id, slot);
                var new_amount = amount_ref + amount;
                var cap = get_cap(id, slot);
                
                amount_ref = (u16)min(new_amount, cap);
                return (u16)max(0, new_amount - cap);
            }

            public u16 /* overflow */ add(entity_id id, res_id res, u16 amount) {
                (amount != 0 && res != res_id.none).assert();
                
                var count = get_slots_count(id);
                for (slot_id slot_i = 0; slot_i < count && amount > 0; slot_i++)
                    amount = add(id, slot_i, res, amount);

                return amount;
            }
            
            public u16 /* remainder */ sub(entity_id id, res_id res, u16 amount) {
                (amount != 0 && res != res_id.none).assert();
                
                var count = get_slots_count(id);
                for (slot_id slot_i = 0; slot_i < count && amount > 0; slot_i++)
                    amount = sub(id, slot_i, res, amount);

                return amount;
            }
            
            public bool try_sub(entity_id id, res_id res, u16 amount) {
                (amount != 0 && res != res_id.none).assert();
                
                if (has_amount(id, res, amount)) {} else return false;
                
                var remainder = sub(id, res, amount);
                (remainder == 0).assert();
                return true;
            }

            public bool         has_space (entity_id id, res_load<u16> load) => has_space (id, load.type, load.amount);
            public bool         has_amount(entity_id id, res_load<u16> load) => has_amount(id, load.type, load.amount);
            public u16 /* overflow  */ add(entity_id id, res_load<u16> load) => add       (id, load.type, load.amount);
            public u16 /* remainder */ sub(entity_id id, res_load<u16> load) => sub       (id, load.type, load.amount);
            public bool            try_sub(entity_id id, res_load<u16> load) => try_sub   (id, load.type, load.amount);

            public bool          has_space (entity_id id, in load_8 load_8, u8 count) { var r = true; for (u8 i = 0; i < count; i++) r = r && has_space(id, load_8[i]); return r; }
            public bool          has_amount(entity_id id, in load_8 load_8, u8 count) { var r = true; for (u8 i = 0; i < count; i++) r = r && has_amount(id, load_8[i]); return r; }
            public u8_8 /* overflow  */ add(entity_id id, in load_8 load_8, u8 count) { var o = new u8_8(); for (u8 i = 0; i < count; i++) o[i] = add(id, load_8[i]); return o; }
            public u8_8 /* remainder */ sub(entity_id id, in load_8 load_8, u8 count) { var r = new u8_8(); for (u8 i = 0; i < count; i++) r[i] = sub(id, load_8[i]); return r; }
            
            public bool try_sub(entity_id id, in load_8 load_8, u8 count) { // find slots of all required resources, sub amounts if successful
                for (u8 i = 0; i < count; i++) {
                    var load = load_8[i];
                    if (has_amount(id, load.type, load.amount)) {} else return false;
                }

                sub(id, load_8, count);
                return true;
            }
        }

        public struct storage_slot_id {
            public u8 value;
            public static readonly u8 none = u8_max; 
            
            public static implicit operator storage_slot_id(u8 v) => new storage_slot_id { value = v };
            public static implicit operator u8(storage_slot_id e) => e.value;
        }

        public partial struct stats {
            public u32_arr total_stored;
            
            void init_stored() {
                total_stored.init(res_id.max_count);
            }

            void count_stored_resources() {
                total_stored.clear();
                _entities.for_each(ref total_stored, (ref u32_arr result, ref entity_type type) => type.add_resource_amounts(ref result));
            }
        }

        public partial struct entity_type {
            public void add_resource_amounts(ref u32_arr result) {
                if (props.all(stores)) {} else return;

                for (entity_id entity = 0; entity < count; entity++) {
                    var slot_count = get_slots_count(entity);
                    for (slot_id slot = 0; slot < slot_count; slot++) 
                        result[get_res(entity, slot)] += get_amount(entity, slot);
                }
            }
        }
    }
}