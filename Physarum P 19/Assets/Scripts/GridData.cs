using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridData : MonoBehaviour
{
    //Textures
    public Texture2D viewTexture;
    public Texture2D chemicalTexture;
    Texture2D currentlyShowing;

    [SerializeField]
    RawImage rawImage;
    [SerializeField]
    public bool ShowChemicals;

    //// Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        ShowChemicals = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Check if the 2 Textures are asigned
        if ((viewTexture != null) && (chemicalTexture != null))
        {
            //if the bool is true
            if (ShowChemicals)
            {
                currentlyShowing = chemicalTexture;
            }
            //if the bool is false
            else
            {
                currentlyShowing = viewTexture;
            }
            //Set Texture of Image to current Texture
            rawImage.texture = currentlyShowing;
        }
        else
        {
            Debug.LogError("no viewTexture or chemicalTexture");
        }

    }

    //Toggle Chemical bool 
    public void ToggleImage()
    {
        ShowChemicals = !ShowChemicals;
    }
}
