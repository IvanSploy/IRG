using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

namespace IRG.Editor
{
    [CustomPropertyDrawer(typeof(AbstractHashSet<>))]
    public class AbstractHashSetDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, ReorderableList> _lists = new();
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Generic)
            {
                EditorGUI.LabelField(position, label.text, "Make sure AbstractHashSet<> is used.");
                return;
            }

            var fieldType = fieldInfo.FieldType;
            if (!fieldType.IsGenericType || fieldType.GetGenericTypeDefinition() != typeof(AbstractList<>))
            {
                EditorGUI.LabelField(position, label.text, "Make sure AbstractHashSet<> is used.");
                return;
            }
            
            Type itemType = fieldType.GetGenericArguments()[0];
            var listProperty = property.FindPropertyRelative("List");

            if (listProperty == null)
            {
                EditorGUI.LabelField(position, label.text, $"Make sure {itemType.FullName} is Serializable.");
                return;
            }
            
            var propertyId = listProperty.propertyPath;
            if (!_lists.TryGetValue(propertyId, out var reorderableList))
            {
                reorderableList = new ReorderableList(listProperty.serializedObject, listProperty, true,
                    true, true, true);

                reorderableList.drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, $"{label.text.WithSpaces()}");
                };

                reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    if (element.managedReferenceValue == null) return;
                    EditorGUI.indentLevel++;
                    EditorGUI.PropertyField(rect, element,
                        new GUIContent($"{element.managedReferenceValue.GetType().Name.WithSpaces()}"),
                        true);
                    EditorGUI.indentLevel--;
                };

                reorderableList.elementHeightCallback = index =>
                    EditorGUI.GetPropertyHeight(reorderableList.serializedProperty
                        .GetArrayElementAtIndex(index), true);

                reorderableList.onAddDropdownCallback = (buttonRect, list) =>
                {
                    //TODO: modify to show only not repeated elements.
                    var mousePosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                    SearchWindowContext context = new SearchWindowContext(mousePosition);
                    var searchWindow = ScriptableObject.CreateInstance<TypeSearchWindow>();
                    searchWindow.Init(itemType, (type, ctx) =>
                    {
                        if (type.GetConstructor(Type.EmptyTypes) == null)
                        {
                            Debug.LogError($"[AbstractHashSet] Make sure the class {type.Name} has an empty constructor.");
                            return;
                        }

                        var instance = Activator.CreateInstance(type);
                        listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
                        listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1).managedReferenceValue = instance;
                        listProperty.serializedObject.ApplyModifiedProperties();
                    });
                    SearchWindow.Open(context, searchWindow);
                };
                _lists[propertyId] = reorderableList;
            }

            reorderableList.DoList(position);
            listProperty.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight * 4;

            if (property.propertyType != SerializedPropertyType.Generic) return height;

            var fieldType = fieldInfo.FieldType;
            if (!fieldType.IsGenericType || fieldType.GetGenericTypeDefinition() != typeof(AbstractList<>))
                return height;

            var listProperty = property.FindPropertyRelative("List");
            if (listProperty == null) return height;

            if (!_lists.TryGetValue(listProperty.propertyPath, out var list)) return height;
            return list.GetHeight();
        }
    }
}