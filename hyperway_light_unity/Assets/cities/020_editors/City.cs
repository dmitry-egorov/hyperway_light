using NaughtyAttributes;
using UnityEngine;

namespace Cities.editors {
    public class City: MonoBehaviour {
        [Header("Random")]
        [Label("seed")] public uint random_seed = 42;

        [Header("Camera")]
        [Label("rig transform" )] public Transform camera_rig_transform;
        [Label("keyboard speed")] public float     camera_keyboard_speed = 10;

        [HorizontalLine(height: 1)]
        [ReadOnly]
        [Label("data")] public city city;

        void Awake () {
            city._data_source = () => ref city;
            
            update_settings();
            city.init();
        }

        void Update() {
            #if UNITY_EDITOR
            update_settings();
            #endif
            city.update();
        }

        void update_settings() {
            city._random.seed = random_seed;

            city._camera.keyboard_move_speed = camera_keyboard_speed;
            city._camera.rig_transform = camera_rig_transform;
        }
    }
}