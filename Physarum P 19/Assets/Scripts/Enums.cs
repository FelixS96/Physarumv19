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
    //Speed of the game
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
    //what can be drawn
    public enum DrawMode
    {
        Deactivated,
        Wall,
        Slime,
        Food,
        Repellent,
        Erase

    }
    public enum Colors
    {
        Wall = 1,
        Slime,
        Food,
        Repellent
    }
    //showable layers
    public enum Layers
    {
        Object,
        Chemical

    }
    //pixel made out of vector2 and color
    public struct Pixel
    {
        public Pixel(Vector2 vec, Color col)
        {
            position = vec;
            color = col;
        }
        public Vector2 position { get; }
        public Color color { get; }

    }
    

}
