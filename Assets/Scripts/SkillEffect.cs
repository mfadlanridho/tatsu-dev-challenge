using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class SkillEffect {
    public enum EffectType {
        STUN,
        SLOW,
    }

    public EffectType type;
    public int duration;
    
    public bool withVFX;
    [ShowIf("withVFX", true)]
    public SkillVFX vfx;
}