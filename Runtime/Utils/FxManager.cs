using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace IRG
{
    public static class FxManager
    {
        private const int PoolSize = 20;
        private static readonly Dictionary<string, GameObject> _prefabCache = new();
        private static readonly DictionaryPool<string, GameObject> _instancesPool = new(
            key =>
            {
                var fx = Get(key);
                return Object.Instantiate(fx);
            },
            go => go.SetActive(true),
            go => go.SetActive(false),
            Object.Destroy,
            maxSize: PoolSize
            );
        
        public static GameObject Create(string key, Transform parent, Vector3 localOffset = default, float duration = 0)
        {
            var instance = _instancesPool.Get(key);
            instance.transform.SetParent(parent);
            instance.transform.localPosition = localOffset;
            if(duration > 0) ReleaseAfterSeconds(key, instance, duration);
            return instance;
        }
        
        public static GameObject Create(string key, Vector3 pos, float duration = 0)
        {
            var instance = _instancesPool.Get(key);
            instance.transform.position = pos;
            if(duration > 0) ReleaseAfterSeconds(key, instance, duration);
            return instance;
        }
        
        public static void Release(string key, GameObject instance)
        {
            _instancesPool.Release(key, instance);
        }

        public static void Clear()
        {
            _prefabCache.Clear();
            _instancesPool.Clear();
        }
        
        private static GameObject Get(string key)
        {
            if (_prefabCache.TryGetValue(key, out var go)) return go;
            GameObject gameObject = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();
            _prefabCache.Add(key, gameObject);
            return gameObject;
        }

        private static async void ReleaseAfterSeconds(string key, GameObject fx, float duration)
        {
            try
            {
                var initialTime = Time.time;
                while (Time.time - initialTime < duration) { await Task.Yield();}
                Release(key, fx);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}