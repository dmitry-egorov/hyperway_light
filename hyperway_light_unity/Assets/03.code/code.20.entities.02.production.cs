using Lanski.Utilities.assertions;
using static Hyperway.entity_type_props;
using static Hyperway.hyperway;

namespace Hyperway {
    public partial struct entity_type {
        public void produce() {
            if (all(produces)) {} else return;

            var types = _buildings;
            var input_arr  = types.production_input_arr;
            var ticks_arr  = types.production_ticks_arr;
            var output_arr = types.production_output_arr;

            for (ushort entity = 0; entity < count; entity++) {
                var type  = building_type_arr[entity];
                if (type != building_type_id.none) {} else continue;

                var remaining = remaining_ticks_arr[entity];
                if (remaining > 1) { // progress production
                    remaining_ticks_arr[entity] = (ushort)(remaining - 1);
                    continue;
                }
                
                var output = output_arr[type];
                if (remaining == 1) { // finish production
                    add_overflow(entity, output);
                    remaining_ticks_arr[entity] = 0;
                    continue;
                }

                (remaining == 0).assert(); { // check required resources and start production
                    var input = input_arr[type];
                    
                    if (has_space_for(entity, output)) {} else continue; // don't start production if there's no empty space in the storage
                    if (has_amount   (entity, input )) {} else continue; // don't start production if there's not enough resources

                    sub(entity, input);
                    remaining_ticks_arr[entity] = ticks_arr[type];
                    continue;
                }
            }
        }
    }
}