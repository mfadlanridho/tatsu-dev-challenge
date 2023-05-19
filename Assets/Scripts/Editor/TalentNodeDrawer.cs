using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(TalentNode))]
public class TalentNodeDrawer : NodeEditor {
    bool showEntryNode, showTalent;

    public override void OnCreate() {
        base.OnCreate();

        (target as TalentNode).TriggerOnValidate();
    }
}