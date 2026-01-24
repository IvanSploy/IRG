using System;

namespace IRG
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AbstractNameAttribute : Attribute
    {
        public string Name { get; }

        public AbstractNameAttribute(string name)
        {
            Name = name;
        }
    }
}