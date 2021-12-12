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
            
            public static readonly storage_spec_id none      = u8_max;
            public static readonly storage_spec_id max_count = u8_max - 1;
            
            public static implicit operator byte(spec_id i) => i.value;
            public static implicit operator spec_id(byte b) => new spec_id {value = b};
        }

        [save] public partial struct storage_specs {
            public byte count;

            [scenario] public u8_arr       slots_count_arr;  // number of storage slots
            [scenario] public filter_8_arr slots_filter_arr; // resource type filter per slot
            [scenario] public u8_8_arr     slots_cap_arr;    // capacity per slot

            public void init() {
                init(ref slots_count_arr);
                init(ref slots_filter_arr);
                init(ref slots_cap_arr );
            }

            void init(ref res_id_8_arr arr) {
                var types = new res_id_8(); types.set(res_id.none);
                init<res_id_8>(ref arr); arr.set(types);
            }

            void init<t>(ref NativeArray<t> arr) where t : struct => arr.init(spec_id.max_count);
            
            public           u8 get_slots_count(spec_id spec_id              ) =>     slots_count_arr[spec_id];
            public ref filter_8 get_filters_ref (spec_id spec_id              ) => ref slots_filter_arr.@ref(spec_id);
            public          u16 get_cap        (spec_id spec_id, slot_id slot) =>     slots_cap_arr[spec_id][slot];
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
            public       filter get_filter     (entity_id id, slot_id slot) =>     get_filters_ref(id)[slot];
            public          u16 get_cap        (entity_id id, slot_id slot) =>     _storage_specs.get_cap        (storage_spec_arr[id], slot);
            public ref   res_id get_type_ref   (entity_id id, slot_id slot) => ref storage_slots_type_arr  .@ref(id).@ref(slot);
            public ref      u16 get_amount_ref (entity_id id, slot_id slot) => ref storage_slots_amount_arr.@ref(id).@ref(slot);
                                
            public u16 get_total_space(entity_id id, res_id res_id) {
                    var   space = (u16) 0;
                    var   count =     get_slots_count(id);
                ref var filters = ref get_filters_ref(id); 

                for (slot_id slot_i = 0; slot_i < count; slot_i++)
                    if (filters[slot_i].matches(res_id)) {
                        var type = get_type_ref(id, slot_i);
                        if (type == res_id.none)
                            space += get_cap(id, slot_i);
                        else if (type == res_id) {
                            var slot_space = get_cap(id, slot_i) - get_amount_ref(id, slot_i);
                            (slot_space > 0).assert();
                            space += (u16)slot_space;
                        }
                    }

                return space;
            }
                                
            public u16 get_total_cap(entity_id id, res_id res_id) {
                    var     cap = (u16)0;
                    var   count =     get_slots_count(id);
                ref var filters = ref get_filters_ref(id); 

                for (slot_id slot_i = 0; slot_i < count; slot_i++)
                    if (filters[slot_i].matches(res_id)) {
                        var type = get_type_ref(id, slot_i);
                        if (type == res_id.none || type == res_id) 
                            cap += get_cap(id, slot_i);
                    }

                return cap;
            }
            
            public u16 get_total_amount(entity_id id, filter filter) {
                var amount = (u16)0;
                var  count = get_slots_count(id);

                for (slot_id slot_i = 0; slot_i < count; slot_i++)
                    if (filter.matches(get_type_ref(id, slot_i)))
                        amount += get_amount_ref(id, slot_i);

                return amount;
            }

            public bool has_space (entity_id id, res_id    res, u16 amount) => amount == 0 || get_total_space (id, res   ) >= amount;
            public bool has_amount(entity_id id, filter filter, u16 amount) => amount == 0 || get_total_amount(id, filter) >= amount;

            public u16 /* remainder */ sub(entity_id id, slot_id slot, filter filter, u16 amount) {
                if (amount != 0) {} else return amount;
                if (filter.matches(get_type_ref(id, slot))) {} else return amount;
                
                ref var amount_ref = ref get_amount_ref(id, slot);
                var new_amount = amount_ref - amount;

                if (new_amount <= 0) {
                    get_type_ref(id, slot) = res_id.none;
                    amount_ref = 0;
                    return (u16)(-new_amount);
                }

                amount_ref = (u16)new_amount;
                return 0;
            }

            public u16 /* overflow */ add(entity_id id, slot_id slot, res_id res_id, u16 amount) {
                if (amount != 0) {} else return amount;
                
                ref var type = ref get_type_ref(id, slot);
                if (type == res_id.none && get_filter(id, slot).matches(res_id)) type = res_id;
                if (type == res_id) {} else return amount;
                
                ref var s = ref get_amount_ref(id, slot);
                var new_amount = s + amount;
                var cap = get_cap(id, slot);
                
                   s = (u16)min(new_amount, cap);
                return (u16)max(0, new_amount - cap);
            }

            public u16 /* overflow */ add(entity_id id, res_id res, u16 amount) {
                if (amount != 0) {} else return 0;
                
                var count = get_slots_count(id);
                for (slot_id slot_i = 0; slot_i < count && amount > 0; slot_i++)
                    amount = add(id, slot_i, res, amount);

                return amount;
            }
            
            public u16 /* remainder */ sub(entity_id id, filter filter, u16 amount) {
                if (amount != 0) {} else return 0;
                
                var count = get_slots_count(id);
                for (slot_id slot_i = 0; slot_i < count && amount > 0; slot_i++)
                    amount = sub(id, slot_i, filter, amount);

                return amount;
            }
            
            public bool try_sub(entity_id id, filter filter, u16 amount) {
                if (amount != 0) {} else return true;
                if (has_amount(id, filter, amount)) {} else return false;
                
                var remainder = sub(id, filter, amount);
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
                    if (get_total_amount(id, load.type) < load.amount)
                        return false;
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

                for (var entity_id = 0; entity_id < count; entity_id++) {
                    var type_id    =  storage_spec_arr[entity_id];
                    var slot_types = _storage_specs.slots_filter_arr[type_id];
                    var slot_count = _storage_specs.slots_count_arr[type_id];
                    for (u8 i = 0; i < slot_count; i++)
                        result[slot_types[i]] += storage_slots_amount_arr[entity_id][i];
                }
            }
        }
    }

    public static class storage_slot_types_ext {
        public static slot_id index_of(this res_id_8 types, res_id res) {
            for (u8 i = 0; i < 8; i++)
                if (types[i] == res)
                    return i;

            return slot_id.none;
        }
    }
}