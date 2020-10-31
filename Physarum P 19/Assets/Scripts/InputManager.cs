using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
        
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
        if (Input.GetMouseButton(0))
        {
            uiController.DrawAtCoordinate(uiController.ReturnGrid(GetMousePos()));
        }
    }
    Vector2 GetMousePos()
    {
        Vector2 vector;
        vector = new Vector2((int)Input.mousePosition.x, (int)Input.mousePosition.y);
        return vector;
    }
}
