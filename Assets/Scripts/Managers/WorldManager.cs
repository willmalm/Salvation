using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

    public static WorldManager _instance;

    public static WorldManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<WorldManager>();

            if (_instance == null)
                Debug.Log("Couldn't find a World Manager in the scene.");

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    public GameObject[] worldPrefabs;
    public Transform[] loadedPrefabs;

    private void Start()
    {
        loadedPrefabs = new Transform[worldPrefabs.Length];
    }

    public static void LoadWorldPrefab(int index)
    {
        if (index >= Instance.worldPrefabs.Length)
            return;

        Instance.StartCoroutine("LoadPrefabAsync", index);
    }

    public static void UnloadWorldPrefab(int index)
    {
        if (index >= Instance.loadedPrefabs.Length || Instance.loadedPrefabs[index] == null)
            return;

        Instance.StartCoroutine("UnloadPrefabAsync", index);
    }

    IEnumerator LoadPrefabAsync(int index)
    {
        Transform prefab = Instance.worldPrefabs[index].transform;
        Transform parent = new GameObject("Level" + index).transform;

        for (int i = 0; i < prefab.childCount; i++)
        {
            Transform child = Instantiate(prefab.GetChild(i), prefab.GetChild(i).position, prefab.GetChild(i).rotation);
            child.SetParent(parent);
            yield return new WaitForEndOfFrame();
        }

        Instance.loadedPrefabs[index] = parent;
        yield return null;
    }

    IEnumerator UnloadPrefabAsync(int index)
    {
        Transform prefab = Instance.loadedPrefabs[index].transform;

        for (int i = 0; i < prefab.childCount; i++)
        {
            Destroy(prefab.GetChild(0).gameObject);
            yield return new WaitForEndOfFrame();
        }

        Destroy(Instance.loadedPrefabs[index].gameObject);
        Instance.loadedPrefabs[index] = null;
        yield break;
    }
}
