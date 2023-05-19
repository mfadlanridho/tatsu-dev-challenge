using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class GameManager : MonoBehaviour {
    [Header("Skills")]
    [SerializeField] GameObject skillButtonPrefab;
    [SerializeField] Transform skillButtonHolder;

    [Header("Talents")]
    [SerializeField] GameObject talentGraphPrefab;
    [SerializeField] GameObject talentDepthPrefab;
    [SerializeField] GameObject talentSelectionPrefab;
    [SerializeField] GameObject talentButtonPrefab;
    [SerializeField] Transform talentGraphHolder;

    Dictionary<Talent, Button> talentButtonDictionary = new Dictionary<Talent, Button>();
    HashSet<TalentNode> purchasedTalentNodes = new HashSet<TalentNode>();

    void Start() {
        var player = FindObjectOfType<Player>();
        StartCoroutine(LoadSkillsAndTalents(player.character, player.talentGraph));
    }

    IEnumerator LoadSkillsAndTalents(CharacterData character, TalentGraph characterTalentGraph) {
        // set skill buttons
        foreach (var skill in character.skills) {
            // instantiate button
            var skillButton = Instantiate(skillButtonPrefab, skillButtonHolder);

            // set button image
            foreach (Transform child in skillButton.transform) {
                if (child.name == "Skill Image") {
                    child.GetComponent<Image>().sprite = skill.icon;
                } else if (child.name == "Damage") {
                } else if (child.name == "Mana") {
                    child.GetComponent<TextMeshProUGUI>().text = skill.manaCost.ToString();
                }
            }
        }

        // create talent graph
        var firstNodes = characterTalentGraph.GetFirstNodes();
        Dictionary<TalentNode, RectTransform> dict = new Dictionary<TalentNode, RectTransform>();
        var talentsUI = talentGraphHolder.parent;

        foreach (var node in firstNodes) {
            var talentGraph = Instantiate(talentGraphPrefab, talentGraphHolder);

            var queue = new Queue<TalentNode>();
            queue.Enqueue(node);

            var talentDepth = Instantiate(talentDepthPrefab, talentGraph.transform);
            var newQueue = new Queue<TalentNode>();

            bool firstNode = true;

            while (queue.Count > 0) {
                var cur = queue.Dequeue();

                if (dict.ContainsKey(cur)) {
                    Destroy(dict[cur].gameObject);
                    var newTalentSelection = Instantiate(talentSelectionPrefab, talentDepth.transform);
                    dict[cur] = newTalentSelection.transform as RectTransform;
                    foreach (var talent in cur.talents) {
                        Button button = Instantiate(talentButtonPrefab, newTalentSelection.transform).GetComponent<Button>();
                        button.GetComponentInChildren<TextMeshProUGUI>().text = talent.talentDescription;
                        talentButtonDictionary[talent] = button;
                        button.onClick.AddListener(delegate{OnTalentButtonPressed(cur, talent);});
                    }

                    talentDepth = Instantiate(talentDepthPrefab, talentGraph.transform);
                    continue;
                }
                
                // enqueue next connected nodes
                foreach (var connectedNodePort in cur.GetPort("unlocks").GetConnections()) {
                    if (newQueue.Contains(connectedNodePort.node as TalentNode)) {
                        continue;
                    }
                    
                    newQueue.Enqueue(connectedNodePort.node as TalentNode);
                }

                var talentSelection = Instantiate(talentSelectionPrefab, talentDepth.transform);
                dict.Add(cur, talentSelection.transform as RectTransform);

                foreach (var talent in cur.talents) {
                    var button = Instantiate(talentButtonPrefab, talentSelection.transform).GetComponent<Button>();
                    button.GetComponentInChildren<TextMeshProUGUI>().text = talent.talentDescription;
                    talentButtonDictionary.Add(talent, button);
                    button.onClick.AddListener(delegate{OnTalentButtonPressed(cur, talent);});

                    if (firstNode) {
                        button.interactable = true;
                        Debug.Log("Setting interactable");
                    }
                }
                firstNode = false;

                if (queue.Count == 0) {
                    talentDepth = Instantiate(talentDepthPrefab, talentGraph.transform);
                    queue = newQueue;
                    newQueue = new Queue<TalentNode>();
                }
            }
        }

        yield return null;

        // make lines
        foreach (var item in dict) {   
            var prerequisites = item.Key.GetPort("prerequisite").GetConnections();
            if (prerequisites.Count == 0)
                continue;
            
            foreach (var prevConnection in prerequisites) {
                UILineRenderer lineRenderer = new GameObject().AddComponent<UILineRenderer>();
                lineRenderer.transform.SetParent(talentsUI);
                lineRenderer.Points = new Vector2[2];
                lineRenderer.Points[0] = item.Value.position;
                lineRenderer.Points[1] = dict[prevConnection.node as TalentNode].position;
            }
        }
    }

    void OnTalentButtonPressed(TalentNode node, Talent talent) {
        if (purchasedTalentNodes.Contains(node)) {
            Debug.Log("Already purchased talent");
            return;
        }

        purchasedTalentNodes.Add(node);
        talentButtonDictionary[talent].targetGraphic.color = Color.green;
        
        Skill skill = System.Array.Find(FindObjectOfType<Player>().character.skills, skill => skill.skillName.ToString() == talent.targetSkill.skillName.ToString());
        talent.ApplyModifications(skill);

        foreach (var connection in node.GetPort("unlocks").GetConnections()) {
            var talentNode = connection.node as TalentNode;

            bool unlockedAllPrerequisites = true;
            foreach (var prerequisiteConnection in talentNode.GetPort("prerequisite").GetConnections()) {
                if (!purchasedTalentNodes.Contains((TalentNode) prerequisiteConnection.node)) {
                    unlockedAllPrerequisites = false; 
                    break;
                }
            }

            if (unlockedAllPrerequisites) {
                foreach (var newTalent in talentNode.talents) {
                    talentButtonDictionary[newTalent].interactable = true;
                }
            }
        }
    }
}