#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utilities.EditorTools {
    public static class EditorGUIEx {
        public static void ObjectField<T>(Rect position, SerializedProperty property, string name) => 
            EditorGUI.ObjectField(position, property.FindPropertyRelative(name), typeof(T).GetField(name).FieldType, GUIContent.none);
    }
}
#endif