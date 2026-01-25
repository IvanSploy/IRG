using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Editor
{
    public sealed class GraphGroup : Group, IConvertible<GroupData>
    {
        public string ID { get; private set; }

        public void Initialize(string groupTitle, Vector2 position, string id = null)
        {
            ID = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            title = groupTitle;
            SetPosition(new Rect(position, Vector2.zero));
        }

        public void SetTextColor(Color color)
        {
            this.Q<Label>().style.color = color;
        }

        public GroupData ToData()
        {
            var groupData = new GroupData
            {
                ID = ID,
                Name = title,
                Position = GetPosition().position,
            };
            return groupData;
        }
        
        public void FromData(GroupData groupData)
        {
            Initialize(groupData.Name, groupData.Position, groupData.ID);
        }
    }
}