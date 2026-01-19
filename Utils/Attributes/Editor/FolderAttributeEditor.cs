using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FolderAttribute), true)]
public class FolderAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        FolderAttribute folderAttribute = (FolderAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label);

            // Indentation for foldout
            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            var options = new List<string> { "None" };
            options.AddRange(folderAttribute.Options);
            var previousIndex = options.IndexOf(property.stringValue);
            var selectedIndex = Mathf.Max(0, previousIndex);

            selectedIndex = EditorGUI.Popup(position, selectedIndex, options.ToArray());
            if (selectedIndex > 0)
            {
                property.stringValue = options[selectedIndex];
            }
            else if (previousIndex >= 0)
            {
                property.stringValue = "";
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use InventoryIdAttribute with string.");
        }
    }
}
