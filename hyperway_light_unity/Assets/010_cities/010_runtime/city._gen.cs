using UnityEngine;

namespace Cities {
    public partial struct city {
        public runtime     _runtime   ;
        public random      _random    ;
        public camera      _camera    ;
        public archetype[] _archetypes;

        public partial struct archetype {
            public ref runtime     runtime    => ref data._runtime   ;
            public ref random      random     => ref data._random    ;
            public ref camera      camera     => ref data._camera    ;
            public ref archetype[] archetypes => ref data._archetypes;
        }

        public partial struct entity {
            public ref runtime     runtime    => ref data._runtime   ;
            public ref random      random     => ref data._random    ;
            public ref camera      camera     => ref data._camera    ;
            public ref archetype[] archetypes => ref data._archetypes;
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