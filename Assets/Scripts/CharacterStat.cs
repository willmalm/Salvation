using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStat
{
    protected float baseValue;

    [SerializeField]
    protected List<StatModifier> statModifiers;

    protected bool recalculateValue;

    protected float _value;

    public float Value
    {
        get
        {
            if (recalculateValue)
                _value = CalculateFinalValue();

            return _value;
        }
        set
        {
            baseValue = value;
        }
    }

    public CharacterStat() { }

    public CharacterStat(float value)
    {
        baseValue = value;
        recalculateValue = true;
        statModifiers = new List<StatModifier>();
    }

    public void AddModifier(StatModifier statModifier)
    {
        if (statModifier == null)
            return;

        recalculateValue = true;

        statModifiers.Add(statModifier);
    }

    public void RemoveModifier(StatModifier statModifier)
    {
        if (statModifier == null)
            return;

        recalculateValue = true;

        statModifiers.Remove(statModifier);
    }

    public void RemoveModifiers(string sourceName)
    {
        recalculateValue = true;

        List<StatModifier> flaggedModifiers = new List<StatModifier>();

        foreach (StatModifier statModifier in statModifiers)
        {
            if (statModifier.sourceName.Equals(sourceName))
                flaggedModifiers.Add(statModifier);
        }

        foreach (StatModifier statModifier in flaggedModifiers)
        {
            statModifiers.Remove(statModifier);
        }
    }

    protected float CalculateFinalValue()
    {
        float finalValue = baseValue;

        float percentage = 0;

        foreach (StatModifier statModifier in statModifiers)
        {
            if (statModifier.type == ModifierType.Standard)
                finalValue += statModifier.value;

            else if (statModifier.type == ModifierType.Percentage)
                percentage += statModifier.value;

            else if (statModifier.type == ModifierType.Set)
                return statModifier.value;
        }

        finalValue *= 1 + percentage;

        return finalValue;
    }
}

[System.Serializable]
public class LimitedCharacterStat : CharacterStat
{
    private float _max;

    public new float Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (value > Max)
                _value = Max;
            else if (value < Min)
                _value = Min;
            else
                _value = value;
        }
    }

    public float Min
    {
        get; set;
    }

    public float Max
    {
        get
        {
            if (recalculateValue)
                _max = CalculateFinalValue();

            return _max;
        }
        set
        {
            if (value < _value)
                _value = value;

            baseValue = value;
        }
    }

    public float Percentage
    {
        get
        {
            return Value / Max;
        }
    }

    public LimitedCharacterStat() { }

    public LimitedCharacterStat(float max)
    {
        Min = 0;
        baseValue = max;
        _value = max;
        recalculateValue = true;
        statModifiers = new List<StatModifier>();
    }

    public LimitedCharacterStat(float min, float max, float value)
    {
        Min = min;
        baseValue = max;
        _value = value;
        recalculateValue = true;
        statModifiers = new List<StatModifier>();
    }
}
