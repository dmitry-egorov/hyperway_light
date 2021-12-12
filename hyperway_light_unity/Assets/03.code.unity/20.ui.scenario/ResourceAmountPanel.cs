using Common;
using TMPro;
using UnityEngine;

namespace Hyperway.scenario.ui {
    //TODO: implement before and after at the same time
    //[after(typeof(ScenarioConfig))]
    [before(typeof(ResourceAmountBlock))]
    public class ResourceAmountPanel : MonoBehaviour {
        public void Start() {
            var scenario = FindObjectOfType<ScenarioConfig>();
            var blocks   = transform.GetComponentsInChildren<ResourceAmountBlock>();
            var tips     = transform.GetComponentsInChildren<ResourceNameText>();
            var ress = scenario.ui_displayed_resources;
            for (var i = 0; i < ress.Length; i++) {
                blocks[i].type = ress[i];
                tips[i].GetComponent<TextMeshProUGUI>().text = ress[i].name;
            }
        }
    }
}