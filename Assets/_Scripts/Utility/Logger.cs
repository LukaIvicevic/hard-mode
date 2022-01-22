using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : Singleton<Logger>
{
    [SerializeField]
    private bool debug = true;

    [SerializeField]
    private bool rayDebug = true;

    public void Log(string value)
    {
        if (debug)
        {
            Debug.Log(value);
        }
    }

    public void LogWarning(string value)
    {
        if (debug)
        {
            Debug.LogWarning(value);
        }
    }

    public void LogError(string value)
    {
        if (debug)
        {
            Debug.LogError(value);
        }
    }

    public void DrawRay(Vector3 start, Vector3 direction, Color color, float duration)
    {
        if (rayDebug)
        {
            Debug.DrawRay(start, direction, color, duration);
        }
    }
}
