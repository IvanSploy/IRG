using System;

namespace IRG
{
    public class EnumAttribute : FolderAttribute
    {
        public Type EnumType;
        
        public EnumAttribute(Type type)
        {
            EnumType = type;
            var values = Enum.GetValues(type);
            Options = new String[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                Options[i] = values.GetValue(i).ToString();
            }
        }
    }
}