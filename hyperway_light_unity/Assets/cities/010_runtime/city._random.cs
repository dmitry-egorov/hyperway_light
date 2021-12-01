using System;
using Random = Unity.Mathematics.Random;

namespace Cities {
    using save = SerializableAttribute;
    
    public partial struct city {
        [save] public partial struct 
        random {
            public uint seed;
            public Random generator;

            public void init() {
                generator = new Random(seed);
            }
        }
    }
}