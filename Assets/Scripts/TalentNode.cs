using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;
using XNode;

[NodeWidth(SkillAndTalentSystemStatics.talentNodeWidth)] // NodeTint(SkillAndTalentSystemStatics.talentNodeColor)
public class TalentNode : Node {
    [Input(backingValue = ShowBackingValue.Never)] public int prerequisite;
    
    [ListDrawerSettings(HideAddButton = true, Expanded = true)]
    public List<Talent> talents;
    [Output(backingValue = ShowBackingValue.Never)] public int unlocks;

    [Button("Add Talent")]
    void AddTalent() {
        Debug.Log("Add talent");
        talents.Add(new Talent());
    }

    public override object GetValue(NodePort port) {
        return null;
    }

    #if UNITY_EDITOR
    void OnValidate() {
        foreach (var talent in talents) {
            if (System.String.IsNullOrEmpty(talent.targetSkillPopup)) {
                talent.targetSkillPopup = TalentGraph.SkillList[0].skillName.ToString();
            }
            talent.SkillPopupChangeCallback();            
        }
    }
    #endif
}