using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum VariableType
    {
        var1 = 1,
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
        SlimeMold,
        Food,
        Repellent//,
        //Erase
        //todo: add erase

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
    public enum InfluenceNames
    {
        LowFood,
        IntenseLight,
        FoodSource,
        Repellent,
        Attractant
    } 

    public struct Influence
    {
        public Influence(InfluenceNames influenceName,int strenghtI,Vector2 targetPosI)
        {
            name = influenceName;
            strenght = strenghtI;
            targetPos = targetPosI;
        }
        public InfluenceNames name;
        public int strenght;
        public Vector2 targetPos;
    }
    public struct Slime
    {
        public Vector2 position;
        public float food;
        public List<Influence> influences;
    }

}
