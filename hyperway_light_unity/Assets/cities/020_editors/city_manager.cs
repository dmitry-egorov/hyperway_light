using UnityEngine;
using static Cities.city;

namespace Cities.editors {
    public class city_manager: MonoBehaviour {
        public uint random_seed;
        
        void Awake () => instance.init(random_seed);
        void Update() => instance.update();
    }
}