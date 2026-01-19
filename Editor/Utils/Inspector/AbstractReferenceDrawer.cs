using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace IRG.Editor
{
    [CustomPropertyDrawer(typeof(AbstractReferenceAttribute), true)]
    public class AbstractReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                var fieldType = fieldInfo.FieldType;

                if (fieldType.IsAbstract)
                {
                    var selectedName = property.managedReferenceValue == null 
                        ? "None" 
                        : property.managedReferenceValue.GetType().Name.WithSpaces();
                    
                    var buttonPosition = position;
                    var labelWidth = EditorGUIUtility.labelWidth;
                    buttonPosition.height = EditorGUIUtility.singleLineHeight;
                    buttonPosition.x += labelWidth;
                    buttonPosition.width -= labelWidth;
                    
                    if (GUI.Button(buttonPosition, new GUIContent(selectedName)))
                    {
                        var mousePosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                        SearchWindowContext context = new SearchWindowContext(mousePosition);
                        var searchWindow = ScriptableObject.CreateInstance<TypeSearchWindow>();
                        searchWindow.Init(fieldType, type =>
                        {
                            if (type == null)
                            {
                                property.managedReferenceValue = null;
                                property.serializedObject.ApplyModifiedProperties();
                                return;
                            }
                            var instance = Activator.CreateInstance(type);
                            property.managedReferenceValue = instance;
                            property.serializedObject.ApplyModifiedProperties();
                        }, 
                            true);
                        SearchWindow.Open(context, searchWindow);
                    }
                    
                    EditorGUI.PropertyField(position, property, true);
                    
                    property.serializedObject.ApplyModifiedProperties();
                    return;
                }
            }
            
            EditorGUI.LabelField(position, label.text, "Use AbstractReferenceAttribute with SerializeReference.");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }
    }
}