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

    int[,] gridArea;
    //int pixelSize = 4;
    int deadZoneLeft = 160;
    int deadZoneDown = 90;
    int imageWidth;
    int imageHeight;

    // Start is called before the first frame update
    void Start()
    {
        //Deactivate Panels
        sidePanel.SetActive(false);
        editorPanel.SetActive(false);

        modulePrefab = Resources.Load("modulePrefab") as GameObject;
        SetUIData(true);
        SetVariablesInSlime(true);
        slimeManager = this.GetComponent<SlimeManager>();
        //gridArea = new int[(Screen.width - (borderLeft * pixelSize - borderRight * pixelSize)) / pixelSize, (Screen.height - (borderTop * pixelSize - borderBottom * pixelSize)) / pixelSize];
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
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(coordinate);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            Debug.Log(hit.transform.name);
        }
        Vector2 returnValue;
        returnValue = new Vector2((int)coordinate.x - deadZoneLeft, (int)coordinate.y - deadZoneDown);
        Debug.Log("MouseLeft:x " + returnValue.x + " y " + returnValue.y);
        return returnValue;
    }
    void DrawAtCoordinate(Vector2 position)
    {
        if (position.x < 1 || position.y < 1 || position.x > imageWidth || position.y > imageHeight)
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
                    ImageAddPixel(position, new Color(1, 1, 1, 1));
                    break;
                case Enums.DrawMode.Slime:
                    ImageAddPixel(position, new Color(1, 1, 0, 1));
                    break;
                case Enums.DrawMode.Food:
                    ImageAddPixel(position, new Color(0, 0, 1, 1));
                    break;
                default:
                    break;
            }
        }
    }
    private void ImageAddPixel(Vector2 coordinate, Color color)
    {

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
            drawMode = Enums.DrawMode.Deactivated;
        }
    }
    //toggle Level Editor Panel visibility
    public void ToggleEditor()
    {
        editorPanel.SetActive(!editorPanel.activeSelf);
        if (!editorPanel.activeSelf)
        {
            drawMode = Enums.DrawMode.Deactivated;
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
        }
        else
        {
            playPauseButton.text = "Play";
        }
    }
    public void ToggleGameSpeed()
    {
        if (slimeManager.gameSpeed == (int)Enums.GameSpeed.FastMode || slimeManager.gameSpeed == (int)Enums.GameSpeed.Play)
        {
            slimeManager.gameSpeed = (int)Enums.GameSpeed.Pause;
            playPauseButton.text = "Play";
        }
        else
        {
            slimeManager.gameSpeed = (int)Enums.GameSpeed.Play;
            playPauseButton.text = "Pause";
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
