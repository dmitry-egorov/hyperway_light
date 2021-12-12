using Common;
using TMPro;
using UnityEngine;

namespace Hyperway.scenario.ui {
    using text = TextMeshProUGUI;

    [RequireComponent(typeof(IntegerText))]
    [before(typeof(IntegerText))]
    public class FamiliesCountText : MonoBehaviour {
        IntegerText text;

        void Start () => text = GetComponent<IntegerText>();
        void Update() => text.value = hyperway._stats.total_families;
    }
}