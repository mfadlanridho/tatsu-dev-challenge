using UnityEngine;

[CreateAssetMenu(fileName = "SkillVFX")]
public class SkillVFX : ScriptableObject {
    [field: SerializeField] public ParticleSystem particleSystem {get; private set;}
}