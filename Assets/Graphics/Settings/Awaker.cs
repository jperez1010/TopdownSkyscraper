using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awaker : MonoBehaviour
{
    public TitleScreenFader titleScreenFader;

    private void Awake()
    {

            titleScreenFader.StartFadeOut();
        
     

    }

}
