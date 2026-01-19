using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IRG.Utils
{
    [Serializable]
    public class AbstractList<T> : IEnumerable<T>
    {
        [SerializeReference] public List<T> List = new();

        public int Count => List.Count;
        
        public T this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}