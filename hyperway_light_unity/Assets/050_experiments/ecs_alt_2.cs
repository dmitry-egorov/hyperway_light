#if UNITY_EDITOR
using System;
using UnityEngine;
using Utilities.Collections;
using static Experiment.ecs_alternative.city;

/*
 * entities filtered by archetype data only (tags, shared data);
 *
 * reset current state on each method
 * set current archetype
 *
 * can make a t4 partial class that lists components
 * do we need a separate entity classes? (city, ui, etc)
 * struct entity with methods
 */
namespace Experiment.ecs_alternative {
    using err = ApplicationException;

    public class hyper_way_city: MonoBehaviour {
        public void Update() {
            update_simulation();
        }
        
        public void update_simulation() {
            for_each((ref entity a) => a.remember_prev_position());
            for_each((ref entity a) => a.apply_velocity());
            
            for_each((ref archetype a) => a.remember_prev_positions());
            for_each((ref archetype a) => a.apply_velocities());
        }

        static void for_each(entity_action action) {
            var entity = new entity();
            
            ref var arch_i   = ref entity.arch_index;
            ref var entity_i = ref entity.index;

            var archetypes = current.archetypes;
            for (; arch_i < archetypes.Length; arch_i++) {
                ref var archetype = ref archetypes[arch_i];
                for (entity_i = 0; entity_i < archetype.count; entity_i++) 
                    action(ref entity);
            }
        }

        static void for_each(archetype_action action) {
            var archetypes = current.archetypes;
            for (var arch_i = 0; arch_i < archetypes.Length; arch_i++) {
                action(ref archetypes[arch_i]);
            }
        }
        
        delegate void archetype_action(ref archetype entity);
        delegate void entity_action   (ref entity entity);
    }
    
    partial class city {
        public static city current;
        
        public archetype[] archetypes;

        public partial struct entity {
            public ushort arch_index;
            public ushort index;

            ref position prev_position => ref current.archetypes[arch_index].prev_position[index];
            ref position curr_position => ref current.archetypes[arch_index].curr_position[index];
            ref velocity curr_velocity => ref current.archetypes[arch_index].curr_velocity[index];

            public void remember_prev_position() => prev_position = curr_position;
            public void         apply_velocity() => curr_position.apply(curr_velocity);
        }

        public partial struct archetype {
            public ushort count;
            public ushort capacity;
        
            public readonly position[] prev_position;
            public readonly position[] curr_position;
            public readonly velocity[] curr_velocity;

            public void remember_prev_positions() {
                if (use(prev_position, curr_position)) {} else return;
                prev_position.copy_from(curr_position, count);
            }
            
            public void apply_velocities() {
                if (use(curr_position, curr_velocity)) {} else return;
                for (var i = 0; i < count; i++)
                    curr_position[i].apply(curr_velocity[i]);
            }
            
            bool use(Array a1) => a1 != null && count <= a1.Length; 
            bool use(Array a1, Array a2) => use(a1) && use(a2);
            bool use(Array a1, Array a2, Array a3) => use(a1) && use(a2) && use(a3);
            bool use(Array a1, Array a2, Array a3, Array a4) => use(a1) && use(a2) && use(a3) && use(a4);
        }
    }
    
    public partial struct position { public float2 vec; }
    public partial struct velocity { public float2 vec; } 
    public partial struct position { public void apply(in velocity v) => vec.add(v.vec); }
    
    public struct float2 {
        public float x, y;
        public void add(float2 o) {
            x += o.x;
            y += o.y;
        }
    }
}
#endif