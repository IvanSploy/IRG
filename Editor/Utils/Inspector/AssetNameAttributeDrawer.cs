using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IRG.Editor
{
    [CustomPropertyDrawer(typeof(AssetNameAttribute), true)]
    public class AssetNameAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!typeof(Object).IsAssignableFrom(fieldInfo.FieldType))
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }
            
            EditorGUI.LabelField(position, label);

            // Indentation for foldout
            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            var hash = new HashSet<Object>();
            
            var characters = AssetsIO.LoadAll(fieldInfo.FieldType).ToList();
            hash.AddRange(characters);
            
            List<string> names = new List<string> {"None"};
            names.AddRange(characters.Select(c => c.name));

            var previousIndex = -1;
            var current = property.objectReferenceValue;
            if(current) previousIndex = names.IndexOf(current.name);
            var selectedIndex = Mathf.Max(0, previousIndex);

            selectedIndex = EditorGUI.Popup(position, selectedIndex, names.ToArray());
            if (selectedIndex > 0)
            {
                property.objectReferenceValue = characters[selectedIndex - 1];
            }
            else if (previousIndex >= 0)
            {
                property.objectReferenceValue = null;
            }
        }
    }
}
