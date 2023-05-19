using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

public class CharacterDataEditor : OdinMenuEditorWindow {
    [MenuItem("Tools/Character Data")]
    static void OpenWindow() {
        GetWindow<CharacterDataEditor>().Show();
    }
    
    CreateNewCharacterData createNewCharacterData;

    protected override void OnDestroy() {
        base.OnDestroy();

        if (createNewCharacterData != null)
            DestroyImmediate(createNewCharacterData.data);
    }

    protected override void OnBeginDrawEditors() {
        base.OnBeginDrawEditors();

        if (this.MenuTree == null)
            return;

        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if (SirenixEditorGUI.ToolbarButton("Delete Current")) {
                var path = AssetDatabase.GetAssetPath(selected.SelectedValue as CharacterData);

                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    protected override OdinMenuTree BuildMenuTree() {
        var tree = new OdinMenuTree();

        createNewCharacterData = new CreateNewCharacterData();
        tree.Add("Create New", createNewCharacterData);
        tree.AddAllAssetsAtPath("Character Data", "Assets/Characters", typeof(CharacterData));
        return tree;
    }

    public class CreateNewCharacterData {
        public CreateNewCharacterData() {
            data = ScriptableObject.CreateInstance<CharacterData>();
            // data.characterName = "New character data";
        }

        [InlineEditor(objectFieldMode: InlineEditorObjectFieldModes.Hidden)]
        public CharacterData data;

        [Button("Add New Enemy SO")]
        void CreateNewData() {
            AssetDatabase.CreateAsset(data, "Assets/Characters/" + data.characterName + ".asset");
            AssetDatabase.SaveAssets();

            // reset / create new instance of SO
            data = ScriptableObject.CreateInstance<CharacterData>();
            // data.characterName = "New enemy data";
        }
    }
}