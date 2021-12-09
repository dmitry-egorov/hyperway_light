using System;

namespace Lanski.Utilities.boxing {
    using save = SerializableAttribute;
    
    [save] public class box<t> where t: struct {
        public t value;
    }
}