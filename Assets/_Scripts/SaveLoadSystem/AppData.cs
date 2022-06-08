using System;
using UnityEngine;

[Serializable]
public class AppData
{
    public bool switchMode;
    public bool inputMode;
    public bool detalization;
    public bool postProcessing;
    public bool hints;
    public int dimention;
    public Vector3 possition, endPossition;

    public AppData()
    {
        switchMode = false;
        inputMode = true;
        detalization = true;
        possition = new Vector3(0, 0.159f, 0);
        endPossition = possition;
    }
}