using System;
using static Hyperway.resource_type;

namespace Hyperway.unity {
    using save = SerializableAttribute;
    
    [save] public struct storage_spec {
        public spec[] specs;
        
        [save] public struct spec {
            public resource_type type;
            public uint capacity;
            public uint amount;
        }

        public batch capacity {
            get {
                var r = new batch();
                var a = specs;
                for (var i = first; i < count; i++)
                for (var j = 0; j < a.Length; j++)
                    r[i] += a[j].type == i ? a[j].capacity : 0;

                return r;
            }
        }

        public batch amount {
            get {
                var r = new batch();
                var a = specs;
                for (var i = first; i < count; i++)
                for (var j = 0; j < a.Length; j++)
                    r[i] += a[j].type == i ? a[j].amount : 0;

                return r;
            }
        }
    }
}