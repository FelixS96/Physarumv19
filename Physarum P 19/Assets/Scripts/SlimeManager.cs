using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SlimeManager : MonoBehaviour
{
    #region Variables
    public int gameSpeed;
    public float timePassedUI;

    int preDefWidth = 1600, preDefHeight = 810;
    public bool calculating;
    float deltaTime;
    float lastTime;
    float calculationTime;
    
    [SerializeField]
    List<Enums.Modules> activeModules;
    [SerializeField]
    List<Enums.Pixel> finishedPixels= new List<Enums.Pixel>();

    [SerializeField]
    Color[] pixelDataObj;
    [SerializeField]
    public Color[] pixelDataChem;
    
    [SerializeField]
    UIController uiController;
    [SerializeField]
    GridData gridData;

    public int attractantEffectRange;
    public int repellentEffectRange;

    #endregion
    void Awake()
    {
        //#if UNITY_EDITOR
        //        Debug.unityLogger.logEnabled = true;
        //#else
        //  Debug.unityLogger.logEnabled = false;
        //#endif
    }
    // Start is called before the first frame update
    void Start()
    {
        timePassedUI = 0;
        foreach (Enums.Modules module in (Enums.Modules[]) Enum.GetValues(typeof(Enums.Modules)))
        {
            PlayerPrefs.SetInt(module.ToString(), 1);
            uiController.AddModuleToUI(module);
            activeModules.Add(module);
        }
        gridData = FindObjectOfType<GridData>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameSpeed > 0)
        {
            simulateWithSpeed(gameSpeed);
        } 
    }
    void simulateWithSpeed(int speed)
    {
        if (uiController.drawChanges&&!calculating)
        {
            uiController.drawChanges = false;
            calculating = true;
            GetInformation();
            StartCoroutine(GenerateChemicalMap());
        }
        if (!calculating)
        {
            timePassedUI += speed;
            uiController.SetTimeUI(timePassedUI);
        }else
        {
            uiController.SetGameSpeed(0);
        }
        
        
        //Think();
        //Act();
    }

    private void GetInformation()
    {
        GetDataFromTexture(uiController.texture2DObject, ref pixelDataObj);
        SetDataInTexture(gridData.viewTexture, pixelDataObj);
    }
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

    Color ReturnColorBasedOnDistance(Color maxColor, Color currentColor, float maxDistance, float distance)
    {
        Color returnColor = currentColor;
        float multiplier = 1 - (distance / maxDistance);
        Color tempColor = maxColor * multiplier;
        if (multiplier >= 0)
        {
            if (maxColor.r == 1)
            {
                if (currentColor.r <= tempColor.r)
                {
                    returnColor.r = tempColor.r;
                }
            }
            else if(maxColor.g == 1)
            {
                if (currentColor.g <= tempColor.g)
                {
                    returnColor.g = tempColor.g;
                }
            }
            else if(maxColor.b == 1)
            {
                if (currentColor.b <= tempColor.b)
                {
                    returnColor.b = tempColor.b;
                }
            }
        }
        return returnColor;
    }

    int ConvertPositionTo1D(Vector2 position)
    {
        int converted;
        converted = (int)(position.y * preDefWidth + position.x);
        return converted;
    }
    float ConvertColorIntoEffect(int layer, Color color)
    {
        float result = 0;
        switch (layer)
        {
            case (int)Enums.Layers.Object:

                break;
            case (int)Enums.Layers.Chemical:

                break;
            default:
                break;
        }

        return result;
    }
    private void Act()
    {
        throw new NotImplementedException();
    }

    private void Think()
    {

        throw new NotImplementedException();
    }
    
    void SetVirtualPixel(Vector2 pos, Color color, ref Color[] target)
    {
        target[ConvertPositionTo1D(new Vector2(pos.x, pos.y))] = color;
    }
    void GetDataFromTexture(Texture2D texture, ref Color[] data)
    {
        int width = uiController.imageWidth;
        int height = uiController.imageHeight;
        data = texture.GetPixels(0, 0, width, height);
        //UnityEngine.Debug.Log("Testcolor0: " + data[0]);
    }
    void SetDataInTexture(Texture2D texture, Color[] data)
    {
        texture.SetPixels(0, 0, uiController.imageWidth, uiController.imageHeight, data);
        texture.Apply();
    }

    IEnumerator GenerateChemicalMap()
    {
        calculationTime = Time.realtimeSinceStartup;
        for (int h = 0; h < uiController.newPixels.Count; h++)
        {
            if (lastTime == 0)
            {
                lastTime = Time.realtimeSinceStartup;
            }
            Vector2 currentPosition = uiController.newPixels[h];
            //UnityEngine.Debug.Log(uiController.newPixels[h]);
            //Debug.Log(currentPosition.x + " " + currentPosition.y + " chemicalmapposition/number " + ConvertPositionTo1D(currentPosition));
            //UnityEngine.Debug.Log(pixelDataObj[ConvertPositionTo1D(currentPosition)] + "   " + uiController.drawColorFood);
            if (pixelDataObj[ConvertPositionTo1D(currentPosition)] == uiController.drawColorFood)
            {
                
                for (int yy = (0-attractantEffectRange); yy < (attractantEffectRange); yy++)
                {
                    for (int xx = (0 - attractantEffectRange); xx < (attractantEffectRange); xx++)
                        //Parallel.For((0 - attractantEffectRange), attractantEffectRange, xx =>
                    {
                        Vector2 effectPosition = new Vector2(xx + currentPosition.x, yy + currentPosition.y);
                        if ((effectPosition.x > -1) && (effectPosition.x < preDefWidth) && (effectPosition.y > -1) && (effectPosition.y < preDefHeight))
                        {
                            float distance = Vector2.Distance(currentPosition, effectPosition);
                            Color currentColor = pixelDataChem[ConvertPositionTo1D(effectPosition)];
                            //UnityEngine.Debug.Log("currentcolor " + currentColor + " returncolor" + ReturnColorBasedOnDistance(Color.blue, currentColor, attractantEffectRange, distance));
                            pixelDataChem[ConvertPositionTo1D(effectPosition)] = ReturnColorBasedOnDistance(Color.blue, currentColor, attractantEffectRange, distance);
                            Enums.Pixel pixel = new Enums.Pixel(new Vector2(xx + (int)currentPosition.x, yy + (int)currentPosition.y), pixelDataChem[ConvertPositionTo1D(effectPosition)]);
                            finishedPixels.Add(pixel);
                        }
                    }

                    if ((Time.realtimeSinceStartup - lastTime) > 0.3)
                    {
                        //UnityEngine.Debug.Log(Time.realtimeSinceStartup - lastTime);
                        lastTime = Time.realtimeSinceStartup;
                        yield return null;
                    }
                }
            }
            else if (pixelDataObj[ConvertPositionTo1D(currentPosition)] == uiController.drawColorRepellent)
            {

            }
            //if ((Time.realtimeSinceStartup - lastTime) > 0.3)
            //{
            //    //UnityEngine.Debug.Log(Time.realtimeSinceStartup - lastTime);
            //    lastTime = Time.realtimeSinceStartup;
            //    yield return null;
            //}
            //gridData.chemicalTexture.SetPixels(pixelDataChem);
            //gridData.chemicalTexture.Apply();
            for (int i = 0; i < finishedPixels.Count; i++)
            {
                gridData.chemicalTexture.SetPixel((int)finishedPixels[i].position.x, (int)finishedPixels[i].position.y, finishedPixels[i].color);
            }
            finishedPixels.Clear();
            gridData.chemicalTexture.Apply();
            //for (int i = 0; i < uiController.newPixels.Count; i++)
            //{
            //    UnityEngine.Debug.Log("New Color for Pixel: " + uiController.newPixels[i].x + " , " + uiController.newPixels[i].y + " is " + pixelDataChem[ConvertPositionTo1D(uiController.newPixels[i])]);
            //}
        }
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
        
        calculating = false;
        
        UnityEngine.Debug.Log(Time.realtimeSinceStartup - calculationTime + " for calculation");
        uiController.newPixels.Clear();
    }
    //bool OutOfBounds(Vector2 searchPos, Vector2 startPos)
    //{
    //    bool returnVal = false;


    //    return returnVal;
    //}
}
