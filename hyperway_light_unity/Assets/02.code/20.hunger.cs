using System;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using Utilities.Collections;
using static Hyperway.hyperway.entity_type_props;

namespace Hyperway {
    using save = SerializableAttribute;

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

            public void hunger_fields() {
                req(houses, ref hunger_level_arr); 
            }
        
            public void update_hunger() {
                if (all(houses)) {} else return;

                if (_hunger.ticks == 0) {} else return;
            
                for (u16 entity_id = 0; entity_id < count; entity_id++) {
                    if (occupied_arr.IsSet(entity_id)) {} else continue;

                    // todo: find food resource properly, with a flag on resource type, is_food
                    var food_id = new res_id {value = 3};
                    if (has_amount(entity_id, food_id, 1)) {
                        sub(entity_id, food_id, 1);
                        continue;
                    }
                
                    // no food
                    ref var hunger = ref hunger_level_arr.@ref(entity_id);
                    hunger++;
                
                    if (hunger > _hunger.max_level)
                        occupied_arr.Set(entity_id, false); // die
                }
            }
        }
    }
}