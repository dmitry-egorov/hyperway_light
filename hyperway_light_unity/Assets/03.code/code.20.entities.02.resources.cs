using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using static Hyperway.res_type;
using static Hyperway.res_type_ext;
using static Unity.Mathematics.math;

namespace Hyperway {
    using out_of_range = ArgumentOutOfRangeException;
    public partial struct entity_resources {
        public entity_resources(int count) => data = new entities_resource_data[count];
        public ref entities_resource_data this[res_type t] => ref data[(int)t];
    }

    public partial struct entities_resource_data {
        public void sub(ushort entity, ushort amount) => stored_arr[entity] -= min(stored_arr[entity], amount); // since we use unsigned integers, this is the way to prevent overflow

        public bool try_sub(ushort entity, ushort amount) {
            if (stored_arr[entity] >= amount) {} else return false;
            stored_arr[entity] -= amount;
            return true;
        }
    }

    public partial struct entity_type {
        public uint get_stored  (ushort entity_id, res_type res_type             ) => resources[res_type].stored_arr[entity_id];
        public uint set_stored  (ushort entity_id, res_type res_type, uint amount) => resources[res_type].stored_arr[entity_id] = amount;
        public uint get_capacity(ushort entity_id, res_type res_type             ) => buildings.get_capacity(building_type_arr[entity_id], res_type);

        public bool is_not_over_cap(ushort entity_id, res_type res_type) => get_stored(entity_id, res_type) <= get_capacity(entity_id, res_type);

        public bool has_space_for(ushort entity_id, res_type res_type, ushort amount) => get_stored(entity_id, res_type) + amount <= get_capacity(entity_id, res_type);
        public bool has_amount   (ushort entity_id, res_type res_type, ushort amount) => get_stored(entity_id, res_type) >= amount;
        
        public void add_overflow (ushort entity_id, res_type res_type, ushort amount) => resources[res_type].stored_arr[entity_id] += amount;
        public void add          (ushort entity_id, res_type res_type, ushort amount) => set_stored(entity_id, res_type, min(get_stored(entity_id, res_type) + amount, get_capacity(entity_id, res_type)));
        public void sub          (ushort entity_id, res_type res_type, ushort amount) => resources[res_type].sub(entity_id, amount);

        public bool try_sub      (ushort entity_id, res_type res_type, ushort amount) => resources[res_type].try_sub(entity_id, amount);
        
        public bool has_space_for(ushort entity_id, batch_u16 batch) => has_space_for(entity_id, batch.type, batch.amount);
        public bool has_amount   (ushort entity_id, batch_u16 batch) => has_amount   (entity_id, batch.type, batch.amount);
        public void add_overflow (ushort entity_id, batch_u16 batch) => add_overflow (entity_id, batch.type, batch.amount);
        public void add          (ushort entity_id, batch_u16 batch) => add          (entity_id, batch.type, batch.amount);
        public void sub          (ushort entity_id, batch_u16 batch) => sub          (entity_id, batch.type, batch.amount);
        public bool try_sub      (ushort entity_id, batch_u16 batch) => try_sub      (entity_id, batch.type, batch.amount);
        
        public bool has_space_for(ushort entity_id, batch2_u16 batch) => has_space_for(entity_id, batch.b0) && has_space_for(entity_id, batch.b1);
        public bool has_amount   (ushort entity_id, batch2_u16 batch) => has_amount   (entity_id, batch.b0) && has_amount   (entity_id, batch.b1);
        public void add_overflow (ushort entity_id, batch2_u16 batch)  { add_overflow (entity_id, batch.b0);   add_overflow (entity_id, batch.b1); }
        public void add          (ushort entity_id, batch2_u16 batch)  { add          (entity_id, batch.b0);   add          (entity_id, batch.b1); }
        public void sub          (ushort entity_id, batch2_u16 batch)  { sub          (entity_id, batch.b0);   sub          (entity_id, batch.b1); }
        
        // todo: need to sub from both at the same time (not used yet)
        // public bool try_sub      (ushort entity, batch2_u16 batch)  { try_sub      (entity, batch.b0);   try_sub      (entity, batch.b1); }
    }

    public partial struct batch2_u16 {
        public batch_u16 this[int i] => i switch { 0 => b0, 1 => b1, _ => throw new out_of_range(nameof(i), i, null) };
    }

    public partial struct fixed_batch {
        public unsafe ref uint this[     int t]  { get { check(t); return ref amounts[t]; } }
        public        ref uint this[res_type t] => ref this[(int)t];

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [Conditional("UNITY_EDITOR")]
        static void check(int index) {
            if (index < 0 || index >= (int)count) {
                throw new IndexOutOfRangeException();
            }
        }

        public void reset() {
            foreach (var t in all_res_types)
                this[t] = 0;
        }

        public static fixed_batch operator +(fixed_batch b1, fixed_batch b2) {
            var r = new fixed_batch();
            for (res_type t = 0; t < count; t++) 
                r[t] = b1[t] + b2[t];
            return r;
        }

        public static implicit operator fixed_batch(batch_u16[] b) {
            var r = new fixed_batch();
            for (var i = 0; i < b.Length; i++)
                r[b[i].type] += b[i].amount;
            return r;
        }
    }

    public static class res_type_ext {
        public static enumerable all_res_types => new enumerable();

        public struct enumerable : IEnumerable<res_type> {
            public enumerator GetEnumerator() => new enumerator { t = unchecked((res_type)0 - 1) };
            
            IEnumerator<res_type> IEnumerable<res_type>.GetEnumerator() => GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public struct enumerator : IEnumerator<res_type> {
                public res_type t;
                
                public res_type Current => t;
                public bool MoveNext() { t++; return t < count; }
                public void Reset() => t = unchecked((res_type)0 - 1);
                
                object IEnumerator.Current => Current;
                public void Dispose() { }
            }
        }
    }
}