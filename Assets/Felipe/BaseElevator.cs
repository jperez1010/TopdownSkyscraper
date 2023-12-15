using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseElevator : MonoBehaviour
{
    public void GoToBase(string sceneName)
    {
        LevelManager.levelManagerInstance.LoadScene(sceneName);
    }
}
