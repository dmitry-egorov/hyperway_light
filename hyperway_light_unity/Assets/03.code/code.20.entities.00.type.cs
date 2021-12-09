using System;
using Lanski.Utilities.attributes;
using Unity.Collections;
using UnityEngine.Jobs;
using static Unity.Collections.Allocator;
using static Unity.Collections.NativeArrayOptions;
using static Utilities.Collections.arr_ext;

namespace Hyperway {
    using props = entity_type_props;
    public partial struct entity_type {
        public void fields(entity_type_id i) {
            var _ = i.attr<entity_type_spec>();
            capacity = _.capacity;
               props = _.props;
            fields();
        }
        
        public bool all(props check) => props.all(check);
        public bool any(props check) => props.any(check);

        
        public bool req<t1>(props p, ref t1[] a1) { if (props.all(p)) expand(ref a1, capacity); return true; }
        public bool req<t1, t2>(props p, ref t1[] a1, ref t2[] a2) { if (props.all(p)) expand(ref a1, ref a2, capacity); return true; }
        public bool req<t1, t2, t3>(props p, ref t1[] a1, ref t2[] a2, ref t3[] a3) { if (props.all(p)) expand(ref a1, ref a2, ref a3, capacity); return true; }
        public bool req<t1, t2, t3, t4>(props p, ref t1[] a1, ref t2[] a2, ref t3[] a3, ref t4[] a4) { if (props.all(p)) expand(ref a1, ref a2, ref a3, ref a4, capacity); return true; }
        public bool req<t1, t2, t3, t4, t5>(props p, ref t1[] a1, ref t2[] a2, ref t3[] a3, ref t4[] a4, ref t5[] a5) { if (props.all(p)) expand(ref a1, ref a2, ref a3, ref a4, ref a5, capacity); return true; }
        public bool req<t1, t2, t3, t4, t5, t6>(props p, ref t1[] a1, ref t2[] a2, ref t3[] a3, ref t4[] a4, ref t5[] a5, ref t6[] a6) { if (props.all(p)) expand(ref a1, ref a2, ref a3, ref a4, ref a5, ref a6, capacity); return true; }
        
        public bool req<t1>(props p, ref NativeArray<t1> a1) where t1 : struct { if (props.all(p)) init(ref a1); return true; }
        public bool req<t1, t2>(props p, ref NativeArray<t1> a1, ref NativeArray<t2> a2) where t1 : struct where t2 : struct { if (props.all(p)) { init(ref a1, ref a2); } return true; }
        
        public bool req(props p, ref TransformAccessArray a1) { if (props.all(p)) a1 = new TransformAccessArray(capacity); return true; }

        public void init<t1>(ref NativeArray<t1> a1) where t1 : struct { if (a1.IsCreated) { a1.Dispose(); } a1 = new NativeArray<t1>(capacity, Persistent, ClearMemory); }
        public void init<t1, t2>(ref NativeArray<t1> a1, ref NativeArray<t2> a2) where t1 : struct where t2 : struct { init(ref a1); init(ref a2); }

        
        public delegate void action(ref entity_type entity_type);
        public delegate void action<t>(ref t data, ref entity_type entity_type);
    }

    public class entity_type_spec : Attribute {
        public readonly ushort capacity; public readonly props props;
        public entity_type_spec(ushort capacity, props props) { this.capacity = capacity; this.props = props; }
    }
}