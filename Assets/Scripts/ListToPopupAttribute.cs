using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ListToPopupAttribute : PropertyAttribute {
    public System.Type type;
    public string propertyName;
    public string diplayName;

    public ListToPopupAttribute(System.Type type, string propertyName, string displayName = null) {
        this.type = type;
        this.propertyName = propertyName;
        this.diplayName = displayName;
    }
}