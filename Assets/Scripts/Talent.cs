using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class Talent {
    public TalentModification modification;

    #if UNITY_EDITOR
    [ListToPopup(typeof(TalentGraph), "TMPList", displayName: "Target Skill")]
    [LabelWidth(100)]
    [OnValueChanged("SkillPopupChangeCallback")]
    public string targetSkillPopup;

    [ShowInInspector, HideLabel, ReadOnly, PreviewField(100)] 
    Sprite skillIcon;
    #endif

    [HideInInspector]
    public Skill targetSkill;

    public string talentDescription {
        get {
            return modification.GetDescription() + " to " + targetSkill.skillName;
        }
    }

    public void SkillPopupChangeCallback() {
        try {
            targetSkill = TalentGraph.SkillList.Find(x => x.skillName == targetSkillPopup);
            if (targetSkill == null) 
                return;

            skillIcon = targetSkill.icon;
        } catch (System.Exception e) { }
    }

    public void ApplyModifications(Skill targetSkill) {
        targetSkill.AddTalent(this);
    }
}