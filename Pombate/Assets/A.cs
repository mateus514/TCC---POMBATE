using System;
using UnityEngine;

[CreateAssetMenu(fileName = "String Eevent Channel", menuName = "Scriptable Objects/StringEeventChannel")]

public class StringEeventChannel : ScriptableObject
{
    public event Action<string> OnEventRaised;

    public void RaiseEvent(string value)
    {
        OnEventRaised?.Invoke(value);
    }
}