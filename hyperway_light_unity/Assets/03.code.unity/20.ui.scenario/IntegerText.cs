using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Hyperway.scenario.ui {
    using text = TextMeshProUGUI;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class IntegerText : MonoBehaviour {
        public byte digits_count;
        public char leading_char;

        public uint value;

        char[] chars;
        uint?  last_value;
        uint   max_value;

        TextMeshProUGUI text;

        void Start() {
            last_value = default;
            chars = new char[digits_count];
            var max = 1u;
            for (var i = 0; i < digits_count; i++) {
                max *= 10;
            }
            max -= 1;
            max_value = max;
            
            text = GetComponent<TextMeshProUGUI>();
        }

        void Update() {
            var amount = value;
            if (amount != last_value) {
                update_chars(this, amount);
                text.SetText(chars);
                last_value = amount;
            }
        }

        static void update_chars(IntegerText text, uint value) {
            value = math.clamp(value, 0, text.max_value);
            var chars        = text.chars;
            var digits_count = text.digits_count;
            
            if (chars.Length != digits_count) text.chars = new char[digits_count];
            
            var measure = 1u;
            var measured_amount = math.max(1, value);
            for (var i = digits_count - 1; i >= 0; i--) {
                chars[i] = measured_amount >= measure ? get_digit(value, measure) : text.leading_char;
                measure *= 10;
            }
        }

        static char get_digit(uint amount, uint measure) => (char) ('0' + (amount / measure) % 10);
    }
}