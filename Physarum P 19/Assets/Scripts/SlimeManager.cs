using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    #region Variables
    public int gameSpeed;
    public float timePassedUI;

    private float lastTime;
    private float deltaTime;
    [SerializeField]
    List<Enums.Modules> activeModules;
    [SerializeField]
    Color[] pixeldata;
    
    [SerializeField]
    UIController uiController;
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
        GetInformation();
        //Think();
        //Act();
    }

    private void GetInformation()
    {
        GetDataFromTexture(uiController.texture2D, ref pixeldata);
        //for testing
        Debug.Log(ConvertPositionTo1D(new Vector2(0, 0)) + " " + pixeldata[ConvertPositionTo1D(new Vector2(0, 0))]);
        SetVirtualPixel(new Vector2(0, 1), Color.red, ref pixeldata);
        SetVirtualPixel(new Vector2(0, 0), Color.red, ref pixeldata);
        SetVirtualPixel(new Vector2(1, 1), Color.red, ref pixeldata);
        SetVirtualPixel(new Vector2(1, 0), Color.red, ref pixeldata);
        SetDataInTexture(uiController.texture2D, pixeldata);
    }
    int ConvertPositionTo1D(Vector2 position)
    {
        int converted;
        converted = (int)(position.x * uiController.imageWidth + position.y);
        return converted;
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
