using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Params
    [SerializeField]
    SlimeManager slimeManager;
    [SerializeField]
    TextMeshProUGUI playPauseButton;
    [SerializeField]
    TextMeshProUGUI fastButton;
    [SerializeField]
    TextMeshProUGUI timeDisplay;
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
    Enums.DrawMode drawMode = Enums.DrawMode.Deactivated;
    [SerializeField]
    int drawSize = 3;

    [SerializeField]
    public RawImage rawImage;


    int[,] gridArea;
    //int pixelSize = 4;
    int deadZoneLeft = 160;
    int deadZoneDown = 135;
    public int imageWidth;
    public int imageHeight;


    RectTransform rect;
    public Texture2D texture2D;
    //List<float> red;
    //List<float> green;
    //List<float> blue;

    // Start is called before the first frame update
    void Start()
    {
        //Deactivate Panels
        sidePanel.SetActive(false);
        editorPanel.SetActive(false);
        SetupImage(rawImage);
        drawMode = Enums.DrawMode.Wall;
        modulePrefab = Resources.Load("modulePrefab") as GameObject;
        SetUIData(true);
        SetVariablesInSlime(true);
        slimeManager = this.GetComponent<SlimeManager>();
        //gridArea = new int[(Screen.width - (borderLeft * pixelSize - borderRight * pixelSize)) / pixelSize, (Screen.height - (borderTop * pixelSize - borderBottom * pixelSize)) / pixelSize];
       
    }
    
    void SetupImage(RawImage image)
    {
        //red = new List<float>();
        //red = new List<float>();
        //red = new List<float>();
        //https://www.youtube.com/watch?v=HjwVDhMLVN0
        rect = image.GetComponent<RectTransform>();
        
        
        imageWidth = (int)rect.rect.width;;
        imageHeight = (int)rect.rect.height;
        
        texture2D = image.texture as Texture2D;
        texture2D = new Texture2D(imageWidth, imageHeight);
        texture2D.filterMode = FilterMode.Point;
        //texture2D.SetPixels32(texture2D.GetPixels32());
        texture2D.Apply();
        //image.texture = texture2D;
        SetWhiteAllPixel(texture2D);
    }

    internal void AddModuleToUI(Enums.Modules enumModuleName)
    {
        modulePlacement += new Vector3(0, -50, 0);
        GameObject newModule = Instantiate(modulePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        newModule.transform.SetParent(sidePanelContent.transform, false);
        newModule.GetComponent<RectTransform>().anchorMin = new Vector2(0,1);
        newModule.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, modulePlacement.y);
        newModule.name = enumModuleName.ToString();
        newModule.GetComponentInChildren<TextMeshProUGUI>().text = enumModuleName.ToString();
        newModule.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate
        {
            ToggleModule(newModule.GetComponentInChildren<Toggle>());
        });
    }
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
    //void SetRectTransformSettings(float, float, float, float)
    //{

    //}
    public void ToggleModule(Toggle toggle)
    {
        Debug.Log("ToggleModule");
        string name = toggle.gameObject.transform.parent.name;
        if (toggle.isOn)
        {
            AddVariables(name);
        }
        else
        {
            DisableVariables(name);
        }
    }
    public Vector2 ReturnGrid(Vector2 coordinate)
    {
        //Ray ray;
        //RaycastHit hit;
        //ray = Camera.main.ScreenPointToRay(coordinate);
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
        //if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        //{
        //    Debug.Log(hit.transform.name);
        //}
        Vector2 returnValue;
        returnValue = new Vector2((int)coordinate.x - deadZoneLeft, (int)coordinate.y - deadZoneDown);
        //Debug.Log("MouseLeft:x " + returnValue.x + " y " + returnValue.y);
        return returnValue;
    }
    public void DrawAtCoordinate(Vector2 position)
    {
        if ((position.x < 1) || (position.y < 1) || (position.x > imageWidth) || (position.y > imageHeight))
        {
            Debug.Log("Outside");
        }
        else
        {
            switch (drawMode)
            {
                case Enums.DrawMode.Deactivated:
                    
                    break;
                case Enums.DrawMode.Wall:
                    ImageAddPixel(position, new Color(0, 0, 0, 1), texture2D);
                    break;
                case Enums.DrawMode.Slime:
                    ImageAddPixel(position, new Color(1, 1, 0, 1), texture2D);
                    break;
                case Enums.DrawMode.Food:
                    ImageAddPixel(position, new Color(0, 0, 1, 1), texture2D);
                    break;
                default:
                    break;
            }
        }
    }
    public void SetWhiteAllPixel(Texture2D texture)
    {
        Color[] color = new Color[texture.width * texture.height];
        for (int i = 0; i < texture.height; i++)
        {
            for (int j = 0; j < texture.width; j++)
            {
                texture.SetPixel(j, i, Color.white);
            }
        }
        texture.Apply();
        //rawImage.GetComponent<Renderer>().material.mainTexture = texture;
        rawImage.texture = texture;
    }
    private void ImageAddPixel(Vector2 coordinate, Color color, Texture2D target)
    {
        Debug.Log(coordinate.x + ", " + coordinate.y + ", " + color);
        target.SetPixel((int)coordinate.x, (int)coordinate.y, color);
        int halfSize = drawSize / 2;
        for (int i = (0 - halfSize); i < (drawSize + halfSize); i++)
        {
            for (int j = (0 -halfSize); j < (drawSize +halfSize); j++)
            {
                target.SetPixel((int)coordinate.x + i, (int)coordinate.y + j, color);
            }
        }
        target.Apply();
        //Debug.Log("Color: "+target.GetPixel((int)coordinate.x, (int)coordinate.y));
        rawImage.texture = target;
    }
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
        }
    }
    public void SetGameSpeed(int SpeedMode)
    {
        if (SpeedMode>-1)
        {
            slimeManager.gameSpeed = SpeedMode;
            
        }
        else
        {
            slimeManager.gameSpeed = 0;
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
        if (slimeManager.gameSpeed == (int)Enums.GameSpeed.FastMode || slimeManager.gameSpeed == (int)Enums.GameSpeed.Play)
        {
            slimeManager.gameSpeed = (int)Enums.GameSpeed.Pause;
            playPauseButton.text = "Play";
        }
        else
        {
            slimeManager.gameSpeed = (int)Enums.GameSpeed.Play;
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
    public void SetVariablesInSlime(bool reset)
    {

    }
    public void SetTimeUI(float time)
    {
        timeDisplay.text = "Time: " + time;
    }
}
