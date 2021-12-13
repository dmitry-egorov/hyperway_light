using System;
using Common;
using Lanski.Utilities.assertions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using static Hyperway.hyperway;

namespace Hyperway {
    using prev = FormerlySerializedAsAttribute;
    using u16  = UInt16;
    
    [after(typeof(ScenarioConfig))]
    public class ProductionType : MonoBehaviour {
        public prod_spec_id id;

        public ResourceLoad[] @in;
        public ResourceLoad[] @out;
        public  u16   ticks;

        void Start() {
            (@in.Length <= 8 && @out.Length <= 8).assert();

            _prod_specs.  in_resources_arr[id] = @in;
            _prod_specs. out_resources_arr[id] = @out;
            _prod_specs.required_ticks_arr[id] = ticks;
        }
    }

    public static partial class hyperway {
        [InlineProperty(LabelWidth = 1)] public partial struct prod_spec_id { }
    } 
}