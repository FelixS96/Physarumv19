using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unused : MonoBehaviour
{



    //public enum Colors
    //{
    //    Wall = 1,
    //    SlimeMold,
    //    Food,
    //    Repellent,
    //    Slime
    //}

    private void AddVariables(string name)
    {
        //foreach (Enums.name module in (Enums.Modules[])Enum.GetValues(typeof(Enums.name)))
        //{
        //    PlayerPrefs.SetInt(module.ToString(), 1);
        //    uiController.AddModuleToUI(module.ToString());
        //    activeModules.Add(module);
        //}
        //throw new NotImplementedException();
    }

    //#if UNITY_EDITOR
    //        Debug.unityLogger.logEnabled = true;
    //#else
    //  Debug.unityLogger.logEnabled = false;
    //#endif

    //Color[] GenerateChemicalMap()
    //{
    //    Color[] color = uiController.globalBlackColorAll; //todo: add instead of new generation (walls can block chemicals, slime leaves repellent

    //    for (int y = 0; y < preDefHeight; y++)
    //    {
    //        for (int x = 0; x < preDefWidth; x++)
    //        {
    //            UnityEngine.Debug.Log(x+" "+y+" chemicalmapposition/number "+ConvertPositionTo1D(new Vector2(x, y)));
    //            if(pixelDataObj[ConvertPositionTo1D(new Vector2(x, y))] == uiController.drawColorFood)
    //            {
    //                for (int yy = 0; yy < attractantEffectRange; yy++)
    //                {
    //                    for (int xx = 0; xx < attractantEffectRange; xx++)
    //                    {
    //                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(xx, yy));
    //                        Color currentColor = pixelDataChem[ConvertPositionTo1D(new Vector2(xx, yy))];
    //                        pixelDataChem[ConvertPositionTo1D(new Vector2(xx, yy))] = ReturnColorBasedOnDistance(Color.blue, currentColor, attractantEffectRange, distance);
    //                    }
    //                }
    //            }
    //            else if(pixelDataObj[ConvertPositionTo1D(new Vector2(x, y))] == uiController.drawColorRepellent)
    //            {

    //            }

    //        }
    //    }
    //    return color;
    //}

    //float ConvertColorIntoEffect(int layer, Color color)
    //{
    //    float result = 0;
    //    switch (layer)
    //    {
    //        case (int)Enums.Layers.Object:

    //            break;
    //        case (int)Enums.Layers.Chemical:

    //            break;
    //        default:
    //            break;
    //    }

    //    return result;
    //}

    //if ((Time.realtimeSinceStartup - lastTime) > 0.3)
    //{
    //    //UnityEngine.Debug.Log(Time.realtimeSinceStartup - lastTime);
    //    lastTime = Time.realtimeSinceStartup;
    //    yield return null;
    //}
    //gridData.chemicalTexture.SetPixels(pixelDataChem);
    //gridData.chemicalTexture.Apply();

    //for (int i = 0; i < uiController.newPixels.Count; i++)
    //{
    //    UnityEngine.Debug.Log("New Color for Pixel: " + uiController.newPixels[i].x + " , " + uiController.newPixels[i].y + " is " + pixelDataChem[ConvertPositionTo1D(uiController.newPixels[i])]);
    //}

    //for (int y = 0; y < preDefHeight; y++)
    //{
    //    if (lastTime==0)
    //    {
    //        lastTime = Time.realtimeSinceStartup;
    //    }
    //    if((Time.realtimeSinceStartup - lastTime) > 0.3)
    //    {
    //        UnityEngine.Debug.Log(Time.realtimeSinceStartup - lastTime);
    //        lastTime = Time.realtimeSinceStartup;
    //        yield return null;
    //    }
    //    for (int x = 0; x < preDefWidth; x++)
    //    {
    //        UnityEngine.Debug.Log(x + " " + y + " chemicalmapposition/number " + ConvertPositionTo1D(new Vector2(x, y)));
    //        //if (pixelDataObj[ConvertPositionTo1D(new Vector2(x, y))] == uiController.drawColorFood)
    //        //{
    //        //    //for (int yy = 0; yy < attractantEffectRange; yy++)
    //        //    //{
    //        //    //    for (int xx = 0; xx < attractantEffectRange; xx++)
    //        //    //    {
    //        //    //        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(xx, yy));
    //        //    //        Color currentColor = pixelDataChem[ConvertPositionTo1D(new Vector2(xx, yy))];
    //        //    //        pixelDataChem[ConvertPositionTo1D(new Vector2(xx, yy))] = ReturnColorBasedOnDistance(Color.blue, currentColor, attractantEffectRange, distance);
    //        //    //    }
    //        //    //}
    //        //}
    //        //else if (pixelDataObj[ConvertPositionTo1D(new Vector2(x, y))] == uiController.drawColorRepellent)
    //        //{

    //        //}
    //    }
    //}

    //bool OutOfBounds(Vector2 searchPos, Vector2 startPos)
    //{
    //    bool returnVal = false;


    //    return returnVal;
    //}

}
