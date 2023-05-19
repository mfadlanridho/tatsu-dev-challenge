using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(TalentGraph))]
public class TalentGraphDrawer : NodeGraphEditor {
    public override void OnOpen() {
        base.OnOpen();

        var talentGraph = target as TalentGraph;

        TalentGraph.TMPList = new System.Collections.Generic.List<string>();
        TalentGraph.SkillList = new System.Collections.Generic.List<Skill>();

        foreach (var item in talentGraph.character.skills) {
            TalentGraph.TMPList.Add(item.skillName);
            TalentGraph.SkillList.Add(item);
        }
    }
}