using System;
using Common;
using UnityEngine;
using static Hyperway.hyperway;

namespace Hyperway.scenario.ui {
    using no_save = NonSerializedAttribute;
    
    [before(typeof(ResourceAmountText))]
    public class ResourceAmountBlock : MonoBehaviour {
        [no_save] public res_id type;

        void Start() {
            foreach (var text in GetComponentsInChildren<ResourceAmountText>()) 
                text.set_type(type);
        }
    }
}