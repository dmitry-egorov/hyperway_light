using System;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using UnityEngine;
using Utilities.Collections;
using static Lanski.Utilities.constants.consts;
using static Unity.Mathematics.math;
using static Hyperway.hyperway;
using static Hyperway.hyperway.entity_type_props;

namespace Hyperway {
    using save    =  SerializableAttribute;
    using name    = InspectorNameAttribute;
    
    using out_of_range = ArgumentOutOfRangeException;
    
    using slot_types_8 = fixed_arr_8<res_id>;
    using   res_load_8 = fixed_arr_8<res_load<ushort>>;
    using      slot_id = storage_slot_id;
    using      spec_id = storage_spec_id;
    
    using res_id_8_arr = NativeArray<fixed_arr_8<res_id>>;
    using u8_8_arr = NativeArray<fixed_arr_8<ushort>>;
    using ss_arr   = NativeArray<storage_spec_id>;

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

            [scenario] public u8_arr             slot_count_arr; // number of inventory slots
            [scenario] public res_id_8_arr   slots_type_arr; // type of resource per slot
            [scenario] public u8_8_arr slots_cap_arr;  // capacity         per slot

            public void init() {
                init(ref slot_count_arr);
                init(ref slots_type_arr);
                init(ref slots_cap_arr );
            }

            void init(ref res_id_8_arr arr) {
                var types = new slot_types_8(); types.set(hyperway.res_id.none);
                init<slot_types_8>(ref arr); arr.set(types);
            }

            void init<t>(ref NativeArray<t> arr) where t : struct => arr.init(spec_id.max_count);
            
            public slot_id slot_id_of  (spec_id spec_id, res_id res) => slots_type_arr[spec_id].index_of(res);
            public     u16 get_capacity(spec_id spec_id, res_id res) => slots_cap_arr [spec_id][slot_id_of(spec_id, res)];
        }
        
        public partial struct entity_type {
            [scenario] public ss_arr   storage_spec_arr;         // storage spec id
            [savefile] public u8_8_arr storage_slots_amount_arr; // amount of stored resources per slot

            public void storage_fields() {
                req(stores, ref storage_spec_arr, ref storage_slots_amount_arr);
            }
            
            public    slot_id slot_id_of  (entity_id entity_id, res_id res) => _storage_specs.slot_id_of(storage_spec_arr[entity_id], res);
            public ref ushort stored      (entity_id entity_id, res_id res) => ref storage_slots_amount_arr.@ref(entity_id).@ref(slot_id_of(entity_id, res));
            public     ushort get_capacity(entity_id entity_id, res_id res) => _storage_specs.get_capacity(storage_spec_arr[entity_id], res);

            public bool has_space_for(entity_id entity_id, res_id res, u16 amount) => amount == 0 || stored(entity_id, res) + amount <= get_capacity(entity_id, res);
            public bool has_amount   (entity_id entity_id, res_id res, u16 amount) => amount == 0 || stored(entity_id, res) >= amount;
                                     
            public void add_overflow (entity_id entity_id, res_id res, u16 amount) { { if (amount == 0) return; } storage_slots_amount_arr.@ref(entity_id).@ref(slot_id_of(entity_id, res)) += amount; }

            public void add(entity_id entity_id, res_id res, u16 amount) { { if (amount == 0) return; } ref var s = ref stored(entity_id, res); s = (u16)min(s + amount, get_capacity(entity_id, res));}
            public void sub(entity_id entity_id, res_id res, u16 amount) { { if (amount == 0) return; } ref var s = ref stored(entity_id, res); s = (u16)max(s - amount, 0); }

            public bool try_sub(entity_id entity_id, res_id res, u16 amount) {
                { if (amount == 0) return true; }
                
                ref var stored = ref this.stored(entity_id, res);
                var new_stored = stored - amount;
                if (new_stored < 0)
                    return false;
                stored = (u16)new_stored;
                return true;
            }

            public bool has_space_for(entity_id entity_id, res_load<u16> load) => has_space_for(entity_id, load.type, load.amount);
            public bool has_amount   (entity_id entity_id, res_load<u16> load) => has_amount   (entity_id, load.type, load.amount);
            public void add_overflow (entity_id entity_id, res_load<u16> load) => add_overflow (entity_id, load.type, load.amount);
            public void add          (entity_id entity_id, res_load<u16> load) => add          (entity_id, load.type, load.amount);
            public void sub          (entity_id entity_id, res_load<u16> load) => sub          (entity_id, load.type, load.amount);
            public bool try_sub      (entity_id entity_id, res_load<u16> load) => try_sub      (entity_id, load.type, load.amount);
                                     
            public bool has_space_for(entity_id entity_id, in res_load_8 load, byte count) { var r = true; for (byte i = 0; i < count; i++) r = r && has_space_for(entity_id, load[i]); return r; }
            public bool has_amount   (entity_id entity_id, in res_load_8 load, byte count) { var r = true; for (byte i = 0; i < count; i++) r = r && has_amount   (entity_id, load[i]); return r; }
            public void add_overflow (entity_id entity_id, in res_load_8 load, byte count) { for (byte i = 0; i < count; i++) add_overflow (entity_id, load[i]); }
            public void add          (entity_id entity_id, in res_load_8 load, byte count) { for (byte i = 0; i < count; i++) add          (entity_id, load[i]); }
            public void sub          (entity_id entity_id, in res_load_8 load, byte count) { for (byte i = 0; i < count; i++) sub          (entity_id, load[i]); }
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
                    var slot_types = _storage_specs.slots_type_arr[type_id];
                    var slot_count = _storage_specs.slot_count_arr[type_id];
                    for (byte i = 0; i < slot_count; i++)
                        result[slot_types[i]] += storage_slots_amount_arr[entity_id][i];
                }
            }
        }
    }

    public static class storage_slot_types_ext {
        public static storage_slot_id index_of(this slot_types_8 types, res_id res) {
            for (u8 i = 0; i < 8; i++) {
                if (types[i] == res)
                    return i;
            }

            return storage_slot_id.none;
        }
    }
}