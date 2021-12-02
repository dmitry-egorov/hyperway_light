using UnityEngine;

namespace Utilities.Runtime {
    public static partial class singletons {
        public static void check_and_create_persistant<t>(ref t instance) where t : Component {
            if (instance == null) {} else return;

            instance = Object.FindObjectOfType<t>();
            if (instance == null) {
                instance = new GameObject().AddComponent<t>();
                instance.name = $"[{typeof(t).Name}]";
                Object.DontDestroyOnLoad(instance);
            }
        }
    }
}