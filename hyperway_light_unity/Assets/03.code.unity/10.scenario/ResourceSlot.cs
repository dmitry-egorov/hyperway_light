using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Collections;
using static Hyperway.hyperway;
using static Hyperway.hyperway.res_filter;
using static Hyperway.ResourceSlot.ResFilter;

namespace Hyperway {
    using former = FormerlySerializedAsAttribute;
    using save = SerializableAttribute;
    using        u16 = UInt16;
    using res_load_8 = fixed_arr_8<res_load<ushort>>;
    
    [save] public struct ResourceSlot {
        [former("type")] public ResFilter filter; 
        [former("type")]   public Resource resource;
        [former("amount")] public UInt16   capacity;
        
        [save] public enum ResFilter {
            Specific = 0,
            Any = 1,
            Food = 2
        }

        public res_filter to_filter() => filter switch {
              Specific => resource.id
            , Any      => any
            , Food     => food
            , _ => throw new ArgumentOutOfRangeException()
        };

            
        [CustomPropertyDrawer(typeof(ResourceSlot))]
        class PropertyDrawer : UnityEditor.PropertyDrawer {
            public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
                pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
                
                const int aw = 100;
                const int rw = 150;
                var tw = pos.width - aw - rw - 2*5;
                var h  = pos.height;
                var x  = pos.x; var y  = pos.y;
            
                var r0 = new Rect(x                , y, tw, h);
                var r1 = new Rect(x + tw      +   5, y, rw, h);
                var r2 = new Rect(x + tw + rw + 2*5, y, aw, h);
                var type_prop = prop(nameof(filter));
                field1(r0, type_prop);
                if (type_prop.enumValueIndex == (int)Specific) 
                    field(r1, nameof(resource));
                field(r2, nameof(capacity));

                SerializedProperty prop(string name) => property.FindPropertyRelative(name);
                void field (Rect r, string name) => field1(r, prop(name));
                void field1(Rect r, SerializedProperty prop) => EditorGUI.PropertyField(r, prop, GUIContent.none);
            }
        }
    }

    public static partial class res_filter_ext {
    }
}