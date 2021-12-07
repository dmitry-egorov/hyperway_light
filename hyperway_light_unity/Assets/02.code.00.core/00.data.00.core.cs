using System;
using Common.spaces;
using Lanski.Plugins.Persistance;
using Unity.Jobs;

namespace Hyperway {
    using save  = SerializableAttribute;
    using rand  = Unity.Mathematics.Random;

    [save] public partial struct mouse   {
        [config] public  float min_drag_dpi_distance;
        
        public point2 position;
        public   bool is_pressed;
        public   bool is_down;
        public   bool is_up;
        
        public   bool drag_in_progress;
        public   bool drag_started;
        public   bool drag_finished;
        public point2 drag_prev_position;
        public point2 drag_start_position;
    }

    [save] public partial struct random  {
        [scenario] public uint initial_seed;
        [savefile] public rand rand;
    }
    
    [save] public partial struct runtime {
        public  bool paused;
        public float time_till_next_tick;
        public float frame_to_tick_ratio;
        
        public JobHandle job_handle;
    }
}