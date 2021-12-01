using System;
using Scenario;
using Random = Unity.Mathematics.Random;

namespace Cities {
    using save = SerializableAttribute;
    
    public partial struct city {
        [save] public partial struct 
        random {
            public Random generator;

            public void init() {
                generator = new Random(settings.initial_seed);
            }
        }
        
        public static ref settings.random settings => ref Scenario.settings.data._random;
    }
}