using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown refreshRateDropdown; // Reference to the refresh rate dropdown
    [SerializeField] private TMP_Text fullScreenButtonText;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private List<float> refreshRates;
    private bool isFullScreen = true;
    private int currentResolutionIndex = 0;
    private int currentRefreshRateIndex = 0;
    private float currentRefreshRate = 60.0f;

    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        refreshRates = new List<float>();

        resolutionDropdown.ClearOptions();
        refreshRateDropdown.ClearOptions();

        foreach (Resolution res in resolutions)
        {
            if (!refreshRates.Contains((float)res.refreshRateRatio.value))
            {
                refreshRates.Add((float)res.refreshRateRatio.value);
            }
        }

        List<string> options = new List<string>();
        List<string> refreshRateOptions = new List<string>();

        currentRefreshRateIndex = refreshRates.IndexOf(currentRefreshRate);

        foreach (float rate in refreshRates)
        {
            refreshRateOptions.Add(rate + "Hz");
        }

        refreshRateDropdown.AddOptions(refreshRateOptions);
        refreshRateDropdown.value = currentRefreshRateIndex;
        refreshRateDropdown.RefreshShownValue();

        foreach (Resolution res in resolutions)
        {
            if (res.refreshRateRatio.value == refreshRates[currentRefreshRateIndex])
            {
                filteredResolutions.Add(res);
            }
        }

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Find and set 1920x1080 as default resolution
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (filteredResolutions[i].width == 1920 && filteredResolutions[i].height == 1080)
            {
                currentResolutionIndex = i;
                Screen.SetResolution(filteredResolutions[i].width, filteredResolutions[i].height, isFullScreen);
                resolutionDropdown.value = currentResolutionIndex;
                resolutionDropdown.RefreshShownValue();
                break;
            }
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
    }

    public void SetRefreshRate(int rateIndex)
    {
        currentRefreshRateIndex = rateIndex;
        filteredResolutions.Clear();

        foreach (Resolution res in resolutions)
        {
            if (res.refreshRateRatio.value == refreshRates[currentRefreshRateIndex])
            {
                filteredResolutions.Add(res);
            }
        }

        List<string> options = new List<string>();

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;

        if (isFullScreen)
        {
            fullScreenButtonText.text = "Full Screen: ON";
        }
        else
        {
            fullScreenButtonText.text = "Full Screen: OFF";
        }
    }

        public void ExitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }

}
