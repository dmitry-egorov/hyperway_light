using Common;
using UnityEngine;
using static Hyperway.hyperway;

namespace Hyperway {
    [after(typeof(HyperwayConfig))]
    public class CameraRigMarker : MonoBehaviour {
        public void Start() {
            _camera.transform = transform;
            _camera.unity_cam = GetComponentInChildren<Camera>();
        }
    }
}