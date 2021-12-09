using System;
using UnityEditor;
using UnityEngine;
using static Hyperway.res_type;

namespace Hyperway {
    public partial struct batch_u16 {
        [CustomPropertyDrawer(typeof(batch_u16))]
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
}