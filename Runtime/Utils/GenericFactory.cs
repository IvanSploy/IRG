using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IRG
{
    public static class GenericFactory<TGeneric> where TGeneric : class
    {
        private static bool _isInit;
        private static readonly Dictionary<Type, Type> _map = new();

        private static void Initialize()
        {
            _map.Clear();
            var types = Assembly.GetAssembly(typeof(TGeneric))
                .GetTypes()
                .Where(t => typeof(TGeneric).IsAssignableFrom(t) && !t.IsAbstract);

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

            _isInit = true;
        }
        
        public static Type GetKey(Type type)
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

        public static TGeneric Create(Type type)
        {
            if(!_isInit) Initialize();
            if (_map.TryGetValue(type, out var graphNodeType))
            {
                return (TGeneric)Activator.CreateInstance(graphNodeType);
            }

            _isInit = false;
            throw new InvalidOperationException($"No element registered for data type {type.Name}");
        }
    }
}