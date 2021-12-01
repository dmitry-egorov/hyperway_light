using System;
using UnityEngine;
using Utilities.Maths;
using static Cities.offset;
using static UnityEngine.Input;
using static UnityEngine.KeyCode;
using static UnityEngine.Time;

namespace Cities {
    using save = SerializableAttribute;
    using trans = Transform;
    
    public partial struct city {
        [save] public partial struct 
        camera {
            public float keyboard_move_speed;
            public position position;
            public Transform rig_transform;

            public void init() {
                position = rig_transform.localPosition.xz().to_f2();
            }

            public void update() {
                move_with_keys();

                update_transform();
            }

            void move_with_keys() {
                var dir = zero;
                if (key(A) || key(LeftArrow )) dir  = left  + up   ;
                if (key(D) || key(RightArrow)) dir += right + down ;
                if (key(W) || key(UpArrow   )) dir += up    + right;
                if (key(S) || key(DownArrow )) dir += down  + left ;
                
                if (dir != zero) {} else return;

                position += dir * (deltaTime * keyboard_move_speed);
                
                static bool key(KeyCode key) => GetKey(key);
            }

            void update_transform() => rig_transform.localPosition = position.to_v3();
        }
    }
}