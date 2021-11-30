using UnityEngine;
using UnityEngine.UI;
using Utilities.GameObjects;

namespace Utilities.UI {
    public static class ScrollRectExtensions {
        public static void ensure_horizontal_visibility(this ScrollRect scroll_rect, RectTransform element, float padding = 0) {
            var c = scroll_rect.content;
            var cw = c.rect.width;
            var e0 = cw - element.anchoredPosition.x;
            var e1 = e0 + element.rect.width;
            e0 -= padding;
            e1 += padding;

            var vp = scroll_rect.viewport;
            if (vp == null)
                vp = scroll_rect.get<RectTransform>();

            var vw = vp.rect.width;
            var v0 = c.anchoredPosition.x;
            var v1 = v0 + vw;

            float d;
            if (e1 > v1)
                d = e1 - v1;
            else if (e0 < v0) 
                d = e0 - v0;
            else
                return;

            var cpos = c.anchoredPosition;
            cpos.x += d;
            c.anchoredPosition = cpos;
        }
    }
}