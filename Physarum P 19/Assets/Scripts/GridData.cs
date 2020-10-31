using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridData : MonoBehaviour
{
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
        if ((viewTexture != null) && (chemicalTexture != null))
        {
            if (ShowChemicals)
            {
                currentlyShowing = chemicalTexture;
            }
            else
            {
                currentlyShowing = viewTexture;
            }
            rawImage.texture = currentlyShowing;
        }
        else
        {
            Debug.LogError("no viewTexture or chemicalTexture");
        }

    }
    public void ToggleImage()
    {
        ShowChemicals = !ShowChemicals;
    }
}
