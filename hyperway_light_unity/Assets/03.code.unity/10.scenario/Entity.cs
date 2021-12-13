#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Jobs;
#endif
using Unity.Collections;
using UnityEngine;
using static Hyperway.hyperway;
using static UnityEditor.EditorGUILayout;

namespace Hyperway {
    public class Entity: MonoBehaviour {
        public remote_entity_id id;
        
        #if UNITY_EDITOR
        [CustomEditor(typeof(Entity))]
        public class LookAtPointEditor : Editor {
            public override void OnInspectorGUI() {
                var entity = ((Entity)target).id;
                ref var type = ref _entities[entity.type_id];

                GUI.enabled = false;
                TextField("type", type.name);
                
                type.inspect_rendering (entity.id);
                type.inspect_position  (entity.id);
                type.inspect_movement  (entity.id);
                type.inspect_storage   (entity.id);
                type.inspect_logistics (entity.id);
                type.inspect_production(entity.id);
                type.inspect_families  (entity.id);
                type.inspect_hunger  (entity.id);
                
                GUI.enabled = true;
            }
        }
        #endif
    }

    public static partial class hyperway {
        public partial struct entity_type {
            public static void draw<t>(string name, NativeArray<t> arr, entity_id id) where t : struct {
                if (arr.IsCreated) {} else return;
                TextField(name, arr[id].ToString());
            }

            public static void draw(string name, TransformAccessArray arr, entity_id id) {
                if (arr.isCreated) {} else return;
                ObjectField(name, arr[id], typeof(Transform), true);
            }

            public static void draw(string name, NativeBitArray arr, entity_id id) {
                if (arr.IsCreated) {} else return;
                TextField(name, arr.IsSet(id).ToString());
            }
        }
    } 
}