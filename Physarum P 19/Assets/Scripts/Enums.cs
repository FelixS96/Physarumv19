using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum VariableType
    {
        var1=1,
        var2,
        var3
    }
    public enum GameSpeed
    {
        Pause,
        Play,
        FastMode = 5
    }
}
