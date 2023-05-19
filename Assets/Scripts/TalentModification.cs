using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Talent Modification")]
public class TalentModification : ScriptableObject {
    public enum ModificationType {
        INCREASE_DAMAGE,
        DECREASE_MANA_COST,
        ADD_EFFECT
    }

    public enum ModifierType { 
        MultiplicativePercentage, 
        Additive, 
    };
    
    public ModificationType modificationType;

    [ShowIf("@this.modificationType != ModificationType.ADD_EFFECT")]
    public ModifierType modifierType;
    [ShowIf("@this.modificationType != ModificationType.ADD_EFFECT"), Range(0, 1000)]
    public int value;

    [ShowIf("@this.modificationType == ModificationType.ADD_EFFECT")]
    public SkillEffect additionalEffect;

    public void ApplyToSkill(Skill skill) {
        switch (modificationType) {
            case TalentModification.ModificationType.INCREASE_DAMAGE:
                skill.SetDamage(GetFinalDamage(skill.damage));
                break;
            case TalentModification.ModificationType.DECREASE_MANA_COST:
                skill.SetManaCost(GetFinalManaCost(skill.manaCost));
                break;
            case TalentModification.ModificationType.ADD_EFFECT:
                skill.effects.Add(additionalEffect);
                break;
        }
    }

    int GetFinalDamage(int baseValue) {
        return baseValue + (modifierType == TalentModification.ModifierType.Additive ? value : Mathf.CeilToInt(baseValue * value * .01f));
    }

    int GetFinalManaCost(int baseValue) {
        return baseValue - (modifierType == TalentModification.ModifierType.Additive ? value : Mathf.FloorToInt(baseValue * value * .01f));
    }

    public string GetDescription() {
        string str = "";
        switch (modificationType){
            case ModificationType.INCREASE_DAMAGE:
                str += "Increase damage by " + value + (modifierType == ModifierType.MultiplicativePercentage ? "%" : "");
                break;
            case ModificationType.DECREASE_MANA_COST:
                str += "Decrease mana cost by " + value + (modifierType == ModifierType.MultiplicativePercentage ? "%" : "");
                break;
            case ModificationType.ADD_EFFECT:
                str += "Add " + additionalEffect.type + " for " + additionalEffect.duration + " second(s)";
                break;
        }

        return str;
    }
}