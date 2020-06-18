using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private float baseValue = 1f;

    bool isDirty = true;
    float lastValue = -1f;

    public float value
    {
        get
        {
            if (isDirty)
            {
                CalculateValue();
            }

            return lastValue;
        }
    }

    private List<float> modifiers;

    public Stat(float initValue)
    {
        SetBaseValue(initValue);
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;

        isDirty = true;
    }

    public void AddModifier(float mult)
    {
        if (modifiers == null)
            modifiers = new List<float>();

        modifiers.Add(mult);

        isDirty = true;
    }

    public void RemoveModifier(float mult)
    {
        if (modifiers == null)
            modifiers = new List<float>();

        modifiers.Remove(mult);

        isDirty = true;
    }

    public void RemoveAllModifiers()
    {
        if(modifiers != null)
            modifiers.Clear();

        isDirty = true;
    }

    void CalculateValue()
    {
        if (modifiers == null)
            modifiers = new List<float>();

        lastValue = baseValue;

        foreach(float mult in modifiers)
        {
            lastValue *= mult;
        }

        isDirty = false;
    }
    
}
