using System;
using System.Runtime.CompilerServices;
using Lanski.Plugins.Persistance;
using Lanski.Utilities.attributes;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using Utilities.Collections;
using Utilities.Strings;
using static Hyperway.hyperway;
using static Hyperway.hyperway.entity_type_id;
using static Hyperway.hyperway.entity_type;
using static Hyperway.hyperway.entity_type_props;
using static Utilities.Profiling.profiler_ex;
using static Utilities.Collections.arr_ext;
using static Lanski.Utilities.constants.consts;

namespace Hyperway {
    using ln = CallerLineNumberAttribute;
    
    using props = entity_type_props;
    using spec  = entity_type_spec;
    using trans_arr = TransformAccessArray;

    using save =  SerializableAttribute;
    using bits =         FlagsAttribute;
    using name = InspectorNameAttribute;

    using u16 = UInt16;


    public static partial class hyperway {
        public static entities _entities;

        public partial struct remote_entity_id {
            public entity_type_id type_id;
            public entity_id id;
        }

        public partial struct entity_id {
            public u16 value;

            public static implicit operator entity_id(u16 v) => new entity_id { value = v };
            public static implicit operator u16(entity_id e) => e.value;
        }

        public enum entity_type_id: byte {
            [spec(u16_max, rendered               )] scenery   = 0,
            [spec(u16_max, rendered | moves       )] figure    = 1,
            [spec(    256, rendered | produces    )] producer  = 2,
            [spec(    256, rendered | houses      )] house     = 3, //TODO: house with production
            [spec(     64, rendered | accepts)] warehouse = 4,

            [name(null)] count
        }

        [save, bits] public enum entity_type_props: uint {
            none         = 0,

            positioned = 1 << 0,
            moves      = 1 << 1 | positioned,
            stores     = 1 << 2,
            produces   = 1 << 3 | stores | positioned,
            houses     = 1 << 4 | stores,
            accepts    = 1 << 5 | stores | positioned, // accepts resources

            rendered   = 1 << 16 | positioned,
        }

        [save] public partial struct entities {
            public entity_type[] type_arr;

            public ref entity_type this[entity_type_id i] => ref type_arr[(int)i];

            public void init() {
                type_arr = new entity_type[(int)count];
                for (entity_type_id i = 0; i < count; i++) {
                    this[i].name = Enum.GetName(typeof(entity_type_id), i);
                    this[i].fields(i);
                    this[i].id = i;
                }
            }
            
            public void start() {
                for_each((ref entity_type _) => _.set_initial_transform());
            }

            public void update_simulation() {
                for_each((ref entity_type _) => _.produce());
                for_each((ref entity_type _) => _.send_products());
                for_each((ref entity_type _) => _.update_hunger());

                for_each((ref entity_type _) => _.remember_prev_positions());
                for_each((ref entity_type _) => _.apply_velocities());
            }

            public void update_visualisation() {
                for_each((ref entity_type _) => _.update_transform());
            }

            public void for_each(action action, [ln] int ln = 0) {
                using var _ = profile(ln.cached_to_string());

                for (var type_i = 0; type_i < type_arr.Length; type_i++) {
                    ref var type = ref type_arr[type_i];
                    if (type.count != 0) { } else continue;
                    action(ref type);
                }
            }

            public void for_each<t>(ref t data, action<t> action, [ln] int ln = 0) {
                using var _ = profile(ln.cached_to_string());

                for (var type_i = 0; type_i < type_arr.Length; type_i++) {
                    ref var type = ref type_arr[type_i];
                    if (type.count != 0) { } else continue;
                    action(ref data, ref type);
                }
            }
        }

        [save] public partial struct entity_type {
            [permanent] public entity_type_id id;
            [permanent] public string name;
            [permanent] public props props;
            [permanent] public   u16 capacity;
            
            [savefile] public u16 count;

            public void fields(entity_type_id i) {
                var _ = i.attr<spec>();
                capacity = _.capacity;
                   props = _.props;
                   
                  position_fields();
                 rendering_fields();
                  movement_fields();
                   storage_fields();
                production_fields();
                 logistics_fields();
                    family_fields();
                    hunger_fields();
            }
            
            public bool all(props check) => props.all(check);
            public bool any(props check) => props.any(check);

            public ref entity_type get_type_ref(entity_type_id id) => ref _entities[id];
            public remote_entity_id remote_from(entity_id id) => new remote_entity_id {type_id = this.id, id = id};

            public bool req<t1>(props p, ref t1[] a1) { if (props.all(p)) expand(ref a1, capacity); return true; }
            public bool req<t1, t2>(props p, ref t1[] a1, ref t2[] a2) { if (props.all(p)) expand(ref a1, ref a2, capacity); return true; }
            public bool req<t1, t2, t3>(props p, ref t1[] a1, ref t2[] a2, ref t3[] a3) { if (props.all(p)) expand(ref a1, ref a2, ref a3, capacity); return true; }
            public bool req<t1, t2, t3, t4>(props p, ref t1[] a1, ref t2[] a2, ref t3[] a3, ref t4[] a4) { if (props.all(p)) expand(ref a1, ref a2, ref a3, ref a4, capacity); return true; }
            public bool req<t1, t2, t3, t4, t5>(props p, ref t1[] a1, ref t2[] a2, ref t3[] a3, ref t4[] a4, ref t5[] a5) { if (props.all(p)) expand(ref a1, ref a2, ref a3, ref a4, ref a5, capacity); return true; }
            public bool req<t1, t2, t3, t4, t5, t6>(props p, ref t1[] a1, ref t2[] a2, ref t3[] a3, ref t4[] a4, ref t5[] a5, ref t6[] a6) { if (props.all(p)) expand(ref a1, ref a2, ref a3, ref a4, ref a5, ref a6, capacity); return true; }
            
            public bool req<t1>(props p, ref NativeArray<t1> a1) where t1 : struct { if (props.all(p)) init(ref a1); return true; }
            public bool req<t1, t2>(props p, ref NativeArray<t1> a1, ref NativeArray<t2> a2) where t1 : struct where t2 : struct { if (props.all(p)) { init(ref a1, ref a2); } return true; }
            public bool req<t1, t2, t3>(props p, ref NativeArray<t1> a1, ref NativeArray<t2> a2, ref NativeArray<t3> a3) where t1 : struct where t2 : struct where t3 : struct { if (props.all(p)) { init(ref a1, ref a2, ref a3); } return true; }
            
            public bool req(props p, ref NativeBitArray a1) { if (props.all(p)) init(ref a1); return true; }
            public bool req(props p, ref trans_arr a1) { if (props.all(p)) init(ref a1); return true; }

            public void init(ref trans_arr a1) => a1.init(capacity);
            public void init(ref NativeBitArray a1) => a1.init(capacity);
            public void init<t1>(ref NativeArray<t1> a1) where t1 : struct => a1.init(capacity);
            public void init<t1, t2>(ref NativeArray<t1> a1, ref NativeArray<t2> a2) where t1 : struct where t2 : struct { init(ref a1); init(ref a2); }
            public void init<t1, t2, t3>(ref NativeArray<t1> a1, ref NativeArray<t2> a2, ref NativeArray<t3> a3) where t1 : struct where t2 : struct where t3 : struct { init(ref a1); init(ref a2); init(ref a3); }

            public delegate void action   (            ref entity_type entity_type);
            public delegate void action<t>(ref t data, ref entity_type entity_type);
        }

        public class entity_type_spec : Attribute {
            public readonly u16 capacity; public readonly props props;
            public entity_type_spec(u16 capacity, props props) { this.capacity = capacity; this.props = props; }
        }
    }
        
    public static class entity_type_props_ext {
        public static bool all(this props src, props check) => (src & check) == check;
        public static bool any(this props src, props check) => (src & check) != 0;
    }
}