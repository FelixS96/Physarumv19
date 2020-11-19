using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
        //Set UIController
        uiController = FindObjectOfType<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetButtonDown("Space"))
            {
                uiController.ToggleGameSpeed();
                //Debug.Log("SpaceDown");
            }
            if (Input.GetButtonDown("Tab"))
            {
                uiController.ToggleSidePanel();
                //Debug.Log("TabDown");
            }
            if (Input.GetButtonDown("E"))
            {
                uiController.ToggleEditor();
                //Debug.Log("EditorDown");
            }
        }

        //On Leftclick
        if (Input.GetMouseButton(0))
        {
            //Draw at Mouse Coordinate
            uiController.DrawAtCoordinate(uiController.ReturnGrid(Input.mousePosition));
        }
    }
}
