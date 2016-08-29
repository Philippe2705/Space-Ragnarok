#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class EditorScript : MonoBehaviour
{
    [MenuItem("Ships/Create Ships For Editing")]
    static void GenerateShipsForEditing()
    {
        var shipsNotCreated = new List<string>();
        var sprites = new List<Sprite>(Resources.LoadAll<Sprite>("Images/"));
        var prefabs = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Ships/"));
        var prefabsNames = new List<string>();

        foreach (var o in prefabs)
        {
            prefabsNames.Add(o.name);
        }
        foreach (var o in sprites)
        {
            if (!prefabsNames.Contains(o.name + "Bot") || !prefabsNames.Contains(o.name + "Player"))
            {
                shipsNotCreated.Add(o.name);
            }
        }
        foreach (var name in shipsNotCreated)
        {
            Sprite sprite = Resources.Load<Sprite>("Images/" + name);
            var go = Instantiate(Resources.Load<GameObject>("DefaultShip")) as GameObject;
            go.GetComponent<SpriteRenderer>().sprite = sprite;
            if (Resources.Load("Prefabs/NotEdited/" + name) == null)
            {
                PrefabUtility.CreatePrefab("Assets/Resources/Prefabs/NotEdited/" + name + ".prefab", go);
            }
            DestroyImmediate(go);
        }
    }

    [MenuItem("Ships/Create Final Ships")]
    static void GenerateFinalShips()
    {
        var name = Selection.activeObject.name;
        GameObject go = Instantiate((GameObject)Selection.activeObject) as GameObject;
        go.AddComponent<PlayerShip>();
        PrefabUtility.CreatePrefab("Assets/Resources/Prefabs/Ships/" + name + " Player.prefab", go);
        DestroyImmediate(go);
        go = Instantiate((GameObject)Selection.activeObject) as GameObject;
        go.AddComponent<BotShip>();
        PrefabUtility.CreatePrefab("Assets/Resources/Prefabs/Ships/" + name + " Bot.prefab", go);
        DestroyImmediate(go);
    }

    [MenuItem("Ships/Add Ships and Bullets to Lobby")]
    static void AddSpawnableToLobby()
    {
        if (EditorSceneManager.GetActiveScene() != EditorSceneManager.GetSceneByName("Lobby"))
        {
            Debug.LogError("Lobby scene needs to be open");
        }
        else
        {
            var so = new SerializedObject(FindObjectOfType<Prototype.NetworkLobby.LobbyManager>());
            var p = so.FindProperty("m_SpawnPrefabs");
            while (p.arraySize != 0)
            {
                p.DeleteArrayElementAtIndex(0);
            }
            foreach (var go in Resources.LoadAll<GameObject>("Prefabs/Ships/"))
            {
                p.InsertArrayElementAtIndex(0);
                p.GetArrayElementAtIndex(0).objectReferenceValue = go;
            }
            foreach (var go in Resources.LoadAll<GameObject>("Prefabs/Bullets/"))
            {
                p.InsertArrayElementAtIndex(0);
                p.GetArrayElementAtIndex(0).objectReferenceValue = go;
            }
            so.ApplyModifiedProperties();
        }
    }

    [MenuItem("Tools/Replace Text by TextI18N")]
    static void ReplaceText()
    {
        foreach (var text in Resources.FindObjectsOfTypeAll<Text>())
        {
            if (text.gameObject.scene == EditorSceneManager.GetActiveScene())
            {
                var save = Instantiate(text);
                var go = text.gameObject;
                DestroyImmediate(text);
                var i18n = go.AddComponent<TextI18N>();
                i18n.text = save.text;
                i18n.font = save.font;
                i18n.fontSize = save.fontSize;
                i18n.lineSpacing = save.lineSpacing;
                i18n.supportRichText = save.supportRichText;
                i18n.alignment = save.alignment;
                i18n.alignByGeometry = save.alignByGeometry;
                i18n.horizontalOverflow = save.horizontalOverflow;
                i18n.verticalOverflow = save.verticalOverflow;
                i18n.resizeTextForBestFit = save.resizeTextForBestFit;
                i18n.color = save.color;
                i18n.material = save.material;
                i18n.raycastTarget = save.raycastTarget;
                DestroyImmediate(save.gameObject);
            }
        }
    }
}

#endif
