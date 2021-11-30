

using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Cities {
    public partial struct city {
        public archetype[] archetypes         ;
        public Random      random             ;
        public bool        paused             ;
        public float       time_till_next_tick;
        public float       frame_to_tick_ratio;

        public partial struct entity {
            public ref archetype[] archetypes          => ref instance.archetypes         ;
            public ref Random      random              => ref instance.random             ;
            public ref bool        paused              => ref instance.paused             ;
            public ref float       time_till_next_tick => ref instance.time_till_next_tick;
            public ref float       frame_to_tick_ratio => ref instance.frame_to_tick_ratio;
        }

        public partial struct archetype {
            public ref archetype[] archetypes          => ref instance.archetypes         ;
            public ref Random      random              => ref instance.random             ;
            public ref bool        paused              => ref instance.paused             ;
            public ref float       time_till_next_tick => ref instance.time_till_next_tick;
            public ref float       frame_to_tick_ratio => ref instance.frame_to_tick_ratio;
        }
 
        public partial struct archetype {
            public ushort count;
            public ushort capacity;

            public  position[] curr_position;
            public  position[] prev_position;
            public  velocity[] curr_velocity;
            public Transform[] transform    ;
        }

        public partial struct entity {
            public ref  position curr_position => ref archetypes[arch_index].curr_position[index];
            public ref  position prev_position => ref archetypes[arch_index].prev_position[index];
            public ref  velocity curr_velocity => ref archetypes[arch_index].curr_velocity[index];
            public ref Transform transform     => ref archetypes[arch_index].transform    [index];
        }
    }
}