using Common;
using TMPro;
using UnityEngine;
using static Hyperway.hyperway;

namespace Hyperway.scenario.ui {
    using text = TextMeshProUGUI;

    [RequireComponent(typeof(IntegerText))]
    [before(typeof(IntegerText))]
    public class ResourceAmountText : MonoBehaviour {
        res_id type;
        IntegerText text;

        public void set_type(res_id type) => this.type = type;

        void Start () => text = GetComponent<IntegerText>();
        void Update() => text.value = _stats.total_stored[type];
    }
}