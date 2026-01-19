using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IRG.Addressable.Editor
{
    [CustomPropertyDrawer(typeof(AddressableElementAttribute), true)]
    public class AddressableElementAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var vfxAttribute = (AddressableElementAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label);

                // Indentation for foldout
                position.x += EditorGUIUtility.labelWidth;
                position.width -= EditorGUIUtility.labelWidth;

                var options = new List<string> { "None" };
                var addresses = GetAddressesInGroup(vfxAttribute.AssetGroup).Select(name => name.Replace(' ', '/')).ToArray();
                options.AddRange(addresses);
                var value = property.stringValue.Replace(' ', '/');
                var previousIndex = options.IndexOf(value);
                var selectedIndex = Mathf.Max(0, previousIndex);

                selectedIndex = EditorGUI.Popup(position, selectedIndex, options.ToArray());
                if (selectedIndex > 0)
                {
                    property.stringValue = options[selectedIndex].Replace('/', ' ');
                }
                else if (previousIndex >= 0)
                {
                    property.stringValue = "";
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use AddressableElementAttribute with string.");
            }
        }
        
        public static List<string> GetAddressesInGroup(string groupName)
        {
            var addresses = new List<string>();
            var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            var group = settings.FindGroup(groupName);

            if (group == null)
            {
                UnityEngine.Debug.LogWarning($"Group {groupName} not found.");
                return addresses;
            }

            foreach (var entry in group.entries)
            {
                addresses.Add(entry.address);
            }
            return addresses;
        }
    }
}
