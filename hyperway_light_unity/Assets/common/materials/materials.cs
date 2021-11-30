using UnityEngine;

namespace Common.materials {
    [CreateAssetMenu(fileName = "all_materials", menuName = "Game/all materials", order = 1)]
    public class materials: ScriptableObject {
        public Material progress_material;
        public Material planned_material;
        public Material highlight_material;
    }
}