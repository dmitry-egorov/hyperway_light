using UnityEngine;

namespace Hyperway.scenario.ui {
    public class ResourceAmountBlock : MonoBehaviour {
        public res_type type;

        void Start() {
            foreach (var text in GetComponentsInChildren<ResourceAmountText>()) 
                text.set_type(type);
        }
    }
}