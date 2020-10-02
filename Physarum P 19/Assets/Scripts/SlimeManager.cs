using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    public int gameSpeed;
    public float timePassedUI;

    private float lastTime;
    private float deltaTime;

    [SerializeField]
    UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
        timePassedUI = 0;
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
