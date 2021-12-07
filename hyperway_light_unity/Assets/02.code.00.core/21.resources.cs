using System;
using System.Diagnostics;
using static Hyperway.resource_type;

namespace Hyperway {
    using type = resource_type;

    public partial struct batch {
        public unsafe uint this[resource_type t] {
            get { var i = (int)t; check(i); return amounts[i]; }
            set { var i = (int)t; check(i); amounts[i] = value; }
        }
        
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [Conditional("UNITY_EDITOR")]
        static void check(int index) {
            if (index < 0 || index >= (int)count) {
                throw new IndexOutOfRangeException();
            }
        }

        public void reset() {
            for (var i = first; i < count; i++) this[i] = 0;
        }

        public static batch operator +(batch b1, batch b2) {
            var r = new batch();
            for (var i = first; i < count; i++) r[i] = b1[i] + b2[i];
            return r;
        } 
    }
}