using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public float Dtime = 2.0f;
    public GameObject faderObject;
    public string nextSceneName = "NextScene";
    AudioManager Manager;



    public void OnStartButtonClick()
    {
        AudioManager.Instance.ClearOneShot();
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.StartButton);
        if (faderObject != null)
        {
            faderObject.SetActive(true);
        }

        Invoke("LoadNextScene", Dtime);
    }

    private void LoadNextScene()
    {

        SceneManager.LoadScene(nextSceneName);
    }
}
