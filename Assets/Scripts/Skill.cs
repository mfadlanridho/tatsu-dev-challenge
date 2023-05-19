using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[System.Serializable]
public class Skill {    
    [field: SerializeField] public string skillName {get; private set;}
    
    [field: PreviewField(100, ObjectFieldAlignment.Left)]
    [field: SerializeField] public Sprite icon {get; private set;}

    [field: TextArea(3, 10), FoldoutGroup("More")]
    [field: SerializeField] public string skillDescription {get; private set;}

    [field: FoldoutGroup("More")]
    [field: SerializeField] public int damage {get; private set;}

    [field: FoldoutGroup("More")]
    [field: SerializeField] public int manaCost {get; private set;}

    [field: FoldoutGroup("More")]
    [field: SerializeField] public int cooldown {get; private set;}
    
    [field: FoldoutGroup("More")]
    [field: SerializeField] public SkillVFX skillVFX {get; private set;}

    [field: FoldoutGroup("More")]
    [field: SerializeField] public List<SkillEffect> effects {get; private set;}

    [Header("Debug purpose"), ShowInInspector, ReadOnly] 
    List<Talent> appliedTalents;

    public void SetDamage(int value) {
        damage = Mathf.Max(0, value);
    }

    public void SetManaCost(int value) {
        manaCost = Mathf.Max(0, value);
    }

    public void AddTalent(Talent talent) {
        if (appliedTalents == null)
            appliedTalents = new List<Talent>();

        if (appliedTalents.Contains(talent))
            return;

        appliedTalents.Add(talent);

        talent.modification.ApplyToSkill(this);
    }
}