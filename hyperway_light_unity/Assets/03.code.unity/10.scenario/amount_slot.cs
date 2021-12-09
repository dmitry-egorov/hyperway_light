using System;
using UnityEditor;
using UnityEngine;
using static Hyperway.res_type;

namespace Hyperway.unity {
    [Serializable] public struct amount_slot {
        public res_type type;
        public ushort amount;
        
        [CustomPropertyDrawer(typeof(amount_slot))]
        class PropertyDrawer : UnityEditor.PropertyDrawer {
            public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
                const int aw = 100;
                var tw = pos.width - 2*aw - 2*5;
                var h  = pos.height;
                var x  = pos.x; var y  = pos.y;
            
                var r0 = new Rect(x               , y, tw, h);
                var r1 = new Rect(x + tw + aw + 5 , y, aw, h);
                prop(r0, nameof(type  ));
                prop(r1, nameof(amount));

                void prop(Rect r, string name) => EditorGUI.PropertyField(r, property.FindPropertyRelative(name), GUIContent.none);
            }
        }
    }

    public static class amount_slot_ext {
        public static fixed_batch amount(this amount_slot[] specs) {
            var r = new fixed_batch();
            for (res_type i = 0; i < count; i++)
            for (var j = 0; j < specs.Length; j++)
                r[i] += specs[j].type == i ? (uint)specs[j].amount : 0;

            return r;
        }
    }
}