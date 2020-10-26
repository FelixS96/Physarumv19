using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    #region Variables
    public int gameSpeed;
    public float timePassedUI;

    int preDefWidth = 1600, preDefHeight = 810;
    
    [SerializeField]
    List<Enums.Modules> activeModules;
    
    [SerializeField]
    Color[] pixelDataObj;
    [SerializeField]
    Color[] pixelDataChem;
    
    [SerializeField]
    UIController uiController;
    [SerializeField]
    GridData gridData;
    #endregion

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
        timePassedUI += speed;
        uiController.SetTimeUI(timePassedUI);
        if (uiController.drawChanges)
        {
            GetInformation();
            pixelDataChem = GenerateChemicalMap();
            Debug.Log("GetInformation + GenerateChemicalMap");
            uiController.drawChanges = false;
        }
        GetInformation();
        //Think();
        //Act();
    }

    private void GetInformation()
    {
        GetDataFromTexture(uiController.texture2DObject, ref pixelDataObj);
        //for testing
        Debug.Log(ConvertPositionTo1D(new Vector2(0, 0)) + " " + pixelDataObj[ConvertPositionTo1D(new Vector2(0, 0))]);
        SetDataInTexture(gridData.viewTexture, pixelDataObj);
    }
    Color[] GenerateChemicalMap()
    {
        Color[] color = uiController.globalBlackColorAll;

        return color;
    }
    int ConvertPositionTo1D(Vector2 position)
    {
        int converted;
        converted = (int)(position.x * preDefWidth + position.y);
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
        Debug.Log("Testcolor0: " + data[0]);
    }
    void SetDataInTexture(Texture2D texture, Color[] data)
    {
        texture.SetPixels(0, 0, uiController.imageWidth, uiController.imageHeight, data);
        texture.Apply();
    }
}
