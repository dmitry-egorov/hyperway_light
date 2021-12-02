using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Game.editors {
    using head = HeaderAttribute; using gray = DisableIfAttribute; using show = ShowIfAttribute; using name = LabelAttribute; using line = HorizontalLineAttribute;

    public class GameSettings: MonoBehaviour {
        [head("Camera")]
        [name("keyboard speed" )] public float camera_keyboard_speed  = 10;
        [name("mouse max speed")] public float camera_mouse_max_speed = 150;
        [name("mouse slowdown" )] public float camera_mouse_slowdown  = 100;
        
        [line(height: 1)]
        [show("playing")]
        [name("data"   )] public settings settings;

        void Awake() {
            settings.data_provider = () => ref settings;
            update_settings();
        }

        #if UNITY_EDITOR
        void Update() => update_settings();
        #endif

        void update_settings() {
            settings._camera.keyboard_speed       = camera_keyboard_speed;
            settings._camera.mouse_drag_max_speed = camera_mouse_max_speed;
            settings._camera.mouse_drag_slowdown  = camera_mouse_slowdown;
        }

        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}