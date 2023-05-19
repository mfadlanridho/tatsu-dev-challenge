using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] CharacterData characterSO;
    
    [field: SerializeField] public TalentGraph talentGraph {get; private set;}
    public CharacterData character {get; private set;}

    [ShowInInspector] 
    public Skill[] skills {
        get {
            if (!character)
                return new Skill[0];
            return character.skills;
        }
    }

    private void Start() {
        character = Instantiate(characterSO);
    }
}