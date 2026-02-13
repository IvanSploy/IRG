using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace IRG.Editor
{
    public class TypeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public Type Type;
        public Action<Type, SearchWindowContext> OnSelect;
        public bool AllowNone;
        protected Texture2D _indentIcon;

        public void Init(Type type, Action<Type, SearchWindowContext> onSelect, bool allowNone = false)
        {
            Type = type;
            OnSelect = onSelect;
            AllowNone = allowNone;
            
            _indentIcon = new Texture2D(1, 1);
            _indentIcon.SetPixel(0,0, Color.clear);
            _indentIcon.Apply();
        }
        
        public virtual List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var typeName = Type.Name.WithSpaces();
            typeName = typeName.Replace("I ", "");
            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent($"{typeName}")),
            };
            
            if (AllowNone)
            {
                entries.Add(new SearchTreeEntry(new GUIContent($"     None"))
                {
                    userData = null,
                    level = 1
                });
            }
            
            var types = TypeCache.GetTypesDerivedFrom(Type).Where(t => !t.IsInterface).ToList();
            types.Sort((item1, item2) =>
            {
                if(item1.IsAbstract && item2.IsAbstract) return String.Compare(item1.Name, item2.Name, StringComparison.InvariantCulture);
                if (item1.IsAbstract) return -1;
                if (item2.IsAbstract) return 1;
                return String.Compare(item1.Name, item2.Name, StringComparison.InvariantCulture);
            });
            
            var groups = new Dictionary<Type, List<Type>>();
            foreach (var type in types)
            {
                var parent = type.BaseType;
                
                if (Type.IsInterface)
                {
                    if (parent == null || !parent.GetInterfaces().Contains(Type))
                    {
                        groups.AddToList(Type, type);
                        continue;
                    }
                }
                
                groups.AddToList(parent, type);
            }

            entries.AddRange(CreateEntries(groups, Type, 1));
            return entries;
        }
        
        private List<SearchTreeEntry> CreateEntries(Dictionary<Type, List<Type>> groups, Type parentType, int level)
        {
            var entries = new List<SearchTreeEntry>();
            if(!groups.TryGetValue(parentType, out var group)) return entries;
            foreach (var type in group)
            {
                if (type.IsAbstract)
                {
                    entries.Add(new SearchTreeGroupEntry(new GUIContent($"{type.Name.WithSpaces()}", _indentIcon))
                    {
                        userData = type,
                        level = level
                    });
                    entries.AddRange(CreateEntries(groups, type, level + 1));
                }
                else
                {
                    entries.Add(new SearchTreeEntry(new GUIContent($"     {type.Name.WithSpaces()}"))
                    {
                        userData = type,
                        level = level
                    });
                }
            }
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry.userData == null)
            {
                OnSelect?.Invoke(null, context);
                return true;
            }
            
            if (searchTreeEntry.userData is Type type)
            {
                OnSelect?.Invoke(type, context);
                return true;
            }
            
            return false;
        }
    } 
}
