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
    public enum Modules
    {
        Repellent,
        Chemotaxis/*,
        Phototaxis,
        ExternalMemorySlime,
        Sporolation,
        Splitting*/
    }

    public enum Repellant
    {
        variable1,
        variable2,
        variable3
    }
    public enum Chemotaxis
    {
        variable1,
        variable2,
        variable3
    }
    public enum DrawMode
    {
        Deactivated,
        Wall,
        Slime,
        Food

    }
    public enum Colors
    {
        Wall,
        Slime,
        Food,
        Repellent
    }
    public enum Layers
    {
        Object,
        Chemical,
        Visual

    }
    struct Parameters
    {

    }

}
