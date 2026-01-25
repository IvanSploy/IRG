using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IRG
{
    public static class AbstractFactory<TAbstract> where TAbstract : class
    {
        private static readonly Dictionary<string, Type> _map = new();

        private static void Initialize()
        {
            _map.Clear();
            var types =
#if UNITY_EDITOR
                UnityEditor.TypeCache.GetTypesDerivedFrom<TAbstract>()
                .Where(t => !t.IsAbstract);
#else
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly =>
                    assembly.GetTypes().Where(t => typeof(TAbstract).IsAssignableFrom(t) && !t.IsAbstract));
#endif
            
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<AbstractNameAttribute>();
                if (attr != null)
                {
                    if (!_map.TryAdd(attr.Name, type))
                        throw new Exception($"Duplicated attribute key '{attr.Name}' for type {type.FullName}");
                }
                
                if (!_map.TryAdd(type.Name, type))
                    throw new Exception($"Duplicated class key '{type.Name}' for type {type.FullName}");
            }
        }

        public static string GetKey(Type type)
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

        public static TAbstract Create(string id)
        {
            if (!_map.ContainsKey(id)) Initialize();
            if (_map.TryGetValue(id, out var type)) return (TAbstract)Activator.CreateInstance(type);
            
            throw new InvalidOperationException($"Unknown {nameof(TAbstract)} for '{id}'");
        }
    }
}