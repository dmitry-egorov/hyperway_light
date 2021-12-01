using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utilities.ScriptReferencing {
    public static class MonoScriptStorage<T> where T : class {
        public static T get(string type_name) {
            instances ??= new Dictionary<string, T>();
            if (!instances.TryGetValue(type_name, out var instance)) {
                var type = Type.GetType(type_name);
                Debug.Assert(type != null);
                instance = Activator.CreateInstance(type) as T;
                Debug.Assert(instance != null);
                instances.Add(type_name, instance);
            }

            return instance;
        }

        static Dictionary<string, T> instances;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class MonoScriptAttribute : PropertyAttribute
    {
        public Type type;
        public MonoScriptAttribute(Type type = default) => this.type = type;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MonoScriptAttribute), false)]
    public class MonoScriptPropertyDrawer : PropertyDrawer {
        static Dictionary<string, MonoScript> m_ScriptCache;
        static MonoScriptPropertyDrawer()
        {
            m_ScriptCache = new Dictionary<string, MonoScript>();
            var scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            for (int i = 0; i < scripts.Length; i++)
            {
                var type = scripts[i].GetClass();
                if (type != null && !m_ScriptCache.ContainsKey(type.FullName))
                {
                    m_ScriptCache.Add(type.FullName, scripts[i]);
                }
            }
        }
        bool m_ViewString;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.String) {
                GUI.Label(position, "The MonoScript attribute can only be used on string variables");
                return;
            }

            var r = EditorGUI.PrefixLabel(position, label);
            var labelRect = position;
            labelRect.xMax = r.xMin;
            position = r;
            m_ViewString = GUI.Toggle(labelRect, m_ViewString, "", "label");
            if (m_ViewString) {
                property.stringValue = EditorGUI.TextField(position, property.stringValue);
                return;
            }

            MonoScript script = null;
            var typeName = property.stringValue;
            if (!string.IsNullOrEmpty(typeName)) {
                m_ScriptCache.TryGetValue(typeName, out script);
                if (script == null)
                    GUI.color = Color.red;
            }

            script = (MonoScript) EditorGUI.ObjectField(position, script, typeof(MonoScript), false);

            if (!GUI.changed) return;

            if (script != null) {
                var type = script.GetClass();
                var attr = (MonoScriptAttribute) attribute;
                if (type == null)
                    Debug.LogWarning("The script file " + script.name + " doesn't contain a class");
                else if (attr.type != null && !attr.type.IsAssignableFrom(type))
                    Debug.LogWarning("The script file " + script.name + " doesn't contain a class that implements " + attr.type.Name);
                else
                    property.stringValue = script.GetClass().Name;
            }
            else
                property.stringValue = "";
        }
    }
#endif
}