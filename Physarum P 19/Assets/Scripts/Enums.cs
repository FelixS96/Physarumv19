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
    //InfluenceType
    public enum InfluenceNames
    {
        LowFood,
        IntenseLight,
        FoodSource,
        Repellent,
        Attractant
    } 
    //showable layers
    public enum Layers
    {
        Object,
        Chemical

    }
    //Position and Color
    public struct PixelData
    {
        public PixelData(DrawMode type, Vector2 pos)
        {
            pixelType = type;
            position = pos;
        }

        public DrawMode pixelType { get; }
        public Vector2 position { get; }
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
    //Influence type with a strenght with a Vector to
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
    //SlimeMold
    public struct SlimeMold
    {
        public SlimeMold(Vector2 pos/*, List<Influence> infs*/)
        {
            position = pos;
            //influences = infs;
        }
        public Vector2 position;
        //public float food;
        //public List<Influence> influences;
    }
}
