using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ModifierType
{
    Standard,
    Percentage,
    Set,
    Count
}

[System.Serializable]
public class StatModifier
{
    public ModifierType type;
    public float value;
    public string sourceName;
}
