using UnityEngine;

public class ShowIfAttribute : PropertyAttribute
{
    public string BooleanFieldName;

    public ShowIfAttribute(string booleanFieldName)
    {
        BooleanFieldName = booleanFieldName;
    }
}
