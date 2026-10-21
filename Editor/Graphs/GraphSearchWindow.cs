using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace IRG.Graphs.Editor
{
    public class GraphSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private Texture2D _indentIcon;
        private string _filter;
        private Func<SearchTreeEntry, SearchWindowContext, bool> _onSelect;

        public void Init(string filter, Func<SearchTreeEntry, SearchWindowContext, bool> onSelect)
        {
            _filter = filter.ToLowerInvariant();
            _onSelect = onSelect;
            
            _indentIcon = new Texture2D(1, 1);
            _indentIcon.SetPixel(0,0, Color.clear);
            _indentIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var types = TypeCache.GetTypesDerivedFrom(typeof(NodeData)).Where(type => !type.IsAbstract).ToList();
            types.Sort((item1, item2) =>
                String.Compare(item1.Name, item2.Name, StringComparison.InvariantCulture));

            Dictionary<string, List<Type>> groups = new();
            foreach (var type in types)
            {
                if(type == typeof(InitialNodeData)) continue;

                var ctx = NodeContext.Get(type);
                
                if (string.IsNullOrEmpty(ctx.Filter) && string.IsNullOrEmpty(ctx.Group))
                {
                    groups.AddToList("", type);
                    continue;
                }
                
                if (!string.IsNullOrEmpty(_filter) && !string.IsNullOrEmpty(ctx.Filter) 
                                                   && !ctx.Filter.Equals(_filter, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                string groupName = string.IsNullOrEmpty(ctx.Group) ? "" : ctx.Group;
                groups.AddToList(groupName, type);
            }

            string filterName = string.IsNullOrEmpty(_filter) ? "All" : _filter.StartWithUpper();
            
            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent($"{filterName} Nodes")),
            };
            
            var sortedGroupNames = groups.Keys.ToList();
            sortedGroupNames.Sort((item1, item2) => String.Compare(item1, item2, StringComparison.InvariantCulture));

            foreach (var key in sortedGroupNames)
            {
                entries.AddRange(CreateEntries(key, groups[key], 2));
            }
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
                var ctx = NodeContext.Get(type);
                entries.Add(new SearchTreeEntry(new GUIContent($"     {ctx.Title}"))
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
