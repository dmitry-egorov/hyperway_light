using System;

namespace Hyperway {
    using save  = SerializableAttribute;

    [save] public partial struct stats {
        public fixed_batch total_stored;
    }
}