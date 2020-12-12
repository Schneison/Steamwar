using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontFix : MonoBehaviour
{
    public Font[] fonts;

    void Start()
    {
        foreach(Font font in fonts)
        {
            font.material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}
