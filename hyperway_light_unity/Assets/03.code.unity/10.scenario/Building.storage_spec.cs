using System;
using UnityEditor;
using UnityEngine;
using static Hyperway.res_type;
using static UnityEditor.EditorGUI;

namespace Hyperway.unity {
    using save = SerializableAttribute;
        
    [save] public struct storage_spec_slot {
        public res_type type;
        public uint capacity;
        public uint amount;

        [CustomPropertyDrawer(typeof(storage_spec_slot))]
        class PropertyDrawer : UnityEditor.PropertyDrawer {
            public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label) {
                const int aw = 100;
                var tw = pos.width - 2*aw - 2*5;
                var h  = pos.height;
                var x  = pos.x; var y  = pos.y;
            
                var r0 = new Rect(x                , y, tw, h);
                var r1 = new Rect(x + tw + 5       , y, aw, h);
                var r2 = new Rect(x + tw + aw + 2*5, y, aw, h);
                prop(r0, nameof(type    ));
                prop(r1, nameof(capacity));
                prop(r2, nameof(amount  ));

                void prop(Rect r, string name) => PropertyField(r, property.FindPropertyRelative(name), GUIContent.none);;
            }
        }
    }

    public static class storage_spec_ext {
        public static fixed_batch capacity(this storage_spec_slot[] specs) {
            var r = new fixed_batch();
            for (var i = 0; i < specs.Length; i++)
                r[specs[i].type] += specs[i].capacity;
            return r;
        }

        public static fixed_batch amount(this storage_spec_slot[] specs) {
            var r = new fixed_batch();
            for (res_type i = 0; i < count; i++)
            for (var j = 0; j < specs.Length; j++)
                r[i] += specs[j].type == i ? specs[j].amount : 0;

            return r;
        }
    }
}