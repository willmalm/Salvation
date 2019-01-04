using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData
{
    string ownerName;

    Dictionary<string, object> values;

    public void AddValue(string valueName, object value)
    {
        values.Add(valueName, value);
    }

    public void SetValue(string valueName, object value)
    {
        if (!values.ContainsKey(valueName))
            AddValue(valueName, value);
        else
            values[valueName] = value;
    }

    public bool GetBool(string valueName)
    {
        if (!ValueExists(valueName))
            return false;

        return (bool)values[valueName];
    }

    public CharacterStat GetStat(string valueName)
    {
        if (!ValueExists(valueName))
            return null;

        return (CharacterStat)values[valueName];
    }

    public int GetInt(string valueName)
    {
        if (!ValueExists(valueName))
            return 0;

        return (int)values[valueName];
    }

    public float GetFloat(string valueName)
    {
        if (!ValueExists(valueName))
            return 0f;

        return (float)values[valueName];
    }

    public bool ValueExists(string valueName)
    {
        if (!values.ContainsKey(valueName))
        {
            Debug.Log("ObjectData (" + ownerName + "): does not contain value called " + valueName);
            return false;
        }
        else
            return true;
    }
}
