using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        SetUIData(true);
        SetVariablesInSlime(true);
        slimeManager = this.GetComponent<SlimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleUIVisibility()
    {

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
