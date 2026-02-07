using System;
using System.Runtime.CompilerServices;
using Object = UnityEngine.Object;

namespace IRG
{
    public static class EasyDebug
    {
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void Log(string message)
        {
            UnityEngine.Debug.Log(AddHeader(message));
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void Log(string message, Object context)
        {
            UnityEngine.Debug.Log(AddHeader(message), context);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning(AddHeader(message));
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void LogWarning(string message, Object context)
        {
            UnityEngine.Debug.LogWarning(AddHeader(message), context);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void LogError(string message)
        {
            UnityEngine.Debug.LogError(AddHeader(message));
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void LogError(string message, Object context)
        {
            UnityEngine.Debug.LogError(AddHeader(message), context);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public static void LogException(Exception exception, Object context)
        {
            UnityEngine.Debug.LogException(exception, context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string AddHeader(string message)
        {
            return  $"[IRG] {message}";
        }
    }
}