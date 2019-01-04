using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using GlobalEnums;

public enum Cards
{
    HealthBoost,
    StaminaBoost,
    Count
}

[System.Serializable]
public class SavedData
{
    public static SavedData _instance;

    public static SavedData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SavedData();
                _instance.SetUpSavedData();
            }

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    public enum SavedObject
    {
        Health,
        EquppedCards,
        Count
    }

    Dictionary<SavedObject, object> savedObjects = new Dictionary<SavedObject, object>();

    public static bool GetBool(SavedObject savedObject)
    {
        return (bool)Instance.savedObjects[savedObject];
    }

    public static int[] GetIntegers(SavedObject savedObject)
    {
        return (int[])Instance.savedObjects[savedObject];
    }

    public static BitArray GetBitArray(SavedObject savedObject)
    {
        return (BitArray)Instance.savedObjects[savedObject];
    }

    public static T GetValue<T>(string valueName)
    {
        FieldInfo field = Instance.GetType().GetField(valueName);

        return (T)field.GetValue(Instance);
    }

    public void SetUpSavedData()
    {
        health = new LimitedCharacterStat(100);

        spirit = new LimitedCharacterStat(0, 60, 20);

        stamina = new LimitedCharacterStat(-200, 50, 50);

        staminaRegen = new CharacterStat(15);

        equippedSpirits = new bool[3];
        availableSpirits = new bool[3];

        availableWeapons = new bool[(int)WeaponID.Count];
        currentWeapon = WeaponID.Sword;
        secondWeapon = WeaponID.None;

        //loadedPrefabs = WorldManager.Instance.loadedPrefabs;
        essence = 1000;


        currentScene = "MainScene";
    }


    public LimitedCharacterStat health;

    public LimitedCharacterStat spirit;

    public LimitedCharacterStat stamina;

    public CharacterStat staminaRegen;

    public CharacterStat healthWhileAttacking;

    public bool[] equippedSpirits;
    public bool[] availableSpirits;

    public Transform[] loadedPrefabs;

    public Vector3[] characterPositions;
    public bool[] characterLives;
    public string[] permanentDeaths;

    public string currentScene = "";

    public bool[] availableWeapons;
    public WeaponID currentWeapon;
    public WeaponID secondWeapon;

    public CheckpointData lastCheckpoint;

    public int essence;
}


