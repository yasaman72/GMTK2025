using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ShowIfAttribute : PropertyAttribute
{
    public readonly string ConditionField;
    public readonly bool Invert;

    public ShowIfAttribute(string conditionField, bool invert = false)
    {
        ConditionField = conditionField;
        Invert = invert;
    }
}
