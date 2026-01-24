using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.ResourceManagement.Exceptions;

namespace IRG
{
    public static class AbstractFactory<TAbstract> where TAbstract : class
    {
        private static bool _isInit;
        private static readonly Dictionary<string, Type> _map = new();

        private static void Initialize()
        {
            _map.Clear();
            var types = Assembly.GetAssembly(typeof(TAbstract))
                .GetTypes()
                .Where(t => typeof(TAbstract).IsAssignableFrom(t) && !t.IsAbstract);
            
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

            if(_map.Count > 0) _isInit = true;
        }

        public static string GetKey(Type type)
        {
            if (!_isInit) Initialize();
            foreach (var kvp in _map)
            {
                if (kvp.Value == type)
                    return kvp.Key;
            }

            _isInit = false;
            throw new InvalidOperationException($"Type {type} is not registered.");
        }

        public static TAbstract Create(string id)
        {
            if (!_isInit) Initialize();
            if (_map.TryGetValue(id, out var type)) return (TAbstract)Activator.CreateInstance(type);
            
            _isInit = false;
            throw new InvalidOperationException($"Unknown {nameof(TAbstract)} for '{id}'");
        }
    }
}