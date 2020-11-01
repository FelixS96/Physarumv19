using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using static Enums;

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
    List<Modules> activeModules;
    [SerializeField]
    List<Pixel> finishedPixels = new List<Pixel>();

    [SerializeField]
    List<Slime> slimePixel;
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

    }
    // Start is called before the first frame update
    void Start()
    {
        timePassedUI = 0;
        foreach (Modules module in (Modules[])Enum.GetValues(typeof(Modules)))
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
        if (uiController.drawChanges && !calculating)
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
        }
        else
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
    Influence GetHighestImportance(Slime slime)
    {
        Influence highestInfluence = new Influence(InfluenceNames.LowFood, 0, new Vector2(0, 0));
        foreach (Influence influence in slime.influences)
        {
            if (influence.strenght>highestInfluence.strenght)
            {
                highestInfluence = influence;
            }
        }
        return highestInfluence;
    }

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
            else if (maxColor.g == 1)
            {
                if (currentColor.g <= tempColor.g)
                {
                    returnColor.g = tempColor.g;
                }
            }
            else if (maxColor.b == 1)
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
    //todo: gauss filter, metropolis algorithm
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
            if (pixelDataObj[ConvertPositionTo1D(currentPosition)] == uiController.drawColorFood)
            {
                for (int yy = (0 - attractantEffectRange); yy < (attractantEffectRange); yy++)
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
                            Pixel pixel = new Pixel(new Vector2(xx + (int)currentPosition.x, yy + (int)currentPosition.y), pixelDataChem[ConvertPositionTo1D(effectPosition)]);
                            finishedPixels.Add(pixel);
                        }
                    }
                    if ((Time.realtimeSinceStartup - lastTime) > 0.3)
                    {
                        lastTime = Time.realtimeSinceStartup;
                        yield return null;
                    }
                }
            }
            else if (pixelDataObj[ConvertPositionTo1D(currentPosition)] == uiController.drawColorRepellent)
            {

            }
            for (int i = 0; i < finishedPixels.Count; i++)
            {
                gridData.chemicalTexture.SetPixel((int)finishedPixels[i].position.x, (int)finishedPixels[i].position.y, finishedPixels[i].color);
            }
            finishedPixels.Clear();
            gridData.chemicalTexture.Apply();
        }
        calculating = false;
        Debug.Log(Time.realtimeSinceStartup - calculationTime + " for calculation");
        uiController.newPixels.Clear();
    }
}
