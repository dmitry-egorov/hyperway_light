using Common.spaces;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using static Hyperway.entity_type.props_;
using static Hyperway.hyperway;

namespace Hyperway {
    using burst = BurstCompileAttribute; using read = ReadOnlyAttribute;
    using point_arr = NativeArray<point2>; using trans = TransformAccess; using trans_job = IJobParallelForTransform;

    public partial struct entity_type {
        public void set_initial_transform() {
            if (props.all(rendered)) {} else return;

            for (var i = 0; i < count; i++)
                transform[i].localPosition = curr_position[i].x0y_v3();
        }

        public void update_transform() {
            if (props.all(rendered | moves)) {} else return;
            var ratio = _runtime.frame_to_tick_ratio;

            _runtime.schedule(new update_transform_job {
                 prev = prev_position,
                 curr = curr_position,
                ratio = ratio
            }, transform);
        }

        [burst] struct update_transform_job : trans_job {
            [read] public point_arr prev;
            [read] public point_arr curr;
            public float ratio;
            
            public void Execute(int i, trans transform) => transform.localPosition = prev[i].lerp(curr[i], ratio).x0y_v3();
        }
    }
}