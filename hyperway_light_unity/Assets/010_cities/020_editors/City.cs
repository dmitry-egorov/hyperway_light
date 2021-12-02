using System;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Cities.editors {
    using head = HeaderAttribute; using gray = DisableIfAttribute; using show = ShowIfAttribute; using name = LabelAttribute; using line = HorizontalLineAttribute;

    public class City : MonoBehaviour {
        [line(height: 1  )]
        [show("playing"  )]
        [name("city data")] public city city;

        void Awake () {
            city.data_provider = () => ref city;
        }

        void Update() {
            if (!initialized) {
                city._archetypes = ArchetypeConstructor.archetypes.ToArray();
                city.init();
                initialized = true;
            }
            
            city.update();
        }
        
        [NonSerialized] bool initialized;
        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}