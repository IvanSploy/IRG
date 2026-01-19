using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG
{
    public static class UIExtensions
    {
        public static T AddTo<T>(this T element, VisualElement child) where T : VisualElement
        {
            element.Add(child);
            return element;
        }

        public static T WithManipulator<T>(this T element, Manipulator manipulator) where T : VisualElement
        {
            element.AddManipulator(manipulator);
            return element;
        }

        #region Utilities

        public static void SetInteractable(this VisualElement element, bool interactable, bool recursive = true)
        {
            element.pickingMode = interactable ? PickingMode.Position : PickingMode.Ignore;
            for (int i = 0; i < element.hierarchy.childCount; i++)
                element.hierarchy.ElementAt(i).SetInteractable(interactable);
        }

        public static void SetDisplay(this VisualElement element, bool enabled)
        {
            element.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public static VisualElement GetRoot(this VisualElement element)
        {
            return element.parent == null ? element : element.parent.GetRoot();
        }

        public static T AscendantQuery<T>(this VisualElement element) where T : VisualElement
        {
            if (element is T tElement) return tElement;
            if (element.parent == null) return null;
            return element.parent.AscendantQuery<T>();
        }

        public static void SetMargin(this VisualElement element, int margin)
        {
            element.style.marginTop = margin;
            element.style.marginRight = margin;
            element.style.marginBottom = margin;
            element.style.marginLeft = margin;
        }

        public static void SetPadding(this VisualElement element, int margin)
        {
            element.style.paddingTop = margin;
            element.style.paddingRight = margin;
            element.style.paddingBottom = margin;
            element.style.paddingLeft = margin;
        }

        #endregion

        #region Styling

        public static void SetFocus(this VisualElement element, bool focus)
        {
            element.RemoveFromClassList("focus");
            if (focus) element.AddToClassList("focus");
        }

        public static void SetAvailable(this VisualElement element, bool available)
        {
            element.ClearAvailable();

            if (available) element.AddToClassList("available");
            else element.AddToClassList("unavailable");
        }

        public static void ClearAvailable(this VisualElement element)
        {
            element.RemoveFromClassList("available");
            element.RemoveFromClassList("unavailable");
        }

        public static void ClearRarity(this VisualElement element)
        {
            element.RemoveFromClassList("rarity-common");
            element.RemoveFromClassList("rarity-uncommon");
            element.RemoveFromClassList("rarity-epic");
            element.RemoveFromClassList("rarity-legendary");
        }

        public static void AddRange(this VisualElementStyleSheetSet set, IEnumerable<StyleSheet> styleSheets)
        {
            foreach (var styleSheet in styleSheets)
            {
                set.Add(styleSheet);
            }
        }

        public static void AddRange(this VisualElement set, IEnumerable<VisualElement> visualElements)
        {
            foreach (var visualElement in visualElements.ToList())
            {
                set.Add(visualElement);
            }
        }

        #endregion

        #region Label

        public enum AdaptMode
        {
            Width,
            Height,
            Both
        }

        public static void AutoSize(this Label label, AdaptMode adaptMode)
        {
            if (label.contentRect.size.x <= 0 || label.contentRect.size.y <= 0) return;

            string afterTrim = label.text.Trim();
            if (string.IsNullOrEmpty(afterTrim)) return;

            Vector2 labelContentSize = label.contentRect.size;

            Vector2 measureTextSize = CalculateMeasureTextSize(label, labelContentSize);

            //Decrease the font size
            while (CheckSizeDown(adaptMode, measureTextSize, labelContentSize))
            {
                label.style.fontSize = label.resolvedStyle.fontSize - 1;
                measureTextSize = CalculateMeasureTextSize(label, labelContentSize);
                if (label.resolvedStyle.fontSize <= 0) break;
            }

            //Increase the font size
            while (CheckSizeUp(adaptMode, measureTextSize, labelContentSize))
            {
                label.style.fontSize = label.resolvedStyle.fontSize + 1;
                measureTextSize = CalculateMeasureTextSize(label, labelContentSize);

                // We need this control because last visual of text can be oversize of the text area.
                if (label.resolvedStyle.fontSize > 1 && CheckSizeDown(adaptMode, measureTextSize, labelContentSize))
                {
                    label.style.fontSize = label.resolvedStyle.fontSize - 1;
                }
            }

            label.style.fontSize = Mathf.Max(1, label.resolvedStyle.fontSize);
        }

        private static bool CheckSizeUp(AdaptMode adaptMode, Vector2 measureTextSize, Vector2 labelContentSize)
        {
            return adaptMode switch
            {
                AdaptMode.Width => measureTextSize.x < labelContentSize.x,
                AdaptMode.Height => measureTextSize.y < labelContentSize.y,
                AdaptMode.Both => measureTextSize.x < labelContentSize.x && measureTextSize.y < labelContentSize.y,
                _ => false,
            };
        }

        private static bool CheckSizeDown(AdaptMode adaptMode, Vector2 measureTextSize, Vector2 labelContentSize)
        {
            return adaptMode switch
            {
                AdaptMode.Width => measureTextSize.x > labelContentSize.x,
                AdaptMode.Height => measureTextSize.y > labelContentSize.y,
                AdaptMode.Both => measureTextSize.x > labelContentSize.x && measureTextSize.y > labelContentSize.y,
                _ => false,
            };
        }

        private static Vector2 CalculateMeasureTextSize(Label label, Vector2 labelContentSize)
        {
            return label.MeasureTextSize(label.text, labelContentSize.x, VisualElement.MeasureMode.Undefined,
                labelContentSize.y, VisualElement.MeasureMode.Undefined);
        }

        #endregion
    }
}