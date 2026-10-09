using UnityEditor;
using UnityEngine;

namespace IRG.Editor
{
    [CustomPropertyDrawer(typeof(AssetInline))]
    public class AssetInlineAttributeDrawer : PropertyDrawer
    {
        private bool _expanded;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(fieldRect, property, label);

            // Expandir sus datos
            if (property.objectReferenceValue)
            {
                Rect foldoutRect = new Rect(position.x, position.y, 15, EditorGUIUtility.singleLineHeight);
                _expanded = EditorGUI.Foldout(foldoutRect, _expanded, GUIContent.none, true);

                if (_expanded)
                {
                    SerializedObject serializedTarget = new SerializedObject(property.objectReferenceValue);
                    serializedTarget.Update();

                    SerializedProperty prop = serializedTarget.GetIterator();
                    float currentY = position.y + EditorGUIUtility.singleLineHeight + 2;

                    prop.NextVisible(true); 

                    EditorGUI.indentLevel++;
                    while (prop.NextVisible(false))
                    {
                        Rect propRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);
                        EditorGUI.PropertyField(propRect, prop, true);
                        currentY += EditorGUI.GetPropertyHeight(prop, true) + 2;
                    }
                    EditorGUI.indentLevel--;

                    serializedTarget.ApplyModifiedProperties();
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;

            if (_expanded && property.objectReferenceValue != null)
            {
                SerializedObject serializedTarget = new SerializedObject(property.objectReferenceValue);
                SerializedProperty prop = serializedTarget.GetIterator();
                prop.NextVisible(true);

                while (prop.NextVisible(false))
                {
                    totalHeight += EditorGUI.GetPropertyHeight(prop, true) + 2;
                }
            }

            return totalHeight;
        }
    }
}