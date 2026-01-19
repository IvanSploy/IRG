/*using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IRG;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace IRG
{
    public static class FxManager
    {
        private static readonly Dictionary<string, GameObject> _fxPool = new();
        private static readonly Dictionary<string, List<GameObject>> _spawnedFx = new();
        
        public static GameObject Create(string name, float duration, Transform parent, Vector3 localOffset = default)
        {
            var fx = Get(name);
            var instance = Object.Instantiate(fx, parent);
            instance.transform.localPosition = localOffset;
            DestroyAfterSeconds(instance, duration);
            return instance;
        }
        
        public static GameObject Create(string name, string id, Transform parent, Vector3 localOffset = default)
        {
            var fx = Get(name);
            var instance = Object.Instantiate(fx, parent);
            instance.transform.localPosition = localOffset;
            _spawnedFx.AddToList(id, instance);
            return instance;
        }
        
        public static GameObject Create(string name, float duration, Vector3 pos)
        {
            var fx = Get(name);
            var instance = Object.Instantiate(fx);
            instance.transform.position = pos;
            DestroyAfterSeconds(instance, duration);
            return instance;
        }
        
        public static GameObject Create(string name, string id, Vector3 pos)
        {
            var fx = Get(name);
            var instance = Object.Instantiate(fx);
            instance.transform.position = pos;
            _spawnedFx.AddToList(id, instance);
            return instance;
        }

        public static void Destroy(string id)
        {
            if (!_spawnedFx.Remove(id, out var fxs)) return;
            foreach (var fx in fxs)
            {
                Object.DestroyImmediate(fx);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static async void DestroyAfterSeconds(GameObject fx, float duration)
        {
            try
            {
                var initialTime = Time.time;
                while (Time.time - initialTime < duration) { await Task.Yield();}
                Object.DestroyImmediate(fx);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static GameObject Get(string name)
        {
            if (_fxPool.TryGetValue(name, out var value)) return value;
            return Addressables.LoadAssetAsync<GameObject>(name).WaitForCompletion();
        }
    }
}*/