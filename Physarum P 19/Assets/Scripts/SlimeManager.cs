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
    //todo add slimelist

    [SerializeField]
    UIController uiController;
    [SerializeField]
    GridData gridData;

    public int attractantEffectRange;
    public int repellentEffectRange;
    [SerializeField]
    float food;

    #endregion

    #region UserChangeable
    //variables that can be changed by the user
    [SerializeField]
    int amountPixelsMoved = 100;
    float startFood = 100;

    #endregion
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        slimePixel = new List<Slime>();
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
            AddSlimeToList();
            StartCoroutine(GenerateChemicalMap());
        }
        if (!calculating)
        {
            timePassedUI += speed;
            uiController.SetTimeUI(timePassedUI);
            SpreadChemicals(speed);
            CheckForFood();
            MoveSlime(speed, amountPixelsMoved);
            DrawNewSlime();
        }
        else
        {
            uiController.SetGameSpeed(0);
        }
        
    }

    private void CheckForFood()
    {
        for (int i = 0; i < slimePixel.Count; i++)
        {
            if (pixelDataObj[ConvertPositionTo1D(slimePixel[i].position)] == uiController.drawColorFood)
            {
                food += 1.0f;
            }
            else if(pixelDataChem[ConvertPositionTo1D(slimePixel[i].position)].b>0)
            {
                food += (0.1f * pixelDataChem[ConvertPositionTo1D(slimePixel[i].position)].b);
            }
        }
    }

    private void DrawNewSlime()
    {
        if (slimePixel.Count > 0)
        {
            for (int i = 0; i < slimePixel.Count; i++)
            {
                pixelDataObj[ConvertPositionTo1D(slimePixel[i].position)] = uiController.drawColorSlimeMold;
            }
            gridData.viewTexture.SetPixels(0, 0, uiController.imageWidth, uiController.imageHeight, pixelDataObj);
            gridData.viewTexture.Apply();
        }
    }

    void AddSlimeToList()
    {
        for (int i = 0; i < uiController.newPixels.Count; i++)
        {
            if (uiController.newPixels[i].pixelType == DrawMode.SlimeMold)
            {
                Slime newSlime = new Slime(uiController.newPixels[i].position);
                if(!slimePixel.Contains(newSlime))
                {
                    slimePixel.Add(newSlime);
                    food += startFood;
                }
            }
        }
        //for (int i = 0; i < slimePixel.Count; i++)
        //{
        //    Debug.Log("New pixel at " + slimePixel[i].position.x + " , " + slimePixel[i].position.y);
        //}
    }
    private void SpreadChemicals(int speed)
    {
        //throw new NotImplementedException();
    }
    //todo generate new slimemold pixel
    private void MoveSlime(int speed, int countPixel)
    {
        List<Slime> localSlimePixels = new List<Slime>();
        if (countPixel <= slimePixel.Count)
        {
            //Debug.Log("amount to check<=Count");
            for (int i = 0; i < countPixel; i++)
            {
                int random = UnityEngine.Random.Range(0, slimePixel.Count - 1);
                Slime slime = slimePixel[random];
                if (localSlimePixels.Contains(slime))
                {
                    //i--;
                }
                else
                {
                    localSlimePixels.Add(slime);
                }
            }
        }
        else
        {
            //Debug.Log("else");
            for (int i = 0; i < slimePixel.Count; i++)
            {
                int random = UnityEngine.Random.Range(0, slimePixel.Count - 1);
                Slime slime = slimePixel[random];
                localSlimePixels.Add(slime);
                if (localSlimePixels.Contains(slime))
                {
                    //i--;
                }
                else
                {
                    localSlimePixels.Add(slime);
                }
            }
        }

        //new list filled with countPixel
        for (int i = 0; i < localSlimePixels.Count; i++)
        {
            int count = slimePixel.FindIndex(item => item.position == localSlimePixels[i].position);
            Slime currentSlime = new Slime(MoveOrNot(localSlimePixels[i].position));
            if (count == -1)
            {
                //todo Debug.Log("WTF");
            }
            else
            {

                slimePixel[count] = currentSlime;
            }
            //make pixels stay together
        }
    }

    private void GetInformation()
    {
        GetDataFromTexture(uiController.texture2DObject, ref pixelDataObj);
        //CopySlime();
        SetDataInTexture(gridData.viewTexture, pixelDataObj);
    }
    //void CopySlime()
    //{
    //    slimePixel = new List<Slime>();
    //    for (int i = 0; i < pixelDataObj.Length; i++)
    //    {
    //        if (pixelDataObj[i] == uiController.drawColorSlimeMold)
    //        {
    //            slimePixel.Add()
    //        }
    //    }
    //}
    //Influence GetHighestImportance(Slime slime)
    //{
    //    Influence highestInfluence = new Influence(InfluenceNames.LowFood, 0, new Vector2(0, 0));
    //    foreach (Influence influence in slime.influences)
    //    {
    //        if (influence.strenght>highestInfluence.strenght)
    //        {
    //            highestInfluence = influence;
    //        }
    //    }
    //    return highestInfluence;
    //}
    //void Test(int num)
    //{
    //    for (int i = 0; i < num; i++)
    //    {
    //        SuccessOfChance(i);
    //    }
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
    Vector2 RandomDirectionVector(int rangeX, int rangeY)
    {
        return new Vector2((int)UnityEngine.Random.Range(-rangeX, rangeX+1), (int)UnityEngine.Random.Range(-rangeY, rangeY+1));
    }
    int CalculateEnergy(Vector2 pos)
    {
        int calculatedEnergy = 0;
        int oneDimArray = ConvertPositionTo1D(pos);
        Color colorChem = pixelDataChem[oneDimArray];

        if (pixelDataObj[ConvertPositionTo1D(pos)] == Color.black)
        {
            calculatedEnergy = 1000000;
        }
        else 
        {
            if (colorChem.b > 0)
            {
                calculatedEnergy -= (int)(colorChem.b * 1000);
            }
            if (colorChem.r > 0)
            {
                calculatedEnergy += (int)(colorChem.r * 1000);
            }
        }
        return calculatedEnergy;
    }
    Vector2 MoveOrNot(Vector2 pos)
    {
        Vector2 newPos = pos + RandomDirectionVector(1, 1);
        int newEnergy = CalculateEnergy(newPos);
        int oldEnergy = CalculateEnergy(pos);
        //todo slower with lower food
        //less energy
        if (oldEnergy >= newEnergy) 
        {
            food-=0.01f;
            return newPos;
        }
        else if(newEnergy > oldEnergy)
        {
            int deltaE = newEnergy - oldEnergy;
            if (SuccessOfChance(deltaE)) 
            {
                //add deltaE to food calculation
                food-=0.02f;
                return newPos;
            }
        }
        return pos;
    }
    bool SuccessOfChance(int deltaEnergy)
    {
        float chance = Math.Min(1, (float)deltaEnergy/1000/*Math.Exp(-(deltaEnergy/0.1))*/);
        //Debug.Log("Energydifference: " + deltaEnergy + " chance: " + chance);
        bool successBool = (UnityEngine.Random.value < chance);
        return successBool;
    }
    void LeaveSlimeBehind(Vector2 pos)
    {

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
            if (uiController.newPixels[h].pixelType == DrawMode.SlimeMold)
            {

            }
            else
            {
                if (lastTime == 0)
                {
                    lastTime = Time.realtimeSinceStartup;
                }
                Vector2 currentPosition = uiController.newPixels[h].position;
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
        }
        calculating = false;
        Debug.Log(Time.realtimeSinceStartup - calculationTime + " for calculation");
        uiController.newPixels.Clear();
    }
}
