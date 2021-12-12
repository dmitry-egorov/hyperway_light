using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utilities.Nullable {
 
  /// <summary>
  /// Optional. Does the same as C# System.Nullable, except it's an ordinary
  /// serializable struct, allowing unity to serialize it and show it in the inspector.
  /// </summary>
  [System.Serializable]
  public struct o<T> where T : struct {
      [SerializeField] public T value;
      [SerializeField] public bool has_value;
   
      public o(bool has_value, T value) {
          this.value = value;
          this.has_value = has_value;
      }

      o(T value) {
          this.value = value;
          has_value = true;
      }

      public T get_or_default() => has_value ? value : default;
      public bool try_get(out T value) {
          if (has_value) {
            value = this.value;
            return true;
          }

          value = default;
          return false;
      }
   
      public static implicit operator o<T>(T value) => new o<T>(value);
      public static implicit operator o<T>(T? value) => value.HasValue ? new o<T>(value.Value) : new o<T>();
      public static implicit operator T?(o<T> value) => value.has_value ? (T?)value.value : default;
  }
  
#if UNITY_EDITOR
  [CustomPropertyDrawer(typeof(o<>))]
  class optional_drawer : PropertyDrawer {
      public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
          var hvp = get_has_value_property(property);
          return hvp.boolValue ? EditorGUI.GetPropertyHeight(get_value_property(property)) : EditorGUI.GetPropertyHeight(hvp);
      }

      public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
          // Draw fields - pass GUIContent.none to each so they are drawn without labels
          var has_value_property = get_has_value_property(property);
          var value_property = get_value_property(property);
          var has_value = has_value_property.boolValue;

          const int box_size = 15;
          var setRect = new Rect(position.x + EditorGUIUtility.labelWidth + 2, position.y + 1, box_size, box_size);
          
          if (has_value) {
              EditorGUI.PropertyField(setRect, has_value_property, GUIContent.none);
              EditorGUI.PropertyField(position, value_property, label, true);
          }
          else {
              EditorGUI.LabelField(position, label);
              EditorGUI.PropertyField(setRect, has_value_property, GUIContent.none);
          }
      }

      static SerializedProperty get_value_property(SerializedProperty property) => property.FindPropertyRelative(nameof(o<int>.value));
      static SerializedProperty get_has_value_property(SerializedProperty property) => property.FindPropertyRelative(nameof(o<int>.has_value));
  }
#endif
}