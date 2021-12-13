using System;
using Lanski.Utilities.assertions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Collections;
using static Hyperway.hyperway;

namespace Hyperway {
    
    using former = FormerlySerializedAsAttribute;
    using save = SerializableAttribute;
    using        u16 = UInt16;
    using res_load_8 = fixed_arr_8<res_load<ushort>>;
    
    [save] public struct ResourceLoad {
        public Resource resource;
        public UInt16 amount;
        
        public static implicit operator res_load<UInt16>(ResourceLoad rl) => new res_load<ushort> { type = rl.resource, amount = rl.amount };
        
        [CustomPropertyDrawer(typeof(ResourceLoad))]
        class PropertyDrawer : UnityEditor.PropertyDrawer {
            public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
                pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
                
                const int aw = 100;
                var tw = pos.width - aw - 2*5;
                var h  = pos.height;
                var x  = pos.x; var y  = pos.y;
            
                var r0 = new Rect(x         , y, tw, h);
                var r1 = new Rect(x + tw + 5, y, aw, h);
                field(r0, nameof(resource));
                field(r1, nameof(amount));

                SerializedProperty prop(string name) => property.FindPropertyRelative(name);
                void field (Rect r, string name) => field1(r, prop(name));
                void field1(Rect r, SerializedProperty prop) => EditorGUI.PropertyField(r, prop, GUIContent.none);
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
            public ResourceLoad[] this[prod_spec_id id] {
                set {
                    counts[id] = (byte)value.Length;
                    loads [id] = value.to_res_load_8();
                } 
            }
        }
    }
}