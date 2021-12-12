using System;
using Lanski.Utilities.assertions;
using UnityEditor;
using UnityEngine;
using Utilities.Collections;
using static Hyperway.hyperway;

namespace Hyperway {
    using save = SerializableAttribute;
    using Load = ResourceLoad;

    using        u16 = UInt16;
    using res_load_8 = fixed_arr_8<res_load<ushort>>;
    
    [save] public struct ResourceLoad {
        public Resource type;
        public UInt16 amount;

        public static implicit operator res_load<UInt16>(ResourceLoad rl) => new res_load<ushort> { type = rl.type, amount = rl.amount };
        
        [CustomPropertyDrawer(typeof(ResourceLoad))]
        class PropertyDrawer : UnityEditor.PropertyDrawer {
            public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
                pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
                
                const int aw = 100;
                var tw = pos.width - aw - 2*5;
                var h  = pos.height;
                var x  = pos.x; var y  = pos.y;
            
                var r0 = new Rect(x           , y, tw, h);
                var r1 = new Rect(x + tw  + 5 , y, aw, h);
                prop(r0, nameof(type));
                prop(r1, nameof(amount));

                void prop(Rect r, string name) => EditorGUI.PropertyField(r, property.FindPropertyRelative(name), GUIContent.none);
            }
        }
    }

    public static class reosurce_load_arr_ext {
        public static res_load_8 to_res_load_8(this ResourceLoad[] rl) {
            (rl.Length <= 4).assert();
            var r = new res_load_8();
            for (byte i = 0; i < rl.Length; i++) r.@ref(i) = rl[i];
            return r;
        }
    }

    public static partial class hyperway {
        public partial struct res_multi_8_arr {
            public Load[] this[prod_spec_id id] {
                set {
                    counts[id] = (byte)value.Length;
                    loads [id] = value.to_res_load_8();
                } 
            }
        }
    } 
}