using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace IRG.Graphs.Drawers
{
    [NodeFieldDrawer(typeof(Object), 99, true)]
    public class ImageDrawer : NodeFieldDrawer
    {
        public override bool IsValid()
        {
            return FieldType == typeof(Texture2D)
            || FieldType == typeof(Sprite)
            || FieldType == typeof(VectorImage);
        }

        public override VisualElement Build()
        {
            VisualElement container = new VisualElement {};
            container.style.flexDirection = FlexDirection.Row;
            container.style.height = 75f;

            ObjectField selector = new ObjectField(Name);
            selector.objectType = FieldType;
            selector.style.flexGrow = 0f;
            selector.style.flexShrink = 0f;
            selector.style.alignItems = Align.Center;
            
            selector.SetValueWithoutNotify((Object)GetValue());
            selector.RegisterValueChangedCallback(evt =>
            {
                SetValue(evt.newValue);
            });
            container.Add(selector);

            VisualElement image = new VisualElement();
            image.style.aspectRatio = 1f;
            image.style.flexGrow = 1f;
            image.style.flexShrink = 1f;
            image.style.backgroundSize = new StyleBackgroundSize(new BackgroundSize(BackgroundSizeType.Contain));
            
            Refresh();
            OnValueChanged += Refresh;
            container.Add(image);
            return container;
            
            void Refresh()
            {
                image.style.backgroundImage = GetValue() switch
                {
                    Texture2D texture2D => new StyleBackground(texture2D),
                    Sprite sprite => new StyleBackground(sprite),
                    VectorImage vectorImage => new StyleBackground(vectorImage),
                    _ => null
                };
            }
        }
    }
}