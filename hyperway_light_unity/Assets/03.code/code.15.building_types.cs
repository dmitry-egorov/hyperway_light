using Unity.Collections;
using static Hyperway.building_type_id;
using static Hyperway.hyperway;
using static Unity.Collections.Allocator;
using static Unity.Collections.NativeArrayOptions;

namespace Hyperway {
    using arr_rt  = NativeArray<res_type>;
    using arr_u16 = NativeArray<  ushort>;

    public partial struct buildings {
        public void init() {
             init(ref storage_capacity_arr );
             init(ref production_input_arr );
             init(ref production_output_arr);
             init(ref production_ticks_arr );

            void init<t>(ref NativeArray<t> arr) where t : struct {
                if (arr.IsCreated) arr.Dispose();
                arr = new NativeArray<t>(max_count, Persistent, ClearMemory);
            }
        }
        
        public static uint get_capacity(building_type_id id, res_type res_type) => _buildings.storage_capacity_arr[id][res_type];
    }
}