using System;
using System.Collections.Generic;
using System.Linq;

namespace IRG
{
    public static class GenericFactory<TGeneric> where TGeneric : class
    {
        private static readonly Dictionary<Type, Type> _map = new();

        private static void Initialize()
        {
            _map.Clear();
            var types =
#if UNITY_EDITOR
                UnityEditor.TypeCache.GetTypesDerivedFrom<TGeneric>()
                    .Where(t => !t.IsAbstract);
#else
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly =>
                    assembly.GetTypes().Where(t => typeof(TGeneric).IsAssignableFrom(t) && !t.IsAbstract));
#endif

            foreach (var type in types)
            {
                Type baseType = type.BaseType;
                while (baseType is { IsGenericType: false })
                {
                    baseType = baseType.BaseType;
                }
                if (baseType == null) continue;
                
                var attr = baseType.GetGenericArguments();
                if (attr.Length > 0) _map[attr[0]] = type;
            }
        }
        
        public static Type GetKey(Type type)
        {
            foreach (var kvp in _map)
            {
                if (kvp.Value == type)
                    return kvp.Key;
            }

            Initialize();
            
            foreach (var kvp in _map)
            {
                if (kvp.Value == type)
                    return kvp.Key;
            }

            throw new InvalidOperationException($"Type {type} is not registered.");
        }

        public static TGeneric Create(Type type)
        {
            if(!_map.ContainsKey(type)) Initialize();
            if (_map.TryGetValue(type, out var graphNodeType))
            {
                return (TGeneric)Activator.CreateInstance(graphNodeType);
            }

            throw new InvalidOperationException($"No element registered for data type {type.Name}");
        }
    }
}