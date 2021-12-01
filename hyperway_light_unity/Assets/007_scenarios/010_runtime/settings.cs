using System;
using Utilities.Runtime;

namespace Scenario {
    using save = SerializableAttribute;
    
    [save] public partial struct 
    settings {
        public static provider<settings> data_provider;
        public static ref settings data => ref data_provider();

        public random _random;
        
        [save] public struct 
        random {
            public uint initial_seed;
        }
    }
}