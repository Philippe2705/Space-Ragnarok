#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

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
}

#endif
