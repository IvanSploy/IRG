using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace IRG.Graphs.Editor
{
    public class GraphSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private Texture2D _indentIcon;
        private Type _type;
        private string _filter;
        private Func<SearchTreeEntry, SearchWindowContext, bool> _onSelect;

        public void Initialize(Type type, string filter, Func<SearchTreeEntry, SearchWindowContext, bool> onSelect)
        {
            _type = type;
            _filter = filter.ToLowerInvariant();
            _onSelect = onSelect;
            
            _indentIcon = new Texture2D(1, 1);
            _indentIcon.SetPixel(0,0, Color.clear);
            _indentIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent($"Cinematic Nodes")),
            };
            
            var types = TypeCache.GetTypesDerivedFrom(_type).Where(type => !type.IsAbstract).ToList();
            types.Sort((item1, item2) =>
                String.Compare(item1.Name, item2.Name, StringComparison.InvariantCulture));

            Dictionary<string, List<Type>> groups = new();
            foreach (var type in types)
            {
                if(type == typeof(InitialNode)) continue;

                var graphNodeAttr = type.GetCustomAttribute<GraphNodeAttribute>();
                if (graphNodeAttr == null)
                {
                    groups.AddToList("", type);
                    continue;
                }
                
                if (!string.IsNullOrEmpty(_filter) && !string.IsNullOrEmpty(graphNodeAttr.Filter) 
                                                   && graphNodeAttr.Filter != _filter)
                    continue;

                groups.AddToList(string.IsNullOrEmpty(graphNodeAttr.Group) ? "" : graphNodeAttr.Group, type);
            }

            foreach (var group in groups)
            {
                entries.AddRange(CreateEntries(group.Key, group.Value, 2));
            }
            entries.Add(new SearchTreeEntry(new GUIContent("Group", _indentIcon))
            {
                userData = typeof(GraphGroup),
                level = 1,
            });
            return entries;
        }
        
        private List<SearchTreeEntry> CreateEntries(string group, List<Type> types, int level)
        {
            var entries = new List<SearchTreeEntry>();
            if (!string.IsNullOrEmpty(group))
                entries.Add(new SearchTreeGroupEntry(new GUIContent(group), level - 1));
            else level--;
            
            foreach (var type in types)
            {
                entries.Add(new SearchTreeEntry(new GUIContent($"     {type.Name.WithSpaces()}"))
                {
                    userData = type,
                    level = level
                });
            }
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            return _onSelect.Invoke(searchTreeEntry,  context);
        }
    } 
}
