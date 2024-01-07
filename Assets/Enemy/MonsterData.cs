using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : MonoBehaviour
{
    public Material material;

    public void ActivateIdle()
    {
        // Create a new RGBA color using the Color constructor and store it in a variable
        Color customColor = new Color(1f, 0f, 0f, 1.0f);

        // Call SetColor using the shader property name "_Color" and setting the color to the custom color you created
        material.color = customColor;
    }

    public void ActivateChase()
    {
        // Create a new RGBA color using the Color constructor and store it in a variable
        Color customColor = new Color(1f, 1f, 1f, 1.0f);

        // Call SetColor using the shader property name "_Color" and setting the color to the custom color you created
        material.color = customColor;
    }
}
