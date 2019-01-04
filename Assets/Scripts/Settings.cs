using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindowMode
{
    Windowed,
    Fullscreen,
    Borderless,
    Count
}

public class Settings
{
    public int lastSave;
    public Resolution resolution;
    public float viewDistance;
    public WindowMode windowMode;

    public double mouseSensitivity;
    public double aimAssist;

    public Settings()
    {
        lastSave = -1;
        mouseSensitivity = 3.6;
        aimAssist = 0.3;
    }
}
