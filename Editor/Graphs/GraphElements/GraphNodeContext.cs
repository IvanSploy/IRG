using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Editor
{
    public struct GraphNodeContext
    {
        public string Title;
        public PortType InputType;
        public List<string> InputIds;
        public PortType OutputType;
        public List<string> OutputIds;
        public Color TitleColor;
        public Color HeaderColor;
        public StyleLength MinWidth;
        public StyleLength MaxWidth;

        public static GraphNodeContext Default()
        {
            GraphNodeContext context = new GraphNodeContext
            {
                Title = "Title",
                InputType = PortType.Fixed,
                InputIds = new List<string>() { "In" },
                OutputType = PortType.Fixed,
                OutputIds = new List<string>() { "Next" },
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
            context.Title = type.Name.RemoveFromEnd("data").WithSpaces();
            
            if (type.IsDefined(typeof(GraphNodeAttribute), false))
            {
                var attr = type.GetCustomAttribute<GraphNodeAttribute>();
                if(!string.IsNullOrEmpty(attr.Title)) context.Title = attr.Title;
                context.HeaderColor = GetColor(attr.Color);
                context.OutputType = attr.OutputPortType;
            }
            return context;
        }

        private static Color GetColor(string colorName, Color defaultColor = default)
        {
            if (string.IsNullOrEmpty(colorName)) return defaultColor;
            PropertyInfo property = typeof(Color).GetProperty(colorName, BindingFlags.Public | BindingFlags.Static);

            if (property != null && property.PropertyType == typeof(Color))
            {
                return (Color)property.GetValue(null);
            }

            return defaultColor;
        }
    }
}