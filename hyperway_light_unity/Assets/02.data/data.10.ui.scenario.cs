using System;
using Lanski.Utilities.collections;
using static Hyperway.hyperway;

namespace Hyperway {
    using save = SerializableAttribute;
    using bits = FlagsAttribute;

    [save] public partial struct ui_scenario {
        public static ref flags_u32<panel_id> _visible_panels => ref _ui_scenario.visible_panels;
            
        public flags_u32<panel_id> visible_panels;
        
        [save] public enum panel_id {
                resources = 0,
                    speed = 1,
            normal_screen = 2,
             build_screen = 3,

                    count
        }
    }
}