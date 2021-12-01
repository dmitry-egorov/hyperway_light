using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Common.features.editor {
    public class PaletteEditorWindow : EditorWindow {
        const int texture_size = 256;
        const int cell_width = 8;
        const int cell_count = texture_size / cell_width;

        [MenuItem("Tools/Palette Editor")]
        static void ShowWindow() {
            var window = GetWindow<PaletteEditorWindow>();
            window.titleContent = new GUIContent("Palette Editor");
            window.Show();
        }

        void OnEnable() {
            load_colors();
        }

        void OnGUI() {
            draw_texture();
            EditorGUILayout.Space();
            draw_colors();
            EditorGUILayout.Space();
            draw_save_button();
        }

        void draw_texture() {
            var prev_texture = target_texture;
            target_texture = EditorGUILayout.ObjectField("Texture to be edited", target_texture, typeof(Texture2D), false, GUILayout.MaxWidth(400)) as Texture2D;
            if (target_texture != prev_texture) {
                if (target_texture != null) 
                    Debug.Assert(target_texture.width == texture_size && target_texture.height == texture_size, $"texture width and height must be {texture_size}");

                load_colors();
            }
        }

        void draw_colors() {
            if (colors == null)
                return;

            var colors_count = colors.Length;
            var rows = colors_count / 2;
            for (var i = 0; i < rows; i++) {
                EditorGUILayout.BeginHorizontal();
                
                for (var j = 0; j < 2 ; j++) {
                    var index = i * 2 + j;
                    var prev_colors = colors[index];
                    colors[index].left = EditorGUILayout.ColorField(prev_colors.left);
                    colors[index].right = EditorGUILayout.ColorField(prev_colors.right);

                    if (prev_colors != colors[index]) 
                        update_color(index);
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            target_texture.Apply();
        }

        void draw_save_button() {
            var has_texture = target_texture != null;
            GUI.enabled = has_texture;
            if (GUILayout.Button("Save") && has_texture) {
                var path = AssetDatabase.GetAssetPath(target_texture);
                var png_data = target_texture.EncodeToPNG();
                File.WriteAllBytes(path, png_data);
                AssetDatabase.Refresh();
            }

            GUI.enabled = true;
        }

        void load_colors() {
            colors ??= new gradient[cell_count];

            if (target_texture == null) {
                Array.Clear(colors, 0, cell_count);
                return;
            }

            for (var i = 0; i < cell_count; i++) {
                var y = (cell_count - i - 1) * cell_width;
                var left  = target_texture.GetPixel(0, y);
                var right = target_texture.GetPixel(255, y);
                colors[i] = new gradient(left, right);
                update_color(i);
            }
            
            target_texture.Apply();
        }

        void update_color(int i) {
            var y0 = (cell_count - i - 1) * cell_width;
            
            for (var y = 0; y < cell_width; y++)
            for (var x = 0; x < texture_size; x++)
                target_texture.SetPixel(x, y0 + y, Color.Lerp(colors[i].left, colors[i].right, x / (float) texture_size));
        }

        struct gradient {
            public Color left;
            public Color right;

            public gradient(Color left, Color right) {
                this.left = left;
                this.right = right;
            }

            public bool Equals(gradient other) => right.Equals(other.right) && left.Equals(other.left);
            public override bool Equals(object obj) => obj is gradient other && Equals(other);

            public override int GetHashCode() {
                unchecked {
                    return (right.GetHashCode() * 397) ^ left.GetHashCode();
                }
            }
            public static bool operator==(gradient g1, gradient g2) => g1.left == g2.left && g1.right == g2.right;
            public static bool operator !=(gradient g1, gradient g2) => !(g1 == g2);
        }


        gradient[] colors;
        [SerializeField] Texture2D target_texture;
    }
}