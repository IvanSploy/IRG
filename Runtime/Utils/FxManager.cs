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
        private static readonly Dictionary<string, GameObject> _fxPool = new();
        
        public static GameObject Create(string name, Transform parent, Vector3 localOffset = default, float duration = 0)
        {
            var fx = Get(name);
            var instance = Object.Instantiate(fx, parent);
            instance.transform.localPosition = localOffset;
            if(duration > 0) DestroyAfterSeconds(instance, duration);
            return instance;
        }
        
        public static GameObject Create(string name, Vector3 pos, float duration = 0)
        {
            var fx = Get(name);
            var instance = Object.Instantiate(fx);
            instance.transform.position = pos;
            if(duration > 0) DestroyAfterSeconds(instance, duration);
            return instance;
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
}