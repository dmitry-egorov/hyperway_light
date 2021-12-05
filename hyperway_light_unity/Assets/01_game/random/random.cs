using System;
using Random = Unity.Mathematics.Random;

namespace Hyperway {
    using save = SerializableAttribute;
    
    public partial struct random {
        public void init() {
            generator = new Random(initial_seed);
        }
    }
}