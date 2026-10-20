using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace IRG.Graphs
{
    public static class NodeFieldDrawerFactory
    {
        public record NodeFieldDrawerComparer : IComparer<Type>
        {
            public int Compare(Type x, Type y)
            {
                if(x == y) return 0;
                if(x == null || y == null) return 0;
                
                var xAttr = x.GetCustomAttribute<NodeFieldDrawerAttribute>();
                var yAttr = y.GetCustomAttribute<NodeFieldDrawerAttribute>();
                
                int compare = xAttr.Priority - yAttr.Priority;
                if (compare != 0) return compare;
                return -1;
            }
        }
        
        private static readonly SortedSet<Type> _drawers = new(new NodeFieldDrawerComparer());
        
        private static void Initialize()
        {
            _drawers.Clear();
            var types =
#if UNITY_EDITOR
                UnityEditor.TypeCache.GetTypesWithAttribute<NodeFieldDrawerAttribute>()
                    .Where(t => typeof(NodeFieldDrawer).IsAssignableFrom(t) && !t.IsAbstract);
#else
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly =>
                    assembly.GetTypes().Where(t => typeof(FieldDrawer).IsAssignableFrom(t) && t.IsDefined(typeof(FieldDrawerAttribute)) && !t.IsAbstract));
#endif
            foreach (var type in types)
            {
                _drawers.Add(type);
            }
        }
        
        public static List<NodeFieldDrawer> CreateDrawers(object data)
        {
            Initialize();
            List<NodeFieldDrawer> drawers = new List<NodeFieldDrawer>();
            FieldInfo[] fields = data.GetType().GetFields();
            foreach (var field in fields)
            {
                var drawer = CreateDrawer(field, data);
                if(drawer != null) drawers.Add(drawer);
            }

            return drawers;
        }

        public static NodeFieldDrawer CreateDrawer(FieldInfo field, object data)
        {
            if (field.IsDefined(typeof(HideInNodeAttribute))) return null;
            if (!field.IsPublic && !field.IsDefined(typeof(SerializeField))) return null;
            
            foreach (var drawer in _drawers)
            {
                var drawerAttr = drawer.GetCustomAttribute<NodeFieldDrawerAttribute>();
                bool valid = true;
                if (field.IsDefined(typeof(ShowIfAttribute)))
                {
                    var showIfAttr = field.GetCustomAttribute<ShowIfAttribute>();
                    var methodInfo = data.GetType().GetMethod(showIfAttr.ConditionName);
                    if (methodInfo != null && methodInfo.IsPublic && methodInfo.ReturnType == typeof(bool))
                    {
                        valid &= (bool)methodInfo.Invoke(data, null);
                    }
                }

                valid &= drawerAttr.Type == field.FieldType ||
                         (drawerAttr.AllowChildren && drawerAttr.Type.IsAssignableFrom(field.FieldType));
                if (valid)
                {
                    NodeFieldDrawer nodeFieldDrawer = (NodeFieldDrawer)Activator.CreateInstance(drawer);
                    nodeFieldDrawer.Initialize(field, data);
                    if (nodeFieldDrawer.IsValid()) return nodeFieldDrawer;
                }
            }

            return null;
        }
        
        public static NodeFieldDrawer CreateIndexedDrawer(FieldInfo field, object data, IList list, int index)
        {
            Type elementType = list.GetType().IsArray
                ? list.GetType().GetElementType() 
                : list.GetType().GenericTypeArguments[0];
            
            foreach (var drawer in _drawers)
            {
                var drawerAttr = drawer.GetCustomAttribute<NodeFieldDrawerAttribute>();
                bool valid;
                if (drawerAttr.AllowChildren) valid = drawerAttr.Type.IsAssignableFrom(elementType);
                else valid = drawerAttr.Type == elementType;
                if (valid)
                {
                    NodeFieldDrawer nodeFieldDrawer = (NodeFieldDrawer)Activator.CreateInstance(drawer);
                    nodeFieldDrawer.Initialize(field, data, list, index);
                    if (nodeFieldDrawer.IsValid()) return nodeFieldDrawer;
                }
            }

            return null;
        }
    }
}