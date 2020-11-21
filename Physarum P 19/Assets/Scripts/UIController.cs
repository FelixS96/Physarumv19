using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIController : MonoBehaviour
{
    //Scripts
    [SerializeField]
    SlimeManager slimeMoldManager;
    [SerializeField]
    GridData gridData;

    //Text
    [SerializeField]
    TextMeshProUGUI playPauseButton;
    [SerializeField]
    TextMeshProUGUI fastButton;
    [SerializeField]
    TextMeshProUGUI timeDisplay;

    //Panels
    [SerializeField]
    GameObject sidePanel;
    [SerializeField]
    GameObject editorPanel;

    [SerializeField]
    Vector3 modulePlacement;
    [SerializeField]
    GameObject modulePrefab;
    [SerializeField]
    GameObject sidePanelContent;
    [SerializeField]
    public GameObject calculatePanel;
    [SerializeField]
    DrawMode drawMode = DrawMode.Deactivated;
    [SerializeField]
    int drawSize = 3;
    public List<PixelData> newPixels;

    [SerializeField]
    List<GameObject> chemoGroup;
    [SerializeField]
    List<GameObject> trailGroup;

    //Imagecomponents
    [SerializeField]
    public RawImage rawImage;
    //Colors
    public Color drawColorWall;
    public Color drawColorSlimeMold;
    public Color drawColorFood;
    public Color drawColorRepellent;
    public Color drawColorSlime;
    
    //Image and Screendata
    int deadZoneLeft = 160;
    int deadZoneDown = 135;
    public int imageWidth;
    public int imageHeight;
    private int screenWidth;
    private int screenHeight;

    //Texturedata
    RectTransform rect;
    [SerializeField]
    public Color[] globalBlackColorAll;
    public Texture2D texture2DObject;
    public Texture2D texture2DChemical;
    public bool drawChanges;

    // Start is called before the first frame update
    void Start()
    {
        //Set Values
        gridData = FindObjectOfType<GridData>();
        newPixels = new List<PixelData>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        modulePrefab = Resources.Load("modulePrefab") as GameObject;
        slimeMoldManager = this.GetComponent<SlimeManager>();

        //Deactivate Panels
        sidePanel.SetActive(false);
        editorPanel.SetActive(false);
        FillArray();

        SetupImage(rawImage);
        slimeMoldManager.pixelDataChem = texture2DChemical.GetPixels();
        drawMode = Enums.DrawMode.Deactivated;
        
        SetUIData(true);
        SetVariablesInSlimeMold(true);
        
    }

    void SetupImage(RawImage image)
    {
        rect = image.GetComponent<RectTransform>();
        imageWidth = (int)rect.rect.width; ;
        imageHeight = (int)rect.rect.height;

        texture2DObject = image.texture as Texture2D;
        texture2DObject = new Texture2D(imageWidth, imageHeight);
        texture2DObject.filterMode = FilterMode.Point;
        texture2DObject.Apply();
        SetAllPixel(ref texture2DObject, Color.white);
        CreateBorder(ref texture2DObject, 2);
        rawImage.GetComponent<GridData>().viewTexture = texture2DObject;

        texture2DChemical = image.texture as Texture2D;
        texture2DChemical = new Texture2D(imageWidth, imageHeight);
        texture2DChemical.filterMode = FilterMode.Point;
        texture2DChemical.Apply();
        SetAllPixel(ref texture2DChemical, Color.black);
        rawImage.GetComponent<GridData>().chemicalTexture = texture2DChemical;


    }

    internal void AddModuleToUI(Enums.Modules enumModuleName)
    {
        modulePlacement += new Vector3(0, -50, 0);
        GameObject newModule = Instantiate(modulePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        newModule.transform.SetParent(sidePanelContent.transform, false);
        newModule.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        newModule.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, modulePlacement.y);
        newModule.name = enumModuleName.ToString();
        newModule.GetComponentInChildren<TextMeshProUGUI>().text = enumModuleName.ToString();
        newModule.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate
        {
            ToggleModule(newModule.GetComponentInChildren<Toggle>());
        });
    }
    void CreateBorder(ref Texture2D texture, int thickness)
    {
        for (int y = 0; y < thickness; y++)
        {
            for (int x = 0; x < imageWidth; x++)
            {
                texture.SetPixel(x, y, drawColorWall);
            }
        }
        for (int y = imageHeight-thickness; y < imageHeight; y++)
        {
            for (int x = 0; x < imageWidth; x++)
            {
                texture.SetPixel(x, y, drawColorWall);
            }
        }
        for (int y = 0; y < imageHeight; y++)
        {
            for (int x = 0; x < thickness; x++)
            {
                texture.SetPixel(x, y, drawColorWall);
            }
        }
        for (int y = 0; y < imageHeight; y++)
        {
            for (int x = imageWidth-thickness; x < imageWidth; x++)
            {
                texture.SetPixel(x, y, drawColorWall);
            }
        }
        texture.Apply();
    }

    //Set Drawmode and pause if its a drawable material
    public void SetDrawMode(int mode)
    {
        drawMode = (Enums.DrawMode)mode;
        if (drawMode != 0)
        {
            SetGameSpeed(0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnEndEditPixelsMoved(TMP_InputField value)
    {
        slimeMoldManager.amountPixelsMoved = int.Parse(value.text);
    }
    public void OnEndEditRepellent(TMP_InputField value)
    {
        Debug.Log(value.text);
        float result = slimeMoldManager.slimeRepellentStrenght;
        if(float.TryParse(value.text,out result))
        {
            Debug.Log("got " + result);
            slimeMoldManager.slimeRepellentStrenght = result;
        }
    }


    public void ToggleModule(Toggle toggle)
    {
        Debug.Log("ToggleModule");
        string name = toggle.gameObject.transform.parent.name;
        
        AddVariables(name);
        if (toggle.transform.parent.name.Contains("Chemo"))
        {
            slimeMoldManager.chemicalDetecting = !slimeMoldManager.chemicalDetecting;
            for (int i = 0; i < chemoGroup.Count; i++)
            {
                chemoGroup[i].SetActive(slimeMoldManager.chemicalDetecting);
            }
        }
        else if(toggle.transform.parent.name.Contains("Trail"))
        {
            slimeMoldManager.slimeTrail = !slimeMoldManager.slimeTrail;
            for (int i = 0; i < trailGroup.Count; i++)
            {
                trailGroup[i].SetActive(slimeMoldManager.slimeTrail);
            }
        }
        
    }
    public Vector2 ReturnGrid(Vector2 coordinate)
    {
        //Debug.Log("MouseLeft:x " + returnValue.x + " y " + returnValue.y);
        return new Vector2((int)coordinate.x - ((screenWidth - imageWidth) / 2), (int)coordinate.y - ((screenHeight - imageHeight) / 2));
    }
    void ActivateCalculationPanel()
    {
        calculatePanel.SetActive(true);
    }
    public void DeactivateCalculationPanel()
    {
        calculatePanel.SetActive(false);
    }
    public void DrawAtCoordinate(Vector2 position)
    {
        if ((position.x < 1) || (position.y < 1) || (position.x > imageWidth) || (position.y > imageHeight))
        {
            //Debug.Log("Outside");
        }
        else
        {
            PixelData newPixel = new PixelData();
            switch (drawMode)
            {
                case DrawMode.Deactivated:

                    break;
                case DrawMode.Wall:
                    ImageAddPixel(position, drawColorWall, texture2DObject);
                    drawChanges = true;
                    ActivateCalculationPanel();
                    break;
                case DrawMode.SlimeMold:
                    ImageAddPixel(position, drawColorSlimeMold, texture2DObject);
                    newPixels.Add(new PixelData(drawMode, position));
                    drawChanges = true;
                    ActivateCalculationPanel();
                    break;
                case DrawMode.Food:
                    ImageAddPixel(position, drawColorFood, texture2DObject);
                    newPixels.Add(new PixelData(drawMode, position));
                    drawChanges = true;
                    ActivateCalculationPanel();
                    break;
                case DrawMode.Repellent:
                    ImageAddPixel(position, drawColorRepellent, texture2DObject);
                    newPixels.Add(new PixelData(drawMode, position));
                    drawChanges = true;
                    ActivateCalculationPanel();
                    break;
                default:
                    break;
            }
        }
    }
    //Fill 1D color array with black
    void FillArray()
    {
        globalBlackColorAll = new Color[imageWidth * imageHeight];
        for (int i = 0; i < (imageHeight * imageWidth); i++)
        {
            globalBlackColorAll[i] = Color.black;
        }
    }
    public void SetAllPixel(ref Texture2D texture, Color targetColor)
    {
        for (int i = 0; i < texture.height; i++)
        {
            for (int j = 0; j < texture.width; j++)
            {
                texture.SetPixel(j, i, targetColor);
            }
        }
        texture.Apply();
    }
    private void ImageAddPixel(Vector2 coordinate, Color color, Texture2D target)
    {
        //target.SetPixel((int)coordinate.x, (int)coordinate.y, color);
        int halfSize = drawSize / 2;
        for (int i = (0 - halfSize); i < (drawSize + halfSize); i++)
        {
            for (int j = (0 - halfSize); j < (drawSize + halfSize); j++)
            {
                Vector2 combinedVector = new Vector2(coordinate.x + i, coordinate.y + j);
                if ((combinedVector.x > -1) && (combinedVector.x < imageWidth) && (combinedVector.y > -1) && (combinedVector.y < imageHeight))
                {
                    if (drawMode == DrawMode.SlimeMold)
                    { 
                        newPixels.Add(new PixelData(drawMode, new Vector2((int)combinedVector.x, (int)combinedVector.y)));
                    }
                    target.SetPixel((int)combinedVector.x, (int)combinedVector.y, color);
                }
            }
        }
        target.Apply();
        gridData.viewTexture = target;
    }
    private void AddVariables(string name)
    {

    }

    private void DisableVariables(string name)
    {
        throw new NotImplementedException();
    }
    //toggle Parameter Side Panel visibility
    public void ToggleSidePanel()
    {
        sidePanel.SetActive(!sidePanel.activeSelf);
        if (sidePanel.activeSelf)
        {
            editorPanel.SetActive(false);
            SetDrawMode(0);
            SetGameSpeed(0);
        }
    }
    //toggle Level Editor Panel visibility
    public void ToggleEditor()
    {
        editorPanel.SetActive(!editorPanel.activeSelf);
        if (!editorPanel.activeSelf)
        {
            SetDrawMode(0);
        }
        else
        {
            sidePanel.SetActive(false);
            SetGameSpeed(0);
        }
    }
    public void SetGameSpeed(int SpeedMode)
    {
        if (SpeedMode > -1)
        {
            slimeMoldManager.gameSpeed = SpeedMode;

        }
        else
        {
            slimeMoldManager.gameSpeed = 0;
        }
        if (SpeedMode > 0)
        {
            playPauseButton.text = "Pause";
            SetDrawMode(0);
        }
        else
        {
            playPauseButton.text = "Play";
        }
    }
    public void ToggleGameSpeed()
    {
        SetDrawMode(0);
        if (slimeMoldManager.gameSpeed == (int)Enums.GameSpeed.FastMode || slimeMoldManager.gameSpeed == (int)Enums.GameSpeed.Play)
        {
            slimeMoldManager.gameSpeed = (int)Enums.GameSpeed.Pause;
            playPauseButton.text = "Play";
        }
        else
        {
            slimeMoldManager.gameSpeed = (int)Enums.GameSpeed.Play;
            playPauseButton.text = "Pause";

            editorPanel.SetActive(false);
            sidePanel.SetActive(false);
        }
    }
    public void ResetToDefaultVariables()
    {

    }
    public void GetUIData(Enums.VariableType variable)
    {

    }
    public void SetUIData(bool reset)
    {

    }
    public void SetVariablesInSlimeMold(bool reset)
    {

    }
    public void SetTimeUI(float time)
    {
        timeDisplay.text = "Time: " + time;
    }
}
