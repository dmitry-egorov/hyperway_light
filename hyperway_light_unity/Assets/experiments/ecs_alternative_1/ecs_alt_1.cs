using System;
using System.Collections.Generic;
using static Experiment.ecs_alternative_1.ECS;
using static Experiment.ecs_alternative_1.ECS_access_mode_ext;
using static Experiment.ecs_alternative_1.ECS.access_mode;

namespace Experiment.ecs_alternative_1 {
    using comp = component;
    using am = access_mode;

    static class hyperway_game {
        public static void main_loop() {
            movement.remember_prev_position();
            movement.apply_velocity();
        }
    }

    static class movement {
        public static List<entity> moved = new List<entity>();

        public static void remember_prev_position() =>
            /*at<pre_movement>().*/
            for_each((ref prev_position prev, in curr_position curr) => prev.remember(curr));
        
        public static void apply_velocity() =>
            for_each((ref curr_position  pos, in curr_velocity  vel, in entity ent) => { pos.apply(vel); moved.Add(ent); });

        public static void remember(this ref prev_position pp, in curr_position p) => pp.pos = p.pos;
        public static void apply   (this ref curr_position  p, in curr_velocity v) => p.pos.apply(v.vel);
    }
    
    public struct pre_movement: before<regular<curr_position>> {}
    
    public struct prev_position : comp { public position pos; }
    public struct curr_position : comp { public position pos; }
    public struct curr_velocity : comp { public velocity vel; }
    public struct entity        : comp { public entity_id id; }

    public struct position  { public float2 vec; }
    public struct velocity  { public float2 vec; } public static class velocity_ext { public static void apply(this ref position p, in velocity v) => p.vec.add(v.vec); }
    public struct entity_id { public   uint  id; }
    
    public struct float2 {
        public float x, y;
        public void add(float2 o) {
            x += o.x;
            y += o.y;
        }
    }

    public interface  before<t>: stage where t: stage {}
    public interface regular<c>: stage where c: comp  {}
    
    public interface stage {}
    public interface component {}

    public class system : Attribute {}

    public static class ECS {
        public static void for_each<c1, c2>(action_rr<c1, c2> action) where c1: comp where c2: comp => @for<c1, c2>((r, r), (c1s, c2s) => { for (var i = 0; i < c1s.Length; i++) { action(    c1s[i],     c2s[i]); }});
        public static void for_each<c1, c2>(action_wr<c1, c2> action) where c1: comp where c2: comp => @for<c1, c2>((w, r), (c1s, c2s) => { for (var i = 0; i < c1s.Length; i++) { action(ref c1s[i],     c2s[i]); }});
        public static void for_each<c1, c2>(action_ww<c1, c2> action) where c1: comp where c2: comp => @for<c1, c2>((w, w), (c1s, c2s) => { for (var i = 0; i < c1s.Length; i++) { action(ref c1s[i], ref c2s[i]); }});
        
        public static void for_each<c1, c2, c3>(action_rrr<c1, c2, c3> action) where c1: comp where c2: comp where c3: comp => @for<c1, c2, c3>((r, r, r), (c1s, c2s, c3s) => { for (var i = 0; i < c1s.Length; i++) { action(    c1s[i],     c2s[i],     c3s[i]); }});
        public static void for_each<c1, c2, c3>(action_wrr<c1, c2, c3> action) where c1: comp where c2: comp where c3: comp => @for<c1, c2, c3>((w, r, r), (c1s, c2s, c3s) => { for (var i = 0; i < c1s.Length; i++) { action(ref c1s[i],     c2s[i],     c3s[i]); }});
        public static void for_each<c1, c2, c3>(action_wwr<c1, c2, c3> action) where c1: comp where c2: comp where c3: comp => @for<c1, c2, c3>((w, w, r), (c1s, c2s, c3s) => { for (var i = 0; i < c1s.Length; i++) { action(ref c1s[i], ref c2s[i],     c3s[i]); }});
        public static void for_each<c1, c2, c3>(action_www<c1, c2, c3> action) where c1: comp where c2: comp where c3: comp => @for<c1, c2, c3>((w, w, w), (c1s, c2s, c3s) => { for (var i = 0; i < c1s.Length; i++) { action(ref c1s[i], ref c2s[i], ref c3s[i]); }});
        
        static void @for<c1, c2>    ((am, am)     m, Action<c1[], c2[]>       action) where c1: comp where c2: comp {}
        static void @for<c1, c2, c3>((am, am, am) m, Action<c1[], c2[], c3[]> action) where c1: comp where c2: comp where c3: comp {}
        
        public delegate void action_rr<c1, c2>(in  c1 c1, in  c2 c2) where c1: comp where c2: comp;
        public delegate void action_wr<c1, c2>(ref c1 c1, in  c2 c2) where c1: comp where c2: comp;
        public delegate void action_ww<c1, c2>(ref c1 c1, ref c2 c2) where c1: comp where c2: comp;
        
        public delegate void action_rrr<c1, c2, c3>(in  c1 c1, in  c2 c2, in  c3 c3) where c1: comp where c2: comp where c3: comp;
        public delegate void action_wrr<c1, c2, c3>(ref c1 c1, in  c2 c2, in  c3 c3) where c1: comp where c2: comp where c3: comp;
        public delegate void action_wwr<c1, c2, c3>(ref c1 c1, ref c2 c2, in  c3 c3) where c1: comp where c2: comp where c3: comp;
        public delegate void action_www<c1, c2, c3>(ref c1 c1, ref c2 c2, ref c3 c3) where c1: comp where c2: comp where c3: comp;

        public enum access_mode { write, read }
    }

    public static class ECS_access_mode_ext {
        public const access_mode w = write;
        public const access_mode r = read;
    }
}

