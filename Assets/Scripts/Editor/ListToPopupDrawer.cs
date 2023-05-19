using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ListToPopupAttribute))]
public class ListToPopupDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        ListToPopupAttribute atb = attribute as ListToPopupAttribute;

        List<string> stringList = null;

        if (atb.type.GetField(atb.propertyName) != null) {
            stringList = atb.type.GetField(atb.propertyName).GetValue(atb.type) as List<string>;
            // Debug.Log(stringList);
        } else {
            Debug.LogWarning("atb type is null");
        }

        if (stringList != null && stringList.Count != 0) {
            int selectedIndex = Mathf.Max(0, stringList.IndexOf(property.stringValue));
            selectedIndex = EditorGUI.Popup(position, atb.diplayName ?? property.displayName, selectedIndex, stringList.ToArray());
            property.stringValue = stringList[selectedIndex];
        } else {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}