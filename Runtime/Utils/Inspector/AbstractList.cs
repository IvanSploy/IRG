using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IRG
{
    [Serializable]
    public class AbstractList<T> : IList<T>
    {
        [SerializeReference] public List<T> List = new();

        public int Count => List.Count;
        public bool IsReadOnly => false;
        
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

        public void Add(T item)
        {
            List.Add(item);
        }

        public void Clear()
        {
            List.Clear();
        }

        public bool Contains(T item)
        {
            return List.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return List.Remove(item);
        }
        
        public int IndexOf(T item)
        {
            return List.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            List.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            List.RemoveAt(index);
        }
    }
}