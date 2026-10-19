using System;

namespace IRG.Graphs
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : Attribute
    {
        public string ConditionName { get; private set; }
        public ShowIfAttribute(string conditionName)
        {
            ConditionName = conditionName;
        }
    }
}