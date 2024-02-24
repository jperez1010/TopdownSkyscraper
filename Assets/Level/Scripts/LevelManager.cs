using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManagerInstance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private GameObject playerPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        if (levelManagerInstance == null)
        {
            levelManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(1000);

            _progressBar.value = scene.progress;

        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
        StartCoroutine(WaitForSceneLoad(scene));
    }

    public async void LoadScene(int sceneIndex)
    {
        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(1000);

           _progressBar.value = scene.progress;

        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
        StartCoroutine(WaitForSceneLoad(scene));
    }

    public IEnumerator WaitForSceneLoad(AsyncOperation sceneLoad)
    {
        while (!sceneLoad.isDone)
        {
            yield return null;
        }
        Instantiate(playerPrefab);
    }
}
