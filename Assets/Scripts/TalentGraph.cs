using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class TalentGraph : NodeGraph {
    public static List<string> TMPList;
    public static List<Skill> SkillList;
    public CharacterData character;

    public List<TalentNode> GetFirstNodes() {
        var talentNodes = new List<TalentNode>();
        foreach (TalentNode node in nodes) {
            if (!node.GetPort("prerequisite").IsConnected) {
                talentNodes.Add(node);
            }
        }
        return talentNodes;
    }
}