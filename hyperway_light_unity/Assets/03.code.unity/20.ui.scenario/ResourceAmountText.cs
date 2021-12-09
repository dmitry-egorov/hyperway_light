using TMPro;
using Unity.Mathematics;
using UnityEngine;
using static Hyperway.hyperway;

namespace Hyperway.scenario.ui {
    using text = TextMeshProUGUI;

    [RequireComponent(typeof(text))]
    public class ResourceAmountText : MonoBehaviour {
        public byte digits_count;
        public char leading_char;

        res_type type; // set by ResourceAmountBlock

        char[] chars;
        uint?  last_amount;
        uint   max_value;

        text text;

        public void set_type(res_type type) => this.type = type;

        void Start() {
            last_amount = default;
            chars = new char[digits_count];
            var max = 1u;
            for (var i = 0; i < digits_count; i++) {
                max *= 10;
            }
            max -= 1;
            max_value = max;
            
            text = GetComponent<text>();
        }

        void Update() {
            var amount = _stats.total_stored[type];
            if (amount != last_amount) {
                update_chars(this, amount);
                text.SetText(chars);
                last_amount = amount;
            }
        }

        static void update_chars(ResourceAmountText shower, uint amount) {
            amount = math.clamp(amount, 0, shower.max_value);
            var chars        = shower.chars;
            var digits_count = shower.digits_count;
            
            if (chars.Length != digits_count) shower.chars = new char[digits_count];
            
            var measure = 1u;
            var measured_amount = math.max(1, amount);
            for (var i = digits_count - 1; i >= 0; i--) {
                chars[i] = measured_amount >= measure ? get_digit(amount, measure) : shower.leading_char;
                measure *= 10;
            }
        }

        static char get_digit(uint amount, uint measure) => (char) ('0' + (amount / measure) % 10);
    }
}