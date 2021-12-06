using System;
using Lanski.Utilities.attributes;
using static Utilities.Collections.arr_ext;

namespace Hyperway {
    public partial struct entity_type {
        public void req(type i) {
            var _ = i.attr<spec>();
            capacity = _.capacity;
               props = _.props;
            fields();
        }
        
        bool req<t1>(properties p, ref t1[] a1) { if (props.all(p)) expand(ref a1, capacity); return true; }
        bool req<t1, t2>(properties p, ref t1[] a1, ref t2[] a2) { if (props.all(p)) expand(ref a1, ref a2, capacity); return true; }
        bool req<t1, t2, t3>(properties p, ref t1[] a1, ref t2[] a2, ref t3[] a3) { if (props.all(p)) expand(ref a1, ref a2, ref a3, capacity); return true; }
        bool req<t1, t2, t3, t4>(properties p, ref t1[] a1, ref t2[] a2, ref t3[] a3, ref t4[] a4) { if (props.all(p)) expand(ref a1, ref a2, ref a3, ref a4, capacity); return true; }
        bool req<t1, t2, t3, t4, t5>(properties p, ref t1[] a1, ref t2[] a2, ref t3[] a3, ref t4[] a4, ref t5[] a5) { if (props.all(p)) expand(ref a1, ref a2, ref a3, ref a4, ref a5, capacity); return true; }
        bool req<t1, t2, t3, t4, t5, t6>(properties p, ref t1[] a1, ref t2[] a2, ref t3[] a3, ref t4[] a4, ref t5[] a5, ref t6[] a6) { if (props.all(p)) expand(ref a1, ref a2, ref a3, ref a4, ref a5, ref a6, capacity); return true; }
        
        public delegate void action(ref entity_type entity_type);

        class spec : Attribute {
            public readonly ushort capacity; public readonly properties props;
            public spec(ushort capacity, properties props) { this.capacity = capacity; this.props = props; }
        }
    }
}