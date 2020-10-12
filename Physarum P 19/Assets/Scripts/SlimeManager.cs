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
    void Update()
    {
        if (deltaTime >= 0.1)
        {
            if (gameSpeed > 0)
            {
                simulateWithSpeed(gameSpeed);
            }
            deltaTime = 0;
        }
        deltaTime += Time.deltaTime;
    }
    void simulateWithSpeed(int speed)
    {
        timePassedUI += speed;
        uiController.SetTimeUI(timePassedUI);
    }
}
