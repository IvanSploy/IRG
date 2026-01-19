using UnityEngine;

namespace IRG.Addressable
{
    public class AddressableElementAttribute : PropertyAttribute
    {
        public string AssetGroup;
        
        public AddressableElementAttribute(string assetGroup)
        {
            AssetGroup = assetGroup;
        }
    }
}