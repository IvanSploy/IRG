using System.Collections.Generic;
using UnityEngine;

namespace IRG
{
    public class ScriptableObjectList<TData> : ScriptableObject
    {
        public List<TData> Data = new();
    }
}