using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            if (_instance == null)
                Debug.Log("Couldn't find a Game Manager in the scene.");

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private Settings _settings;

    public static Settings Settings
    {
        get
        {
            if (Instance._settings == null)
                Instance.LoadSettings();

            return Instance._settings;
        }
        set
        {
            Instance._settings = value;
        }
    }

    private bool _paused;
    public static bool IsPaused
    {
        get
        {
            return Instance._paused;
        }
        set
        {
            if (value)               
                EventManager.TriggerEvent("PauseGame", null);
            else
                EventManager.TriggerEvent("ContinueGame", null);

            Instance._paused = value;
        }
    }

    private Animator fadePanel;
    private AsyncOperation loadingScene;

    private void Start()
    {
        //DontDestroyOnLoad(this);

        fadePanel = GameObject.Find("ScreenFade").GetComponent<Animator>();
    }

    //void OnLevelWasLoaded(int i)
    //{
    //    fadePanel = GameObject.Find("ScreenFade").GetComponent<Animator>();
    //    fadePanel.SetTrigger("Start");
    //}

    void Update()
    {
        if (loadingScene != null)
        {
            if (fadePanel.GetCurrentAnimatorStateInfo(0).IsName("End") && !fadePanel.IsInTransition(0) && fadePanel.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                loadingScene.allowSceneActivation = true;
            }
        }
    }

    //public static void PauseGame()
    //{
    //    Instance._paused = false;
    //}

    //public static void UnpauseGame()
    //{
    //    Instance._paused = true;
    //}

    public static void LoadGame(int saveSlot)
    {
        string filePath = "Assets/Resources/Save_" + saveSlot;

        if (!File.Exists(filePath))
        {
            SavedData.Instance = new SavedData();
            SavedData.Instance.SetUpSavedData();

            Instance.SaveFile(SavedData.Instance, filePath);
        }
        else
        {
            SavedData.Instance = Instance.LoadFile<SavedData>(filePath);       
        }

        Instance.LoadScene("MainScene");
        Settings.lastSave = saveSlot;
        SaveSettings();
    }

    private void LoadScene(string sceneName)
    {
        loadingScene = SceneManager.LoadSceneAsync(sceneName);
        loadingScene.allowSceneActivation = false;
        fadePanel.SetTrigger("End");
    }

    public static void SaveGame(int saveSlot)
    {
        string filePath = "Assets/Resources/Save_" + saveSlot;

        Instance.SaveFile(SavedData.Instance, filePath);
    }

    private void LoadSettings()
    {
        string filePath = "Assets/Resources/Settings";

        if (!File.Exists(filePath))
        {
            _settings = new Settings();

            SaveFile(_settings, filePath);
        }
        else
        {
            _settings = LoadFile<Settings>(filePath);
        }
    }

    public static void SaveSettings()
    {
        string filePath = "Assets/Resources/Settings";

        Instance.SaveFile(Instance._settings, filePath);
    }

    public void ApplySettings()
    {
        if (Settings.windowMode == WindowMode.Fullscreen)
            Screen.fullScreen = true;
        else
            Screen.fullScreen = false;
    }

    private void SaveFile<T>(T source, string filePath)
    {
        string json = JsonUtility.ToJson(source, true);

        //BinaryFormatter binaryFormatter = new BinaryFormatter();
        //FileStream fileStream = File.Create(filePath);

        Debug.Log(json);

        FileStream fileStream = File.Create(filePath);
        fileStream.Close();


        //binaryFormatter.Serialize(fileStream, json);

        File.WriteAllText(filePath, json);

        //fileStream.Close();
    }

    private T LoadFile<T>(string filePath)
    {
        string json;
        //BinaryFormatter binaryFormatter = new BinaryFormatter();
        //FileStream fileStream = File.Open(filePath, FileMode.Open);

        json = File.ReadAllText(filePath);
        //json = (string)binaryFormatter.Deserialize(fileStream);

        //fileStream.Close();

        T content = JsonUtility.FromJson<T>(json);

        if (content == null)
            Debug.Log("Could not load file from path '" + filePath + "'. File format might be invalid and in need of a reset.'");

        return content;
    }

    public static bool SaveExists(int saveSlot)
    {
        string filePath = "Assets/Resources/Save_" + saveSlot;

        return File.Exists(filePath);
    }
}
