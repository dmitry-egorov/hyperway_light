using UnityEngine;
using static Hyperway.ui_scenario;

namespace Hyperway.scenario.ui {
    public class PanelManager : MonoBehaviour {
        void Start() => panels = GetComponentsInChildren<Panel>(true);

        void Update() {
            foreach (var panel in panels)
                panel.gameObject.SetActive(_visible_panels.has(panel.id));
        }

        Panel[] panels;
    }
}