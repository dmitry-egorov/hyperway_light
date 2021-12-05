using System;
using static System.AttributeTargets;

namespace Lanski.Plugins.Persistance {
    using on = AttributeUsageAttribute;
    
    [on(Field)] public class permanentAttribute : Attribute { } // precalculated data
    [on(Field)] public class    configAttribute : Attribute { } // data saved in the game's global config file
    [on(Field)] public class  scenarioAttribute : Attribute { } // data saved in the scenario files
    [on(Field)] public class  savefileAttribute : Attribute { } // data saved in the save files
    [on(Field)] public class transientAttribute : Attribute { } // discarded data
    [on(Field)] public class aggregateAttribute : Attribute { } // location specified for each field
}