
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Utilities.EditorTools {
    public class RuntimeOnlyAttribute : PropertyAttribute {
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RuntimeOnlyAttribute))]
    public class RuntimeOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 
            Application.isPlaying ? EditorGUI.GetPropertyHeight(property, label, true) : 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (!Application.isPlaying)
                return;
            
            var prev_state = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = prev_state;
        }
    }
#endif
}