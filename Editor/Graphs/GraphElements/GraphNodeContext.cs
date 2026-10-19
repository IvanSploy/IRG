using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Editor
{
    public struct GraphNodeContext
    {
        public string Title;
        public bool HasInput;
        public bool HasOutput;
        public Color TitleColor;
        public Color HeaderColor;
        public StyleLength MinWidth;
        public StyleLength MaxWidth;

        public static GraphNodeContext Default()
        {
            GraphNodeContext context = new GraphNodeContext
            {
                Title = "Title",
                HasInput = true,
                HasOutput = true,
                TitleColor = Color.white,
                HeaderColor = Color.black,
                MinWidth = 150f,
                MaxWidth = 300f,
            };
            return context;
        }

        public static GraphNodeContext Get(Type type)
        {
            GraphNodeContext context = Default();
            if (type.IsDefined(typeof(GraphNodeAttribute), false))
            {
                var attr = type.GetCustomAttribute<GraphNodeAttribute>();
                context.Title = attr.Title;
                context.HeaderColor = GetColor(attr.Color);
                
            }
            else
            {
                context.Title = type.Name.RemoveFromEnd("data").WithSpaces();
            }
            return context;
        }

        private static Color GetColor(string colorName, Color defaultColor = default)
        {
            PropertyInfo property = typeof(Color).GetProperty(colorName, BindingFlags.Public | BindingFlags.Static);

            if (property != null && property.PropertyType == typeof(Color))
            {
                return (Color)property.GetValue(null);
            }

            return defaultColor;
        }
    }
}