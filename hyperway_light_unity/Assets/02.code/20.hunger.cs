using System;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using Utilities.Collections;
using static Hyperway.hyperway;
using static Hyperway.hyperway.entity_type_props;
using static Hyperway.hyperway.res_filter;

namespace Hyperway {
    using save = SerializableAttribute;
    
    using slot_id = storage_slot_id;

    using u8 = Byte;
    using u16 = UInt16;

    using u8__arr = NativeArray<byte>;
    using u16_arr = NativeArray<ushort>;

    public static partial class hyperway {
        public static hunger _hunger;
    
        [save] public partial struct hunger {
            [scenario] public u8  max_level; // maximum hunger, after which the family dies
            [scenario] public u16 cooldown;  // hunger cooldown
        
            [savefile] public u16 ticks;     // remaining ticks till the next hunger update;

            public void start() => ticks = cooldown;

            public void update() {
                if (ticks == 0) {
                    ticks = cooldown;
                    return;
                }
            
                ticks -= 1;
            }
        }
    
        public partial struct entity_type {
            [savefile] public u8__arr hunger_level_arr; // current hunger level

            public void hunger_fields() => req(houses, ref hunger_level_arr);

            public ref u8 get_hunger_level_ref(entity_id id) => ref hunger_level_arr.@ref(id);
        
            public void update_hunger() {
                if (all(houses)) {} else return;

                if (_hunger.ticks == 0) {} else return;
            
                for (u16 entity_id = 0; entity_id < count; entity_id++) {
                    if (is_occupied(entity_id)) {} else continue;

                    var food_res = try_find(entity_id, food);
                    
                    var had_food = food_res != res_id.none && try_sub(entity_id, food_res, 1);
                    if (had_food) continue;
                    
                    ref var hunger = ref get_hunger_level_ref(entity_id);
                    hunger++;
                
                    if (hunger > _hunger.max_level)
                        reset_occupied(entity_id); // die
                }
            }
        }
    }
}