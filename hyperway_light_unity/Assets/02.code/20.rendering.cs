using Common.spaces;
using Lanski.Plugins.Persistance;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using static Hyperway.hyperway.entity_type_props;
using static UnityEngine.GUILayout;

namespace Hyperway {
    using burst = BurstCompileAttribute; using read = ReadOnlyAttribute;
    using point_arr = NativeArray<point2>; using trans = TransformAccess; using trans_job = IJobParallelForTransform;
    using trans_arr = TransformAccessArray;

    public static partial class hyperway {
        public partial struct entity_type {
            [savefile] public trans_arr trans_arr; // transform
        
            public void rendering_fields() {
                req(rendered, ref trans_arr);
            }

            public void inspect_rendering(entity_id id) {
                Label("Rendering");
                draw(nameof(trans_arr), trans_arr, id);
            }

            public void set_initial_transform() {
                if (props.all(rendered)) {} else return;

                for (var i = 0; i < count; i++)
                    trans_arr[i].localPosition = curr_pos_arr[i].x0y_v3();
            }

            public void update_transform() {
                if (props.all(rendered | moves)) {} else return;
                var ratio = _runtime.frame_to_tick_ratio;

                _runtime.schedule(new update_transform_job {
                    prev = prev_pos_arr,
                    curr = curr_pos_arr,
                    ratio = ratio
                }, trans_arr);
            }

            [burst] struct update_transform_job : trans_job {
                [read] public point_arr prev;
                [read] public point_arr curr;
                public float ratio;
            
                public void Execute(int i, trans transform) => transform.localPosition = prev[i].lerp(curr[i], ratio).x0y_v3();
            }
        }
    } 
}