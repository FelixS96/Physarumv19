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
    List<SlimeMold> slimeMoldPixel;
    //[SerializeField]
    //List<SlimeMold> leftSlimeMoldPixel;
    [SerializeField]
    Color[] pixelDataObj;
    [SerializeField]
    public Color[] pixelDataChem;
    List<Pos> slime;
    //todo add slimelist

    [SerializeField]
    UIController uiController;
    [SerializeField]
    GridData gridData;

    
    [SerializeField]
    float food;

    #endregion
    public bool chemicalDetecting; //other detection not in yet
    public bool slimeTrail; //repelling slime left behind

    #region UserChangeable
    //variables that can be changed by the user
    [SerializeField]
    public int amountPixelsMoved = 100;
    float startFood = 100;
    public float slimeRepellentStrenght = 0.01f;
    public int attractantEffectRange;
    public int repellentEffectRange;
    #endregion
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        slime = new List<Pos>();
        slimeMoldPixel = new List<SlimeMold>();
        timePassedUI = 0;
        //foreach (Modules module in (Modules[])Enum.GetValues(typeof(Modules)))
        //{
        //    PlayerPrefs.SetInt(module.ToString(), 1);
        //    uiController.AddModuleToUI(module);
        //    activeModules.Add(module);
        //}
        gridData = FindObjectOfType<GridData>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameSpeed > 0)
        {
            SimulateWithSpeed(gameSpeed);
        }
    }
    //Do tasks for the slimemold
    void SimulateWithSpeed(int speed)
    {
        //if changes were made by drawing
        if (uiController.drawChanges && !calculating)
        {
            uiController.drawChanges = false;
            calculating = true;
            GetInformation();
            AddSlimeMoldToList();
            StartCoroutine(GenerateChemicalMap());
        }
        //while not calculating = while normal runtime
        if (!calculating)
        {
            timePassedUI += speed;
            uiController.SetTimeUI(timePassedUI);
            SpreadChemicals(speed);
            CheckForFood();
            MoveSlimeMold(speed, amountPixelsMoved);
            DrawNewSlimeMold();
            if (slimeTrail)
            {
                LeaveSlimeBehind(ref slime);
            }
        }
        //pause while calculation
        else
        {
            uiController.SetGameSpeed(0);
        }
        
    }
    public void Calculate()
    {
        if (uiController.drawChanges && !calculating)
        {
            uiController.drawChanges = false;
            calculating = true;
            GetInformation();
            AddSlimeMoldToList();
            StartCoroutine(GenerateChemicalMap());
        }
    }

    private void CheckForFood()
    {
        for (int i = 0; i < slimeMoldPixel.Count; i++)
        {
            if (pixelDataObj[ConvertPositionTo1D(slimeMoldPixel[i].position)] == uiController.drawColorFood)
            {
                food += 1.0f;
            }
            else if(pixelDataChem[ConvertPositionTo1D(slimeMoldPixel[i].position)].b>0)
            {
                food += (0.1f * pixelDataChem[ConvertPositionTo1D(slimeMoldPixel[i].position)].b);
            }
        }
    }

    private void DrawNewSlimeMold()
    {
        if (slimeMoldPixel.Count > 0)
        {
            for (int i = 0; i < slimeMoldPixel.Count; i++)
            {
                pixelDataObj[ConvertPositionTo1D(slimeMoldPixel[i].position)] = uiController.drawColorSlimeMold;
            }
            gridData.viewTexture.SetPixels(0, 0, uiController.imageWidth, uiController.imageHeight, pixelDataObj);
            gridData.viewTexture.Apply();
        }
    }

    void AddSlimeMoldToList()
    {
        for (int i = 0; i < uiController.newPixels.Count; i++)
        {
            if (uiController.newPixels[i].pixelType == DrawMode.SlimeMold)
            {
                SlimeMold newSlimeMold = new SlimeMold(uiController.newPixels[i].position);
                if(!slimeMoldPixel.Contains(newSlimeMold))
                {
                    slimeMoldPixel.Add(newSlimeMold);
                    food += startFood;
                }
            }
        }
        //for (int i = 0; i < slimeMoldPixel.Count; i++)
        //{
        //    Debug.Log("New pixel at " + slimeMoldPixel[i].position.x + " , " + slimeMoldPixel[i].position.y);
        //}
    }
    private void SpreadChemicals(int speed)
    {
        //throw new NotImplementedException();
    }
    //todo generate new slimemold pixel
    //Move SlimeMold with Speed and move Count Pixel per use
    private void MoveSlimeMold(int speed, int countPixel)
    {
        List<SlimeMold> localSlimeMoldPixels = new List<SlimeMold>();
        if (countPixel <= slimeMoldPixel.Count)
        {
            //amount to check<=Count;
            for (int i = 0; i < countPixel; i++)
            {
                int random = UnityEngine.Random.Range(0, slimeMoldPixel.Count - 1);
                SlimeMold slimeMold = slimeMoldPixel[random];
                if (localSlimeMoldPixels.Contains(slimeMold))
                {
                    //i--;
                }
                else
                {
                    localSlimeMoldPixels.Add(slimeMold);
                }
            }
        }
        else
        {
            //
            for (int i = 0; i < slimeMoldPixel.Count; i++)
            {
                int random = UnityEngine.Random.Range(0, slimeMoldPixel.Count - 1);
                SlimeMold slimeMold = slimeMoldPixel[random];
                //localSlimeMoldPixels.Add(slimeMold);
                if (localSlimeMoldPixels.Contains(slimeMold))
                {
                    //i--;
                }
                else
                {
                    if (HasAnEpmtySpaceAround(slimeMold.position)){
                        localSlimeMoldPixels.Add(slimeMold);
                    }
                }
            }
        }
        Debug.Log("Chosen Pixels "+localSlimeMoldPixels.Count);
        //new list filled with countPixel
        for (int i = 0; i < localSlimeMoldPixels.Count; i++)
        {
            int count = slimeMoldPixel.FindIndex(item => item.position == localSlimeMoldPixels[i].position);
            if (count == -1)
            {
                //todo 
                Debug.Log("Error");
            }
            else
            {
                pixelDataObj[ConvertPositionTo1D(localSlimeMoldPixels[i].position)] = Color.white;//nur wenn bewegt
                slime.Add(new Pos(slimeMoldPixel[count].position));
                SlimeMold currentSlime = new SlimeMold(MoveOrNot(localSlimeMoldPixels[i].position));
                slimeMoldPixel[count] = currentSlime;

            }
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
    //    slimeMoldPixel = new List<SlimeMold>();
    //    for (int i = 0; i < pixelDataObj.Length; i++)
    //    {
    //        if (pixelDataObj[i] == uiController.drawColorSlimeMold)
    //        {
    //            slimeMoldPixel.Add()
    //        }
    //    }
    //}
    //Influence GetHighestImportance(SlimeMold slime)
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
    //convert 2d position to 1d array
    int ConvertPositionTo1D(Vector2 position)
    {
        int converted;
        converted = (int)(position.y * preDefWidth + position.x);
        return converted;
    }
    //create a vector2 from -rangx and -rangey to rangex and rangey
    Vector2 RandomDirectionVector(int rangeX, int rangeY)
    {
        return new Vector2((int)UnityEngine.Random.Range(-rangeX, rangeX+1), (int)UnityEngine.Random.Range(-rangeY, rangeY+1));
    }
    //Calculate Energy of a Position and return an int
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
        if (chemicalDetecting)
        {

            return calculatedEnergy;
        }
        else
        {
            return 0;
        }
    }
    //Moves the Slimemoldpixel to a new Position or not and returns the Position
    Vector2 MoveOrNot(Vector2 pos)
    {
        //Create a new Position with a random Vector and the old Pos
        Vector2 newPos = pos + RandomDirectionVector(1, 1);
        //calculate Energy of old and new Position and difference
        int newEnergy = CalculateEnergy(newPos);
        int oldEnergy = CalculateEnergy(pos);
        int deltaE = newEnergy - oldEnergy;
        //less energy = goes to new pos
        if (HasSlimeMoldAround(newPos))
        {
            if (oldEnergy >= newEnergy)
            {
                food -= 0.01f;
                return newPos;
            }
            //more energy = Chance to move
            else if (newEnergy > oldEnergy)
            {
                if (SuccessOfChance(deltaE))
                {
                    //add deltaE to food calculation
                    food -= 0.02f;
                    return newPos;
                }
            }
        }
        return pos;
    }
    private bool HasAnEpmtySpaceAround(Vector2 pos)
    {
        bool returnVal = false;
        for (int x = -1; x < 1; x++)
        {
            for (int y = -1; y < 1; y++)
            {
                if (pixelDataObj[ConvertPositionTo1D(pos + new Vector2(x, y))] == Color.white)
                {
                    Debug.Log("Is true");
                    return true;
                }
                //if()
                //newPos + new Vector2(x, y);
            }
        }
        return returnVal;
    }
    //check surrounding pixels for slimemold
    private bool HasSlimeMoldAround(Vector2 newPos)
    {
        bool returnVal = false;
        for (int x = -1; x < 1; x++)
        {
            for (int y =- 1; y < 1; y++)
            {
                if(pixelDataObj[ConvertPositionTo1D(newPos + new Vector2(x, y))] == uiController.drawColorSlimeMold)
                {
                    //Debug.Log("Is true");
                    return true;
                }
                //if()
                //newPos + new Vector2(x, y);
            }
        }
        return returnVal;
    }

    bool SuccessOfChance(int deltaEnergy)
    {
        float chance = Math.Min(1, (float)deltaEnergy/1000/*Math.Exp(-(deltaEnergy/0.1))*/);
        //Debug.Log("Energydifference: " + deltaEnergy + " chance: " + chance);
        bool successBool = (UnityEngine.Random.value < chance);
        if (deltaEnergy < 10000)
        {
            return successBool;
        }
        else
        {
            return false;
        }
    }
    void LeaveSlimeBehind(ref List<Pos> leftAreas)
    {
        foreach (Pos position in leftAreas)
        {
            Color gotColor = pixelDataChem[ConvertPositionTo1D(position.position)];
            if (gotColor.r <= 0.9)
            {
                gotColor.r += slimeRepellentStrenght;
            }
            pixelDataChem[ConvertPositionTo1D(position.position)] = gotColor;
        }
        gridData.chemicalTexture.SetPixels(pixelDataChem);
        gridData.chemicalTexture.Apply();
        leftAreas.Clear();
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
    //Gets all Pixeldata and writes it into array
    void GetDataFromTexture(Texture2D texture, ref Color[] data)
    {
        int width = uiController.imageWidth;
        int height = uiController.imageHeight;
        data = texture.GetPixels(0, 0, width, height);
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
                            if ((pixelDataObj[ConvertPositionTo1D(effectPosition)]!=Color.black)&&(effectPosition.x > -1) && (effectPosition.x < preDefWidth) && (effectPosition.y > -1) && (effectPosition.y < preDefHeight))
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
        uiController.calculatePanel.SetActive(false);
    }
}
